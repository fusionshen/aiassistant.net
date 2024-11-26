using AI_Assistant_Win.Models.Enums;
using System.Linq;
using Yolov8.Net;

namespace AI_Assistant_Win.Models.Middle
{
    public class Blackness(BlacknessLocationKind location, Prediction prediction)
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
            get { return $"等级{Level}，宽度：{Prediction.Rectangle.Height:F2}mm"; }
        }
        /// <summary>
        /// 识别结果
        /// </summary>
        public Prediction Prediction { get; set; } = prediction;
    }
}
