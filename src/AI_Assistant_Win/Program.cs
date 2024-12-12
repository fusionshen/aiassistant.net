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
            Application.ThreadException += Application_ThreadException; // �����й��߳��е�δ�����쳣��
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException; // ������й��߳��е�δ�����쳣
            // �����û�ͨ������������������̵����������������������õ���Windows API ExitProcess����ֹ���̣����FormClosing��FormClosed�¼����ᱻ������
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
                // ��¼������־
                string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "error.log");
                File.AppendAllText(logPath, $"{DateTime.Now}: {ex}{Environment.NewLine}");

                // ��ʾ��ʾ��
                //MessageBox.Show($"������������ϵ����Ա�������Ѽ�¼��{logPath}��", "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AntdUI.Modal.open(new AntdUI.Modal.Config("������ʾ", $"������������ϵ����Ա�������Ѽ�¼��{logPath}��", AntdUI.TType.Error)
                {
                    OnButtonStyle = (id, btn) =>
                    {
                        btn.BackExtend = "135, #6253E1, #04BEFE";
                    },
                    CancelText = null,
                    OkText = "ȷ��"
                });

                // ���ŵعر�Ӧ�ó���
                Application.Exit();
            }
            catch (Exception)
            {
                // �����¼��־����ʾ��ʾ��ʱ�����쳣������ȷ��Ӧ�ó����ܹ��˳�
                Environment.Exit(1);
            }
        }

        /// <summary>
        /// ����ʱ������󣬻��������ر�ʱ����Ҫ�ر�����ͷռ�ã������ʱ�����ٴ�ʹ��ʱ������ͷû��Ȩ�ޡ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnApplicationExit(object sender, EventArgs e)
        {
            CameraHelper.CAMERA_DEVICES.ForEach(t => t.CloseDevice());
        }
    }
}