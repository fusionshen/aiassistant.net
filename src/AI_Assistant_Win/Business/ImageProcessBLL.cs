using SixLabors.ImageSharp;
using System;
using System.ComponentModel;
using System.IO;

namespace AI_Assistant_Win.Business
{
    public class ImageProcessBLL(string application) : INotifyPropertyChanged
    {
        private string originImagePath = string.Empty;

        public string OriginImagePath
        {
            get { return originImagePath; }
            set
            {
                if (originImagePath != value)
                {
                    originImagePath = value;
                    OnPropertyChanged(nameof(OriginImagePath));
                }
            }
        }

        private string renderImagePath = string.Empty;
        public string RenderImagePath
        {
            get { return renderImagePath; }
            set
            {
                if (renderImagePath != value)
                {
                    renderImagePath = value;
                    OnPropertyChanged(nameof(RenderImagePath));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void SaveOriginImage(string filePath)
        {
            string directoryPath = $".\\Images\\{application}\\Origin";
            Directory.CreateDirectory(directoryPath);
            string fullPath = Path.Combine(directoryPath, DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".jpg");
            using var image = Image.Load(filePath);
            image.Save(fullPath);
            OriginImagePath = fullPath;
        }

        public void SaveRenderImage(Image image)
        {
            string directoryPath = $".\\Images\\{application}\\Render";
            Directory.CreateDirectory(directoryPath);
            string fullPath = Path.Combine(directoryPath, DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".jpg");
            image.Save(fullPath);
            RenderImagePath = fullPath;
        }
    }
}
