using AI_Assistant_Win.Models.Enums;
using AI_Assistant_Win.Utils;
using System;
using System.Linq;
using Yolov8.Net;

namespace AI_Assistant_Win.Models.Middle
{
    public class Blackness(BlacknessLocationKind location, Prediction prediction) : IEquatable<Blackness>
    {
        /// <summary>
        /// 位置
        /// todo: 现场获得比例尺后，应该根据XY位置确定
        /// </summary>
        public BlacknessLocationKind Location { get; set; } = location;
        /// <summary>
        /// 黑度
        /// </summary>
        public string Level { get { return new(Prediction.Label.Name.Where(char.IsDigit).ToArray()); } }
        /// <summary>
        /// 置信度
        /// </summary>
        public float Score { get { return Prediction.Score; } }
        /// <summary>
        /// 宽度，实际指图片高度
        /// todo：现场获取比例尺后，计算出最终mm数
        /// </summary>
        public float Width { get { return Prediction.Rectangle.Height; } }
        /// <summary>
        /// 表述，用于在结果判定区显示
        /// </summary>
        public string Description
        {
            get { return $"{LocalizeHelper.LEVEL}{Level}{LocalizeHelper.BLACKNESS_WITH}{Prediction.Rectangle.Height:F2}{LocalizeHelper.MILLIMETER}"; }
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
                Width == other.Width;
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
