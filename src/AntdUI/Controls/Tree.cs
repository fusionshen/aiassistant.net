﻿// COPYRIGHT (C) Tom. ALL RIGHTS RESERVED.
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
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// Tree 树形控件
    /// </summary>
    /// <remarks>多层次的结构列表。</remarks>
    [Description("Tree 树形控件")]
    [ToolboxItem(true)]
    [DefaultProperty("Items")]
    [DefaultEvent("SelectChanged")]
    public class Tree : IControl
    {
        #region 属性

        /// <summary>
        /// 悬停背景颜色
        /// </summary>
        [Description("悬停背景颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? BackHover { get; set; }

        /// <summary>
        /// 激活背景颜色
        /// </summary>
        [Description("激活背景颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? BackActive { get; set; }

        Color? fore;
        /// <summary>
        /// 文字颜色
        /// </summary>
        [Description("文字颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public new Color? ForeColor
        {
            get => fore;
            set
            {
                if (fore == value) fore = value;
                fore = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 激活字体颜色
        /// </summary>
        [Description("激活字体颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? ForeActive { get; set; }

        float iconratio = 1F;
        /// <summary>
        /// 图标比例
        /// </summary>
        [Description("图标比例"), Category("外观"), DefaultValue(1F)]
        public float IconRatio
        {
            get => iconratio;
            set
            {
                if (iconratio == value) return;
                iconratio = value;
                if (IsHandleCreated)
                {
                    ChangeList();
                    Invalidate();
                }
            }
        }

        int radius = 6;
        /// <summary>
        /// 圆角
        /// </summary>
        [Description("圆角"), Category("外观"), DefaultValue(6)]
        public int Radius
        {
            get => radius;
            set
            {
                if (radius == value) return;
                radius = value;
                Invalidate();
            }
        }

        int _gap = 8;
        /// <summary>
        /// 间距
        /// </summary>
        [Description("间距"), Category("外观"), DefaultValue(8)]
        public int Gap
        {
            get => _gap;
            set
            {
                if (_gap == value) return;
                _gap = value;
                ChangeList();
                Invalidate();
            }
        }

        bool round = false;
        /// <summary>
        /// 圆角样式
        /// </summary>
        [Description("圆角样式"), Category("外观"), DefaultValue(false)]
        public bool Round
        {
            get => round;
            set
            {
                if (round == value) return;
                round = value;
                Invalidate();
            }
        }

        bool checkable = false;
        /// <summary>
        /// 节点前添加 Checkbox 复选框
        /// </summary>
        [Description("节点前添加 Checkbox 复选框"), Category("外观"), DefaultValue(false)]
        public bool Checkable
        {
            get => checkable;
            set
            {
                if (checkable == value) return;
                checkable = value;
                ChangeList();
                Invalidate();
            }
        }

        /// <summary>
        /// Checkable 状态下节点选择完全受控（父子节点选中状态不再关联）
        /// </summary>
        [Description("Checkable 状态下节点选择完全受控（父子节点选中状态不再关联）"), Category("行为"), DefaultValue(true)]
        public bool CheckStrictly { get; set; } = true;

        bool blockNode = false;
        /// <summary>
        /// 节点占据一行
        /// </summary>
        [Description("节点占据一行"), Category("外观"), DefaultValue(false)]
        public bool BlockNode
        {
            get => blockNode;
            set
            {
                if (blockNode == value) return;
                blockNode = value;
                ChangeList();
                Invalidate();
            }
        }

        #region 集合操作

        /// <summary>
        /// 展开全部
        /// </summary>
        /// <param name="value">true 展开、false 收起</param>
        public void ExpandAll(bool value = true)
        {
            if (items != null && items.Count > 0) ExpandAll(items, value);
        }

        public void ExpandAll(TreeItemCollection items, bool value = true)
        {
            if (items != null && items.Count > 0)
            {
                foreach (var it in items)
                {
                    it.Expand = value;
                    ExpandAll(it.Sub, value);
                }
            }
        }

        #region 获取项

        /// <summary>
        /// 获取所有选中项
        /// </summary>
        /// <param name="Indeterminate">是否包含 Indeterminate</param>
        public List<TreeItem> GetCheckeds(bool Indeterminate = true)
        {
            if (items == null) return new List<TreeItem>(0);
            return GetCheckeds(items, Indeterminate);
        }
        List<TreeItem> GetCheckeds(TreeItemCollection items, bool Indeterminate)
        {
            var list = new List<TreeItem>();
            if (Indeterminate)
            {
                foreach (var it in items)
                {
                    if (it.CheckState != CheckState.Unchecked) list.Add(it);
                    if (it.items != null && it.items.Count > 0)
                    {
                        var list_sub = GetCheckeds(it.items, Indeterminate);
                        if (list_sub.Count > 0) list.AddRange(list_sub);
                    }
                }
            }
            else
            {
                foreach (var it in items)
                {
                    if (it.Checked) list.Add(it);
                    if (it.items != null && it.items.Count > 0)
                    {
                        var list_sub = GetCheckeds(it.items, Indeterminate);
                        if (list_sub.Count > 0) list.AddRange(list_sub);
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 全选/全不选
        /// </summary>
        public void SetCheckeds()
        {
            if (items == null) return;
            var list = GetCheckeds();
            SetCheckeds(list.Count == 0);
        }

        /// <summary>
        /// 全选/全不选
        /// </summary>
        public void SetCheckeds(bool check)
        {
            if (items == null) return;
            SetCheckeds(items, check);
        }
        public void SetCheckeds(TreeItemCollection items, bool check)
        {
            foreach (var it in items)
            {
                it.Checked = check;
                if (it.items != null && it.items.Count > 0) SetCheckeds(it.items, check);
            }
        }

        public void Focus(TreeItem item)
        {
            if (ScrollBar.ShowY) ScrollBar.ValueY = item.rect.Y - (int)(_gap * Config.Dpi);
            Invalidate();
        }

        #endregion

        #region 事件

        /// <summary>
        /// Select 属性值更改时发生
        /// </summary>
        [Description("Select 属性值更改时发生"), Category("行为")]
        public event TreeSelectEventHandler? SelectChanged = null;

        /// <summary>
        /// Checked 属性值更改时发生
        /// </summary>
        [Description("Checked 属性值更改时发生"), Category("行为")]
        public event TreeCheckedEventHandler? CheckedChanged = null;

        /// <summary>
        /// 点击项事件
        /// </summary>
        [Description("点击项事件"), Category("行为")]
        public event TreeSelectEventHandler? NodeMouseClick = null;

        /// <summary>
        /// 双击项事件
        /// </summary>
        [Description("双击项事件"), Category("行为")]
        public event TreeSelectEventHandler? NodeMouseDoubleClick = null;

        /// <summary>
        /// 移动项事件
        /// </summary>
        [Description("移动项事件"), Category("行为")]
        public event TreeHoverEventHandler? NodeMouseMove = null;
        internal void OnNodeMouseMove(TreeItem item, bool hover)
        {
            if (NodeMouseMove == null) return;
            int sx = ScrollBar.ValueX, sy = ScrollBar.ValueY;
            NodeMouseMove(this, new TreeHoverEventArgs(item, new Rectangle(item.txt_rect.X, item.txt_rect.Y - sy, item.txt_rect.Width, item.txt_rect.Height), hover));
        }

        internal void OnSelectChanged(TreeItem item, MouseEventArgs args)
        {
            if (SelectChanged == null) return;
            int sx = ScrollBar.ValueX, sy = ScrollBar.ValueY;
            SelectChanged(this, new TreeSelectEventArgs(item, new Rectangle(item.txt_rect.X, item.txt_rect.Y - sy, item.txt_rect.Width, item.txt_rect.Height), args));
        }
        internal void OnNodeMouseClick(TreeItem item, MouseEventArgs args)
        {
            if (NodeMouseClick == null) return;
            int sx = ScrollBar.ValueX, sy = ScrollBar.ValueY;
            NodeMouseClick(this, new TreeSelectEventArgs(item, new Rectangle(item.txt_rect.X, item.txt_rect.Y - sy, item.txt_rect.Width, item.txt_rect.Height), args));
        }
        internal void OnNodeMouseDoubleClick(TreeItem item, MouseEventArgs args)
        {
            if (NodeMouseDoubleClick == null) return;
            int sx = ScrollBar.ValueX, sy = ScrollBar.ValueY;
            NodeMouseDoubleClick(this, new TreeSelectEventArgs(item, new Rectangle(item.txt_rect.X, item.txt_rect.Y - sy, item.txt_rect.Width, item.txt_rect.Height), args));
        }
        internal void OnCheckedChanged(TreeItem item, bool value)
        {
            CheckedChanged?.Invoke(this, new TreeCheckedEventArgs(item, value));
        }

        #endregion

        #endregion

        TreeItemCollection? items;
        /// <summary>
        /// 集合
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("集合"), Category("数据")]
        public TreeItemCollection Items
        {
            get
            {
                items ??= new TreeItemCollection(this);
                return items;
            }
            set => items = value.BindData(this);
        }


        bool pauseLayout = false;
        [Browsable(false), Description("暂停布局"), Category("行为"), DefaultValue(false)]
        public bool PauseLayout
        {
            get => pauseLayout;
            set
            {
                if (pauseLayout == value) return;
                pauseLayout = value;
                if (!value)
                {
                    ChangeList();
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// 滚动条
        /// </summary>
        [Browsable(false)]
        public ScrollBar ScrollBar;

        #endregion

        #region 布局

        protected override void OnSizeChanged(EventArgs e)
        {
            var rect = ChangeList();
            ScrollBar.SizeChange(rect);
            base.OnSizeChanged(e);
        }

        internal Rectangle ChangeList()
        {
            var rect = ClientRectangle;
            if (pauseLayout || items == null || items.Count == 0 || (rect.Width == 0 || rect.Height == 0)) return rect;

            int x = 0, y = 0;
            bool has = HasSub(items);
            Helper.GDI(g =>
            {
                var size = g.MeasureString(Config.NullText, Font);
                int icon_size = (int)(size.Height * iconratio), gap = (int)(_gap * Config.Dpi), gapI = gap / 2, height = icon_size + gap * 2;

                check_radius = icon_size * .2F;
                if (CheckStrictly && has && items[0].PARENT == null && items[0].PARENTITEM == null)
                {
                    //新数据
                    var dir = new List<TreeItem>();
                    TestSub(ref dir, items);
                    foreach (var item in dir)
                    {
                        int check_count = 0;
                        foreach (var sub in item.Sub)
                        { if (sub.CheckState == CheckState.Checked || sub.CheckState == CheckState.Indeterminate) check_count++; }
                        if (check_count > 0) item.CheckState = check_count == item.Sub.Count ? CheckState.Checked : CheckState.Indeterminate;
                        else item.CheckState = CheckState.Unchecked;
                    }
                }
                ChangeList(g, rect, null, items, has, ref x, ref y, height, icon_size, gap, gapI, 0, true);
            });
            ScrollBar.SetVrSize(x, y);
            return rect;
        }

        bool HasSub(TreeItemCollection items)
        {
            foreach (var it in items)
            {
                if (it.CanExpand) return true;
            }
            return false;
        }
        void TestSub(ref List<TreeItem> dir, TreeItemCollection items)
        {
            foreach (var it in items)
            {
                if (it.CanExpand)
                {
                    dir.Insert(0, it);
                    TestSub(ref dir, it.Sub);
                }
            }
        }

        void ChangeList(Canvas g, Rectangle rect, TreeItem? Parent, TreeItemCollection items, bool has_sub, ref int x, ref int y, int height, int icon_size, int gap, int gapI, int depth, bool expand)
        {
            foreach (var it in items)
            {
                it.PARENT = this;
                it.PARENTITEM = Parent;
                it.SetRect(g, Font, depth, checkable, blockNode, has_sub, new Rectangle(0, y, rect.Width, height), icon_size, gap);
                if (expand && it.txt_rect.Right > x) x = it.txt_rect.Right;
                if (it.Show && it.Visible)
                {
                    y += height + gapI;
                    if (it.CanExpand)
                    {
                        int y_item = y;
                        ChangeList(g, rect, it, it.Sub, has_sub, ref x, ref y, height, icon_size, gap, gapI, depth + 1, expand ? it.Expand : false);
                        it.SubY = y_item - gapI / 2;
                        it.SubHeight = y - y_item;
                        if ((it.Expand || it.ExpandThread) && it.ExpandProg > 0)
                        {
                            it.ExpandHeight = y - y_item;
                            y = y_item + (int)(it.ExpandHeight * it.ExpandProg);
                        }
                        else if (!it.Expand) y = y_item;
                    }
                }
            }
        }

        #endregion

        #region 渲染

        float check_radius = 0F;
        public Tree()
        {
            ScrollBar = new ScrollBar(this, true, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (items == null || items.Count == 0) return;
            var rect = ClientRectangle;
            if (rect.Width == 0 || rect.Height == 0) return;
            var g = e.Graphics.High();
            int sx = ScrollBar.ValueX, sy = ScrollBar.ValueY;
            g.TranslateTransform(-sx, -sy);
            float _radius = radius * Config.Dpi;
            using (var brush_fore = new SolidBrush(fore ?? Style.Db.TextBase))
            using (var brush_fore_active = new SolidBrush(ForeActive ?? Style.Db.Primary))
            using (var brush_hover = new SolidBrush(BackHover ?? Style.Db.FillSecondary))
            using (var brush_active = new SolidBrush(BackActive ?? Style.Db.PrimaryBg))
            using (var brush_TextTertiary = new SolidBrush(Style.Db.TextTertiary))
            {
                PaintItem(g, rect, sx, sy, items, brush_fore, brush_fore_active, brush_hover, brush_active, brush_TextTertiary, _radius);
            }
            g.ResetTransform();
            ScrollBar.Paint(g);
            this.PaintBadge(g);
            base.OnPaint(e);
        }

        void PaintItem(Canvas g, Rectangle rect, int sx, int sy, TreeItemCollection items, SolidBrush fore, SolidBrush fore_active, SolidBrush hover, SolidBrush active, SolidBrush brushTextTertiary, float radius)
        {
            foreach (var it in items)
            {
                it.show = it.Show && it.Visible && it.rect.Y > sy - rect.Height - (it.Expand ? it.SubHeight : 0) && it.rect.Bottom < sy + rect.Height + it.rect.Height;
                if (it.show)
                {
                    PaintItem(g, it, fore, fore_active, hover, active, brushTextTertiary, radius, sx, sy);
                    if ((it.Expand || it.ExpandThread) && it.items != null && it.items.Count > 0)
                    {
                        var state = g.Save();
                        if (it.ExpandThread) g.SetClip(new RectangleF(rect.X, it.rect.Bottom, rect.Width, it.ExpandHeight * it.ExpandProg));
                        PaintItem(g, rect, sx, sy, it.items, fore, fore_active, hover, active, brushTextTertiary, radius);
                        g.Restore(state);
                    }
                }
            }
        }

        readonly StringFormat s_c = Helper.SF_Ellipsis(), s_l = Helper.SF_ALL(lr: StringAlignment.Near);
        void PaintItem(Canvas g, TreeItem item, SolidBrush fore, SolidBrush fore_active, SolidBrush hover, SolidBrush active, SolidBrush brushTextTertiary, float radius, int sx, int sy)
        {
            if (item.Select)
            {
                if (blockNode)
                {
                    g.ResetTransform();
                    g.TranslateTransform(0, -sy);
                    PaintBack(g, active, item.rect, radius);
                    g.TranslateTransform(-sx, 0);
                }
                else PaintBack(g, active, item.rect, radius);
                if (item.CanExpand) PaintArrow(g, item, fore_active, sx, sy);
                PaintItemText(g, item, fore_active, brushTextTertiary);
            }
            else
            {
                if (item.Back.HasValue)
                {
                    using (var brush = new SolidBrush(item.Back.Value))
                    {
                        PaintBack(g, brush, item.rect, radius);
                    }
                }
                if (blockNode)
                {
                    g.ResetTransform();
                    g.TranslateTransform(0, -sy);
                    if (item.AnimationHover)
                    {
                        using (var brush = new SolidBrush(Helper.ToColorN(item.AnimationHoverValue, hover.Color)))
                        {
                            PaintBack(g, brush, item.rect, radius);
                        }
                    }
                    else if (item.Hover) PaintBack(g, hover, item.rect, radius);
                    g.TranslateTransform(-sx, 0);
                }
                else
                {
                    if (item.AnimationHover)
                    {
                        using (var brush = new SolidBrush(Helper.ToColorN(item.AnimationHoverValue, hover.Color)))
                        {
                            PaintBack(g, brush, item.rect, radius);
                        }
                    }
                    else if (item.Hover) PaintBack(g, hover, item.rect, radius);
                }
                if (item.CanExpand) PaintArrow(g, item, fore, sx, sy);
                if (item.Enabled) PaintItemText(g, item, fore, brushTextTertiary);
                else
                {
                    using (var brush = new SolidBrush(Style.Db.TextQuaternary))
                    {
                        PaintItemText(g, item, brush, brushTextTertiary);
                    }
                }
            }
            if (checkable)
            {
                using (var path_check = Helper.RoundPath(item.check_rect, check_radius, false))
                {
                    var bor2 = 2F * Config.Dpi;
                    if (item.Enabled)
                    {
                        if (item.AnimationCheck)
                        {
                            var alpha = 255 * item.AnimationCheckValue;

                            if (item.CheckState == CheckState.Indeterminate || (item.checkStateOld == CheckState.Indeterminate && !item.Checked))
                            {
                                g.Draw(Style.Db.BorderColor, bor2, path_check);
                                g.Fill(Helper.ToColor(alpha, Style.Db.Primary), PaintBlock(item.check_rect));
                            }
                            else
                            {
                                float dot = item.check_rect.Width * 0.3F;

                                g.Fill(Helper.ToColor(alpha, Style.Db.Primary), path_check);
                                g.DrawLines(Helper.ToColor(alpha, Style.Db.BgBase), 3F * Config.Dpi, PaintArrow(item.check_rect));

                                if (item.Checked)
                                {
                                    float max = item.check_rect.Height + item.check_rect.Height * item.AnimationCheckValue, alpha2 = 100 * (1F - item.AnimationCheckValue);
                                    using (var brush = new SolidBrush(Helper.ToColor(alpha2, Style.Db.Primary)))
                                    {
                                        g.FillEllipse(brush, new RectangleF(item.check_rect.X + (item.check_rect.Width - max) / 2F, item.check_rect.Y + (item.check_rect.Height - max) / 2F, max, max));
                                    }
                                }
                                g.Draw(Style.Db.Primary, 2F * Config.Dpi, path_check);
                            }
                        }
                        else if (item.CheckState == CheckState.Indeterminate)
                        {
                            g.Draw(Style.Db.BorderColor, bor2, path_check);
                            g.Fill(Style.Db.Primary, PaintBlock(item.check_rect));
                        }
                        else if (item.Checked)
                        {
                            g.Fill(Style.Db.Primary, path_check);
                            g.DrawLines(Style.Db.BgBase, bor2, PaintArrow(item.check_rect));
                        }
                        else g.Draw(Style.Db.BorderColor, bor2, path_check);
                    }
                    else
                    {
                        g.Fill(Style.Db.FillQuaternary, path_check);
                        if (item.CheckState == CheckState.Indeterminate) g.Fill(Style.Db.TextQuaternary, PaintBlock(item.check_rect));
                        else if (item.Checked) g.DrawLines(Style.Db.TextQuaternary, bor2, PaintArrow(item.check_rect));
                        g.Draw(Style.Db.BorderColorDisable, bor2, path_check);
                    }
                }
            }
        }
        void PaintItemText(Canvas g, TreeItem item, SolidBrush fore, SolidBrush brushTextTertiary)
        {
            Color color = fore.Color;
            if (item.Fore.HasValue)
            {
                color = item.Fore.Value;
                using (var brush = new SolidBrush(color))
                {
                    g.String(item.Text, Font, brush, item.txt_rect, blockNode ? s_l : s_c);
                }
            }
            else g.String(item.Text, Font, fore, item.txt_rect, blockNode ? s_l : s_c);
            if (item.SubTitle != null) g.String(item.SubTitle, Font, brushTextTertiary, item.subtxt_rect, s_l);
            if (item.Icon != null) g.Image(item.Icon, item.ico_rect);
            if (item.IconSvg != null) g.GetImgExtend(item.IconSvg, item.ico_rect, color);
        }

        internal RectangleF PaintBlock(RectangleF rect)
        {
            float size = rect.Height * 0.2F, size2 = size * 2F;
            return new RectangleF(rect.X + size, rect.Y + size, rect.Width - size2, rect.Height - size2);
        }
        internal PointF[] PaintArrow(RectangleF rect)
        {
            float size = rect.Height * 0.15F, size2 = rect.Height * 0.2F, size3 = rect.Height * 0.26F;
            return new PointF[] {
                new PointF(rect.X+size,rect.Y+rect.Height/2),
                new PointF(rect.X+rect.Width*0.4F,rect.Y+(rect.Height-size3)),
                new PointF(rect.X+rect.Width-size2,rect.Y+size2),
            };
        }

        void PaintArrow(Canvas g, TreeItem item, SolidBrush color, int sx, int sy)
        {
            using (var pen = new Pen(color, 2F))
            {
                pen.StartCap = pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;

                if (item.ExpandThread) PaintArrow(g, item, pen, sx, sy, -90F + (90F * item.ExpandProg));
                else if (item.Expand) g.DrawLines(pen, item.arr_rect.TriangleLines(-1, .4F));
                else PaintArrow(g, item, pen, sx, sy, -90F);
            }
        }

        void PaintArrow(Canvas g, TreeItem item, Pen pen, int sx, int sy, float rotate)
        {
            int size_arrow = item.arr_rect.Width / 2;
            g.TranslateTransform(item.arr_rect.X + size_arrow, item.arr_rect.Y + size_arrow);
            g.RotateTransform(rotate);
            g.DrawLines(pen, new Rectangle(-size_arrow, -size_arrow, item.arr_rect.Width, item.arr_rect.Height).TriangleLines(-1, .4F));
            g.ResetTransform();
            g.TranslateTransform(-sx, -sy);
        }

        void PaintBack(Canvas g, SolidBrush brush, Rectangle rect, float radius)
        {
            if (round || radius > 0)
            {
                using (var path = rect.RoundPath(radius, round))
                {
                    g.Fill(brush, path);
                }
            }
            else g.Fill(brush, rect);
        }

        #endregion

        #region 鼠标

        TreeItem? MDown = null;
        bool doubleClick = false;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            doubleClick = e.Clicks > 1;
            MDown = null;
            if (ScrollBar.MouseDownY(e.Location) && ScrollBar.MouseDownX(e.Location))
            {
                if (items == null || items.Count == 0) return;
                OnTouchDown(e.X, e.Y);
                foreach (var it in items)
                {
                    if (IMouseDown(e, it)) return;
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (ScrollBar.MouseUpY() && ScrollBar.MouseUpX() && OnTouchUp())
            {
                if (items == null || items.Count == 0 || MDown == null) return;
                foreach (var it in items)
                {
                    if (IMouseUp(e, it, MDown)) return;
                }
            }
        }
        bool IMouseDown(MouseEventArgs e, TreeItem item)
        {
            int down = item.Contains(e.Location, blockNode ? 0 : ScrollBar.ValueX, ScrollBar.ValueY, checkable);
            if (down > 0)
            {
                MDown = item;
                return true;
            }
            if (item.CanExpand && item.Expand)
            {
                foreach (var sub in item.Sub)
                {
                    if (IMouseDown(e, sub)) return true;
                }
            }
            return false;
        }
        bool IMouseUp(MouseEventArgs e, TreeItem item, TreeItem MDown)
        {
            bool can = item.CanExpand;
            if (MDown == item)
            {
                int down = item.Contains(e.Location, blockNode ? 0 : ScrollBar.ValueX, ScrollBar.ValueY, checkable);
                if (down > 0)
                {
                    if (blockNode)
                    {
                        if (can) item.Expand = !item.Expand;
                        item.Select = true;
                        OnSelectChanged(item, e);
                        Invalidate();
                    }
                    else if (down == 3 && item.Enabled)
                    {
                        item.Checked = !item.Checked;
                        if (CheckStrictly)
                        {
                            SetCheck(item, item.Checked);
                            SetCheckStrictly(item.PARENTITEM);
                        }
                    }
                    else if (down == 2 && can) item.Expand = !item.Expand;
                    else
                    {
                        item.Select = true;
                        OnSelectChanged(item, e);
                        Invalidate();
                    }
                    if (doubleClick) OnNodeMouseDoubleClick(item, e);
                    OnNodeMouseClick(item, e);
                }
                return true;
            }
            if (can && item.Expand)
            {
                foreach (var sub in item.Sub)
                {
                    if (IMouseUp(e, sub, MDown)) return true;
                }
            }
            return false;
        }

        void SetCheck(TreeItem item, bool value)
        {
            if (item.items != null && item.items.Count > 0)
            {
                foreach (var it in item.items)
                {
                    it.Checked = value;
                    SetCheck(it, value);
                }
            }
        }

        void SetCheckStrictly(TreeItem? item)
        {
            if (item == null) return;
            int check_count = 0;
            foreach (var sub in item.Sub)
            {
                if (sub.CheckState == CheckState.Checked || sub.CheckState == CheckState.Indeterminate) check_count++;
            }
            if (check_count > 0) item.CheckState = check_count == item.Sub.Count ? CheckState.Checked : CheckState.Indeterminate;
            else item.CheckState = CheckState.Unchecked;
            SetCheckStrictly(item.PARENTITEM);

        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (ScrollBar.MouseMoveY(e.Location) && ScrollBar.MouseMoveX(e.Location))
            {
                if (OnTouchMove(e.X, e.Y))
                {
                    if (items == null || items.Count == 0) return;
                    int hand = 0;
                    foreach (var it in items) IMouseMove(it, e.Location, ref hand);
                    SetCursor(hand > 0);
                }
            }
            else ILeave();
        }

        void IMouseMove(TreeItem item, Point point, ref int hand)
        {
            if (item.show)
            {
                if (item.Contains(point, blockNode ? 0 : ScrollBar.ValueX, ScrollBar.ValueY, checkable) > 0) hand++;
                if (item.items != null) foreach (var sub in item.items) IMouseMove(sub, point, ref hand);
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            ScrollBar.Leave();
            ILeave();
        }
        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            ScrollBar.Leave();
            ILeave();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            ScrollBar.MouseWheel(e.Delta);
            base.OnMouseWheel(e);
        }
        protected override bool OnTouchScrollX(int value) => ScrollBar.MouseWheelX(value);
        protected override bool OnTouchScrollY(int value) => ScrollBar.MouseWheelY(value);

        void ILeave()
        {
            SetCursor(false);
            if (items == null || items.Count == 0) return;
            int count = 0;
            foreach (var it in items)
            {
                ILeave(it, ref count);
            }
            if (count > 0) Invalidate();
        }
        void ILeave(TreeItem item, ref int count)
        {
            if (item.Hover) count++;
            item.Hover = false;
            if (item.items != null)
                foreach (var sub in item.items) ILeave(sub, ref count);
        }

        internal void IUSelect()
        {
            if (items == null || items.Count == 0) return;
            foreach (var it in items) IUSelect(it);
        }
        void IUSelect(TreeItem item)
        {
            item.Select = false;
            if (item.items != null) foreach (var sub in item.items) IUSelect(sub);
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            ScrollBar.Dispose();
            base.Dispose(disposing);
        }
    }

    public class TreeItemCollection : iCollection<TreeItem>
    {
        public TreeItemCollection(Tree it)
        {
            BindData(it);
        }
        public TreeItemCollection(TreeItem it)
        {
            BindData(it);
        }

        internal TreeItemCollection BindData(Tree it)
        {
            action = render =>
            {
                if (render) it.ChangeList();
                it.Invalidate();
            };
            return this;
        }

        internal TreeItemCollection BindData(TreeItem it)
        {
            action = render =>
            {
                if (it.PARENT == null) return;
                if (render) it.PARENT.ChangeList();
                it.PARENT.Invalidate();
            };
            return this;
        }
    }

    public class TreeItem : NotifyProperty
    {
        public TreeItem() { }
        public TreeItem(string text)
        {
            Text = text;
        }
        public TreeItem(string text, Image? icon)
        {
            Text = text;
            Icon = icon;
        }

        /// <summary>
        /// ID
        /// </summary>
        [Description("ID"), Category("数据"), DefaultValue(null)]
        public string? ID { get; set; }

        Image? icon = null;
        /// <summary>
        /// 图标
        /// </summary>
        [Description("图标"), Category("外观"), DefaultValue(null)]
        public Image? Icon
        {
            get => icon;
            set
            {
                if (icon == value) return;
                icon = value;
                OnPropertyChanged("Icon");
            }
        }

        string? iconSvg = null;
        /// <summary>
        /// 图标
        /// </summary>
        [Description("图标SVG"), Category("外观"), DefaultValue(null)]
        public string? IconSvg
        {
            get => iconSvg;
            set
            {
                if (iconSvg == value) return;
                iconSvg = value;
                OnPropertyChanged("IconSvg");
            }
        }

        /// <summary>
        /// 是否包含图片
        /// </summary>
        internal bool HasIcon
        {
            get => iconSvg != null || Icon != null;
        }

        /// <summary>
        /// 名称
        /// </summary>
        [Description("名称"), Category("数据"), DefaultValue(null)]
        public string? Name { get; set; }

        string? text = null;
        /// <summary>
        /// 文本
        /// </summary>
        [Description("文本"), Category("外观"), DefaultValue(null)]
        public string? Text
        {
            get => text;
            set
            {
                if (text == value) return;
                text = value;
                Invalidates();
            }
        }

        string? subTitle = null;
        /// <summary>
        /// 子标题
        /// </summary>
        [Description("子标题"), Category("外观"), DefaultValue(null)]
        public string? SubTitle
        {
            get => subTitle;
            set
            {
                if (string.IsNullOrEmpty(value)) value = null;
                if (subTitle == value) return;
                subTitle = value;
                Invalidates();
            }
        }

        bool visible = true;
        /// <summary>
        /// 是否显示
        /// </summary>
        [Description("是否显示"), Category("外观"), DefaultValue(true)]
        public bool Visible
        {
            get => visible;
            set
            {
                if (visible == value) return;
                visible = value;
                Invalidates();
            }
        }

        /// <summary>
        /// 用户定义数据
        /// </summary>
        [Description("用户定义数据"), Category("数据"), DefaultValue(null)]
        public object? Tag { get; set; }

        internal TreeItemCollection? items;
        /// <summary>
        /// 获取列表中所有列表项的集合
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("子集合"), Category("外观")]
        public TreeItemCollection Sub
        {
            get
            {
                items ??= new TreeItemCollection(this);
                return items;
            }
            set => items = value.BindData(this);
        }

        #region 禁用

        bool enabled = true;
        /// <summary>
        /// 禁掉响应
        /// </summary>
        [Description("禁掉响应"), Category("行为"), DefaultValue(true)]
        public bool Enabled
        {
            get => enabled;
            set
            {
                if (enabled == value) return;
                enabled = value;
                Invalidate();
            }
        }

        #endregion

        #region 展开

        ITask? ThreadExpand = null;
        bool expand = false;
        /// <summary>
        /// 展开
        /// </summary>
        [Description("展开"), Category("行为"), DefaultValue(false)]
        public bool Expand
        {
            get => expand;
            set
            {
                if (expand == value) return;
                expand = value;
                if (items != null && items.Count > 0)
                {
                    if (PARENT != null && PARENT.IsHandleCreated && Config.Animation)
                    {
                        ThreadExpand?.Dispose();
                        float oldval = -1;
                        if (ThreadExpand?.Tag is float oldv) oldval = oldv;
                        ExpandThread = true;
                        if (value)
                        {
                            int time = ExpandCount(this) * 10;
                            if (time < 100) time = 100;
                            else if (time > 1000) time = 1000;
                            int t = Animation.TotalFrames(10, time);
                            ThreadExpand = new ITask(false, 10, t, oldval, AnimationType.Ball, (i, val) =>
                            {
                                ExpandProg = val;
                                Invalidates();
                            }, () =>
                            {
                                ExpandProg = 1F;
                                ExpandThread = false;
                                Invalidates();
                            });
                        }
                        else
                        {
                            var t = Animation.TotalFrames(10, 200);
                            ThreadExpand = new ITask(true, 10, t, oldval, AnimationType.Ball, (i, val) =>
                            {
                                ExpandProg = val;
                                Invalidates();
                            }, () =>
                            {
                                ExpandProg = 1F;
                                ExpandThread = false;
                                Invalidates();
                            });
                        }
                    }
                    else
                    {
                        ExpandProg = 1F;
                        Invalidates();
                    }
                }
                else
                {
                    ExpandProg = 1F;
                    Invalidates();
                }
            }
        }

        internal int ExpandCount(TreeItem it)
        {
            int count = 0;
            if (it.items != null && it.items.Count > 0)
            {
                count += it.items.Count;
                foreach (var item in it.items)
                {
                    if (item.Expand) count += ExpandCount(item);
                }
            }
            return count;
        }

        [Description("是否可以展开"), Category("行为"), DefaultValue(false)]
        public bool CanExpand
        {
            get => visible && items != null && items.Count > 0;
        }

        #endregion

        #region 选中状态

        internal bool AnimationCheck = false;
        internal float AnimationCheckValue = 0;

        //ITask? ThreadHover = null;
        ITask? ThreadCheck = null;

        bool _checked = false;
        [Description("选中状态"), Category("行为"), DefaultValue(false)]
        public bool Checked
        {
            get => _checked;
            set
            {
                if (_checked == value) return;
                _checked = value;
                PARENT?.OnCheckedChanged(this, value);
                OnCheck();
                CheckState = value ? CheckState.Checked : CheckState.Unchecked;
            }
        }

        internal CheckState checkStateOld = CheckState.Unchecked;
        CheckState checkState = CheckState.Unchecked;
        [Description("选中状态"), Category("行为"), DefaultValue(CheckState.Unchecked)]
        public CheckState CheckState
        {
            get => checkState;
            set
            {
                if (checkState == value) return;
                checkState = value;
                bool __checked = value == CheckState.Checked;
                if (_checked != __checked)
                {
                    _checked = __checked;
                    PARENT?.OnCheckedChanged(this, __checked);
                    OnCheck();
                }
                if (value != CheckState.Unchecked) checkStateOld = value;
            }
        }

        void OnCheck()
        {
            ThreadCheck?.Dispose();
            if (PARENT != null && PARENT.IsHandleCreated && (PARENTITEM == null || PARENTITEM.expand) && show && Config.Animation)
            {
                AnimationCheck = true;
                if (_checked)
                {
                    ThreadCheck = new ITask(PARENT, () =>
                    {
                        AnimationCheckValue = AnimationCheckValue.Calculate(0.2F);
                        if (AnimationCheckValue > 1) { AnimationCheckValue = 1F; return false; }
                        Invalidate();
                        return true;
                    }, 20, () =>
                    {
                        AnimationCheck = false;
                        Invalidate();
                    });
                }
                else
                {
                    ThreadCheck = new ITask(PARENT, () =>
                    {
                        AnimationCheckValue = AnimationCheckValue.Calculate(-0.2F);
                        if (AnimationCheckValue <= 0) { AnimationCheckValue = 0F; return false; }
                        Invalidate();
                        return true;
                    }, 20, () =>
                    {
                        AnimationCheck = false;
                        Invalidate();
                    });
                }
            }
            else AnimationCheckValue = _checked ? 1F : 0F;
            Invalidate();
        }

        #endregion

        #region 样式

        Color? fore;
        /// <summary>
        /// 文本颜色
        /// </summary>
        [Description("文本颜色"), Category("外观"), DefaultValue(null)]
        public Color? Fore
        {
            get => fore;
            set
            {
                if (fore == value) return;
                fore = value;
                Invalidate();
            }
        }

        Color? back;
        /// <summary>
        /// 背景颜色
        /// </summary>
        [Description("背景颜色"), Category("外观"), DefaultValue(null)]
        public Color? Back
        {
            get => back;
            set
            {
                if (back == value) return;
                back = value;
                Invalidate();
            }
        }

        #endregion

        void Invalidate()
        {
            PARENT?.Invalidate();
        }
        void Invalidates()
        {
            if (PARENT == null) return;
            PARENT.ChangeList();
            PARENT.Invalidate();
        }

        internal float SubY { get; set; }
        internal float SubHeight { get; set; }
        internal float ExpandHeight { get; set; }
        internal float ExpandProg { get; set; }
        internal bool ExpandThread { get; set; }
        internal bool show { get; set; }
        internal bool Show { get; set; }

        bool hover = false;
        /// <summary>
        /// 是否移动
        /// </summary>
        internal bool Hover
        {
            get => hover;
            set
            {
                if (hover == value) return;
                hover = value;
                PARENT?.OnNodeMouseMove(this, value);
                if (Config.Animation)
                {
                    ThreadHover?.Dispose();
                    AnimationHover = true;
                    var t = Animation.TotalFrames(20, 200);
                    if (value)
                    {
                        ThreadHover = new ITask((i) =>
                        {
                            AnimationHoverValue = Animation.Animate(i, t, 1F, AnimationType.Ball);
                            Invalidate();
                            return true;
                        }, 20, t, () =>
                        {
                            AnimationHover = false;
                            AnimationHoverValue = 1;
                            Invalidate();
                        });
                    }
                    else
                    {
                        ThreadHover = new ITask((i) =>
                        {
                            AnimationHoverValue = 1F - Animation.Animate(i, t, 1F, AnimationType.Ball);
                            Invalidate();
                            return true;
                        }, 20, t, () =>
                        {
                            AnimationHover = false;
                            AnimationHoverValue = 0;
                            Invalidate();
                        });
                    }
                }
                else Invalidate();
            }
        }

        bool select = false;
        [Description("激活状态"), Category("行为"), DefaultValue(false)]
        public bool Select
        {
            get => select;
            set
            {
                if (select == value) return;
                if (value) PARENT?.IUSelect();
                select = value;
                Invalidate();
            }
        }

        public int Depth { get; private set; }
        internal Tree? PARENT { get; set; }
        public TreeItem? PARENTITEM { get; set; }

        internal void SetRect(Canvas g, Font font, int depth, bool checkable, bool blockNode, bool has_sub, Rectangle _rect, int icon_size, int gap)
        {
            Depth = depth;
            rect = _rect;
            int x = _rect.X + gap + (icon_size * depth), y = _rect.Y + (_rect.Height - icon_size) / 2;
            if (has_sub)
            {
                arr_rect = new Rectangle(x, y, icon_size, icon_size);
                x += icon_size + gap;
            }

            if (checkable)
            {
                check_rect = new Rectangle(x, y, icon_size, icon_size);
                x += icon_size + gap;
            }

            if (HasIcon)
            {
                ico_rect = new Rectangle(x, y, icon_size, icon_size);
                x += icon_size + gap;
            }

            var size = g.MeasureString(Text, font);
            txt_rect = new Rectangle(x, _rect.Y + (_rect.Height - size.Height) / 2, size.Width, size.Height);
            if (SubTitle != null)
            {
                var sizesub = g.MeasureString(SubTitle, font);
                subtxt_rect = new Rectangle(txt_rect.Right, txt_rect.Y, sizesub.Width, txt_rect.Height);
                if (!blockNode) rect = new Rectangle(txt_rect.X, txt_rect.Y, txt_rect.Width + subtxt_rect.Width, subtxt_rect.Height);
            }
            else if (!blockNode) rect = txt_rect;

            Show = true;
        }
        internal Rectangle rect { get; set; }
        internal Rectangle arr_rect { get; set; }

        internal int Contains(Point point, int x, int y, bool checkable)
        {
            var p = new Point(point.X + x, point.Y + y);
            if (rect.Contains(p))
            {
                Hover = true;
                return 1;
            }
            else if (arr_rect.Contains(p) && CanExpand)
            {
                Hover = rect.Contains(arr_rect);
                return 2;
            }
            else if (checkable && check_rect.Contains(p))
            {
                Hover = rect.Contains(arr_rect);
                return 3;
            }
            else
            {
                Hover = false;
                return 0;
            }
        }

        internal float AnimationHoverValue = 0;
        internal bool AnimationHover = false;
        ITask? ThreadHover = null;

        internal Rectangle check_rect { get; set; }
        internal Rectangle txt_rect { get; set; }
        internal Rectangle subtxt_rect { get; set; }
        internal Rectangle ico_rect { get; set; }

        public override string? ToString() => text;
    }
}