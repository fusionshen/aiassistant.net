using System.Drawing;
using System.Windows.Forms;

namespace AI_Assistant_Win.Controls
{
    partial class CircularAreaReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CircularAreaReport));
            panel1 = new System.Windows.Forms.Panel();
            panel3 = new System.Windows.Forms.Panel();
            tablePanel = new TableLayoutPanel();
            avatarLowerSufaceDR = new AntdUI.Avatar();
            labelLowerSufaceDR = new Label();
            avatarUpperSufaceDR = new AntdUI.Avatar();
            labelUpperSufaceDR = new Label();
            avatarLowerSufaceCE = new AntdUI.Avatar();
            labelLowerSufaceCE = new Label();
            avatarUpperSufaceCE = new AntdUI.Avatar();
            labelUpperSufaceCE = new Label();
            avatarLowerSufaceOP = new AntdUI.Avatar();
            labelLowerSufaceOP = new Label();
            avatarUpperSufaceOP = new AntdUI.Avatar();
            labelUpperSufaceOP = new Label();
            label16 = new AntdUI.Label();
            label15 = new AntdUI.Label();
            label12 = new AntdUI.Label();
            label11 = new AntdUI.Label();
            label7 = new AntdUI.Label();
            label8 = new AntdUI.Label();
            panel17 = new AntdUI.Panel();
            checkboxUploaded = new AntdUI.Checkbox();
            labelTestNo = new Label();
            label10 = new Label();
            labelCoilNumber = new Label();
            label6 = new Label();
            button22 = new AntdUI.Button();
            panel7 = new AntdUI.Panel();
            label_Analyst = new Label();
            label4 = new Label();
            panel16 = new AntdUI.Panel();
            checkbox_Ding_Night = new AntdUI.Checkbox();
            button13 = new AntdUI.Button();
            panel15 = new AntdUI.Panel();
            checkbox_Ding_Day = new AntdUI.Checkbox();
            button12 = new AntdUI.Button();
            panel14 = new AntdUI.Panel();
            checkbox_Bing_Night = new AntdUI.Checkbox();
            button11 = new AntdUI.Button();
            panel13 = new AntdUI.Panel();
            checkbox_Bing_Day = new AntdUI.Checkbox();
            button10 = new AntdUI.Button();
            panel12 = new AntdUI.Panel();
            checkbox_Yi_Night = new AntdUI.Checkbox();
            button9 = new AntdUI.Button();
            panel11 = new AntdUI.Panel();
            checkbox_Yi_Day = new AntdUI.Checkbox();
            button8 = new AntdUI.Button();
            panel10 = new AntdUI.Panel();
            checkbox_Jia_Night = new AntdUI.Checkbox();
            button7 = new AntdUI.Button();
            panel9 = new AntdUI.Panel();
            checkbox_Jia_Day = new AntdUI.Checkbox();
            button6 = new AntdUI.Button();
            button4 = new AntdUI.Button();
            panel6 = new AntdUI.Panel();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            blacknessMethod_OriginImage_Zone = new Label();
            button3 = new AntdUI.Button();
            panel4 = new AntdUI.Panel();
            button2 = new AntdUI.Button();
            panel5 = new AntdUI.Panel();
            labelDate = new Label();
            labelTitle = new Label();
            printDocument1 = new System.Drawing.Printing.PrintDocument();
            printPreviewDialog1 = new PrintPreviewDialog();
            pageSetupDialog1 = new PageSetupDialog();
            panel1.SuspendLayout();
            panel3.SuspendLayout();
            tablePanel.SuspendLayout();
            avatarLowerSufaceDR.SuspendLayout();
            avatarUpperSufaceDR.SuspendLayout();
            avatarLowerSufaceCE.SuspendLayout();
            avatarUpperSufaceCE.SuspendLayout();
            avatarLowerSufaceOP.SuspendLayout();
            avatarUpperSufaceOP.SuspendLayout();
            panel17.SuspendLayout();
            panel7.SuspendLayout();
            panel16.SuspendLayout();
            panel15.SuspendLayout();
            panel14.SuspendLayout();
            panel13.SuspendLayout();
            panel12.SuspendLayout();
            panel11.SuspendLayout();
            panel10.SuspendLayout();
            panel9.SuspendLayout();
            panel6.SuspendLayout();
            panel4.SuspendLayout();
            panel5.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.AutoScroll = true;
            panel1.Controls.Add(panel3);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(420, 622);
            panel1.TabIndex = 6;
            // 
            // panel3
            // 
            panel3.AutoSize = true;
            panel3.Controls.Add(tablePanel);
            panel3.Controls.Add(panel17);
            panel3.Controls.Add(panel7);
            panel3.Controls.Add(panel6);
            panel3.Controls.Add(panel4);
            panel3.Controls.Add(panel5);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(0, 0);
            panel3.Name = "panel3";
            panel3.Size = new Size(420, 622);
            panel3.TabIndex = 1;
            // 
            // tablePanel
            // 
            tablePanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tablePanel.ColumnCount = 2;
            tablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tablePanel.Controls.Add(avatarLowerSufaceDR, 1, 5);
            tablePanel.Controls.Add(avatarUpperSufaceDR, 0, 5);
            tablePanel.Controls.Add(avatarLowerSufaceCE, 1, 3);
            tablePanel.Controls.Add(avatarUpperSufaceCE, 0, 3);
            tablePanel.Controls.Add(avatarLowerSufaceOP, 1, 1);
            tablePanel.Controls.Add(avatarUpperSufaceOP, 0, 1);
            tablePanel.Controls.Add(label16, 1, 4);
            tablePanel.Controls.Add(label15, 0, 4);
            tablePanel.Controls.Add(label12, 1, 2);
            tablePanel.Controls.Add(label11, 0, 2);
            tablePanel.Controls.Add(label7, 1, 0);
            tablePanel.Controls.Add(label8, 0, 0);
            tablePanel.Dock = DockStyle.Top;
            tablePanel.Location = new Point(0, 89);
            tablePanel.Name = "tablePanel";
            tablePanel.RowCount = 6;
            tablePanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tablePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tablePanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tablePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tablePanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tablePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tablePanel.Size = new Size(420, 520);
            tablePanel.TabIndex = 14;
            // 
            // avatarLowerSufaceDR
            // 
            avatarLowerSufaceDR.Controls.Add(labelLowerSufaceDR);
            avatarLowerSufaceDR.Dock = DockStyle.Fill;
            avatarLowerSufaceDR.Image = Properties.Resources.area_template;
            avatarLowerSufaceDR.Location = new Point(213, 369);
            avatarLowerSufaceDR.Name = "avatarLowerSufaceDR";
            avatarLowerSufaceDR.Radius = 6;
            avatarLowerSufaceDR.Size = new Size(203, 147);
            avatarLowerSufaceDR.TabIndex = 18;
            // 
            // labelLowerSufaceDR
            // 
            labelLowerSufaceDR.BackColor = Color.Transparent;
            labelLowerSufaceDR.Dock = DockStyle.Right;
            labelLowerSufaceDR.Font = new Font("Microsoft YaHei UI", 4F);
            labelLowerSufaceDR.ForeColor = Color.Red;
            labelLowerSufaceDR.Location = new Point(103, 0);
            labelLowerSufaceDR.Margin = new Padding(0);
            labelLowerSufaceDR.Name = "labelLowerSufaceDR";
            labelLowerSufaceDR.Size = new Size(100, 147);
            labelLowerSufaceDR.TabIndex = 22;
            labelLowerSufaceDR.Text = "   张  三     ";
            labelLowerSufaceDR.TextAlign = ContentAlignment.BottomRight;
            // 
            // avatarUpperSufaceDR
            // 
            avatarUpperSufaceDR.Controls.Add(labelUpperSufaceDR);
            avatarUpperSufaceDR.Dock = DockStyle.Fill;
            avatarUpperSufaceDR.Image = Properties.Resources.area_template;
            avatarUpperSufaceDR.Location = new Point(4, 369);
            avatarUpperSufaceDR.Name = "avatarUpperSufaceDR";
            avatarUpperSufaceDR.Radius = 6;
            avatarUpperSufaceDR.Size = new Size(202, 147);
            avatarUpperSufaceDR.TabIndex = 17;
            // 
            // labelUpperSufaceDR
            // 
            labelUpperSufaceDR.BackColor = Color.Transparent;
            labelUpperSufaceDR.Dock = DockStyle.Right;
            labelUpperSufaceDR.Font = new Font("Microsoft YaHei UI", 4F);
            labelUpperSufaceDR.ForeColor = Color.Red;
            labelUpperSufaceDR.Location = new Point(102, 0);
            labelUpperSufaceDR.Margin = new Padding(0);
            labelUpperSufaceDR.Name = "labelUpperSufaceDR";
            labelUpperSufaceDR.Size = new Size(100, 147);
            labelUpperSufaceDR.TabIndex = 22;
            labelUpperSufaceDR.Text = "   张  三     ";
            labelUpperSufaceDR.TextAlign = ContentAlignment.BottomRight;
            // 
            // avatarLowerSufaceCE
            // 
            avatarLowerSufaceCE.Controls.Add(labelLowerSufaceCE);
            avatarLowerSufaceCE.Dock = DockStyle.Fill;
            avatarLowerSufaceCE.Image = Properties.Resources.area_template;
            avatarLowerSufaceCE.Location = new Point(213, 197);
            avatarLowerSufaceCE.Name = "avatarLowerSufaceCE";
            avatarLowerSufaceCE.Radius = 6;
            avatarLowerSufaceCE.Size = new Size(203, 144);
            avatarLowerSufaceCE.TabIndex = 16;
            // 
            // labelLowerSufaceCE
            // 
            labelLowerSufaceCE.BackColor = Color.Transparent;
            labelLowerSufaceCE.Dock = DockStyle.Right;
            labelLowerSufaceCE.Font = new Font("Microsoft YaHei UI", 4F);
            labelLowerSufaceCE.ForeColor = Color.Red;
            labelLowerSufaceCE.Location = new Point(103, 0);
            labelLowerSufaceCE.Margin = new Padding(0);
            labelLowerSufaceCE.Name = "labelLowerSufaceCE";
            labelLowerSufaceCE.Size = new Size(100, 144);
            labelLowerSufaceCE.TabIndex = 22;
            labelLowerSufaceCE.Text = "   张  三     ";
            labelLowerSufaceCE.TextAlign = ContentAlignment.BottomRight;
            // 
            // avatarUpperSufaceCE
            // 
            avatarUpperSufaceCE.Controls.Add(labelUpperSufaceCE);
            avatarUpperSufaceCE.Dock = DockStyle.Fill;
            avatarUpperSufaceCE.Image = Properties.Resources.area_template;
            avatarUpperSufaceCE.Location = new Point(4, 197);
            avatarUpperSufaceCE.Name = "avatarUpperSufaceCE";
            avatarUpperSufaceCE.Radius = 6;
            avatarUpperSufaceCE.Size = new Size(202, 144);
            avatarUpperSufaceCE.TabIndex = 15;
            // 
            // labelUpperSufaceCE
            // 
            labelUpperSufaceCE.BackColor = Color.Transparent;
            labelUpperSufaceCE.Dock = DockStyle.Right;
            labelUpperSufaceCE.Font = new Font("Microsoft YaHei UI", 4F);
            labelUpperSufaceCE.ForeColor = Color.Red;
            labelUpperSufaceCE.Location = new Point(102, 0);
            labelUpperSufaceCE.Margin = new Padding(0);
            labelUpperSufaceCE.Name = "labelUpperSufaceCE";
            labelUpperSufaceCE.Size = new Size(100, 144);
            labelUpperSufaceCE.TabIndex = 21;
            labelUpperSufaceCE.Text = "   张  三     ";
            labelUpperSufaceCE.TextAlign = ContentAlignment.BottomRight;
            // 
            // avatarLowerSufaceOP
            // 
            avatarLowerSufaceOP.Controls.Add(labelLowerSufaceOP);
            avatarLowerSufaceOP.Dock = DockStyle.Fill;
            avatarLowerSufaceOP.Image = Properties.Resources.area_template;
            avatarLowerSufaceOP.Location = new Point(213, 25);
            avatarLowerSufaceOP.Name = "avatarLowerSufaceOP";
            avatarLowerSufaceOP.Radius = 6;
            avatarLowerSufaceOP.Size = new Size(203, 144);
            avatarLowerSufaceOP.TabIndex = 14;
            // 
            // labelLowerSufaceOP
            // 
            labelLowerSufaceOP.BackColor = Color.Transparent;
            labelLowerSufaceOP.Dock = DockStyle.Right;
            labelLowerSufaceOP.Font = new Font("Microsoft YaHei UI", 4F);
            labelLowerSufaceOP.ForeColor = Color.Red;
            labelLowerSufaceOP.Location = new Point(103, 0);
            labelLowerSufaceOP.Margin = new Padding(0);
            labelLowerSufaceOP.Name = "labelLowerSufaceOP";
            labelLowerSufaceOP.Size = new Size(100, 144);
            labelLowerSufaceOP.TabIndex = 21;
            labelLowerSufaceOP.Text = "   张  三     ";
            labelLowerSufaceOP.TextAlign = ContentAlignment.BottomRight;
            // 
            // avatarUpperSufaceOP
            // 
            avatarUpperSufaceOP.Controls.Add(labelUpperSufaceOP);
            avatarUpperSufaceOP.Dock = DockStyle.Fill;
            avatarUpperSufaceOP.Image = Properties.Resources.area_template;
            avatarUpperSufaceOP.Location = new Point(4, 25);
            avatarUpperSufaceOP.Name = "avatarUpperSufaceOP";
            avatarUpperSufaceOP.Radius = 6;
            avatarUpperSufaceOP.Size = new Size(202, 144);
            avatarUpperSufaceOP.TabIndex = 13;
            // 
            // labelUpperSufaceOP
            // 
            labelUpperSufaceOP.BackColor = Color.Transparent;
            labelUpperSufaceOP.Dock = DockStyle.Right;
            labelUpperSufaceOP.Font = new Font("Microsoft YaHei UI", 4F);
            labelUpperSufaceOP.ForeColor = Color.Red;
            labelUpperSufaceOP.Location = new Point(102, 0);
            labelUpperSufaceOP.Margin = new Padding(0);
            labelUpperSufaceOP.Name = "labelUpperSufaceOP";
            labelUpperSufaceOP.Size = new Size(100, 144);
            labelUpperSufaceOP.TabIndex = 20;
            labelUpperSufaceOP.Text = "   张  三     ";
            labelUpperSufaceOP.TextAlign = ContentAlignment.BottomRight;
            // 
            // label16
            // 
            label16.Dock = DockStyle.Fill;
            label16.Font = new Font("Microsoft YaHei UI", 12F);
            label16.LocalizationText = "Location";
            label16.Location = new Point(213, 348);
            label16.Name = "label16";
            label16.Size = new Size(203, 14);
            label16.TabIndex = 9;
            label16.Text = "下表面DR";
            label16.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label15
            // 
            label15.Dock = DockStyle.Fill;
            label15.Font = new Font("Microsoft YaHei UI", 12F);
            label15.LocalizationText = "Location";
            label15.Location = new Point(4, 348);
            label15.Name = "label15";
            label15.Size = new Size(202, 14);
            label15.TabIndex = 8;
            label15.Text = "上表面DR";
            label15.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label12
            // 
            label12.Dock = DockStyle.Fill;
            label12.Font = new Font("Microsoft YaHei UI", 12F);
            label12.LocalizationText = "Location";
            label12.Location = new Point(213, 176);
            label12.Name = "label12";
            label12.Size = new Size(203, 14);
            label12.TabIndex = 5;
            label12.Text = "下表面CE";
            label12.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label11
            // 
            label11.Dock = DockStyle.Fill;
            label11.Font = new Font("Microsoft YaHei UI", 12F);
            label11.LocalizationText = "Location";
            label11.Location = new Point(4, 176);
            label11.Name = "label11";
            label11.Size = new Size(202, 14);
            label11.TabIndex = 4;
            label11.Text = "上表面CE";
            label11.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            label7.Dock = DockStyle.Fill;
            label7.Font = new Font("Microsoft YaHei UI", 12F);
            label7.LocalizationText = "Pixels";
            label7.Location = new Point(213, 4);
            label7.Name = "label7";
            label7.Size = new Size(203, 14);
            label7.TabIndex = 1;
            label7.Text = "下表面OP";
            label7.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            label8.Dock = DockStyle.Fill;
            label8.Font = new Font("Microsoft YaHei UI", 12F);
            label8.LocalizationText = "Location";
            label8.Location = new Point(4, 4);
            label8.Name = "label8";
            label8.Size = new Size(202, 14);
            label8.TabIndex = 0;
            label8.Text = "上表面OP";
            label8.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel17
            // 
            panel17.Controls.Add(checkboxUploaded);
            panel17.Controls.Add(labelTestNo);
            panel17.Controls.Add(label10);
            panel17.Controls.Add(labelCoilNumber);
            panel17.Controls.Add(label6);
            panel17.Controls.Add(button22);
            panel17.Dock = DockStyle.Top;
            panel17.Location = new Point(0, 71);
            panel17.Name = "panel17";
            panel17.Radius = 0;
            panel17.Size = new Size(420, 18);
            panel17.TabIndex = 9;
            panel17.Text = "panel17";
            // 
            // checkboxUploaded
            // 
            checkboxUploaded.AutoCheck = true;
            checkboxUploaded.BackColor = Color.Transparent;
            checkboxUploaded.Checked = true;
            checkboxUploaded.Dock = DockStyle.Fill;
            checkboxUploaded.Fill = Color.FromArgb(200, 0, 0);
            checkboxUploaded.Font = new Font("Microsoft YaHei UI", 6F);
            checkboxUploaded.Location = new Point(340, 0);
            checkboxUploaded.Name = "checkboxUploaded";
            checkboxUploaded.RightToLeft = RightToLeft.Yes;
            checkboxUploaded.Size = new Size(80, 18);
            checkboxUploaded.TabIndex = 22;
            checkboxUploaded.Text = "是否上传";
            checkboxUploaded.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // labelTestNo
            // 
            labelTestNo.BackColor = Color.Transparent;
            labelTestNo.Dock = DockStyle.Left;
            labelTestNo.Font = new Font("Microsoft YaHei UI", 6F, FontStyle.Underline);
            labelTestNo.Location = new Point(190, 0);
            labelTestNo.Margin = new Padding(0);
            labelTestNo.Name = "labelTestNo";
            labelTestNo.Size = new Size(150, 18);
            labelTestNo.TabIndex = 21;
            labelTestNo.Text = "   张  三     ";
            labelTestNo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label10
            // 
            label10.BackColor = Color.Transparent;
            label10.Dock = DockStyle.Left;
            label10.Font = new Font("Microsoft YaHei UI", 6F);
            label10.Location = new Point(140, 0);
            label10.Margin = new Padding(0);
            label10.Name = "label10";
            label10.Size = new Size(50, 18);
            label10.TabIndex = 20;
            label10.Text = "试样编号：";
            label10.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // labelCoilNumber
            // 
            labelCoilNumber.BackColor = Color.Transparent;
            labelCoilNumber.Dock = DockStyle.Left;
            labelCoilNumber.Font = new Font("Microsoft YaHei UI", 6F, FontStyle.Underline);
            labelCoilNumber.Location = new Point(40, 0);
            labelCoilNumber.Margin = new Padding(0);
            labelCoilNumber.Name = "labelCoilNumber";
            labelCoilNumber.Size = new Size(100, 18);
            labelCoilNumber.TabIndex = 19;
            labelCoilNumber.Text = "   张  三     ";
            labelCoilNumber.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            label6.BackColor = Color.Transparent;
            label6.Dock = DockStyle.Left;
            label6.Font = new Font("Microsoft YaHei UI", 6F);
            label6.Location = new Point(0, 0);
            label6.Margin = new Padding(0);
            label6.Name = "label6";
            label6.Size = new Size(40, 18);
            label6.TabIndex = 18;
            label6.Text = "钢卷号：";
            label6.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // button22
            // 
            button22.Location = new Point(0, 0);
            button22.Name = "button22";
            button22.Size = new Size(0, 0);
            button22.TabIndex = 0;
            // 
            // panel7
            // 
            panel7.Controls.Add(label_Analyst);
            panel7.Controls.Add(label4);
            panel7.Controls.Add(panel16);
            panel7.Controls.Add(panel15);
            panel7.Controls.Add(panel14);
            panel7.Controls.Add(panel13);
            panel7.Controls.Add(panel12);
            panel7.Controls.Add(panel11);
            panel7.Controls.Add(panel10);
            panel7.Controls.Add(panel9);
            panel7.Controls.Add(button4);
            panel7.Dock = DockStyle.Top;
            panel7.Location = new Point(0, 53);
            panel7.Name = "panel7";
            panel7.Radius = 0;
            panel7.Size = new Size(420, 18);
            panel7.TabIndex = 7;
            panel7.Text = "panel7";
            // 
            // label_Analyst
            // 
            label_Analyst.BackColor = Color.Transparent;
            label_Analyst.Dock = DockStyle.Left;
            label_Analyst.Font = new Font("Microsoft YaHei UI", 7F, FontStyle.Underline);
            label_Analyst.Location = new Point(367, 0);
            label_Analyst.Margin = new Padding(0);
            label_Analyst.Name = "label_Analyst";
            label_Analyst.Size = new Size(50, 18);
            label_Analyst.TabIndex = 18;
            label_Analyst.Text = " 张  三   ";
            label_Analyst.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            label4.BackColor = Color.Transparent;
            label4.Dock = DockStyle.Left;
            label4.Font = new Font("Microsoft YaHei UI", 7F);
            label4.Location = new Point(320, 0);
            label4.Margin = new Padding(0);
            label4.Name = "label4";
            label4.Size = new Size(47, 18);
            label4.TabIndex = 17;
            label4.Text = "分析人：";
            label4.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel16
            // 
            panel16.BorderColor = Color.Black;
            panel16.BorderWidth = 1F;
            panel16.Controls.Add(checkbox_Ding_Night);
            panel16.Controls.Add(button13);
            panel16.Dock = DockStyle.Left;
            panel16.Location = new Point(280, 0);
            panel16.Name = "panel16";
            panel16.Radius = 0;
            panel16.Size = new Size(40, 18);
            panel16.TabIndex = 16;
            panel16.Text = "panel16";
            // 
            // checkbox_Ding_Night
            // 
            checkbox_Ding_Night.AutoCheck = true;
            checkbox_Ding_Night.BackColor = Color.Transparent;
            checkbox_Ding_Night.Dock = DockStyle.Left;
            checkbox_Ding_Night.Fill = Color.FromArgb(200, 0, 0);
            checkbox_Ding_Night.Font = new Font("Microsoft YaHei UI", 8F);
            checkbox_Ding_Night.Location = new Point(1, 1);
            checkbox_Ding_Night.Name = "checkbox_Ding_Night";
            checkbox_Ding_Night.RightToLeft = RightToLeft.Yes;
            checkbox_Ding_Night.Size = new Size(40, 16);
            checkbox_Ding_Night.TabIndex = 3;
            checkbox_Ding_Night.Text = "夜";
            checkbox_Ding_Night.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // button13
            // 
            button13.Location = new Point(0, 0);
            button13.Name = "button13";
            button13.Size = new Size(0, 0);
            button13.TabIndex = 0;
            // 
            // panel15
            // 
            panel15.BorderColor = Color.Black;
            panel15.BorderWidth = 1F;
            panel15.Controls.Add(checkbox_Ding_Day);
            panel15.Controls.Add(button12);
            panel15.Dock = DockStyle.Left;
            panel15.Location = new Point(240, 0);
            panel15.Name = "panel15";
            panel15.Radius = 0;
            panel15.Size = new Size(40, 18);
            panel15.TabIndex = 15;
            panel15.Text = "panel15";
            // 
            // checkbox_Ding_Day
            // 
            checkbox_Ding_Day.AutoCheck = true;
            checkbox_Ding_Day.BackColor = Color.Transparent;
            checkbox_Ding_Day.Dock = DockStyle.Left;
            checkbox_Ding_Day.Fill = Color.FromArgb(200, 0, 0);
            checkbox_Ding_Day.Font = new Font("Microsoft YaHei UI", 8F);
            checkbox_Ding_Day.Location = new Point(1, 1);
            checkbox_Ding_Day.Name = "checkbox_Ding_Day";
            checkbox_Ding_Day.RightToLeft = RightToLeft.Yes;
            checkbox_Ding_Day.Size = new Size(40, 16);
            checkbox_Ding_Day.TabIndex = 3;
            checkbox_Ding_Day.Text = "白   ";
            checkbox_Ding_Day.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // button12
            // 
            button12.Location = new Point(0, 0);
            button12.Name = "button12";
            button12.Size = new Size(0, 0);
            button12.TabIndex = 0;
            // 
            // panel14
            // 
            panel14.BorderColor = Color.Black;
            panel14.BorderWidth = 1F;
            panel14.Controls.Add(checkbox_Bing_Night);
            panel14.Controls.Add(button11);
            panel14.Dock = DockStyle.Left;
            panel14.Location = new Point(200, 0);
            panel14.Name = "panel14";
            panel14.Radius = 0;
            panel14.Size = new Size(40, 18);
            panel14.TabIndex = 14;
            panel14.Text = "panel14";
            // 
            // checkbox_Bing_Night
            // 
            checkbox_Bing_Night.AutoCheck = true;
            checkbox_Bing_Night.BackColor = Color.Transparent;
            checkbox_Bing_Night.Dock = DockStyle.Left;
            checkbox_Bing_Night.Fill = Color.FromArgb(200, 0, 0);
            checkbox_Bing_Night.Font = new Font("Microsoft YaHei UI", 8F);
            checkbox_Bing_Night.Location = new Point(1, 1);
            checkbox_Bing_Night.Name = "checkbox_Bing_Night";
            checkbox_Bing_Night.RightToLeft = RightToLeft.Yes;
            checkbox_Bing_Night.Size = new Size(40, 16);
            checkbox_Bing_Night.TabIndex = 3;
            checkbox_Bing_Night.Text = "夜";
            checkbox_Bing_Night.TextAlign = ContentAlignment.MiddleCenter;
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
            panel13.BorderColor = Color.Black;
            panel13.BorderWidth = 1F;
            panel13.Controls.Add(checkbox_Bing_Day);
            panel13.Controls.Add(button10);
            panel13.Dock = DockStyle.Left;
            panel13.Location = new Point(160, 0);
            panel13.Name = "panel13";
            panel13.Radius = 0;
            panel13.Size = new Size(40, 18);
            panel13.TabIndex = 13;
            panel13.Text = "panel13";
            // 
            // checkbox_Bing_Day
            // 
            checkbox_Bing_Day.AutoCheck = true;
            checkbox_Bing_Day.BackColor = Color.Transparent;
            checkbox_Bing_Day.Dock = DockStyle.Left;
            checkbox_Bing_Day.Fill = Color.FromArgb(200, 0, 0);
            checkbox_Bing_Day.Font = new Font("Microsoft YaHei UI", 8F);
            checkbox_Bing_Day.Location = new Point(1, 1);
            checkbox_Bing_Day.Name = "checkbox_Bing_Day";
            checkbox_Bing_Day.RightToLeft = RightToLeft.Yes;
            checkbox_Bing_Day.Size = new Size(40, 16);
            checkbox_Bing_Day.TabIndex = 3;
            checkbox_Bing_Day.Text = "白   ";
            checkbox_Bing_Day.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // button10
            // 
            button10.Location = new Point(0, 0);
            button10.Name = "button10";
            button10.Size = new Size(0, 0);
            button10.TabIndex = 0;
            // 
            // panel12
            // 
            panel12.BorderColor = Color.Black;
            panel12.BorderWidth = 1F;
            panel12.Controls.Add(checkbox_Yi_Night);
            panel12.Controls.Add(button9);
            panel12.Dock = DockStyle.Left;
            panel12.Location = new Point(120, 0);
            panel12.Name = "panel12";
            panel12.Radius = 0;
            panel12.Size = new Size(40, 18);
            panel12.TabIndex = 12;
            panel12.Text = "panel12";
            // 
            // checkbox_Yi_Night
            // 
            checkbox_Yi_Night.AutoCheck = true;
            checkbox_Yi_Night.BackColor = Color.Transparent;
            checkbox_Yi_Night.Checked = true;
            checkbox_Yi_Night.Dock = DockStyle.Left;
            checkbox_Yi_Night.Fill = Color.FromArgb(200, 0, 0);
            checkbox_Yi_Night.Font = new Font("Microsoft YaHei UI", 8F);
            checkbox_Yi_Night.Location = new Point(1, 1);
            checkbox_Yi_Night.Name = "checkbox_Yi_Night";
            checkbox_Yi_Night.RightToLeft = RightToLeft.Yes;
            checkbox_Yi_Night.Size = new Size(40, 16);
            checkbox_Yi_Night.TabIndex = 3;
            checkbox_Yi_Night.Text = "夜";
            checkbox_Yi_Night.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // button9
            // 
            button9.Location = new Point(0, 0);
            button9.Name = "button9";
            button9.Size = new Size(0, 0);
            button9.TabIndex = 0;
            // 
            // panel11
            // 
            panel11.BorderColor = Color.Black;
            panel11.BorderWidth = 1F;
            panel11.Controls.Add(checkbox_Yi_Day);
            panel11.Controls.Add(button8);
            panel11.Dock = DockStyle.Left;
            panel11.Location = new Point(80, 0);
            panel11.Name = "panel11";
            panel11.Radius = 0;
            panel11.Size = new Size(40, 18);
            panel11.TabIndex = 11;
            panel11.Text = "panel11";
            // 
            // checkbox_Yi_Day
            // 
            checkbox_Yi_Day.AutoCheck = true;
            checkbox_Yi_Day.BackColor = Color.Transparent;
            checkbox_Yi_Day.Dock = DockStyle.Left;
            checkbox_Yi_Day.Fill = Color.FromArgb(200, 0, 0);
            checkbox_Yi_Day.Font = new Font("Microsoft YaHei UI", 8F);
            checkbox_Yi_Day.Location = new Point(1, 1);
            checkbox_Yi_Day.Name = "checkbox_Yi_Day";
            checkbox_Yi_Day.RightToLeft = RightToLeft.Yes;
            checkbox_Yi_Day.Size = new Size(40, 16);
            checkbox_Yi_Day.TabIndex = 3;
            checkbox_Yi_Day.Text = "白   ";
            checkbox_Yi_Day.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // button8
            // 
            button8.Location = new Point(0, 0);
            button8.Name = "button8";
            button8.Size = new Size(0, 0);
            button8.TabIndex = 0;
            // 
            // panel10
            // 
            panel10.BorderColor = Color.Black;
            panel10.BorderWidth = 1F;
            panel10.Controls.Add(checkbox_Jia_Night);
            panel10.Controls.Add(button7);
            panel10.Dock = DockStyle.Left;
            panel10.Location = new Point(40, 0);
            panel10.Name = "panel10";
            panel10.Radius = 0;
            panel10.Size = new Size(40, 18);
            panel10.TabIndex = 10;
            panel10.Text = "panel10";
            // 
            // checkbox_Jia_Night
            // 
            checkbox_Jia_Night.AutoCheck = true;
            checkbox_Jia_Night.BackColor = Color.Transparent;
            checkbox_Jia_Night.Dock = DockStyle.Left;
            checkbox_Jia_Night.Fill = Color.FromArgb(200, 0, 0);
            checkbox_Jia_Night.Font = new Font("Microsoft YaHei UI", 8F);
            checkbox_Jia_Night.Location = new Point(1, 1);
            checkbox_Jia_Night.Name = "checkbox_Jia_Night";
            checkbox_Jia_Night.RightToLeft = RightToLeft.Yes;
            checkbox_Jia_Night.Size = new Size(40, 16);
            checkbox_Jia_Night.TabIndex = 3;
            checkbox_Jia_Night.Text = "夜";
            checkbox_Jia_Night.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // button7
            // 
            button7.Location = new Point(0, 0);
            button7.Name = "button7";
            button7.Size = new Size(0, 0);
            button7.TabIndex = 0;
            // 
            // panel9
            // 
            panel9.BorderColor = Color.Black;
            panel9.BorderWidth = 1F;
            panel9.Controls.Add(checkbox_Jia_Day);
            panel9.Controls.Add(button6);
            panel9.Dock = DockStyle.Left;
            panel9.Location = new Point(0, 0);
            panel9.Name = "panel9";
            panel9.Radius = 0;
            panel9.Size = new Size(40, 18);
            panel9.TabIndex = 9;
            panel9.Text = "panel9";
            // 
            // checkbox_Jia_Day
            // 
            checkbox_Jia_Day.AutoCheck = true;
            checkbox_Jia_Day.BackColor = Color.Transparent;
            checkbox_Jia_Day.Dock = DockStyle.Left;
            checkbox_Jia_Day.Fill = Color.FromArgb(200, 0, 0);
            checkbox_Jia_Day.Font = new Font("Microsoft YaHei UI", 8F);
            checkbox_Jia_Day.Location = new Point(1, 1);
            checkbox_Jia_Day.Name = "checkbox_Jia_Day";
            checkbox_Jia_Day.RightToLeft = RightToLeft.Yes;
            checkbox_Jia_Day.Size = new Size(40, 16);
            checkbox_Jia_Day.TabIndex = 3;
            checkbox_Jia_Day.Text = "白   ";
            checkbox_Jia_Day.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // button6
            // 
            button6.Location = new Point(0, 0);
            button6.Name = "button6";
            button6.Size = new Size(0, 0);
            button6.TabIndex = 0;
            // 
            // button4
            // 
            button4.Location = new Point(0, 0);
            button4.Name = "button4";
            button4.Size = new Size(0, 0);
            button4.TabIndex = 0;
            // 
            // panel6
            // 
            panel6.Controls.Add(label3);
            panel6.Controls.Add(label2);
            panel6.Controls.Add(label1);
            panel6.Controls.Add(blacknessMethod_OriginImage_Zone);
            panel6.Controls.Add(button3);
            panel6.Dock = DockStyle.Top;
            panel6.Location = new Point(0, 38);
            panel6.Name = "panel6";
            panel6.Radius = 0;
            panel6.Size = new Size(420, 15);
            panel6.TabIndex = 6;
            panel6.Text = "panel6";
            // 
            // label3
            // 
            label3.BackColor = Color.Transparent;
            label3.BorderStyle = BorderStyle.FixedSingle;
            label3.Dock = DockStyle.Left;
            label3.Font = new Font("Microsoft YaHei UI", 8F);
            label3.ForeColor = Color.Black;
            label3.Location = new Point(240, 0);
            label3.Name = "label3";
            label3.Size = new Size(80, 15);
            label3.TabIndex = 15;
            label3.Text = "丁";
            label3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            label2.BackColor = Color.Transparent;
            label2.BorderStyle = BorderStyle.FixedSingle;
            label2.Dock = DockStyle.Left;
            label2.Font = new Font("Microsoft YaHei UI", 8F);
            label2.ForeColor = Color.Black;
            label2.Location = new Point(160, 0);
            label2.Name = "label2";
            label2.Size = new Size(80, 15);
            label2.TabIndex = 14;
            label2.Text = "丙";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            label1.BackColor = Color.Transparent;
            label1.BorderStyle = BorderStyle.FixedSingle;
            label1.Dock = DockStyle.Left;
            label1.Font = new Font("Microsoft YaHei UI", 8F);
            label1.ForeColor = Color.Black;
            label1.Location = new Point(80, 0);
            label1.Name = "label1";
            label1.Size = new Size(80, 15);
            label1.TabIndex = 13;
            label1.Text = "乙";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // blacknessMethod_OriginImage_Zone
            // 
            blacknessMethod_OriginImage_Zone.BackColor = Color.Transparent;
            blacknessMethod_OriginImage_Zone.BorderStyle = BorderStyle.FixedSingle;
            blacknessMethod_OriginImage_Zone.Dock = DockStyle.Left;
            blacknessMethod_OriginImage_Zone.Font = new Font("Microsoft YaHei UI", 8F);
            blacknessMethod_OriginImage_Zone.ForeColor = Color.Black;
            blacknessMethod_OriginImage_Zone.Location = new Point(0, 0);
            blacknessMethod_OriginImage_Zone.Name = "blacknessMethod_OriginImage_Zone";
            blacknessMethod_OriginImage_Zone.Size = new Size(80, 15);
            blacknessMethod_OriginImage_Zone.TabIndex = 12;
            blacknessMethod_OriginImage_Zone.Text = "甲";
            blacknessMethod_OriginImage_Zone.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // button3
            // 
            button3.Location = new Point(0, 0);
            button3.Name = "button3";
            button3.Size = new Size(0, 0);
            button3.TabIndex = 0;
            // 
            // panel4
            // 
            panel4.BorderColor = Color.Gray;
            panel4.BorderStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
            panel4.BorderWidth = 2F;
            panel4.Controls.Add(button2);
            panel4.Dock = DockStyle.Top;
            panel4.Location = new Point(0, 34);
            panel4.Name = "panel4";
            panel4.Size = new Size(420, 4);
            panel4.TabIndex = 5;
            panel4.Text = "panel4";
            // 
            // button2
            // 
            button2.Location = new Point(0, 0);
            button2.Name = "button2";
            button2.Size = new Size(0, 0);
            button2.TabIndex = 0;
            // 
            // panel5
            // 
            panel5.BorderColor = SystemColors.WindowFrame;
            panel5.Controls.Add(labelDate);
            panel5.Controls.Add(labelTitle);
            panel5.Dock = DockStyle.Top;
            panel5.Location = new Point(0, 0);
            panel5.Name = "panel5";
            panel5.Radius = 0;
            panel5.Size = new Size(420, 34);
            panel5.TabIndex = 1;
            panel5.Text = "panel4";
            // 
            // labelDate
            // 
            labelDate.BackColor = Color.Transparent;
            labelDate.Dock = DockStyle.Left;
            labelDate.Font = new Font("Microsoft YaHei UI", 8F);
            labelDate.Location = new Point(300, 0);
            labelDate.Margin = new Padding(0);
            labelDate.Name = "labelDate";
            labelDate.Size = new Size(120, 34);
            labelDate.TabIndex = 14;
            labelDate.Text = "2024 年 11 月 28 日";
            labelDate.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // labelTitle
            // 
            labelTitle.BackColor = Color.Transparent;
            labelTitle.Dock = DockStyle.Left;
            labelTitle.Font = new Font("Microsoft YaHei UI", 12F);
            labelTitle.Location = new Point(0, 0);
            labelTitle.Margin = new Padding(0);
            labelTitle.Name = "labelTitle";
            labelTitle.Size = new Size(300, 34);
            labelTitle.TabIndex = 13;
            labelTitle.Text = "圆片面积检测结果报告书";
            labelTitle.TextAlign = ContentAlignment.MiddleRight;
            // 
            // printDocument1
            // 
            printDocument1.PrintPage += PrintDocument1_PrintPage;
            // 
            // printPreviewDialog1
            // 
            printPreviewDialog1.AutoScrollMargin = new Size(0, 0);
            printPreviewDialog1.AutoScrollMinSize = new Size(0, 0);
            printPreviewDialog1.ClientSize = new Size(400, 300);
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.Enabled = true;
            printPreviewDialog1.Icon = (System.Drawing.Icon)resources.GetObject("printPreviewDialog1.Icon");
            printPreviewDialog1.Name = "printPreviewDialog1";
            printPreviewDialog1.Visible = false;
            // 
            // CircularAreaReport
            // 
            AutoScaleMode = AutoScaleMode.Inherit;
            Controls.Add(panel1);
            Font = new Font("Microsoft YaHei UI", 12F);
            Name = "CircularAreaReport";
            Size = new Size(420, 622);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel3.ResumeLayout(false);
            tablePanel.ResumeLayout(false);
            avatarLowerSufaceDR.ResumeLayout(false);
            avatarUpperSufaceDR.ResumeLayout(false);
            avatarLowerSufaceCE.ResumeLayout(false);
            avatarUpperSufaceCE.ResumeLayout(false);
            avatarLowerSufaceOP.ResumeLayout(false);
            avatarUpperSufaceOP.ResumeLayout(false);
            panel17.ResumeLayout(false);
            panel7.ResumeLayout(false);
            panel16.ResumeLayout(false);
            panel15.ResumeLayout(false);
            panel14.ResumeLayout(false);
            panel13.ResumeLayout(false);
            panel12.ResumeLayout(false);
            panel11.ResumeLayout(false);
            panel10.ResumeLayout(false);
            panel9.ResumeLayout(false);
            panel6.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel5.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private AntdUI.Panel panel5;
        private Label labelTitle;
        private Label label_date11;
        private Label labelDate;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private PrintPreviewDialog printPreviewDialog1;
        private PageSetupDialog pageSetupDialog1;
        private AntdUI.Panel panel4;
        private AntdUI.Button button2;
        private AntdUI.Panel panel6;
        private AntdUI.Button button3;
        private Label blacknessMethod_OriginImage_Zone;
        private Label label3;
        private Label label2;
        private Label label1;
        private AntdUI.Panel panel7;
        private AntdUI.Button button4;
        private AntdUI.Panel panel16;
        private AntdUI.Checkbox checkbox_Ding_Night;
        private AntdUI.Button button13;
        private AntdUI.Panel panel15;
        private AntdUI.Checkbox checkbox_Ding_Day;
        private AntdUI.Button button12;
        private AntdUI.Panel panel14;
        private AntdUI.Checkbox checkbox_Bing_Night;
        private AntdUI.Button button11;
        private AntdUI.Panel panel13;
        private AntdUI.Checkbox checkbox_Bing_Day;
        private AntdUI.Button button10;
        private AntdUI.Panel panel12;
        private AntdUI.Checkbox checkbox_Yi_Night;
        private AntdUI.Button button9;
        private AntdUI.Panel panel11;
        private AntdUI.Checkbox checkbox_Yi_Day;
        private AntdUI.Button button8;
        private AntdUI.Panel panel10;
        private AntdUI.Checkbox checkbox_Jia_Night;
        private AntdUI.Button button7;
        private AntdUI.Panel panel9;
        private AntdUI.Checkbox checkbox_Jia_Day;
        private AntdUI.Button button6;
        private Label label4;
        private Label label_Analyst;
        private AntdUI.Panel panel17;
        private AntdUI.Button button22;
        private Label label6;
        private Label labelTestNo;
        private Label label10;
        private Label labelCoilNumber;
        private TableLayoutPanel tablePanel;
        private AntdUI.Label label7;
        private AntdUI.Label label8;
        private AntdUI.Label label16;
        private AntdUI.Label label15;
        private AntdUI.Label label12;
        private AntdUI.Label label11;
        private AntdUI.Avatar avatarUpperSufaceOP;
        private AntdUI.Avatar avatarLowerSufaceDR;
        private AntdUI.Avatar avatarUpperSufaceDR;
        private AntdUI.Avatar avatarLowerSufaceCE;
        private AntdUI.Avatar avatarUpperSufaceCE;
        private AntdUI.Avatar avatarLowerSufaceOP;
        private Label labelUpperSufaceOP;
        private Label labelUpperSufaceCE;
        private Label labelUpperSufaceDR;
        private Label labelLowerSufaceOP;
        private Label labelLowerSufaceDR;
        private Label labelLowerSufaceCE;
        private AntdUI.Checkbox checkboxUploaded;
    }
}