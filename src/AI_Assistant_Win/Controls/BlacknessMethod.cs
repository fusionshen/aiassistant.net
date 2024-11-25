using AI_Assistant_Win.Entities;
using AI_Assistant_Win.Entities.Enums;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Yolov8.Net;
using FontStyle = SixLabors.Fonts.FontStyle;

namespace AI_Assistant_Win.Controls
{
    public partial class BlacknessMethod : UserControl
    {
        private readonly MainWindow form;

        private readonly SixLabors.Fonts.Font font;

        private string originImagePath = string.Empty;

        public BlacknessMethod(MainWindow _form)
        {
            var fontCollection = new FontCollection();
            var fontFamily = fontCollection.Add("./Resources/SourceHanSansCN-Regular.ttf");
            font = fontFamily.CreateFont(48, FontStyle.Bold);
            form = _form;
            InitializeComponent();
        }

        private void OriginImage_Click(object sender, System.EventArgs e)
        {
            AntdUI.Preview.open(new AntdUI.Preview.Config(form, blacknessMethod_OriginImage.Image));
        }

        private void BlacknessMethod_renderImage_Click(object sender, EventArgs e)
        {
            AntdUI.Preview.open(new AntdUI.Preview.Config(form, blacknessMethod_RenderImage.Image));

        }

        private void Btn_UploadImage_Click(object sender, System.EventArgs e)
        {
            if (blacknessMethod_OpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                originImagePath = blacknessMethod_OpenFileDialog.FileName;
                AntdUI.Button btn = (AntdUI.Button)sender;
                btn.LoadingWaveValue = 0;
                btn.Loading = true;
                AntdUI.ITask.Run(() =>
                {
                    blacknessMethod_OriginImage.Image = System.Drawing.Image.FromFile(originImagePath);
                    if (btn.IsDisposed) return;
                    btn.Loading = false;
                    AntdUI.Notification.success(form, "�ɹ�", "�ϴ��ɹ���", AntdUI.TAlignFrom.BR, Font);
                    Btn_Predict_Click(btn_Predict, null);  // auto predict
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
        private void Btn_Predict_Click(object sender, EventArgs e)
        {
            AntdUI.Button btn = (AntdUI.Button)sender;
            btn.LoadingWaveValue = 0;
            btn.Loading = true;
            AntdUI.ITask.Run(() =>
            {
                var labels = File.ReadAllLines("./Resources/Blackness/labels.txt");
                using var yolo = YoloV8Predictor.Create("./Resources/blackness/model.onnx", labels, false);

                using var image = SixLabors.ImageSharp.Image.Load(originImagePath);
                var predictions = yolo.Predict(image);
                if (predictions.Length == 6)
                {
                    AntdUI.Notification.success(form, "�ɹ�", "ʶ��ɹ���", AntdUI.TAlignFrom.BR, Font);
                    DrawBoxes(yolo.ModelInputHeight, yolo.ModelInputWidth, image, predictions);
                    OutputTexts(predictions);
                    image.Save("ai_assistant_blackness_result.jpg");
                    blacknessMethod_RenderImage.Image = System.Drawing.Image.FromFile("ai_assistant_blackness_result.jpg");
                }
                else
                {
                    AntdUI.Notification.error(form, "����", "��ʹ����ȷ�ĺڶ�����ͼƬ����ʶ��", AntdUI.TAlignFrom.BR, Font);
                }
            }, () =>
            {
                if (btn.IsDisposed) return;
                btn.Loading = false;
            });
        }

        private void OutputTexts(Prediction[] predictions)
        {
            var sorted = predictions.OrderByDescending(t => t.Rectangle.X).ToArray();
            // TODO: �ֳ�����X����λ���жϣ���Ϊ���ܳ���δ����6����λ����δʶ���������λ�����
            var resultList = new List<Blackness>
                {
                    new(Entities.Enums.BlacknessLocationKind.SURFACE_OP, sorted[0]),
                    new(Entities.Enums.BlacknessLocationKind.SURFACE_CE, sorted[1]),
                    new(Entities.Enums.BlacknessLocationKind.SURFACE_DR, sorted[2]),
                    new(Entities.Enums.BlacknessLocationKind.INSIDE_OP, sorted[3]),
                    new(Entities.Enums.BlacknessLocationKind.INSIDE_CE, sorted[4]),
                    new(Entities.Enums.BlacknessLocationKind.INSIDE_DR, sorted[5])
                };

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

        private void Btn_CameraCapture_Click(object sender, EventArgs e)
        {
            AntdUI.Notification.warn(form, "����", "�����У������ڴ���", AntdUI.TAlignFrom.BR, Font);
        }

        private void Btn_Save_Click(object sender, EventArgs e)
        {

        }
    }
}