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

        public BlacknessHistory(Form _form)
        {
            form = _form;
            blacknessMethodBLL = new BlacknessMethodBLL();
            InitializeComponent();
            table_Blackness_History.EditMode = AntdUI.TEditMode.DoubleClick;
            pagination1.PageSizeOptions = [10, 20, 30, 50, 100];

            #region table header
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
                        AntdUI.Drawer.open(form, new BlacknessReport(form, id)
                        {
                            Size = new Size(420, 596)  // 常用到的纸张规格为A4，即21cm×29.7cm（210mm×297mm）
                        }, AntdUI.TAlignMini.Right);
                        break;
                    default:
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
                        break;
                }

            }
        }

        #endregion

        #region 获取页面数据

        List<AntdUI.AntItem[]> GetPageData(int current, int pageSize)
        {
            var dbList = blacknessMethodBLL.GetResultListFromDB();
            // lower memory coss
            var pagedList = dbList.Skip(pageSize * Math.Abs(current - 1)).Take(pageSize).ToList();
            pagination1.Total = pagedList.Count;
            // format table items
            var dataList = pagedList.Select(t =>
            {
                // single row
                var columnsInOneRow = new List<AntdUI.AntItem>
                {
                    new AntdUI.AntItem("check", false),
                    new AntdUI.AntItem("id", t.Id),
                    new AntdUI.AntItem("coilNumber", t.CoilNumber),
                    new AntdUI.AntItem("size", t.Size),
                    new AntdUI.AntItem("isOK", t.IsOK ? new AntdUI.CellBadge(AntdUI.TState.Success, "OK") :
                    new AntdUI.CellBadge(AntdUI.TState.Error, "NG")),
                    new AntdUI.AntItem("isUploaded", t.IsUploaded),
                    new AntdUI.AntItem("levels", FormatCellTagList(t).ToArray()),
                    new AntdUI.AntItem("analyst", t.Analyst),
                    new AntdUI.AntItem("workGroup", t.WorkGroup),
                    new AntdUI.AntItem("createTime", t.CreateTime),
                    new AntdUI.AntItem("uploader", t.Uploader),
                    new AntdUI.AntItem("uploadTime", t.UploadTime),
                    new AntdUI.AntItem("lastReviser", t.LastReviser),
                    new AntdUI.AntItem("lastModifiedTime", t.LastModifiedTime),
                    // for preview
                    new AntdUI.AntItem("originImage", t.OriginImagePath),
                    new AntdUI.AntItem("renderImage", t.RenderImagePath),
                    // for levels cell click
                    new AntdUI.AntItem("levelDetail", FormatLevelDetail(t)),

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
    }
}