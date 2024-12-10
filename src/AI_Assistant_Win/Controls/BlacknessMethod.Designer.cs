using System.Drawing;
using System.Windows.Forms;

namespace AI_Assistant_Win.Controls
{
    partial class BlacknessMethod
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
            header1 = new AntdUI.PageHeader();
            panel1 = new AntdUI.Panel();
            blacknessMethod_RenderImage_Text = new Label();
            blacknessMethod_RenderImage = new AntdUI.Avatar();
            panel2 = new AntdUI.Panel();
            btn_Predict = new AntdUI.Button();
            blacknessMethod_RenderImage_Zone = new Label();
            panel4 = new AntdUI.Panel();
            blacknessMethod_OriginImage_Text = new Label();
            avatarOriginImage = new AntdUI.Avatar();
            panel5 = new AntdUI.Panel();
            btnCameraSetting = new AntdUI.Button();
            btn_CameraCapture = new AntdUI.Button();
            btn_UploadImage = new AntdUI.Button();
            blacknessMethod_OriginImage_Zone = new Label();
            flowLayoutPanel1 = new FlowLayoutPanel();
            panel6 = new AntdUI.Panel();
            label12 = new Label();
            panel17 = new AntdUI.Panel();
            btn_Save = new AntdUI.Button();
            panel16 = new AntdUI.Panel();
            input_Size = new AntdUI.Input();
            label_Size = new Label();
            panel15 = new AntdUI.Panel();
            input_Coil_Number = new AntdUI.Input();
            label_Coil_Number = new Label();
            panel14 = new AntdUI.Panel();
            select_Analyst = new AntdUI.Select();
            label_Analyst = new Label();
            panel8 = new AntdUI.Panel();
            select_Work_Group = new AntdUI.Select();
            label_Work_Group = new Label();
            divider2 = new AntdUI.Divider();
            panel13 = new AntdUI.Panel();
            radio_Result_NG = new AntdUI.Radio();
            radio_Result_OK = new AntdUI.Radio();
            panel12 = new AntdUI.Panel();
            input_Inside_DR = new AntdUI.Input();
            label_Inside_DR = new Label();
            panel11 = new AntdUI.Panel();
            input_Inside_CE = new AntdUI.Input();
            label_Inside_CE = new Label();
            panel10 = new AntdUI.Panel();
            input_Inside_OP = new AntdUI.Input();
            label_Inside_OP = new Label();
            panel9 = new AntdUI.Panel();
            input_Surface_DR = new AntdUI.Input();
            label_Surface_DR = new Label();
            panel7 = new AntdUI.Panel();
            input_Surface_CE = new AntdUI.Input();
            label_Surface_CE = new Label();
            panel3 = new AntdUI.Panel();
            input_Surface_OP = new AntdUI.Input();
            label_Surface_OP = new Label();
            label1 = new Label();
            blacknessMethod_OpenFileDialog = new OpenFileDialog();
            blacknessMethod_FileSystemWatcher = new System.IO.FileSystemWatcher();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel4.SuspendLayout();
            panel5.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            panel6.SuspendLayout();
            panel17.SuspendLayout();
            panel16.SuspendLayout();
            panel15.SuspendLayout();
            panel14.SuspendLayout();
            panel8.SuspendLayout();
            panel13.SuspendLayout();
            panel12.SuspendLayout();
            panel11.SuspendLayout();
            panel10.SuspendLayout();
            panel9.SuspendLayout();
            panel7.SuspendLayout();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)blacknessMethod_FileSystemWatcher).BeginInit();
            SuspendLayout();
            // 
            // header1
            // 
            header1.Description = "本方法适用于GA板锌层密着性试验中V60试验后胶带的黑度评级。测试白板、产品特性、试验弯曲角度等有明显相关性，胶带的氧化发黄会对该评定有干扰。";
            header1.Dock = DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F);
            header1.LocalizationDescription = "BlacknessMethod.Description";
            header1.LocalizationText = "BlacknessMethod.Text";
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new Padding(0, 0, 0, 10);
            header1.Size = new Size(1300, 74);
            header1.TabIndex = 0;
            header1.Text = "GA板锌层密着性V60黑度检测";
            header1.UseTitleFont = true;
            // 
            // panel1
            // 
            panel1.ArrowSize = 10;
            panel1.Controls.Add(blacknessMethod_RenderImage_Text);
            panel1.Controls.Add(blacknessMethod_RenderImage);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(blacknessMethod_RenderImage_Zone);
            panel1.Location = new Point(449, 3);
            panel1.Name = "panel1";
            panel1.Padding = new Padding(7);
            panel1.Radius = 10;
            panel1.Shadow = 2;
            panel1.ShadowOpacityAnimation = true;
            panel1.Size = new Size(462, 584);
            panel1.TabIndex = 20;
            // 
            // blacknessMethod_RenderImage_Text
            // 
            blacknessMethod_RenderImage_Text.BackColor = Color.Transparent;
            blacknessMethod_RenderImage_Text.Dock = DockStyle.Fill;
            blacknessMethod_RenderImage_Text.Font = new Font("Microsoft YaHei UI", 10F);
            blacknessMethod_RenderImage_Text.Location = new Point(9, 494);
            blacknessMethod_RenderImage_Text.Name = "blacknessMethod_RenderImage_Text";
            blacknessMethod_RenderImage_Text.Padding = new Padding(2, 0, 2, 0);
            blacknessMethod_RenderImage_Text.Size = new Size(444, 41);
            blacknessMethod_RenderImage_Text.TabIndex = 12;
            blacknessMethod_RenderImage_Text.Text = "点击识别后的图片可进行放大预览，各部位识别结果会在右侧结果判定区展示。";
            // 
            // blacknessMethod_RenderImage
            // 
            blacknessMethod_RenderImage.Dock = DockStyle.Top;
            blacknessMethod_RenderImage.Image = Properties.Resources.blackness_template;
            blacknessMethod_RenderImage.Location = new Point(9, 39);
            blacknessMethod_RenderImage.Name = "blacknessMethod_RenderImage";
            blacknessMethod_RenderImage.Radius = 6;
            blacknessMethod_RenderImage.Size = new Size(444, 455);
            blacknessMethod_RenderImage.TabIndex = 9;
            blacknessMethod_RenderImage.Click += BlacknessMethod_renderImage_Click;
            // 
            // panel2
            // 
            panel2.Back = Color.Transparent;
            panel2.BackColor = Color.Transparent;
            panel2.Controls.Add(btn_Predict);
            panel2.Dock = DockStyle.Bottom;
            panel2.Location = new Point(9, 535);
            panel2.Name = "panel2";
            panel2.Radius = 0;
            panel2.Size = new Size(444, 40);
            panel2.TabIndex = 13;
            // 
            // btn_Predict
            // 
            btn_Predict.Dock = DockStyle.Right;
            btn_Predict.Font = new Font("Microsoft YaHei UI", 10F);
            btn_Predict.LocalizationText = "Predict";
            btn_Predict.Location = new Point(371, 0);
            btn_Predict.Name = "btn_Predict";
            btn_Predict.Size = new Size(73, 40);
            btn_Predict.TabIndex = 0;
            btn_Predict.Text = "识别";
            btn_Predict.Type = AntdUI.TTypeMini.Primary;
            btn_Predict.Click += Btn_Predict_Click;
            // 
            // blacknessMethod_RenderImage_Zone
            // 
            blacknessMethod_RenderImage_Zone.BackColor = Color.Transparent;
            blacknessMethod_RenderImage_Zone.Dock = DockStyle.Top;
            blacknessMethod_RenderImage_Zone.Font = new Font("Microsoft YaHei UI", 15F, FontStyle.Bold);
            blacknessMethod_RenderImage_Zone.ForeColor = Color.DarkGray;
            blacknessMethod_RenderImage_Zone.Location = new Point(9, 9);
            blacknessMethod_RenderImage_Zone.Name = "blacknessMethod_RenderImage_Zone";
            blacknessMethod_RenderImage_Zone.Size = new Size(444, 30);
            blacknessMethod_RenderImage_Zone.TabIndex = 11;
            blacknessMethod_RenderImage_Zone.Text = "识 别 展 示 区";
            blacknessMethod_RenderImage_Zone.TextAlign = ContentAlignment.TopCenter;
            // 
            // panel4
            // 
            panel4.ArrowSize = 10;
            panel4.Controls.Add(blacknessMethod_OriginImage_Text);
            panel4.Controls.Add(avatarOriginImage);
            panel4.Controls.Add(panel5);
            panel4.Controls.Add(blacknessMethod_OriginImage_Zone);
            panel4.Location = new Point(3, 3);
            panel4.Name = "panel4";
            panel4.Padding = new Padding(7);
            panel4.Radius = 10;
            panel4.Shadow = 2;
            panel4.ShadowOpacityAnimation = true;
            panel4.Size = new Size(440, 584);
            panel4.TabIndex = 19;
            // 
            // blacknessMethod_OriginImage_Text
            // 
            blacknessMethod_OriginImage_Text.BackColor = Color.Transparent;
            blacknessMethod_OriginImage_Text.Dock = DockStyle.Fill;
            blacknessMethod_OriginImage_Text.Font = new Font("Microsoft YaHei UI", 10F);
            blacknessMethod_OriginImage_Text.Location = new Point(9, 494);
            blacknessMethod_OriginImage_Text.Name = "blacknessMethod_OriginImage_Text";
            blacknessMethod_OriginImage_Text.Padding = new Padding(2, 0, 2, 0);
            blacknessMethod_OriginImage_Text.Size = new Size(422, 41);
            blacknessMethod_OriginImage_Text.TabIndex = 12;
            blacknessMethod_OriginImage_Text.Text = "选择本地图片上传或者调用黑度检测工作台摄像头拍摄照片，上传成功后结果会自动识别。";
            // 
            // avatarOriginImage
            // 
            avatarOriginImage.Dock = DockStyle.Top;
            avatarOriginImage.Image = Properties.Resources.blackness_template;
            avatarOriginImage.Location = new Point(9, 39);
            avatarOriginImage.Name = "avatarOriginImage";
            avatarOriginImage.Radius = 6;
            avatarOriginImage.Size = new Size(422, 455);
            avatarOriginImage.TabIndex = 9;
            avatarOriginImage.Click += AvatarOriginImage_Click;
            // 
            // panel5
            // 
            panel5.Back = Color.Transparent;
            panel5.BackColor = Color.Transparent;
            panel5.Controls.Add(btnCameraSetting);
            panel5.Controls.Add(btn_CameraCapture);
            panel5.Controls.Add(btn_UploadImage);
            panel5.Dock = DockStyle.Bottom;
            panel5.Location = new Point(9, 535);
            panel5.Name = "panel5";
            panel5.Radius = 0;
            panel5.Size = new Size(422, 40);
            panel5.TabIndex = 13;
            // 
            // btnCameraSetting
            // 
            btnCameraSetting.IconSvg = "SettingOutlined";
            btnCameraSetting.LoadingWaveVertical = true;
            btnCameraSetting.Location = new Point(0, 0);
            btnCameraSetting.Name = "btnCameraSetting";
            btnCameraSetting.Shape = AntdUI.TShape.Circle;
            btnCameraSetting.Size = new Size(40, 40);
            btnCameraSetting.TabIndex = 2;
            btnCameraSetting.Type = AntdUI.TTypeMini.Primary;
            btnCameraSetting.Click += BtnCameraSetting_Click;
            // 
            // btn_CameraCapture
            // 
            btn_CameraCapture.Dock = DockStyle.Right;
            btn_CameraCapture.Font = new Font("Microsoft YaHei UI", 10F);
            btn_CameraCapture.LocalizationText = "CameraCapture";
            btn_CameraCapture.Location = new Point(192, 0);
            btn_CameraCapture.Name = "btn_CameraCapture";
            btn_CameraCapture.Size = new Size(119, 40);
            btn_CameraCapture.TabIndex = 1;
            btn_CameraCapture.Text = "相机拍摄";
            btn_CameraCapture.Type = AntdUI.TTypeMini.Primary;
            btn_CameraCapture.Click += Btn_CameraCapture_Click;
            // 
            // btn_UploadImage
            // 
            btn_UploadImage.Dock = DockStyle.Right;
            btn_UploadImage.Font = new Font("Microsoft YaHei UI", 10F);
            btn_UploadImage.LocalizationText = "UploadImage";
            btn_UploadImage.Location = new Point(311, 0);
            btn_UploadImage.Name = "btn_UploadImage";
            btn_UploadImage.Size = new Size(111, 40);
            btn_UploadImage.TabIndex = 0;
            btn_UploadImage.Text = "选择本地图片";
            btn_UploadImage.Type = AntdUI.TTypeMini.Primary;
            btn_UploadImage.Click += Btn_UploadImage_Click;
            // 
            // blacknessMethod_OriginImage_Zone
            // 
            blacknessMethod_OriginImage_Zone.BackColor = Color.Transparent;
            blacknessMethod_OriginImage_Zone.Dock = DockStyle.Top;
            blacknessMethod_OriginImage_Zone.Font = new Font("Microsoft YaHei UI", 15F, FontStyle.Bold);
            blacknessMethod_OriginImage_Zone.ForeColor = Color.DarkGray;
            blacknessMethod_OriginImage_Zone.Location = new Point(9, 9);
            blacknessMethod_OriginImage_Zone.Name = "blacknessMethod_OriginImage_Zone";
            blacknessMethod_OriginImage_Zone.Size = new Size(422, 30);
            blacknessMethod_OriginImage_Zone.TabIndex = 11;
            blacknessMethod_OriginImage_Zone.Text = "原 图 展 示 区";
            blacknessMethod_OriginImage_Zone.TextAlign = ContentAlignment.TopCenter;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.Controls.Add(panel4);
            flowLayoutPanel1.Controls.Add(panel1);
            flowLayoutPanel1.Controls.Add(panel6);
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Location = new Point(0, 74);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(1300, 560);
            flowLayoutPanel1.TabIndex = 5;
            // 
            // panel6
            // 
            panel6.ArrowSize = 10;
            panel6.Controls.Add(label12);
            panel6.Controls.Add(panel17);
            panel6.Controls.Add(panel16);
            panel6.Controls.Add(panel15);
            panel6.Controls.Add(panel14);
            panel6.Controls.Add(panel8);
            panel6.Controls.Add(divider2);
            panel6.Controls.Add(panel13);
            panel6.Controls.Add(panel12);
            panel6.Controls.Add(panel11);
            panel6.Controls.Add(panel10);
            panel6.Controls.Add(panel9);
            panel6.Controls.Add(panel7);
            panel6.Controls.Add(panel3);
            panel6.Controls.Add(label1);
            panel6.Location = new Point(917, 3);
            panel6.Name = "panel6";
            panel6.Padding = new Padding(7);
            panel6.Radius = 10;
            panel6.Shadow = 2;
            panel6.ShadowOpacity = 0.18F;
            panel6.ShadowOpacityAnimation = true;
            panel6.Size = new Size(360, 584);
            panel6.TabIndex = 20;
            // 
            // label12
            // 
            label12.BackColor = Color.Transparent;
            label12.Dock = DockStyle.Fill;
            label12.Font = new Font("Microsoft YaHei UI", 10F);
            label12.ForeColor = Color.Red;
            label12.Location = new Point(9, 436);
            label12.Name = "label12";
            label12.Padding = new Padding(2, 0, 2, 0);
            label12.Size = new Size(342, 99);
            label12.TabIndex = 27;
            label12.Text = "注：此测试是应用于GA板V60黑度判定，板厚要求在0.3~2.3mm";
            label12.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel17
            // 
            panel17.Back = Color.Transparent;
            panel17.BackColor = Color.Transparent;
            panel17.Controls.Add(btn_Save);
            panel17.Dock = DockStyle.Bottom;
            panel17.Location = new Point(9, 535);
            panel17.Name = "panel17";
            panel17.Radius = 0;
            panel17.Size = new Size(342, 40);
            panel17.TabIndex = 26;
            // 
            // btn_Save
            // 
            btn_Save.Dock = DockStyle.Right;
            btn_Save.Font = new Font("Microsoft YaHei UI", 10F);
            btn_Save.LocalizationText = "Save";
            btn_Save.Location = new Point(269, 0);
            btn_Save.Name = "btn_Save";
            btn_Save.Size = new Size(73, 40);
            btn_Save.TabIndex = 0;
            btn_Save.Text = "保存";
            btn_Save.Type = AntdUI.TTypeMini.Primary;
            btn_Save.Click += Btn_Save_Click;
            // 
            // panel16
            // 
            panel16.Back = Color.Transparent;
            panel16.BackColor = Color.Transparent;
            panel16.Controls.Add(input_Size);
            panel16.Controls.Add(label_Size);
            panel16.Dock = DockStyle.Top;
            panel16.Location = new Point(9, 403);
            panel16.Name = "panel16";
            panel16.Radius = 0;
            panel16.Size = new Size(342, 33);
            panel16.TabIndex = 25;
            // 
            // input_Size
            // 
            input_Size.Font = new Font("Microsoft YaHei UI", 10F);
            input_Size.Location = new Point(78, -1);
            input_Size.Name = "input_Size";
            input_Size.Size = new Size(261, 34);
            input_Size.TabIndex = 17;
            // 
            // label_Size
            // 
            label_Size.BackColor = Color.Transparent;
            label_Size.Dock = DockStyle.Left;
            label_Size.Font = new Font("Microsoft YaHei UI", 10F);
            label_Size.Location = new Point(0, 0);
            label_Size.Name = "label_Size";
            label_Size.Size = new Size(72, 33);
            label_Size.TabIndex = 16;
            label_Size.Text = "尺寸：";
            label_Size.TextAlign = ContentAlignment.MiddleRight;
            // 
            // panel15
            // 
            panel15.Back = Color.Transparent;
            panel15.BackColor = Color.Transparent;
            panel15.Controls.Add(input_Coil_Number);
            panel15.Controls.Add(label_Coil_Number);
            panel15.Dock = DockStyle.Top;
            panel15.Location = new Point(9, 370);
            panel15.Name = "panel15";
            panel15.Radius = 0;
            panel15.Size = new Size(342, 33);
            panel15.TabIndex = 24;
            // 
            // input_Coil_Number
            // 
            input_Coil_Number.Font = new Font("Microsoft YaHei UI", 10F);
            input_Coil_Number.Location = new Point(78, -1);
            input_Coil_Number.Name = "input_Coil_Number";
            input_Coil_Number.Size = new Size(261, 34);
            input_Coil_Number.TabIndex = 17;
            // 
            // label_Coil_Number
            // 
            label_Coil_Number.BackColor = Color.Transparent;
            label_Coil_Number.Dock = DockStyle.Left;
            label_Coil_Number.Font = new Font("Microsoft YaHei UI", 10F);
            label_Coil_Number.Location = new Point(0, 0);
            label_Coil_Number.Name = "label_Coil_Number";
            label_Coil_Number.Size = new Size(72, 33);
            label_Coil_Number.TabIndex = 16;
            label_Coil_Number.Text = "钢卷号：";
            label_Coil_Number.TextAlign = ContentAlignment.MiddleRight;
            // 
            // panel14
            // 
            panel14.Back = Color.Transparent;
            panel14.BackColor = Color.Transparent;
            panel14.Controls.Add(select_Analyst);
            panel14.Controls.Add(label_Analyst);
            panel14.Dock = DockStyle.Top;
            panel14.Location = new Point(9, 337);
            panel14.Name = "panel14";
            panel14.Radius = 0;
            panel14.Size = new Size(342, 33);
            panel14.TabIndex = 23;
            // 
            // select_Analyst
            // 
            select_Analyst.AllowClear = true;
            select_Analyst.Dock = DockStyle.Left;
            select_Analyst.DropDownArrow = true;
            select_Analyst.Font = new Font("Microsoft YaHei UI", 10F);
            select_Analyst.Items.AddRange(new object[] { "00001-刘一", "00002-陈二", "00003-张三", "00004-李四", "00005-王五", "00006-赵六", "00007-孙七", "00008-周八", "00009-吴九", "00010-郑十" });
            select_Analyst.LocalizationPlaceholderText = "Select.{id}";
            select_Analyst.Location = new Point(72, 0);
            select_Analyst.Name = "select_Analyst";
            select_Analyst.Padding = new Padding(5, 0, 0, 0);
            select_Analyst.PlaceholderText = "请选择分析人员";
            select_Analyst.Size = new Size(267, 33);
            select_Analyst.TabIndex = 18;
            // 
            // label_Analyst
            // 
            label_Analyst.BackColor = Color.Transparent;
            label_Analyst.Dock = DockStyle.Left;
            label_Analyst.Font = new Font("Microsoft YaHei UI", 10F);
            label_Analyst.Location = new Point(0, 0);
            label_Analyst.Name = "label_Analyst";
            label_Analyst.Size = new Size(72, 33);
            label_Analyst.TabIndex = 16;
            label_Analyst.Text = "分析人：";
            label_Analyst.TextAlign = ContentAlignment.MiddleRight;
            // 
            // panel8
            // 
            panel8.Back = Color.Transparent;
            panel8.BackColor = Color.Transparent;
            panel8.Controls.Add(select_Work_Group);
            panel8.Controls.Add(label_Work_Group);
            panel8.Dock = DockStyle.Top;
            panel8.Location = new Point(9, 304);
            panel8.Name = "panel8";
            panel8.Radius = 0;
            panel8.Size = new Size(342, 33);
            panel8.TabIndex = 22;
            // 
            // select_Work_Group
            // 
            select_Work_Group.AllowClear = true;
            select_Work_Group.Dock = DockStyle.Left;
            select_Work_Group.DropDownArrow = true;
            select_Work_Group.Font = new Font("Microsoft YaHei UI", 10F);
            select_Work_Group.Items.AddRange(new object[] { "甲-白", "甲-夜", "乙-白", "乙-夜", "丙-白", "丙-夜", "丁-白", "丁-夜" });
            select_Work_Group.LocalizationPlaceholderText = "Select.{id}";
            select_Work_Group.Location = new Point(72, 0);
            select_Work_Group.Name = "select_Work_Group";
            select_Work_Group.Padding = new Padding(5, 0, 0, 0);
            select_Work_Group.PlaceholderText = "请选择班组";
            select_Work_Group.Size = new Size(267, 33);
            select_Work_Group.TabIndex = 17;
            // 
            // label_Work_Group
            // 
            label_Work_Group.BackColor = Color.Transparent;
            label_Work_Group.Dock = DockStyle.Left;
            label_Work_Group.Font = new Font("Microsoft YaHei UI", 10F);
            label_Work_Group.Location = new Point(0, 0);
            label_Work_Group.Name = "label_Work_Group";
            label_Work_Group.Size = new Size(72, 33);
            label_Work_Group.TabIndex = 16;
            label_Work_Group.Text = "班组：";
            label_Work_Group.TextAlign = ContentAlignment.MiddleRight;
            // 
            // divider2
            // 
            divider2.BackColor = Color.Transparent;
            divider2.Dock = DockStyle.Top;
            divider2.Font = new Font("Microsoft YaHei UI", 16F, FontStyle.Bold);
            divider2.ForeColor = Color.DarkGray;
            divider2.Location = new Point(9, 270);
            divider2.Name = "divider2";
            divider2.Size = new Size(342, 34);
            divider2.TabIndex = 21;
            divider2.Text = "数 据 录 入 区";
            divider2.Thickness = 2F;
            // 
            // panel13
            // 
            panel13.Back = Color.Transparent;
            panel13.BackColor = Color.Transparent;
            panel13.Controls.Add(radio_Result_NG);
            panel13.Controls.Add(radio_Result_OK);
            panel13.Dock = DockStyle.Top;
            panel13.Location = new Point(9, 237);
            panel13.Name = "panel13";
            panel13.Radius = 0;
            panel13.RightToLeft = RightToLeft.No;
            panel13.Size = new Size(342, 33);
            panel13.TabIndex = 20;
            // 
            // radio_Result_NG
            // 
            radio_Result_NG.AutoCheck = true;
            radio_Result_NG.AutoSizeMode = AntdUI.TAutoSize.Width;
            radio_Result_NG.Checked = true;
            radio_Result_NG.Fill = Color.FromArgb(100, 0, 0);
            radio_Result_NG.Font = new Font("Microsoft YaHei UI", 12F);
            radio_Result_NG.ForeColor = Color.Red;
            radio_Result_NG.Location = new Point(180, 0);
            radio_Result_NG.Name = "radio_Result_NG";
            radio_Result_NG.Size = new Size(74, 33);
            radio_Result_NG.TabIndex = 18;
            radio_Result_NG.Text = "NG";
            // 
            // radio_Result_OK
            // 
            radio_Result_OK.AutoCheck = true;
            radio_Result_OK.AutoSizeMode = AntdUI.TAutoSize.Width;
            radio_Result_OK.BackColor = Color.Transparent;
            radio_Result_OK.Fill = Color.FromArgb(250, 0, 0);
            radio_Result_OK.Font = new Font("Microsoft YaHei UI", 12F);
            radio_Result_OK.ForeColor = Color.Green;
            radio_Result_OK.Location = new Point(90, 0);
            radio_Result_OK.Name = "radio_Result_OK";
            radio_Result_OK.Size = new Size(73, 33);
            radio_Result_OK.TabIndex = 17;
            radio_Result_OK.Text = "OK";
            // 
            // panel12
            // 
            panel12.Back = Color.Transparent;
            panel12.BackColor = Color.Transparent;
            panel12.Controls.Add(input_Inside_DR);
            panel12.Controls.Add(label_Inside_DR);
            panel12.Dock = DockStyle.Top;
            panel12.Location = new Point(9, 204);
            panel12.Name = "panel12";
            panel12.Radius = 0;
            panel12.Size = new Size(342, 33);
            panel12.TabIndex = 19;
            // 
            // input_Inside_DR
            // 
            input_Inside_DR.Font = new Font("Microsoft YaHei UI", 10F);
            input_Inside_DR.Location = new Point(78, -1);
            input_Inside_DR.Name = "input_Inside_DR";
            input_Inside_DR.Size = new Size(261, 34);
            input_Inside_DR.TabIndex = 17;
            // 
            // label_Inside_DR
            // 
            label_Inside_DR.BackColor = Color.Transparent;
            label_Inside_DR.Dock = DockStyle.Left;
            label_Inside_DR.Font = new Font("Microsoft YaHei UI", 10F);
            label_Inside_DR.Location = new Point(0, 0);
            label_Inside_DR.Name = "label_Inside_DR";
            label_Inside_DR.Size = new Size(72, 33);
            label_Inside_DR.TabIndex = 16;
            label_Inside_DR.Text = "里面DR：";
            label_Inside_DR.TextAlign = ContentAlignment.MiddleRight;
            // 
            // panel11
            // 
            panel11.Back = Color.Transparent;
            panel11.BackColor = Color.Transparent;
            panel11.Controls.Add(input_Inside_CE);
            panel11.Controls.Add(label_Inside_CE);
            panel11.Dock = DockStyle.Top;
            panel11.Location = new Point(9, 171);
            panel11.Name = "panel11";
            panel11.Radius = 0;
            panel11.Size = new Size(342, 33);
            panel11.TabIndex = 18;
            // 
            // input_Inside_CE
            // 
            input_Inside_CE.Font = new Font("Microsoft YaHei UI", 10F);
            input_Inside_CE.Location = new Point(78, -1);
            input_Inside_CE.Name = "input_Inside_CE";
            input_Inside_CE.Size = new Size(261, 34);
            input_Inside_CE.TabIndex = 17;
            // 
            // label_Inside_CE
            // 
            label_Inside_CE.BackColor = Color.Transparent;
            label_Inside_CE.Dock = DockStyle.Left;
            label_Inside_CE.Font = new Font("Microsoft YaHei UI", 10F);
            label_Inside_CE.Location = new Point(0, 0);
            label_Inside_CE.Name = "label_Inside_CE";
            label_Inside_CE.Size = new Size(72, 33);
            label_Inside_CE.TabIndex = 16;
            label_Inside_CE.Text = "里面CE：";
            label_Inside_CE.TextAlign = ContentAlignment.MiddleRight;
            // 
            // panel10
            // 
            panel10.Back = Color.Transparent;
            panel10.BackColor = Color.Transparent;
            panel10.Controls.Add(input_Inside_OP);
            panel10.Controls.Add(label_Inside_OP);
            panel10.Dock = DockStyle.Top;
            panel10.Location = new Point(9, 138);
            panel10.Name = "panel10";
            panel10.Radius = 0;
            panel10.Size = new Size(342, 33);
            panel10.TabIndex = 17;
            // 
            // input_Inside_OP
            // 
            input_Inside_OP.Font = new Font("Microsoft YaHei UI", 10F);
            input_Inside_OP.Location = new Point(78, -1);
            input_Inside_OP.Name = "input_Inside_OP";
            input_Inside_OP.Size = new Size(261, 34);
            input_Inside_OP.TabIndex = 17;
            // 
            // label_Inside_OP
            // 
            label_Inside_OP.BackColor = Color.Transparent;
            label_Inside_OP.Dock = DockStyle.Left;
            label_Inside_OP.Font = new Font("Microsoft YaHei UI", 10F);
            label_Inside_OP.Location = new Point(0, 0);
            label_Inside_OP.Name = "label_Inside_OP";
            label_Inside_OP.Size = new Size(72, 33);
            label_Inside_OP.TabIndex = 16;
            label_Inside_OP.Text = "里面OP：";
            label_Inside_OP.TextAlign = ContentAlignment.MiddleRight;
            // 
            // panel9
            // 
            panel9.Back = Color.Transparent;
            panel9.BackColor = Color.Transparent;
            panel9.Controls.Add(input_Surface_DR);
            panel9.Controls.Add(label_Surface_DR);
            panel9.Dock = DockStyle.Top;
            panel9.Location = new Point(9, 105);
            panel9.Name = "panel9";
            panel9.Radius = 0;
            panel9.Size = new Size(342, 33);
            panel9.TabIndex = 16;
            // 
            // input_Surface_DR
            // 
            input_Surface_DR.Font = new Font("Microsoft YaHei UI", 10F);
            input_Surface_DR.Location = new Point(78, -1);
            input_Surface_DR.Name = "input_Surface_DR";
            input_Surface_DR.Size = new Size(261, 34);
            input_Surface_DR.TabIndex = 17;
            // 
            // label_Surface_DR
            // 
            label_Surface_DR.BackColor = Color.Transparent;
            label_Surface_DR.Dock = DockStyle.Left;
            label_Surface_DR.Font = new Font("Microsoft YaHei UI", 10F);
            label_Surface_DR.Location = new Point(0, 0);
            label_Surface_DR.Name = "label_Surface_DR";
            label_Surface_DR.Size = new Size(72, 33);
            label_Surface_DR.TabIndex = 16;
            label_Surface_DR.Text = "表面DR：";
            label_Surface_DR.TextAlign = ContentAlignment.MiddleRight;
            // 
            // panel7
            // 
            panel7.Back = Color.Transparent;
            panel7.BackColor = Color.Transparent;
            panel7.Controls.Add(input_Surface_CE);
            panel7.Controls.Add(label_Surface_CE);
            panel7.Dock = DockStyle.Top;
            panel7.Location = new Point(9, 72);
            panel7.Name = "panel7";
            panel7.Radius = 0;
            panel7.Size = new Size(342, 33);
            panel7.TabIndex = 15;
            // 
            // input_Surface_CE
            // 
            input_Surface_CE.Font = new Font("Microsoft YaHei UI", 10F);
            input_Surface_CE.Location = new Point(78, -1);
            input_Surface_CE.Name = "input_Surface_CE";
            input_Surface_CE.Size = new Size(261, 34);
            input_Surface_CE.TabIndex = 17;
            // 
            // label_Surface_CE
            // 
            label_Surface_CE.BackColor = Color.Transparent;
            label_Surface_CE.Dock = DockStyle.Left;
            label_Surface_CE.Font = new Font("Microsoft YaHei UI", 10F);
            label_Surface_CE.Location = new Point(0, 0);
            label_Surface_CE.Name = "label_Surface_CE";
            label_Surface_CE.Size = new Size(72, 33);
            label_Surface_CE.TabIndex = 16;
            label_Surface_CE.Text = "表面CE：";
            label_Surface_CE.TextAlign = ContentAlignment.MiddleRight;
            // 
            // panel3
            // 
            panel3.Back = Color.Transparent;
            panel3.BackColor = Color.Transparent;
            panel3.Controls.Add(input_Surface_OP);
            panel3.Controls.Add(label_Surface_OP);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(9, 39);
            panel3.Name = "panel3";
            panel3.Radius = 0;
            panel3.Size = new Size(342, 33);
            panel3.TabIndex = 14;
            // 
            // input_Surface_OP
            // 
            input_Surface_OP.Font = new Font("Microsoft YaHei UI", 10F);
            input_Surface_OP.Location = new Point(78, -1);
            input_Surface_OP.Name = "input_Surface_OP";
            input_Surface_OP.Size = new Size(261, 34);
            input_Surface_OP.TabIndex = 17;
            // 
            // label_Surface_OP
            // 
            label_Surface_OP.BackColor = Color.Transparent;
            label_Surface_OP.Dock = DockStyle.Left;
            label_Surface_OP.Font = new Font("Microsoft YaHei UI", 10F);
            label_Surface_OP.Location = new Point(0, 0);
            label_Surface_OP.Name = "label_Surface_OP";
            label_Surface_OP.Size = new Size(72, 33);
            label_Surface_OP.TabIndex = 16;
            label_Surface_OP.Text = "表面OP：";
            label_Surface_OP.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            label1.BackColor = Color.Transparent;
            label1.Dock = DockStyle.Top;
            label1.Font = new Font("Microsoft YaHei UI", 15F, FontStyle.Bold);
            label1.ForeColor = Color.DarkGray;
            label1.Location = new Point(9, 9);
            label1.Name = "label1";
            label1.Size = new Size(342, 30);
            label1.TabIndex = 12;
            label1.Text = "结 果 判 定 区";
            label1.TextAlign = ContentAlignment.TopCenter;
            // 
            // blacknessMethod_OpenFileDialog
            // 
            blacknessMethod_OpenFileDialog.FileName = "file";
            // 
            // blacknessMethod_FileSystemWatcher
            // 
            blacknessMethod_FileSystemWatcher.EnableRaisingEvents = true;
            blacknessMethod_FileSystemWatcher.SynchronizingObject = this;
            // 
            // BlacknessMethod
            // 
            Controls.Add(flowLayoutPanel1);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 16F);
            Name = "BlacknessMethod";
            Size = new Size(1300, 634);
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel5.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            panel6.ResumeLayout(false);
            panel17.ResumeLayout(false);
            panel16.ResumeLayout(false);
            panel15.ResumeLayout(false);
            panel14.ResumeLayout(false);
            panel8.ResumeLayout(false);
            panel13.ResumeLayout(false);
            panel13.PerformLayout();
            panel12.ResumeLayout(false);
            panel11.ResumeLayout(false);
            panel10.ResumeLayout(false);
            panel9.ResumeLayout(false);
            panel7.ResumeLayout(false);
            panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)blacknessMethod_FileSystemWatcher).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private AntdUI.PageHeader header1;
        private AntdUI.Panel panel1;
        private Label blacknessMethod_RenderImage_Text;
        private Label blacknessMethod_RenderImage_Zone;
        private AntdUI.Avatar blacknessMethod_RenderImage;
        private AntdUI.Panel panel2;
        private AntdUI.Button btn_Predict;
        private AntdUI.Panel panel4;
        private Label blacknessMethod_OriginImage_Text;
        private Label blacknessMethod_OriginImage_Zone;
        private AntdUI.Avatar avatarOriginImage;
        private AntdUI.Panel panel5;
        private AntdUI.Button btn_UploadImage;
        private FlowLayoutPanel flowLayoutPanel1;
        private AntdUI.Panel panel6;
        private OpenFileDialog blacknessMethod_OpenFileDialog;
        private System.IO.FileSystemWatcher blacknessMethod_FileSystemWatcher;
        private AntdUI.Button btn_CameraCapture;
        private Label label1;
        private AntdUI.Panel panel3;
        private AntdUI.Button btn_Save;
        private AntdUI.Button button7;
        private AntdUI.Panel panel12;
        private AntdUI.Button button6;
        private AntdUI.Panel panel11;
        private AntdUI.Button button5;
        private AntdUI.Panel panel10;
        private AntdUI.Button button4;
        private AntdUI.Panel panel9;
        private AntdUI.Button button3;
        private AntdUI.Panel panel7;
        private AntdUI.Button button2;
        private Label label_Surface_OP;
        private AntdUI.Input input_Surface_OP;
        private AntdUI.Input input_Inside_DR;
        private Label label_Inside_DR;
        private AntdUI.Input input_Inside_CE;
        private Label label_Inside_CE;
        private AntdUI.Input input_Inside_OP;
        private Label label_Inside_OP;
        private AntdUI.Input input_Surface_DR;
        private Label label_Surface_DR;
        private AntdUI.Input input_Surface_CE;
        private Label label_Surface_CE;
        private AntdUI.Panel panel13;
        private AntdUI.Radio radio_Result_OK;
        private AntdUI.Radio radio_Result_NG;
        private AntdUI.Divider divider2;
        private AntdUI.Panel panel16;
        private AntdUI.Input input_Size;
        private Label label_Size;
        private AntdUI.Panel panel15;
        private AntdUI.Input input_Coil_Number;
        private Label label_Coil_Number;
        private AntdUI.Panel panel14;
        private Label label_Analyst;
        private AntdUI.Panel panel8;
        private Label label_Work_Group;
        private Label label12;
        private AntdUI.Panel panel17;
        private AntdUI.Select select_Work_Group;
        private AntdUI.Select select_Analyst;
        private AntdUI.Button btnCameraSetting;
    }
}