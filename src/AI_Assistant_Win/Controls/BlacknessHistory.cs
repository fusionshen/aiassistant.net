using AI_Assistant_Win.Business;
using AI_Assistant_Win.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AI_Assistant_Win.Controls
{
    public partial class BlacknessHistory : UserControl
    {
        private readonly Form form;

        private readonly BlacknessMethodBLL blacknessMethodBLL;

        public BlacknessHistory(Form _form)
        {
            form = _form;
            blacknessMethodBLL = new BlacknessMethodBLL();
            InitializeComponent();
            table_Blackness_History.EditMode = AntdUI.TEditMode.DoubleClick;
            pagination1.PageSizeOptions = [10, 20, 30, 50, 100];

            #region 表头
            table_Blackness_History.Columns = new AntdUI.ColumnCollection {
                new AntdUI.ColumnCheck("check"){ Fixed = true },
                new AntdUI.Column("id","编号", AntdUI.ColumnAlign.Center){ Fixed = true },
                new AntdUI.Column("coilNumber","钢卷号", AntdUI.ColumnAlign.Center){ Fixed = true },
                new AntdUI.Column("size","尺寸", AntdUI.ColumnAlign.Center){ Fixed = true },
                new AntdUI.Column("isOK","OK/NG", AntdUI.ColumnAlign.Center){ Fixed = true },
                new AntdUI.ColumnSwitch("isUploaded","已上传",AntdUI.ColumnAlign.Center){ Fixed = true, Call=(value, record, i_row, i_col)=>{
                    System.Threading.Thread.Sleep(2000);
                    return value;
                } },
                new AntdUI.Column("tags","等级", AntdUI.ColumnAlign.Center),
                new AntdUI.Column("analyst","分析人",AntdUI.ColumnAlign.Center),
                new AntdUI.Column("workGroup","班组",AntdUI.ColumnAlign.Center),
                new AntdUI.Column("createTime","创建时间",AntdUI.ColumnAlign.Center),
                new AntdUI.Column("uploader","上传人",AntdUI.ColumnAlign.Center),
                new AntdUI.Column("uploadTime","上传时间",AntdUI.ColumnAlign.Center),
                new AntdUI.Column("lastReviser","最后修改人",AntdUI.ColumnAlign.Center),
                new AntdUI.Column("lastModifiedTime","最后修改时间",AntdUI.ColumnAlign.Center),
                // new AntdUI.Column("address","地址"){ Width="120", LineBreak=true},
                new AntdUI.Column("btns","操作"){ Fixed=true, Width="auto"},
            };
            #endregion
            #region 数据
            var pagedList = GetPageData(pagination1.Current, pagination1.PageSize);
            table_Blackness_History.DataSource = pagedList;
            #endregion

        }

        #region 事件

        void CheckFixedHeader_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            table_Blackness_History.FixedHeader = e.Value;
        }

        void CheckColumnDragSort_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            table_Blackness_History.ColumnDragSort = e.Value;
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
            if (table_Blackness_History.Columns != null) table_Blackness_History.Columns[6].SortOrder = table_Blackness_History.Columns[7].SortOrder = e.Value;
        }

        void CheckEnableHeaderResizing_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            table_Blackness_History.EnableHeaderResizing = e.Value;
        }

        void CheckVisibleHeader_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            table_Blackness_History.VisibleHeader = e.Value;
        }

        #endregion

        #region 单元格事件

        void Table1_CellClick(object sender, AntdUI.TableClickEventArgs e)
        {
            if (e.Record is IList<AntdUI.AntItem> data)
            {
                if (e.RowIndex > 0 && e.ColumnIndex == 6) AntdUI.Popover.open(new AntdUI.Popover.Config(table_Blackness_History, "这只是个测试") { Offset = e.Rect });
                else if (e.RowIndex > 0 && e.ColumnIndex == 8)
                {
                    var tag = data[10];
                    if (tag.value is AntdUI.CellTag[] tags)
                    {
                        if (tags.Length == 1)
                        {
                            if (tags[0].Text == "ERROR") tag.value = new AntdUI.CellTag[] { new AntdUI.CellTag("NICE", AntdUI.TTypeMini.Success), new AntdUI.CellTag("DEVELOPER", AntdUI.TTypeMini.Info) };
                            else
                            {
                                tags[0].Type = AntdUI.TTypeMini.Error;
                                tags[0].Text = "ERROR";
                            }
                        }
                        else tag.value = new AntdUI.CellTag[] { new AntdUI.CellTag("DEVELOPER", AntdUI.TTypeMini.Info) };
                    }
                    else tag.value = new AntdUI.CellTag[] { new AntdUI.CellTag("NICE", AntdUI.TTypeMini.Success), new AntdUI.CellTag("DEVELOPER", AntdUI.TTypeMini.Info) };
                }
            }
        }

        void Table1_CellButtonClick(object sender, AntdUI.TableButtonEventArgs e)
        {
            if (e.Record is IList<AntdUI.AntItem> data)
            {
                if (AntdUI.Modal.open(new AntdUI.Modal.Config(form, "提示", new AntdUI.Modal.TextLine[] {
                    new AntdUI.Modal.TextLine(data[3].value.ToString(),AntdUI.Style.Db.Primary),
                    new AntdUI.Modal.TextLine(data[9].value.ToString(),6,AntdUI.Style.Db.TextSecondary)
                }, AntdUI.TType.Error)
                {
                    CancelText = null,
                    OkType = AntdUI.TTypeMini.Error,
                    OkText = "成功"
                }) == DialogResult.OK)
                {
                    table_Blackness_History.Spin("等待中...", () =>
                    {
                        System.Threading.Thread.Sleep(2000);
                    }, () =>
                    {
                        System.Diagnostics.Debug.WriteLine("操作成功");
                    });
                }
            }
        }

        #endregion

        #region 获取页面数据

        List<AntdUI.AntItem[]> GetPageData(int current, int pageSize)
        {
            var dbList = blacknessMethodBLL.GetResultListFromDB();
            // lower memory coss
            dbList = dbList.Skip(pageSize * Math.Abs(current - 1)).Take(pageSize).ToList();
            pagination1.Total = dbList.Count;
            // format table items
            var dataList = dbList.Select(t =>
            {
                // single row
                var columnsInOneRow = new List<AntdUI.AntItem>();
                columnsInOneRow.Add(new AntdUI.AntItem("check", false));
                columnsInOneRow.Add(new AntdUI.AntItem("id", t.Id));
                columnsInOneRow.Add(new AntdUI.AntItem("coilNumber", t.CoilNumber));
                columnsInOneRow.Add(new AntdUI.AntItem("size", t.Size));
                columnsInOneRow.Add(new AntdUI.AntItem("isOK", t.IsOK ? new AntdUI.CellBadge(AntdUI.TState.Success, "OK") : new AntdUI.CellBadge(AntdUI.TState.Error, "NG")));
                columnsInOneRow.Add(new AntdUI.AntItem("isUploaded", t.IsUploaded));
                columnsInOneRow.Add(new AntdUI.AntItem("tags", FormatCellTagList(t).ToArray()));
                columnsInOneRow.Add(new AntdUI.AntItem("analyst", t.Analyst));
                columnsInOneRow.Add(new AntdUI.AntItem("workGroup", t.WorkGroup));
                columnsInOneRow.Add(new AntdUI.AntItem("createTime", t.CreateTime));
                columnsInOneRow.Add(new AntdUI.AntItem("uploader", t.Uploader));
                columnsInOneRow.Add(new AntdUI.AntItem("uploadTime", t.UploadTime));
                columnsInOneRow.Add(new AntdUI.AntItem("lastReviser", t.LastReviser));
                columnsInOneRow.Add(new AntdUI.AntItem("lastModifiedTime", t.LastModifiedTime));
                AntdUI.CellLink[] btns = new AntdUI.CellLink[] {
                    new AntdUI.CellButton("b1") { BorderWidth = 1, IconSvg = "SearchOutlined", IconHoverSvg = Properties.Resources.icon_like, ShowArrow = true, Tooltip = "预览"},
                    new AntdUI.CellButton("b2") { ShowArrow = true },
                    new AntdUI.CellButton("b3") { Type = AntdUI.TTypeMini.Primary, IconSvg = "SearchOutlined" },
                    new AntdUI.CellButton("edit", "Edit", AntdUI.TTypeMini.Primary) { Ghost = true, BorderWidth = 1 },
                    new AntdUI.CellButton("delete", "Delete", AntdUI.TTypeMini.Error) { Ghost = true, BorderWidth = 1 }
                };
                columnsInOneRow.Add(new AntdUI.AntItem("btns", btns));
                return columnsInOneRow.ToArray();
            }).ToList();
            return dataList;
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
    }
}