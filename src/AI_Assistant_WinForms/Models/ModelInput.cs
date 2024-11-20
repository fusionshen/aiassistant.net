using Microsoft.ML.Data;
using Microsoft.ML.Transforms.Image;

namespace AI_Assistant_WinForms.Models
{
    public class ModelInput
    {
        [ImageType(ImageSettings.imageHeight, ImageSettings.imageWidth)]
        public MLImage Image { get; set; }
    }
}
