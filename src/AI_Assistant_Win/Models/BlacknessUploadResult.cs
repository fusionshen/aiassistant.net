using SQLite;
using System;

namespace AI_Assistant_Win.Models
{
    [Table("BlacknessUploadResults")]
    public class BlacknessUploadResult
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
        /// 上传人
        /// </summary>
        [Column("uploader")]
        public string Uploader { get; set; }
        /// <summary>
        /// 本地开始的上传时间：实体构造时的时间
        /// </summary>
        [Column("create_time")]
        public DateTime CreateTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 本地文件地址
        /// </summary>
        [Column("local_file_path")]
        public string LocalFilePath { get; set; }
        #region 
        [Column("file_manager_id")]
        public int FileManagerId { get; set; } = 0;

        [Column("file_name")]
        public string FileName { get; set; }

        [Column("file_category_id")]
        public int FileCategoryId { get; set; }

        [Column("file_category")]
        public string FileCategory { get; set; }

        [Column("file_version_id")]
        public int FileVersionId { get; set; }

        [Column("file_version")]
        public string FileVersion { get; set; }

        [Column("upload_file_id")]
        public string UploadFileId { get; set; }
        /// <summary>
        /// 系统返回的上传时间，只会返回FileManager创建时间
        /// </summary>
        [Column("upload_time")]
        public DateTime? UploadTime { get; set; }
        #endregion
    }
}
