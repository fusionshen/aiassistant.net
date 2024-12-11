using System;

namespace AI_Assistant_Win.Utils
{
    public class CameraSDKException : Exception
    {
        // 额外的属性（可选）
        public int ErrorCode { get; }

        // 默认构造函数
        public CameraSDKException() : base()
        {
        }

        // 带有错误消息的构造函数
        public CameraSDKException(string message) : base(message)
        {
        }

        // 带有错误消息和内部异常的构造函数
        public CameraSDKException(string message, Exception inner) : base(message, inner)
        {
        }

        // 带有错误消息和错误代码的构造函数
        public CameraSDKException(string message, int errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }

        // 带有错误消息、内部异常和错误代码的构造函数
        public CameraSDKException(string message, Exception inner, int errorCode) : base(message, inner)
        {
            ErrorCode = errorCode;
        }
    }
}
