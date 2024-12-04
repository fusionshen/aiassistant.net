using System.Drawing;
using System.Windows.Forms;

namespace AI_Assistant_Win.Controls
{
    partial class BlacknessHistory
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
            pagination1 = new AntdUI.Pagination();
            panel1 = new AntdUI.Panel();
            selectMultiple_Table_Setting = new AntdUI.SelectMultiple();
            panel6 = new System.Windows.Forms.Panel();
            input9 = new AntdUI.Input();
            button2 = new AntdUI.Button();
            inputRange1 = new AntdUI.DatePickerRange();
            table_Blackness_History = new AntdUI.Table();
            panel1.SuspendLayout();
            panel6.SuspendLayout();
            SuspendLayout();
            // 
            // header1
            // 
            header1.Description = "展示行列数据。";
            header1.Dock = DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F);
            header1.LocalizationDescription = "BlacknessHistory.Description";
            header1.LocalizationText = "BlacknessHistory.Text";
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new Padding(0, 0, 0, 10);
            header1.Size = new Size(1300, 74);
            header1.TabIndex = 0;
            header1.Text = "GA板V60黑度检测历史记录";
            header1.UseTitleFont = true;
            // 
            // pagination1
            // 
            pagination1.Dock = DockStyle.Bottom;
            pagination1.Font = new Font("Microsoft YaHei UI", 11F);
            pagination1.Location = new Point(0, 636);
            pagination1.Name = "pagination1";
            pagination1.RightToLeft = RightToLeft.Yes;
            pagination1.ShowSizeChanger = true;
            pagination1.Size = new Size(1300, 40);
            pagination1.TabIndex = 5;
            pagination1.Total = 100;
            pagination1.ValueChanged += Pagination1_ValueChanged;
            pagination1.ShowTotalChanged += Pagination1_ShowTotalChanged;
            // 
            // panel1
            // 
            panel1.Controls.Add(selectMultiple_Table_Setting);
            panel1.Controls.Add(panel6);
            panel1.Controls.Add(inputRange1);
            panel1.Dock = DockStyle.Top;
            panel1.Font = new Font("Microsoft YaHei UI", 12F);
            panel1.Location = new Point(0, 74);
            panel1.Name = "panel1";
            panel1.Padding = new Padding(10, 0, 0, 0);
            panel1.Size = new Size(1300, 43);
            panel1.TabIndex = 1;
            panel1.Text = "panel1";
            // 
            // selectMultiple_Table_Setting
            // 
            selectMultiple_Table_Setting.Items.AddRange(new object[] { "显示表头", "固定表头", "显示列边框", "奇偶列", "部分列排序", "手动调整列头宽度", "列拖拽" });
            selectMultiple_Table_Setting.Location = new Point(1, -1);
            selectMultiple_Table_Setting.Name = "selectMultiple_Table_Setting";
            selectMultiple_Table_Setting.PlaceholderText = "表格设置";
            selectMultiple_Table_Setting.Size = new Size(243, 46);
            selectMultiple_Table_Setting.SuffixText = "";
            selectMultiple_Table_Setting.TabIndex = 27;
            selectMultiple_Table_Setting.SelectedValueChanged += SelectMultiple_Table_Setting_SelectedValueChanged;
            // 
            // panel6
            // 
            panel6.Controls.Add(input9);
            panel6.Controls.Add(button2);
            panel6.Location = new Point(1080, -2);
            panel6.Name = "panel6";
            panel6.Size = new Size(220, 46);
            panel6.TabIndex = 26;
            panel6.Text = "panel4";
            // 
            // input9
            // 
            input9.Dock = DockStyle.Fill;
            input9.JoinRight = true;
            input9.LocalizationPlaceholderText = "Input.{id}";
            input9.Location = new Point(0, 0);
            input9.Name = "input9";
            input9.PlaceholderText = "输入点什么搜索";
            input9.Size = new Size(170, 46);
            input9.TabIndex = 0;
            // 
            // button2
            // 
            button2.Dock = DockStyle.Right;
            button2.IconSvg = "SearchOutlined";
            button2.JoinLeft = true;
            button2.Location = new Point(170, 0);
            button2.Name = "button2";
            button2.Size = new Size(50, 46);
            button2.TabIndex = 1;
            button2.Type = AntdUI.TTypeMini.Primary;
            // 
            // inputRange1
            // 
            inputRange1.LocalizationPlaceholderEnd = "DatePicker.PlaceholderE";
            inputRange1.LocalizationPlaceholderStart = "DatePicker.PlaceholderS";
            inputRange1.Location = new Point(777, -1);
            inputRange1.Name = "inputRange1";
            inputRange1.PlaceholderEnd = "结束时间";
            inputRange1.PlaceholderStart = "开始时间";
            inputRange1.Size = new Size(300, 46);
            inputRange1.TabIndex = 25;
            // 
            // table_Blackness_History
            // 
            table_Blackness_History.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            table_Blackness_History.Font = new Font("Microsoft YaHei UI", 11F);
            table_Blackness_History.Location = new Point(3, 123);
            table_Blackness_History.Name = "table_Blackness_History";
            table_Blackness_History.Radius = 6;
            table_Blackness_History.Size = new Size(1297, 550);
            table_Blackness_History.TabIndex = 0;
            table_Blackness_History.CellClick += Table1_CellClick;
            table_Blackness_History.CellButtonClick += Table1_CellButtonClick;
            // 
            // BlacknessHistory
            // 
            Controls.Add(panel1);
            Controls.Add(header1);
            Controls.Add(table_Blackness_History);
            Controls.Add(pagination1);
            Font = new Font("Microsoft YaHei UI", 16F);
            Name = "BlacknessHistory";
            Size = new Size(1300, 676);
            panel1.ResumeLayout(false);
            panel6.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private AntdUI.PageHeader header1;
        private AntdUI.Pagination pagination1;
        private AntdUI.Panel panel1;
        private AntdUI.Table table_Blackness_History;
        private AntdUI.DatePickerRange inputRange1;
        private System.Windows.Forms.Panel panel6;
        private AntdUI.Input input9;
        private AntdUI.Button button2;
        private AntdUI.SelectMultiple selectMultiple_Table_Setting;
    }
}