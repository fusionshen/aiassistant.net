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
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// Radio 单选框
    /// </summary>
    /// <remarks>单选框。</remarks>
    [Description("Radio 单选框")]
    [ToolboxItem(true)]
    [DefaultProperty("Checked")]
    [DefaultEvent("CheckedChanged")]
    public class Radio : IControl, IEventListener
    {
        #region 属性

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

        Color? fill;
        /// <summary>
        /// 颜色
        /// </summary>
        [Description("颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? Fill
        {
            get => fill;
            set
            {
                if (fill == value) return;
                fill = value;
                Invalidate();
            }
        }

        string? text = null;
        /// <summary>
        /// 文本
        /// </summary>
        [Description("文本"), Category("外观"), DefaultValue(null)]
        public override string? Text
        {
            get => this.GetLangI(LocalizationText, text);
            set
            {
                if (text == value) return;
                text = value;
                if (BeforeAutoSize()) Invalidate();
                OnTextChanged(EventArgs.Empty);
            }
        }

        [Description("文本"), Category("国际化"), DefaultValue(null)]
        public string? LocalizationText { get; set; }

        StringFormat stringFormat = Helper.SF_ALL(lr: StringAlignment.Near);
        ContentAlignment textAlign = ContentAlignment.MiddleLeft;
        /// <summary>
        /// 文本位置
        /// </summary>
        [Description("文本位置"), Category("外观"), DefaultValue(ContentAlignment.MiddleLeft)]
        public ContentAlignment TextAlign
        {
            get => textAlign;
            set
            {
                if (textAlign == value) return;
                textAlign = value;
                textAlign.SetAlignment(ref stringFormat);
                Invalidate();
            }
        }

        bool AnimationCheck = false;
        float AnimationCheckValue = 0;
        bool _checked = false;
        /// <summary>
        /// 选中状态
        /// </summary>
        [Description("选中状态"), Category("数据"), DefaultValue(false)]
        public bool Checked
        {
            get => _checked;
            set
            {
                if (_checked == value) return;
                _checked = value;
                CheckedChanged?.Invoke(this, new BoolEventArgs(value));
                ThreadCheck?.Dispose();
                if (IsHandleCreated && Config.Animation)
                {
                    AnimationCheck = true;
                    if (value)
                    {
                        ThreadCheck = new ITask(this, () =>
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

                        if (Parent != null)
                        {
                            foreach (var it in Parent.Controls)
                            {
                                if (it != this && it is Radio radio) radio.Checked = false;
                            }
                        }
                    }
                    else
                    {
                        ThreadCheck = new ITask(this, () =>
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
                else
                {
                    AnimationCheckValue = value ? 1F : 0F;
                    if (value)
                    {
                        if (Parent != null)
                        {
                            foreach (var it in Parent.Controls)
                            {
                                if (it != this && it is Radio radio) radio.Checked = false;
                            }
                        }
                    }
                }
                Invalidate();
            }
        }

        /// <summary>
        /// 点击时自动改变选中状态
        /// </summary>
        [Description("点击时自动改变选中状态"), Category("行为"), DefaultValue(false)]
        public bool AutoCheck { get; set; } = true;

        RightToLeft rightToLeft = RightToLeft.No;
        [Description("反向"), Category("外观"), DefaultValue(RightToLeft.No)]
        public override RightToLeft RightToLeft
        {
            get => rightToLeft;
            set
            {
                if (rightToLeft == value) return;
                rightToLeft = value;
                stringFormat.Alignment = RightToLeft == RightToLeft.Yes ? StringAlignment.Far : StringAlignment.Near;
                Invalidate();
            }
        }

        #endregion

        #region 事件

        /// <summary>
        /// Checked 属性值更改时发生
        /// </summary>
        [Description("Checked 属性值更改时发生"), Category("行为")]
        public event BoolEventHandler? CheckedChanged = null;

        #endregion

        #region 渲染

        protected override void OnPaint(PaintEventArgs e)
        {
            var rect = ClientRectangle.DeflateRect(Padding);
            var g = e.Graphics.High();
            var font_size = g.MeasureString(Text ?? Config.NullText, Font);
            rect.IconRectL(font_size.Height, out var icon_rect, out var text_rect);
            bool right = rightToLeft == RightToLeft.Yes;
            PaintChecked(g, rect, Enabled, icon_rect, right);
            if (right) text_rect.X = rect.Width - text_rect.X - text_rect.Width;
            using (var brush = new SolidBrush(Enabled ? (fore ?? Style.Db.Text) : Style.Db.TextQuaternary))
            {
                g.String(Text, Font, brush, text_rect, stringFormat);
            }
            this.PaintBadge(g);
            base.OnPaint(e);
        }

        #region 渲染帮助

        internal void PaintChecked(Canvas g, Rectangle rect, bool enabled, RectangleF icon_rect, bool right)
        {
            float dot_size = icon_rect.Height;
            if (right) icon_rect.X = rect.Width - icon_rect.X - icon_rect.Width;
            var bor2 = 2F * Config.Dpi;
            if (enabled)
            {
                var color = fill ?? Style.Db.Primary;
                if (AnimationCheck)
                {
                    float dot = dot_size * 0.3F;
                    using (var path = new GraphicsPath())
                    {
                        float dot_ant = dot_size - dot * AnimationCheckValue, dot_ant2 = dot_ant / 2F, alpha = 255 * AnimationCheckValue;
                        path.AddEllipse(icon_rect);
                        path.AddEllipse(new RectangleF(icon_rect.X + dot_ant2, icon_rect.Y + dot_ant2, icon_rect.Width - dot_ant, icon_rect.Height - dot_ant));
                        g.Fill(Helper.ToColor(alpha, color), path);
                    }
                    if (_checked)
                    {
                        float max = icon_rect.Height + ((rect.Height - icon_rect.Height) * AnimationCheckValue), alpha2 = 100 * (1F - AnimationCheckValue);
                        g.FillEllipse(Helper.ToColor(alpha2, color), new RectangleF(icon_rect.X + (icon_rect.Width - max) / 2F, icon_rect.Y + (icon_rect.Height - max) / 2F, max, max));
                    }
                    g.DrawEllipse(color, bor2, icon_rect);
                }
                else if (_checked)
                {
                    float dot = dot_size * 0.3F, dot2 = dot / 2F;
                    g.DrawEllipse(Color.FromArgb(250, color), dot, new RectangleF(icon_rect.X + dot2, icon_rect.Y + dot2, icon_rect.Width - dot, icon_rect.Height - dot));
                    g.DrawEllipse(color, bor2, icon_rect);
                }
                else
                {
                    if (AnimationHover)
                    {
                        g.DrawEllipse(Style.Db.BorderColor, bor2, icon_rect);
                        g.DrawEllipse(Helper.ToColor(AnimationHoverValue, color), bor2, icon_rect);
                    }
                    else if (ExtraMouseHover) g.DrawEllipse(color, bor2, icon_rect);
                    else g.DrawEllipse(Style.Db.BorderColor, bor2, icon_rect);
                }
            }
            else
            {
                g.FillEllipse(Style.Db.FillQuaternary, icon_rect);
                if (_checked)
                {
                    float dot = dot_size / 2F, dot2 = dot / 2F;
                    g.FillEllipse(Style.Db.TextQuaternary, new RectangleF(icon_rect.X + dot2, icon_rect.Y + dot2, icon_rect.Width - dot, icon_rect.Height - dot));
                }
                g.DrawEllipse(Style.Db.BorderColorDisable, bor2, icon_rect);
            }
        }

        #endregion

        #endregion

        #region 鼠标

        protected override void OnClick(EventArgs e)
        {
            if (AutoCheck) Checked = true;
            base.OnClick(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            Focus();
            base.OnMouseDown(e);
        }

        int AnimationHoverValue = 0;
        bool AnimationHover = false;
        bool _mouseHover = false;
        bool ExtraMouseHover
        {
            get => _mouseHover;
            set
            {
                if (_mouseHover == value) return;
                _mouseHover = value;
                var enabled = Enabled;
                SetCursor(value && enabled);
                if (enabled)
                {
                    if (Config.Animation)
                    {
                        ThreadHover?.Dispose();
                        AnimationHover = true;
                        if (value)
                        {
                            ThreadHover = new ITask(this, () =>
                            {
                                AnimationHoverValue += 20;
                                if (AnimationHoverValue > 255) { AnimationHoverValue = 255; return false; }
                                Invalidate();
                                return true;
                            }, 10, () =>
                            {
                                AnimationHover = false;
                                Invalidate();
                            });
                        }
                        else
                        {
                            ThreadHover = new ITask(this, () =>
                            {
                                AnimationHoverValue -= 20;
                                if (AnimationHoverValue < 1) { AnimationHoverValue = 0; return false; }
                                Invalidate();
                                return true;
                            }, 10, () =>
                            {
                                AnimationHover = false;
                                Invalidate();
                            });
                        }
                    }
                    else AnimationHoverValue = 255;
                    Invalidate();
                }
            }
        }

        #region 动画

        protected override void Dispose(bool disposing)
        {
            ThreadCheck?.Dispose();
            ThreadHover?.Dispose();
            base.Dispose(disposing);
        }
        ITask? ThreadHover = null;
        ITask? ThreadCheck = null;

        #endregion

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            ExtraMouseHover = true;
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            ExtraMouseHover = false;
        }
        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            ExtraMouseHover = false;
        }

        #endregion

        #region 自动大小

        /// <summary>
        /// 自动大小
        /// </summary>
        [Browsable(true)]
        [Description("自动大小"), Category("外观"), DefaultValue(false)]
        public override bool AutoSize
        {
            get => base.AutoSize;
            set
            {
                if (base.AutoSize == value) return;
                base.AutoSize = value;
                if (value)
                {
                    if (autoSize == TAutoSize.None) autoSize = TAutoSize.Auto;
                }
                else autoSize = TAutoSize.None;
                BeforeAutoSize();
            }
        }

        TAutoSize autoSize = TAutoSize.None;
        /// <summary>
        /// 自动大小模式
        /// </summary>
        [Description("自动大小模式"), Category("外观"), DefaultValue(TAutoSize.None)]
        public TAutoSize AutoSizeMode
        {
            get => autoSize;
            set
            {
                if (autoSize == value) return;
                autoSize = value;
                base.AutoSize = autoSize != TAutoSize.None;
                BeforeAutoSize();
            }
        }

        protected override void OnFontChanged(EventArgs e)
        {
            if (BeforeAutoSize()) Invalidate();
            base.OnFontChanged(e);
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            if (autoSize == TAutoSize.None) return base.GetPreferredSize(proposedSize);
            else if (autoSize == TAutoSize.Width) return new Size(PSize.Width, base.GetPreferredSize(proposedSize).Height);
            else if (autoSize == TAutoSize.Height) return new Size(base.GetPreferredSize(proposedSize).Width, PSize.Height);
            return PSize;
        }

        internal Size PSize
        {
            get
            {
                return Helper.GDI(g =>
                {
                    var font_size = g.MeasureString(Text ?? Config.NullText, Font);
                    int gap = (int)(20 * Config.Dpi);
                    return new Size(font_size.Width + font_size.Height + gap, font_size.Height + gap);
                });
            }
        }

        protected override void OnResize(EventArgs e)
        {
            BeforeAutoSize();
            base.OnResize(e);
        }

        internal bool BeforeAutoSize()
        {
            if (autoSize == TAutoSize.None) return true;
            if (InvokeRequired)
            {
                bool flag = false;
                Invoke(new Action(() =>
                {
                    flag = BeforeAutoSize();
                }));
                return flag;
            }
            var PS = PSize;
            switch (autoSize)
            {
                case TAutoSize.Width:
                    if (Width == PS.Width) return true;
                    Width = PS.Width;
                    break;
                case TAutoSize.Height:
                    if (Height == PS.Height) return true;
                    Height = PS.Height;
                    break;
                case TAutoSize.Auto:
                default:
                    if (Width == PS.Width && Height == PS.Height) return true;
                    Size = PS;
                    break;
            }
            return false;
        }

        #endregion

        #region 语言变化

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            this.AddListener();
        }

        public void HandleEvent(EventType id, object? tag)
        {
            switch (id)
            {
                case EventType.LANG:
                    BeforeAutoSize();
                    break;
            }
        }

        #endregion
    }
}