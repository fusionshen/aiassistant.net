namespace AI_Assistant_Win.Models.Middle
{
    public class CameraGrabbing()
    {
        /// <summary>
        /// 应用名称
        /// </summary>
        public string Application { get; set; }
        /// <summary>
        /// 用于摄像头实时渲染的句柄
        /// </summary>
        public nint ImageHandle { get; set; }
    }
}
