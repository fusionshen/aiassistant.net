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
    partial class BlacknessReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BlacknessReport));
            panel1 = new System.Windows.Forms.Panel();
            panel3 = new System.Windows.Forms.Panel();
            panel4 = new AntdUI.Panel();
            btn_Print = new AntdUI.Button();
            panel5 = new AntdUI.Panel();
            label_Date = new Label();
            label_title = new Label();
            printDocument1 = new System.Drawing.Printing.PrintDocument();
            printPreviewDialog1 = new PrintPreviewDialog();
            pageSetupDialog1 = new PageSetupDialog();
            panel1.SuspendLayout();
            panel3.SuspendLayout();
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
            panel1.Size = new Size(840, 1188);
            panel1.TabIndex = 6;
            // 
            // panel3
            // 
            panel3.Controls.Add(panel4);
            panel3.Controls.Add(panel5);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(0, 0);
            panel3.Name = "panel3";
            panel3.Size = new Size(840, 1188);
            panel3.TabIndex = 1;
            // 
            // panel4
            // 
            panel4.Controls.Add(btn_Print);
            panel4.Location = new Point(292, 243);
            panel4.Name = "panel4";
            panel4.Size = new Size(220, 50);
            panel4.TabIndex = 3;
            panel4.Text = "panel4";
            // 
            // panel5
            // 
            panel5.BorderColor = SystemColors.WindowFrame;
            panel5.Controls.Add(label_Date);
            panel5.Controls.Add(label_title);
            panel5.Dock = DockStyle.Top;
            panel5.Location = new Point(0, 0);
            panel5.Name = "panel5";
            panel5.Radius = 0;
            panel5.Size = new Size(840, 98);
            panel5.TabIndex = 1;
            panel5.Text = "panel4";
            // 
            // label_Date
            // 
            label_Date.BackColor = Color.Transparent;
            label_Date.Dock = DockStyle.Left;
            label_Date.Font = new Font("Microsoft YaHei UI", 14F);
            label_Date.Location = new Point(550, 0);
            label_Date.Margin = new Padding(0);
            label_Date.Name = "label_Date";
            label_Date.Size = new Size(289, 98);
            label_Date.TabIndex = 14;
            label_Date.Text = "2024 年 11 月 28 日";
            label_Date.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label_title
            // 
            label_title.BackColor = Color.Transparent;
            label_title.Dock = DockStyle.Left;
            label_title.Font = new Font("Microsoft YaHei UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label_title.Location = new Point(0, 0);
            label_title.Margin = new Padding(0);
            label_title.Name = "label_title";
            label_title.Size = new Size(550, 98);
            label_title.TabIndex = 13;
            label_title.Text = "GA板黑度测试结果报告书";
            label_title.TextAlign = ContentAlignment.MiddleRight;
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
            // BlacknessReport
            // 
            Controls.Add(panel1);
            Font = new Font("Microsoft YaHei UI", 12F);
            Name = "BlacknessReport";
            Size = new Size(840, 1188);
            panel1.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel5.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private AntdUI.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private AntdUI.Panel panel5;
        private Label label_title;
        private Label label_date11;
        private Label label_Date;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private AntdUI.Button btn_Print;
        private PrintPreviewDialog printPreviewDialog1;
        private PageSetupDialog pageSetupDialog1;
    }
}