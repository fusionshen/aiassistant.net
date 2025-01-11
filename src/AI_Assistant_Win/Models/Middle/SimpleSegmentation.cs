using SkiaSharp;
using YoloDotNet.Models;

namespace AI_Assistant_Win.Models.Middle
{
    public class SimpleSegmentation
    {
        /// <summary>
        /// Label information associated with the detected object.
        /// </summary>
        public LabelModel Label { get; init; } = new();

        /// <summary>
        /// Confidence score of the detected object.
        /// </summary>
        public double Confidence { get; init; }

        /// <summary>
        /// Rectangle defining the region of interest (bounding box) of the detected object.
        /// </summary>
        public SKRectI BoundingBox { get; init; }

        /// <summary>
        /// count of Segmentated pixels (x,y) with the pixel confidence value
        /// </summary>
        public long SegmentedPixelsCount { get; set; }
    }
}
