namespace AI_Assistant_WinForms.Models
{
    public class BoundingBoxDimensions
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Height { get; set; }
        public float Width { get; set; }
    }

    public class BoundingBox
    {
        public BoundingBoxDimensions Dimensions { get; set; }

        public string Label { get; set; }

        public float Confidence { get; set; }

        public RectangleF Rect
        {
            get { return new RectangleF(Dimensions.X, Dimensions.Y, Dimensions.Width, Dimensions.Height); }
        }

        public Color BoxColor { get; set; }

        public string Description => $"{Label} ({Confidence * 100:0}%)";

        private static readonly Color[] classColors = new Color[]
        {
            Color.Khaki, Color.Fuchsia, Color.Silver, Color.RoyalBlue,
            Color.Green, Color.DarkOrange, Color.Purple, Color.Gold,
            Color.Red, Color.Aquamarine, Color.Lime, Color.AliceBlue,
            Color.Sienna, Color.Orchid, Color.Tan, Color.LightPink,
            Color.Yellow, Color.HotPink, Color.OliveDrab, Color.SandyBrown,
            Color.DarkTurquoise
        };

        public static Color GetColor(int index) => index < classColors.Length ? classColors[index] : classColors[index % classColors.Length];

        // 计算IoU（交并比）的辅助方法
        public float IoU(BoundingBox other)
        {
            float interX1 = Math.Max(Dimensions.X, other.Dimensions.X);
            float interY1 = Math.Max(Dimensions.Y, other.Dimensions.Y);
            float interX2 = Math.Min(Dimensions.X + Dimensions.Width, other.Dimensions.X + other.Dimensions.Width);
            float interY2 = Math.Min(Dimensions.Y + Dimensions.Height, other.Dimensions.Y + other.Dimensions.Height);

            float interWidth = Math.Max(0, interX2 - interX1);
            float interHeight = Math.Max(0, interY2 - interY1);

            float interArea = interWidth * interHeight;

            float box1Area = Dimensions.Width * Dimensions.Height;
            float box2Area = other.Dimensions.Width * other.Dimensions.Height;

            float iou = interArea / (box1Area + box2Area - interArea);

            return iou;
        }
    }


    public class NonMaximumSuppression
    {
        public static List<BoundingBox> Apply(List<BoundingBox> boxes, float iouThreshold = 0.7f)
        {
            List<BoundingBox> result = new List<BoundingBox>();

            // 根据置信度对边界框进行排序
            var sortedBoxes = boxes.OrderByDescending(b => b.Confidence).ToList();

            foreach (var box in sortedBoxes)
            {
                bool keep = true;

                // 检查与已保留的边界框的重叠情况
                foreach (var resultBox in result)
                {
                    if (box.IoU(resultBox) > iouThreshold)
                    {
                        keep = false;
                        break;
                    }
                }

                if (keep)
                {
                    result.Add(box);
                }
            }

            return result;
        }
    }
}
