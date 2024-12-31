using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace AI_Assistant_Win.Utils
{
    public class FileHelper
    {
        public static void SaveImageAsPDF(Bitmap memoryImage, string filePath)
        {
            using (var writer = new PdfWriter(filePath))
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
        }
    }
}
