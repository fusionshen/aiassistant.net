using AntdUI;

namespace AI_Assistant_Win.Utils
{
    public class LocalizeHelper
    {
        public static string LOGIN_MODAL_TITLE => Localization.Get("Please enter your platform account", "请输入平台账户");
        public static string LOGIN => Localization.Get("Login", "登录");

        public static string SUCCESS => Localization.Get("Success", "成功");

        public static string LOGIN_SUCCESS => Localization.Get("Successful login! Welcome!", "成功登录！欢迎您！");

        public static string FAIL => Localization.Get("Fail", "失败");

        public static string PROMPT => Localization.Get("Prompt", "提示");

        public static string CONFIRM => Localization.Get("Confirm", "确认");

        public static string CANCEL => Localization.Get("Cancel", "取消");

        public static string LOGOUT_CONFIRM_WHEN_SOMETHING_IS_UNDONE => Localization.Get("Are you sure you want to log out with unsaved work?",
            "您有未保存的工作，确定要注销吗？");

        public static string LOGOUT_CONFIRM => Localization.Get("Are you sure you want to log out?", "确定要注销吗？");

        public static string CLOSE_CONFIRM_WHEN_SOMETHING_IS_UNDONE => Localization.Get("Are you sure you want to close the application with unsaved work?",
            "您有未保存的工作，确定要关闭程序吗？");

        public static string CLOSE_CONFIRM => Localization.Get("Are you sure you want to close the application?", "确定要关闭程序吗？");

        public static string OFFLINE_PROMPT => Localization.Get("You are currently offline. You can log in again if needed.", "您已离线，如果需要可重新登录。");


        public static string LEAVE_PAGE_CONFIRM_WHEN_SOMETHING_IS_UNDONE => Localization.Get("Are you sure you want to leave this page with unsaved work?",
            "您有未保存的工作，确定要离开此页面吗？");

        public static string SETTING => Localization.Get("Setting", "设置");

        public static string ERROR => Localization.Get("Error", "错误");

    }
}
