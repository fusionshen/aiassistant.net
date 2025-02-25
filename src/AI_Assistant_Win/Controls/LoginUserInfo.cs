using AI_Assistant_Win.Business;
using AI_Assistant_Win.Utils;
using AntdUI;
using System;
using System.Windows.Forms;

namespace AI_Assistant_Win
{
    public partial class LoginUserInfo : UserControl
    {
        Form form;

        private readonly ApiBLL apiBLL = ApiHandler.Instance.GetApiBLL();
        public LoginUserInfo(Form _form)
        {
            form = _form;
            InitializeComponent();
            InitializeData();
        }

        private void InitializeData()
        {
            if (!string.IsNullOrEmpty(apiBLL.LoginUserInfo.Avatar))
            {
                avatar.Spin(async () =>
                {
                    try
                    {
                        avatar.Image = await apiBLL.GetUserAvatarAsync();
                    }
                    catch (Exception error)
                    {
                        AntdUI.Notification.warn(form, LocalizeHelper.PROMPT, error.Message, AntdUI.TAlignFrom.BR, Font);
                    }
                });
            }
            labelUsername.Text = $"{apiBLL.LoginUserInfo.Username}-{apiBLL.LoginUserInfo.Nickname}";
            labelDepartment.Text = Localization.Get($"Department:{apiBLL.LoginUserInfo.DepartmentName}", $"部门：{apiBLL.LoginUserInfo.DepartmentName}");
            labelPhone.Text = Localization.Get($"Phone:{apiBLL.LoginUserInfo.MobilePhone}", $"手机：{apiBLL.LoginUserInfo.MobilePhone}");
            labelLastLoginTime.Text = Localization.Get($"Last Login Time:{apiBLL.LoginUserInfo.LastLoginTime?.ToString("yyyy-MM-dd HH:mm:ss")}",
                $"上次登录：{apiBLL.LoginUserInfo.LastLoginTime?.ToString("yyyy-MM-dd HH:mm:ss")}");
            labelDescription.Text = string.IsNullOrEmpty(apiBLL.LoginUserInfo.Description) ? Localization.Get("No Description Available", "无简介") : apiBLL.LoginUserInfo.Description;

        }

        private void BtnSignout_Click(object sender, EventArgs e)
        {
            // form: MainWindow.if no, the style will change.
            var result = AntdUI.Modal.open(new AntdUI.Modal.Config(form, LocalizeHelper.CONFIRM,
                MainWindow.SOMETHING_IS_UNDONE ?
                LocalizeHelper.LOGOUT_CONFIRM_WHEN_SOMETHING_IS_UNDONE :
                LocalizeHelper.LOGOUT_CONFIRM, AntdUI.TType.Warn)
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
                apiBLL.Logout();
            }
        }
    }
}
