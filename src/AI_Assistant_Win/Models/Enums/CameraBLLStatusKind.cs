using System.ComponentModel;

namespace AI_Assistant_Win.Models.Enums
{
    public enum CameraBLLStatusKind
    {
        [Description("未能找到可用摄像头。")]
        NoCamera = 0,
        [Description("请设置摄像头进行实时拍摄。")]
        NoCameraSettings = 1,
        [Description("请打开摄像头进行实时拍摄。")]
        NoCameraOpen = 2,
        [Description("请开启采集进行实时拍摄。")]
        NoCameraGrabbing = 3,
        [Description("触发模式。")]
        TriggerMode = 4,
        [Description("实时模式。")]
        ContinuousMode = 5
    }
}
