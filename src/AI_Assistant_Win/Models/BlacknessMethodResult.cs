using SQLite;
using System;

namespace AI_Assistant_Win.Models
{
    [Table("BlacknessMethodResults")]
    public class BlacknessMethodResult
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int Id { get; set; }
        /// <summary>
        /// 比例尺
        /// </summary>
        [Column("scale_id")]
        public int? ScaleId { get; set; }
        /// <summary>
        /// 钢卷号
        /// </summary>
        [Column("coil_number")]
        public string CoilNumber { get; set; }
        /// <summary>
        /// 尺寸
        /// </summary>
        [Column("size")]
        public string Size { get; set; }
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
        /// 总体结果
        /// </summary>
        [Column("is_ok")]
        public bool IsOK { get; set; }
        #region 结果快速预览
        /// <summary>
        /// 表面OP侧等级
        /// </summary>
        [Column("surface_op_level")]
        public string SurfaceOPLevel { get; set; }
        /// <summary>
        /// 表面OP侧宽度
        /// </summary>
        [Column("surface_op_width")]
        public float SurfaceOPWidth { get; set; }
        /// <summary>
        /// 表面OP侧置信度
        /// </summary>
        [Column("surface_op_score")]
        public float SurfaceOPScore { get; set; }
        /// <summary>
        /// 表面CE侧等级
        /// </summary>
        [Column("surface_ce_level")]
        public string SurfaceCELevel { get; set; }
        /// <summary>
        /// 表面CE侧宽度
        /// </summary>
        [Column("surface_ce_width")]
        public float? SurfaceCEWidth { get; set; }
        /// <summary>
        /// 表面CE侧置信度
        /// </summary>
        [Column("surface_ce_score")]
        public float? SurfaceCEScore { get; set; }
        /// <summary>
        /// 表面DR侧等级
        /// </summary>
        [Column("surface_dr_level")]
        public string SurfaceDRLevel { get; set; }
        /// <summary>
        /// 表面DR侧宽度
        /// </summary>
        [Column("surface_dr_width")]
        public float SurfaceDRWidth { get; set; }
        /// <summary>
        /// 表面DR侧置信度
        /// </summary>
        [Column("surface_dr_score")]
        public float SurfaceDRScore { get; set; }
        /// <summary>
        /// 里面OP侧等级
        /// </summary>
        [Column("inside_op_level")]
        public string InsideOPLevel { get; set; }
        /// <summary>
        /// 里面OP侧宽度
        /// </summary>
        [Column("inside_op_width")]
        public float InsideOPWidth { get; set; }
        /// <summary>
        /// 里面OP侧置信度
        /// </summary>
        [Column("inside_op_score")]
        public float InsideOPScore { get; set; }
        /// <summary>
        /// 里面CE侧等级
        /// </summary>
        [Column("inside_ce_level")]
        public string InsideCELevel { get; set; }
        /// <summary>
        /// 里面CE侧宽度
        /// </summary>
        [Column("inside_ce_width")]
        public float InsideCEWidth { get; set; }
        /// <summary>
        /// 里面CE侧置信度
        /// </summary>
        [Column("inside_ce_score")]
        public float InsideCEScore { get; set; }
        /// <summary>
        /// 里面DR侧等级
        /// </summary>
        [Column("inside_dr_level")]
        public string InsideDRLevel { get; set; }
        /// <summary>
        /// 里面DR侧宽度
        /// </summary>
        [Column("inside_dr_width")]
        public float InsideDRWidth { get; set; }
        /// <summary>
        /// 里面DR侧置信度
        /// </summary>
        [Column("inside_dr_score")]
        public float InsideDRScore { get; set; }
        #endregion
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
