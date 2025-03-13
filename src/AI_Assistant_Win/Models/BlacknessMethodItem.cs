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
        public double? Score { get; set; }
        /// <summary>
        /// 计算过后的宽度，用于保存数据库和history、report展示
        /// </summary>
        [Column("width")]
        public double? Width { get; set; }
        /// <summary>
        /// 识别结果
        /// </summary>
        [Column("prediction")]
        public string Prediction { get; set; }

    }
}
