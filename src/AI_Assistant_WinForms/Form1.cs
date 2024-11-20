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

                // GPU��ʾ����OK3�ģ����Ǳ���û�У�˵��ģ�;���û����ȷԤ��
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
        /// ��YOLOv8�����κ�YOLO�汾���У�ģ��ͨ������������ı߽��bounding boxes����ÿ���߽�򶼰����йؼ�⵽�Ķ����λ�á����ŶȺ������Ϣ��
        /// ����YOLOv8�����8400���߽��ɸѡ��ǡ�����Ǹ�������Щ��ͨ���漰���¼������裺
        /// ���Ŷ���ֵ���ˣ�
        /// ���ȣ����������һ�����Ŷ���ֵ�����˵���Щ���ŶȽϵ͵ı߽�����Ŷȱ�ʾģ�Ͷ����⵽�Ķ�������ĳ̶ȡ�
        /// ͨ���������ֵ�Ǹ�����ľ���Ӧ�ó��������������õġ����磬����Խ����Ŷ���ֵ����Ϊ0.5������ζ��ֻ����Щ���Ŷȴ��ڻ����0.5�ı߽��Żᱻ������
        /// �Ǽ���ֵ���ƣ�NMS����
        /// ��ʹ�������Ŷ���ֵ���˺���Ȼ�����ж���߽���ص�����⵽ͬһ�����󡣷Ǽ���ֵ���ƣ�Non-Maximum Suppression, NMS����һ�����ڽ���������ļ�����
        /// �����ݱ߽������ŶȺ��ص��̶ȣ�ͨ��ʹ��IoU���������ȣ���ѡ����ѵı߽�򣬲����ƣ���ɾ������Щ����ѱ߽���ص�����������߽��
        /// �����ˣ������Ҫ����
        /// ������Ӧ�ó���ֻ�����ض����͵Ķ������磬ֻ����˶�����⳵��������������NMS֮���һ�����ݱ߽��������Ϣ���й��ˡ�
        /// ѡ����ǡ���ı߽��
        /// �ھ����������������ܻ�õ�һ������ʣ��ı߽�������ֻ��Ҫһ����ǡ���ı߽������Ը������Ŷȡ��߽��Ĵ�С��λ�û��������ָ����ѡ��һ����ѵı߽��
        /// ���磬�����ѡ�����Ŷ���ߵı߽�򣬻���ѡ�񸲸�Ŀ������������ı߽��
        /// ������ѡ����
        /// ��ĳЩ����£�����ܻ���Ҫ��ѡ���ı߽����н�һ���ĺ������磬�Ա߽�����΢���Ը�׼ȷ�ض�λ���󣬻��߶Ա߽���ڵ�ͼ�����ݽ��ж���ķ�������
        /// ��ע�⣬YOLOv8ģ�ͱ�����ܻ���������а���һЩ����ɸѡ�߽��Ļ��ƣ����磬���õ�NMS����
        /// ��ˣ���ʵ��Ӧ���У��������Ҫ��ϸ�Ķ�YOLOv8���ĵ���Դ���룬���˽�ģ������ľ����ʽ�Ϳ��õ�ɸѡѡ�
        /// ���⣬������ľ��������Ӧ�ó������������Ҫ�������Ŷ���ֵ��NMS������������������Ի����ѽ����
        /// </summary>
        /// <param name="modelOutput"></param>
        /// <param name="labels"></param>
        /// <param name="probabilityThreshold"></param>
        /// <returns></returns>
        public static List<BoundingBox> ParseOutputs(float[] modelOutput, string[] labels, float probabilityThreshold = .25f)
        {
            var boxes = new List<BoundingBox>();

            for (int row = 0; row < rowCount; row++)  // 8400����
            {
                // 9�����ԣ��ֱ��� x��y��w��h��5���������Ŷ�
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

                // ����ͼ��Χ�Ĳ�Ҫ
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

                // �ų����ߺ͹��͵�
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
