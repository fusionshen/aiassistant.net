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

        private List<AntdUI.AntItem[]> dataList;
        public BlacknessHistory(Form _form)
        {
            form = _form;
            InitializeComponent();
            table1.EditMode = AntdUI.TEditMode.DoubleClick;

            pagination1.PageSizeOptions = new int[] { 10, 20, 30, 50, 100 };
            #region Table 1
            table1.Columns = new AntdUI.ColumnCollection {
                new AntdUI.ColumnCheck("check"){ Fixed=true},
                new AntdUI.Column("name","名称"){ Fixed=true},
                new AntdUI.ColumnCheck("checkTitle","选择"){ColAlign=AntdUI.ColumnAlign.Center},
                new AntdUI.ColumnRadio("radio","单选"),
                new AntdUI.Column("online","在线",AntdUI.ColumnAlign.Center),
                new AntdUI.ColumnSwitch("enable","是否",AntdUI.ColumnAlign.Center){ Call=(value,record, i_row, i_col)=>{
                    System.Threading.Thread.Sleep(2000);
                    return value;
                } },
                new AntdUI.Column("age","年龄",AntdUI.ColumnAlign.Center),
                new AntdUI.Column("address","地址"){ Width="120", LineBreak=true},
                new AntdUI.Column("tag","Tags"),
                new AntdUI.Column("imgs","头像"),
                new AntdUI.Column("btns","操作"){ Fixed=true,Width="auto"},
            };

            // 
            dataList = new List<AntdUI.AntItem[]>(25) {
                new AntdUI.AntItem[]{
                    new AntdUI.AntItem("no",1),
                    new AntdUI.AntItem("key",1),
                    new AntdUI.AntItem("check",false),
                    new AntdUI.AntItem("name","张三"),
                    new AntdUI.AntItem("checkTitle",false),
                    new AntdUI.AntItem("radio",false),
                    new AntdUI.AntItem("online", new AntdUI.CellBadge(AntdUI.TState.Success, "在线")),
                    new AntdUI.AntItem("enable",false),
                    new AntdUI.AntItem("age",32),
                    new AntdUI.AntItem("address","飞翔广场"),
                    new AntdUI.AntItem("tag"),
                    new AntdUI.AntItem("imgs",new AntdUI.CellImage[] {
                        new AntdUI.CellImage(Properties.Resources.img1){ BorderWidth=4,BorderColor=Color.BlueViolet},
                        new AntdUI.CellImage(Properties.Resources.bg1)
                    }),
                    new AntdUI.AntItem("btns", new AntdUI.CellLink[] {
                        new AntdUI.CellLink("delete","Delete")
                    }),
                },

                new AntdUI.AntItem[]{
                    new AntdUI.AntItem("no",2),
                    new AntdUI.AntItem("key",2),
                    new AntdUI.AntItem("check",false),
                    new AntdUI.AntItem("name","李四"),
                    new AntdUI.AntItem("checkTitle",false),
                    new AntdUI.AntItem("radio",false),
                    new AntdUI.AntItem("online",new AntdUI.CellBadge(AntdUI.TState.Processing, "登陆中")),
                    new AntdUI.AntItem("enable",false),
                    new AntdUI.AntItem("age",22),
                    new AntdUI.AntItem("address","大运村"),
                    new AntdUI.AntItem("tag",new AntdUI.CellTag[]{ new AntdUI.CellTag("NICE", AntdUI.TTypeMini.Success), new AntdUI.CellTag("DEVELOPER", AntdUI.TTypeMini.Info) }),
                    new AntdUI.AntItem("imgs"),
                    new AntdUI.AntItem("btns", new AntdUI.CellLink[] {
                        new AntdUI.CellButton("b1") { BorderWidth=1, IconSvg="SearchOutlined",IconHoverSvg=Properties.Resources.icon_like,ShowArrow=true},
                        new AntdUI.CellButton("b2") {  ShowArrow=true},
                        new AntdUI.CellButton("b3") { Type= AntdUI.TTypeMini.Primary, IconSvg="SearchOutlined" }
                    }),
                }
            };
            // 
            for (int i = 0; i < 23; i++)
            {
                AntdUI.CellBadge online;
                AntdUI.CellLink[] btns;
                if (i == 0) online = new AntdUI.CellBadge(AntdUI.TState.Error, "离线");
                else if (i == 1) online = new AntdUI.CellBadge(AntdUI.TState.Warn, "警告");
                else online = new AntdUI.CellBadge(AntdUI.TState.Default, "默认");

                if (i == 0)
                {
                    btns = new AntdUI.CellLink[] {
                        new AntdUI.CellButton("b1") { BorderWidth=1, IconSvg="SearchOutlined",IconHoverSvg=Properties.Resources.icon_like,ShowArrow=true},
                        new AntdUI.CellButton("b2") {  ShowArrow=true},
                        new AntdUI.CellButton("b3") { Type= AntdUI.TTypeMini.Primary, IconSvg="SearchOutlined" }
                    };
                }
                else if (i == 1)
                {
                    btns = new AntdUI.CellLink[] {
                        new AntdUI.CellButton("b1","Border") {  BorderWidth=1},
                        new AntdUI.CellButton("b2","GhostBorder") {  Ghost = true,BorderWidth=1,ShowArrow=true,IsLink=true }
                    };
                }
                else if (i == 2)
                {
                    btns = new AntdUI.CellLink[] {
                        new AntdUI.CellButton("edit","Edit",AntdUI.TTypeMini.Primary) {  Ghost = true,BorderWidth=1 },
                        new AntdUI.CellButton("delete","Delete",AntdUI.TTypeMini.Error) {  Ghost = true ,BorderWidth=1}
                    };
                }
                else if (i == 3)
                {
                    btns = new AntdUI.CellLink[] {
                        new AntdUI.CellButton("edit","Edit",AntdUI.TTypeMini.Primary),
                        new AntdUI.CellButton("delete","Delete",AntdUI.TTypeMini.Error)
                    };
                }
                else
                {
                    btns = new AntdUI.CellLink[] {
                        new AntdUI.CellLink("delete","Delete")
                    };
                }
                dataList.Add(new AntdUI.AntItem[]{
                    new AntdUI.AntItem("no",2+i),
                    new AntdUI.AntItem("key",2+i),
                    new AntdUI.AntItem("check",false),
                    new AntdUI.AntItem("name","王五"),
                    new AntdUI.AntItem("checkTitle",false),
                    new AntdUI.AntItem("radio",false),
                    new AntdUI.AntItem("online",online),
                    new AntdUI.AntItem("enable",i % 2 == 0),
                    new AntdUI.AntItem("age",33 +i),
                    new AntdUI.AntItem("address", "洪洞村" + (i + 2) + "栋"),
                    new AntdUI.AntItem("tag",null),
                    new AntdUI.AntItem("imgs"),
                    new AntdUI.AntItem("btns", btns),
                });
            }
            //table1.DataSource = dataList;
            pagination1.Total = dataList.Count;
            var pagedList = GetPageData(pagination1.Current, pagination1.PageSize);
            table1.DataSource = pagedList;
            #endregion
        }

        #region 事件

        void CheckFixedHeader_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            table1.FixedHeader = e.Value;
        }

        void CheckColumnDragSort_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            table1.ColumnDragSort = e.Value;
        }

        void CheckBordered_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            table1.Bordered = e.Value;
        }

        #region 行状态

        void CheckSetRowStyle_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            if (e.Value) table1.SetRowStyle += Table1_SetRowStyle;
            else table1.SetRowStyle -= Table1_SetRowStyle;
            table1.Invalidate();
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
            if (table1.Columns != null) table1.Columns[6].SortOrder = table1.Columns[7].SortOrder = e.Value;
        }

        void CheckEnableHeaderResizing_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            table1.EnableHeaderResizing = e.Value;
        }

        void CheckVisibleHeader_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            table1.VisibleHeader = e.Value;
        }

        #endregion

        #region 单元格事件

        void Table1_CellClick(object sender, AntdUI.TableClickEventArgs e)
        {
            if (e.Record is IList<AntdUI.AntItem> data)
            {
                if (e.RowIndex > 0 && e.ColumnIndex == 6) AntdUI.Popover.open(new AntdUI.Popover.Config(table1, "这只是个测试") { Offset = e.Rect });
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
                    table1.Spin("等待中...", () =>
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

        object GetPageData(int current, int pageSize)
        {
            var pagedList = dataList.Skip(pageSize * Math.Abs(current - 1)).Take(pageSize).ToList();
            return pagedList;
        }

        void Pagination1_ValueChanged(object sender, AntdUI.PagePageEventArgs e)
        {
            table1.DataSource = GetPageData(e.Current, e.PageSize);
        }

        private string Pagination1_ShowTotalChanged(object sender, AntdUI.PagePageEventArgs e)
        {
            return $"{e.PageSize} / {e.Total} 共{e.PageTotal}页";
        }

        #endregion
    }
}