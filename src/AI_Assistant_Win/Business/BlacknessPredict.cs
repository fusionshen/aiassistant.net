using AI_Assistant_Win.Models;
using AI_Assistant_Win.Utils;
using Emgu.CV;
using Emgu.CV.Structure;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Yolov8.Net;

namespace AI_Assistant_Win.Business
{
    /// <summary>
    /// TODO: switch to yolodotnet
    /// </summary>
    public class BlacknessPredict : INotifyPropertyChanged
    {
        private readonly Font font;

        private readonly string[] labels = File.ReadAllLines("./Resources/Blackness/labels.txt");

        private readonly ImageProcessBLL imageProcessBLL;

        private Prediction[] predictions = new Prediction[6];
        public Prediction[] Predictions
        {
            get { return predictions; }
            set
            {
                if (!predictions.SequenceEqual(value))
                {
                    predictions = value;
                    OnPropertyChanged(nameof(Predictions));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public BlacknessPredict(ImageProcessBLL _imageProcessBLL)
        {
            imageProcessBLL = _imageProcessBLL;
            var fontCollection = new FontCollection();
            var fontFamily = fontCollection.Add("./Resources/SourceHanSansCN-Regular.ttf");
            font = fontFamily.CreateFont(48, FontStyle.Bold);
        }
        public void Predict(CalculateScale currentScale)
        {
            using var yolo = YoloV8Predictor.Create("./Resources/Blackness/v3.onnx", labels, false);

            // **解决文件占用问题**
            byte[] imageData = File.ReadAllBytes(imageProcessBLL.OriginImagePath); // 一次性读取，不再占用文件
            using var imageStream = new MemoryStream(imageData);
            using var image = Image.Load(imageStream); // 现在 `image` 依赖内存，而非原始文件

            var yoloPredictions = yolo.Predict(image);

            if (yoloPredictions.Length > 0)
            {
                if (yoloPredictions.Length == 6)
                {
                    predictions = yoloPredictions.OrderBy(t => t.Rectangle.Y).ToArray();
                }
                else
                {
                    // 现在 `imageProcessBLL.OriginImagePath` 已经不被占用了！
                    using var test = new Image<Bgr, byte>(imageProcessBLL.OriginImagePath);
                    using var matcher = new OcrBasedMatcher();
                    predictions = matcher.MatchRegions(test, yoloPredictions);
                }
                DrawBoxes(image, currentScale);
                imageProcessBLL.SaveRenderImage(image);
            }
            else
            {
                predictions = []; // page clears inputs
                imageProcessBLL.RenderImagePath = string.Empty; // clear render image
            }

            OnPropertyChanged(nameof(Predictions));
        }

        private void DrawBoxes(Image image, CalculateScale currentScale)
        {
            foreach (var pred in predictions)
            {
                if (pred == null)
                {
                    continue;
                }
                var originalImageHeight = image.Height;
                var originalImageWidth = image.Width;

                var x = (int)Math.Max(pred.Rectangle.X, 0);
                var y = (int)Math.Max(pred.Rectangle.Y, 0);
                var width = (int)Math.Min(originalImageWidth - x, pred.Rectangle.Width);
                var height = (int)Math.Min(originalImageHeight - y, pred.Rectangle.Height);
                //Note that the output is already scaled to the original image height and width.

                // Bounding Box Text
                string number = new(pred.Label.Name.Where(char.IsDigit).ToArray());
                string text = $"{LocalizeHelper.LEVEL}{number}{LocalizeHelper.BLACKNESS_WITH}{CalculateRealScale(pred, currentScale)}";
                var size = TextMeasurer.MeasureSize(text, new TextOptions(font));
                var color = GetRetangleColorByNumber(number);

                image.Mutate(d => d.Draw(Pens.Solid(color, 5),
                    new Rectangle(x, y, width, height)));
                image.Mutate(d => d.DrawText(text, font, color, new Point(x, (int)(y - size.Height - 1))));
            }
        }

        private string CalculateRealScale(Prediction pred, CalculateScale calculateScale)
        {
            var result = calculateScale == null ?
                $"{pred.Rectangle.Height:F2}{LocalizeHelper.PIXEL}" :
                $"{pred.Rectangle.Height * calculateScale.Value:F2}{LocalizeHelper.MILLIMETER}";
            return result;
        }

        private static Color GetRetangleColorByNumber(string number)
        {
            var color = Color.Black;
            switch (number)
            {
                case "1":
                    color = Color.DarkRed;
                    break;
                case "2":
                    color = Color.Red;
                    break;
                case "3":
                    color = Color.DarkOrange;
                    break;
                case "4":
                    color = Color.Black;
                    break;
                case "5":
                    color = Color.DarkGreen;
                    break;
                default:
                    break;
            }
            return color;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
