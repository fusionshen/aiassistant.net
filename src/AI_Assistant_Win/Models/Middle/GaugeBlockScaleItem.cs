using System.Collections.Generic;

namespace AI_Assistant_Win.Models.Middle
{
    public class GaugeBlockScaleItem
    {
        public string TopGraduations { get; set; }
        public string Pixels { get; set; }
        public string ExtractedPixels { get; set; }
        public string AreaLoss { get; set; }
        public QuadrilateralSegmentation Prediction { get; set; }
        public List<QuadrilateralEdge> Edges { get; set; }
        public string DisplayText { get; set; }
    }

    public class QuadrilateralEdge(string edge, string pixelLength, string realLength)
    {
        public string Edge { get; set; } = edge;
        public string PixelLength { get; set; } = pixelLength;
        public string RealLength { get; set; } = realLength;
    }
}
