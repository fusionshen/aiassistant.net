using SQLite;

namespace AI_Assistant_Win.Models
{
    [Table("CircularAreaUploadResults")]
    public class CircularAreaUploadResult : FileUploader
    {
        /// <summary>
        /// foreign key
        /// <see cref="CircularAreaMethodSummary"/>
        /// </summary>
        [Column("summary_id")]
        public int SummaryId { get; set; }
        /// <summary>
        /// 试样编号
        /// </summary>
        [Column("test_no")]
        public string TestNo { get; set; }
        /// <summary>
        /// 钢卷号
        /// </summary>
        [Column("coil_number")]
        public string CoilNumber { get; set; }
        /// <summary>
        /// 第几次
        /// </summary>
        [Column("nth")]
        public int? Nth { get; set; }
    }
}
