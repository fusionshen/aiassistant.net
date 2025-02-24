using SQLite;
using System;

namespace AI_Assistant_Win.Models
{
    [Table("CalculateScales")]
    public class CalculateScale : IEquatable<CalculateScale>
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
        public double Value { get; set; }
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

        public bool Equals(CalculateScale other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id ||
                (Key == other.Key && Value == other.Value && ImagePath == other.ImagePath && Settings == other.Settings);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals(obj as CalculateScale);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return Settings != null ? Settings.GetHashCode() : 0; // 根据你的字段定义哈希码计算逻辑
            }
        }
    }
}
