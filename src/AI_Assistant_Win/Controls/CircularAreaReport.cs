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
                                // �����ļ�����Ի���
                                SaveFileDialog saveFileDialog = new()
                                {
                                    Filter = "PDF�ļ�|*.pdf",
                                    DefaultExt = "pdf",
                                    FileName = $"{target.Summary.TestNo}_Բ�������ⱨ��.pdf",
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
                                        // ����һ������ͬ������Invoke���Ƽ���
                                        // Invoke(callBack); // ע�⣺�������ͬ������
                                        // �ؼ�˵����
                                        // 1.Invoke��ͬ����������������ǰ�߳�ֱ���ص�ִ�����
                                        // 2.ȷ��callBack��û����ѭ����ʱ����������������ῨסUI�߳�
                                        // 3. ��������Ҫ�ϸ�ִ֤��˳��ĳ���
                                        // ���������첽�ȴ�BeginInvoke��ɣ����ӳ�����
                                        // ��BeginInvokeת��Ϊ�ɵȴ���Task
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
                                        await tcs.Task; // �ȴ��ص����
                                                        // �ؼ�˵����
                                                        // 1.ʹ��TaskCompletionSource���첽�ص�ת��Ϊ�ɵȴ���Task
                                                        // 2.�ܲ���ص����׳����쳣��ͨ��await������catch��
                                                        // 3.��������Ҫ����UI��Ӧ�ĳ�ʱ�����
                                                        // 4.ע���߳��л����⣬�ص��е�UI��������Ҫ����Invoke
                                                        // ��ȫ�ͷ���Դ
                                        AntdUI.Message.success(form, LocalizeHelper.REPORT_UPLOAD_SUCCESS);
                                        memoryImage?.Dispose();
                                        if (!this.IsDisposed)
                                        {
                                            this.Dispose();
                                        }
                                        // ���ַ����Աȣ�
                                        // ����       ����һ��Invoke��	��������BeginInvoke + Task��
                                        // �߳�����          ��           ��
                                        // UI��Ӧ��       ���ܿ���       ��������
                                        // �쳣����        ֱ���׳�     ͨ��Task����
                                        // ���븴�Ӷ�        ��         �ϸ���
                                        // ���ó���         �̲���        ������
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
                                // ѡ�񷽰�ʱӦ����ʵ��ҵ�񳡾�����������ص�������ʱ�̣�<200ms��������ʹ�÷���һ������������Ӳ������Ƽ��������Ա�֤UI�����ԡ�
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
                        Vertical = false,  //  ���ڵ�����
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
                labelDate.Text = target.Summary.CreateTime?.ToString("yyyy �� MM �� dd ��");
                #region workGroup
                switch (target.MethodList.FirstOrDefault()?.WorkGroup)
                {
                    case "��-��":
                        checkbox_Jia_Day.Checked = true;
                        checkbox_Jia_Night.Checked = false;
                        checkbox_Yi_Day.Checked = false;
                        checkbox_Yi_Night.Checked = false;
                        checkbox_Bing_Day.Checked = false;
                        checkbox_Bing_Night.Checked = false;
                        checkbox_Ding_Day.Checked = false;
                        checkbox_Ding_Night.Checked = false;
                        break;
                    case "��-ҹ":
                        checkbox_Jia_Day.Checked = false;
                        checkbox_Jia_Night.Checked = true;
                        checkbox_Yi_Day.Checked = false;
                        checkbox_Yi_Night.Checked = false;
                        checkbox_Bing_Day.Checked = false;
                        checkbox_Bing_Night.Checked = false;
                        checkbox_Ding_Day.Checked = false;
                        checkbox_Ding_Night.Checked = false;
                        break;
                    case "��-��":
                        checkbox_Jia_Day.Checked = false;
                        checkbox_Jia_Night.Checked = false;
                        checkbox_Yi_Day.Checked = true;
                        checkbox_Yi_Night.Checked = false;
                        checkbox_Bing_Day.Checked = false;
                        checkbox_Bing_Night.Checked = false;
                        checkbox_Ding_Day.Checked = false;
                        checkbox_Ding_Night.Checked = false;
                        break;
                    case "��-ҹ":
                        checkbox_Jia_Day.Checked = false;
                        checkbox_Jia_Night.Checked = false;
                        checkbox_Yi_Day.Checked = false;
                        checkbox_Yi_Night.Checked = true;
                        checkbox_Bing_Day.Checked = false;
                        checkbox_Bing_Night.Checked = false;
                        checkbox_Ding_Day.Checked = false;
                        checkbox_Ding_Night.Checked = false;
                        break;
                    case "��-��":
                        checkbox_Jia_Day.Checked = false;
                        checkbox_Jia_Night.Checked = false;
                        checkbox_Yi_Day.Checked = false;
                        checkbox_Yi_Night.Checked = false;
                        checkbox_Bing_Day.Checked = true;
                        checkbox_Bing_Night.Checked = false;
                        checkbox_Ding_Day.Checked = false;
                        checkbox_Ding_Night.Checked = false;
                        break;
                    case "��-ҹ":
                        checkbox_Jia_Day.Checked = false;
                        checkbox_Jia_Night.Checked = false;
                        checkbox_Yi_Day.Checked = false;
                        checkbox_Yi_Night.Checked = false;
                        checkbox_Bing_Day.Checked = false;
                        checkbox_Bing_Night.Checked = true;
                        checkbox_Ding_Day.Checked = false;
                        checkbox_Ding_Night.Checked = false;
                        break;
                    case "��-��":
                        checkbox_Jia_Day.Checked = false;
                        checkbox_Jia_Night.Checked = false;
                        checkbox_Yi_Day.Checked = false;
                        checkbox_Yi_Night.Checked = false;
                        checkbox_Bing_Day.Checked = false;
                        checkbox_Bing_Night.Checked = false;
                        checkbox_Ding_Day.Checked = true;
                        checkbox_Ding_Night.Checked = false;
                        break;
                    case "��-ҹ":
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
        /// TODO��ֻ�̶ܹ�С�ĳ��������ҳ����ȫ��ʾ����ӡ�����ֹ��͡���ĳ�����ӡ�ֲ�����ʾ��ȫ����
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