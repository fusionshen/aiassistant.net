using SQLite;
using System;

namespace AI_Assistant_Win.Models
{
    [Table("ScaleAccuracyTracers")]
    public class ScaleAccuracyTracer
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
        /// 测量长度
        /// </summary>
        [Column("measured_length")]
        public double MeasuredLength { get; set; }
        /// <summary>
        /// 系统误差也叫最大允许误差（MPE, Maximum Permissible Error）
        /// MPE的计算通常基于以下三种途径：
        /// 1. 校准法（直接对比标准器）
        /// 步骤：
        ///     1.使用高精度标准器（如标准量块、激光干涉仪）对被测仪器进行校准。
        ///     2.在仪器全量程范围内选取多个校准点（如0%、50%、100%量程）。
        ///     3.记录仪器示值L示与标准器参考值L标准。
        ///     4.计算每个校准点的误差：误差=L示−L标准
        ///     5.取所有误差绝对值的最大值作为MPE：MPE=max(∣误差1∣,∣误差2∣,...,∣误差n∣)
        ///     示例：
        ///     某千分尺在5个校准点的误差分别为+0.001mm、-0.002mm、+0.003mm、+0.001mm、-0.001mm，则其MPE为 0.003mm。
        ///  2. 统计分析法（基于多次测量数据）
        ///  3. 标准规范法（遵循国际/行业标准）
        /// </summary>
        [Column("mpe")]
        public double MPE { get; set; }
        /// <summary>
        /// 平均值
        /// </summary>
        [Column("average")]
        public double Average { get; set; }
        /// <summary>
        /// 随机误差(标准差σ)random error(standard deviation)
        /// Random error: 指的是在实验或测量中由于各种不可控的因素所产生的误差，这些误差是无法避免的，并且其分布通常是均匀的。
        /// Standard deviation: 是统计学中的一个常用术语，表示一组数据的离散程度或波动大小。标准差越大，表示数据点离均值的距离越远，越不稳定；反之，标准差越小，数据越集中
        /// </summary>
        [Column("standard_deviation")]
        public double StandardDeviation { get; set; }
        /// <summary>
        /// 平均值的标准不确定度
        /// </summary>
        [Column("standard_error")]
        public double StandardError { get; set; }
        /// <summary>
        /// 总不确定度（Uncertainty）
        /// </summary>
        [Column("uncertainty")]
        public double Uncertainty { get; set; }
        /// <summary>
        /// 理论68.27% 置信度 
        /// </summary>
        [Column("pct_1_sigma")]
        public double Pct1Sigma { get; set; }
        /// <summary>
        /// 理论95.45% 置信度
        /// 95% 置信度表示：
        /// 如果重复测量无限次，约有 95% 的测量值会落在μ±2σ 范围内；
        /// 仅有 5% 的测量值会超出此范围（小概率事件）。
        /// </summary>
        [Column("pct_2_sigma")]
        public double Pct2Sigma { get; set; }
        /// <summary>
        /// 理论99.73% 置信度
        /// </summary>
        [Column("pct_3_sigma")]
        public double Pct3Sigma { get; set; }
        /// <summary>
        /// 完整表示L=10.0016mm±0.003mm(k=2,95%)
        /// 在统计学中，k⋅σ 中的k 值（通常称为置信因子或覆盖因子）与置信度（即数据落在某个范围内的概率）直接相关
        /// 正态分布与标准差
        /// 正态分布（钟形曲线）
        /// 许多自然现象（包括测量误差）的分布近似服从正态分布。其特点是：
        /// 均值（μ）为分布中心。
        /// 标准差（σ）描述数据的分散程度（σ越大，数据越分散）。
        /// 分布形状对称，且满足68-95-99.7经验法则：
        /// 68% 的数据落在 μ±1σ 内；
        /// 95% 的数据落在 μ±2σ 内；
        /// 99.7% 的数据落在 μ±3σ 内。
        /// </summary>
        [Column("display_name")]
        public string DisplayName { get; set; }
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
