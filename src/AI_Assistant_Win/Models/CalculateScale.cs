using SQLite;
using System;

namespace AI_Assistant_Win.Models
{
    [Table("CalculateScales")]
    public class CalculateScale
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int Id { get; set; }
        /// <summary>
        /// which scale
        /// </summary>
        [Column("key")]
        public string Key { get; set; }
        /// <summary>
        /// scale ratio
        /// </summary>
        [Column("value")]
        public float Value { get; set; }
        /// <summary>
        /// 单位名称
        /// </summary>
        [Column("unit")]
        public string Unit { get; set; }
        /// <summary>
        /// 示例渲染图地址
        /// </summary>
        [Column("image_path")]
        public string ImagePath { get; set; }
        /// <summary>
        /// 示例渲染图解析和比例尺设定结果，用于结果保存和界面展示
        /// </summary>
        [Column("settings")]
        public string Settings { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        [Column("creator")]
        public string Creator { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Column("create_time")]
        public DateTime? CreateTime { get; set; }
    }
}
