using System.Drawing;
using System.Windows.Forms;

namespace AI_Assistant_Win.Controls
{
    partial class CameraSetting
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
            panel1 = new System.Windows.Forms.Panel();
            panel3 = new System.Windows.Forms.Panel();
            panel7 = new System.Windows.Forms.Panel();
            panel14 = new AntdUI.Panel();
            bnSetParam = new AntdUI.Button();
            bnGetParam = new AntdUI.Button();
            button11 = new AntdUI.Button();
            panel13 = new AntdUI.Panel();
            cbPixelFormat = new AntdUI.Select();
            labelPixelFormat = new AntdUI.Label();
            panel12 = new AntdUI.Panel();
            tbFrameRate = new AntdUI.Input();
            labelFrameRate = new AntdUI.Label();
            panel9 = new AntdUI.Panel();
            tbGain = new AntdUI.Input();
            labelGain = new AntdUI.Label();
            panel15 = new AntdUI.Panel();
            tbExposure = new AntdUI.Input();
            labelExposureTime = new AntdUI.Label();
            divider3 = new AntdUI.Divider();
            panel4 = new System.Windows.Forms.Panel();
            panel11 = new AntdUI.Panel();
            bnTriggerExec = new AntdUI.Button();
            cbSoftTrigger = new AntdUI.Checkbox();
            button7 = new AntdUI.Button();
            panel5 = new AntdUI.Panel();
            bnStopGrab = new AntdUI.Button();
            bnStartGrab = new AntdUI.Button();
            bnTriggerMode = new AntdUI.Radio();
            bnContinuesMode = new AntdUI.Radio();
            button5 = new AntdUI.Button();
            divider2 = new AntdUI.Divider();
            panel2 = new System.Windows.Forms.Panel();
            bnClose = new AntdUI.Button();
            bnOpen = new AntdUI.Button();
            panel8 = new System.Windows.Forms.Panel();
            cbDeviceList = new AntdUI.Select();
            bnEnum = new AntdUI.Button();
            divider1 = new AntdUI.Divider();
            header1 = new AntdUI.PageHeader();
            panel17 = new AntdUI.Panel();
            button13 = new AntdUI.Button();
            button16 = new AntdUI.Button();
            button17 = new AntdUI.Button();
            panel1.SuspendLayout();
            panel3.SuspendLayout();
            panel7.SuspendLayout();
            panel14.SuspendLayout();
            panel13.SuspendLayout();
            panel12.SuspendLayout();
            panel9.SuspendLayout();
            panel15.SuspendLayout();
            panel4.SuspendLayout();
            panel11.SuspendLayout();
            panel5.SuspendLayout();
            panel2.SuspendLayout();
            panel8.SuspendLayout();
            panel17.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.AutoScroll = true;
            panel1.Controls.Add(panel3);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(420, 560);
            panel1.TabIndex = 6;
            // 
            // panel3
            // 
            panel3.AutoSize = true;
            panel3.Controls.Add(panel7);
            panel3.Controls.Add(divider3);
            panel3.Controls.Add(panel4);
            panel3.Controls.Add(divider2);
            panel3.Controls.Add(panel2);
            panel3.Controls.Add(divider1);
            panel3.Controls.Add(header1);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(0, 0);
            panel3.Name = "panel3";
            panel3.Size = new Size(420, 560);
            panel3.TabIndex = 1;
            // 
            // panel7
            // 
            panel7.Controls.Add(panel14);
            panel7.Controls.Add(panel13);
            panel7.Controls.Add(panel12);
            panel7.Controls.Add(panel9);
            panel7.Controls.Add(panel15);
            panel7.Dock = DockStyle.Top;
            panel7.Location = new Point(0, 349);
            panel7.Name = "panel7";
            panel7.Padding = new Padding(5);
            panel7.Size = new Size(420, 213);
            panel7.TabIndex = 13;
            // 
            // panel14
            // 
            panel14.Controls.Add(bnSetParam);
            panel14.Controls.Add(bnGetParam);
            panel14.Controls.Add(button11);
            panel14.Dock = DockStyle.Top;
            panel14.Location = new Point(5, 165);
            panel14.Name = "panel14";
            panel14.Padding = new Padding(80, 0, 80, 0);
            panel14.Radius = 0;
            panel14.Size = new Size(410, 46);
            panel14.TabIndex = 29;
            panel14.Text = "panel14";
            // 
            // bnSetParam
            // 
            bnSetParam.AutoSizeMode = AntdUI.TAutoSize.Auto;
            bnSetParam.Dock = DockStyle.Right;
            bnSetParam.Enabled = false;
            bnSetParam.LocalizationText = "Set Params";
            bnSetParam.Location = new Point(230, 0);
            bnSetParam.Name = "bnSetParam";
            bnSetParam.Size = new Size(100, 47);
            bnSetParam.TabIndex = 7;
            bnSetParam.Text = "参数设置";
            bnSetParam.Type = AntdUI.TTypeMini.Error;
            bnSetParam.Click += BnSetParam_Click;
            // 
            // bnGetParam
            // 
            bnGetParam.AutoSizeMode = AntdUI.TAutoSize.Auto;
            bnGetParam.Dock = DockStyle.Left;
            bnGetParam.Enabled = false;
            bnGetParam.LocalizationText = "Get Params";
            bnGetParam.Location = new Point(80, 0);
            bnGetParam.Name = "bnGetParam";
            bnGetParam.Size = new Size(100, 47);
            bnGetParam.TabIndex = 6;
            bnGetParam.Text = "参数获取";
            bnGetParam.Type = AntdUI.TTypeMini.Primary;
            bnGetParam.Click += BnGetParam_Click;
            // 
            // button11
            // 
            button11.Location = new Point(0, 0);
            button11.Name = "button11";
            button11.Size = new Size(0, 0);
            button11.TabIndex = 0;
            // 
            // panel13
            // 
            panel13.Back = Color.Transparent;
            panel13.BackColor = Color.Transparent;
            panel13.Controls.Add(cbPixelFormat);
            panel13.Controls.Add(labelPixelFormat);
            panel13.Dock = DockStyle.Top;
            panel13.Location = new Point(5, 125);
            panel13.Name = "panel13";
            panel13.Radius = 0;
            panel13.Size = new Size(410, 40);
            panel13.TabIndex = 28;
            // 
            // cbPixelFormat
            // 
            cbPixelFormat.AllowClear = true;
            cbPixelFormat.Dock = DockStyle.Left;
            cbPixelFormat.DropDownArrow = true;
            cbPixelFormat.Enabled = false;
            cbPixelFormat.Font = new Font("Microsoft YaHei UI", 10F);
            cbPixelFormat.LocalizationPlaceholderText = "";
            cbPixelFormat.Location = new Point(100, 0);
            cbPixelFormat.Name = "cbPixelFormat";
            cbPixelFormat.PlaceholderText = "";
            cbPixelFormat.Size = new Size(261, 40);
            cbPixelFormat.TabIndex = 17;
            // 
            // labelPixelFormat
            // 
            labelPixelFormat.BackColor = Color.Transparent;
            labelPixelFormat.Dock = DockStyle.Left;
            labelPixelFormat.Font = new Font("Microsoft YaHei UI", 10F);
            labelPixelFormat.LocalizationText = "PixelFormat:";
            labelPixelFormat.Location = new Point(0, 0);
            labelPixelFormat.Name = "labelPixelFormat";
            labelPixelFormat.Size = new Size(100, 40);
            labelPixelFormat.TabIndex = 16;
            labelPixelFormat.Text = "像素格式：";
            labelPixelFormat.TextAlign = ContentAlignment.MiddleRight;
            // 
            // panel12
            // 
            panel12.Back = Color.Transparent;
            panel12.BackColor = Color.Transparent;
            panel12.Controls.Add(tbFrameRate);
            panel12.Controls.Add(labelFrameRate);
            panel12.Dock = DockStyle.Top;
            panel12.Location = new Point(5, 85);
            panel12.Name = "panel12";
            panel12.Radius = 0;
            panel12.Size = new Size(410, 40);
            panel12.TabIndex = 27;
            // 
            // tbFrameRate
            // 
            tbFrameRate.Dock = DockStyle.Left;
            tbFrameRate.Enabled = false;
            tbFrameRate.Font = new Font("Microsoft YaHei UI", 10F);
            tbFrameRate.Location = new Point(100, 0);
            tbFrameRate.Name = "tbFrameRate";
            tbFrameRate.Size = new Size(261, 40);
            tbFrameRate.TabIndex = 17;
            // 
            // labelFrameRate
            // 
            labelFrameRate.BackColor = Color.Transparent;
            labelFrameRate.Dock = DockStyle.Left;
            labelFrameRate.Font = new Font("Microsoft YaHei UI", 10F);
            labelFrameRate.LocalizationText = "FrameRate:";
            labelFrameRate.Location = new Point(0, 0);
            labelFrameRate.Name = "labelFrameRate";
            labelFrameRate.Size = new Size(100, 40);
            labelFrameRate.TabIndex = 16;
            labelFrameRate.Text = "帧率：";
            labelFrameRate.TextAlign = ContentAlignment.MiddleRight;
            // 
            // panel9
            // 
            panel9.Back = Color.Transparent;
            panel9.BackColor = Color.Transparent;
            panel9.Controls.Add(tbGain);
            panel9.Controls.Add(labelGain);
            panel9.Dock = DockStyle.Top;
            panel9.Location = new Point(5, 45);
            panel9.Name = "panel9";
            panel9.Radius = 0;
            panel9.Size = new Size(410, 40);
            panel9.TabIndex = 26;
            // 
            // tbGain
            // 
            tbGain.Dock = DockStyle.Left;
            tbGain.Enabled = false;
            tbGain.Font = new Font("Microsoft YaHei UI", 10F);
            tbGain.Location = new Point(100, 0);
            tbGain.Name = "tbGain";
            tbGain.Size = new Size(261, 40);
            tbGain.TabIndex = 17;
            // 
            // labelGain
            // 
            labelGain.BackColor = Color.Transparent;
            labelGain.Dock = DockStyle.Left;
            labelGain.Font = new Font("Microsoft YaHei UI", 10F);
            labelGain.LocalizationText = "Gain:";
            labelGain.Location = new Point(0, 0);
            labelGain.Name = "labelGain";
            labelGain.Size = new Size(100, 40);
            labelGain.TabIndex = 16;
            labelGain.Text = "增益：";
            labelGain.TextAlign = ContentAlignment.MiddleRight;
            // 
            // panel15
            // 
            panel15.Back = Color.Transparent;
            panel15.BackColor = Color.Transparent;
            panel15.Controls.Add(tbExposure);
            panel15.Controls.Add(labelExposureTime);
            panel15.Dock = DockStyle.Top;
            panel15.Location = new Point(5, 5);
            panel15.Name = "panel15";
            panel15.Radius = 0;
            panel15.Size = new Size(410, 40);
            panel15.TabIndex = 25;
            // 
            // tbExposure
            // 
            tbExposure.Dock = DockStyle.Left;
            tbExposure.Enabled = false;
            tbExposure.Font = new Font("Microsoft YaHei UI", 10F);
            tbExposure.Location = new Point(100, 0);
            tbExposure.Name = "tbExposure";
            tbExposure.Size = new Size(261, 40);
            tbExposure.TabIndex = 17;
            // 
            // labelExposureTime
            // 
            labelExposureTime.BackColor = Color.Transparent;
            labelExposureTime.Dock = DockStyle.Left;
            labelExposureTime.Font = new Font("Microsoft YaHei UI", 10F);
            labelExposureTime.LocalizationText = "ExposureTime:";
            labelExposureTime.Location = new Point(0, 0);
            labelExposureTime.Name = "labelExposureTime";
            labelExposureTime.Size = new Size(100, 40);
            labelExposureTime.TabIndex = 16;
            labelExposureTime.Text = "曝光：";
            labelExposureTime.TextAlign = ContentAlignment.MiddleRight;
            // 
            // divider3
            // 
            divider3.Dock = DockStyle.Top;
            divider3.Font = new Font("Microsoft YaHei UI", 10F);
            divider3.LocalizationText = "Settings";
            divider3.Location = new Point(0, 315);
            divider3.Name = "divider3";
            divider3.Orientation = AntdUI.TOrientation.Left;
            divider3.Size = new Size(420, 34);
            divider3.TabIndex = 12;
            divider3.TabStop = false;
            divider3.Text = "参数设置";
            // 
            // panel4
            // 
            panel4.Controls.Add(panel11);
            panel4.Controls.Add(panel5);
            panel4.Dock = DockStyle.Top;
            panel4.Location = new Point(0, 210);
            panel4.Name = "panel4";
            panel4.Padding = new Padding(5);
            panel4.Size = new Size(420, 105);
            panel4.TabIndex = 11;
            // 
            // panel11
            // 
            panel11.Controls.Add(bnTriggerExec);
            panel11.Controls.Add(cbSoftTrigger);
            panel11.Controls.Add(button7);
            panel11.Dock = DockStyle.Top;
            panel11.Location = new Point(5, 53);
            panel11.Name = "panel11";
            panel11.Padding = new Padding(50, 0, 50, 0);
            panel11.Radius = 0;
            panel11.Size = new Size(410, 46);
            panel11.TabIndex = 9;
            panel11.Text = "panel11";
            // 
            // bnTriggerExec
            // 
            bnTriggerExec.AutoSizeMode = AntdUI.TAutoSize.Auto;
            bnTriggerExec.Dock = DockStyle.Right;
            bnTriggerExec.Enabled = false;
            bnTriggerExec.LocalizationText = "Soft Trigger Once";
            bnTriggerExec.Location = new Point(244, 0);
            bnTriggerExec.Name = "bnTriggerExec";
            bnTriggerExec.Size = new Size(116, 47);
            bnTriggerExec.TabIndex = 6;
            bnTriggerExec.Text = "软触发一次";
            bnTriggerExec.Type = AntdUI.TTypeMini.Primary;
            bnTriggerExec.Click += BnTriggerExec_Click;
            // 
            // cbSoftTrigger
            // 
            cbSoftTrigger.AutoCheck = true;
            cbSoftTrigger.AutoSizeMode = AntdUI.TAutoSize.Width;
            cbSoftTrigger.Dock = DockStyle.Left;
            cbSoftTrigger.Enabled = false;
            cbSoftTrigger.LocalizationText = "SoftTrigger";
            cbSoftTrigger.Location = new Point(50, 0);
            cbSoftTrigger.Name = "cbSoftTrigger";
            cbSoftTrigger.Size = new Size(98, 46);
            cbSoftTrigger.TabIndex = 1;
            cbSoftTrigger.Text = "软触发";
            cbSoftTrigger.CheckedChanged += CbSoftTrigger_CheckedChanged;
            // 
            // button7
            // 
            button7.Location = new Point(0, 0);
            button7.Name = "button7";
            button7.Size = new Size(0, 0);
            button7.TabIndex = 0;
            // 
            // panel5
            // 
            panel5.Controls.Add(bnStopGrab);
            panel5.Controls.Add(bnStartGrab);
            panel5.Controls.Add(bnTriggerMode);
            panel5.Controls.Add(bnContinuesMode);
            panel5.Controls.Add(button5);
            panel5.Dock = DockStyle.Top;
            panel5.Location = new Point(5, 5);
            panel5.Margin = new Padding(0);
            panel5.Name = "panel5";
            panel5.Radius = 0;
            panel5.Size = new Size(410, 48);
            panel5.TabIndex = 7;
            panel5.Text = "panel5";
            // 
            // bnStopGrab
            // 
            bnStopGrab.AutoSizeMode = AntdUI.TAutoSize.Auto;
            bnStopGrab.Dock = DockStyle.Left;
            bnStopGrab.Enabled = false;
            bnStopGrab.LocalizationText = "   Stop      ";
            bnStopGrab.Location = new Point(312, 0);
            bnStopGrab.Name = "bnStopGrab";
            bnStopGrab.Size = new Size(100, 47);
            bnStopGrab.TabIndex = 6;
            bnStopGrab.Text = "停止采集";
            bnStopGrab.Type = AntdUI.TTypeMini.Error;
            bnStopGrab.Click += BnStopGrab_Click;
            // 
            // bnStartGrab
            // 
            bnStartGrab.AutoSizeMode = AntdUI.TAutoSize.Auto;
            bnStartGrab.Dock = DockStyle.Left;
            bnStartGrab.Enabled = false;
            bnStartGrab.LocalizationText = "  Start   ";
            bnStartGrab.Location = new Point(212, 0);
            bnStartGrab.Name = "bnStartGrab";
            bnStartGrab.Size = new Size(100, 47);
            bnStartGrab.TabIndex = 5;
            bnStartGrab.Text = "开始采集";
            bnStartGrab.Type = AntdUI.TTypeMini.Primary;
            bnStartGrab.Click += BnStartGrab_Click;
            // 
            // bnTriggerMode
            // 
            bnTriggerMode.AutoCheck = true;
            bnTriggerMode.Dock = DockStyle.Left;
            bnTriggerMode.Enabled = false;
            bnTriggerMode.Fill = Color.FromArgb(200, 0, 0);
            bnTriggerMode.LocalizationText = "Trigger";
            bnTriggerMode.Location = new Point(106, 0);
            bnTriggerMode.Name = "bnTriggerMode";
            bnTriggerMode.Size = new Size(106, 48);
            bnTriggerMode.TabIndex = 3;
            bnTriggerMode.Text = "触发模式";
            bnTriggerMode.CheckedChanged += BnTriggerMode_CheckedChanged;
            // 
            // bnContinuesMode
            // 
            bnContinuesMode.AutoCheck = true;
            bnContinuesMode.Dock = DockStyle.Left;
            bnContinuesMode.Enabled = false;
            bnContinuesMode.Fill = Color.FromArgb(200, 0, 0);
            bnContinuesMode.LocalizationText = "Conti";
            bnContinuesMode.Location = new Point(0, 0);
            bnContinuesMode.Name = "bnContinuesMode";
            bnContinuesMode.Size = new Size(106, 48);
            bnContinuesMode.TabIndex = 2;
            bnContinuesMode.Text = "连续模式";
            bnContinuesMode.CheckedChanged += BnContinuesMode_CheckedChanged;
            // 
            // button5
            // 
            button5.Location = new Point(0, 0);
            button5.Name = "button5";
            button5.Size = new Size(0, 0);
            button5.TabIndex = 0;
            // 
            // divider2
            // 
            divider2.Dock = DockStyle.Top;
            divider2.Font = new Font("Microsoft YaHei UI", 10F);
            divider2.LocalizationText = "Acquisition";
            divider2.Location = new Point(0, 176);
            divider2.Name = "divider2";
            divider2.Orientation = AntdUI.TOrientation.Left;
            divider2.Size = new Size(420, 34);
            divider2.TabIndex = 10;
            divider2.TabStop = false;
            divider2.Text = "图像采集";
            // 
            // panel2
            // 
            panel2.Controls.Add(bnClose);
            panel2.Controls.Add(bnOpen);
            panel2.Controls.Add(panel8);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 108);
            panel2.Name = "panel2";
            panel2.Size = new Size(420, 68);
            panel2.TabIndex = 9;
            // 
            // bnClose
            // 
            bnClose.AutoSizeMode = AntdUI.TAutoSize.Auto;
            bnClose.Enabled = false;
            bnClose.LocalizationText = "  Close   ";
            bnClose.Location = new Point(319, 12);
            bnClose.Name = "bnClose";
            bnClose.Size = new Size(100, 47);
            bnClose.TabIndex = 5;
            bnClose.Text = "关闭设备";
            bnClose.Type = AntdUI.TTypeMini.Error;
            bnClose.Click += BnClose_Click;
            // 
            // bnOpen
            // 
            bnOpen.AutoSizeMode = AntdUI.TAutoSize.Auto;
            bnOpen.LocalizationText = "  Open   ";
            bnOpen.Location = new Point(222, 12);
            bnOpen.Name = "bnOpen";
            bnOpen.Size = new Size(100, 47);
            bnOpen.TabIndex = 4;
            bnOpen.Text = "打开设备";
            bnOpen.Type = AntdUI.TTypeMini.Primary;
            bnOpen.Click += BnOpen_Click;
            // 
            // panel8
            // 
            panel8.Controls.Add(cbDeviceList);
            panel8.Controls.Add(bnEnum);
            panel8.Location = new Point(4, 12);
            panel8.Name = "panel8";
            panel8.Size = new Size(220, 46);
            panel8.TabIndex = 3;
            panel8.Text = "panel4";
            // 
            // cbDeviceList
            // 
            cbDeviceList.AllowClear = true;
            cbDeviceList.Dock = DockStyle.Fill;
            cbDeviceList.JoinRight = true;
            cbDeviceList.LocalizationPlaceholderText = "Text something.";
            cbDeviceList.Location = new Point(0, 0);
            cbDeviceList.Name = "cbDeviceList";
            cbDeviceList.PlaceholderText = "输入点什么搜索";
            cbDeviceList.Size = new Size(170, 46);
            cbDeviceList.TabIndex = 0;
            // 
            // bnEnum
            // 
            bnEnum.Dock = DockStyle.Right;
            bnEnum.IconSvg = "SearchOutlined";
            bnEnum.JoinLeft = true;
            bnEnum.Location = new Point(170, 0);
            bnEnum.Name = "bnEnum";
            bnEnum.Size = new Size(50, 46);
            bnEnum.TabIndex = 1;
            bnEnum.Type = AntdUI.TTypeMini.Primary;
            bnEnum.Click += BnEnum_Click;
            // 
            // divider1
            // 
            divider1.Dock = DockStyle.Top;
            divider1.Font = new Font("Microsoft YaHei UI", 10F);
            divider1.LocalizationText = "Find";
            divider1.Location = new Point(0, 74);
            divider1.Name = "divider1";
            divider1.Orientation = AntdUI.TOrientation.Left;
            divider1.Size = new Size(420, 34);
            divider1.TabIndex = 8;
            divider1.TabStop = false;
            divider1.Text = "设备查找";
            // 
            // header1
            // 
            header1.Description = "查找设备、打开/关闭设备、参数设置";
            header1.Dock = DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F);
            header1.LocalizationDescription = "Find Device,Turn On/OFF,Parameter Settings";
            header1.LocalizationText = "Camera Settings";
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new Padding(0, 0, 0, 10);
            header1.Size = new Size(420, 74);
            header1.TabIndex = 7;
            header1.Text = "摄像头设置";
            header1.UseTitleFont = true;
            // 
            // panel17
            // 
            panel17.Controls.Add(button13);
            panel17.Controls.Add(button16);
            panel17.Controls.Add(button17);
            panel17.Dock = DockStyle.Top;
            panel17.Location = new Point(5, 165);
            panel17.Name = "panel17";
            panel17.Padding = new Padding(100, 0, 100, 0);
            panel17.Radius = 0;
            panel17.Size = new Size(410, 46);
            panel17.TabIndex = 29;
            panel17.Text = "panel14";
            // 
            // button13
            // 
            button13.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button13.Dock = DockStyle.Right;
            button13.Location = new Point(228, 0);
            button13.Name = "button13";
            button13.Size = new Size(82, 41);
            button13.TabIndex = 7;
            button13.Text = "参数设置";
            button13.Type = AntdUI.TTypeMini.Error;
            // 
            // button16
            // 
            button16.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button16.Dock = DockStyle.Left;
            button16.Location = new Point(100, 0);
            button16.Name = "button16";
            button16.Size = new Size(82, 41);
            button16.TabIndex = 6;
            button16.Text = "参数获取";
            button16.Type = AntdUI.TTypeMini.Primary;
            // 
            // button17
            // 
            button17.Location = new Point(0, 0);
            button17.Name = "button17";
            button17.Size = new Size(0, 0);
            button17.TabIndex = 0;
            // 
            // CameraSetting
            // 
            AutoScaleMode = AutoScaleMode.Inherit;
            Controls.Add(panel1);
            Font = new Font("Microsoft YaHei UI", 12F);
            Name = "CameraSetting";
            Size = new Size(420, 560);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel3.ResumeLayout(false);
            panel7.ResumeLayout(false);
            panel14.ResumeLayout(false);
            panel14.PerformLayout();
            panel13.ResumeLayout(false);
            panel12.ResumeLayout(false);
            panel9.ResumeLayout(false);
            panel15.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel11.ResumeLayout(false);
            panel11.PerformLayout();
            panel5.ResumeLayout(false);
            panel5.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel8.ResumeLayout(false);
            panel17.ResumeLayout(false);
            panel17.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private Label label_date11;
        private AntdUI.PageHeader header1;
        private AntdUI.Divider divider1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel8;
        private AntdUI.Select cbDeviceList;
        private AntdUI.Button bnEnum;
        private System.Windows.Forms.Panel panel7;
        private AntdUI.Divider divider3;
        private System.Windows.Forms.Panel panel4;
        private AntdUI.Divider divider2;
        private AntdUI.Button bnOpen;
        private AntdUI.Button bnClose;
        private AntdUI.Panel panel5;
        private AntdUI.Button button5;
        private AntdUI.Radio bnTriggerMode;
        private AntdUI.Radio bnContinuesMode;
        private AntdUI.Panel panel11;
        private AntdUI.Button button7;
        private AntdUI.Button bnStopGrab;
        private AntdUI.Button bnStartGrab;
        private AntdUI.Checkbox cbSoftTrigger;
        private AntdUI.Button bnTriggerExec;
        private AntdUI.Panel panel12;
        private AntdUI.Panel panel9;
        private AntdUI.Input tbGain;
        private AntdUI.Label labelGain;
        private AntdUI.Panel panel15;
        private AntdUI.Label labelExposureTime;
        private AntdUI.Panel panel13;
        private AntdUI.Select cbPixelFormat;
        private AntdUI.Label labelPixelFormat;
        private AntdUI.Input tbFrameRate;
        private AntdUI.Label labelFrameRate;
        private AntdUI.Input tbExposure;
        private AntdUI.Panel panel14;
        private AntdUI.Button button11;
        private AntdUI.Button bnSetParam;
        private AntdUI.Button bnGetParam;
        private AntdUI.Panel panel17;
        private AntdUI.Button button13;
        private AntdUI.Button button16;
        private AntdUI.Button button17;
    }
}