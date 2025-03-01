using System;
using System.Linq;
using System.Windows.Forms;

namespace AI_Assistant_Win.Utils
{
    // 安全的钩子实现
    public class UserActivityMessageFilter : IMessageFilter
    {
        public event EventHandler ActivityDetected;

        private readonly int[] watchMessages =
        {
            0x0200, // WM_MOUSEMOVE
            0x00A0, // WM_NCMOUSEMOVE
            0x0100, // WM_KEYDOWN
            0x0104, // WM_SYSKEYDOWN
            0x0201, // WM_LBUTTONDOWN
            0x0204, // WM_RBUTTONDOWN
            0x0207, // WM_MBUTTONDOWN
            0x020A, // WM_MOUSEWHEEL
            0x0112  // WM_SYSCOMMAND (例如最大化/最小化)
        };

        public bool PreFilterMessage(ref Message m)
        {
            if (watchMessages.Contains(m.Msg))
            {
                ActivityDetected?.Invoke(this, EventArgs.Empty);
            }
            return false;
        }
    }
}