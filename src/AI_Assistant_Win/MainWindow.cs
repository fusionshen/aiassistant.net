using AI_Assistant_Win.Business;
using AI_Assistant_Win.Properties;
using AI_Assistant_Win.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace AI_Assistant_Win
{
    public partial class MainWindow : AntdUI.Window
    {
        public static string PRODUCT_VERSION = "1.0.1";

        public static bool SOMETHING_IS_UNDONE = false;

        private readonly ApiBLL apiBLL = ApiHandler.Instance.GetApiBLL();

        public MainWindow(bool top)
        {
            PRODUCT_VERSION = this.ProductVersion;
            InitializeComponent();
            windowBar.Text += " " + PRODUCT_VERSION;
            notifyIcon1.Text += " " + PRODUCT_VERSION;
            TopMost = top;
            colorTheme.ValueChanged += ColorTheme_ValueChanged;
            btn_global.Items.AddRange([
                new AntdUI.SelectItem("简体中文","zh-CN"),
                new AntdUI.SelectItem("English","en-US")
            ]);
            var lang = AntdUI.Localization.CurrentLanguage;
            if (lang.StartsWith("en")) btn_global.SelectedValue = btn_global.Items[1];
            else btn_global.SelectedValue = btn_global.Items[0];
            apiBLL.PropertyChanged += LoginInfo_PropertyChangedAsync;
        }

        #region aotu shutdown timer
        private Timer inactivityTimer;
        private Timer dailyTimer;
        private UserActivityMessageFilter activityFilter;
        private CloseReason closeReason = CloseReason.None;
        private CancellationTokenSource cts;
        private readonly object syncLock = new object();

        // 初始化方法
        private void InitializeActivityFilter()
        {
            activityFilter = new UserActivityMessageFilter();
            activityFilter.RealUserActivity += (s, e) =>
            {
                ResetInactivityTimer();
                CancelShutdown("用户操作");
            };
            Application.AddMessageFilter(activityFilter);
        }

        private void InitializeTimers()
        {
            // 空闲计时器（测试用10秒）
            inactivityTimer = new Timer { Interval = 15 * 60 * 1000 };
            inactivityTimer.Tick += async (s, e) => await TriggerClose(CloseReason.ApplicationExitCall, LocalizeHelper.PROLONGED_INACTIVITY);
            inactivityTimer.Start();

            // 每日定时器
            dailyTimer = new Timer();
            UpdateDailyTimer();
        }

        private void UpdateDailyTimer()
        {
            var nextTime = GetNextTriggerTime();
            dailyTimer.Interval = (int)Math.Max((nextTime - DateTime.Now).TotalMilliseconds, 1);
            dailyTimer.Tick += async (s, e) => await TriggerClose(CloseReason.ApplicationExitCall, LocalizeHelper.SCHEDULED_SHUTDOWN);
            dailyTimer.Start();
        }

        private DateTime GetNextTriggerTime()
        {
            DateTime now = DateTime.Now;

            // 当天9点和21点
            DateTime today9AM = new DateTime(now.Year, now.Month, now.Day, 9, 0, 0);
            DateTime today9PM = new DateTime(now.Year, now.Month, now.Day, 21, 0, 0);

            // 当前时间在9点前 -> 返回当天9点
            if (now < today9AM) return today9AM;

            // 当前时间在9点~21点之间 -> 返回当天21点
            if (now < today9PM) return today9PM;

            // 当前时间已过21点 -> 返回次日9点
            return today9AM.AddDays(1);
        }

        private void ResetInactivityTimer()
        {
            lock (syncLock)
            {
                inactivityTimer?.Stop();
                inactivityTimer?.Start();
            }
        }

        // 增强版关闭流程控制
        private async Task TriggerClose(CloseReason reason, string logMessage)
        {
            lock (syncLock)
            {
                if (closeReason != CloseReason.None) return;
                closeReason = reason;
                cts?.Dispose();
                cts = new CancellationTokenSource();
            }
            try
            {
                // 显示3秒提示
                ShowNotification(logMessage, 3);
                await Task.Delay(1000, cts.Token);

                // 显示2秒提示
                ShowNotification(logMessage, 2);
                await Task.Delay(1000, cts.Token);

                // 显示1秒提示
                ShowNotification(logMessage, 1);
                await Task.Delay(1000, cts.Token);

                // 执行关闭
                SafeClose();
            }
            catch (TaskCanceledException)
            {
                // 取消时显示提示
                ShowCancelNotification();
            }
        }

        // 安全关闭方法
        private void SafeClose()
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)SafeClose);
                return;
            }

            if (!IsDisposed)
            {
                Close();
            }
        }

        // 取消关闭流程
        private void CancelShutdown(string reason)
        {
            lock (syncLock)
            {
                if (closeReason == CloseReason.None) return;

                cts?.Cancel();
                closeReason = CloseReason.None;

                Debug.WriteLine($"{DateTime.Now:HH:mm:ss} 关闭已取消 - {reason}");
            }
        }

        // 显示通知的封装方法
        private void ShowNotification(string message, int seconds)
        {
            BeginInvoke((MethodInvoker)(() =>
            {
                AntdUI.Notification.warn(
                    form: this,
                    title: LocalizeHelper.PROMPT,
                    text: $"{message},{LocalizeHelper.APPLICATION_WILL_SHUT_DOWN_IN_SECONDS(seconds)}",
                    align: AntdUI.TAlignFrom.BR,
                    font: Font
                );
            }));
        }

        // 显示取消通知
        private void ShowCancelNotification()
        {
            BeginInvoke((MethodInvoker)(() =>
            {
                AntdUI.Notification.success(
                    form: this,
                    title: LocalizeHelper.SUCCESS,
                    text: LocalizeHelper.SHUTDOWN_PROCESS_HAS_BEEN_CANCELLED,
                    align: AntdUI.TAlignFrom.BR,
                    font: Font
                );
            }));
        }

        // 窗体关闭处理
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (closeReason == CloseReason.ApplicationExitCall)
            {
                e.Cancel = false;
                CleanupResources();
            }
            else
            {
                var config = new AntdUI.Modal.Config(
                    LocalizeHelper.CONFIRM,
                    LocalizeHelper.CLOSE_CONFIRM,
                    AntdUI.TType.Warn)
                {
                    OnButtonStyle = (id, btn) => btn.BackExtend = "135, #6253E1, #04BEFE",
                    CancelText = LocalizeHelper.CANCEL,
                    OkText = LocalizeHelper.CONFIRM
                };
                e.Cancel = AntdUI.Modal.open(config) != DialogResult.OK;
            }
            // 仅在确认关闭时释放
            if (!e.Cancel)
            {
                notifyIcon1.Dispose();
            }
        }

        // 资源清理
        private void CleanupResources()
        {
            lock (syncLock)
            {
                cts?.Cancel();
                cts?.Dispose();

                inactivityTimer.SafeDispose();
                dailyTimer.SafeDispose();

                Application.RemoveMessageFilter(activityFilter);
                activityFilter = null;
            }
        }

        #endregion

        #region login & userinfo
        private void LoginInfo_PropertyChangedAsync(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "LoginToken")
            {
                if (apiBLL.LoginToken == null)
                {
                    AntdUI.Notification.warn(this, LocalizeHelper.PROMPT, LocalizeHelper.OFFLINE_PROMPT, AntdUI.TAlignFrom.BR, Font);
                    CheckLogin();
                }
            }
            else if (e.PropertyName == "LoginUserInfo")
            {
                if (apiBLL.LoginUserInfo != null)
                {
                    avatarLoginUser.Enabled = true;
                    AntdUI.Message.success(this, $"{LocalizeHelper.LOGIN_SUCCESS}{apiBLL.LoginUserInfo.Nickname}", Font);
                    // update avater
                    if (!string.IsNullOrEmpty(apiBLL.LoginUserInfo.Avatar))
                    {
                        avatarLoginUser.Spin(async () =>
                        {
                            try
                            {
                                avatarLoginUser.Image = await apiBLL.GetUserAvatarAsync();
                            }
                            catch (Exception error)
                            {
                                AntdUI.Notification.warn(this, LocalizeHelper.PROMPT, error.Message, AntdUI.TAlignFrom.BR, Font);
                            }
                        });
                    }
                }
                else
                {
                    avatarLoginUser.Image = Resources.img1;
                    avatarLoginUser.Enabled = false;
                    apiBLL.LoginToken = null;
                }
            }
        }

        private void AvatarLoginUser_Click(object sender, EventArgs e)
        {
            AntdUI.Popover.open(avatarLoginUser, new LoginUserInfo(this) { Size = new Size(278, 500) });
        }


        private void CheckLogin()
        {
            if (apiBLL.LoginUserInfo != null)
            {
                return;
            }
            var login = new Login(this);
            var result = AntdUI.Modal.open(new AntdUI.Modal.Config(this, LocalizeHelper.LOGIN_MODAL_TITLE, login)
            {
                OnButtonStyle = (id, btn) =>
                {
                    btn.BackExtend = "135, #6253E1, #04BEFE";
                },
                CancelText = null,
                OkText = LocalizeHelper.LOGIN,
                OnOk = config =>
                {
                    try
                    {
                        // 如果你在UI线程（即主线程）上调用PostAsync并且没有正确地处理异步操作，可能会导致死锁。
                        // 这是因为HttpClient在某些情况下可能会等待UI线程上的某些操作完成（例如，等待同步上下文完成），而UI线程又因为等待HttpClient的响应而被阻塞。
                        // 解决方案：确保你在UI线程上调用PostAsync时使用await关键字，并且不要使用.Result或.GetAwaiter().GetResult()来同步地等待异步方法的结果。
                        // 这些同步等待方法会阻塞调用线程，从而导致死锁。
                        // 超时登录必须这么写，await也不能解决这种情况，因为\AntdUI\Forms\LayeredWindow\LayeredFormModal.cs中btn_ok_Click开了另外条线程来获取OK按钮结果。
                        // 在网络不连通的情况下,20秒才会给出返回，此时过早获取OK状态，modal框消失，主界面显示，后台线程运行登录结果，过20秒之后才会弹出网络不通的message。
                        // 类型代码可以说一点作用都没有，因为OK就是登录按钮是一定会点的。modal框过早消失就会弹出主界面，如果在去控制主界面登录逻辑就太费时费力不讨好了。
                        // 客户要求所有操作必须先登录
                        // if (result == DialogResult.OK)
                        // {
                        //    // spin too short
                        //    avatarLoginUser.Spin(async () =>
                        //    {
                        //        avatarLoginUser.Enabled = false;
                        //        try
                        //        {
                        //            await login.SignIn();
                        //            AntdUI.Notification.success(this, "成功", "登录成功", AntdUI.TAlignFrom.BR, Font);
                        //        }
                        //        catch (Exception error)
                        //        {
                        //            AntdUI.Notification.error(this, "失败", error.Message, AntdUI.TAlignFrom.BR, Font);
                        //            AvatarLoginUser_Click(null, null);
                        //        }
                        //        avatarLoginUser.Enabled = true;
                        //    });
                        // }
                        var success = login.SignInAsync().Result;   // 同时，正好利用了自带的转圈圈等待效果，简直完美2024年12月25日01点41分
                        return success;
                    }
                    catch (Exception error)
                    {
                        AntdUI.Notification.error(this, LocalizeHelper.FAIL, error.Message, AntdUI.TAlignFrom.BR, Font);
                        return false;
                    }
                }
            });
            if (result == DialogResult.OK)
            {
                try
                {
                    _ = apiBLL.GetUserInfoAsync();   // 这个地方使用.Result会导致死锁
                }
                catch (Exception error)
                {
                    AntdUI.Notification.error(this, LocalizeHelper.FAIL, error.Message, AntdUI.TAlignFrom.BR, Font);
                }
            }
            else
            {
                // 初始化会卡出主界面阴影，而且注销时会自动Cancel
                CheckLogin();
            }
        }
        #endregion

        #region 加载列表
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            CheckLogin();
            windowBar.Loading = true;
            AntdUI.ITask.Run(LoadList);
            #region timer
            InitializeActivityFilter();
            InitializeTimers();
            #endregion
        }

        void LoadList()
        {
            var dir = GetMenuTree();
            var china = !System.Threading.Thread.CurrentThread.CurrentUICulture.Name.StartsWith("en");
            var list = new List<AntdUI.VirtualItem>();
            foreach (var it in dir)
            {
                var list_sub = new List<AntdUI.VirtualItem>();
                list_sub.AddRange(it.Value.Select(item =>
                new VItem(item, china)).ToList());
                list.Add(new TItem(it.Key, list_sub));
                list.AddRange(list_sub);
            }
            virtualPanel.Items.AddRange(list);
            windowBar.Loading = false;
            virtualPanel.BlurBar = windowBar;
        }

        private Dictionary<string, IList[]> GetMenuTree()
        {
            IList[] dir_Application =
                        [
                            // 应用
                            new IList("V60 Blackness Method On GA Sheet","GA板锌层密着性V60黑度检测", res_light.Divider, res_dark.Divider),
                            new IList("Circular Area Measurement On Galvanized Sheet","镀锌片圆形面积检测", res_light.Divider, res_dark.Divider)
                        ],
                    dir_History =
                        [
                            // 历史
                            new IList("Historical Record Of V60 Blackness Method","GA板V60黑度检测历史记录", res_light.Table, res_dark.Table),
                            new IList("Historical Record Of Circular Area Measurement","圆形面积检测历史记录", res_light.Table, res_dark.Table),
                            new IList("Tracer Of The Scale's Accuracy","精度溯源", res_light.Table, res_dark.Table)
                        ],
                    dir_Setting =
                        [
                            // 设置
                            new IList("Scale Setting","比例尺设置", res_light.Menu, res_dark.Menu)
                        ],
                    dir_General =
                        [
                            // 通用
                            new IList("Button","按钮", res_light.Button, res_dark.Button),
                            new IList("FloatButton","悬浮按钮",res_light.FloatButton, res_dark.FloatButton),
                            new IList("Icon","图标",res_light.Icon, res_dark.Icon)
                        ],
                    dir_Layout =
                        [
                            // 布局
                            new IList("Divider","分割线", res_light.Divider, res_dark.Divider)
                        ],
                    dir_Navigation =
                        [
                            // 导航
                            new IList("Breadcrumb","面包屑", res_light.Breadcrumb, res_dark.Breadcrumb),
                            new IList("Dropdown","下拉菜单", res_light.Dropdown, res_dark.Dropdown),
                            new IList("Menu","导航菜单", res_light.Menu, res_dark.Menu),
                            new IList("PageHeader","页头",res_light.PageHeader, res_dark.PageHeader),
                            new IList("Pagination","分页",res_light.Pagination, res_dark.Pagination),
                            new IList("Steps","步骤条",res_light.Steps, res_dark.Steps)
                        ],
                    dir_DataEntry =
                        [
                            // 数据录入
                            new IList("Checkbox","多选框", res_light.Checkbox, res_dark.Checkbox),
                            new IList("ColorPicker","颜色选择器", res_light.ColorPicker, res_dark.ColorPicker),
                            new IList("DatePicker","日期选择框", res_light.DatePicker, res_dark.DatePicker),
                            new IList("Input","输入框", res_light.Input, res_dark.Input),
                            new IList("InputNumber","数字输入框", res_light.InputNumber, res_dark.InputNumber),
                            new IList("Radio","单选框", res_light.Radio, res_dark.Radio),
                            new IList("Rate","评分", res_light.Rate, res_dark.Rate),
                            new IList("Select","选择器", res_light.Select, res_dark.Select),
                            new IList("Slider","滑动输入条",res_light.Slider, res_dark.Slider),
                            new IList("Switch","开关",res_light.Switch, res_dark.Switch),
                            new IList("TimePicker","时间选择框",res_light.TimePicker, res_dark.TimePicker)
                        ],
                    dir_DataDisplay =
                        [
                            // 数据展示
                            new IList("Avatar","头像", res_light.Avatar, res_dark.Avatar),
                            new IList("Badge","徽标数",res_light.Badge, res_dark.Badge),
                            new IList("Panel","面板", res_light.Panel, res_dark.Panel),
                            new IList("Carousel","走马灯",res_light.Carousel, res_dark.Carousel),
                            new IList("Collapse","折叠面板",res_light.Collapse, res_dark.Collapse),
                            new IList("Image","图片",res_light.Image, res_dark.Image),
                            new IList("Popover","气泡卡片",res_light.Popover, res_dark.Popover),
                            new IList("Segmented","分段控制器",res_light.Segmented, res_dark.Segmented),
                            new IList("Table","表格",res_light.Table, res_dark.Table),
                            new IList("Tabs","标签页",res_light.Tabs, res_dark.Tabs),
                            new IList("Tag","标签",res_light.Tag, res_dark.Tag),
                            new IList("Timeline","时间轴",res_light.Timeline, res_dark.Timeline),
                            new IList("Tooltip","文字提示",res_light.Tooltip, res_dark.Tooltip),
                            new IList("Tree","树形控件",res_light.Tree, res_dark.Tree)
                        ],
                    dir_Feedback =
                        [
                            // 反馈
                            new IList("Alert","警告提示",res_light.Alert, res_dark.Alert),
                            new IList("Drawer","抽屉",res_light.Drawer, res_dark.Drawer),
                            new IList("Message","全局提示",res_light.Message, res_dark.Message),
                            new IList("Modal","对话框",res_light.Modal, res_dark.Modal),
                            new IList("Notification","通知提醒框",res_light.Notification, res_dark.Notification),
                            new IList("Progress","进度条",res_light.Progress, res_dark.Progress),
                            new IList("Result","结果",res_light.Result, res_dark.Result)
                        ];

            var dir = new Dictionary<string, IList[]> {
                { AntdUI.Localization.Get("Application","应用"), dir_Application },
                { AntdUI.Localization.Get("HistoricalRecord","历史记录"), dir_History },
                { AntdUI.Localization.Get("SystemSetting","系统设置"), dir_Setting },
            };
            if (apiBLL.LoginUserInfo.Username.Contains("fusionshen"))
            {
                dir.Add(AntdUI.Localization.Get("General", "通用"), dir_General);
                dir.Add(AntdUI.Localization.Get("Layout", "布局"), dir_Layout);
                dir.Add(AntdUI.Localization.Get("Navigation", "导航"), dir_Navigation);
                dir.Add(AntdUI.Localization.Get("DataEntry", "数据录入"), dir_DataEntry);
                dir.Add(AntdUI.Localization.Get("DataDisplay", "数据展示"), dir_DataDisplay);
                dir.Add(AntdUI.Localization.Get("Feedback", "反馈"), dir_Feedback);
            }

            return dir;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            DraggableMouseDown();
            base.OnMouseDown(e);
        }

        private void ItemClick(object sender, AntdUI.VirtualItemEventArgs e) => OpenPage(e.Item.Tag.ToString());

        AntdUI.FormFloatButton FloatButton = null;

        public void OpenPage(string id)
        {
            windowBar.ShowBack = false;
            Control control_add = null;
            switch (id)
            {
                case "V60 Blackness Method On GA Sheet":
                    control_add = new Controls.BlacknessMethod(this);
                    break;
                case "Historical Record Of V60 Blackness Method":
                    control_add = new Controls.BlacknessHistory(this);
                    break;
                case "Circular Area Measurement On Galvanized Sheet":
                    control_add = new Controls.CircularAreaMethod(this);
                    break;
                case "Historical Record Of Circular Area Measurement":
                    control_add = new Controls.CircularAreaHistory(this);
                    break;
                case "Scale Setting":
                    control_add = new Controls.GaugeBlockMethod(this);
                    break;
                case "Tracer Of The Scale's Accuracy":
                    control_add = new Controls.ScaleAccuracyHistory(this);
                    break;
                case "Button":
                    control_add = new Controls.Button(this);
                    break;
                case "Icon":
                    control_add = new Controls.Icon(this);
                    break;
                case "Avatar":
                    control_add = new Controls.Avatar(this);
                    break;
                case "Carousel":
                    control_add = new Controls.Carousel(this);
                    break;
                case "Badge":
                    control_add = new Controls.Badge(this);
                    break;
                case "Checkbox":
                    control_add = new Controls.Checkbox(this);
                    break;
                case "Radio":
                    control_add = new Controls.Radio(this);
                    break;
                case "Input":
                    control_add = new Controls.Input(this);
                    break;
                case "Select":
                    control_add = new Controls.Select(this);
                    break;
                case "Panel":
                    control_add = new Controls.MainPanel(this);
                    break;
                case "Progress":
                    control_add = new Controls.Progress(this);
                    break;
                case "Result":
                    control_add = new Controls.Result(this);
                    break;
                case "Tooltip":
                    control_add = new Controls.Tooltip(this);
                    break;
                case "Divider":
                    control_add = new Controls.Divider(this);
                    break;
                case "Slider":
                    control_add = new Controls.Slider(this);
                    break;
                case "Tabs":
                    control_add = new Controls.Tabs(this);
                    break;
                case "Switch":
                    control_add = new Controls.Switch(this);
                    break;
                case "Pagination":
                    control_add = new Controls.Pagination(this);
                    break;
                case "Alert":
                    control_add = new Controls.Alert(this);
                    break;
                case "Message":
                    control_add = new Controls.Message(this);
                    break;
                case "Notification":
                    control_add = new Controls.Notification(this);
                    break;
                case "Menu":
                    control_add = new Controls.Menu(this);
                    break;
                case "Segmented":
                    control_add = new Controls.Segmented(this);
                    break;
                case "Modal":
                    control_add = new Controls.Modal(this);
                    break;
                case "DatePicker":
                    control_add = new Controls.DatePicker(this);
                    break;
                case "TimePicker":
                    control_add = new Controls.TimePicker(this);
                    break;
                case "Dropdown":
                    control_add = new Controls.Dropdown(this);
                    break;
                case "Tree":
                    control_add = new Controls.Tree(this);
                    break;
                case "Popover":
                    control_add = new Controls.Popover(this);
                    break;
                case "Timeline":
                    control_add = new Controls.Timeline(this);
                    break;
                case "Steps":
                    control_add = new Controls.Steps(this);
                    break;
                case "ColorPicker":
                    control_add = new Controls.ColorPicker(this);
                    break;
                case "InputNumber":
                    control_add = new Controls.InputNumber(this);
                    break;
                case "Tag":
                    control_add = new Controls.Tag(this);
                    break;
                case "Drawer":
                    control_add = new Controls.Drawer(this);
                    break;
                case "FloatButton":
                    if (FloatButton == null)
                    {
                        FloatButton = AntdUI.FloatButton.open(new AntdUI.FloatButton.Config(this, [
                            new("id1", "SearchOutlined", true){
                                Tooltip = "搜索一下",
                                Type= AntdUI.TTypeMini.Primary
                            },
                            new AntdUI.FloatButton.ConfigBtn("id2", Properties.Resources.img1){
                                Badge = " ",
                                Tooltip = "笑死人",
                            },
                            new AntdUI.FloatButton.ConfigBtn("id3",Properties.Resources.icon_like, true){
                                Badge = "9",
                                Tooltip = "救救我"
                            },
                            new AntdUI.FloatButton.ConfigBtn("id4", "PoweroffOutlined", true){
                                Badge = "99+",
                                Tooltip = "没救了",
                                Round = false,
                                Type= AntdUI.TTypeMini.Primary
                            }
                        ], btn =>
                        {
                            AntdUI.Message.info(this, "点击了：" + btn.Name, Font);
                        }));
                    }
                    else
                    {
                        FloatButton.Close();
                        FloatButton = null;
                    }
                    break;
                case "Rate":
                    control_add = new Controls.Rate(this);
                    break;
                case "Table":
#if DEBUG
                    control_add = new Controls.Table(this);
#else
                    control_add = new Controls.TableAOT(this);
#endif
                    break;
                case "Image":
                    control_add = new Controls.Preview(this);
                    break;
                case "VirtualPanel":
                    control_add = new Controls.VirtualPanel(this);
                    break;
                case "PageHeader":
                    control_add = new Controls.PageHeader(this);
                    break;
                case "Breadcrumb":
                    control_add = new Controls.Breadcrumb(this);
                    break;
                case "Collapse":
                    control_add = new Controls.Collapse(this);
                    break;
                default:
                    AntdUI.Notification.warn(this, LocalizeHelper.PROMPT, LocalizeHelper.UNDER_DEVELOPMENT, AntdUI.TAlignFrom.BR, Font);
                    break;
            }
            if (control_add != null)
            {
                windowBar.LocalizationSubText = id;
                windowBar.SubText = GetHeaderSubText(id);
                if (windowBar.Tag is Control control)
                {
                    control.Dispose();
                    Controls.Remove(control);
                }
                windowBar.Tag = control_add;
                BeginInvoke(new Action(async () =>
                {
                    virtualPanel.Visible = false;
                    control_add.Dock = DockStyle.Fill;
                    AutoDpi(control_add);
                    Controls.Add(control_add);
                    control_add.BringToFront();
                    control_add.Focus();

                    // 基础控件创建等待
                    if (!control_add.IsHandleCreated)
                    {
                        var tcs = new TaskCompletionSource<bool>();
                        control_add.HandleCreated += (s, e) => tcs.SetResult(true);
                        await tcs.Task;
                    }

                    // 自定义初始化等待（如果有）
                    if (control_add is IAsyncInit asyncInit)
                    {
                        await asyncInit.InitializeAsync();
                    }

                    windowBar.ShowBack = true;
                }));
            }
        }

        private void Btn_back_Click(object sender, EventArgs e)
        {
            ConfirmUnderUnsaved(() =>
            {
                if (windowBar.Tag is Control control)
                {
                    control.Dispose();
                    Controls.Remove(control);
                }
                windowBar.ShowBack = false;
                virtualPanel.Visible = true;
                windowBar.LocalizationSubText = "Workbench";
                windowBar.SubText = GetHeaderSubText("Workbench");
            }, LocalizeHelper.LEAVE_PAGE_CONFIRM_WHEN_SOMETHING_IS_UNDONE);
        }

        public void ConfirmUnderUnsaved(Action action, string content)
        {
            if (MainWindow.SOMETHING_IS_UNDONE)
            {
                var result = AntdUI.Modal.open(new AntdUI.Modal.Config(LocalizeHelper.CONFIRM,
                    content,
                    AntdUI.TType.Error)
                {
                    OnButtonStyle = (id, btn) =>
                    {
                        btn.BackExtend = "135, #6253E1, #04BEFE";
                    },
                    CancelText = LocalizeHelper.CANCEL,
                    OkText = LocalizeHelper.CONFIRM
                });
                if (result == DialogResult.OK)
                {
                    BeginInvoke(action);
                }
                ;
            }
            else
            {
                // 异步
                BeginInvoke(action);
            }
        }

        private void Btn_mode_Click(object sender, EventArgs e)
        {
            var color = AntdUI.Style.Db.Primary;
            AntdUI.Config.IsDark = !AntdUI.Config.IsDark;
            Dark = AntdUI.Config.IsDark;
            AntdUI.Style.Db.SetPrimary(color);
            btn_mode.Toggle = Dark;
            if (Dark)
            {
                BackColor = Color.Black;
                ForeColor = Color.White;
            }
            else
            {
                BackColor = Color.White;
                ForeColor = Color.Black;
            }
            OnSizeChanged(e);
        }

        private void Btn_setting_Click(object sender, EventArgs e)
        {
            var setting = new Setting(this);
            if (AntdUI.Modal.open(this, LocalizeHelper.SETTING, setting) == DialogResult.OK)
            {
                AntdUI.Config.Animation = setting.Animation;
                AntdUI.Config.ShadowEnabled = setting.ShadowEnabled;
                AntdUI.Config.ShowInWindow = setting.ShowInWindow;
                AntdUI.Config.ScrollBarHide = setting.ScrollBarHide;
            }
        }

        private void Btn_global_Changed(object sender, AntdUI.ObjectNEventArgs e)
        {
            if (e.Value is AntdUI.SelectItem value)
            {
                if (btn_global.SelectedValue == value) return;
                btn_global.SelectedValue = value;
                btn_global.Loading = true;
                string lang = value.Tag.ToString();
                if (lang.StartsWith("en")) AntdUI.Localization.Provider = new Localizer();
                else AntdUI.Localization.Provider = null;
                AntdUI.Localization.SetLanguage(lang);
                Refresh();
                AntdUI.ITask.Run(() =>
                {
                    int ScrollBarValue = virtualPanel.ScrollBar.Value;
                    virtualPanel.PauseLayout = true;
                    virtualPanel.Items.Clear();
                    LoadList();
                    virtualPanel.ScrollBar.Value = ScrollBarValue;
                    virtualPanel.PauseLayout = false;
                }, () =>
                {
                    btn_global.Loading = false;
                });
            }
        }

        private void ColorTheme_ValueChanged(object sender, AntdUI.ColorEventArgs e)
        {
            AntdUI.Style.Db.SetPrimary(e.Value);
            Refresh();
        }

        #region 搜索

        private void Txt_search_PrefixClick(object sender, MouseEventArgs e) => LoadSearchList();

        private void Txt_search_TextChanged(object sender, EventArgs e) => LoadSearchList();

        void LoadSearchList()
        {
            string search = txt_search.Text;
            windowBar.Loading = true;
            BeginInvoke(new Action(() =>
            {
                virtualPanel.PauseLayout = true;
                if (string.IsNullOrEmpty(search))
                {
                    foreach (var it in virtualPanel.Items) it.Visible = true;
                    virtualPanel.Empty = false;
                }
                else
                {
                    virtualPanel.Empty = true;
                    string searchLower = search.ToLower();
                    var titles = new List<TItem>(virtualPanel.Items.Count);
                    foreach (var it in virtualPanel.Items)
                    {
                        if (it is VItem item) it.Visible = item.data.Id.Contains(search) || item.data.Key.Contains(search) || item.data.Keyword.Contains(searchLower) || item.data.Keywordmini.Contains(searchLower);
                        else if (it is TItem itemTitle) titles.Add(itemTitle);
                    }
                    foreach (var it in titles)
                    {
                        int count = 0;
                        foreach (var item in it.data)
                        {
                            if (item.Visible) count++;
                        }
                        it.Visible = count > 0;
                    }
                }
                virtualPanel.PauseLayout = false;
                windowBar.Loading = false;
            }));
        }

        #endregion

        class IList(string _id, string _key, string _img_light, string _img_dark)
        {
            public string Id { get; set; } = _id;
            public string Keyword { get; set; } = _id.ToLower() + AntdUI.Pinyin.GetPinyin(_key).ToLower();
            public string Keywordmini { get; set; } = AntdUI.Pinyin.GetInitials(_key).ToLower();
            public string Key { get; set; } = _key;
            public Image[] Imgs { get; set; } = [AntdUI.SvgExtend.SvgToBmp(_img_light), AntdUI.SvgExtend.SvgToBmp(_img_dark)];
        }

        class TItem : AntdUI.VirtualItem
        {
            private readonly string title;
            private readonly string count;
            public List<AntdUI.VirtualItem> data;
            public TItem(string t, List<AntdUI.VirtualItem> d)
            {
                CanClick = false;
                data = d;
                title = t;
                count = d.Count.ToString();
            }

            readonly StringFormat s_f = AntdUI.Helper.SF_NoWrap(lr: StringAlignment.Near);
            readonly StringFormat s_c = AntdUI.Helper.SF_NoWrap();
            public override void Paint(AntdUI.Canvas g, AntdUI.VirtualPanelArgs e)
            {
                var dpi = AntdUI.Config.Dpi;

                int x = (int)(30 * dpi);
                using var font_title = new Font(e.Panel.Font, FontStyle.Bold);
                using var font_count = new Font(e.Panel.Font.FontFamily, e.Panel.Font.Size * .74F, e.Panel.Font.Style);
                var size = AntdUI.Helper.Size(g.MeasureString(title, font_title));
                g.String(title, font_title, AntdUI.Style.Db.Text, new Rectangle(e.Rect.X + x, e.Rect.Y, e.Rect.Width, e.Rect.Height), s_f);

                var rect_count = new Rectangle(e.Rect.X + x + size.Width, e.Rect.Y + (e.Rect.Height - size.Height) / 2, size.Height, size.Height);
                using (var path = AntdUI.Helper.RoundPath(rect_count, e.Radius))
                {
                    g.Fill(AntdUI.Style.Db.TagDefaultBg, path);
                    g.Draw(AntdUI.Style.Db.DefaultBorder, 1 * dpi, path);
                }
                g.String(count, font_count, AntdUI.Style.Db.Text, rect_count, s_c);
            }

            public override Size Size(AntdUI.Canvas g, AntdUI.VirtualPanelArgs e)
            {
                var dpi = AntdUI.Config.Dpi;
                return new Size(e.Rect.Width, (int)(44 * dpi));
            }
        }

        class VItem : AntdUI.VirtualShadowItem
        {
            public IList data;
            readonly string name;
            public VItem(IList d, bool china)
            {
                data = d;
                Tag = d.Id;
                // if (china) name = data.Id + " "+ data.Key;
                if (china) name = data.Key;
                else name = data.Id;
            }


            readonly StringFormat s_f = new() { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Near };
            public override void Paint(AntdUI.Canvas g, AntdUI.VirtualPanelArgs e)
            {
                var dpi = AntdUI.Config.Dpi;
                int title_height = (int)(44 * dpi), thickness = (int)(1 * dpi), size = (int)(10 * dpi), size2 = size * 2;
                using (var brush = new SolidBrush(AntdUI.Style.Db.BgContainer))
                {
                    using var path = AntdUI.Helper.RoundPath(e.Rect, e.Radius);
                    g.Fill(brush, path);
                    using var brush_bor = new Pen(Hover ? AntdUI.Style.Db.BorderColorDisable : AntdUI.Style.Db.BorderColor, thickness);
                    g.Draw(brush_bor, path);
                }
                using (var fore = new SolidBrush(AntdUI.Style.Db.Text))
                {
                    using var font_title = new Font(e.Panel.Font.FontFamily, 11F, FontStyle.Bold);
                    g.String(name, font_title, fore, new Rectangle(e.Rect.X + size2, e.Rect.Y, e.Rect.Width - size2, title_height), s_f);
                }
                using (var brush = new SolidBrush(AntdUI.Style.Db.Split))
                {
                    g.Fill(brush, new RectangleF(e.Rect.X + size, e.Rect.Y + title_height - thickness / 2F, e.Rect.Width - size2, thickness));
                }
                try
                {
                    var bmp = AntdUI.Config.IsDark ? data.Imgs[1] : data.Imgs[0];
                    g.Image(bmp, e.Rect.X + (e.Rect.Width - bmp.Width) / 2, (e.Rect.Y + title_height) + ((e.Rect.Height - title_height) - bmp.Height) / 2, bmp.Width, bmp.Height);
                }
                catch { }
            }

            public override Size Size(AntdUI.Canvas g, AntdUI.VirtualPanelArgs e)
            {
                var dpi = AntdUI.Config.Dpi;
                return new Size((int)(300 * dpi), (int)(244 * dpi));
            }
        }

        #endregion

        #region 副标题
        private string GetHeaderSubText(string id)
        {
            var result = GetMenuTree().SelectMany(t => t.Value.ToList()).ToList();
            var chinese = result.FirstOrDefault(t => t.Id.Equals(id))?.Key;
            return chinese ?? "工作台";
        }
        #endregion

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal; // 恢复窗体
            this.ShowInTaskbar = true;                // 显示任务栏图标
            this.Show();                              // 显示窗体
        }

        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Close(); // 退出程序
        }
    }
}