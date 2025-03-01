using System.Windows.Forms;

namespace AI_Assistant_Win.Utils
{
    // 扩展方法简化资源释放
    public static class DisposableExtensions
    {
        public static void SafeDispose(this Timer timer)
        {
            if (timer == null) return;

            try
            {
                timer.Stop();
                timer.Tick -= null; // 清空所有事件绑定
                timer.Dispose();
            }
            catch { /* 忽略释放异常 */ }
        }
    }
}
