using System.Drawing;

namespace AI_Assistant_Win.Models.Middle
{
    public class QuadrilateralSegmentation : SimpleSegmentation
    {
        public Quadrilateral Quadrilateral { get; set; }
    }

    public class Quadrilateral(PointF topLeft, PointF topRight, PointF bottomLeft, PointF bottomRight)
    {
        public PointF TopLeft { get; set; } = topLeft;
        public PointF TopRight { get; set; } = topRight;
        public PointF BottomLeft { get; set; } = bottomLeft;
        public PointF BottomRight { get; set; } = bottomRight;
    }
}
