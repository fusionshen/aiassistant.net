using AI_Assistant_Win.Models;
using AI_Assistant_Win.Utils;
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
            using var image = Image.Load(imageProcessBLL.OriginImagePath);
            predictions = yolo.Predict(image);
            if (predictions.Length == 6)
            {
                DrawBoxes(image, currentScale);
                imageProcessBLL.SaveRenderImage(image);  // show render image 
            }
            else
            {
                predictions = []; // page clears inputs
                imageProcessBLL.RenderImagePath = string.Empty; // clear render image 
            }
            OnPropertyChanged(nameof(Predictions)); //  page outputs/clears predictions
        }

        private void DrawBoxes(Image image, CalculateScale currentScale)
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
