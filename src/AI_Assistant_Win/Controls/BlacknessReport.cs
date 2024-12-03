using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace AI_Assistant_Win.Controls
{
    public partial class BlacknessReport : UserControl
    {
        private Form form;

        Bitmap memoryImage;

        AntdUI.FormFloatButton floatButton = null;

        public BlacknessReport(Form _form)
        {
            form = _form;
            InitializeComponent();
            Disposed += BlacknessReport_Disposed;
            AntdUI.ITask.Run(() =>
            {
                System.Threading.Thread.Sleep(1000);
                if (floatButton == null)
                {
                    var config = new AntdUI.FloatButton.Config(form, [
                            new("preview", "SearchOutlined", true){
                                Tooltip = "��ӡԤ��"
                            },
                            new("print",  "PrinterOutlined", true){
                                Tooltip = "��ӡ",
                                Type= AntdUI.TTypeMini.Primary
                            },
                             new("download",  "DownloadOutlined", true){
                                Tooltip = "����"
                            },
                            new("setting", "SettingOutlined", true){
                                Tooltip = "ҳ������"
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
                                    AntdUI.Notification.warn(form, "��ʾ", "����Ԥ��Ȼ��������", AntdUI.TAlignFrom.BR, Font);
                                    return;
                                }
                                // �����ļ�����Ի���
                                SaveFileDialog saveFileDialog = new SaveFileDialog
                                {
                                    Filter = "PDF�ļ�|*.pdf",
                                    DefaultExt = "pdf",
                                    Title = "ѡ�񱣴�PDF�ļ���λ��"
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
                                    AntdUI.Notification.success(form, "�ɹ�", "PDF�ļ��ѳɹ����浽: " + pdfPath, AntdUI.TAlignFrom.BR, Font);
                                }
                                break;
                            case "setting":
                                pageSetupDialog1.Document = printDocument1;
                                pageSetupDialog1.ShowDialog();
                                break;
                            default:
                                AntdUI.Message.info(form, "����ˣ�" + btn.Name, Font);
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

        private void BlacknessReport_Disposed(object sender, EventArgs e)
        {
            if (floatButton != null)
            {
                floatButton.Close();
                floatButton = null;
            }
        }

        private void PrintDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            memoryImage = new Bitmap(this.Width, this.Height);
            this.DrawToBitmap(memoryImage, new Rectangle(0, 0, this.Width, this.Height));
            e.Graphics.DrawImage(memoryImage, e.MarginBounds);
        }
    }
}