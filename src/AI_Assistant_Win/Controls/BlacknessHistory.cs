using AI_Assistant_Win.Business;
using AI_Assistant_Win.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AI_Assistant_Win.Controls
{
    public partial class BlacknessHistory : UserControl
    {
        private readonly Form form;

        private readonly BlacknessMethodBLL blacknessMethodBLL;

        private DateTime? startDate = null;

        private DateTime? endDate = null;
        public BlacknessHistory(Form _form)
        {
            form = _form;
            blacknessMethodBLL = new BlacknessMethodBLL();
            InitializeComponent();
            InitializeSearch();
            InitializeTable();
        }

        private void InitializeSearch()
        {
            inputRangeDate.MaxDate = DateTime.Now;
            if (!string.IsNullOrEmpty(BlacknessMethod.EDIT_ITEM_ID))
            {
                inputSearch.Text = BlacknessMethod.EDIT_ITEM_ID;
            }
        }

        private void InitializeTable()
        {
            // table_Blackness_History.EditMode = AntdUI.TEditMode.DoubleClick;
            pagination1.PageSizeOptions = [10, 20, 30, 50, 100];
            selectMultiple_Table_Setting.SelectedValue = ["显示表头", "固定表头"];
            #region table header
            table_Blackness_History.Columns = [
                new AntdUI.ColumnCheck("check"){ Fixed = true },
                new AntdUI.Column("id","编号", AntdUI.ColumnAlign.Center){ Fixed = true },
                new AntdUI.Column("coilNumber","钢卷号", AntdUI.ColumnAlign.Center){ Fixed = true },
                new AntdUI.Column("size","尺寸", AntdUI.ColumnAlign.Center){ Fixed = true },
                new AntdUI.Column("isOK","OK/NG", AntdUI.ColumnAlign.Center){ Fixed = true },
                new AntdUI.ColumnSwitch("isUploaded","已上传",AntdUI.ColumnAlign.Center){ Fixed = true, Call=(value, record, i_row, i_col)=>{
                    System.Threading.Thread.Sleep(2000);
                    return value;
                } },
                new AntdUI.Column("levels","等级", AntdUI.ColumnAlign.Center),
                new AntdUI.Column("analyst","分析人",AntdUI.ColumnAlign.Center),
                new AntdUI.Column("workGroup","班组",AntdUI.ColumnAlign.Center),
                new AntdUI.Column("createTime","创建时间",AntdUI.ColumnAlign.Center),
                new AntdUI.Column("uploader","上传人",AntdUI.ColumnAlign.Center),
                new AntdUI.Column("uploadTime","上传时间",AntdUI.ColumnAlign.Center),
                new AntdUI.Column("lastReviser","最后修改人",AntdUI.ColumnAlign.Center),
                new AntdUI.Column("lastModifiedTime","最后修改时间",AntdUI.ColumnAlign.Center),
                // new AntdUI.Column("address","地址"){ Width="120", LineBreak=true},
                new AntdUI.Column("btns","操作"){ Fixed=true, Width="auto"},
            ];
            #endregion
            #region 数据
            LoadData(pagination1.Current);  // 1
            #endregion
        }

        private void LoadData(int current)
        {
            var pagedList = GetPageData(current, pagination1.PageSize);
            table_Blackness_History.DataSource = pagedList;
        }
        #region 单元格事件
        void Table1_CellClick(object sender, AntdUI.TableClickEventArgs e)
        {
            // if enable dragging column, columnIndex will change. but e only have the attribute: columnIndex, so must disable it.:)
            if (e.Record is IList<AntdUI.AntItem> data)
            {
                if (e.RowIndex > 0 && e.ColumnIndex == 6) // levels
                {
                    var levelDatail = data.FirstOrDefault(t => "levelDetail".Equals(t.key))?.value.ToString();
                    AntdUI.Popover.open(new AntdUI.Popover.Config(table_Blackness_History, levelDatail) { Offset = e.Rect });
                }
            }
        }
        void Table1_CellButtonClick(object sender, AntdUI.TableButtonEventArgs e)
        {
            if (e.Record is IList<AntdUI.AntItem> data)
            {
                switch (e.Btn.Id)
                {
                    case "preview":
                        var renderImage = data.FirstOrDefault(t => "renderImage".Equals(t.key))?.value.ToString();
                        var originImage = data.FirstOrDefault(t => "originImage".Equals(t.key))?.value.ToString();
                        AntdUI.Preview.open(new AntdUI.Preview.Config(form, [Image.FromFile(renderImage), Image.FromFile(originImage)])
                        {
                            Btns = [new AntdUI.Preview.Btn("download", Properties.Resources.btn_download)],
                            OnBtns = (id, config) =>
                            {
                                switch (id)
                                {
                                    case "download":
                                        // TODO: download image
                                        break;
                                }
                            }
                        });
                        break;
                    case "report":
                        var id = data.FirstOrDefault(t => "id".Equals(t.key))?.value.ToString();
                        try
                        {
                            AntdUI.Drawer.open(form, new BlacknessReport(form, id)
                            {
                                Size = new Size(420, 596)  // 常用到的纸张规格为A4，即21cm×29.7cm（210mm×297mm）
                            }, AntdUI.TAlignMini.Right);
                        }
                        catch (Exception error)
                        {
                            AntdUI.Notification.error(form, "错误", error.Message, AntdUI.TAlignFrom.BR, Font);
                        }
                        break;
                    default:
                        // TODO: uploaded
                        if (AntdUI.Modal.open(form, "请确认", "是否对本次黑度检测结果进行修改？") == DialogResult.OK)
                        {
                            try
                            {
                                BlacknessMethod.EDIT_ITEM_ID = data.FirstOrDefault(t => "id".Equals(t.key))?.value.ToString();
                                ((MainWindow)form).OpenPage("V60 Blackness Method On GA Sheet");
                            }
                            catch (Exception error)
                            {
                                AntdUI.Notification.error(form, "错误", error.Message, AntdUI.TAlignFrom.BR, Font);
                            }
                            break;
                        }
                        break;
                }

            }
        }
        #endregion
        #region 获取页面数据
        List<AntdUI.AntItem[]> GetPageData(int current, int pageSize)
        {
            // startDate endDate keywords
            var dbList = blacknessMethodBLL.GetResultListByConditions(startDate, endDate, inputSearch.Text);
            pagination1.Total = dbList.Count;
            // lower memory coss
            var pagedList = dbList.Skip(pageSize * Math.Abs(current - 1)).Take(pageSize).ToList();
            // format table items
            var dataList = pagedList.Select(t =>
            {
                // single row
                var columnsInOneRow = new List<AntdUI.AntItem>
                {
                    new("check", false),
                    new("id", t.Id),
                    new("coilNumber", t.CoilNumber),
                    new("size", t.Size),
                    new("isOK", t.IsOK ? new AntdUI.CellBadge(AntdUI.TState.Success, "OK") :
                    new AntdUI.CellBadge(AntdUI.TState.Error, "NG")),
                    new("isUploaded", t.IsUploaded),
                    new("levels", FormatCellTagList(t).ToArray()),
                    new("analyst", t.Analyst),
                    new("workGroup", t.WorkGroup),
                    new("createTime", t.CreateTime),
                    new("uploader", t.Uploader),
                    new("uploadTime", t.UploadTime),
                    new("lastReviser", t.LastReviser),
                    new("lastModifiedTime", t.LastModifiedTime),
                    // for preview
                    new("originImage", t.OriginImagePath),
                    new("renderImage", t.RenderImagePath),
                    // for levels cell click
                    new("levelDetail", FormatLevelDetail(t)),

                };
                // 预览、报告(导出/打印)、修改、删除
                AntdUI.CellLink[] btns = [
                    new AntdUI.CellButton("preview") { BorderWidth = 1, IconSvg = "PlayCircleOutlined", ShowArrow = true, Tooltip = "图像"},
                    new AntdUI.CellButton("report") { BorderWidth = 1, IconSvg = "SnippetsOutlined", ShowArrow = true, Tooltip = "报告"},
                    new AntdUI.CellButton("edit")  { BorderWidth = 1, IconSvg = "EditOutlined", ShowArrow = true, Tooltip = "修改", Type= AntdUI.TTypeMini.Warn},
                    new AntdUI.CellButton("delete")  { BorderWidth = 1, IconSvg = "DeleteOutlined", ShowArrow = true, Tooltip = "删除", Type= AntdUI.TTypeMini.Error},
                ];
                columnsInOneRow.Add(new AntdUI.AntItem("btns", btns));
                return columnsInOneRow.ToArray();
            }).ToList();
            return dataList;
        }

        private string FormatLevelDetail(BlacknessMethodResult blacknessMethodResult)
        {
            return $"表面OP：等级{blacknessMethodResult.SurfaceOPLevel}，宽度{blacknessMethodResult.SurfaceOPWidth:F2}mm\n" +
                   $"表面CE：等级{blacknessMethodResult.SurfaceCELevel}，宽度{blacknessMethodResult.SurfaceCEWidth:F2}mm\n" +
                   $"表面DR：等级{blacknessMethodResult.SurfaceDRLevel}，宽度{blacknessMethodResult.SurfaceDRWidth:F2}mm\n" +
                   $"里面OP：等级{blacknessMethodResult.InsideOPLevel}，宽度{blacknessMethodResult.InsideOPWidth:F2}mm\n" +
                   $"里面CE：等级{blacknessMethodResult.InsideCELevel}，宽度{blacknessMethodResult.InsideCEWidth:F2}mm\n" +
                   $"里面DR：等级{blacknessMethodResult.InsideDRLevel}，宽度{blacknessMethodResult.InsideDRWidth:F2}mm";
        }

        private List<AntdUI.CellTag> FormatCellTagList(BlacknessMethodResult blacknessMethodResult)
        {
            // 各部位等级列表
            var levelList = new List<string>
            {
                blacknessMethodResult.SurfaceOPLevel,
                blacknessMethodResult.SurfaceCELevel,
                blacknessMethodResult.SurfaceDRLevel,
                blacknessMethodResult.InsideOPLevel,
                blacknessMethodResult.InsideCELevel,
                blacknessMethodResult.InsideDRLevel
            }.Where(t => !string.IsNullOrEmpty(t)).Distinct().Order().ToList();
            var tagList = levelList.Select(t =>
            {
                if (int.TryParse(t, out int intValue))
                {
                    return new AntdUI.CellTag($"等级{t}", (AntdUI.TTypeMini)intValue);
                }
                return new AntdUI.CellTag($"等级{t}", AntdUI.TTypeMini.Default);
            }).ToList();
            return tagList;
        }

        void Pagination1_ValueChanged(object sender, AntdUI.PagePageEventArgs e)
        {
            table_Blackness_History.DataSource = GetPageData(e.Current, e.PageSize);
        }

        private string Pagination1_ShowTotalChanged(object sender, AntdUI.PagePageEventArgs e)
        {
            return $"{e.PageSize} / {e.Total} 共 {e.PageTotal} 页";
        }
        #endregion
        #region 表格设置
        /// <summary>
        /// 显示表头
        /// 固定表头
        /// 显示列边框
        /// 奇偶列
        /// 部分列排序
        /// 手动调整列头宽度
        /// 列拖拽
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectMultiple_Table_Setting_SelectedValueChanged(object sender, AntdUI.ObjectsEventArgs e)
        {
            var visibleHeader = e.Value.Any(t => "显示表头".Equals(t.ToString()));
            CheckVisibleHeader_CheckedChanged(null, new AntdUI.BoolEventArgs(visibleHeader));
            var fixedHeader = e.Value.Any(t => "固定表头".Equals(t.ToString()));
            CheckFixedHeader_CheckedChanged(null, new AntdUI.BoolEventArgs(fixedHeader));
            var bordered = e.Value.Any(t => "显示列边框".Equals(t.ToString()));
            CheckBordered_CheckedChanged(null, new AntdUI.BoolEventArgs(bordered));
            var setRowStyle = e.Value.Any(t => "奇偶列".Equals(t.ToString()));
            CheckSetRowStyle_CheckedChanged(null, new AntdUI.BoolEventArgs(setRowStyle));
            var sortOrder = e.Value.Any(t => "部分列排序".Equals(t.ToString()));
            CheckSortOrder_CheckedChanged(null, new AntdUI.BoolEventArgs(sortOrder));
            var enableHeaderResizing = e.Value.Any(t => "手动调整列头宽度".Equals(t.ToString()));
            CheckEnableHeaderResizing_CheckedChanged(null, new AntdUI.BoolEventArgs(enableHeaderResizing));
            var columnDragSort = e.Value.Any(t => "列拖拽".Equals(t.ToString()));
            CheckColumnDragSort_CheckedChanged(null, new AntdUI.BoolEventArgs(columnDragSort));
        }
        void CheckVisibleHeader_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            table_Blackness_History.VisibleHeader = e.Value;
        }
        void CheckFixedHeader_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            table_Blackness_History.FixedHeader = e.Value;
        }
        void CheckBordered_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            table_Blackness_History.Bordered = e.Value;
        }
        #region 行状态
        void CheckSetRowStyle_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            if (e.Value) table_Blackness_History.SetRowStyle += Table1_SetRowStyle;
            else table_Blackness_History.SetRowStyle -= Table1_SetRowStyle;
            table_Blackness_History.Invalidate();
        }
        AntdUI.Table.CellStyleInfo Table1_SetRowStyle(object sender, AntdUI.TableSetRowStyleEventArgs e)
        {
            if (e.RowIndex % 2 == 0)
            {
                return new AntdUI.Table.CellStyleInfo
                {
                    BackColor = AntdUI.Style.Db.ErrorBg,
                    ForeColor = AntdUI.Style.Db.Error
                };
            }
            return null;
        }
        #endregion
        void CheckSortOrder_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            if (table_Blackness_History.Columns != null) table_Blackness_History.Columns[2].SortOrder = table_Blackness_History.Columns[3].SortOrder = e.Value;
        }
        void CheckEnableHeaderResizing_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            table_Blackness_History.EnableHeaderResizing = e.Value;
        }
        void CheckColumnDragSort_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            table_Blackness_History.ColumnDragSort = e.Value;
        }
        #endregion

        private void InputRangeDate_ValueChanged(object sender, AntdUI.DateTimesEventArgs e)
        {
            startDate = e.Value[0];
            endDate = e.Value[1];
            BtnSearch_Click(null, null);
        }

        private void BtnReload_Click(object sender, EventArgs e)
        {
            inputRangeDate.Clear();
            startDate = null;
            endDate = null;
            BtnSearch_Click(null, null);
        }

        private void InputSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // 这里处理Enter键被按下的情况
                // MessageBox.Show("Enter键被按下！");
                BtnSearch_Click(null, null);
                // 如果你想阻止Enter键的默认行为（例如，发出“叮”声或移动焦点），可以调用e.SuppressKeyPress()
                // 但通常对于TextBox，你可能不希望阻止它，因为用户可能期望焦点移动到下一个控件
                // 如果你确实想阻止默认行为，请取消注释以下行：
                // e.SuppressKeyPress();
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            LoadData(1);
        }
    }
}