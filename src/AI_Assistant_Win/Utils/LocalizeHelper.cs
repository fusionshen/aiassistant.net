﻿using AI_Assistant_Win.Models.Enums;
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
        public static string PREVIEW_IMAGE => Localization.Get("Preview images", "预览图像");
        public static string REPORT => Localization.Get("Report", "报告");
        public static string EDIT => Localization.Get("Edit", "修改");
        public static string DELETE => Localization.Get("Delete", "删除");
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
        public static string WOULD_SAVE_BLACKNESS_RESULT => Localization.Get("Would you like to save the blackness detection result?", "是否保存本次黑度检测结果？");
        public static string WOULD_EDIT_BLACKNESS_RESULT => Localization.Get("Would you like to edit the blackness detection result?", "是否对本次黑度检测结果进行修改？");
        public static string BLACKNESS_TABLE_HEADER_ID => Localization.Get("Id", "等级");
        public static string BLACKNESS_TABLE_HEADER_TESTNO => Localization.Get("TestNo", "试样编号");
        public static string BLACKNESS_TABLE_HEADER_SIZE => Localization.Get("Size", "尺寸");
        public static string BLACKNESS_TABLE_HEADER_UPLOADED => Localization.Get("Uploaded", "是否上传");
        public static string BLACKNESS_TABLE_HEADER_COILNUMBER => Localization.Get("CoilNumber", "钢卷号");
        public static string BLACKNESS_TABLE_HEADER_LEVEL => Localization.Get("Level", "等级");
        public static string BLACKNESS_TABLE_HEADER_ANALYST => Localization.Get("Analyst", "分析人");
        public static string BLACKNESS_TABLE_HEADER_WORKGROUP => Localization.Get("WorkGroup", "班组");
        public static string BLACKNESS_TABLE_HEADER_CREATETIME => Localization.Get("CreateTime", "创建时间");
        public static string BLACKNESS_TABLE_HEADER_UPLOADER => Localization.Get("Uploader", "上传人");
        public static string BLACKNESS_TABLE_HEADER_UPLOADTIME => Localization.Get("UploadTime", "上传时间");
        public static string BLACKNESS_TABLE_HEADER_LASTREVISER => Localization.Get("LastReviser", "最近修改人");
        public static string BLACKNESS_TABLE_HEADER_LASTMODIFIEDTIME => Localization.Get("LastModifiedTime", "最后修改时间");
        public static string BLACKNESS_TABLE_HEADER_OPERATIONS => Localization.Get("Operations", "操作");
        public static string SURFACE_OP => Localization.Get("SurfaceOP-", "表面OP-");
        public static string SURFACE_CE => Localization.Get("SurfaceCE-", "表面CE-");
        public static string SURFACE_DR => Localization.Get("SurfaceDR-", "表面DR-");
        public static string INSIDE_OP => Localization.Get("InsideOP-", "里面OP-");
        public static string INSIDE_CE => Localization.Get("InsideCE-", "里面CE-");
        public static string INSIDE_DR => Localization.Get("InsideDR-", "里面DR-");
        public static string PRINT_PREVIEW => Localization.Get("Print Preview", "打印预览");
        public static string PRINT_PRINT => Localization.Get("Print", "打印");
        public static string PRINT_DOWNLOAD => Localization.Get("Download", "下载");
        public static string PRINT_UPLOAD => Localization.Get("Upload", "上传至业务系统");
        public static string PRINT_SETTINGS => Localization.Get("Print Settings", "打印设置");
        public static string PREVIEW_BEFORE_DOWNLOADING => Localization.Get("Please preview it before downloading.", "请先预览然后再下载");
        public static string PREVIEW_BEFORE_UPLOADING => Localization.Get("Please preview it before uploading.", "请先预览然后再上传");
        public static string CHOOSE_THE_LOCATION => Localization.Get("Choose the location to save the PDF file", "选择保存PDF文件的位置");
        public static string FILE_SAVED_LOCATION => Localization.Get("The PDF file has been successfully saved to:", "PDF文件已成功保存到: ");
        public static string WOULD_UPLOAD_BLACKNESS_RESULT => Localization.Get("Would you like to upload this blackness detection report and its results to the business system?", "是否将本次黑度检测报告及结果上传至业务系统？");
        public static string WOULD_REUPLOAD_BLACKNESS_RESULT(string coilNumber) => Localization.Get($"The system has detected that the steel coil number [{coilNumber}] has already been uploaded. Would you like to re-upload this report and its results to the business system and update the version?", $"系统检测到钢卷号[{coilNumber}]已经上传，是否将本次报告及结果重新上传至业务系统并更新版本？");
        #endregion
        #region table
        public static string DISPLAY_HEADER => Localization.Get("Display header", "显示表头");
        public static string FIX_HEADER => Localization.Get("Fix header", "固定表头");
        public static string DISPLAY_COLUMN_BORDER => Localization.Get("Display column border", "显示列边框");
        public static string ODD_AND_EVEN => Localization.Get("Odd and even rows", "奇偶显示");
        public static string COLUMN_SORTING => Localization.Get("Column sorting", "部分列排序");
        public static string MANUALLY_ADJUST_COLUMN_WIDTH => Localization.Get("Manually adjust the width", "手动调整列头宽度");
        public static string DRAG_COLUMN => Localization.Get("Drag colomns", "列拖拽");
        public static string GONG => Localization.Get("Total", "共");
        public static string YE => Localization.Get("Pages", "页");

        #endregion
    }
}