using AI_Assistant_Win.Business;
using AI_Assistant_Win.Utils;
using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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

        public BlacknessReport(Form _form, string id)
        {
            form = _form;
            blacknessMethodBLL = new BlacknessMethodBLL();
            InitializeComponent();
            LoadData(id);
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
                            new("setting", "SettingOutlined", true){
                                Tooltip = LocalizeHelper.PRINT_SETTINGS
                            }
                    ], btn =>
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
                                SaveFileDialog saveFileDialog = new SaveFileDialog
                                {
                                    Filter = "PDF�ļ�|*.pdf",
                                    DefaultExt = "pdf",
                                    Title = LocalizeHelper.CHOOSE_THE_LOCATION
                                };
                                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                                {
                                    string pdfPath = saveFileDialog.FileName;
                                    using (var writer = new PdfWriter(pdfPath))
                                    {
                                        using var pdf = new PdfDocument(writer);
                                        var document = new Document(pdf);
                                        using MemoryStream ms = new();
                                        memoryImage.Save(ms, ImageFormat.Png); // ��Bitmap����ΪPNG��ʽ��������֧�ֵĸ�ʽ��
                                        var pdfImage = new iText.Layout.Element.Image(ImageDataFactory.Create(ms.ToArray()));
                                        // ��ͼ����ӵ�PDF�ĵ���
                                        document.Add(pdfImage);
                                        document.Close();
                                    }
                                    AntdUI.Notification.success(form, LocalizeHelper.SUCCESS, LocalizeHelper.FILE_SAVED_LOCATION + pdfPath, AntdUI.TAlignFrom.BR, Font);
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
                    config.Vertical = false;  //  ���ڵ�����
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
                var result = blacknessMethodBLL.GetResultById(id);
                label_Date.Text = result.CreateTime?.ToString("yyyy �� MM �� dd ��");
                #region workGroup
                switch (result.WorkGroup)
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
                label_Analyst.Text = result.Analyst.Split("-").LastOrDefault();
                label_Coil_Number.Text = result.CoilNumber;
                label_Size.Text = result.Size;
                #region OK/NG
                if (result.IsOK)
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
                if (result.IsUploaded)
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
                blacknessReport_RenderImage.Image = System.Drawing.Image.FromFile(result.RenderImagePath);
                #endregion
                label_Surface_OP_Level.Text = result.SurfaceOPLevel;
                label_Surface_OP_Width.Text = $"{result.SurfaceOPWidth:F2}";
                label_Surface_CE_Level.Text = result.SurfaceCELevel;
                label_Surface_CE_Width.Text = $"{result.SurfaceCEWidth:F2}";
                label_Surface_DR_Level.Text = result.SurfaceDRLevel;
                label_Surface_DR_Width.Text = $"{result.SurfaceDRWidth:F2}";
                label_Inside_OP_Level.Text = result.InsideOPLevel;
                label_Inside_OP_Width.Text = $"{result.InsideOPWidth:F2}";
                label_Inside_CE_Level.Text = result.InsideCELevel;
                label_Inside_CE_Width.Text = $"{result.InsideCEWidth:F2}";
                label_Inside_DR_Level.Text = result.InsideDRLevel;
                label_Inside_DR_Width.Text = $"{result.InsideDRWidth:F2}";
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