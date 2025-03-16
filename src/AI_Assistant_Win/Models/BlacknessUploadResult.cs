using SQLite;

namespace AI_Assistant_Win.Models
{
    [Table("BlacknessUploadResults")]
    public class BlacknessUploadResult : FileUploader
    {
        /// <summary>
        /// foreign key
        /// <see cref="BlacknessMethodResult"/>
        /// </summary>
        [Column("result_id")]
        public int ResultId { get; set; }
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
