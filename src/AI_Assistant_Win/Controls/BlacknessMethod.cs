using AI_Assistant_Win.Business;
using AI_Assistant_Win.Models.Enums;
using AI_Assistant_Win.Models.Middle;
using AI_Assistant_Win.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AI_Assistant_Win.Controls
{
    /// <summary>
    /// TODO：reconnect camera
    /// </summary>
    public partial class BlacknessMethod : UserControl
    {
        public static string EDIT_ITEM_ID = string.Empty;

        private readonly MainWindow form;

        private readonly BlacknessMethodBLL blacknessMethodBLL;

        private readonly ImageProcessBLL imageProcessBLL;

        private readonly CameraBLL cameraBLL;

        private readonly BlacknessPredict blacknessPredict;

        private BlacknessResult originalBlacknessResult;

        private readonly BlacknessResult tempBlacknessResult;

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
            originalBlacknessResult = new BlacknessResult();
            originalBlacknessResult.PropertyChanged += OriginalBlacknessResult_PropertyChanged;
            tempBlacknessResult = new BlacknessResult();
            tempBlacknessResult.PropertyChanged += TempBlacknessResult_PropertyChanged;
            this.Load += async (s, e) => await InitializeAsync();
            this.HandleDestroyed += async (s, e) => await CloseCameraAsync();
        }

        private void OriginalBlacknessResult_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Id")
            {
                tempBlacknessResult.Id = originalBlacknessResult.Id;
            }
            else if (e.PropertyName == "CoilNumber")
            {
                inputCoilNumber.Text = originalBlacknessResult.CoilNumber;
            }
            else if (e.PropertyName == "OriginImagePath")
            {
                imageProcessBLL.OriginImagePath = originalBlacknessResult.OriginImagePath;
            }
            else if (e.PropertyName == "RenderImagePath")
            {
                imageProcessBLL.RenderImagePath = originalBlacknessResult.RenderImagePath;
            }
            else if (e.PropertyName == "WorkGroup")
            {
                selectWorkGroup.SelectedValue = originalBlacknessResult.WorkGroup;
            }
            else if (e.PropertyName == "Analyst")
            {
                selectAnalyst.SelectedValue = originalBlacknessResult.Analyst;
            }
            else if (e.PropertyName == "Size")
            {
                inputSize.Text = originalBlacknessResult.Size;
            }
            else if (e.PropertyName == "Items")
            {
                // 观察者模式虽好，但是跳来跳去，不好好考虑，真的容易多写代码
                blacknessPredict.Predictions = originalBlacknessResult.Items.Select(x => x.Prediction).ToArray();
            }
        }

        private void TempBlacknessResult_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!originalBlacknessResult.Equals(tempBlacknessResult))
            {
                try
                {
                    CheckValid();
                    btnSave.Enabled = true;
                }
                catch (Exception)
                {
                    btnSave.Enabled = false;
                }
            }
            else
            {
                btnSave.Enabled = false;
            }
            MainWindow.SOMETHING_IS_UNDONE = btnSave.Enabled;
        }

        private void CheckValid()
        {
            // 数据验证
            if (tempBlacknessResult == null || tempBlacknessResult.Items.Count == 0)
            {
                throw new ArgumentNullException("请进行识别后再保存");
            }
            if (selectWorkGroup.SelectedValue == null)
            {
                throw new ArgumentNullException("请选择班组");
            }
            if (selectAnalyst.SelectedValue == null)
            {
                throw new ArgumentNullException("请选择分析人员");
            }
            if (string.IsNullOrEmpty(inputCoilNumber.Text))
            {
                throw new ArgumentNullException("请输入正确的钢卷号");
            }
            if (string.IsNullOrEmpty(inputSize.Text))
            {
                throw new ArgumentNullException("请输入正确的尺寸");
            }
        }

        private void BlacknessPredict_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Predictions")
            {
                if (blacknessPredict.Predictions != null && blacknessPredict.Predictions.Length != 0)
                {
                    OutputTexts();
                    // call when edit
                    //AntdUI.Notification.success(form, "成功", "识别成功！", AntdUI.TAlignFrom.BR, Font);
                }
                else
                {
                    ClearTexts();
                    AntdUI.Notification.warn(form, "提示", "请使用正确的黑度样板图片进行识别", AntdUI.TAlignFrom.BR, Font);
                }
            }
        }

        private void ImageProcessBLL_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "OriginImagePath")
            {
                tempBlacknessResult.OriginImagePath = imageProcessBLL.OriginImagePath;
                btnPredict.Enabled = !string.IsNullOrEmpty(imageProcessBLL.OriginImagePath);
                if (!string.IsNullOrEmpty(imageProcessBLL.OriginImagePath))
                {
                    avatarOriginImage.Image = Image.FromFile(imageProcessBLL.OriginImagePath);
                    // cant aotu predict under the status of editing
                    if (!originalBlacknessResult.OriginImagePath.Equals(tempBlacknessResult.OriginImagePath))
                    {
                        BtnPredict_Click(btnPredict, null);  // auto predict
                    }
                }
                else
                {
                    avatarOriginImage.Image = Properties.Resources.blackness_template;
                }
            }
            else if (e.PropertyName == "RenderImagePath")
            {
                tempBlacknessResult.RenderImagePath = imageProcessBLL.RenderImagePath;
                if (!string.IsNullOrEmpty(imageProcessBLL.RenderImagePath))
                {
                    avatarRenderImage.Image = Image.FromFile(imageProcessBLL.RenderImagePath);
                }
                else
                {
                    avatarRenderImage.Image = Properties.Resources.blackness_template;
                }
            }
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
            EDIT_ITEM_ID = string.Empty;
            await Task.Delay(50);
            cameraBLL.CloseDevice();
            CameraHelper.CAMERA_DEVICES.Remove(cameraBLL);
        }

        private async Task InitializeAsync()
        {
            if (!string.IsNullOrEmpty(EDIT_ITEM_ID))
            {
                // 为了通过观察者模式实现界面数据效果
                blacknessMethodBLL.LoadOriginalResultFromDB(originalBlacknessResult, EDIT_ITEM_ID);
                return;
            }
            try
            {
                var result = cameraBLL.StartRendering();
                switch (result)
                {
                    case "NoCamera":
                        AntdUI.Notification.warn(form, "提示", "未能找到可用摄像头", AntdUI.TAlignFrom.BR, Font);
                        break;
                    case "NoCameraSettings":
                        AntdUI.Notification.warn(form, "提示", "请设置摄像头进行实时拍摄", AntdUI.TAlignFrom.BR, Font);
                        // 延迟1秒
                        await Task.Delay(1000);
                        BtnCameraSetting_Click(null, null);
                        break;
                    case "NoCameraOpen":
                        AntdUI.Notification.warn(form, "提示", "请打开摄像头进行实时拍摄", AntdUI.TAlignFrom.BR, Font);
                        // 延迟1秒
                        await Task.Delay(1000);
                        BtnCameraSetting_Click(null, null);
                        break;
                    case "NoCameraGrabbing":
                        AntdUI.Notification.warn(form, "提示", "请开启采集进行实时拍摄", AntdUI.TAlignFrom.BR, Font);
                        // 延迟1秒
                        await Task.Delay(1000);
                        BtnCameraSetting_Click(null, null);
                        break;
                    case "TriggerMode":
                        AntdUI.Notification.success(form, "成功", "触发模式", AntdUI.TAlignFrom.BR, Font);
                        break;
                    case "ContinuousMode":
                        AntdUI.Notification.success(form, "成功", "实时模式", AntdUI.TAlignFrom.BR, Font);
                        break;
                    default:
                        AntdUI.Notification.error(form, "错误", "请联系管理员", AntdUI.TAlignFrom.BR, Font);
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
                    AntdUI.Notification.success(form, "成功", "上传成功！", AntdUI.TAlignFrom.BR, Font);
                    if (btn.IsDisposed) return;
                    btn.Loading = false;
                });
            }
        }

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

        private void OutputTexts()
        {
            var sorted = blacknessPredict.Predictions.OrderBy(t => t.Rectangle.Y).ToArray();
            // TODO: 现场根据X具体位置判断，因为可能出现未贴完6个部位或者未识别出六个部位的情况
            tempBlacknessResult.Items = [new(BlacknessLocationKind.SURFACE_OP, sorted[0]),
                            new(BlacknessLocationKind.SURFACE_CE, sorted[1]),
                            new(BlacknessLocationKind.SURFACE_DR, sorted[2]),
                            new(BlacknessLocationKind.INSIDE_OP, sorted[3]),
                            new(BlacknessLocationKind.INSIDE_CE, sorted[4]),
                            new(BlacknessLocationKind.INSIDE_DR, sorted[5])];
            inputSurfaceOP.Text = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_OP))?.Description;
            inputSurfaceCE.Text = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_CE))?.Description;
            inputSurfaceDR.Text = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_DR))?.Description;
            inputInsideOP.Text = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_OP))?.Description;
            inputInsideCE.Text = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_CE))?.Description;
            inputInsideDR.Text = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_DR))?.Description;
            radioResultOK.Checked = tempBlacknessResult.Items.All(t => new List<string> { "3", "4", "5" }.Contains(t.Level)); // [1，2] not exit，otherwise，NG is checked
        }

        private void ClearTexts()
        {
            tempBlacknessResult.Items = [];
            inputSurfaceOP.Text = string.Empty;
            inputSurfaceCE.Text = string.Empty;
            inputSurfaceDR.Text = string.Empty;
            inputInsideOP.Text = string.Empty;
            inputInsideCE.Text = string.Empty;
            inputInsideDR.Text = string.Empty;
            radioResultNG.Checked = true;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // double check
            try
            {
                CheckValid();
            }
            catch (Exception error)
            {
                AntdUI.Notification.warn(form, "提示", error.Message, AntdUI.TAlignFrom.BR, Font);
                return;
            }
            if (AntdUI.Modal.open(form, "请确认", "是否保存本次黑度检测结果？") == DialogResult.OK)
            {
                AntdUI.Button btn = (AntdUI.Button)sender;
                btn.LoadingWaveValue = 0;
                btn.Loading = true;
                AntdUI.ITask.Run(() =>
                {
                    try
                    {
                        var result = blacknessMethodBLL.SaveResult(tempBlacknessResult);
                        if (result == 0)
                        {
                            AntdUI.Notification.error(form, "错误", "保存失败！", AntdUI.TAlignFrom.BR, Font);
                            return;
                        }
                        else
                        {
                            tempBlacknessResult.Id = result;
                            originalBlacknessResult = (BlacknessResult)tempBlacknessResult.Clone();
                            btn.Enabled = false;
                            AntdUI.Notification.success(form, "成功", "保存成功！", AntdUI.TAlignFrom.BR, Font);
                            return;
                        }
                    }
                    catch (Exception error)
                    {

                        AntdUI.Notification.error(form, "错误", error.Message, AntdUI.TAlignFrom.BR, Font);
                        return;
                    }
                }, () =>
                {
                    if (btn.IsDisposed) return;
                    btn.Loading = false;
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
                AntdUI.Notification.error(form, "错误", error.Message, AntdUI.TAlignFrom.BR, Font);
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
                AntdUI.Notification.success(form, "成功", "拍摄成功！", AntdUI.TAlignFrom.BR, Font);
            });
        }

        private void SelectWorkGroup_SelectedIndexChanged(object sender, AntdUI.IntEventArgs e)
        {
            tempBlacknessResult.WorkGroup = selectWorkGroup.SelectedValue.ToString();
        }

        private void SelectAnalyst_SelectedIndexChanged(object sender, AntdUI.IntEventArgs e)
        {
            tempBlacknessResult.Analyst = selectAnalyst.SelectedValue.ToString();
        }

        private void InputCoilNumber_TextChanged(object sender, EventArgs e)
        {
            tempBlacknessResult.CoilNumber = inputCoilNumber.Text;
        }

        private void InputSize_TextChanged(object sender, EventArgs e)
        {
            tempBlacknessResult.Size = inputSize.Text;
        }
    }
}