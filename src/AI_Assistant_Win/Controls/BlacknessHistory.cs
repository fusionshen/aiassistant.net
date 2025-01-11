using AI_Assistant_Win.Business;
using AI_Assistant_Win.Models;
using AI_Assistant_Win.Utils;
using AntdUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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
            if (!string.IsNullOrEmpty(BlacknessMethod.EDIT_METHOD_ID))
            {
                inputSearch.Text = BlacknessMethod.EDIT_METHOD_ID;
            }
        }

        private void InitializeTable()
        {
            // table_Blackness_History.EditMode = AntdUI.TEditMode.DoubleClick;
            pagination1.PageSizeOptions = [10, 20, 30, 50, 100];
            selectMultipleTableSetting.Items = [
                LocalizeHelper.DISPLAY_HEADER,
                LocalizeHelper.FIX_HEADER,
                LocalizeHelper.DISPLAY_COLUMN_BORDER,
                LocalizeHelper.ODD_AND_EVEN,
                LocalizeHelper.COLUMN_SORTING,
                LocalizeHelper.MANUALLY_ADJUST_COLUMN_WIDTH,
                LocalizeHelper.DRAG_COLUMN
            ];
            selectMultipleTableSetting.SelectedValue = [
                LocalizeHelper.DISPLAY_HEADER,
                LocalizeHelper.FIX_HEADER
                ];
            #region table header
            table_Blackness_History.Columns = [
                new AntdUI.ColumnCheck("check"){ Fixed = true },
                new AntdUI.Column("id",LocalizeHelper.BLACKNESS_TABLE_HEADER_ID, AntdUI.ColumnAlign.Center){ Fixed = true },
                new AntdUI.Column("testNo",LocalizeHelper.BLACKNESS_TABLE_HEADER_TESTNO, AntdUI.ColumnAlign.Center){ Fixed = true },
                new AntdUI.Column("size",LocalizeHelper.BLACKNESS_TABLE_HEADER_SIZE, AntdUI.ColumnAlign.Center){ Fixed = true },
                new AntdUI.Column("isOK","OK/NG", AntdUI.ColumnAlign.Center){ Fixed = true },
                new AntdUI.Column("isUploaded",LocalizeHelper.BLACKNESS_TABLE_HEADER_UPLOADED,AntdUI.ColumnAlign.Center){ Fixed = true },
                //new AntdUI.ColumnSwitch("isUploaded",LocalizeHelper.BLACKNESS_TABLE_HEADER_UPLOADED,AntdUI.ColumnAlign.Center)
                //{
                //    Fixed = true,
                //    Call=(value, record, i_row, i_col) => {
                //        System.Threading.Thread.Sleep(2000);
                //        return value;
                //    }
                //},
                new AntdUI.Column("coilNumber",LocalizeHelper.BLACKNESS_TABLE_HEADER_COILNUMBER, AntdUI.ColumnAlign.Center),
                new AntdUI.Column("levels",LocalizeHelper.BLACKNESS_TABLE_HEADER_LEVEL, AntdUI.ColumnAlign.Center),
                new AntdUI.Column("analyst",LocalizeHelper.BLACKNESS_TABLE_HEADER_ANALYST,AntdUI.ColumnAlign.Center),
                new AntdUI.Column("workGroup",LocalizeHelper.BLACKNESS_TABLE_HEADER_WORKGROUP,AntdUI.ColumnAlign.Center),
                new AntdUI.Column("createTime",LocalizeHelper.BLACKNESS_TABLE_HEADER_CREATETIME,AntdUI.ColumnAlign.Center),
                new AntdUI.Column("uploader",LocalizeHelper.BLACKNESS_TABLE_HEADER_UPLOADER,AntdUI.ColumnAlign.Center),
                new AntdUI.Column("uploadTime",LocalizeHelper.BLACKNESS_TABLE_HEADER_UPLOADTIME,AntdUI.ColumnAlign.Center),
                new AntdUI.Column("lastReviser",LocalizeHelper.BLACKNESS_TABLE_HEADER_LASTREVISER,AntdUI.ColumnAlign.Center),
                new AntdUI.Column("lastModifiedTime",LocalizeHelper.BLACKNESS_TABLE_HEADER_LASTMODIFIEDTIME,AntdUI.ColumnAlign.Center),
                new AntdUI.Column("btns",LocalizeHelper.BLACKNESS_TABLE_HEADER_OPERATIONS){ Fixed=true, Width="auto"},
            ];
            #endregion
            LoadData(pagination1.Current);  // 1
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
                if (e.RowIndex > 0 && e.ColumnIndex == 7) // levels
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
                        var renderImagePath = data.FirstOrDefault(t => "renderImage".Equals(t.key))?.value.ToString();
                        var originImagePath = data.FirstOrDefault(t => "originImage".Equals(t.key))?.value.ToString();
                        AntdUI.Preview.open(new AntdUI.Preview.Config(form, [Image.FromFile(renderImagePath), Image.FromFile(originImagePath)])
                        {
                            Btns = [new AntdUI.Preview.Btn("download", Properties.Resources.btn_download)],
                            OnBtns = (id, config) =>
                            {
                                switch (id)
                                {
                                    case "download":
                                        // 弹出文件保存对话框
                                        SaveFileDialog saveFileDialog = new()
                                        {
                                            Filter = "JPEG Image Files|*.jpg;*.jpeg|All Files|*.*",
                                            DefaultExt = "jpg",
                                            FileName = $"{data.FirstOrDefault(t => "coilNumber".Equals(t.key))?.value}_黑度检测结果.jpg",
                                            Title = LocalizeHelper.CHOOSE_THE_LOCATION
                                        };
                                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                                        {
                                            try
                                            {
                                                string pdfPath = saveFileDialog.FileName;
                                                var originImage = Image.FromFile(originImagePath);
                                                originImage.Save(saveFileDialog.FileName.Replace(".jpg", "_原图.jpg"), ImageFormat.Jpeg);
                                                var renderImage = Image.FromFile(renderImagePath);
                                                renderImage.Save(saveFileDialog.FileName.Replace(".jpg", "_识别图.jpg"), ImageFormat.Jpeg);
                                                AntdUI.Notification.success(form, LocalizeHelper.SUCCESS, LocalizeHelper.FILE_SAVED_LOCATION + pdfPath,
                                                    AntdUI.TAlignFrom.BR, Font);
                                            }
                                            catch (Exception error)
                                            {
                                                AntdUI.Notification.error(form, LocalizeHelper.ERROR, error.Message, AntdUI.TAlignFrom.BR, Font);
                                            }
                                        }
                                        break;
                                }
                            }
                        });
                        break;
                    case "report":
                        var id = data.FirstOrDefault(t => "id".Equals(t.key))?.value.ToString();
                        try
                        {
                            AntdUI.Drawer.open(form, new BlacknessReport(form, id, () => { BtnSearch_Click(null, null); })
                            {
                                Size = new Size(420, 596)  // 常用到的纸张规格为A4，即21cm×29.7cm（210mm×297mm）
                            }, AntdUI.TAlignMini.Right);
                        }
                        catch (Exception error)
                        {
                            AntdUI.Notification.error(form, LocalizeHelper.ERROR, error.Message, AntdUI.TAlignFrom.BR, Font);
                        }
                        break;
                    case "edit":
                        if (AntdUI.Modal.open(form, LocalizeHelper.CONFIRM, LocalizeHelper.WOULD_EDIT_BLACKNESS_RESULT) == DialogResult.OK)
                        {
                            try
                            {
                                BlacknessMethod.EDIT_METHOD_ID = data.FirstOrDefault(t => "id".Equals(t.key))?.value.ToString();
                                ((MainWindow)form).OpenPage("V60 Blackness Method On GA Sheet");
                            }
                            catch (Exception error)
                            {
                                AntdUI.Notification.error(form, LocalizeHelper.ERROR, error.Message, AntdUI.TAlignFrom.BR, Font);
                            }
                        }
                        break;
                    case "delete":
                        AntdUI.Notification.warn(form, LocalizeHelper.PROMPT, LocalizeHelper.UNDER_DEVELOPMENT, AntdUI.TAlignFrom.BR, Font);
                        break;
                    default:
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
                    new("testNo", t.TestNo),
                    new("size", t.Size),
                    new("isOK", t.IsOK ? new AntdUI.CellBadge(TState.Success, "OK") : new AntdUI.CellBadge(TState.Error, "NG")),
                    new("coilNumber", t.CoilNumber),
                    new("isUploaded", CreateIsUploadedCellBadge(t)),
                    //new("isUploaded", t.IsUploaded),
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
                    new AntdUI.CellButton("preview") { BorderWidth = 1, IconSvg = "PlayCircleOutlined", ShowArrow = true, Tooltip = LocalizeHelper.PREVIEW_IMAGE},
                    new AntdUI.CellButton("report") { BorderWidth = 1, IconSvg = "SnippetsOutlined", ShowArrow = true, Tooltip = LocalizeHelper.REPORT},
                    new AntdUI.CellButton("edit")  { BorderWidth = 1, IconSvg = "EditOutlined", ShowArrow = true, Tooltip = LocalizeHelper.EDIT,
                        Type= AntdUI.TTypeMini.Warn},
                    new AntdUI.CellButton("delete")  { BorderWidth = 1, IconSvg = "DeleteOutlined", ShowArrow = true, Tooltip = LocalizeHelper.DELETE,
                        Type= AntdUI.TTypeMini.Error},
                ];
                columnsInOneRow.Add(new AntdUI.AntItem("btns", btns));
                return columnsInOneRow.ToArray();
            }).ToList();
            return dataList;
        }

        private static CellBadge CreateIsUploadedCellBadge(BlacknessMethodResult t)
        {
            if (!t.IsUploaded)
            {
                return new AntdUI.CellBadge(AntdUI.TState.Error, LocalizeHelper.BLACKNESS_RESULT_NOT_UPLOADED);
            }
            else
            {
                if (t.LastModifiedTime != null && t.LastModifiedTime >= t.UploadTime)
                {
                    return new AntdUI.CellBadge(AntdUI.TState.Processing, LocalizeHelper.BLACKNESS_RESULT_AWAITING_REUPLOAD);
                }
                else
                {
                    return new AntdUI.CellBadge(AntdUI.TState.Success, LocalizeHelper.BLACKNESS_RESULT_UPLOADED);
                }
            }
        }

        private string FormatLevelDetail(BlacknessMethodResult blacknessMethodResult)
        {
            return $"{LocalizeHelper.SURFACE_OP}{LocalizeHelper.LEVEL}{blacknessMethodResult.SurfaceOPLevel}{LocalizeHelper.BLACKNESS_WITH}{blacknessMethodResult.SurfaceOPWidth:F2}{LocalizeHelper.MILLIMETER}\n" +
                   $"{LocalizeHelper.SURFACE_CE}{LocalizeHelper.LEVEL}{blacknessMethodResult.SurfaceCELevel}{LocalizeHelper.BLACKNESS_WITH}{blacknessMethodResult.SurfaceCEWidth:F2}{LocalizeHelper.MILLIMETER}\n" +
                   $"{LocalizeHelper.SURFACE_DR}{LocalizeHelper.LEVEL}{blacknessMethodResult.SurfaceDRLevel}{LocalizeHelper.BLACKNESS_WITH}{blacknessMethodResult.SurfaceDRWidth:F2}{LocalizeHelper.MILLIMETER}\n" +
                   $"{LocalizeHelper.INSIDE_OP}{LocalizeHelper.LEVEL}{blacknessMethodResult.InsideOPLevel}{LocalizeHelper.BLACKNESS_WITH}{blacknessMethodResult.InsideOPWidth:F2}{LocalizeHelper.MILLIMETER}\n" +
                   $"{LocalizeHelper.INSIDE_CE}{LocalizeHelper.LEVEL}{blacknessMethodResult.InsideCELevel}{LocalizeHelper.BLACKNESS_WITH}{blacknessMethodResult.InsideCEWidth:F2}{LocalizeHelper.MILLIMETER}\n" +
                   $"{LocalizeHelper.INSIDE_DR}{LocalizeHelper.LEVEL}{blacknessMethodResult.InsideDRLevel}{LocalizeHelper.BLACKNESS_WITH}{blacknessMethodResult.InsideDRWidth:F2}{LocalizeHelper.MILLIMETER}";
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
                    return new AntdUI.CellTag($"{LocalizeHelper.LEVEL}{t}", (AntdUI.TTypeMini)intValue);
                }
                return new AntdUI.CellTag($"{LocalizeHelper.LEVEL}{t}", AntdUI.TTypeMini.Default);
            }).ToList();
            return tagList;
        }

        void Pagination1_ValueChanged(object sender, AntdUI.PagePageEventArgs e)
        {
            table_Blackness_History.DataSource = GetPageData(e.Current, e.PageSize);
        }

        private string Pagination1_ShowTotalChanged(object sender, AntdUI.PagePageEventArgs e)
        {
            return $"{e.PageSize} / {e.Total} {LocalizeHelper.GONG} {e.PageTotal} {LocalizeHelper.YE}";
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
        private void SelectMultipleTableSettingSelectedValueChanged(object sender, AntdUI.ObjectsEventArgs e)
        {
            var visibleHeader = e.Value.Any(t => LocalizeHelper.DISPLAY_HEADER.Equals(t.ToString()));
            CheckVisibleHeader_CheckedChanged(null, new AntdUI.BoolEventArgs(visibleHeader));
            var fixedHeader = e.Value.Any(t => LocalizeHelper.FIX_HEADER.Equals(t.ToString()));
            CheckFixedHeader_CheckedChanged(null, new AntdUI.BoolEventArgs(fixedHeader));
            var bordered = e.Value.Any(t => LocalizeHelper.DISPLAY_COLUMN_BORDER.Equals(t.ToString()));
            CheckBordered_CheckedChanged(null, new AntdUI.BoolEventArgs(bordered));
            var setRowStyle = e.Value.Any(t => LocalizeHelper.ODD_AND_EVEN.Equals(t.ToString()));
            CheckSetRowStyle_CheckedChanged(null, new AntdUI.BoolEventArgs(setRowStyle));
            var sortOrder = e.Value.Any(t => LocalizeHelper.COLUMN_SORTING.Equals(t.ToString()));
            CheckSortOrder_CheckedChanged(null, new AntdUI.BoolEventArgs(sortOrder));
            var enableHeaderResizing = e.Value.Any(t => LocalizeHelper.MANUALLY_ADJUST_COLUMN_WIDTH.Equals(t.ToString()));
            CheckEnableHeaderResizing_CheckedChanged(null, new AntdUI.BoolEventArgs(enableHeaderResizing));
            var columnDragSort = e.Value.Any(t => LocalizeHelper.DRAG_COLUMN.Equals(t.ToString()));
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