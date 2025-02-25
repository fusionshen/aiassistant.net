using System.Drawing;
using System.Windows.Forms;

namespace AI_Assistant_Win.Controls
{
    partial class ScaleAccuracyHistory
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScaleAccuracyHistory));
            header1 = new AntdUI.PageHeader();
            pagination1 = new AntdUI.Pagination();
            panel1 = new AntdUI.Panel();
            btnReload = new AntdUI.Button();
            selectMultipleTableSetting = new AntdUI.SelectMultiple();
            panel6 = new System.Windows.Forms.Panel();
            inputSearch = new AntdUI.Input();
            btnSearch = new AntdUI.Button();
            inputRangeDate = new AntdUI.DatePickerRange();
            tableTracerAreaHistory = new AntdUI.Table();
            panel1.SuspendLayout();
            panel6.SuspendLayout();
            SuspendLayout();
            // 
            // header1
            // 
            header1.Description = "展示比例尺在不同量程下的精度溯源记录，包含MPE、标准差、不确定度和正态分布置信度等。支持搜索、修改、删除、预览、打印、导出、上传等操作。注意：无法在文本框中修改查询时间，如需清空请点击右侧重置按钮。";
            header1.Dock = DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F);
            header1.LocalizationDescription = resources.GetString("header1.LocalizationDescription");
            header1.LocalizationText = "The Historical Record Of The Scale‘s Accuracy";
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new Padding(0, 0, 0, 10);
            header1.Size = new Size(1300, 74);
            header1.TabIndex = 0;
            header1.Text = "精度溯源";
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
            panel1.Controls.Add(btnReload);
            panel1.Controls.Add(selectMultipleTableSetting);
            panel1.Controls.Add(panel6);
            panel1.Controls.Add(inputRangeDate);
            panel1.Dock = DockStyle.Top;
            panel1.Font = new Font("Microsoft YaHei UI", 12F);
            panel1.Location = new Point(0, 74);
            panel1.Name = "panel1";
            panel1.Padding = new Padding(10, 0, 0, 0);
            panel1.Size = new Size(1300, 43);
            panel1.TabIndex = 1;
            panel1.Text = "panel1";
            // 
            // btnReload
            // 
            btnReload.IconSvg = "ReloadOutlined";
            btnReload.LoadingWaveVertical = true;
            btnReload.Location = new Point(1042, 0);
            btnReload.Name = "btnReload";
            btnReload.Shape = AntdUI.TShape.Circle;
            btnReload.Size = new Size(40, 40);
            btnReload.TabIndex = 28;
            btnReload.Click += BtnReload_Click;
            // 
            // selectMultipleTableSetting
            // 
            selectMultipleTableSetting.Items.AddRange(new object[] { "显示表头", "固定表头", "显示列边框", "奇偶列", "部分列排序", "手动调整列头宽度", "列拖拽" });
            selectMultipleTableSetting.LocalizationPlaceholderText = "Table Settings";
            selectMultipleTableSetting.Location = new Point(1, -1);
            selectMultipleTableSetting.Name = "selectMultipleTableSetting";
            selectMultipleTableSetting.PlaceholderText = "表格设置";
            selectMultipleTableSetting.Size = new Size(243, 46);
            selectMultipleTableSetting.SuffixText = "";
            selectMultipleTableSetting.TabIndex = 27;
            selectMultipleTableSetting.SelectedValueChanged += SelectMultipleTableSettingSelectedValueChanged;
            // 
            // panel6
            // 
            panel6.Controls.Add(inputSearch);
            panel6.Controls.Add(btnSearch);
            panel6.Location = new Point(1080, -2);
            panel6.Name = "panel6";
            panel6.Size = new Size(220, 46);
            panel6.TabIndex = 26;
            panel6.Text = "panel4";
            // 
            // inputSearch
            // 
            inputSearch.Dock = DockStyle.Fill;
            inputSearch.JoinRight = true;
            inputSearch.LocalizationPlaceholderText = "Text something.";
            inputSearch.Location = new Point(0, 0);
            inputSearch.Name = "inputSearch";
            inputSearch.PlaceholderText = "输入点什么搜索";
            inputSearch.Radius = 3;
            inputSearch.RightToLeft = RightToLeft.No;
            inputSearch.Size = new Size(170, 46);
            inputSearch.TabIndex = 0;
            inputSearch.KeyDown += InputSearch_KeyDown;
            // 
            // btnSearch
            // 
            btnSearch.Dock = DockStyle.Right;
            btnSearch.IconSvg = "SearchOutlined";
            btnSearch.JoinLeft = true;
            btnSearch.Location = new Point(170, 0);
            btnSearch.Name = "btnSearch";
            btnSearch.Shape = AntdUI.TShape.Circle;
            btnSearch.Size = new Size(50, 46);
            btnSearch.TabIndex = 1;
            btnSearch.Click += BtnSearch_Click;
            // 
            // inputRangeDate
            // 
            inputRangeDate.LocalizationPlaceholderEnd = "DatePicker.PlaceholderE";
            inputRangeDate.LocalizationPlaceholderStart = "DatePicker.PlaceholderS";
            inputRangeDate.Location = new Point(741, -1);
            inputRangeDate.Name = "inputRangeDate";
            inputRangeDate.PlaceholderEnd = "结束时间";
            inputRangeDate.PlaceholderStart = "开始时间";
            inputRangeDate.Size = new Size(300, 46);
            inputRangeDate.TabIndex = 25;
            inputRangeDate.ValueChanged += InputRangeDate_ValueChanged;
            // 
            // tableCircularAreaHistory
            // 
            tableTracerAreaHistory.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            tableTracerAreaHistory.Font = new Font("Microsoft YaHei UI", 11F);
            tableTracerAreaHistory.Location = new Point(3, 123);
            tableTracerAreaHistory.Name = "tableCircularAreaHistory";
            tableTracerAreaHistory.Radius = 6;
            tableTracerAreaHistory.Size = new Size(1294, 507);
            tableTracerAreaHistory.TabIndex = 0;
            tableTracerAreaHistory.CellClick += Table1_CellClick;
            tableTracerAreaHistory.CellButtonClick += Table1_CellButtonClick;
            // 
            // ScaleAccuracyHistory
            // 
            Controls.Add(panel1);
            Controls.Add(header1);
            Controls.Add(tableTracerAreaHistory);
            Controls.Add(pagination1);
            Font = new Font("Microsoft YaHei UI", 16F);
            Name = "ScaleAccuracyHistory";
            Size = new Size(1300, 676);
            panel1.ResumeLayout(false);
            panel6.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private AntdUI.PageHeader header1;
        private AntdUI.Pagination pagination1;
        private AntdUI.Panel panel1;
        private AntdUI.Table tableTracerAreaHistory;
        private AntdUI.DatePickerRange inputRangeDate;
        private System.Windows.Forms.Panel panel6;
        private AntdUI.Input inputSearch;
        private AntdUI.Button btnSearch;
        private AntdUI.SelectMultiple selectMultipleTableSetting;
        private AntdUI.Button btnClear;
        private AntdUI.Button btnReload;
    }
}