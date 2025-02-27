using AI_Assistant_Win.Business;
using AI_Assistant_Win.Models.Middle;
using AI_Assistant_Win.Utils;
using Newtonsoft.Json;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AI_Assistant_Win.Controls
{
    public partial class ScaleAccuracyReport : UserControl
    {
        private Form form;

        Bitmap memoryImage;

        AntdUI.FormFloatButton floatButton = null;

        private ScaleAccuracyTracerHistory target;

        private readonly ScaleAccuracyUploadBLL scaleAccuracyUploadBLL;

        private readonly Action callBack;

        public ScaleAccuracyReport(Form _form, ScaleAccuracyTracerHistory _target, Action _callBack)
        {
            form = _form;
            target = _target;
            callBack = _callBack;
            scaleAccuracyUploadBLL = new ScaleAccuracyUploadBLL();
            InitializeComponent();
            LoadData();
            Disposed += ScaleAccuracyReport_Disposed;
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
                                    FileName = $"{target.Scale.Value}毫米每像素边长_{target.Tracer.MeasuredLength}mm_精度报告.pdf",
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
                                var lastUploaded = scaleAccuracyUploadBLL.GetLastUploaded(target);
                                if (AntdUI.Modal.open(form, LocalizeHelper.CONFIRM, lastUploaded != null ?
                                    LocalizeHelper.WOULD_REUPLOAD_SCALE_ACCURACY_RESULT(target) :
                                    LocalizeHelper.WOULD_UPLOAD_SCALE_ACCURACY_RESULT) == DialogResult.OK)
                                {
                                    try
                                    {
                                        btn.Loading = true;
                                        await scaleAccuracyUploadBLL.Upload(memoryImage, target, lastUploaded);
                                        // refresh
                                        LoadData();
                                        // refresh parent form
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
                                        AntdUI.Message.success(form, LocalizeHelper.ONLY_PDF_REPORT_UPLOAD_SUCCESS);
                                    }
                                    catch (Exception ex)
                                    {
                                        AntdUI.Notification.error(form, LocalizeHelper.ERROR, ex.Message, AntdUI.TAlignFrom.BR, Font);
                                    }
                                    finally
                                    {
                                        btn.Loading = false;
                                        // 安全释放资源
                                        memoryImage?.Dispose();
                                        if (!this.IsDisposed)
                                        {
                                            this.Dispose();
                                        }
                                    }
                                }
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

        private void LoadData()
        {
            try
            {
                labelDate.Text = target.Tracer.CreateTime?.ToString("yyyy 年 MM 月 dd 日");
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
                label_Analyst.Text = target.Tracer.Creator.Split("-").LastOrDefault();
                avatarScaleRenderImage.Image = Image.FromFile(target.Scale.ImagePath);
                var settings = JsonConvert.DeserializeObject<GaugeBlockScaleItem>(target.Scale.Settings);
                textEdgeABPixels.Text = settings.Edges.FirstOrDefault(t => "AB".Equals(t.Edge))?.PixelLength;
                textEdgeBCPixels.Text = settings.Edges.FirstOrDefault(t => "BC".Equals(t.Edge))?.PixelLength;
                textEdgeCDPixels.Text = settings.Edges.FirstOrDefault(t => "CD".Equals(t.Edge))?.PixelLength;
                textEdgeDAPixels.Text = settings.Edges.FirstOrDefault(t => "DA".Equals(t.Edge))?.PixelLength;
                inputEdgeABLength.Text = settings.Edges.FirstOrDefault(t => "AB".Equals(t.Edge))?.RealLength + "mm";
                inputEdgeBCLength.Text = settings.Edges.FirstOrDefault(t => "BC".Equals(t.Edge))?.RealLength + "mm";
                inputEdgeCDLength.Text = settings.Edges.FirstOrDefault(t => "CD".Equals(t.Edge))?.RealLength + "mm";
                inputEdgeDALength.Text = settings.Edges.FirstOrDefault(t => "DA".Equals(t.Edge))?.RealLength + "mm";
                labelTopGrade.Text = $"上表面刻度：{settings.TopGraduations}";
                labelLengthScale.Text = $"{target.Scale.Value:F2}毫米/像素边长";
                labelAreaScale.Text = $"{Math.Pow(target.Scale.Value, 2):F2}平方毫米/像素面积";
                stepsCalculate.Current = 7;
                labelMPE.Text = $"最大允许误差(MPE, Maximum Permissible Error)是仪器或测量系统在特定条件下允许的最大误差值。编号[{target.Scale.Id}]共测量样本数为{target.MPEList.Count}，最大误差值为{target.Tracer.MPE:F2}mm。";
                labelSame.Text = $"{target.Tracer.MeasuredLength}mm量块重复测量{target.MethodList.Count}次：{string.Join(",", target.MethodList.Select(t => $"{t.CalculatedLength:F2}mm"))}。";
                labelAverage.Text = $"{target.Tracer.Average:F2}mm";
                labelStandardDiviation.Text = target.MethodList.Count < 2 ? "至少需要两个值才能计算" : $"σ≈{target.Tracer.StandardDeviation:F3}mm";
                labelStandardError.Text = target.MethodList.Count < 2 ? "至少需要两个值才能计算" : $"{target.Tracer.StandardError:F3}mm";
                labelUncertainty.Text = target.MethodList.Count < 2 ? "至少需要两个值才能计算" : $"{target.Tracer.Uncertainty:F3}mm";
                labelDistribution.Text = target.MethodList.Count < 2 ? "至少需要两个值才能计算" : $"μ±1σ 内: {target.Tracer.Pct1Sigma:P2} (理论68.27%)\n" +
                    $"μ±2σ 内: {target.Tracer.Pct2Sigma:P2} (理论95.45%)\n" +
                    $"μ±3σ 内: {target.Tracer.Pct3Sigma:P2} (理论99.73%)";
                labelConfidence.Text = target.MethodList.Count < 2 ? "至少需要两个值才能计算" : $"{target.Tracer.DisplayName}";

            }
            catch (Exception error)
            {
                AntdUI.Notification.error(form, LocalizeHelper.ERROR, error.Message, AntdUI.TAlignFrom.BR, Font);
            }
        }

        private void ScaleAccuracyReport_Disposed(object sender, EventArgs e)
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
            // Deepseek
            avatarErrorFormula.DrawToBitmap(memoryImage, new Rectangle(avatarErrorFormula.Left, avatarErrorFormula.Top, avatarErrorFormula.Width, avatarErrorFormula.Height));
            avatarMPE.DrawToBitmap(memoryImage, new Rectangle(avatarMPE.Left, avatarMPE.Top, avatarMPE.Width, avatarMPE.Height));
            avatarNumbers.DrawToBitmap(memoryImage, new Rectangle(avatarNumbers.Left, avatarNumbers.Top, avatarNumbers.Width, avatarNumbers.Height));
            avatarAverage.DrawToBitmap(memoryImage, new Rectangle(avatarAverage.Left, avatarAverage.Top, avatarAverage.Width, avatarAverage.Height));
            avatarStandardDiviation.DrawToBitmap(memoryImage, new Rectangle(avatarStandardDiviation.Left, avatarStandardDiviation.Top, avatarStandardDiviation.Width, avatarStandardDiviation.Height));
            avatarStandardError.DrawToBitmap(memoryImage, new Rectangle(avatarStandardError.Left, avatarStandardError.Top, avatarStandardError.Width, avatarStandardError.Height));
            avatarUncertaincy.DrawToBitmap(memoryImage, new Rectangle(avatarUncertaincy.Left, avatarUncertaincy.Top, avatarUncertaincy.Width, avatarUncertaincy.Height));
            avatarDistribution.DrawToBitmap(memoryImage, new Rectangle(avatarDistribution.Left, avatarDistribution.Top, avatarDistribution.Width, avatarDistribution.Height));
            avatarWholeFormula.DrawToBitmap(memoryImage, new Rectangle(avatarWholeFormula.Left, avatarWholeFormula.Top, avatarWholeFormula.Width, avatarWholeFormula.Height));
            e.Graphics.DrawImage(memoryImage, e.MarginBounds);
        }
    }
}