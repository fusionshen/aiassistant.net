using SQLite;
using System;

namespace AI_Assistant_Win.Models
{
    [Table("CameraBindings")]
    public class CameraBinding
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int Id { get; set; }
        /// <summary>
        /// 应用
        /// </summary>
        [Column("application")]
        public string Application { get; set; }
        /// <summary>
        /// 设备序列号
        /// </summary>
        [Column("serial_number")]
        public string SerialNumber { get; set; }
        /// <summary>
        /// 设备信息
        /// </summary>
        [Column("device_info")]
        public string DeviceInfo { get; set; }
        /// <summary>
        /// 设备是否打开
        /// </summary>
        [Column("is_open")]
        public bool IsOpen { get; set; }
        /// <summary>
        /// 设备是否正在采集
        /// 采集状态下只要接入设备，就一直是连续模式采集图片，点了拍摄按钮之后，其实是触动了一次软触发模式。
        /// 可以设置成停止采集，这样可以释放资源不让摄像头继续被占用
        /// TODO：需要增加按钮重新在页面开启连续模式
        /// </summary>
        [Column("is_grabbing")]
        public string IsGrabbing { get; set; }
        #region 参数设置，仅供查询纠错
        /// <summary>
        /// 曝光时间
        /// </summary>
        [Column("exposure_time")]
        public float ExposureTime { get; set; }
        /// <summary>
        /// 增益
        /// </summary>
        [Column("gain")]
        public float Gain { get; set; }
        /// <summary>
        /// 帧率
        /// </summary>
        [Column("resulting_frame_rate")]
        public float ResultingFrameRate { get; set; }
        /// <summary>
        /// 像素格式
        /// </summary>
        [Column("pixel_format")]
        public string PixelFormat { get; set; }
        #endregion
        /// <summary>
        /// 创建时间
        /// </summary>
        [Column("create_time")]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 最近修改时间
        /// </summary>
        [Column("last_modified_time")]
        public DateTime? LastModifiedTime { get; set; }
    }
}
