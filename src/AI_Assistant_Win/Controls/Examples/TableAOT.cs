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

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AI_Assistant_Win.Controls
{
    public partial class TableAOT : UserControl
    {
        Form form;
        public TableAOT(Form _form)
        {
            form = _form;
            InitializeComponent();
            tabPage1.Text = AntdUI.Localization.Get("Table.Tab1", "����");
            tabPage2.Text = AntdUI.Localization.Get("Table.Tab2", "��ҳ");
            table1.EditMode = table2.EditMode = AntdUI.TEditMode.DoubleClick;

            #region Table 1

            table1.Columns = new AntdUI.ColumnCollection {
                new AntdUI.ColumnCheck("check"){ Fixed=true},
                new AntdUI.Column("name","����"){ Fixed=true},
                new AntdUI.ColumnCheck("checkTitle","��ȫѡ����"){ColAlign=AntdUI.ColumnAlign.Center},
                new AntdUI.ColumnRadio("radio","��ѡ"),
                new AntdUI.Column("online","״̬",AntdUI.ColumnAlign.Center),
                new AntdUI.ColumnSwitch("enable","����",AntdUI.ColumnAlign.Center){ Call=(value,record, i_row, i_col)=>{
                    System.Threading.Thread.Sleep(2000);
                    return value;
                } },
                new AntdUI.Column("age","����",AntdUI.ColumnAlign.Center),
                new AntdUI.Column("address","סַ"){ Width="120", LineBreak=true},
                new AntdUI.Column("tag","Tag"),
                new AntdUI.Column("imgs","ͼƬ"),
                new AntdUI.Column("btns","����"){ Fixed=true,Width="auto"},
            };

            // ���ӱ�ͷ����ģ������
            var list = new List<AntdUI.AntItem[]>(10) {
                new AntdUI.AntItem[]{
                    new AntdUI.AntItem("no",1),
                    new AntdUI.AntItem("key",1),
                    new AntdUI.AntItem("check",false),
                    new AntdUI.AntItem("name","�����"),
                    new AntdUI.AntItem("checkTitle",false),
                    new AntdUI.AntItem("radio",false),
                    new AntdUI.AntItem("online", new AntdUI.CellBadge(AntdUI.TState.Success, "����")),
                    new AntdUI.AntItem("enable",false),
                    new AntdUI.AntItem("age",32),
                    new AntdUI.AntItem("address","���������׹�԰1��"),
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
                    new AntdUI.AntItem("name","������"),
                    new AntdUI.AntItem("checkTitle",false),
                    new AntdUI.AntItem("radio",false),
                    new AntdUI.AntItem("online",new AntdUI.CellBadge(AntdUI.TState.Processing, "����")),
                    new AntdUI.AntItem("enable",false),
                    new AntdUI.AntItem("age",22),
                    new AntdUI.AntItem("address","���������׹�԰22��"),
                    new AntdUI.AntItem("tag",new AntdUI.CellTag[]{ new AntdUI.CellTag("NICE", AntdUI.TTypeMini.Success), new AntdUI.CellTag("DEVELOPER", AntdUI.TTypeMini.Info) }),
                    new AntdUI.AntItem("imgs"),
                    new AntdUI.AntItem("btns", new AntdUI.CellLink[] {
                        new AntdUI.CellButton("b1") { BorderWidth=1, IconSvg="SearchOutlined",IconHoverSvg=Properties.Resources.icon_like,ShowArrow=true},
                        new AntdUI.CellButton("b2") {  ShowArrow=true},
                        new AntdUI.CellButton("b3") { Type= AntdUI.TTypeMini.Primary, IconSvg="SearchOutlined" }
                    }),
                }
            };
            // ��������
            for (int i = 0; i < 8; i++)
            {
                AntdUI.CellBadge online;
                AntdUI.CellLink[] btns;
                if (i == 0) online = new AntdUI.CellBadge(AntdUI.TState.Error, "����");
                else if (i == 1) online = new AntdUI.CellBadge(AntdUI.TState.Warn, "����");
                else online = new AntdUI.CellBadge(AntdUI.TState.Default, "����");

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
                list.Add(new AntdUI.AntItem[]{
                    new AntdUI.AntItem("no",2+i),
                    new AntdUI.AntItem("key",2+i),
                    new AntdUI.AntItem("check",false),
                    new AntdUI.AntItem("name","�����"),
                    new AntdUI.AntItem("checkTitle",false),
                    new AntdUI.AntItem("radio",false),
                    new AntdUI.AntItem("online",online),
                    new AntdUI.AntItem("enable",i % 2 == 0),
                    new AntdUI.AntItem("age",33 +i),
                    new AntdUI.AntItem("address", "���������׹�԰" + (i + 2) + "��"),
                    new AntdUI.AntItem("tag",null),
                    new AntdUI.AntItem("imgs"),
                    new AntdUI.AntItem("btns", btns),
                });
            }

            table1.DataSource = list;

            #endregion

            #region Table 2

            table2.Columns = new AntdUI.ColumnCollection {
                new AntdUI.Column("no","���",AntdUI.ColumnAlign.Right){ Width="auto"},
                new AntdUI.Column("name","����"),
                new AntdUI.Column("age","����",AntdUI.ColumnAlign.Center),
                new AntdUI.Column("address","סַ"),
                new AntdUI.Column("tag","Tag"){ Width="auto"}
            };

            table2.DataSource = GetPageData(pagination1.Current, pagination1.PageSize);
            pagination1.PageSizeOptions = new int[] { 10, 20, 30, 50, 100 };

            #endregion
        }

        #region ʾ��

        void checkFixedHeader_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            table1.FixedHeader = e.Value;
        }

        void checkColumnDragSort_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            table1.ColumnDragSort = e.Value;
        }

        void checkBordered_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            table1.Bordered = e.Value;
        }

        #region ��ż��

        void checkSetRowStyle_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            if (e.Value) table1.SetRowStyle += table1_SetRowStyle;
            else table1.SetRowStyle -= table1_SetRowStyle;
            table1.Invalidate();
        }
        AntdUI.Table.CellStyleInfo table1_SetRowStyle(object sender, AntdUI.TableSetRowStyleEventArgs e)
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

        void checkSortOrder_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            if (table1.Columns != null) table1.Columns[6].SortOrder = table1.Columns[7].SortOrder = e.Value;
        }

        void checkEnableHeaderResizing_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            table1.EnableHeaderResizing = e.Value;
        }

        void checkVisibleHeader_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            table1.VisibleHeader = e.Value;
        }

        #endregion

        #region ���/˫��

        void table1_CellClick(object sender, AntdUI.TableClickEventArgs e)
        {
            if (e.Record is IList<AntdUI.AntItem> data)
            {
                if (e.RowIndex > 0 && e.ColumnIndex == 6) AntdUI.Popover.open(new AntdUI.Popover.Config(table1, "��ʾһ���ܵ����Զ���") { Offset = e.Rect });
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

        void table1_CellButtonClick(object sender, AntdUI.TableButtonEventArgs e)
        {
            if (e.Record is IList<AntdUI.AntItem> data)
            {
                if (AntdUI.Modal.open(new AntdUI.Modal.Config(form, "�Ƿ�ɾ��", new AntdUI.Modal.TextLine[] {
                    new AntdUI.Modal.TextLine(data[3].value.ToString(),AntdUI.Style.Db.Primary),
                    new AntdUI.Modal.TextLine(data[9].value.ToString(),6,AntdUI.Style.Db.TextSecondary)
                }, AntdUI.TType.Error)
                {
                    CancelText = null,
                    OkType = AntdUI.TTypeMini.Error,
                    OkText = "ɾ��"
                }) == DialogResult.OK)
                {
                    table1.Spin("���ڼ�����...", () =>
                    {
                        System.Threading.Thread.Sleep(2000);
                    }, () =>
                    {
                        System.Diagnostics.Debug.WriteLine("���ؽ���");
                    });
                }
            }
        }

        #endregion

        #region ��ҳ������

        object GetPageData(int current, int pageSize)
        {
            var list = new List<AntdUI.AntItem[]>();
            int start = Math.Abs(current - 1) * pageSize;
            for (int i = 0; i < pageSize; i++)
            {
                int index = start + i;
                list.Add(new AntdUI.AntItem[]{
                    new AntdUI.AntItem("no",index+1),
                    new AntdUI.AntItem("name","������" + index),
                    new AntdUI.AntItem("age", (index + 20)),
                    new AntdUI.AntItem("address", "���������׹�԰" + (index + 1) + "��"),
                    new AntdUI.AntItem("tag",index % 2 == 0 ? new AntdUI.CellTag("YES" + index, AntdUI.TTypeMini.Success) : new AntdUI.CellTag("NO" +index, AntdUI.TTypeMini.Error))
                });
            }
            return list;
        }

        void pagination1_ValueChanged(object sender, AntdUI.PagePageEventArgs e)
        {
            table2.DataSource = GetPageData(e.Current, e.PageSize);
        }
        string pagination1_ShowTotalChanged(object sender, AntdUI.PagePageEventArgs e)
        {
            return $"{e.PageSize} / {e.Total}�� {e.PageTotal}ҳ";
        }

        #endregion
    }
}