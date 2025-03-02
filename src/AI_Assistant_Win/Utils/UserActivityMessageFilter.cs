using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AI_Assistant_Win.Utils
{
    public class UserActivityMessageFilter : IMessageFilter
    {
        private const int MouseMovementThreshold = 2; // 像素
        private DateTime lastRealActivity = DateTime.MinValue;
        private Point lastMousePos = Point.Empty;

        private static readonly int[] ValidMessages =
        {
            0x0201, // WM_LBUTTONDOWN
            0x0204, // WM_RBUTTONDOWN
            0x0207, // WM_MBUTTONDOWN
            0x0100, // WM_KEYDOWN
            0x0104  // WM_SYSKEYDOWN (Alt组合键)
        };

        public event EventHandler RealUserActivity;

        public bool PreFilterMessage(ref Message m)
        {
            if (ValidMessages.Contains(m.Msg))
            {
                if (IsRealUserInput())
                {
                    TriggerRealActivity();
                }
            }
            else if (m.Msg == 0x0200) // WM_MOUSEMOVE
            {
                HandleMouseMove(m.LParam);
            }
            return false;
        }

        private void HandleMouseMove(IntPtr lParam)
        {
            var newPos = new Point(
                (int)(ushort)lParam.ToInt32(),
                (int)(ushort)((uint)lParam.ToInt32() >> 16));

            if (lastMousePos == Point.Empty)
            {
                lastMousePos = newPos;
                return;
            }

            if (Math.Abs(newPos.X - lastMousePos.X) > MouseMovementThreshold ||
                Math.Abs(newPos.Y - lastMousePos.Y) > MouseMovementThreshold)
            {
                if (IsRealUserInput())
                {
                    TriggerRealActivity();
                }
            }
            lastMousePos = newPos;
        }

        private bool IsRealUserInput()
        {
            // 排除系统生成的输入
            return (DateTime.Now - lastRealActivity).TotalMilliseconds > 500 &&
                   (Control.ModifierKeys & Keys.Alt) == 0 &&
                   GetForegroundWindow() == Process.GetCurrentProcess().MainWindowHandle;
        }

        private void TriggerRealActivity()
        {
            lastRealActivity = DateTime.Now;
            RealUserActivity?.Invoke(this, EventArgs.Empty);
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();
    }
}