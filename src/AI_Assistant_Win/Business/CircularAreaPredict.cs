using AI_Assistant_Win.Models;
using AI_Assistant_Win.Models.Middle;
using AI_Assistant_Win.Utils;
using SkiaSharp;
using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using YoloDotNet;
using YoloDotNet.Configuration;
using YoloDotNet.Enums;
using YoloDotNet.Extensions;
using YoloDotNet.Models;

namespace AI_Assistant_Win.Business
{
    public class CircularAreaPredict : INotifyPropertyChanged
    {
        private readonly SKFont font;

        private readonly ImageProcessBLL imageProcessBLL;

        private SimpleSegmentation prediction;
        public SimpleSegmentation Prediction
        {
            get { return prediction; }
            set
            {
                if (prediction == null || !prediction.Equals(value))
                {
                    prediction = value;
                    OnPropertyChanged(nameof(Prediction));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public CircularAreaPredict(ImageProcessBLL _imageProcessBLL)
        {
            imageProcessBLL = _imageProcessBLL;
            string fontFilePath = "./Resources/SourceHanSansCN-Regular.ttf";
            using var stream = new FileStream(fontFilePath, FileMode.Open, FileAccess.Read);
            SKTypeface typeface = SKTypeface.FromStream(stream);
            font = new SKFont(typeface, 48);
        }
        public void Predict(CalculateScale currentScale)
        {
            var yolo = new Yolo(new YoloOptions
            {
                OnnxModel = "./Resources/Circular/model.onnx",
                ModelType = ModelType.Segmentation,
                Cuda = false
            });

            using var image = SKImage.FromEncodedData(imageProcessBLL.OriginImagePath);

            var predictions = yolo.RunSegmentation(image, 0.25, 0.65, 0.45);

            if (predictions.Count == 1)  // Label.Name = Circular
            {
                prediction = new SimpleSegmentation
                {
                    Label = predictions[0].Label,
                    Confidence = predictions[0].Confidence,
                    BoundingBox = predictions[0].BoundingBox,
                    SegmentedPixelsCount = predictions[0].SegmentedPixels.Length,
                };
                var rendered = DrawBoxes(image, currentScale, predictions[0]);
                imageProcessBLL.SaveRenderImage(rendered);  // show render image 
            }
            else
            {
                prediction = null; // page clears inputs
                imageProcessBLL.RenderImagePath = string.Empty; // clear render image 
            }

            OnPropertyChanged(nameof(Prediction)); //  page outputs/clears predictions
        }

        private SKImage DrawBoxes(SKImage image, CalculateScale currentScale, Segmentation segmentation)
        {
            ArgumentNullException.ThrowIfNull(segmentation);

            // Convert SKImage to SKBitmap to ensure pixel data is accessible
            SKBitmap bitmap = SKBitmap.FromImage(image);
            IntPtr pixelsPtr = bitmap.GetPixels();
            int width = bitmap.Width;
            int height = bitmap.Height;
            int bytesPerPixel = bitmap.BytesPerPixel;
            int rowBytes = bitmap.RowBytes;
            // Define the overlay color
            var color = HexToRgbaSkia(segmentation.Label.Color, ImageConfig.SEGMENTATION_MASK_OPACITY);

            var pixelSpan = segmentation.SegmentedPixels.AsSpan();

            unsafe
            {
                // Access pixel data directly from memory for higher performance
                byte* pixelData = (byte*)pixelsPtr.ToPointer();

                foreach (var pixel in pixelSpan)
                {
                    int x = pixel.X;
                    int y = pixel.Y;

                    // Prevent any attempt to access or modify pixel data outside the valid range!
                    if (x < 0 || x >= width || y < 0 || y >= height)
                        continue;

                    // Calculate the index for the pixel
                    int index = y * rowBytes + x * bytesPerPixel;

                    // Get original pixel colors
                    byte blue = pixelData[index];
                    byte green = pixelData[index + 1];
                    byte red = pixelData[index + 2];
                    byte alpha = pixelData[index + 3];

                    // Blend the overlay color with the original color
                    byte newRed = (byte)((red * (255 - color.Alpha) + color.Red * color.Alpha) / 255);
                    byte newGreen = (byte)((green * (255 - color.Alpha) + color.Green * color.Alpha) / 255);
                    byte newBlue = (byte)((blue * (255 - color.Alpha) + color.Blue * color.Alpha) / 255);

                    // Set the new color
                    pixelData[index + 0] = newBlue;
                    pixelData[index + 1] = newGreen;
                    pixelData[index + 2] = newRed;
                    pixelData[index + 3] = alpha; // Preserve the original alpha
                }
            }

            return DrawBoundingBoxes(SKImage.FromBitmap(bitmap), segmentation, currentScale);
        }

        private SKImage DrawBoundingBoxes(SKImage image, Segmentation detection, CalculateScale currentScale)
        {
            ArgumentNullException.ThrowIfNull(prediction);

            var fontSize = image.CalculateDynamicSize(ImageConfig.FONT_SIZE);
            var borderThickness = image.CalculateDynamicSize(ImageConfig.BORDER_THICKNESS);

            //float fontSize = image.CalculateFontSize(ImageConfig.DEFAULT_FONT_SIZE);
            var margin = (int)fontSize / 2;
            var labelBoxHeight = (int)fontSize * 2;
            var textOffset = (int)(fontSize + margin) - (margin / 2);
            var shadowOffset = ImageConfig.SHADOW_OFFSET;
            var labelOffset = (int)borderThickness / 2;
            byte textShadowAlpha = ImageConfig.DEFAULT_OPACITY;
            byte labelBoxAlpha = ImageConfig.DEFAULT_OPACITY;

            // Shadow paint
            using var paintShadow = new SKPaint
            {
                //Typeface = font.Typeface,  // 直接使用 SKFont 的 Typeface
                Color = new SKColor(0, 0, 0, textShadowAlpha),
                IsAntialias = true
            };

            // Text paint
            using var paintText = new SKPaint
            {
                //TextSize = fontSize, //ImageConfig.DEFAULT_FONT_SIZE,
                Color = SKColors.White,
                IsAntialias = true
            };

            // Label box background paint
            using var labelBgPaint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                StrokeWidth = borderThickness
            };

            // Bounding box paint
            using var boxPaint = new SKPaint()
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = borderThickness
            };

            using var surface = SKSurface.Create(new SKImageInfo(image.Width, image.Height, SKColorType.Rgba8888));
            using var canvas = surface.Canvas;

            // Draw image on surface
            canvas.DrawImage(image, 0, 0);

            // Draw detections
            var box = detection.BoundingBox;
            var boxColor = HexToRgbaSkia(detection.Label.Color, labelBoxAlpha);
            var labelText = LabelText(currentScale, detection);
            var labelWidth = font.MeasureText(labelText, paintText);

            labelBgPaint.Color = boxColor;
            boxPaint.Color = boxColor;

            // Calculate label background rect size
            var left = box.Left - labelOffset;
            var top = box.Top - labelBoxHeight;
            var right = box.Left + labelWidth + (margin * 2);
            var bottom = box.Top - labelOffset;

            var labelBackground = new SKRectI(left, top, (int)right, bottom);

            // Calculate label text coordinates
            var text_x = labelBackground.Left + margin;
            var text_y = labelBackground.Top + textOffset;

            // Bounding-box
            canvas.DrawRect(box, boxPaint);

            // Label background
            canvas.DrawRect(labelBackground, labelBgPaint);

            // Text shadow
            canvas.DrawText(labelText, text_x + shadowOffset, text_y + shadowOffset, SKTextAlign.Left, font, paintShadow);

            // Label text
            canvas.DrawText(labelText, text_x, text_y, SKTextAlign.Left, font, paintText);

            // Execute all pending draw operations
            canvas.Flush();

            return surface.Snapshot();
        }

        private string LabelText(CalculateScale currentScale, Segmentation detection)
        {
            var result = currentScale == null ?
             $"{detection.SegmentedPixels.Length}{LocalizeHelper.AREA_OF_PIXELS}" :
             $"{detection.SegmentedPixels.Length * Math.Pow(currentScale.Value, 2):F4}{LocalizeHelper.SQUARE_MILLIMETER}";
            return $"{LocalizeHelper.AREA_PREDICTION_TITLE}{result}{LocalizeHelper.AREA_PREDICTION_CONFIDENCE}{detection.Confidence:P2}";
        }

        private static SKColor HexToRgbaSkia(string hexColor, int alpha = 255)
        {
            var hexValid = SKColor.TryParse(hexColor, out _);

            if (hexColor.Length != 7 || hexValid is false)
                throw new ArgumentException("Invalid hexadecimal color format.");

            if (alpha < 0 || alpha > 255)
                throw new ArgumentOutOfRangeException(nameof(alpha), "Alfa value must be between 0-255.");

            byte r = byte.Parse(hexColor.Substring(1, 2), NumberStyles.HexNumber);
            byte g = byte.Parse(hexColor.Substring(3, 2), NumberStyles.HexNumber);
            byte b = byte.Parse(hexColor.Substring(5, 2), NumberStyles.HexNumber);

            return new SKColor(r, g, b, (byte)alpha);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
