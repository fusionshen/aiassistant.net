using AI_Assistant_Win.Business;
using AI_Assistant_Win.Models;
using AI_Assistant_Win.Utils;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AI_Assistant_Win.Controls
{
    public partial class BlacknessReport : UserControl
    {
        private Form form;

        private readonly BlacknessMethodBLL blacknessMethodBLL;

        Bitmap memoryImage;

        AntdUI.FormFloatButton floatButton = null;

        private BlacknessMethodResult target;

        private readonly BlacknessUploadBLL uploadBlacknessBLL;

        private readonly string methodId;

        private readonly Action callBack;

        public BlacknessReport(Form _form, string _methodId, Action _callBack)
        {
            form = _form;
            methodId = _methodId;
            callBack = _callBack;
            blacknessMethodBLL = new BlacknessMethodBLL();
            uploadBlacknessBLL = new BlacknessUploadBLL();
            InitializeComponent();
            LoadData(methodId);
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
                                Type= AntdUI.TTypeMini.Error
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
                                    FileName = $"{target.CoilNumber}_黑度检测报告.pdf",
                                    Title = LocalizeHelper.CHOOSE_THE_LOCATION
                                };
                                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                                {
                                    try
                                    {
                                        string pdfPath = saveFileDialog.FileName;
                                        FileHelper.SaveImageAsPDF(memoryImage, pdfPath);
                                        AntdUI.Notification.success(form, LocalizeHelper.SUCCESS, LocalizeHelper.FILE_SAVED_LOCATION + pdfPath,
                                            AntdUI.TAlignFrom.BR, Font);
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
                                var lastUploaded = uploadBlacknessBLL.GetLastUploaded(target);
                                if (AntdUI.Modal.open(form, LocalizeHelper.CONFIRM, lastUploaded != null ?
                                    LocalizeHelper.WOULD_REUPLOAD_BLACKNESS_RESULT(target.CoilNumber) :
                                    LocalizeHelper.WOULD_UPLOAD_BLACKNESS_RESULT) == DialogResult.OK)
                                {
                                    try
                                    {
                                        // TODO: btn loading
                                        await uploadBlacknessBLL.Upload(memoryImage, target, lastUploaded);
                                        // refresh
                                        LoadData(methodId);
                                        // refresh parent form
                                        callBack();
                                        AntdUI.Notification.success(form, LocalizeHelper.SUCCESS, LocalizeHelper.REPORT_UPLOAD_SUCCESS,
                                         AntdUI.TAlignFrom.BR, Font);
                                    }
                                    catch (Exception ex)
                                    {
                                        AntdUI.Notification.error(form, LocalizeHelper.ERROR, ex.Message, AntdUI.TAlignFrom.BR, Font);
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
                    });
                    config.Vertical = false;  //  不遮挡内容
                    config.TopMost = true;
                    floatButton = AntdUI.FloatButton.open(config);
                }
                else
                {
                    floatButton.Close();
                    floatButton = null;
                }
            });

        }

        private void LoadData(string id)
        {
            try
            {
                target = blacknessMethodBLL.GetResultById(id);
                label_Date.Text = target.CreateTime?.ToString("yyyy 年 MM 月 dd 日");
                #region workGroup
                switch (target.WorkGroup)
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
                label_Analyst.Text = target.Analyst.Split("-").LastOrDefault();
                label_Coil_Number.Text = target.CoilNumber;
                label_Size.Text = target.Size;
                #region OK/NG
                if (target.IsOK)
                {
                    checkbox_OK.Checked = true;
                    checkbox_NG.Checked = false;
                }
                else
                {
                    checkbox_OK.Checked = false;
                    checkbox_NG.Checked = true;
                }
                #endregion
                #region uploaded
                if (target.IsUploaded)
                {
                    checkbox_Uploaded.Checked = true;
                    checkbox_Not_Uploaded.Checked = false;
                }
                else
                {
                    checkbox_Uploaded.Checked = false;
                    checkbox_Not_Uploaded.Checked = true;
                }
                #endregion
                #region image
                blacknessReport_RenderImage.Image = System.Drawing.Image.FromFile(target.RenderImagePath);
                #endregion
                label_Surface_OP_Level.Text = target.SurfaceOPLevel;
                label_Surface_OP_Width.Text = $"{target.SurfaceOPWidth:F2}";
                label_Surface_CE_Level.Text = target.SurfaceCELevel;
                label_Surface_CE_Width.Text = $"{target.SurfaceCEWidth:F2}";
                label_Surface_DR_Level.Text = target.SurfaceDRLevel;
                label_Surface_DR_Width.Text = $"{target.SurfaceDRWidth:F2}";
                label_Inside_OP_Level.Text = target.InsideOPLevel;
                label_Inside_OP_Width.Text = $"{target.InsideOPWidth:F2}";
                label_Inside_CE_Level.Text = target.InsideCELevel;
                label_Inside_CE_Width.Text = $"{target.InsideCEWidth:F2}";
                label_Inside_DR_Level.Text = target.InsideDRLevel;
                label_Inside_DR_Width.Text = $"{target.InsideDRWidth:F2}";
            }
            catch (Exception error)
            {
                AntdUI.Notification.error(form, LocalizeHelper.ERROR, error.Message, AntdUI.TAlignFrom.BR, Font);
            }
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