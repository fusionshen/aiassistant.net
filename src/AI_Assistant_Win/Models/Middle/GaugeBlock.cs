using AI_Assistant_Win.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AI_Assistant_Win.Models.Middle
{
    public class GaugeBlock(QuadrilateralSegmentation prediction, CalculateScale scale = null) : IEquatable<GaugeBlock>
    {
        /// <summary>
        /// 比例尺
        /// </summary>
        public CalculateScale CalculateScale { get; set; } = scale;
        /// <summary>
        /// 置信度
        /// </summary>
        public double Confidence { get => Prediction.Confidence; }
        /// <summary>
        /// 像素面积，可以页面上可以更改比例尺重新计算，原始值不能变化
        /// </summary>
        public long AreaOfPixels { get => Prediction.SegmentedPixelsCount; }
        /// <summary>
        /// 计算后的面积，用于保存数据库和history、report展示
        /// </summary>
        public float CalculatedArea { get => (float)(CalculateScale == null ? Prediction.SegmentedPixelsCount : Prediction.SegmentedPixelsCount * Math.Pow(CalculateScale.Value, 2)); }
        /// <summary>
        /// 截取四边形像素面积
        /// </summary>
        public float ExtractedAreaOfPixels
        {
            get => (float)ShapeHelper.CalculateQuadrilateralArea(Prediction.Quadrilateral.TopLeft,
            Prediction.Quadrilateral.TopRight, Prediction.Quadrilateral.BottomRight, Prediction.Quadrilateral.BottomLeft);
        }
        /// <summary>
        /// 点坐标展示
        /// </summary>
        public string VertexText
        {
            get => $"({Prediction.Quadrilateral.TopLeft.X},{Prediction.Quadrilateral.TopLeft.Y})({Prediction.Quadrilateral.TopRight.X},{Prediction.Quadrilateral.TopRight.Y})({Prediction.Quadrilateral.BottomRight.X},{Prediction.Quadrilateral.BottomRight.Y})({Prediction.Quadrilateral.BottomLeft.X},{Prediction.Quadrilateral.BottomLeft.Y})";
        }
        /// <summary>
        /// pixels of each edge
        /// </summary>
        public Dictionary<string, float> EdgePixels
        {
            get => new()
            {
                {"AB", ShapeHelper.CalculateDistance(Prediction.Quadrilateral.TopLeft, Prediction.Quadrilateral.TopRight)},
                {"BC", ShapeHelper.CalculateDistance(Prediction.Quadrilateral.TopRight, Prediction.Quadrilateral.BottomRight)},
                {"CD", ShapeHelper.CalculateDistance(Prediction.Quadrilateral.BottomRight, Prediction.Quadrilateral.BottomLeft)},
                {"DA", ShapeHelper.CalculateDistance(Prediction.Quadrilateral.BottomLeft, Prediction.Quadrilateral.TopLeft)}
            };
        }

        public Dictionary<string, float> CalculatedEdgeLengths
        {
            get => CalculateScale == null ? EdgePixels : EdgePixels.ToDictionary(pair => pair.Key, pair => pair.Value * CalculateScale.Value);
        }
        /// <summary>
        /// 表述，用于在结果判定区显示
        /// </summary>
        public string Description
        {
            get => $"{LocalizeHelper.AREA_TITLE}{CalculatedArea:F2}{AreaUnit}";
        }

        public string AreaUnit
        {
            get => CalculateScale == null ? LocalizeHelper.AREA_OF_PIXELS : LocalizeHelper.SQUARE_MILLIMETER;
        }

        public string LengthUnit
        {
            get => CalculateScale == null ? string.Empty : LocalizeHelper.MILLIMETER;
        }

        /// <summary>
        /// 识别结果
        /// </summary>
        public QuadrilateralSegmentation Prediction { get; set; } = prediction;

        public bool Equals(GaugeBlock other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return Confidence == other.Confidence &&
                AreaOfPixels == other.AreaOfPixels &&
                CalculateScale != null && CalculateScale.Equals(other.CalculateScale);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals(obj as GaugeBlock);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return Prediction != null ? Prediction.GetHashCode() : 0; // 根据你的字段定义哈希码计算逻辑
            }
        }
    }
}
