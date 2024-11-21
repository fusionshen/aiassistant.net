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
            blacknessMethod_RenderImage_Zone = new Label();
            blacknessMethod_RenderImage = new AntdUI.Avatar();
            panel2 = new AntdUI.Panel();
            btn_Predict = new AntdUI.Button();
            panel4 = new AntdUI.Panel();
            blacknessMethod_OriginImage_Text = new Label();
            blacknessMethod_OriginImage_Zone = new Label();
            blacknessMethod_OriginImage = new AntdUI.Avatar();
            panel5 = new AntdUI.Panel();
            btn_CameraCapture = new AntdUI.Button();
            btn_UploadImage = new AntdUI.Button();
            flowLayoutPanel1 = new FlowLayoutPanel();
            panel6 = new AntdUI.Panel();
            label5 = new Label();
            divider1 = new AntdUI.Divider();
            label6 = new Label();
            panel8 = new AntdUI.Panel();
            label9 = new Label();
            divider4 = new AntdUI.Divider();
            label10 = new Label();
            blacknessMethod_OpenFileDialog = new OpenFileDialog();
            blacknessMethod_FileSystemWatcher = new System.IO.FileSystemWatcher();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel4.SuspendLayout();
            panel5.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            panel6.SuspendLayout();
            panel8.SuspendLayout();
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
            panel1.ArrowAlign = AntdUI.TAlign.Left;
            panel1.ArrowSize = 10;
            panel1.Controls.Add(blacknessMethod_RenderImage_Text);
            panel1.Controls.Add(blacknessMethod_RenderImage_Zone);
            panel1.Controls.Add(blacknessMethod_RenderImage);
            panel1.Controls.Add(panel2);
            panel1.Location = new Point(372, 3);
            panel1.Name = "panel1";
            panel1.Padding = new Padding(7);
            panel1.Radius = 10;
            panel1.Shadow = 24;
            panel1.ShadowOpacityAnimation = true;
            panel1.Size = new Size(363, 557);
            panel1.TabIndex = 20;
            // 
            // blacknessMethod_RenderImage_Text
            // 
            blacknessMethod_RenderImage_Text.BackColor = Color.Transparent;
            blacknessMethod_RenderImage_Text.Dock = DockStyle.Fill;
            blacknessMethod_RenderImage_Text.Font = new Font("Microsoft YaHei UI", 10F);
            blacknessMethod_RenderImage_Text.Location = new Point(31, 421);
            blacknessMethod_RenderImage_Text.Name = "blacknessMethod_RenderImage_Text";
            blacknessMethod_RenderImage_Text.Padding = new Padding(2, 0, 2, 0);
            blacknessMethod_RenderImage_Text.Size = new Size(301, 65);
            blacknessMethod_RenderImage_Text.TabIndex = 12;
            blacknessMethod_RenderImage_Text.Text = "点击识别后的图片可进行放大预览，各部位识别结果会在右侧结果判定区展示。";
            // 
            // blacknessMethod_RenderImage_Zone
            // 
            blacknessMethod_RenderImage_Zone.BackColor = Color.Transparent;
            blacknessMethod_RenderImage_Zone.Dock = DockStyle.Top;
            blacknessMethod_RenderImage_Zone.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
            blacknessMethod_RenderImage_Zone.Location = new Point(31, 391);
            blacknessMethod_RenderImage_Zone.Name = "blacknessMethod_RenderImage_Zone";
            blacknessMethod_RenderImage_Zone.Size = new Size(301, 30);
            blacknessMethod_RenderImage_Zone.TabIndex = 11;
            blacknessMethod_RenderImage_Zone.Text = "识别展示区";
            blacknessMethod_RenderImage_Zone.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // blacknessMethod_RenderImage
            // 
            blacknessMethod_RenderImage.Dock = DockStyle.Top;
            blacknessMethod_RenderImage.Image = Properties.Resources.img1;
            blacknessMethod_RenderImage.Location = new Point(31, 31);
            blacknessMethod_RenderImage.Name = "blacknessMethod_RenderImage";
            blacknessMethod_RenderImage.Radius = 6;
            blacknessMethod_RenderImage.Size = new Size(301, 360);
            blacknessMethod_RenderImage.TabIndex = 9;
            blacknessMethod_RenderImage.Click += BlacknessMethod_renderImage_Click;
            // 
            // panel2
            // 
            panel2.Back = Color.Transparent;
            panel2.BackColor = Color.Transparent;
            panel2.Controls.Add(btn_Predict);
            panel2.Dock = DockStyle.Bottom;
            panel2.Location = new Point(31, 486);
            panel2.Name = "panel2";
            panel2.Radius = 0;
            panel2.Size = new Size(301, 40);
            panel2.TabIndex = 13;
            // 
            // btn_Predict
            // 
            btn_Predict.Dock = DockStyle.Right;
            btn_Predict.Font = new Font("Microsoft YaHei UI", 10F);
            btn_Predict.LocalizationText = "Predict";
            btn_Predict.Location = new Point(228, 0);
            btn_Predict.Name = "btn_Predict";
            btn_Predict.Size = new Size(73, 40);
            btn_Predict.TabIndex = 0;
            btn_Predict.Text = "识别";
            btn_Predict.Type = AntdUI.TTypeMini.Primary;
            btn_Predict.Click += Btn_Predict_Click;
            // 
            // panel4
            // 
            panel4.ArrowAlign = AntdUI.TAlign.Left;
            panel4.ArrowSize = 10;
            panel4.Controls.Add(blacknessMethod_OriginImage_Text);
            panel4.Controls.Add(blacknessMethod_OriginImage_Zone);
            panel4.Controls.Add(blacknessMethod_OriginImage);
            panel4.Controls.Add(panel5);
            panel4.Location = new Point(3, 3);
            panel4.Name = "panel4";
            panel4.Padding = new Padding(7);
            panel4.Radius = 10;
            panel4.Shadow = 24;
            panel4.ShadowOpacityAnimation = true;
            panel4.Size = new Size(363, 557);
            panel4.TabIndex = 19;
            // 
            // blacknessMethod_OriginImage_Text
            // 
            blacknessMethod_OriginImage_Text.BackColor = Color.Transparent;
            blacknessMethod_OriginImage_Text.Dock = DockStyle.Fill;
            blacknessMethod_OriginImage_Text.Font = new Font("Microsoft YaHei UI", 10F);
            blacknessMethod_OriginImage_Text.Location = new Point(31, 421);
            blacknessMethod_OriginImage_Text.Name = "blacknessMethod_OriginImage_Text";
            blacknessMethod_OriginImage_Text.Padding = new Padding(2, 0, 2, 0);
            blacknessMethod_OriginImage_Text.Size = new Size(301, 65);
            blacknessMethod_OriginImage_Text.TabIndex = 12;
            blacknessMethod_OriginImage_Text.Text = "选择本地图片上传或者调用黑度检测工作台摄像头拍摄照片。";
            // 
            // blacknessMethod_OriginImage_Zone
            // 
            blacknessMethod_OriginImage_Zone.BackColor = Color.Transparent;
            blacknessMethod_OriginImage_Zone.Dock = DockStyle.Top;
            blacknessMethod_OriginImage_Zone.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
            blacknessMethod_OriginImage_Zone.Location = new Point(31, 391);
            blacknessMethod_OriginImage_Zone.Name = "blacknessMethod_OriginImage_Zone";
            blacknessMethod_OriginImage_Zone.Size = new Size(301, 30);
            blacknessMethod_OriginImage_Zone.TabIndex = 11;
            blacknessMethod_OriginImage_Zone.Text = "原图展示区";
            blacknessMethod_OriginImage_Zone.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // blacknessMethod_OriginImage
            // 
            blacknessMethod_OriginImage.Dock = DockStyle.Top;
            blacknessMethod_OriginImage.Image = Properties.Resources.img1;
            blacknessMethod_OriginImage.Location = new Point(31, 31);
            blacknessMethod_OriginImage.Name = "blacknessMethod_OriginImage";
            blacknessMethod_OriginImage.Radius = 6;
            blacknessMethod_OriginImage.Size = new Size(301, 360);
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
            panel5.Location = new Point(31, 486);
            panel5.Name = "panel5";
            panel5.Radius = 0;
            panel5.Size = new Size(301, 40);
            panel5.TabIndex = 13;
            // 
            // btn_CameraCapture
            // 
            btn_CameraCapture.Dock = DockStyle.Right;
            btn_CameraCapture.Font = new Font("Microsoft YaHei UI", 10F);
            btn_CameraCapture.LocalizationText = "CameraCapture";
            btn_CameraCapture.Location = new Point(71, 0);
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
            btn_UploadImage.Location = new Point(190, 0);
            btn_UploadImage.Name = "btn_UploadImage";
            btn_UploadImage.Size = new Size(111, 40);
            btn_UploadImage.TabIndex = 0;
            btn_UploadImage.Text = "选择本地图片";
            btn_UploadImage.Type = AntdUI.TTypeMini.Primary;
            btn_UploadImage.Click += Btn_UploadImage_Click;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.Controls.Add(panel4);
            flowLayoutPanel1.Controls.Add(panel1);
            flowLayoutPanel1.Controls.Add(panel6);
            flowLayoutPanel1.Controls.Add(panel8);
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Location = new Point(0, 74);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(1300, 560);
            flowLayoutPanel1.TabIndex = 5;
            // 
            // panel6
            // 
            panel6.ArrowAlign = AntdUI.TAlign.Left;
            panel6.ArrowSize = 10;
            panel6.Controls.Add(label5);
            panel6.Controls.Add(divider1);
            panel6.Controls.Add(label6);
            panel6.Location = new Point(741, 3);
            panel6.Name = "panel6";
            panel6.Radius = 0;
            panel6.Shadow = 24;
            panel6.ShadowOpacity = 0.18F;
            panel6.ShadowOpacityAnimation = true;
            panel6.Size = new Size(269, 557);
            panel6.TabIndex = 20;
            // 
            // label5
            // 
            label5.BackColor = Color.Transparent;
            label5.Dock = DockStyle.Fill;
            label5.Font = new Font("Microsoft YaHei UI", 11F);
            label5.Location = new Point(24, 73);
            label5.Name = "label5";
            label5.Padding = new Padding(20, 10, 0, 0);
            label5.Size = new Size(221, 460);
            label5.TabIndex = 2;
            label5.Text = "Card content\r\n\r\nCard content\r\n\r\nCard content";
            // 
            // divider1
            // 
            divider1.BackColor = Color.Transparent;
            divider1.Dock = DockStyle.Top;
            divider1.Location = new Point(24, 72);
            divider1.Margin = new Padding(10);
            divider1.Name = "divider1";
            divider1.Size = new Size(221, 1);
            divider1.TabIndex = 1;
            // 
            // label6
            // 
            label6.BackColor = Color.Transparent;
            label6.Dock = DockStyle.Top;
            label6.Font = new Font("Microsoft YaHei UI", 15.75F, FontStyle.Bold);
            label6.Location = new Point(24, 24);
            label6.Name = "label6";
            label6.Padding = new Padding(20, 0, 0, 0);
            label6.Size = new Size(221, 48);
            label6.TabIndex = 0;
            label6.Text = "Card title";
            label6.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // panel8
            // 
            panel8.ArrowSize = 10;
            panel8.Controls.Add(label9);
            panel8.Controls.Add(divider4);
            panel8.Controls.Add(label10);
            panel8.Location = new Point(1016, 3);
            panel8.Name = "panel8";
            panel8.Radius = 0;
            panel8.Shadow = 24;
            panel8.ShadowOpacity = 0.18F;
            panel8.ShadowOpacityAnimation = true;
            panel8.Size = new Size(269, 557);
            panel8.TabIndex = 21;
            // 
            // label9
            // 
            label9.BackColor = Color.Transparent;
            label9.Dock = DockStyle.Fill;
            label9.Font = new Font("Microsoft YaHei UI", 11F);
            label9.Location = new Point(24, 73);
            label9.Name = "label9";
            label9.Padding = new Padding(20, 10, 0, 0);
            label9.Size = new Size(221, 460);
            label9.TabIndex = 2;
            label9.Text = "Card content\r\n\r\nCard content\r\n\r\nCard content";
            // 
            // divider4
            // 
            divider4.BackColor = Color.Transparent;
            divider4.Dock = DockStyle.Top;
            divider4.Location = new Point(24, 72);
            divider4.Margin = new Padding(10);
            divider4.Name = "divider4";
            divider4.Size = new Size(221, 1);
            divider4.TabIndex = 1;
            // 
            // label10
            // 
            label10.BackColor = Color.Transparent;
            label10.Dock = DockStyle.Top;
            label10.Font = new Font("Microsoft YaHei UI", 15.75F, FontStyle.Bold);
            label10.Location = new Point(24, 24);
            label10.Name = "label10";
            label10.Padding = new Padding(20, 0, 0, 0);
            label10.Size = new Size(221, 48);
            label10.TabIndex = 0;
            label10.Text = "Card title";
            label10.TextAlign = ContentAlignment.MiddleLeft;
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
            panel8.ResumeLayout(false);
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
        private AntdUI.Button button1;
        private AntdUI.Button btn_Predict;
        private AntdUI.Panel panel4;
        private Label blacknessMethod_OriginImage_Text;
        private Label blacknessMethod_OriginImage_Zone;
        private AntdUI.Avatar blacknessMethod_OriginImage;
        private AntdUI.Panel panel5;
        private AntdUI.Button button3;
        private AntdUI.Button btn_UploadImage;
        private FlowLayoutPanel flowLayoutPanel1;
        private AntdUI.Panel panel6;
        private Label label5;
        private AntdUI.Divider divider1;
        private Label label6;
        private AntdUI.Panel panel8;
        private Label label9;
        private AntdUI.Divider divider4;
        private Label label10;
        private OpenFileDialog blacknessMethod_OpenFileDialog;
        private System.IO.FileSystemWatcher blacknessMethod_FileSystemWatcher;
        private AntdUI.Button btn_CameraCapture;
    }
}