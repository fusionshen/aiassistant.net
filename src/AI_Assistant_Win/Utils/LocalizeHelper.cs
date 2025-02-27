using AI_Assistant_Win.Models.Enums;
using AI_Assistant_Win.Models.Middle;
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
        public static string TOTAL_SAMPLE_SIZE => Localization.Get("Total Size Of Samples: ", "总样本数量：");
        public static string SAMPLE_SIZE(double measurement) => Localization.Get($"Sample Size Of Gauge Block[{measurement}]: ", $"{measurement}毫米量块样本数量：");
        public static string CANCEL => Localization.Get("Cancel", "取消");
        public static string SETTING => Localization.Get("Setting", "设置");
        public static string SETTING_SAVE => Localization.Get("Save Settings", "保存设置");
        public static string ERROR => Localization.Get("Error", "错误");
        public static string PLEASE_CONTACT_ADMIN => Localization.Get("Please contact the administrator.", "请联系管理员。");
        public static string PREDICTED_SUCCESSFULLY => Localization.Get("Predicted successfully!", "识别成功！");
        public static string SAVE_SUCCESSFULLY => Localization.Get("Saved successfully!", "保存成功！");
        public static string SAVE_FAILED => Localization.Get("Saved Failed！", "保存成功！");
        public static string JUMP_TO_HISTORY => Localization.Get("Would you like to jump to the history page?", "是否跳转至历史记录界面？");
        public static string JUMP_TO_GAUGE_BLOCK_METHOD => Localization.Get("Would you like to jump to the scale setting page?", "是否跳转至比例尺设置界面？");
        public static string JUMP_TO_ACCURACY_TRACER => Localization.Get("Would you like to jump to the accuracy tracer page?", "是否跳转至精度溯源界面？");
        public static string PREVIEW_IMAGE => Localization.Get("Preview images", "预览图像");
        public static string REPORT => Localization.Get("Report", "报告");
        public static string EDIT => Localization.Get("Edit", "修改");
        public static string DELETE => Localization.Get("Delete", "删除");
        public static string UNDER_DEVELOPMENT => Localization.Get("This feature is currently under development.", "此功能目前正在开发中。");
        public static string LOADING_PAGE => Localization.Get("The page is loading...", "页面正在加载中...");
        public static string TESTNO_LIST_LOADED_SUCCESS => Localization.Get("The Test List Loaded", "试验清单加载成功");
        public static string OPENING_THE_CAMERA => Localization.Get("Opening The Camera...", "开启摄像头中...");
        public static string PAGE_LOADED_SUCCESS => Localization.Get("The Page Loaded", "页面加载成功");
        public static string IN_USE => Localization.Get("In use", "正在使用");
        public static string DEPRECATED => Localization.Get("Deprecated", "已弃用");

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
        public static string ADD_SUMMARY_FAILED => Localization.Get("Failed to add a new summary.", "新增概要失败。");
        public static string UPDATE_SUMMARY_FAILED => Localization.Get("Failed to save the summary.", "更新概要失败。");
        public static string FIND_SUBJECT_FAILED => Localization.Get("Failed to find the subject.", "查找主体失败。");
        public static string DELETE_DETAILS_FAILED => Localization.Get("Failed to delete the details.", "删除明细失败。");
        public static string UPDATE_SUBJECT_FAILED => Localization.Get("Failed to update the subject.", "更新主体失败。");
        public static string HAVE_NO_SUBJECT => Localization.Get(" has no subject data.", "没有主体数据。");
        public static string HAVE_NO_DETAILS => Localization.Get(" has no details data.", "没有明细数据。");
        public static string ADD_ACCURACY_TRACER_FAILED => Localization.Get("Failed to add a new summary.", "新增精度溯源失败。");
        public static string UPDATE_ACCURACY_TRACER_FAILED => Localization.Get("Failed to save the summary.", "更新精度溯源失败。");
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
        public static string PIXEL => Localization.Get("pixel", "像素");
        public static string PLEASE_SELECT_SCALE => Localization.Get("Please select a scale.", "请选择比例尺。");
        public static string PLEASE_PREDICT_BEFORE_SAVING => Localization.Get("Please predict before saving.", "请进行识别后再保存。");
        public static string PLEASE_PREDICT_WITH_NEW_SCALE_BEFORE_SAVING => Localization.Get("Please predict with the new scale before saving.", "请使用新的比例尺进行识别后再保存。");
        public static string PLEASE_SELECT_WORKBENCH => Localization.Get("Please select a workbench.", "请选择班组。");
        public static string PLEASE_SELECT_TESTNO => Localization.Get("Please select a test no.", "请选择试样编号。");
        public static string PLEASE_SELECT_EDGE => Localization.Get("Select one edge of the identified quadrilateral.", "请选择识别出的四边形的某一条边。");
        public static string PLEASE_INPUT_COIL_NUMBER => Localization.Get("Please text a coil number.", "请输入钢卷号。");
        public static string PLEASE_INPUT_CORRECT_EDGE_LENGTH => Localization.Get("Please text a correct edge length.", "请输入正确的边长。");
        public static string PLEASE_INPUT_EDGE_LENGTH(string edge) => Localization.Get($"Please enter the length of Edge {edge}.", $"请输入{edge}边的长度。");
        public static string PLEASE_INPUT_SIZE => Localization.Get("Please text a size.", "请输入尺寸。");
        public static string PLEASE_INPUT_ANALYST => Localization.Get("Please text a analyst.", "请输入分析人。");
        public static string PLEASE_USE_CORRECT_BLACKNESS_IMAGE => Localization.Get("Please use the correct blackness reference image for identification.", "请使用正确的黑度样板图片进行识别。");
        public static string PLEASE_SET_BLACKNESS_SCALE => Localization.Get("Please set the scale for calculating the correct width of blackness.", "请设置比例尺用于计算正确的黑度检测宽度。");
        public static string LOST_SCALE => Localization.Get("The scale was lost at that time, please re-predict using a new scale and save it.", "当时比例尺丢失，请重新使用新的比例尺进行识别后保存。");
        public static string SETTING_SCALE_FIRSTLY_BEFORE_PREDICTING => Localization.Get("You have not set any scale yet. Please set the length scale before performing recognition.", "您还未设置过任何比例尺，在识别之前请先设置长度比例尺。");
        public static string RESETTING_SCALE_BEFORE_PREDICTING => Localization.Get("You have doubts about the accuracy of the scale, or you want to reset the scale due to platform adjustments. Please confirm:", "您对比例尺准确度抱有疑问，或者调整过平台想要重新设置比例尺。请确认：");
        public static string BLACKNESS_EDIT_MODE(BlacknessResult result) => Localization.Get($"Edit Mode[TestNo:{result.TestNo}]", $"修改模式[试样编号：{result.TestNo}]");
        public static string NEW_MODE => Localization.Get("New Mode", "新增模式");
        public static string ONLY_TEST_NO => Localization.Get("Due to interface issues, only a test sample number is provided.", "因接口问题，仅提供测试试样编号。");
        public static string WOULD_SAVE_BLACKNESS_RESULT => Localization.Get("Would you like to save the blackness detection result?", "是否保存本次黑度检测结果？");
        public static string WOULD_EDIT_BLACKNESS_RESULT => Localization.Get("Would you like to edit the blackness detection result?", "是否对本次黑度检测结果进行修改？");
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
        public static string CHOOSE_THE_LOCATION => Localization.Get("Choose the location to save the file", "选择保存文件的位置");
        public static string FILE_SAVED_LOCATION => Localization.Get("The PDF file has been successfully saved to:", "PDF文件已成功保存到: ");
        public static string WOULD_UPLOAD_BLACKNESS_RESULT => Localization.Get("Would you like to upload this blackness detection report and its results to the business system?", "是否将本次黑度检测报告及结果上传至业务系统？");
        public static string WOULD_REUPLOAD_BLACKNESS_RESULT(string testNo) => Localization.Get($"The system has detected that the test number [{testNo}] has already been uploaded. Would you like to re-upload this report and its results to the business system and update the version?", $"系统检测到[{testNo}]已经上传，是否将本次黑度检测报告及结果重新上传至业务系统并更新版本？");
        public static string REPORT_UPLOAD_SUCCESS => Localization.Get("Successfully uploaded! The report can be viewed in the [File Management] module; the data can be viewed in the [Sample Management - Data Entry] page.", "成功上传！报告可在【文件管理】模块中查看，数据可在【试样管理-数据录入】查看。");
        public static string RESULT_UPLOADED => Localization.Get("Uploaded", "已上传");
        public static string RESULT_NOT_UPLOADED => Localization.Get("Not Uploaded", "未上传");
        public static string RESULT_WAITING_REUPLOAD => Localization.Get("Awaiting re-upload", "待重新上传");
        public static string WOULD_RESAVE_BLACKNESS_RESULT_AFTER_UPLOADING => Localization.Get($"The system has detected that the blackness report has already been uploaded. Do you confirm to modify this result? If so, please remember to re-upload this report and its results to the business system.", $"系统检测到本次黑度报告已经上传，是否确认修改本次结果？如果是，请记得将本次报告及结果重新上传至业务系统。");
        public static string CLEAR_PAGE_CONFIRM_WHEN_SOMETHING_IS_UNDONE => Localization.Get("Are you sure you want to clear this page with unsaved work?",
         "您有未保存的工作，确定要清空该页面吗？");
        public static string PRE_RECORD_CONFIRM_WHEN_SOMETHING_IS_UNDONE => Localization.Get("Are you sure you want to open the previous record with unsaved work?", "您有未保存的工作，确定要打开上一条记录吗？");
        public static string NEXT_RECORD_CONFIRM_WHEN_SOMETHING_IS_UNDONE => Localization.Get("Are you sure you want to open the next record with unsaved work?", "您有未保存的工作，确定要打开下一条记录吗？");
        #endregion
        #region scale
        public static string BLACKNESS_SCALE_MODAL_TITLE => Localization.Get("Blackness-Width Scale", "黑度宽度比例尺");
        public static string SCALE_CACULATED_RESULT_TITLE => Localization.Get("The Result:", "计算结果：");
        public static string LENGTH_SCALE_RESULT_TITLE => Localization.Get("Length Scale:", "长度比例尺：");
        public static string LENGTH_SCALE_CACULATED_RATIO_UNIT => Localization.Get("mm/pixel size", "毫米/像素边长");
        public static string AREA_SCALE_RESULT_TITLE => Localization.Get("Area Scale:", "面积比例尺：");
        public static string AREA_SCALE_CACULATED_RATIO_UNIT => Localization.Get("sq mm/pixel area", "平方毫米/像素面积");
        public static string CALCULATED_AREA_TITLE => Localization.Get("Calculated Scale:", "测算面积：");
        public static string SCALE_INPUT_ERROR => Localization.Get("Invalid Input", "非法输入");
        public static string CURRENT_SCALE_TITLE => Localization.Get("Current", "当前");
        public static string SCALE_TITLE_AT_THAT_TIME => Localization.Get("At That Time", "当时");
        public static string SCALE_LOAD_SUCCESSED => Localization.Get("Load Scale Successed:", "加载比例尺成功：");
        public static string NO_NEED_TO_SAVE_THE_SAME_SCALE => Localization.Get("The scale has not changed, so there is no need to save it again.", "比例尺没有变化，无需再次保存。");
        public static string PLEASE_SET_CIRCULAR_AREA_SCALE => Localization.Get("Please set the scale for calculating the correct area of circular.", "请设置比例尺用于计算正确的圆形检测面积。");
        public static string PLEASE_SET_SCALE => Localization.Get("Please set the scale for methods.", "请设置比例尺用于计算真实值。");
        public static string CIRCULAR_POSITION_TITLE => Localization.Get("Position:", "部位：");
        public static string CIRCULAR_POSITION(CircularPositionKind position) => Localization.Get(position.ToString(), EnumHelper.GetDescriptionOfEnum<CircularPositionKind>(position.ToString()));
        public static string AREA_TITLE => Localization.Get(",Area:", "，面积：");
        public static string CIRCULAR_DIAMETER_TITLE => Localization.Get(",Diameter:", "，直径：");
        public static string AREA_OF_PIXELS => Localization.Get("area of pixels", "像素面积");
        public static string SCALE_GRADE_TITLE => Localization.Get("Grade:", "刻度:");
        public static string SQUARE_MILLIMETER => Localization.Get("sq mm", "平方毫米");
        public static string AREA_PREDICTION_TITLE => Localization.Get("Area:", "面积：");
        public static string AREA_PREDICTION_CONFIDENCE => Localization.Get(",Confidence:", "，置信度：");
        public static string PLEASE_SELECT_POSITION => Localization.Get("Please select a position.", "请选择样品部位。");
        public static string PREDICT_FIRSTLY_BEFORE_SETTING_CIRCULAR_SCALE => Localization.Get("Please take a photo or upload a circular detection image first and successfully recognize it before attempting to set the scale.", "请先拍照或者上传一张圆片面积检测图片并且成功识别后再尝试进行比例尺设置。");
        public static string PREDICT_FIRSTLY_BEFORE_SETTING_GAUGE_SCALE => Localization.Get("Please take a photo or upload a gauge block image first and successfully recognize it before attempting to set the scale.", "请先拍照或者上传一张方形量块图片并且成功识别后再尝试进行比例尺设置。");
        public static string CIRCULAR_SCALE_MODAL_TITLE => Localization.Get("Circular-Area Scale", "圆片面积比例尺");
        public static string SCALE_PREVIEW_MODAL_TITLE => Localization.Get("Scale Preview", "比例尺查看");
        public static string SCALE_ACCURAY_TRACER_MODAL_TITLE => Localization.Get("Scale Accuracy Tracer", "精度溯源");
        public static string GAUGE_SCALE_SETTINGS_MODAL_TITLE => Localization.Get("Scale Setting", "比例尺设置");
        public static string AUTO_CALCULATE => Localization.Get("Auto Calculate", "自动计算");
        public static string NO_TOP_GRADUATIONS => Localization.Get("Please enter the platform scale on the upper surface of the sample at this time.", "请输入此时样品上表面的平台刻度。");
        public static string WOULD_RESAVE_CIRCULAR_AREA_RESULT_AFTER_UPLOADING => Localization.Get($"The system has detected that the area report has already been uploaded. Do you confirm to modify this result? If so, please remember to re-upload this report and its results to the business system.", $"系统检测到本次面积报告已经上传，是否确认修改本次结果？如果是，请记得将本次报告及结果重新上传至业务系统。");
        public static string WOULD_RESAVE_SCALE_PRECISION_RESULT_AFTER_UPLOADING => Localization.Get($"The system has detected that the accuracy report for this scale has already been uploaded. Do you confirm saving the current measurement results for updating the scale accuracy? If yes, please remember to upload this report and results again to the business system.", $"系统检测到本比例尺精度报告已经上传，是否确认保存本次测量结果，用于比例尺精度更新？如果是，请记得将本次报告及结果重新上传至业务系统。");
        public static string WOULD_SAVE_CIRCULAR_AREA_RESULT => Localization.Get("Would you like to save the area detection result?", "是否保存本次面积检测结果？");
        public static string WOULD_SAVE_SCALE_PRECISION_RESULT => Localization.Get("Do you want to save the current scale accuracy test results and perform the accuracy update?", "是否保存本次比例尺准确度检测结果并进行精度更新？");

        public static string WOULD_RESAVE_CIRCULAR_AREA_RESULT_ON_THIS_POSITION(string position) => Localization.Get($"The system has detected an existing area report for the {position} section. Do you confirm to overwrite and update it?", $"系统检测到已存在{position}部位的面积报告，是否确认覆盖更新？");
        public static string WOULD_RESAVE_SCALE_PRECISION_RESULT_ON_THIS_GRADE(string grade) => Localization.Get($"The system has detected that accuracy test results already exist for this scale at the [{grade}] level. Do you confirm saving the current measurement results for updating the scale accuracy?", $"系统检测到本比例尺在【{grade}】刻度下已存在准确度检测结果，是否确认保存本次测量结果，用于比例尺精度更新？");
        public static string PLEASE_USE_CORRECT_CIRCULAR_IMAGE => Localization.Get("Please use the correct circular image for identification.", "请使用正确的圆形图片进行识别。");
        public static string PLEASE_USE_CORRECT_GAUGE_IMAGE => Localization.Get("Please use the correct gauge image for identification.", "请使用正确的量块图片进行识别。");
        public static string CIRCULAR_AREA_EDIT_MODE(CircularAreaResult result) => Localization.Get($"Edit Mode[TestNo:{result.TestNo},Position:{result.Position}]", $"修改模式[试样编号：{result.TestNo}，部位：{result.Position}]");
        public static string GAUGE_BLOCK_EDIT_MODE(GaugeBlockResult result) => Localization.Get($"Edit Mode[ID:{result.Id},Length:{result.InputEdgeLength}]", $"修改模式[编号：{result.Id}，测量长度：{result.InputEdgeLength}mm]");
        public static string TABLE_MPE => Localization.Get("MPE(Maximum Permissible Error)", "MPE(最大允许误差)");
        public static string TABLE_AVERAGE => Localization.Get("Average", "平均值");
        public static string TABLE_STANDARD_DEVIATION => Localization.Get("StandardDeviationσ", "标准差σ");
        public static string TABLE_STANDARD_ERROR => Localization.Get("StandardError", "标准误差");
        public static string TABLE_UNCERTAINTY => Localization.Get("Uncertainty", "总不确定度");
        public static string TABLE_CONFIDENCE => Localization.Get("Confidence", "置信度");
        public static string TABLE_UPPER_SURFACE_OP => Localization.Get("UpperSurfaceOP", "上表面OP");
        public static string TABLE_UPPER_SURFACE_CE => Localization.Get("UpperSurfaceCE", "上表面CE");
        public static string TABLE_UPPER_SURFACE_DR => Localization.Get("UpperSurfaceDR", "上表面DR");
        public static string TABLE_LOWER_SURFACE_OP => Localization.Get("LowerSurfaceOP", "下表面OP");
        public static string TABLE_LOWER_SURFACE_CE => Localization.Get("LowerSurfaceCE", "下表面CE");
        public static string TABLE_LOWER_SURFACE_DR => Localization.Get("LowerSurfaceDR", "下表面DR");
        public static string WOULD_EDIT_CIRCULAR_AREA_RESULT => Localization.Get("Would you like to edit the circular area detection result?", "是否对本次圆片面积检测结果进行修改？");
        public static string WOULD_EDIT_SCALE_ACCURACY_RESULT => Localization.Get("Would you like to edit the gauge block detection result?", "是否对该比例尺下的量块长度检测结果进行修改？");
        public static string WOULD_UPLOAD_CIRCULAR_AREA_RESULT => Localization.Get("Would you like to upload this circular area detection report and its results to the business system?", "是否将本次圆片面积检测报告及结果上传至业务系统？");
        public static string WOULD_REUPLOAD_CIRCULAR_AREA_RESULT(string testNo) => Localization.Get($"The system has detected that the test number [{testNo}] has already been uploaded. Would you like to re-upload this report and its results to the business system and update the version?", $"系统检测到[{testNo}]已经上传，是否将本次圆片面积检测报告及结果重新上传至业务系统并更新版本？");
        public static string WOULD_UPLOAD_SCALE_ACCURACY_RESULT => Localization.Get("Would you like to upload this scale accuracy report to the business system?", "是否将本次比例尺精度报告及结果上传至业务系统？");
        public static string WOULD_REUPLOAD_SCALE_ACCURACY_RESULT(ScaleAccuracyTracerHistory history) => Localization.Get($"The system has detected that the accuracy report[{history.Scale.Value}mm/pixel size scale under {history.Tracer.MeasuredLength}mm] has already been uploaded. Would you like to re-upload this report and update the version?", $"系统检测到精度报告[在{history.Tracer.MeasuredLength}mm下的{history.Scale.Value}毫米/像素边长比例尺]已经上传，是否将本次报告重新上传至业务系统并更新版本？");

        public static string NOT_A_QUADRILATERAL => Localization.Get("Point set cannot form a quadrilateral.", "点集无法构成四边形。");
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
        public static string TABLE_HEADER_ID => Localization.Get("Id", "编号");
        public static string TABLE_HEADER_TESTNO => Localization.Get("TestNo", "试样编号");
        public static string TABLE_HEADER_SCALE => Localization.Get("Scale", "比例尺");
        public static string TABLE_HEADER_SIZE => Localization.Get("Size", "尺寸");
        public static string TABLE_HEADER_UPLOADED => Localization.Get("Uploaded", "是否上传");
        public static string TABLE_HEADER_IN_USE => Localization.Get("In Use", "使用");
        public static string TABLE_HEADER_COILNUMBER => Localization.Get("CoilNumber", "钢卷号");
        public static string TABLE_HEADER_MEASUREDLENGTH => Localization.Get("MeasuringLength", "测量长度");
        public static string TABLE_HEADER_LEVEL => Localization.Get("Level", "等级");
        public static string TABLE_HEADER_ANALYST => Localization.Get("Analyst", "分析人");
        public static string TABLE_HEADER_WORKGROUP => Localization.Get("WorkGroup", "班组");
        public static string TABLE_HEADER_CREATETIME => Localization.Get("CreateTime", "创建时间");
        public static string TABLE_HEADER_UPLOADER => Localization.Get("Uploader", "上传人");
        public static string TABLE_HEADER_UPLOADTIME => Localization.Get("UploadTime", "上传时间");
        public static string TABLE_HEADER_LASTREVISER => Localization.Get("LastReviser", "最近修改人");
        public static string TABLE_HEADER_LASTMODIFIEDTIME => Localization.Get("LastModifiedTime", "最后修改时间");
        public static string TABLE_HEADER_OPERATIONS => Localization.Get("Operations", "操作");
        public static string TABLE_HEADER_CREATOR => Localization.Get("Creator", "创建人");
        public static string CIRCULAR_AREA_DIAMETER => Localization.Get("Diameter:", "类圆直径：");
        public static string CELL_AREA_OF_PIXELS => Localization.Get("Pixels:", "像素面积：");
        public static string CELL_TITLE_ANALYST => Localization.Get("Analyst:", "分析人：");
        public static string CELL_HEADER_CREATETIME => Localization.Get("CreateTime:", "创建时间：");
        public static string CELL_HEADER_LASTREVISER => Localization.Get("LastReviser:", "最近修改人：");
        public static string CELL_HEADER_LASTMODIFIEDTIME => Localization.Get("LastModifiedTime:", "最后修改时间：");
        #endregion
    }
}
