using SQLite;
using System;

namespace AI_Assistant_Win.Models
{
    [Table("CircularAreaMethodSummarys")]
    public class CircularAreaMethodSummary
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int Id { get; set; }
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
        /// 是否外部
        /// </summary>
        [Column("is_external")]
        public bool IsExternal { get; set; }
        /// <summary>
        /// 第几次试验
        /// </summary>
        [Column("nth")]
        public int? Nth { get; set; } = 1;
        /// <summary>
        /// 是否上传至业务系统
        /// </summary>
        [Column("is_uploaded")]
        public bool IsUploaded { get; set; }
        /// <summary>
        /// 上传人
        /// </summary>
        [Column("uploader")]
        public string Uploader { get; set; }
        /// <summary>
        /// 上传时间
        /// </summary>
        [Column("upload_time")]
        public DateTime? UploadTime { get; set; }
        /// <summary>
        /// 新建人员
        /// </summary>
        [Column("creator")]
        public string Creator { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Column("create_time")]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 最近修改人
        /// </summary>
        [Column("last_reviser")]
        public string LastReviser { get; set; }
        /// <summary>
        /// 最近修改时间
        /// </summary>
        [Column("last_modified_time")]
        public DateTime? LastModifiedTime { get; set; }
    }
}
