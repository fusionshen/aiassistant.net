using SixLabors.ImageSharp;
using System;
using System.IO;

namespace AI_Assistant_Win.Business
{
    public class ImageProcessBLL
    {
        public string SaveOriginImageAndReturnPath(string filePath)
        {
            string directoryPath = @".\Images\Origin";
            Directory.CreateDirectory(directoryPath);
            string fullPath = Path.Combine(directoryPath, DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".jpg");
            using var image = SixLabors.ImageSharp.Image.Load(filePath);
            image.Save(fullPath);
            return fullPath;
        }

        public string SaveRenderImageAndReturnPath(SixLabors.ImageSharp.Image image)
        {
            string directoryPath = @".\Images\Render";
            Directory.CreateDirectory(directoryPath);
            string fullPath = Path.Combine(directoryPath, DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".jpg");
            image.Save(fullPath);
            return fullPath;
        }
    }
}
