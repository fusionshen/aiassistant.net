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
                    AntdUI.Notification.success(form, "成功", "上传成功！", AntdUI.TAlignFrom.BR, Font);
                    BtnPredict_Click(btnPredict, null);  // auto predict
                });
            }
        }

        /// <summary>
        /// System.IO.IOException:“The process cannot access the file 'i_assistant_blackness_result.jpg' because it is being used by another process.
        /// 在使用 SixLabors.ImageSharp 库处理图像时，Image.Load 和 Image.Save 方法内部会管理文件流的打开和关闭。
        /// 通常，你不需要手动关闭文件流，因为 ImageSharp 会在 Image 对象被处置（disposed）时自动处理这些资源。
        /// 然而，在你的情况下，如果你连续两次对同一个文件使用 Save 方法（假设你是在对同一个 Image 对象操作，并且两次保存之间没有重新加载图像），
        /// ImageSharp 应该会正确处理文件流的关闭和重新打开（如果需要的话）。但是，通常没有必要对同一个文件执行两次保存操作，除非你在两次保存之间对图像进行了修改。
        /// 如果你确实需要在两次保存之间对图像进行修改，并且想要确保资源被正确管理，你可以使用 using 语句来确保 Image 对象在不再需要时被正确处置。这里是一个示例代码：
        /// csharp
        /// using SixLabors.ImageSharp;
        /// using SixLabors.ImageSharp.Formats.Jpeg;
        /// using System;
        /// public class ImageProcessingExample
        /// {
        ///        public static void Main()
        ///        {
        ///            string filePath = "ai_assistant_blackness_result.jpg";
        ///           // 使用 using 语句来确保 Image 对象被正确处置
        ///            using (var image = Image.Load(filePath))
        ///            {
        ///                // 对图像进行修改（如果需要的话）
        ///                // ...
        ///                // 第一次保存
        ///                image.Save(filePath, new JpegEncoder());
        ///                // 对图像进行进一步的修改（如果需要的话）
        ///                // ...
        ///                // 第二次保存（覆盖之前的文件）
        ///                image.Save(filePath, new JpegEncoder());
        ///            }
        ///            // 此时，image 对象已经被处置，文件流也被关闭了
        ///        }
        /// }
        /// 在这个示例中，using 语句确保了 Image 对象在代码块结束时被正确处置，这包括关闭任何打开的文件流。
        /// 由于你在两次保存之间没有对图像进行重新加载，所以 ImageSharp 会处理文件流的重新打开（如果需要的话）和最终的关闭。
        /// 请注意，如果你的代码在两次保存之间重新加载了图像（例如，通过再次调用 Image.Load），那么每次加载都会创建一个新的 Image 对象和相关的文件流。
        /// 在这种情况下，每个 Image 对象都应该使用 using 语句来确保它们被正确处置。
        /// 如果你遇到文件被锁定的错误，可能是因为其他进程正在访问该文件，或者你的代码中有未正确处置的 Image 对象。
        /// 确保你的代码中没有未关闭的 Image 对象实例，并且没有其他进程（如图片查看器或编辑器）正在使用该文件。
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
                    AntdUI.Notification.warn(form, "提示", "请拍摄或者上传图后进行识别", AntdUI.TAlignFrom.BR, Font);
                    return;
                }
                using var image = SixLabors.ImageSharp.Image.Load(originImagePath);
                var predictions = yolo.Predict(image);
                if (predictions.Length == 6)
                {
                    AntdUI.Notification.success(form, "成功", "识别成功！", AntdUI.TAlignFrom.BR, Font);
                    DrawBoxes(yolo.ModelInputHeight, yolo.ModelInputWidth, image, predictions);
                    OutputTexts(predictions);
                    renderImagePath = imageProcessBLL.SaveRenderImageAndReturnPath(image);
                    blacknessMethod_RenderImage.Image = System.Drawing.Image.FromFile(renderImagePath);
                }
                else
                {
                    AntdUI.Notification.warn(form, "提示", "请使用正确的黑度样板图片进行识别", AntdUI.TAlignFrom.BR, Font);
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
            radio_Result_OK.Checked = resultList.All(t => new List<string> { "3", "4", "5" }.Contains(t.Level)); // [1，2] not exit，otherwise，NG is checked
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
                string text = $"等级{number}，宽度：{pred.Rectangle.Height:F2}mm";
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
            // 数据验证
            if (blacknessMethodBLL.GetTempMethodResultList() == null)
            {
                AntdUI.Notification.warn(form, "提示", "请进行识别后再保存", AntdUI.TAlignFrom.BR, Font);
                return;
            }
            if (select_Work_Group.SelectedValue == null)
            {
                AntdUI.Notification.warn(form, "提示", "请选择班组", AntdUI.TAlignFrom.BR, Font);
                return;
            }
            if (select_Analyst.SelectedValue == null)
            {
                AntdUI.Notification.warn(form, "提示", "请选择分析人员", AntdUI.TAlignFrom.BR, Font);
                return;
            }
            if (string.IsNullOrEmpty(input_Coil_Number.Text))
            {
                AntdUI.Notification.warn(form, "提示", "请输入正确的钢卷号", AntdUI.TAlignFrom.BR, Font);
                return;
            }
            if (string.IsNullOrEmpty(input_Size.Text))
            {
                AntdUI.Notification.warn(form, "提示", "请输入正确的尺寸", AntdUI.TAlignFrom.BR, Font);
                return;
            }
            if (AntdUI.Modal.open(form, "请确认", "是否保存本次黑度检测结果？") == DialogResult.OK)
            {
                AntdUI.Button btn = (AntdUI.Button)sender;
                btn.LoadingWaveValue = 0;
                btn.Loading = true;
                AntdUI.ITask.Run(() =>
                {
                    // 构造黑度测试数据
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
                        AntdUI.Notification.error(form, "错误", "保存失败！", AntdUI.TAlignFrom.BR, Font);
                        return;
                    }
                    else
                    {
                        AntdUI.Notification.success(form, "成功", "保存成功！", AntdUI.TAlignFrom.BR, Font);
                        return;
                    }
                }, () =>
                {
                    if (btn.IsDisposed) return;
                    btn.Loading = false;
                    // 防止多次点击，操作相同的数据，TODO:检测数据变化后，进行新增或更新操作
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
                AntdUI.Notification.success(form, "成功", "拍摄成功！", AntdUI.TAlignFrom.BR, Font);
                BtnPredict_Click(btnPredict, null);  // auto predict
            });
        }
    }
}