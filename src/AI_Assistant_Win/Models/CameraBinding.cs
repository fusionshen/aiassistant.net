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
        /// 采集状态下可以是连续模式和触发模式，触发模式只支持software软触发，TODO:针脚line0暂不支持。
        /// 唯一的区别是连续模式，画面是动态的，触发模式只支持一次触发，即看不到实时画面，但是点击按钮后会拍摄当前照片。
        /// TODO：需要增加按钮重新在页面开启采集模式。
        /// </summary>
        [Column("is_grabbing")]
        public bool IsGrabbing { get; set; }
        /// <summary>
        /// 是否是触发模式，现只支持软触发。
        /// </summary>
        [Column("is_trigger_mode")]
        public bool IsTriggerMode { get; set; }
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
