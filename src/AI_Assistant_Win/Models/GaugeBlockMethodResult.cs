using SQLite;
using System;

namespace AI_Assistant_Win.Models
{
    [Table("GaugeBlockMethodResults")]
    public class GaugeBlockMethodResult
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
        /// 置信度
        /// </summary>
        [Column("confidence")]
        public double Confidence { get; set; }
        /// <summary>
        /// 像素面积，像素数量
        /// </summary>
        [Column("pixels")]
        public long Pixels { get; set; }
        /// <summary>
        /// 测算面积(平方毫米)
        /// </summary>
        [Column("area")]
        public double Area { get; set; }
        /// <summary>
        /// 顶点坐标描述
        /// </summary>
        [Column("vertex_positions")]
        public string VertexPositions { get; set; }
        /// <summary>
        /// 像素边长描述
        /// </summary>
        [Column("edge_pixels")]
        public string EdgePixels { get; set; }
        /// <summary>
        /// 计算后的边长描述
        /// </summary>
        [Column("calculatd_edges")]
        public string CalculatdEdges { get; set; }
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
        /// 分析人-新建人员
        /// </summary>
        [Column("analyst")]
        public string Analyst { get; set; }
        /// <summary>
        /// 真实输入边
        /// </summary>
        [Column("input_edge")]
        public string InputEdge { get; set; }
        /// <summary>
        /// 计算边长
        /// </summary>
        [Column("calculated_length")]
        public double CalculatedLength { get; set; }
        /// <summary>
        /// 真实输入边长
        /// </summary>
        [Column("input_length")]
        public double InputLength { get; set; }
        /// <summary>
        /// 长度准确度描述
        /// </summary>
        [Column("length_accuracy")]
        public string LengthAccuracy { get; set; }
        /// <summary>
        /// 面积准确度描述
        /// </summary>
        [Column("area_accuracy")]
        public string AreaAccuracy { get; set; }
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
