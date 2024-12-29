using AI_Assistant_Win.Models.Enums;
using AntdUI;

namespace AI_Assistant_Win.Utils
{
    public class LocalizeHelper
    {
        #region common
        public static string SUCCESS => Localization.Get("Success", "成功");

        public static string FAIL => Localization.Get("Fail", "失败");

        public static string PROMPT => Localization.Get("Prompt", "提示");

        public static string CONFIRM => Localization.Get("Confirm", "确认");

        public static string CANCEL => Localization.Get("Cancel", "取消");

        public static string SETTING => Localization.Get("Setting", "设置");

        public static string ERROR => Localization.Get("Error", "错误");

        public static string PLEASE_CONTACT_ADMIN => Localization.Get("Please contact the administrator.", "请联系管理员。");

        public static string PREDICTED_SUCCESSFULLY => Localization.Get("Predicted successfully!", "识别成功！");
        public static string SAVE_SUCCESSFULLY => Localization.Get("Saved successfully!", "保存成功！");
        public static string SAVE_FAILED => Localization.Get("Saved Failed！", "保存成功！");
        public static string JUMP_TO_HISTORY => Localization.Get("Would you like to jump to the history page?", "是否跳转至历史记录界面？");
        #endregion
        #region login
        public static string LOGIN_MODAL_TITLE => Localization.Get("Please enter your platform account", "请输入平台账户");

        public static string LOGIN => Localization.Get("Login", "登录");

        public static string LOGIN_SUCCESS => Localization.Get("Successful login! Welcome!", "成功登录！欢迎您！");

        public static string LOGOUT_CONFIRM_WHEN_SOMETHING_IS_UNDONE => Localization.Get("Are you sure you want to log out with unsaved work?",
            "您有未保存的工作，确定要注销吗？");

        public static string LOGOUT_CONFIRM => Localization.Get("Are you sure you want to log out?", "确定要注销吗？");

        public static string OFFLINE_PROMPT => Localization.Get("You are currently offline. You can log in again if needed.", "您已离线，如果需要可重新登录。");
        #endregion
        #region check data 
        public static string ID_IS_EMPTY => Localization.Get("The ID is empty.", "编号为空。");
        public static string ADD_SUBJECT_FAILED => Localization.Get("Failed to add a new subject.", "新增主体失败。");
        public static string SAVE_DETAILS_FAILED => Localization.Get("Failed to save the details.", "保存明细失败。");
        public static string FIND_SUBJECT_FAILED => Localization.Get("Failed to find the subject.", "查找主体失败。");
        public static string DELETE_DETAILS_FAILED => Localization.Get("Failed to delete the details.", "删除明细失败。");
        public static string UPDATE_SUBJECT_FAILED => Localization.Get("Failed to update the subject.", "更新主体失败。");
        public static string HAVE_NO_SUBJECT => Localization.Get(" has no subject data.", "没有主体数据。");
        public static string HAVE_NO_DETAILS => Localization.Get(" has no details data.", "没有明细数据。");
        public static string CERTAIN_ID(string id) => Localization.Get($"ID[{id}]", $"编号[{id}]");
        #endregion
        #region MainWindow
        public static string WORKBENCH => Localization.Get("Workbench", "工作台");
        public static string CLOSE_CONFIRM_WHEN_SOMETHING_IS_UNDONE => Localization.Get("Are you sure you want to close the application with unsaved work?",
            "您有未保存的工作，确定要关闭程序吗？");

        public static string CLOSE_CONFIRM => Localization.Get("Are you sure you want to close the application?", "确定要关闭程序吗？");


        public static string LEAVE_PAGE_CONFIRM_WHEN_SOMETHING_IS_UNDONE => Localization.Get("Are you sure you want to leave this page with unsaved work?",
            "您有未保存的工作，确定要离开此页面吗？");
        #endregion
        #region camera BLL
        public static string ENUMERATE_DEVICES_FAILED => Localization.Get("Enumerate devices fail!", "列举设备失败！");
        public static string CREATE_DEVICE_FAILED => Localization.Get("Create Device fail!", "创建设备失败！");
        public static string OPEN_DEVICE_FAILED => Localization.Get("Open Device fail!", "打开设备失败！");
        public static string GET_PACKET_SIZE_FAILED => Localization.Get("Warning: Get Packet Size failed!", "获取包大小失败！");
        public static string SET_PACKET_SIZE_FAILED => Localization.Get("Warning: Set Packet Size failed!", "设置包大小失败！");
        public static string START_THREAD_FAILED => Localization.Get("Start thread failed!,", "开启取图线程失败！");
        public static string START_GRABBING_FAILED => Localization.Get("Start Grabbing Fail!", "开启采集失败！");
        public static string IFRAMEOUT_CLONE_FAILED => Localization.Get("IFrameOut.Clone failed, ", "取图失败！");
        public static string STOP_GRABBING_FAILED => Localization.Get("Stop Grabbing Fail!", "暂停采集失败！");
        public static string NO_VAILD_IMAGE => Localization.Get("No vaild image!", "没有正确的图像！");
        public static string SAVE_IAMGE_FAILED => Localization.Get("Save Image Fail!", "保存图片失败！");
        public static string CAMERA_CONNECTED => Localization.Get("Camera connected!", "摄像头已连接！");
        public static string CAMERA_DISCONNECTED => Localization.Get("Camera disconnected!", "摄像头已离线！");
        public static string NO_CAMERA => Localization.Get("Failed to find an available camera.",
            EnumHelper.GetDescriptionOfEnum<CameraBLLStatusKind>(CameraBLLStatusKind.NoCamera.ToString()));
        public static string NO_CAMERA_SETTING => Localization.Get("Please set up the camera for real-time shooting.",
            EnumHelper.GetDescriptionOfEnum<CameraBLLStatusKind>(CameraBLLStatusKind.NoCameraSettings.ToString()));
        public static string NO_CAMERA_OPEN => Localization.Get("Please open the camera for real-time shooting.",
          EnumHelper.GetDescriptionOfEnum<CameraBLLStatusKind>(CameraBLLStatusKind.NoCameraOpen.ToString()));
        public static string NO_CAMERA_GRABBING => Localization.Get("Please start the acquisition for real-time shooting.",
   EnumHelper.GetDescriptionOfEnum<CameraBLLStatusKind>(CameraBLLStatusKind.NoCameraGrabbing.ToString()));
        public static string TRIGGER_MODE => Localization.Get("Trigger Mode",
   EnumHelper.GetDescriptionOfEnum<CameraBLLStatusKind>(CameraBLLStatusKind.TriggerMode.ToString()));
        public static string CONTINUOUS_MODE => Localization.Get("Continuous Mode",
  EnumHelper.GetDescriptionOfEnum<CameraBLLStatusKind>(CameraBLLStatusKind.ContinuousMode.ToString()));
        public static string UPLOAD_ORIGINAL_IMAGE_SUCCESSFULLY => Localization.Get("Uploaded original image successfully!", "上传原图成功！");
        public static string CAMERA_CAPTURED_SUCCESSFULLY => Localization.Get("Captured successfully!", "拍摄成功！");

        #endregion
        #region blackness method
        public static string LEVEL => Localization.Get("Level:", "等级：");
        public static string BLACKNESS_WITH => Localization.Get(",Width:", "，宽度：");
        public static string MILLIMETER => Localization.Get("mm", "毫米");
        public static string PLEASE_PREDICT_BEFORE_SAVING => Localization.Get("Please predict before saving.", "请进行识别后再保存。");
        public static string PLEASE_SELECT_WORKBENCH => Localization.Get("Please select a workbench.", "请选择班组。");
        public static string PLEASE_SELECT_TESTNO => Localization.Get("Please select a test no.", "请选择试样编号。");
        public static string PLEASE_INPUT_COIL_NUMBER => Localization.Get("Please text a coil number.", "请输入钢卷号。");
        public static string PLEASE_INPUT_SIZE => Localization.Get("Please text a size.", "请输入尺寸。");
        public static string PLEASE_INPUT_ANALYST => Localization.Get("Please text a analyst.", "请输入分析人。");
        public static string PLEASE_USE_CORRECT_IMAGE => Localization.Get("Please use the correct density pattern image for prediction.",
            "请使用正确的黑度样板图片进行识别。");
        public static string BLACKNESS_EDIT_MODE(string id) => Localization.Get($"Edit Mode[ID：{id}]", $"修改模式[编号：{id}]");
        public static string BLACKNESS_NEW_MODE => Localization.Get("New Mode", "新增模式");
        public static string ONLY_TEST_NO => Localization.Get("Due to interface issues, only a test sample number is provided.", "因接口问题，仅提供测试试样编号。");
        public static string WOULD_SAVE_BLACKNESS_RESULT => Localization.Get("Would you like to save the blackness detection result this time?", "是否保存本次黑度检测结果？");

        #endregion
    }
}
