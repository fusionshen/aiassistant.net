using AI_Assistant_Win.Models.Enums;
using SQLite;
using System;

namespace AI_Assistant_Win.Models
{
    [Table("CircularAreaMethodResults")]
    public class CircularAreaMethodResult
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int Id { get; set; }
        /// <summary>
        /// 原图地址
        /// </summary>
        [Column("origin_image_path")]
        public string OriginImagePath { get; set; }
        /// <summary>
        /// 渲染图地址
        /// </summary>
        [Column("render_image_path")]
        public string RenderImagePath { get; set; }
        /// <summary>
        /// 比例尺
        /// </summary>
        [Column("scale_id")]
        public int? ScaleId { get; set; }
        #region 识别结果
        /// <summary>
        /// 像素面积，像素数量
        /// </summary>
        [Column("pixels")]
        public int Pixels { get; set; }
        /// <summary>
        /// 置信度
        /// </summary>
        [Column("confidence")]
        public double Confidence { get; set; }
        /// <summary>
        /// 测算面积(平方毫米)
        /// </summary>
        [Column("area")]
        public float Area { get; set; }
        /// <summary>
        /// 类圆直径(毫米)
        /// </summary>
        [Column("diameter")]
        public float Diameter { get; set; }
        /// <summary>
        /// 识别结果
        /// </summary>
        [Column("prediction")]
        public string Prediction { get; set; }
        #endregion
        /// <summary>
        /// 班组
        /// </summary>
        [Column("work_group")]
        public string WorkGroup { get; set; }
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
        /// 位置
        /// </summary>
        [Column("position")]
        public CircularPositionKind Position { get; set; }
        /// <summary>
        /// 分析人-新建人员
        /// </summary>
        [Column("analyst")]
        public string Analyst { get; set; }
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
