using AI_Assistant_Win.Models.Enums;
using AI_Assistant_Win.Utils;
using System;
using System.Linq;
using Yolov8.Net;

namespace AI_Assistant_Win.Models.Middle
{
    public class Blackness(BlacknessLocationKind location, Prediction prediction, CalculateScale scale = null) : IEquatable<Blackness>
    {
        /// <summary>
        /// 比例尺
        /// </summary>
        public CalculateScale CalculateScale { get; set; } = scale;
        /// <summary>
        /// 位置
        /// </summary>
        public BlacknessLocationKind Location { get; set; } = location;
        /// <summary>
        /// 黑度
        /// </summary>
        public string Level { get => new(Prediction.Label.Name.Where(char.IsDigit).ToArray()); }
        /// <summary>
        /// 置信度
        /// </summary>
        public float Score { get => Prediction.Score; }
        /// <summary>
        /// 宽度，实际指图片像素高度，可以页面上可以更改比例尺重新计算，原始的像素值不能变化
        /// </summary>
        public float Width { get => Prediction.Rectangle.Height; }
        /// <summary>
        /// 计算过后的宽度，用于保存数据库和history、report展示
        /// </summary>
        public double CalculatedWidth { get => CalculateScale == null ? Prediction.Rectangle.Height : Prediction.Rectangle.Height * CalculateScale.Value; }
        /// <summary>
        /// 表述，用于在结果判定区显示
        /// </summary>
        public string Description
        {
            get => $"{LocalizeHelper.LEVEL}{Level}{LocalizeHelper.BLACKNESS_WITH}{CalculatedWidth:F2}{Unit}";
        }

        private string Unit
        {
            get => CalculateScale == null ? LocalizeHelper.PIXEL : LocalizeHelper.MILLIMETER;
        }

        /// <summary>
        /// 识别结果
        /// </summary>
        public Prediction Prediction { get; set; } = prediction;

        public bool Equals(Blackness other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return Location == other.Location &&
                Level == other.Level &&
                Score == other.Score &&
                Width == other.Width &&
                CalculateScale != null && CalculateScale.Equals(other.CalculateScale);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals(obj as Blackness);
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
