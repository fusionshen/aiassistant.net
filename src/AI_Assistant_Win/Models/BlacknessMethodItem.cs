using AI_Assistant_Win.Models.Enums;
using SQLite;

namespace AI_Assistant_Win.Models
{
    [Table("BlacknessMethodItems")]
    public class BlacknessMethodItem
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int Id { get; set; }
        /// <summary>
        /// foreign key
        /// <see cref="BlacknessMethodResult"/>
        /// </summary>
        [Column("result_id")]
        public int ResultId { get; set; }
        /// <summary>
        /// 位置
        /// todo: 现场获得比例尺后，应该根据XY位置确定
        /// </summary>
        [Column("location")]
        public BlacknessLocationKind Location { get; set; }
        /// <summary>
        /// 黑度等级
        /// </summary>
        [Column("level")]
        public string Level { get; set; }
        /// <summary>
        /// 置信度
        /// </summary>
        [Column("score")]
        public float Score { get; set; }
        /// <summary>
        /// 宽度，实际指图片高度
        /// todo：现场获取比例尺后，计算出最终mm数
        /// </summary>
        [Column("width")]
        public float Width { get; set; }
        /// <summary>
        /// 识别结果
        /// </summary>
        [Column("prediction")]
        public string Prediction { get; set; }

    }
}
