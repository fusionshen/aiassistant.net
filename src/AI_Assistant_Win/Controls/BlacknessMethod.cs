using AI_Assistant_Win.Business;
using AI_Assistant_Win.Models;
using AI_Assistant_Win.Models.Enums;
using AI_Assistant_Win.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Yolov8.Net;

namespace AI_Assistant_Win.Controls
{
    /// <summary>
    /// init camera, if camera was binded, origin image zone will show the image.
    /// reconnect camera
    /// </summary>
    public partial class BlacknessMethod : UserControl
    {
        private readonly MainWindow form;

        private readonly BlacknessMethodBLL blacknessMethodBLL;

        private readonly ImageProcessBLL imageProcessBLL;

        private readonly CameraBLL cameraBLL;

        private readonly BlacknessPredict blacknessPredict;

        public BlacknessMethod(MainWindow _form)
        {
            form = _form;
            InitializeComponent();
            blacknessMethodBLL = new BlacknessMethodBLL();
            imageProcessBLL = new ImageProcessBLL("Blackness");
            imageProcessBLL.PropertyChanged += ImageProcessBLL_PropertyChanged;
            blacknessPredict = new BlacknessPredict(imageProcessBLL);
            blacknessPredict.PropertyChanged += BlacknessPredict_PropertyChanged;
            cameraBLL = new CameraBLL("Blackness", avatarOriginImage.Handle);
            cameraBLL.PropertyChanged += CameraBLL_PropertyChanged;
            CameraHelper.CAMERA_DEVICES.Add(cameraBLL);
            this.Load += async (s, e) => await InitializeCameraAsync();
            this.HandleDestroyed += async (s, e) => await CloseCameraAsync();
        }

        private void BlacknessPredict_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Predictions")
            {
                if (blacknessPredict.Predictions != null && blacknessPredict.Predictions.Length != 0)
                {
                    OutputTexts(blacknessPredict.Predictions);
                    AntdUI.Notification.success(form, "�ɹ�", "ʶ��ɹ���", AntdUI.TAlignFrom.BR, Font);
                }
                else
                {
                    AntdUI.Notification.warn(form, "��ʾ", "��ʹ����ȷ�ĺڶ�����ͼƬ����ʶ��", AntdUI.TAlignFrom.BR, Font);
                }
            }
        }

        private void ImageProcessBLL_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "OriginImagePath")
            {
                btnPredict.Enabled = !string.IsNullOrEmpty(imageProcessBLL.OriginImagePath);
                if (!string.IsNullOrEmpty(imageProcessBLL.OriginImagePath))
                {
                    avatarOriginImage.Image = System.Drawing.Image.FromFile(imageProcessBLL.OriginImagePath);
                    BtnPredict_Click(btnPredict, null);  // auto predict
                }
                else
                {
                    avatarOriginImage.Image = Properties.Resources.blackness_template;
                }
            }
            else if (e.PropertyName == "RenderImagePath")
            {
                if (!string.IsNullOrEmpty(imageProcessBLL.RenderImagePath))
                {
                    avatarRenderImage.Image = System.Drawing.Image.FromFile(imageProcessBLL.RenderImagePath);
                }
                else
                {
                    avatarRenderImage.Image = Properties.Resources.blackness_template;
                }
            }
            // TODO: save
        }

        private void CameraBLL_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsGrabbing")
            {
                btnCameraRecover.Enabled = !cameraBLL.IsGrabbing;
                btnCameraCapture.Enabled = cameraBLL.IsGrabbing;
            }
        }

        private async Task CloseCameraAsync()
        {
            // TODO: unsaved confirm
            await Task.Delay(50);
            cameraBLL.CloseDevice();
            CameraHelper.CAMERA_DEVICES.Remove(cameraBLL);
        }


        private async Task InitializeCameraAsync()
        {
            try
            {
                var result = cameraBLL.StartRendering();
                switch (result)
                {
                    case "NoCameraSettings":
                        AntdUI.Notification.warn(form, "��ʾ", "����������ͷ����ʵʱ����", AntdUI.TAlignFrom.BR, Font);
                        // �ӳ�1��
                        await Task.Delay(1000);
                        BtnCameraSetting_Click(null, null);
                        break;
                    case "NoCameraOpen":
                        AntdUI.Notification.warn(form, "��ʾ", "�������ͷ����ʵʱ����", AntdUI.TAlignFrom.BR, Font);
                        // �ӳ�1��
                        await Task.Delay(1000);
                        BtnCameraSetting_Click(null, null);
                        break;
                    case "NoCameraGrabbing":
                        AntdUI.Notification.warn(form, "��ʾ", "�뿪���ɼ�����ʵʱ����", AntdUI.TAlignFrom.BR, Font);
                        // �ӳ�1��
                        await Task.Delay(1000);
                        BtnCameraSetting_Click(null, null);
                        break;
                    case "TriggerMode":
                        AntdUI.Notification.success(form, "�ɹ�", "����ģʽ", AntdUI.TAlignFrom.BR, Font);
                        break;
                    case "ContinuousMode":
                        AntdUI.Notification.success(form, "�ɹ�", "ʵʱģʽ", AntdUI.TAlignFrom.BR, Font);
                        break;
                    default:
                        AntdUI.Notification.error(form, "����", "����ϵ����Ա", AntdUI.TAlignFrom.BR, Font);
                        break;
                }
            }
            catch (CameraSDKException error)
            {
                CameraHelper.ShowErrorMsg(form, error.Message, error.ErrorCode);
            }
        }

        private void AvatarOriginImage_Click(object sender, System.EventArgs e)
        {
            AntdUI.Preview.open(new AntdUI.Preview.Config(form, avatarOriginImage.Image));
        }

        private void AvatarRenderImage_Click(object sender, EventArgs e)
        {
            AntdUI.Preview.open(new AntdUI.Preview.Config(form, avatarRenderImage.Image));
        }

        private void BtnUploadImage_Click(object sender, System.EventArgs e)
        {
            if (blacknessMethod_OpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                AntdUI.Button btn = (AntdUI.Button)sender;
                btn.LoadingWaveValue = 0;
                btn.Loading = true;
                AntdUI.ITask.Run(() =>
                {
                    imageProcessBLL.SaveOriginImage(blacknessMethod_OpenFileDialog.FileName);
                    // stop realtime image render
                    cameraBLL.StopGrabbing();
                    AntdUI.Notification.success(form, "�ɹ�", "�ϴ��ɹ���", AntdUI.TAlignFrom.BR, Font);
                    if (btn.IsDisposed) return;
                    btn.Loading = false;
                });
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPredict_Click(object sender, EventArgs e)
        {
            AntdUI.Button btn = (AntdUI.Button)sender;
            btn.LoadingWaveValue = 0;
            btn.Loading = true;
            AntdUI.ITask.Run(() =>
            {
                blacknessPredict.Predict();
            }, () =>
            {
                if (btn.IsDisposed) return;
                btn.Loading = false;
            });
        }

        private void OutputTexts(Prediction[] predictions)
        {
            var resultList = blacknessMethodBLL.ParsePredictions(predictions);
            input_Surface_OP.Text = resultList.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_OP))?.Description;
            input_Surface_CE.Text = resultList.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_CE))?.Description;
            input_Surface_DR.Text = resultList.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_DR))?.Description;
            input_Inside_OP.Text = resultList.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_OP))?.Description;
            input_Inside_CE.Text = resultList.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_CE))?.Description;
            input_Inside_DR.Text = resultList.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_DR))?.Description;
            radio_Result_OK.Checked = resultList.All(t => new List<string> { "3", "4", "5" }.Contains(t.Level)); // [1��2] not exit��otherwise��NG is checked
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // ������֤
            if (blacknessMethodBLL.GetTempMethodResultList() == null)
            {
                AntdUI.Notification.warn(form, "��ʾ", "�����ʶ����ٱ���", AntdUI.TAlignFrom.BR, Font);
                return;
            }
            if (select_Work_Group.SelectedValue == null)
            {
                AntdUI.Notification.warn(form, "��ʾ", "��ѡ�����", AntdUI.TAlignFrom.BR, Font);
                return;
            }
            if (select_Analyst.SelectedValue == null)
            {
                AntdUI.Notification.warn(form, "��ʾ", "��ѡ�������Ա", AntdUI.TAlignFrom.BR, Font);
                return;
            }
            if (string.IsNullOrEmpty(input_Coil_Number.Text))
            {
                AntdUI.Notification.warn(form, "��ʾ", "��������ȷ�ĸ־��", AntdUI.TAlignFrom.BR, Font);
                return;
            }
            if (string.IsNullOrEmpty(input_Size.Text))
            {
                AntdUI.Notification.warn(form, "��ʾ", "��������ȷ�ĳߴ�", AntdUI.TAlignFrom.BR, Font);
                return;
            }
            if (AntdUI.Modal.open(form, "��ȷ��", "�Ƿ񱣴汾�κڶȼ������") == DialogResult.OK)
            {
                AntdUI.Button btn = (AntdUI.Button)sender;
                btn.LoadingWaveValue = 0;
                btn.Loading = true;
                AntdUI.ITask.Run(() =>
                {
                    // ����ڶȲ�������
                    var blacknessMethodResult = new BlacknessMethodResult
                    {
                        CoilNumber = input_Coil_Number.Text,
                        Size = input_Size.Text,
                        OriginImagePath = imageProcessBLL.OriginImagePath,
                        RenderImagePath = imageProcessBLL.RenderImagePath,
                        WorkGroup = select_Work_Group.SelectedValue.ToString(),
                        Analyst = select_Analyst.SelectedValue.ToString(),
                    };
                    var result = blacknessMethodBLL.SaveResult(blacknessMethodResult);
                    if (result == 0)
                    {
                        AntdUI.Notification.error(form, "����", "����ʧ�ܣ�", AntdUI.TAlignFrom.BR, Font);
                        return;
                    }
                    else
                    {
                        AntdUI.Notification.success(form, "�ɹ�", "����ɹ���", AntdUI.TAlignFrom.BR, Font);
                        return;
                    }
                }, () =>
                {
                    if (btn.IsDisposed) return;
                    btn.Loading = false;
                    // ��ֹ��ε����������ͬ�����ݣ�TODO:������ݱ仯�󣬽�����������²���
                    btn.Enabled = false;
                });
            }
        }

        private void BtnCameraSetting_Click(object sender, EventArgs e)
        {
            try
            {
                AntdUI.Drawer.open(form, new CameraSetting(form, cameraBLL)
                {
                    Size = new Size(420, 555)
                }, AntdUI.TAlignMini.Right);
            }
            catch (Exception error)
            {
                AntdUI.Notification.error(form, "����", error.Message, AntdUI.TAlignFrom.BR, Font);
            }

        }

        private void BtnCameraRecover_Click(object sender, EventArgs e)
        {
            // start realtime image render
            cameraBLL.StartGrabbing();
        }

        private void BtnCameraCapture_Click(object sender, EventArgs e)
        {
            imageProcessBLL.OriginImagePath = cameraBLL.SaveImage();
            AntdUI.Button btn = (AntdUI.Button)sender;
            btn.LoadingWaveValue = 0;
            btn.Loading = true;
            AntdUI.ITask.Run(() =>
            {
                // stop realtime image render
                cameraBLL.StopGrabbing();
                if (btn.IsDisposed) return;
                btn.Loading = false;
                AntdUI.Notification.success(form, "�ɹ�", "����ɹ���", AntdUI.TAlignFrom.BR, Font);
            });
        }
    }
}