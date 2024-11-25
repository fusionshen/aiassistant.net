using AI_Assistant_Win;
using System;
using System.Runtime.InteropServices;
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
            if (command == "m") Application.Run(new Main());
            else if (command == "color") Application.Run(new Colors());
            else if (command == "t") Application.Run(new Overview(command == "t"));
            else Application.Run(new MainWindow(command == "prod"));
        }
    }
}