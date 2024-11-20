using AI_Assistant_WinForms.Models;
using Microsoft.ML;
using Microsoft.ML.Data;
using System.Windows.Forms;

namespace AI_Assistant_WinForms
{
    public partial class Form1 : Form
    {
        public const int rowCount = 8400, columnCount = 9;

        public const int featuresPerBox = 5;

        private readonly PredictionEngine<ModelInput, ModelPredictions> _predictionEngine;

        public Form1()
        {
            InitializeComponent();

            picPrediction.Visible = false;
            btnNewPrediction.Visible = false;

            var context = new MLContext();

            var emptyData = new List<ModelInput>();

            // https://github.com/dotnet/machinelearning/issues/6764
            var data = context.Data.LoadFromEnumerable(emptyData);

            var pipeline = context.Transforms.ResizeImages(outputColumnName: "images", imageWidth: ImageSettings.imageWidth, imageHeight: ImageSettings.imageHeight, inputColumnName: nameof(ModelInput.Image))
                            .Append(context.Transforms.ExtractPixels(outputColumnName: "images"))
                            .Append(context.Transforms.ApplyOnnxModel(modelFile: "./MLModel/model.onnx",
                            outputColumnName: "output0",
                            inputColumnName: "images",
                            gpuDeviceId: null));

            var model = pipeline.Fit(data);

            _predictionEngine = context.Model.CreatePredictionEngine<ModelInput, ModelPredictions>(model);
        }

        private void BtnSelectImage_Click(object sender, EventArgs e)
        {
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                var image = MLImage.CreateFromFile(fileDialog.FileName);

                var bitMapImage = (Bitmap)Image.FromFile(fileDialog.FileName);

                // GPU演示是有OK3的，但是本地没有，说明模型就是没有正确预测
                var prediction = _predictionEngine.Predict(new ModelInput { Image = image });

                var labels = File.ReadAllLines("./MLModel/labels.txt");

                var boundingBoxes = ParseOutputs(prediction.Blackness, labels);

                boundingBoxes = NonMaximumSuppression.Apply(boundingBoxes, 0.2f);

                var originalWidth = image.Width;
                var originalHeight = image.Height;

                if (boundingBoxes.Count < 1)
                {
                    MessageBox.Show("No prediction for image");
                    return;
                }

                foreach (var boundingBox in boundingBoxes)
                {
                    float x = Math.Max(boundingBox.Dimensions.X, 0);
                    float y = Math.Max(boundingBox.Dimensions.Y, 0);
                    float width = Math.Min(originalWidth - x, boundingBox.Dimensions.Width);
                    float height = Math.Min(originalHeight - y, boundingBox.Dimensions.Height);

                    // fit to current image size
                    x = originalWidth * x / ImageSettings.imageWidth;
                    y = originalHeight * y / ImageSettings.imageHeight;
                    width = originalWidth * width / ImageSettings.imageWidth;
                    height = originalHeight * height / ImageSettings.imageHeight;

                    using (var graphics = Graphics.FromImage(bitMapImage))
                    {
                        graphics.DrawRectangle(new Pen(Color.Red, 3), x, y, width, height);
                        graphics.DrawString(boundingBox.Description, new Font(FontFamily.Families[0], 30f), Brushes.Red, x + 5, y + 5);
                    }
                }
                bitMapImage.Save("./test_draw.jpg");
                picPrediction.Image = bitMapImage;
                picPrediction.SizeMode = PictureBoxSizeMode.AutoSize;
                picPrediction.Visible = true;
                btnSelectImage.Visible = false;
                btnNewPrediction.Visible = true;
            }
        }

        /// <summary>
        /// 在YOLOv8（或任何YOLO版本）中，模型通常会输出大量的边界框（bounding boxes），每个边界框都包含有关检测到的对象的位置、置信度和类别信息。
        /// 对于YOLOv8输出的8400个边界框，筛选最恰当的那个（或那些）通常涉及以下几个步骤：
        /// 置信度阈值过滤：
        /// 首先，你可以设置一个置信度阈值来过滤掉那些置信度较低的边界框。置信度表示模型对其检测到的对象的信心程度。
        /// 通常，这个阈值是根据你的具体应用场景和需求来设置的。例如，你可以将置信度阈值设置为0.5，这意味着只有那些置信度大于或等于0.5的边界框才会被保留。
        /// 非极大值抑制（NMS）：
        /// 即使经过置信度阈值过滤后，仍然可能有多个边界框重叠并检测到同一个对象。非极大值抑制（Non-Maximum Suppression, NMS）是一种用于解决这个问题的技术。
        /// 它根据边界框的置信度和重叠程度（通常使用IoU，即交并比）来选择最佳的边界框，并抑制（即删除）那些与最佳边界框重叠过多的其他边界框。
        /// 类别过滤（如果需要）：
        /// 如果你的应用场景只关心特定类型的对象（例如，只检测人而不检测车辆或动物），你可以在NMS之后进一步根据边界框的类别信息进行过滤。
        /// 选择最恰当的边界框：
        /// 在经过上述步骤后，你可能会得到一个或多个剩余的边界框。如果你只需要一个最恰当的边界框，你可以根据置信度、边界框的大小、位置或其他相关指标来选择一个最佳的边界框。
        /// 例如，你可以选择置信度最高的边界框，或者选择覆盖目标对象最完整的边界框。
        /// 后处理（可选）：
        /// 在某些情况下，你可能还需要对选定的边界框进行进一步的后处理，例如，对边界框进行微调以更准确地定位对象，或者对边界框内的图像内容进行额外的分析或处理。
        /// 请注意，YOLOv8模型本身可能会在其输出中包含一些用于筛选边界框的机制（例如，内置的NMS）。
        /// 因此，在实际应用中，你可能需要仔细阅读YOLOv8的文档或源代码，以了解模型输出的具体格式和可用的筛选选项。
        /// 此外，根据你的具体需求和应用场景，你可能需要调整置信度阈值、NMS参数和其他相关设置以获得最佳结果。
        /// </summary>
        /// <param name="modelOutput"></param>
        /// <param name="labels"></param>
        /// <param name="probabilityThreshold"></param>
        /// <returns></returns>
        public static List<BoundingBox> ParseOutputs(float[] modelOutput, string[] labels, float probabilityThreshold = .25f)
        {
            var boxes = new List<BoundingBox>();

            for (int row = 0; row < rowCount; row++)  // 8400个框
            {
                // 9个属性，分别是 x、y、w、h、5个类别的置信度
                var mappedBoundingBox = new BoundingBoxDimensions
                {
                    X = modelOutput[0 * 8400 + row],
                    Y = modelOutput[1 * 8400 + row],
                    Width = modelOutput[2 * 8400 + row],
                    Height = modelOutput[3 * 8400 + row]
                };

                // The x,y coordinates from the (mapped) bounding box prediction represent the center
                // of the bounding box. We adjust them here to represent the top left corner.
                mappedBoundingBox.X -= mappedBoundingBox.Width / 2;
                mappedBoundingBox.Y -= mappedBoundingBox.Height / 2;

                // 超过图像范围的不要
                if (mappedBoundingBox.X < 0 ||
                    mappedBoundingBox.X > ImageSettings.imageWidth - mappedBoundingBox.Width ||
                    mappedBoundingBox.Y < 0 ||
                    mappedBoundingBox.Y > ImageSettings.imageHeight - mappedBoundingBox.Height)
                {
                    continue;
                }

                var classProbabilities = new List<float> {
                        modelOutput[4 * 8400 + row],     // NG1
                        modelOutput[5 * 8400 + row],     // NG2
                        modelOutput[6 * 8400 + row],     // OK3
                        modelOutput[7 * 8400 + row],     // OK4
                        modelOutput[8 * 8400 + row]      // OK5
                    };

                var topProbability = classProbabilities.Max();

                // 排除过高和过低的
                if (topProbability > 0.90f || topProbability < probabilityThreshold)
                {
                    continue;
                }

                var topIndex = classProbabilities.IndexOf(topProbability);

                boxes.Add(new BoundingBox
                {
                    Dimensions = mappedBoundingBox,
                    Confidence = topProbability,
                    Label = labels[topIndex]
                });
            }
            return boxes;
        }


        private static float[] Softmax(float[] classProbabilities)
        {
            var max = classProbabilities.Max();
            var exp = classProbabilities.Select(v => Math.Exp(v - max));
            var sum = exp.Sum();
            return exp.Select(v => (float)v / (float)sum).ToArray();
        }

        private void BtnNewPrediction_Click(object sender, EventArgs e)
        {
            btnNewPrediction.Visible = false;
            picPrediction.Visible = false;
            btnSelectImage.Visible = true;
        }

        private static int GetOffset(int row, int column, int channel)
        {
            const int channelStride = rowCount * columnCount;
            return (channel * channelStride) + (column * columnCount) + row;
        }
    }
}
