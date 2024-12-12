using AI_Assistant_Win.Utils;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace AI_Assistant_Win
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] arge)
        {
            ComWrappers.RegisterForMarshalling(WinFormsComInterop.WinFormsComWrappers.Instance);
            var command = string.Join(" ", arge);
            AntdUI.Localization.DefaultLanguage = "zh-CN";
            var lang = AntdUI.Localization.CurrentLanguage;
            if (lang.StartsWith("en")) AntdUI.Localization.Provider = new Localizer();
            AntdUI.Config.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.SetCompatibleTextRenderingDefault(false);
            AntdUI.Config.SetCorrectionTextRendering("Microsoft YaHei UI");
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += Application_ThreadException; // 处理托管线程中的未处理异常。
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException; // 处理非托管线程中的未处理异常
            // 对于用户通过任务管理器结束进程的情况，由于任务管理器调用的是Windows API ExitProcess来终止进程，因此FormClosing和FormClosed事件不会被触发。
            Application.ApplicationExit += new EventHandler(OnApplicationExit);
            if (command == "m") Application.Run(new Main());
            else if (command == "color") Application.Run(new Colors());
            else if (command == "overview") Application.Run(new Overview(false));
            else Application.Run(new MainWindow(command == "t"));
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            HandleException((Exception)e.ExceptionObject);
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            HandleException(e.Exception);
        }

        private static void HandleException(Exception ex)
        {
            try
            {
                // 记录错误日志
                string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "error.log");
                File.AppendAllText(logPath, $"{DateTime.Now}: {ex}{Environment.NewLine}");

                // 显示提示框
                //MessageBox.Show($"发生错误，请联系管理员。错误已记录到{logPath}。", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AntdUI.Modal.open(new AntdUI.Modal.Config("错误提示", $"发生错误，请联系管理员。错误已记录到{logPath}。", AntdUI.TType.Error)
                {
                    OnButtonStyle = (id, btn) =>
                    {
                        btn.BackExtend = "135, #6253E1, #04BEFE";
                    },
                    CancelText = null,
                    OkText = "确认"
                });

                // 优雅地关闭应用程序
                Application.Exit();
            }
            catch (Exception)
            {
                // 如果记录日志或显示提示框时发生异常，则尽量确保应用程序能够退出
                Environment.Exit(1);
            }
        }

        /// <summary>
        /// 崩溃时被捕获后，或者主动关闭时，需要关闭摄像头占用，避免短时间内再次使用时报摄像头没有权限。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnApplicationExit(object sender, EventArgs e)
        {
            CameraHelper.CAMERA_DEVICES.ForEach(t => t.CloseDevice());
        }
    }
}