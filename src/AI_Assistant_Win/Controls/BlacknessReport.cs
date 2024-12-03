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
                                Tooltip = "打印预览"
                            },
                            new("print",  "PrinterOutlined", true){
                                Tooltip = "打印",
                                Type= AntdUI.TTypeMini.Primary
                            },
                             new("download",  "DownloadOutlined", true){
                                Tooltip = "下载"
                            },
                            new("setting", "SettingOutlined", true){
                                Tooltip = "页面设置"
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
                                    AntdUI.Notification.warn(form, "提示", "请先预览然后再下载", AntdUI.TAlignFrom.BR, Font);
                                    return;
                                }
                                // 弹出文件保存对话框
                                SaveFileDialog saveFileDialog = new SaveFileDialog
                                {
                                    Filter = "PDF文件|*.pdf",
                                    DefaultExt = "pdf",
                                    Title = "选择保存PDF文件的位置"
                                };
                                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                                {
                                    string pdfPath = saveFileDialog.FileName;
                                    using (var writer = new PdfWriter(pdfPath))
                                    {
                                        using var pdf = new PdfDocument(writer);
                                        var document = new Document(pdf);
                                        using MemoryStream ms = new();
                                        memoryImage.Save(ms, ImageFormat.Png); // 将Bitmap保存为PNG格式（或其他支持的格式）
                                        var pdfImage = new iText.Layout.Element.Image(ImageDataFactory.Create(ms.ToArray()));
                                        // 将图像添加到PDF文档中
                                        document.Add(pdfImage);
                                        document.Close();
                                    }
                                    AntdUI.Notification.success(form, "成功", "PDF文件已成功保存到: " + pdfPath, AntdUI.TAlignFrom.BR, Font);
                                }
                                break;
                            case "setting":
                                pageSetupDialog1.Document = printDocument1;
                                pageSetupDialog1.ShowDialog();
                                break;
                            default:
                                AntdUI.Message.info(form, "点击了：" + btn.Name, Font);
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