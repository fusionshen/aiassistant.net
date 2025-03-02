using System.Drawing;
using System.Windows.Forms;

namespace AI_Assistant_Win
{
    partial class MainWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            btn_mode = new AntdUI.Button();
            btn_global = new AntdUI.Dropdown();
            btn_setting = new AntdUI.Button();
            virtualPanel = new AntdUI.VirtualPanel();
            windowBar = new AntdUI.PageHeader();
            txt_search = new AntdUI.Input();
            avatarLoginUser = new AntdUI.Avatar();
            colorTheme = new AntdUI.ColorPicker();
            notifyIcon1 = new NotifyIcon(components);
            contextMenuStrip1 = new ContextMenuStrip(components);
            exitStripMenuItem = new ToolStripMenuItem();
            windowBar.SuspendLayout();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // btn_mode
            // 
            btn_mode.Dock = DockStyle.Right;
            btn_mode.Ghost = true;
            btn_mode.IconSvg = "SunOutlined";
            btn_mode.Location = new Point(1006, 0);
            btn_mode.Name = "btn_mode";
            btn_mode.Radius = 0;
            btn_mode.Size = new Size(50, 40);
            btn_mode.TabIndex = 6;
            btn_mode.ToggleIconSvg = "MoonOutlined";
            btn_mode.WaveSize = 0;
            btn_mode.Click += Btn_mode_Click;
            // 
            // btn_global
            // 
            btn_global.Dock = DockStyle.Right;
            btn_global.DropDownRadius = 6;
            btn_global.Ghost = true;
            btn_global.IconSvg = "GlobalOutlined";
            btn_global.Location = new Point(1056, 0);
            btn_global.Name = "btn_global";
            btn_global.Placement = AntdUI.TAlignFrom.BR;
            btn_global.Radius = 0;
            btn_global.Size = new Size(50, 40);
            btn_global.TabIndex = 7;
            btn_global.WaveSize = 0;
            btn_global.SelectedValueChanged += Btn_global_Changed;
            // 
            // btn_setting
            // 
            btn_setting.Dock = DockStyle.Right;
            btn_setting.Ghost = true;
            btn_setting.IconSvg = "SettingOutlined";
            btn_setting.Location = new Point(1106, 0);
            btn_setting.Name = "btn_setting";
            btn_setting.Radius = 0;
            btn_setting.Size = new Size(50, 40);
            btn_setting.TabIndex = 8;
            btn_setting.WaveSize = 0;
            btn_setting.Click += Btn_setting_Click;
            // 
            // virtualPanel
            // 
            virtualPanel.Dock = DockStyle.Fill;
            virtualPanel.JustifyContent = AntdUI.TJustifyContent.SpaceEvenly;
            virtualPanel.Location = new Point(0, 40);
            virtualPanel.Name = "virtualPanel";
            virtualPanel.Shadow = 20;
            virtualPanel.ShadowOpacityAnimation = true;
            virtualPanel.Size = new Size(1300, 680);
            virtualPanel.TabIndex = 2;
            virtualPanel.Waterfall = true;
            virtualPanel.ItemClick += ItemClick;
            // 
            // windowBar
            // 
            windowBar.BackgroundImageLayout = ImageLayout.Stretch;
            windowBar.Controls.Add(txt_search);
            windowBar.Controls.Add(avatarLoginUser);
            windowBar.Controls.Add(colorTheme);
            windowBar.Controls.Add(btn_mode);
            windowBar.Controls.Add(btn_global);
            windowBar.Controls.Add(btn_setting);
            windowBar.DividerMargin = 3;
            windowBar.DividerShow = true;
            windowBar.Dock = DockStyle.Top;
            windowBar.Font = new Font("Microsoft YaHei UI", 12F);
            windowBar.Icon = Properties.Resources.logo;
            windowBar.LocalizationSubText = "Workbench";
            windowBar.LocalizationText = "Workbench.{id}";
            windowBar.Location = new Point(0, 0);
            windowBar.Name = "windowBar";
            windowBar.ShowButton = true;
            windowBar.ShowIcon = true;
            windowBar.Size = new Size(1300, 40);
            windowBar.SubText = "工作台";
            windowBar.TabIndex = 0;
            windowBar.Text = "人工智能助手";
            windowBar.BackClick += Btn_back_Click;
            // 
            // txt_search
            // 
            txt_search.Dock = DockStyle.Right;
            txt_search.LocalizationPlaceholderText = "Workbench.{id}";
            txt_search.Location = new Point(725, 0);
            txt_search.Name = "txt_search";
            txt_search.Padding = new Padding(0, 2, 0, 2);
            txt_search.PlaceholderText = "输入关键字搜索...";
            txt_search.PrefixSvg = "SearchOutlined";
            txt_search.Size = new Size(201, 40);
            txt_search.TabIndex = 10;
            txt_search.PrefixClick += Txt_search_PrefixClick;
            txt_search.TextChanged += Txt_search_TextChanged;
            // 
            // avatarLoginUser
            // 
            avatarLoginUser.Dock = DockStyle.Right;
            avatarLoginUser.Image = Properties.Resources.img1;
            avatarLoginUser.Location = new Point(926, 0);
            avatarLoginUser.Name = "avatarLoginUser";
            avatarLoginUser.Padding = new Padding(10);
            avatarLoginUser.Round = true;
            avatarLoginUser.Size = new Size(40, 40);
            avatarLoginUser.TabIndex = 10;
            avatarLoginUser.Click += AvatarLoginUser_Click;
            // 
            // colorTheme
            // 
            colorTheme.Dock = DockStyle.Right;
            colorTheme.Location = new Point(966, 0);
            colorTheme.Name = "colorTheme";
            colorTheme.Padding = new Padding(5);
            colorTheme.Size = new Size(40, 40);
            colorTheme.TabIndex = 8;
            colorTheme.Value = Color.FromArgb(22, 119, 255);
            colorTheme.Visible = false;
            // 
            // notifyIcon1
            // 
            notifyIcon1.ContextMenuStrip = contextMenuStrip1;
            notifyIcon1.Icon = (Icon)resources.GetObject("notifyIcon1.Icon");
            notifyIcon1.Text = "AI Assistant";
            notifyIcon1.Visible = true;
            notifyIcon1.DoubleClick += notifyIcon1_DoubleClick;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { exitStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(101, 26);
            contextMenuStrip1.ItemClicked += contextMenuStrip1_ItemClicked;
            // 
            // exitStripMenuItem
            // 
            exitStripMenuItem.Name = "exitStripMenuItem";
            exitStripMenuItem.Size = new Size(100, 22);
            exitStripMenuItem.Text = "退出";
            // 
            // MainWindow
            // 
            BackColor = Color.White;
            ClientSize = new Size(1300, 720);
            Controls.Add(virtualPanel);
            Controls.Add(windowBar);
            Font = new Font("Microsoft YaHei UI", 12F);
            ForeColor = Color.Black;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(660, 400);
            Name = "MainWindow";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "AI Assistant Workbench";
            windowBar.ResumeLayout(false);
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private AntdUI.Button btn_mode;
        private AntdUI.Dropdown btn_global;
        private AntdUI.Button btn_setting;
        private AntdUI.VirtualPanel virtualPanel;
        private AntdUI.PageHeader windowBar;
        private AntdUI.ColorPicker colorTheme;
        private AntdUI.Avatar avatarLoginUser;
        private AntdUI.Input txt_search;
        private NotifyIcon notifyIcon1;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem exitStripMenuItem;
    }
}