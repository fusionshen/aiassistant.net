// COPYRIGHT (C) Tom. ALL RIGHTS RESERVED.
// THE AntdUI PROJECT IS AN WINFORM LIBRARY LICENSED UNDER THE Apache-2.0 License.
// LICENSED UNDER THE Apache License, VERSION 2.0 (THE "License")
// YOU MAY NOT USE THIS FILE EXCEPT IN COMPLIANCE WITH THE License.
// YOU MAY OBTAIN A COPY OF THE LICENSE AT
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// UNLESS REQUIRED BY APPLICABLE LAW OR AGREED TO IN WRITING, SOFTWARE
// DISTRIBUTED UNDER THE LICENSE IS DISTRIBUTED ON AN "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED.
// SEE THE LICENSE FOR THE SPECIFIC LANGUAGE GOVERNING PERMISSIONS AND
// LIMITATIONS UNDER THE License.
// GITEE: https://gitee.com/antdui/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

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
            blacknessMethod_OriginImage = new AntdUI.Avatar();
            panel5 = new AntdUI.Panel();
            btn_CameraCapture = new AntdUI.Button();
            btn_UploadImage = new AntdUI.Button();
            blacknessMethod_OriginImage_Zone = new Label();
            flowLayoutPanel1 = new FlowLayoutPanel();
            panel6 = new AntdUI.Panel();
            label12 = new Label();
            panel17 = new AntdUI.Panel();
            btn_Save = new AntdUI.Button();
            panel16 = new AntdUI.Panel();
            input10 = new AntdUI.Input();
            label11 = new Label();
            panel15 = new AntdUI.Panel();
            input9 = new AntdUI.Input();
            label10 = new Label();
            panel14 = new AntdUI.Panel();
            select1 = new AntdUI.Select();
            label9 = new Label();
            panel8 = new AntdUI.Panel();
            select5 = new AntdUI.Select();
            label8 = new Label();
            divider2 = new AntdUI.Divider();
            panel13 = new AntdUI.Panel();
            radio9 = new AntdUI.Radio();
            radio12 = new AntdUI.Radio();
            panel12 = new AntdUI.Panel();
            input6 = new AntdUI.Input();
            label7 = new Label();
            panel11 = new AntdUI.Panel();
            input5 = new AntdUI.Input();
            label6 = new Label();
            panel10 = new AntdUI.Panel();
            input4 = new AntdUI.Input();
            label5 = new Label();
            panel9 = new AntdUI.Panel();
            input3 = new AntdUI.Input();
            label4 = new Label();
            panel7 = new AntdUI.Panel();
            input2 = new AntdUI.Input();
            label3 = new Label();
            panel3 = new AntdUI.Panel();
            input1 = new AntdUI.Input();
            label2 = new Label();
            label1 = new Label();
            divider1 = new AntdUI.Divider();
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
            blacknessMethod_RenderImage.Image = Properties.Resources.img1;
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
            panel4.Controls.Add(blacknessMethod_OriginImage);
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
            // blacknessMethod_OriginImage
            // 
            blacknessMethod_OriginImage.Dock = DockStyle.Top;
            blacknessMethod_OriginImage.Image = Properties.Resources.img1;
            blacknessMethod_OriginImage.Location = new Point(9, 39);
            blacknessMethod_OriginImage.Name = "blacknessMethod_OriginImage";
            blacknessMethod_OriginImage.Radius = 6;
            blacknessMethod_OriginImage.Size = new Size(422, 455);
            blacknessMethod_OriginImage.TabIndex = 9;
            blacknessMethod_OriginImage.Click += OriginImage_Click;
            // 
            // panel5
            // 
            panel5.Back = Color.Transparent;
            panel5.BackColor = Color.Transparent;
            panel5.Controls.Add(btn_CameraCapture);
            panel5.Controls.Add(btn_UploadImage);
            panel5.Dock = DockStyle.Bottom;
            panel5.Location = new Point(9, 535);
            panel5.Name = "panel5";
            panel5.Radius = 0;
            panel5.Size = new Size(422, 40);
            panel5.TabIndex = 13;
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
            panel6.Controls.Add(divider1);
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
            label12.Location = new Point(9, 437);
            label12.Name = "label12";
            label12.Padding = new Padding(2, 0, 2, 0);
            label12.Size = new Size(342, 98);
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
            panel16.Controls.Add(input10);
            panel16.Controls.Add(label11);
            panel16.Dock = DockStyle.Top;
            panel16.Location = new Point(9, 404);
            panel16.Name = "panel16";
            panel16.Radius = 0;
            panel16.Size = new Size(342, 33);
            panel16.TabIndex = 25;
            // 
            // input10
            // 
            input10.Font = new Font("Microsoft YaHei UI", 10F);
            input10.Location = new Point(78, -1);
            input10.Name = "input10";
            input10.Size = new Size(261, 34);
            input10.TabIndex = 17;
            input10.Text = "input10";
            // 
            // label11
            // 
            label11.BackColor = Color.Transparent;
            label11.Dock = DockStyle.Left;
            label11.Font = new Font("Microsoft YaHei UI", 10F);
            label11.Location = new Point(0, 0);
            label11.Name = "label11";
            label11.Size = new Size(72, 33);
            label11.TabIndex = 16;
            label11.Text = "尺寸：";
            label11.TextAlign = ContentAlignment.MiddleRight;
            // 
            // panel15
            // 
            panel15.Back = Color.Transparent;
            panel15.BackColor = Color.Transparent;
            panel15.Controls.Add(input9);
            panel15.Controls.Add(label10);
            panel15.Dock = DockStyle.Top;
            panel15.Location = new Point(9, 371);
            panel15.Name = "panel15";
            panel15.Radius = 0;
            panel15.Size = new Size(342, 33);
            panel15.TabIndex = 24;
            // 
            // input9
            // 
            input9.Font = new Font("Microsoft YaHei UI", 10F);
            input9.Location = new Point(78, -1);
            input9.Name = "input9";
            input9.Size = new Size(261, 34);
            input9.TabIndex = 17;
            input9.Text = "input9";
            // 
            // label10
            // 
            label10.BackColor = Color.Transparent;
            label10.Dock = DockStyle.Left;
            label10.Font = new Font("Microsoft YaHei UI", 10F);
            label10.Location = new Point(0, 0);
            label10.Name = "label10";
            label10.Size = new Size(72, 33);
            label10.TabIndex = 16;
            label10.Text = "钢卷号：";
            label10.TextAlign = ContentAlignment.MiddleRight;
            // 
            // panel14
            // 
            panel14.Back = Color.Transparent;
            panel14.BackColor = Color.Transparent;
            panel14.Controls.Add(select1);
            panel14.Controls.Add(label9);
            panel14.Dock = DockStyle.Top;
            panel14.Location = new Point(9, 338);
            panel14.Name = "panel14";
            panel14.Radius = 0;
            panel14.Size = new Size(342, 33);
            panel14.TabIndex = 23;
            // 
            // select1
            // 
            select1.AllowClear = true;
            select1.Dock = DockStyle.Left;
            select1.DropDownArrow = true;
            select1.Font = new Font("Microsoft YaHei UI", 10F);
            select1.Items.AddRange(new object[] { "Lucy", "Tom", "AduSkin", "WangLi", "HUAWEI", "XIAOMI" });
            select1.LocalizationPlaceholderText = "Select.{id}";
            select1.Location = new Point(72, 0);
            select1.Name = "select1";
            select1.Padding = new Padding(5, 0, 0, 0);
            select1.PlaceholderText = "请选择分析人员";
            select1.Size = new Size(267, 33);
            select1.TabIndex = 18;
            // 
            // label9
            // 
            label9.BackColor = Color.Transparent;
            label9.Dock = DockStyle.Left;
            label9.Font = new Font("Microsoft YaHei UI", 10F);
            label9.Location = new Point(0, 0);
            label9.Name = "label9";
            label9.Size = new Size(72, 33);
            label9.TabIndex = 16;
            label9.Text = "分析人：";
            label9.TextAlign = ContentAlignment.MiddleRight;
            // 
            // panel8
            // 
            panel8.Back = Color.Transparent;
            panel8.BackColor = Color.Transparent;
            panel8.Controls.Add(select5);
            panel8.Controls.Add(label8);
            panel8.Dock = DockStyle.Top;
            panel8.Location = new Point(9, 305);
            panel8.Name = "panel8";
            panel8.Radius = 0;
            panel8.Size = new Size(342, 33);
            panel8.TabIndex = 22;
            // 
            // select5
            // 
            select5.AllowClear = true;
            select5.Dock = DockStyle.Left;
            select5.DropDownArrow = true;
            select5.Font = new Font("Microsoft YaHei UI", 10F);
            select5.Items.AddRange(new object[] { "Lucy", "Tom", "AduSkin", "WangLi", "HUAWEI", "XIAOMI" });
            select5.LocalizationPlaceholderText = "Select.{id}";
            select5.Location = new Point(72, 0);
            select5.Name = "select5";
            select5.Padding = new Padding(5, 0, 0, 0);
            select5.PlaceholderText = "请选择班组";
            select5.Size = new Size(267, 33);
            select5.TabIndex = 17;
            // 
            // label8
            // 
            label8.BackColor = Color.Transparent;
            label8.Dock = DockStyle.Left;
            label8.Font = new Font("Microsoft YaHei UI", 10F);
            label8.Location = new Point(0, 0);
            label8.Name = "label8";
            label8.Size = new Size(72, 33);
            label8.TabIndex = 16;
            label8.Text = "班组：";
            label8.TextAlign = ContentAlignment.MiddleRight;
            // 
            // divider2
            // 
            divider2.BackColor = Color.Transparent;
            divider2.Dock = DockStyle.Top;
            divider2.Font = new Font("Microsoft YaHei UI", 16F, FontStyle.Bold);
            divider2.ForeColor = Color.DarkGray;
            divider2.Location = new Point(9, 271);
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
            panel13.Controls.Add(radio9);
            panel13.Controls.Add(radio12);
            panel13.Dock = DockStyle.Top;
            panel13.Location = new Point(9, 238);
            panel13.Name = "panel13";
            panel13.Radius = 0;
            panel13.RightToLeft = RightToLeft.No;
            panel13.Size = new Size(342, 33);
            panel13.TabIndex = 20;
            // 
            // radio9
            // 
            radio9.AutoCheck = true;
            radio9.AutoSizeMode = AntdUI.TAutoSize.Width;
            radio9.Checked = true;
            radio9.Fill = Color.FromArgb(100, 0, 0);
            radio9.Font = new Font("Microsoft YaHei UI", 12F);
            radio9.ForeColor = Color.Red;
            radio9.Location = new Point(180, 0);
            radio9.Name = "radio9";
            radio9.Size = new Size(74, 33);
            radio9.TabIndex = 18;
            radio9.Text = "NG";
            // 
            // radio12
            // 
            radio12.AutoCheck = true;
            radio12.AutoSizeMode = AntdUI.TAutoSize.Width;
            radio12.BackColor = Color.Transparent;
            radio12.Fill = Color.FromArgb(250, 0, 0);
            radio12.Font = new Font("Microsoft YaHei UI", 12F);
            radio12.ForeColor = Color.Green;
            radio12.Location = new Point(90, 0);
            radio12.Name = "radio12";
            radio12.Size = new Size(73, 33);
            radio12.TabIndex = 17;
            radio12.Text = "OK";
            // 
            // panel12
            // 
            panel12.Back = Color.Transparent;
            panel12.BackColor = Color.Transparent;
            panel12.Controls.Add(input6);
            panel12.Controls.Add(label7);
            panel12.Dock = DockStyle.Top;
            panel12.Location = new Point(9, 205);
            panel12.Name = "panel12";
            panel12.Radius = 0;
            panel12.Size = new Size(342, 33);
            panel12.TabIndex = 19;
            // 
            // input6
            // 
            input6.Font = new Font("Microsoft YaHei UI", 10F);
            input6.Location = new Point(78, -1);
            input6.Name = "input6";
            input6.Size = new Size(261, 34);
            input6.TabIndex = 17;
            input6.Text = "input6";
            // 
            // label7
            // 
            label7.BackColor = Color.Transparent;
            label7.Dock = DockStyle.Left;
            label7.Font = new Font("Microsoft YaHei UI", 10F);
            label7.Location = new Point(0, 0);
            label7.Name = "label7";
            label7.Size = new Size(72, 33);
            label7.TabIndex = 16;
            label7.Text = "里面DR：";
            label7.TextAlign = ContentAlignment.MiddleRight;
            // 
            // panel11
            // 
            panel11.Back = Color.Transparent;
            panel11.BackColor = Color.Transparent;
            panel11.Controls.Add(input5);
            panel11.Controls.Add(label6);
            panel11.Dock = DockStyle.Top;
            panel11.Location = new Point(9, 172);
            panel11.Name = "panel11";
            panel11.Radius = 0;
            panel11.Size = new Size(342, 33);
            panel11.TabIndex = 18;
            // 
            // input5
            // 
            input5.Font = new Font("Microsoft YaHei UI", 10F);
            input5.Location = new Point(78, -1);
            input5.Name = "input5";
            input5.Size = new Size(261, 34);
            input5.TabIndex = 17;
            input5.Text = "input5";
            // 
            // label6
            // 
            label6.BackColor = Color.Transparent;
            label6.Dock = DockStyle.Left;
            label6.Font = new Font("Microsoft YaHei UI", 10F);
            label6.Location = new Point(0, 0);
            label6.Name = "label6";
            label6.Size = new Size(72, 33);
            label6.TabIndex = 16;
            label6.Text = "里面CE：";
            label6.TextAlign = ContentAlignment.MiddleRight;
            // 
            // panel10
            // 
            panel10.Back = Color.Transparent;
            panel10.BackColor = Color.Transparent;
            panel10.Controls.Add(input4);
            panel10.Controls.Add(label5);
            panel10.Dock = DockStyle.Top;
            panel10.Location = new Point(9, 139);
            panel10.Name = "panel10";
            panel10.Radius = 0;
            panel10.Size = new Size(342, 33);
            panel10.TabIndex = 17;
            // 
            // input4
            // 
            input4.Font = new Font("Microsoft YaHei UI", 10F);
            input4.Location = new Point(78, -1);
            input4.Name = "input4";
            input4.Size = new Size(261, 34);
            input4.TabIndex = 17;
            input4.Text = "input4";
            // 
            // label5
            // 
            label5.BackColor = Color.Transparent;
            label5.Dock = DockStyle.Left;
            label5.Font = new Font("Microsoft YaHei UI", 10F);
            label5.Location = new Point(0, 0);
            label5.Name = "label5";
            label5.Size = new Size(72, 33);
            label5.TabIndex = 16;
            label5.Text = "里面OP：";
            label5.TextAlign = ContentAlignment.MiddleRight;
            // 
            // panel9
            // 
            panel9.Back = Color.Transparent;
            panel9.BackColor = Color.Transparent;
            panel9.Controls.Add(input3);
            panel9.Controls.Add(label4);
            panel9.Dock = DockStyle.Top;
            panel9.Location = new Point(9, 106);
            panel9.Name = "panel9";
            panel9.Radius = 0;
            panel9.Size = new Size(342, 33);
            panel9.TabIndex = 16;
            // 
            // input3
            // 
            input3.Font = new Font("Microsoft YaHei UI", 10F);
            input3.Location = new Point(78, -1);
            input3.Name = "input3";
            input3.Size = new Size(261, 34);
            input3.TabIndex = 17;
            input3.Text = "input3";
            // 
            // label4
            // 
            label4.BackColor = Color.Transparent;
            label4.Dock = DockStyle.Left;
            label4.Font = new Font("Microsoft YaHei UI", 10F);
            label4.Location = new Point(0, 0);
            label4.Name = "label4";
            label4.Size = new Size(72, 33);
            label4.TabIndex = 16;
            label4.Text = "表面DR：";
            label4.TextAlign = ContentAlignment.MiddleRight;
            // 
            // panel7
            // 
            panel7.Back = Color.Transparent;
            panel7.BackColor = Color.Transparent;
            panel7.Controls.Add(input2);
            panel7.Controls.Add(label3);
            panel7.Dock = DockStyle.Top;
            panel7.Location = new Point(9, 73);
            panel7.Name = "panel7";
            panel7.Radius = 0;
            panel7.Size = new Size(342, 33);
            panel7.TabIndex = 15;
            // 
            // input2
            // 
            input2.Font = new Font("Microsoft YaHei UI", 10F);
            input2.Location = new Point(78, -1);
            input2.Name = "input2";
            input2.Size = new Size(261, 34);
            input2.TabIndex = 17;
            input2.Text = "input2";
            // 
            // label3
            // 
            label3.BackColor = Color.Transparent;
            label3.Dock = DockStyle.Left;
            label3.Font = new Font("Microsoft YaHei UI", 10F);
            label3.Location = new Point(0, 0);
            label3.Name = "label3";
            label3.Size = new Size(72, 33);
            label3.TabIndex = 16;
            label3.Text = "表面CE：";
            label3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // panel3
            // 
            panel3.Back = Color.Transparent;
            panel3.BackColor = Color.Transparent;
            panel3.Controls.Add(input1);
            panel3.Controls.Add(label2);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(9, 40);
            panel3.Name = "panel3";
            panel3.Radius = 0;
            panel3.Size = new Size(342, 33);
            panel3.TabIndex = 14;
            // 
            // input1
            // 
            input1.Font = new Font("Microsoft YaHei UI", 10F);
            input1.Location = new Point(78, -1);
            input1.Name = "input1";
            input1.Size = new Size(261, 34);
            input1.TabIndex = 17;
            input1.Text = "input1";
            // 
            // label2
            // 
            label2.BackColor = Color.Transparent;
            label2.Dock = DockStyle.Left;
            label2.Font = new Font("Microsoft YaHei UI", 10F);
            label2.Location = new Point(0, 0);
            label2.Name = "label2";
            label2.Size = new Size(72, 33);
            label2.TabIndex = 16;
            label2.Text = "表面OP：";
            label2.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            label1.BackColor = Color.Transparent;
            label1.Dock = DockStyle.Top;
            label1.Font = new Font("Microsoft YaHei UI", 15F, FontStyle.Bold);
            label1.ForeColor = Color.DarkGray;
            label1.Location = new Point(9, 10);
            label1.Name = "label1";
            label1.Size = new Size(342, 30);
            label1.TabIndex = 12;
            label1.Text = "结 果 判 定 区";
            label1.TextAlign = ContentAlignment.TopCenter;
            // 
            // divider1
            // 
            divider1.BackColor = Color.Transparent;
            divider1.Dock = DockStyle.Top;
            divider1.Location = new Point(9, 9);
            divider1.Margin = new Padding(10);
            divider1.Name = "divider1";
            divider1.Size = new Size(342, 1);
            divider1.TabIndex = 1;
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
        private AntdUI.Avatar blacknessMethod_OriginImage;
        private AntdUI.Panel panel5;
        private AntdUI.Button btn_UploadImage;
        private FlowLayoutPanel flowLayoutPanel1;
        private AntdUI.Panel panel6;
        private AntdUI.Divider divider1;
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
        private Label label2;
        private AntdUI.Input input1;
        private AntdUI.Input input6;
        private Label label7;
        private AntdUI.Input input5;
        private Label label6;
        private AntdUI.Input input4;
        private Label label5;
        private AntdUI.Input input3;
        private Label label4;
        private AntdUI.Input input2;
        private Label label3;
        private AntdUI.Panel panel13;
        private AntdUI.Radio radio12;
        private AntdUI.Radio radio9;
        private AntdUI.Divider divider2;
        private AntdUI.Panel panel16;
        private AntdUI.Input input10;
        private Label label11;
        private AntdUI.Panel panel15;
        private AntdUI.Input input9;
        private Label label10;
        private AntdUI.Panel panel14;
        private Label label9;
        private AntdUI.Panel panel8;
        private Label label8;
        private Label label12;
        private AntdUI.Panel panel17;
        private AntdUI.Select select5;
        private AntdUI.Select select1;
    }
}