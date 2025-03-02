using AI_Assistant_Win.Utils;
using MvCameraControl;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
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
            const string mutexName = "Global\\{1C87D98D-400E-46DD-9753-68C7F2337B45}";

            using (Mutex mutex = new Mutex(true, mutexName, out bool createdNew))
            {
                if (!createdNew)
                {
                    ActivateExistingWindow();
                    return;
                }
                SDKSystem.Initialize();
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
                Application.ThreadException += async (s, e) => await HandleExceptionAsync(e.Exception); ; // �����й��߳��е�δ�����쳣��
                AppDomain.CurrentDomain.UnhandledException += async (s, e) => await HandleExceptionAsync((Exception)e.ExceptionObject); // ������й��߳��е�δ�����쳣
                // �����û�ͨ������������������̵����������������������õ���Windows API ExitProcess����ֹ���̣����FormClosing��FormClosed�¼����ᱻ������
                Application.ApplicationExit += new EventHandler(OnApplicationExit);
                if (command == "m") Application.Run(new Main());
                else if (command == "color") Application.Run(new Colors());
                else if (command == "overview") Application.Run(new Overview(false));
                else Application.Run(new MainWindow(command == "t"));
                // ����Ҫ�ֶ� ReleaseMutex��using ���Զ��ͷ�
                //_mutex?.ReleaseMutex();
            }
        }


        // ����Windows API����
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        static void ActivateExistingWindow()
        {
            const int SW_RESTORE = 9;
            Process currentProcess = Process.GetCurrentProcess();
            string currentExePath = currentProcess.MainModule.FileName;

            foreach (Process p in Process.GetProcessesByName(currentProcess.ProcessName))
            {
                if (p.Id == currentProcess.Id) continue;
                try
                {
                    // ��֤����·���Ƿ���ͬ
                    if (p.MainModule.FileName.Equals(currentExePath, StringComparison.OrdinalIgnoreCase))
                    {
                        IntPtr hWnd = p.MainWindowHandle;
                        if (hWnd != IntPtr.Zero)
                        {
                            ShowWindow(hWnd, SW_RESTORE);
                            SetForegroundWindow(hWnd);
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // ����Ȩ�޲��㵼�µķ����쳣
                    Debug.WriteLine($"���ʽ��� {p.Id} ʧ��: {ex.Message}");
                }
            }
        }

        private static async Task HandleExceptionAsync(Exception ex)
        {
            try
            {
                // ��¼������־
                string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "error.log");
                await File.AppendAllTextAsync(logPath, $"{DateTime.Now}: {ex}{Environment.NewLine}");

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
            try
            {
                CameraHelper.CAMERA_DEVICES.ForEach(t =>
                {
                    try { t.CloseDevice(); }
                    catch { /* ��¼��־ */ }
                });
                SDKSystem.Finalize();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"�˳�ʱ�ͷ���Դʧ��: {ex}");
            }
        }
    }
}