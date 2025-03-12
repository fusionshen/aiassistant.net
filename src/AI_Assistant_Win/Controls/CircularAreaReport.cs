using AI_Assistant_Win.Business;
using AI_Assistant_Win.Models;
using AI_Assistant_Win.Models.Enums;
using AI_Assistant_Win.Models.Middle;
using AI_Assistant_Win.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using YoloDotNet.Extensions;

namespace AI_Assistant_Win.Controls
{
    public partial class CircularAreaReport : UserControl
    {
        private Form form;

        private readonly CircularAreaMethodBLL circularAreaMethodBLL;

        Bitmap memoryImage;

        AntdUI.FormFloatButton floatButton = null;

        private CircularAreaSummaryHistory target;

        private readonly CircularAreaUploadBLL uploadCircularAreaBLL;

        private readonly string testNo;

        private readonly Action callBack;

        public CircularAreaReport(Form _form, string _testNo, Action _callBack)
        {
            form = _form;
            testNo = _testNo;
            callBack = _callBack;
            circularAreaMethodBLL = new CircularAreaMethodBLL();
            uploadCircularAreaBLL = new CircularAreaUploadBLL();
            InitializeComponent();
            LoadData(testNo);
            Disposed += BlacknessReport_Disposed;
            AntdUI.ITask.Run(() =>
            {
                System.Threading.Thread.Sleep(1000);
                if (floatButton == null)
                {
                    var config = new AntdUI.FloatButton.Config(form, [
                            new("preview", "SearchOutlined", true){
                                Tooltip = LocalizeHelper.PRINT_PREVIEW
                            },
                            new("print",  "PrinterOutlined", true){
                                Tooltip = LocalizeHelper.PRINT_PRINT,
                                Type= AntdUI.TTypeMini.Primary
                            },
                            new("download",  "DownloadOutlined", true){
                                Tooltip = LocalizeHelper.PRINT_DOWNLOAD
                            },
                            new("upload",  "CloudUploadOutlined", true){
                                Tooltip = LocalizeHelper.PRINT_UPLOAD,
                                Type= AntdUI.TTypeMini.Error,
                            },
                            new("setting", "SettingOutlined", true){
                                Tooltip = LocalizeHelper.PRINT_SETTINGS
                            }
                    ], async btn =>
                    {
                        switch (btn.Name)
                        {
                            case "preview":
                                printPreviewDialog1.Document = printDocument1;
                                if (printPreviewDialog1.ShowDialog() == DialogResult.OK)
                                {
                                    printDocument1.Print();
                                }
                                break;
                            case "print":
                                printDocument1.Print();
                                break;
                            case "download":
                                if (memoryImage == null)
                                {
                                    AntdUI.Notification.warn(form, LocalizeHelper.PROMPT, LocalizeHelper.PREVIEW_BEFORE_DOWNLOADING,
                                        AntdUI.TAlignFrom.BR, Font);
                                    return;
                                }
                                // 弹出文件保存对话框
                                SaveFileDialog saveFileDialog = new()
                                {
                                    Filter = "PDF文件|*.pdf",
                                    DefaultExt = "pdf",
                                    FileName = $"{target.Summary.TestNo}_圆形面积检测报告.pdf",
                                    Title = LocalizeHelper.CHOOSE_THE_LOCATION
                                };
                                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                                {
                                    try
                                    {
                                        string pdfPath = saveFileDialog.FileName;
                                        FileHelper.SaveImageAsPDF(memoryImage, pdfPath);
                                        AntdUI.Message.success(form, LocalizeHelper.FILE_SAVED_LOCATION + pdfPath);
                                    }
                                    catch (Exception error)
                                    {
                                        AntdUI.Notification.error(form, LocalizeHelper.ERROR, error.Message, AntdUI.TAlignFrom.BR, Font);
                                    }
                                }
                                break;
                            case "upload":
                                if (memoryImage == null)
                                {
                                    AntdUI.Notification.warn(form, LocalizeHelper.PROMPT, LocalizeHelper.PREVIEW_BEFORE_UPLOADING,
                                        AntdUI.TAlignFrom.BR, Font);
                                    return;
                                }
                                // check uploaded with the same coil number
                                var lastUploaded = uploadCircularAreaBLL.GetLastUploaded(target);
                                if (AntdUI.Modal.open(form, LocalizeHelper.CONFIRM, lastUploaded != null ?
                                    LocalizeHelper.WOULD_REUPLOAD_CIRCULAR_AREA_RESULT(target.Summary.TestNo) :
                                    LocalizeHelper.WOULD_UPLOAD_CIRCULAR_AREA_RESULT) == DialogResult.OK)
                                {
                                    try
                                    {
                                        btn.Loading = true;
                                        await uploadCircularAreaBLL.Upload(memoryImage, target, lastUploaded);
                                        // refresh
                                        LoadData(testNo);
                                        // refresh parent form
                                        // 方案一：改用同步调用Invoke（推荐）
                                        // Invoke(callBack); // 注意：这里改用同步调用
                                        // 关键说明：
                                        // 1.Invoke是同步方法，会阻塞当前线程直到回调执行完成
                                        // 2.确保callBack中没有死循环或长时间阻塞操作，否则会卡住UI线程
                                        // 3. 适用于需要严格保证执行顺序的场景
                                        // 方案二：异步等待BeginInvoke完成（复杂场景）
                                        // 将BeginInvoke转换为可等待的Task
                                        var tcs = new TaskCompletionSource<bool>();
                                        BeginInvoke(new Action(() =>
                                        {
                                            try
                                            {
                                                callBack();
                                                tcs.SetResult(true);
                                            }
                                            catch (Exception ex)
                                            {
                                                tcs.SetException(ex);
                                            }
                                        }));
                                        await tcs.Task; // 等待回调完成
                                                        // 关键说明：
                                                        // 1.使用TaskCompletionSource将异步回调转换为可等待的Task
                                                        // 2.能捕获回调中抛出的异常并通过await传播到catch块
                                                        // 3.适用于需要保持UI响应的长时间操作
                                                        // 4.注意线程切换问题，回调中的UI操作不需要额外Invoke
                                                        // 安全释放资源
                                        AntdUI.Message.success(form, LocalizeHelper.REPORT_UPLOAD_SUCCESS);
                                        memoryImage?.Dispose();
                                        if (!this.IsDisposed)
                                        {
                                            this.Dispose();
                                        }
                                        // 两种方案对比：
                                        // 特性       方案一（Invoke）	方案二（BeginInvoke + Task）
                                        // 线程阻塞          是           否
                                        // UI响应性       可能卡顿       保持流畅
                                        // 异常处理        直接抛出     通过Task传播
                                        // 代码复杂度        简单         较复杂
                                        // 适用场景         短操作        长操作
                                    }
                                    catch (Exception ex)
                                    {
                                        AntdUI.Notification.error(form, LocalizeHelper.ERROR, ex.Message, AntdUI.TAlignFrom.BR, Font);
                                    }
                                    finally
                                    {
                                        btn.Loading = false;
                                    }
                                }
                                // 选择方案时应根据实际业务场景决定。如果回调操作耗时短（<200ms），建议使用方案一；如果包含复杂操作，推荐方案二以保证UI流畅性。
                                break;
                            case "setting":
                                pageSetupDialog1.Document = printDocument1;
                                pageSetupDialog1.ShowDialog();
                                break;
                            default:
                                break;
                        }
                    })
                    {
                        Vertical = false,  //  不遮挡内容
                        TopMost = true
                    };
                    floatButton = AntdUI.FloatButton.open(config);
                }
                else
                {
                    floatButton.Close();
                    floatButton = null;
                }
            });

        }

        private void LoadData(string testNo)
        {
            try
            {
                target = circularAreaMethodBLL.GetSummaryListByConditions(null, null, testNo).Single();
                labelDate.Text = target.Summary.CreateTime?.ToString("yyyy 年 MM 月 dd 日");
                #region workGroup
                switch (target.MethodList.FirstOrDefault()?.WorkGroup)
                {
                    case "甲-白":
                        checkbox_Jia_Day.Checked = true;
                        checkbox_Jia_Night.Checked = false;
                        checkbox_Yi_Day.Checked = false;
                        checkbox_Yi_Night.Checked = false;
                        checkbox_Bing_Day.Checked = false;
                        checkbox_Bing_Night.Checked = false;
                        checkbox_Ding_Day.Checked = false;
                        checkbox_Ding_Night.Checked = false;
                        break;
                    case "甲-夜":
                        checkbox_Jia_Day.Checked = false;
                        checkbox_Jia_Night.Checked = true;
                        checkbox_Yi_Day.Checked = false;
                        checkbox_Yi_Night.Checked = false;
                        checkbox_Bing_Day.Checked = false;
                        checkbox_Bing_Night.Checked = false;
                        checkbox_Ding_Day.Checked = false;
                        checkbox_Ding_Night.Checked = false;
                        break;
                    case "乙-白":
                        checkbox_Jia_Day.Checked = false;
                        checkbox_Jia_Night.Checked = false;
                        checkbox_Yi_Day.Checked = true;
                        checkbox_Yi_Night.Checked = false;
                        checkbox_Bing_Day.Checked = false;
                        checkbox_Bing_Night.Checked = false;
                        checkbox_Ding_Day.Checked = false;
                        checkbox_Ding_Night.Checked = false;
                        break;
                    case "乙-夜":
                        checkbox_Jia_Day.Checked = false;
                        checkbox_Jia_Night.Checked = false;
                        checkbox_Yi_Day.Checked = false;
                        checkbox_Yi_Night.Checked = true;
                        checkbox_Bing_Day.Checked = false;
                        checkbox_Bing_Night.Checked = false;
                        checkbox_Ding_Day.Checked = false;
                        checkbox_Ding_Night.Checked = false;
                        break;
                    case "丙-白":
                        checkbox_Jia_Day.Checked = false;
                        checkbox_Jia_Night.Checked = false;
                        checkbox_Yi_Day.Checked = false;
                        checkbox_Yi_Night.Checked = false;
                        checkbox_Bing_Day.Checked = true;
                        checkbox_Bing_Night.Checked = false;
                        checkbox_Ding_Day.Checked = false;
                        checkbox_Ding_Night.Checked = false;
                        break;
                    case "丙-夜":
                        checkbox_Jia_Day.Checked = false;
                        checkbox_Jia_Night.Checked = false;
                        checkbox_Yi_Day.Checked = false;
                        checkbox_Yi_Night.Checked = false;
                        checkbox_Bing_Day.Checked = false;
                        checkbox_Bing_Night.Checked = true;
                        checkbox_Ding_Day.Checked = false;
                        checkbox_Ding_Night.Checked = false;
                        break;
                    case "丁-白":
                        checkbox_Jia_Day.Checked = false;
                        checkbox_Jia_Night.Checked = false;
                        checkbox_Yi_Day.Checked = false;
                        checkbox_Yi_Night.Checked = false;
                        checkbox_Bing_Day.Checked = false;
                        checkbox_Bing_Night.Checked = false;
                        checkbox_Ding_Day.Checked = true;
                        checkbox_Ding_Night.Checked = false;
                        break;
                    case "丁-夜":
                        checkbox_Jia_Day.Checked = false;
                        checkbox_Jia_Night.Checked = false;
                        checkbox_Yi_Day.Checked = false;
                        checkbox_Yi_Night.Checked = false;
                        checkbox_Bing_Day.Checked = false;
                        checkbox_Bing_Night.Checked = false;
                        checkbox_Ding_Day.Checked = false;
                        checkbox_Ding_Night.Checked = true;
                        break;
                    default:
                        checkbox_Jia_Day.Checked = false;
                        checkbox_Jia_Night.Checked = false;
                        checkbox_Yi_Day.Checked = false;
                        checkbox_Yi_Night.Checked = false;
                        checkbox_Bing_Day.Checked = false;
                        checkbox_Bing_Night.Checked = false;
                        checkbox_Ding_Day.Checked = false;
                        checkbox_Ding_Night.Checked = false;
                        break;
                }
                #endregion
                label_Analyst.Text = target.Summary.Creator.Split("-").LastOrDefault();
                labelCoilNumber.Text = target.Summary.CoilNumber;
                labelTestNo.Text = target.Summary.TestNo;
                #region uploaded
                checkboxUploaded.Checked = target.Summary.IsUploaded;
                #endregion
                #region image
                var upperSurfaceOPRenderImagePath = target.MethodList.FirstOrDefault(t => CircularPositionKind.UPPER_SURFACE_OP.Equals(t.Position))?.RenderImagePath;
                avatarUpperSufaceOP.Image = string.IsNullOrEmpty(upperSurfaceOPRenderImagePath) ? null : Image.FromFile(upperSurfaceOPRenderImagePath);
                var upperSurfaceCERenderImagePath = target.MethodList.FirstOrDefault(t => CircularPositionKind.UPPER_SURFACE_CE.Equals(t.Position))?.RenderImagePath;
                avatarUpperSufaceCE.Image = string.IsNullOrEmpty(upperSurfaceCERenderImagePath) ? null : Image.FromFile(upperSurfaceCERenderImagePath);
                var upperSurfaceDRRenderImagePath = target.MethodList.FirstOrDefault(t => CircularPositionKind.UPPER_SURFACE_DR.Equals(t.Position))?.RenderImagePath;
                avatarUpperSufaceDR.Image = string.IsNullOrEmpty(upperSurfaceDRRenderImagePath) ? null : Image.FromFile(upperSurfaceDRRenderImagePath);
                var lowerSurfaceOPRenderImagePath = target.MethodList.FirstOrDefault(t => CircularPositionKind.LOWER_SURFACE_OP.Equals(t.Position))?.RenderImagePath;
                avatarLowerSufaceOP.Image = string.IsNullOrEmpty(lowerSurfaceOPRenderImagePath) ? null : Image.FromFile(lowerSurfaceOPRenderImagePath);
                var lowerSurfaceCERenderImagePath = target.MethodList.FirstOrDefault(t => CircularPositionKind.LOWER_SURFACE_CE.Equals(t.Position))?.RenderImagePath;
                avatarLowerSufaceCE.Image = string.IsNullOrEmpty(lowerSurfaceCERenderImagePath) ? null : Image.FromFile(lowerSurfaceCERenderImagePath);
                var lowerSurfaceDRRenderImagePath = target.MethodList.FirstOrDefault(t => CircularPositionKind.LOWER_SURFACE_DR.Equals(t.Position))?.RenderImagePath;
                avatarLowerSufaceDR.Image = string.IsNullOrEmpty(lowerSurfaceDRRenderImagePath) ? null : Image.FromFile(lowerSurfaceDRRenderImagePath);
                #endregion
                #region details
                labelUpperSufaceOP.Text = FormatPositionDetail(target.MethodList, CircularPositionKind.UPPER_SURFACE_OP);
                labelUpperSufaceCE.Text = FormatPositionDetail(target.MethodList, CircularPositionKind.UPPER_SURFACE_CE);
                labelUpperSufaceDR.Text = FormatPositionDetail(target.MethodList, CircularPositionKind.UPPER_SURFACE_DR);
                labelLowerSufaceOP.Text = FormatPositionDetail(target.MethodList, CircularPositionKind.LOWER_SURFACE_OP);
                labelLowerSufaceCE.Text = FormatPositionDetail(target.MethodList, CircularPositionKind.LOWER_SURFACE_CE);
                labelLowerSufaceDR.Text = FormatPositionDetail(target.MethodList, CircularPositionKind.LOWER_SURFACE_DR);
                #endregion
            }
            catch (Exception error)
            {
                AntdUI.Notification.error(form, LocalizeHelper.ERROR, error.Message, AntdUI.TAlignFrom.BR, Font);
            }
        }

        private string FormatPositionDetail(List<CircularAreaMethodResult> methodList, CircularPositionKind positionEnum)
        {
            string text = string.Empty;
            var method = methodList.FirstOrDefault(t => positionEnum.Equals(t.Position));
            if (method != null)
            {
                text = $"{LocalizeHelper.CIRCULAR_POSITION_TITLE}{LocalizeHelper.CIRCULAR_POSITION(method.Position)}\n" +
                    $"{LocalizeHelper.CELL_AREA_OF_PIXELS}{method.Pixels}" +
                    $"{LocalizeHelper.AREA_PREDICTION_CONFIDENCE}{method.Confidence.ToPercent()}%\n" +
                    $"{LocalizeHelper.AREA_PREDICTION_TITLE}{method.Area:F2}{LocalizeHelper.SQUARE_MILLIMETER}\n" +
                    $"{LocalizeHelper.CIRCULAR_AREA_DIAMETER}{method.Diameter:F2}{LocalizeHelper.MILLIMETER}\n" +
                    $"{LocalizeHelper.CELL_TITLE_ANALYST}{method.Analyst}\n" +
                    $"{LocalizeHelper.CELL_HEADER_CREATETIME}{method.CreateTime}\n" +
                    $"{LocalizeHelper.CELL_HEADER_LASTREVISER}{method.LastReviser}\n" +
                    $"{LocalizeHelper.CELL_HEADER_LASTMODIFIEDTIME}{method.LastModifiedTime}";
            }
            return text;
        }

        private void BlacknessReport_Disposed(object sender, EventArgs e)
        {
            if (floatButton != null)
            {
                floatButton.Close();
                floatButton = null;
            }
        }

        /// <summary>
        /// TODO：只能固定小的长宽才能在页面完全显示，打印像素又过低。大的长宽，打印又不能显示完全：）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            memoryImage = new Bitmap(this.Width, this.Height);
            this.DrawToBitmap(memoryImage, new Rectangle(0, 0, this.Width, this.Height));
            e.Graphics.DrawImage(memoryImage, e.MarginBounds);
        }
    }
}