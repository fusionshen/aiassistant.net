using AI_Assistant_Win.Utils;
using System;

namespace AI_Assistant_Win.Models.Middle
{
    public class CircularArea(SimpleSegmentation prediction, CalculateScale scale = null) : IEquatable<CircularArea>
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
        public float CalculatedArea { get => CalculateScale == null ? Prediction.SegmentedPixelsCount : Prediction.SegmentedPixelsCount * CalculateScale.Value / 100; }
        /// <summary>
        /// 类圆直径
        /// </summary>
        public float Diameter { get => (float)(CalculateScale == null ? 2 * Math.Sqrt(AreaOfPixels / Math.PI) : 2 * Math.Sqrt(CalculatedArea / Math.PI)); }
        /// <summary>
        /// 表述，用于在结果判定区显示
        /// </summary>
        public string Description
        {
            get => $"{LocalizeHelper.AREA_TITLE}{CalculatedArea:F2}{Unit}{LocalizeHelper.CIRCULAR_DIAMETER_TITLE}{Diameter:F2}{DiameterUnit}";
        }

        public string DiameterUnit
        {
            get => CalculateScale == null ? LocalizeHelper.PIXEL : LocalizeHelper.MILLIMETER;
        }

        public string Unit
        {
            get => CalculateScale == null ? LocalizeHelper.AREA_OF_PIXELS : LocalizeHelper.SQUARE_MILLIMETER;
        }

        /// <summary>
        /// 识别结果
        /// </summary>
        public SimpleSegmentation Prediction { get; set; } = prediction;

        public bool Equals(CircularArea other)
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
            return Equals(obj as CircularArea);
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
