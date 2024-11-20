using SixLabors.ImageSharp;

namespace Yolov8.Net
{
    public class Prediction
    {
        public Label? Label { get; init; }
        public RectangleF Rectangle { get; init; }
        public float Score { get; init; }
    }
}
