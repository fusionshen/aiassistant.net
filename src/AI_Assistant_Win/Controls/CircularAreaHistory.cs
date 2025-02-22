using AI_Assistant_Win.Business;
using AI_Assistant_Win.Models;
using AI_Assistant_Win.Models.Enums;
using AI_Assistant_Win.Utils;
using AntdUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;
using YoloDotNet.Extensions;

namespace AI_Assistant_Win.Controls
{
    public partial class CircularAreaHistory : UserControl
    {
        private readonly Form form;

        private readonly CircularAreaMethodBLL circularAreaMethodBLL;

        private DateTime? startDate = null;

        private DateTime? endDate = null;
        public CircularAreaHistory(Form _form)
        {
            form = _form;
            circularAreaMethodBLL = new CircularAreaMethodBLL();
            InitializeComponent();
            InitializeSearch();
            InitializeTable();
        }

        private void InitializeSearch()
        {
            inputRangeDate.MaxDate = DateTime.Now;
            if (!string.IsNullOrEmpty(CircularAreaMethod.EDIT_ITEM_ID))
            {
                var result = circularAreaMethodBLL.GetResultById(CircularAreaMethod.EDIT_ITEM_ID);
                if (result != null)
                {
                    inputSearch.Text = result.TestNo;
                }
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
            tableCircularAreaHistory.Columns = [
                new AntdUI.ColumnCheck("check"){ Fixed = true },
                new AntdUI.Column("id",LocalizeHelper.TABLE_HEADER_ID, AntdUI.ColumnAlign.Center){ Fixed = true },
                new AntdUI.Column("testNo",LocalizeHelper.TABLE_HEADER_TESTNO, AntdUI.ColumnAlign.Center){ Fixed = true },
                new AntdUI.Column("coilNumber",LocalizeHelper.TABLE_HEADER_COILNUMBER, AntdUI.ColumnAlign.Center){ Fixed = true },
                new AntdUI.Column("isUploaded",LocalizeHelper.TABLE_HEADER_UPLOADED,AntdUI.ColumnAlign.Center){ Fixed = true },
                //new AntdUI.ColumnSwitch("isUploaded",LocalizeHelper.TABLE_HEADER_UPLOADED,AntdUI.ColumnAlign.Center)
                //{
                //    Fixed = true,
                //    Call=(value, record, i_row, i_col) => {
                //        System.Threading.Thread.Sleep(2000);
                //        return value;
                //    }
                //},
                new AntdUI.Column("upperSurfaceOP",LocalizeHelper.TABLE_UPPER_SURFACE_OP, AntdUI.ColumnAlign.Center),
                new AntdUI.Column("upperSurfaceCE",LocalizeHelper.TABLE_UPPER_SURFACE_CE, AntdUI.ColumnAlign.Center),
                new AntdUI.Column("upperSurfaceDR",LocalizeHelper.TABLE_UPPER_SURFACE_DR, AntdUI.ColumnAlign.Center),
                new AntdUI.Column("lowerSurfaceOP",LocalizeHelper.TABLE_LOWER_SURFACE_OP, AntdUI.ColumnAlign.Center),
                new AntdUI.Column("lowerSurfaceCE",LocalizeHelper.TABLE_LOWER_SURFACE_CE, AntdUI.ColumnAlign.Center),
                new AntdUI.Column("lowerSurfaceDR",LocalizeHelper.TABLE_LOWER_SURFACE_DR, AntdUI.ColumnAlign.Center),
                new AntdUI.Column("creator",LocalizeHelper.TABLE_HEADER_CREATOR,AntdUI.ColumnAlign.Center),
                new AntdUI.Column("createTime",LocalizeHelper.TABLE_HEADER_CREATETIME,AntdUI.ColumnAlign.Center),
                new AntdUI.Column("uploader",LocalizeHelper.TABLE_HEADER_UPLOADER,AntdUI.ColumnAlign.Center),
                new AntdUI.Column("uploadTime",LocalizeHelper.TABLE_HEADER_UPLOADTIME,AntdUI.ColumnAlign.Center),
                new AntdUI.Column("lastReviser",LocalizeHelper.TABLE_HEADER_LASTREVISER,AntdUI.ColumnAlign.Center),
                new AntdUI.Column("lastModifiedTime",LocalizeHelper.TABLE_HEADER_LASTMODIFIEDTIME,AntdUI.ColumnAlign.Center),
                new AntdUI.Column("btns",LocalizeHelper.TABLE_HEADER_OPERATIONS){ Fixed=true, Width="auto"},
            ];
            #endregion
            LoadData(pagination1.Current);  // 1
        }

        private void LoadData(int current)
        {
            var pagedList = GetPageData(current, pagination1.PageSize);
            tableCircularAreaHistory.DataSource = pagedList;
        }
        #region 单元格事件
        void Table1_CellClick(object sender, AntdUI.TableClickEventArgs e)
        {
            // if enable dragging column, columnIndex will change. but e only have the attribute: columnIndex, so must disable it.:)
            if (e.Record is IList<AntdUI.AntItem> data && e.RowIndex > 0)
            {
                if (data.FirstOrDefault(t => "methodList".Equals(t.key))?.value is List<CircularAreaMethodResult> methodList)
                {
                    string postionDetail = string.Empty;
                    switch (e.ColumnIndex)
                    {
                        case 5:
                            postionDetail = FormatPositionDetail(methodList, CircularPositionKind.UPPER_SURFACE_OP);
                            break;
                        case 6:
                            postionDetail = FormatPositionDetail(methodList, CircularPositionKind.UPPER_SURFACE_CE);
                            break;
                        case 7:
                            postionDetail = FormatPositionDetail(methodList, CircularPositionKind.UPPER_SURFACE_DR);
                            break;
                        case 8:
                            postionDetail = FormatPositionDetail(methodList, CircularPositionKind.LOWER_SURFACE_OP);
                            break;
                        case 9:
                            postionDetail = FormatPositionDetail(methodList, CircularPositionKind.LOWER_SURFACE_CE);
                            break;
                        case 10:
                            postionDetail = FormatPositionDetail(methodList, CircularPositionKind.LOWER_SURFACE_DR);
                            break;
                        default:
                            break;
                    }
                    if (!string.IsNullOrEmpty(postionDetail))
                    {
                        AntdUI.Popover.open(new AntdUI.Popover.Config(tableCircularAreaHistory, postionDetail) { Offset = e.Rect });
                    }
                }
            }
        }

        private string FormatPositionDetail(List<CircularAreaMethodResult> methodList, CircularPositionKind positionEnum)
        {
            string text = string.Empty;
            var method = methodList.FirstOrDefault(t => positionEnum.Equals(t.Position));
            if (method != null)
            {
                text = $"{LocalizeHelper.CIRCULAR_POSITION_TITLE}{LocalizeHelper.CIRCULAR_POSITION(method.Position)}\n" +
                    $"{LocalizeHelper.CELL_AREA_OF_PIXELS}{method.Pixels}" +
                    $"{LocalizeHelper.AREA_PREDICTION_CONFIDENCE}{method.Confidence.ToPercent()}%\n" +
                    $"{LocalizeHelper.AREA_PREDICTION_TITLE}{method.Area:F2}{LocalizeHelper.SQUARE_MILLIMETER}\n" +
                    $"{LocalizeHelper.CIRCULAR_AREA_DIAMETER}{method.Diameter:F2}{LocalizeHelper.MILLIMETER}\n" +
                    $"{LocalizeHelper.CELL_TITLE_ANALYST}{method.Analyst}\n" +
                    $"{LocalizeHelper.CELL_HEADER_CREATETIME}{method.CreateTime}\n" +
                    $"{LocalizeHelper.CELL_HEADER_LASTREVISER}{method.LastReviser}\n" +
                    $"{LocalizeHelper.CELL_HEADER_LASTMODIFIEDTIME}{method.LastModifiedTime}";
            }
            return text;
        }

        void Table1_CellButtonClick(object sender, AntdUI.TableButtonEventArgs e)
        {
            if (e.Record is IList<AntdUI.AntItem> data)
            {
                switch (e.Btn.Id)
                {
                    case "preview":
                        if (data.FirstOrDefault(t => "methodList".Equals(t.key))?.value is List<CircularAreaMethodResult> methodList)
                        {
                            try
                            {
                                var imageList = methodList.SelectMany(t => new List<Image> { Image.FromFile(t.OriginImagePath), Image.FromFile(t.RenderImagePath) }).ToList();
                                AntdUI.Preview.open(new AntdUI.Preview.Config(form, [.. imageList])
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
                                                    FileName = $"{data.FirstOrDefault(t => "testNo".Equals(t.key))?.value}_圆片面积检测结果.jpg",
                                                    Title = LocalizeHelper.CHOOSE_THE_LOCATION
                                                };
                                                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                                                {

                                                    string pdfPath = saveFileDialog.FileName;
                                                    methodList.ForEach(t =>
                                                {
                                                    var originImage = Image.FromFile(t.OriginImagePath);
                                                    originImage.Save(saveFileDialog.FileName
                                                        .Replace(".jpg", $"_{LocalizeHelper.CIRCULAR_POSITION(t.Position)}_原图.jpg"), ImageFormat.Jpeg);
                                                    var renderImage = Image.FromFile(t.RenderImagePath);
                                                    renderImage.Save(saveFileDialog.FileName
                                                        .Replace(".jpg", $"_{LocalizeHelper.CIRCULAR_POSITION(t.Position)}_识别图.jpg"), ImageFormat.Jpeg);
                                                });
                                                    AntdUI.Message.success(form, LocalizeHelper.FILE_SAVED_LOCATION + pdfPath);

                                                }
                                                break;
                                        }
                                    }
                                });
                            }
                            catch (Exception error)
                            {
                                AntdUI.Notification.error(form, LocalizeHelper.ERROR, error.Message, AntdUI.TAlignFrom.BR, Font);
                            }
                        }

                        break;
                    case "report":
                        var testNo = data.FirstOrDefault(t => "testNo".Equals(t.key))?.value.ToString();
                        try
                        {
                            AntdUI.Drawer.open(form, new CircularAreaReport(form, testNo, () => { BtnSearch_Click(null, null); })
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
                        if (AntdUI.Modal.open(form, LocalizeHelper.CONFIRM, LocalizeHelper.WOULD_EDIT_CIRCULAR_AREA_RESULT) == DialogResult.OK)
                        {
                            try
                            {
                                if (data.FirstOrDefault(t => "methodList".Equals(t.key))?.value is List<CircularAreaMethodResult> methodList1)
                                {
                                    CircularAreaMethod.EDIT_ITEM_ID = methodList1.FirstOrDefault()?.Id.ToString();
                                    ((MainWindow)form).OpenPage("Circular Area Measurement On Galvanized Sheet");
                                }

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
            var dbList = circularAreaMethodBLL.GetSummaryListByConditions(startDate, endDate, inputSearch.Text);
            pagination1.Total = dbList.Count;
            // lower memory coss
            var pagedList = dbList.Skip(pageSize * Math.Abs(current - 1)).Take(pageSize).ToList();
            // format table items
            var dataList = pagedList.Select(t =>
            {
                // single row
                var columnsInOneRow = new List<AntItem>
                {
                    new("check", false),
                    new("id", t.Summary.Id),
                    new("testNo", t.Summary.TestNo),
                    new("coilNumber", t.Summary.CoilNumber),
                    new("isUploaded", CreateIsUploadedCellBadge(t.Summary)),
                    new("upperSurfaceOP", FormatPositionArea(t.MethodList, CircularPositionKind.UPPER_SURFACE_OP)),
                    new("upperSurfaceCE", FormatPositionArea(t.MethodList, CircularPositionKind.UPPER_SURFACE_CE)),
                    new("upperSurfaceDR", FormatPositionArea(t.MethodList, CircularPositionKind.UPPER_SURFACE_DR)),
                    new("lowerSurfaceOP", FormatPositionArea(t.MethodList, CircularPositionKind.LOWER_SURFACE_OP)),
                    new("lowerSurfaceCE", FormatPositionArea(t.MethodList, CircularPositionKind.LOWER_SURFACE_CE)),
                    new("lowerSurfaceDR", FormatPositionArea(t.MethodList, CircularPositionKind.LOWER_SURFACE_DR)),
                    new("creator", t.Summary.Creator),
                    new("createTime", t.Summary.CreateTime),
                    new("uploader", t.Summary.Uploader),
                    new("uploadTime", t.Summary.UploadTime),
                    new("lastReviser", t.Summary.LastReviser),
                    new("lastModifiedTime", t.Summary.LastModifiedTime),
                    // for preview
                    // for details cell click
                    new("methodList", t.MethodList),

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

        private static string FormatPositionArea(List<CircularAreaMethodResult> methodList, CircularPositionKind positionEnum)
        {
            var area = methodList.FirstOrDefault(x => positionEnum.Equals(x.Position))?.Area;
            if (area != null)
            {
                return $"{area:F2}{LocalizeHelper.SQUARE_MILLIMETER}";
            }
            else
            {
                return string.Empty;
            }
        }

        private static CellBadge CreateIsUploadedCellBadge(CircularAreaMethodSummary t)
        {
            if (!t.IsUploaded)
            {
                return new AntdUI.CellBadge(AntdUI.TState.Error, LocalizeHelper.RESULT_NOT_UPLOADED);
            }
            else
            {
                if (t.LastModifiedTime != null && t.LastModifiedTime >= t.UploadTime)
                {
                    return new AntdUI.CellBadge(AntdUI.TState.Processing, LocalizeHelper.RESULT_WAITING_REUPLOAD);
                }
                else
                {
                    return new AntdUI.CellBadge(AntdUI.TState.Success, LocalizeHelper.RESULT_UPLOADED);
                }
            }
        }

        void Pagination1_ValueChanged(object sender, AntdUI.PagePageEventArgs e)
        {
            tableCircularAreaHistory.DataSource = GetPageData(e.Current, e.PageSize);
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
            tableCircularAreaHistory.VisibleHeader = e.Value;
        }
        void CheckFixedHeader_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            tableCircularAreaHistory.FixedHeader = e.Value;
        }
        void CheckBordered_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            tableCircularAreaHistory.Bordered = e.Value;
        }
        #region 行状态
        void CheckSetRowStyle_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            if (e.Value) tableCircularAreaHistory.SetRowStyle += Table1_SetRowStyle;
            else tableCircularAreaHistory.SetRowStyle -= Table1_SetRowStyle;
            tableCircularAreaHistory.Invalidate();
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
            if (tableCircularAreaHistory.Columns != null) tableCircularAreaHistory.Columns[2].SortOrder = tableCircularAreaHistory.Columns[3].SortOrder = e.Value;
        }
        void CheckEnableHeaderResizing_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            tableCircularAreaHistory.EnableHeaderResizing = e.Value;
        }
        void CheckColumnDragSort_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            tableCircularAreaHistory.ColumnDragSort = e.Value;
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