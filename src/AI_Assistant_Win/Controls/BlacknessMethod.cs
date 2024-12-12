using AI_Assistant_Win.Business;
using AI_Assistant_Win.Models;
using AI_Assistant_Win.Models.Enums;
using AI_Assistant_Win.Models.Middle;
using AI_Assistant_Win.Utils;
using SixLabors.Fonts;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Yolov8.Net;
using FontStyle = SixLabors.Fonts.FontStyle;

namespace AI_Assistant_Win.Controls
{
    /// <summary>
    /// init camera, if camera was binded, origin image zone will show the image.
    /// reconnect camera
    /// </summary>
    public partial class BlacknessMethod : UserControl
    {
        private readonly MainWindow form;

        private readonly SixLabors.Fonts.Font font;

        private string originImagePath = string.Empty;

        private string renderImagePath = string.Empty;

        private readonly BlacknessMethodBLL blacknessMethodBLL;

        private readonly ImageProcessBLL imageProcessBLL;

        private readonly CameraBLL cameraBLL;

        public BlacknessMethod(MainWindow _form)
        {
            var fontCollection = new FontCollection();
            var fontFamily = fontCollection.Add("./Resources/SourceHanSansCN-Regular.ttf");
            font = fontFamily.CreateFont(48, FontStyle.Bold);
            blacknessMethodBLL = new BlacknessMethodBLL();
            imageProcessBLL = new ImageProcessBLL();
            cameraBLL = new CameraBLL();
            CameraHelper.CAMERA_DEVICES.Add(cameraBLL);
            form = _form;
            InitializeComponent();
            this.Load += async (s, e) => await InitializeCameraAsync();
            this.HandleDestroyed += async (s, e) => await CloseCameraAsync();
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
                var result = cameraBLL.StartGrabbing(new CameraGrabbing { Application = "blackness", ImageHandle = avatarOriginImage.Handle });
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

        private void BlacknessMethod_renderImage_Click(object sender, EventArgs e)
        {
            AntdUI.Preview.open(new AntdUI.Preview.Config(form, blacknessMethod_RenderImage.Image));
        }

        private void BtnUploadImage_Click(object sender, System.EventArgs e)
        {
            if (blacknessMethod_OpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                originImagePath = imageProcessBLL.SaveOriginImageAndReturnPath(blacknessMethod_OpenFileDialog.FileName);
                AntdUI.Button btn = (AntdUI.Button)sender;
                btn.LoadingWaveValue = 0;
                btn.Loading = true;
                AntdUI.ITask.Run(() =>
                {
                    // stop realtime image render
                    cameraBLL.StopGrabbing();
                    avatarOriginImage.Image = System.Drawing.Image.FromFile(originImagePath);
                    if (btn.IsDisposed) return;
                    btn.Loading = false;
                    AntdUI.Notification.success(form, "�ɹ�", "�ϴ��ɹ���", AntdUI.TAlignFrom.BR, Font);
                    BtnPredict_Click(btnPredict, null);  // auto predict
                });
            }
        }

        /// <summary>
        /// System.IO.IOException:��The process cannot access the file 'i_assistant_blackness_result.jpg' because it is being used by another process.
        /// ��ʹ�� SixLabors.ImageSharp �⴦��ͼ��ʱ��Image.Load �� Image.Save �����ڲ�������ļ����Ĵ򿪺͹رա�
        /// ͨ�����㲻��Ҫ�ֶ��ر��ļ�������Ϊ ImageSharp ���� Image ���󱻴��ã�disposed��ʱ�Զ�������Щ��Դ��
        /// Ȼ�������������£�������������ζ�ͬһ���ļ�ʹ�� Save ���������������ڶ�ͬһ�� Image ����������������α���֮��û�����¼���ͼ�񣩣�
        /// ImageSharp Ӧ�û���ȷ�����ļ����Ĺرպ����´򿪣������Ҫ�Ļ��������ǣ�ͨ��û�б�Ҫ��ͬһ���ļ�ִ�����α�������������������α���֮���ͼ��������޸ġ�
        /// �����ȷʵ��Ҫ�����α���֮���ͼ������޸ģ�������Ҫȷ����Դ����ȷ���������ʹ�� using �����ȷ�� Image �����ڲ�����Ҫʱ����ȷ���á�������һ��ʾ�����룺
        /// csharp
        /// using SixLabors.ImageSharp;
        /// using SixLabors.ImageSharp.Formats.Jpeg;
        /// using System;
        /// public class ImageProcessingExample
        /// {
        ///        public static void Main()
        ///        {
        ///            string filePath = "ai_assistant_blackness_result.jpg";
        ///           // ʹ�� using �����ȷ�� Image ������ȷ����
        ///            using (var image = Image.Load(filePath))
        ///            {
        ///                // ��ͼ������޸ģ������Ҫ�Ļ���
        ///                // ...
        ///                // ��һ�α���
        ///                image.Save(filePath, new JpegEncoder());
        ///                // ��ͼ����н�һ�����޸ģ������Ҫ�Ļ���
        ///                // ...
        ///                // �ڶ��α��棨����֮ǰ���ļ���
        ///                image.Save(filePath, new JpegEncoder());
        ///            }
        ///            // ��ʱ��image �����Ѿ������ã��ļ���Ҳ���ر���
        ///        }
        /// }
        /// �����ʾ���У�using ���ȷ���� Image �����ڴ�������ʱ����ȷ���ã�������ر��κδ򿪵��ļ�����
        /// �����������α���֮��û�ж�ͼ��������¼��أ����� ImageSharp �ᴦ���ļ��������´򿪣������Ҫ�Ļ��������յĹرա�
        /// ��ע�⣬�����Ĵ��������α���֮�����¼�����ͼ�����磬ͨ���ٴε��� Image.Load������ôÿ�μ��ض��ᴴ��һ���µ� Image �������ص��ļ�����
        /// ����������£�ÿ�� Image ����Ӧ��ʹ�� using �����ȷ�����Ǳ���ȷ���á�
        /// ����������ļ��������Ĵ��󣬿�������Ϊ�����������ڷ��ʸ��ļ���������Ĵ�������δ��ȷ���õ� Image ����
        /// ȷ����Ĵ�����û��δ�رյ� Image ����ʵ��������û���������̣���ͼƬ�鿴����༭��������ʹ�ø��ļ���
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
                var labels = File.ReadAllLines("./Resources/Blackness/labels.txt");
                using var yolo = YoloV8Predictor.Create("./Resources/blackness/model.onnx", labels, false);
                if (string.IsNullOrEmpty(originImagePath))
                {
                    AntdUI.Notification.warn(form, "��ʾ", "����������ϴ�ͼ�����ʶ��", AntdUI.TAlignFrom.BR, Font);
                    return;
                }
                using var image = SixLabors.ImageSharp.Image.Load(originImagePath);
                var predictions = yolo.Predict(image);
                if (predictions.Length == 6)
                {
                    AntdUI.Notification.success(form, "�ɹ�", "ʶ��ɹ���", AntdUI.TAlignFrom.BR, Font);
                    DrawBoxes(yolo.ModelInputHeight, yolo.ModelInputWidth, image, predictions);
                    OutputTexts(predictions);
                    renderImagePath = imageProcessBLL.SaveRenderImageAndReturnPath(image);
                    blacknessMethod_RenderImage.Image = System.Drawing.Image.FromFile(renderImagePath);
                }
                else
                {
                    AntdUI.Notification.warn(form, "��ʾ", "��ʹ����ȷ�ĺڶ�����ͼƬ����ʶ��", AntdUI.TAlignFrom.BR, Font);
                }
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

        private void DrawBoxes(int modelInputHeight, int modelInputWidth, SixLabors.ImageSharp.Image image, Prediction[] predictions)
        {
            foreach (var pred in predictions)
            {
                var originalImageHeight = image.Height;
                var originalImageWidth = image.Width;

                var x = (int)Math.Max(pred.Rectangle.X, 0);
                var y = (int)Math.Max(pred.Rectangle.Y, 0);
                var width = (int)Math.Min(originalImageWidth - x, pred.Rectangle.Width);
                var height = (int)Math.Min(originalImageHeight - y, pred.Rectangle.Height);
                //Note that the output is already scaled to the original image height and width.

                // Bounding Box Text
                string number = new(pred.Label.Name.Where(char.IsDigit).ToArray());
                string text = $"�ȼ�{number}����ȣ�{pred.Rectangle.Height:F2}mm";
                var size = TextMeasurer.MeasureSize(text, new TextOptions(font));
                var color = GetRetangleColorByNumber(number);

                image.Mutate(d => d.Draw(SixLabors.ImageSharp.Drawing.Processing.Pens.Solid(color, 5),
                    new SixLabors.ImageSharp.Rectangle(x, y, width, height)));
                image.Mutate(d => d.DrawText(text, font, color, new SixLabors.ImageSharp.Point(x, (int)(y - size.Height - 1))));
            }
        }

        private static SixLabors.ImageSharp.Color GetRetangleColorByNumber(string number)
        {
            var color = SixLabors.ImageSharp.Color.Black;
            switch (number)
            {
                case "1":
                    color = SixLabors.ImageSharp.Color.DarkRed;
                    break;
                case "2":
                    color = SixLabors.ImageSharp.Color.Red;
                    break;
                case "3":
                    color = SixLabors.ImageSharp.Color.DarkOrange;
                    break;
                case "4":
                    color = SixLabors.ImageSharp.Color.Yellow;
                    break;
                case "5":
                    color = SixLabors.ImageSharp.Color.Green;
                    break;
                default:
                    break;
            }
            return color;
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
                        OriginImagePath = originImagePath,
                        RenderImagePath = renderImagePath,
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
            originImagePath = cameraBLL.SaveImage();
            AntdUI.Button btn = (AntdUI.Button)sender;
            btn.LoadingWaveValue = 0;
            btn.Loading = true;
            AntdUI.ITask.Run(() =>
            {
                // stop realtime image render
                cameraBLL.StopGrabbing();
                avatarOriginImage.Image = System.Drawing.Image.FromFile(originImagePath);
                if (btn.IsDisposed) return;
                btn.Loading = false;
                AntdUI.Notification.success(form, "�ɹ�", "����ɹ���", AntdUI.TAlignFrom.BR, Font);
                BtnPredict_Click(btnPredict, null);  // auto predict
            });
        }
    }
}