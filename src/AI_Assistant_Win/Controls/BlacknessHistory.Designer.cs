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
            checkVisibleHeader = new AntdUI.Checkbox();
            checkEnableHeaderResizing = new AntdUI.Checkbox();
            checkSortOrder = new AntdUI.Checkbox();
            checkSetRowStyle = new AntdUI.Checkbox();
            checkBordered = new AntdUI.Checkbox();
            checkColumnDragSort = new AntdUI.Checkbox();
            checkFixedHeader = new AntdUI.Checkbox();
            table_Blackness_History = new AntdUI.Table();
            panel1.SuspendLayout();
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
            panel1.Controls.Add(checkVisibleHeader);
            panel1.Controls.Add(checkEnableHeaderResizing);
            panel1.Controls.Add(checkSortOrder);
            panel1.Controls.Add(checkSetRowStyle);
            panel1.Controls.Add(checkBordered);
            panel1.Controls.Add(checkColumnDragSort);
            panel1.Controls.Add(checkFixedHeader);
            panel1.Dock = DockStyle.Top;
            panel1.Font = new Font("Microsoft YaHei UI", 12F);
            panel1.Location = new Point(0, 74);
            panel1.Name = "panel1";
            panel1.Padding = new Padding(10, 0, 0, 0);
            panel1.Size = new Size(1300, 43);
            panel1.TabIndex = 1;
            panel1.Text = "panel1";
            // 
            // checkVisibleHeader
            // 
            checkVisibleHeader.AutoCheck = true;
            checkVisibleHeader.AutoSizeMode = AntdUI.TAutoSize.Width;
            checkVisibleHeader.Checked = true;
            checkVisibleHeader.Dock = DockStyle.Left;
            checkVisibleHeader.LocalizationText = "Table.{id}";
            checkVisibleHeader.Location = new Point(781, 0);
            checkVisibleHeader.Name = "checkVisibleHeader";
            checkVisibleHeader.Size = new Size(115, 43);
            checkVisibleHeader.TabIndex = 6;
            checkVisibleHeader.Text = "显示表头";
            checkVisibleHeader.CheckedChanged += CheckVisibleHeader_CheckedChanged;
            // 
            // checkEnableHeaderResizing
            // 
            checkEnableHeaderResizing.AutoCheck = true;
            checkEnableHeaderResizing.AutoSizeMode = AntdUI.TAutoSize.Width;
            checkEnableHeaderResizing.Dock = DockStyle.Left;
            checkEnableHeaderResizing.LocalizationText = "Table.{id}";
            checkEnableHeaderResizing.Location = new Point(600, 0);
            checkEnableHeaderResizing.Name = "checkEnableHeaderResizing";
            checkEnableHeaderResizing.Size = new Size(181, 43);
            checkEnableHeaderResizing.TabIndex = 5;
            checkEnableHeaderResizing.Text = "手动调整列头宽度";
            checkEnableHeaderResizing.CheckedChanged += CheckEnableHeaderResizing_CheckedChanged;
            // 
            // checkSortOrder
            // 
            checkSortOrder.AutoCheck = true;
            checkSortOrder.AutoSizeMode = AntdUI.TAutoSize.Width;
            checkSortOrder.Dock = DockStyle.Left;
            checkSortOrder.LocalizationText = "Table.{id}";
            checkSortOrder.Location = new Point(485, 0);
            checkSortOrder.Name = "checkSortOrder";
            checkSortOrder.Size = new Size(115, 43);
            checkSortOrder.TabIndex = 4;
            checkSortOrder.Text = "年龄排序";
            checkSortOrder.CheckedChanged += CheckSortOrder_CheckedChanged;
            // 
            // checkSetRowStyle
            // 
            checkSetRowStyle.AutoCheck = true;
            checkSetRowStyle.AutoSizeMode = AntdUI.TAutoSize.Width;
            checkSetRowStyle.Dock = DockStyle.Left;
            checkSetRowStyle.LocalizationText = "Table.{id}";
            checkSetRowStyle.Location = new Point(387, 0);
            checkSetRowStyle.Name = "checkSetRowStyle";
            checkSetRowStyle.Size = new Size(98, 43);
            checkSetRowStyle.TabIndex = 3;
            checkSetRowStyle.Text = "奇偶列";
            checkSetRowStyle.CheckedChanged += CheckSetRowStyle_CheckedChanged;
            // 
            // checkBordered
            // 
            checkBordered.AutoCheck = true;
            checkBordered.AutoSizeMode = AntdUI.TAutoSize.Width;
            checkBordered.Dock = DockStyle.Left;
            checkBordered.LocalizationText = "Table.{id}";
            checkBordered.Location = new Point(256, 0);
            checkBordered.Name = "checkBordered";
            checkBordered.Size = new Size(131, 43);
            checkBordered.TabIndex = 2;
            checkBordered.Text = "显示列边框";
            checkBordered.CheckedChanged += CheckBordered_CheckedChanged;
            // 
            // checkColumnDragSort
            // 
            checkColumnDragSort.AutoCheck = true;
            checkColumnDragSort.AutoSizeMode = AntdUI.TAutoSize.Width;
            checkColumnDragSort.Dock = DockStyle.Left;
            checkColumnDragSort.LocalizationText = "Table.{id}";
            checkColumnDragSort.Location = new Point(125, 0);
            checkColumnDragSort.Name = "checkColumnDragSort";
            checkColumnDragSort.Size = new Size(131, 43);
            checkColumnDragSort.TabIndex = 1;
            checkColumnDragSort.Text = "列拖拽排序";
            checkColumnDragSort.CheckedChanged += CheckColumnDragSort_CheckedChanged;
            // 
            // checkFixedHeader
            // 
            checkFixedHeader.AutoCheck = true;
            checkFixedHeader.AutoSizeMode = AntdUI.TAutoSize.Width;
            checkFixedHeader.Checked = true;
            checkFixedHeader.Dock = DockStyle.Left;
            checkFixedHeader.LocalizationText = "Table.{id}";
            checkFixedHeader.Location = new Point(10, 0);
            checkFixedHeader.Name = "checkFixedHeader";
            checkFixedHeader.Size = new Size(115, 43);
            checkFixedHeader.TabIndex = 0;
            checkFixedHeader.Text = "固定表头";
            checkFixedHeader.CheckedChanged += CheckFixedHeader_CheckedChanged;
            // 
            // table_Blackness_History
            // 
            table_Blackness_History.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            table_Blackness_History.Dock = DockStyle.Bottom;
            table_Blackness_History.Font = new Font("Microsoft YaHei UI", 11F);
            table_Blackness_History.Location = new Point(0, 128);
            table_Blackness_History.Name = "table_Blackness_History";
            table_Blackness_History.Radius = 6;
            table_Blackness_History.Size = new Size(1300, 508);
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
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private AntdUI.PageHeader header1;
        private AntdUI.Pagination pagination1;
        private AntdUI.Panel panel1;
        private AntdUI.Checkbox checkVisibleHeader;
        private AntdUI.Checkbox checkEnableHeaderResizing;
        private AntdUI.Checkbox checkSortOrder;
        private AntdUI.Checkbox checkSetRowStyle;
        private AntdUI.Checkbox checkBordered;
        private AntdUI.Checkbox checkColumnDragSort;
        private AntdUI.Checkbox checkFixedHeader;
        private AntdUI.Table table_Blackness_History;
    }
}