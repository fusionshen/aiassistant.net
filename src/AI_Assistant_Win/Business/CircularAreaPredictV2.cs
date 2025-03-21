using AI_Assistant_Win.Models;
using AI_Assistant_Win.Models.Middle;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;

namespace AI_Assistant_Win.Business
{
    public class CircularAreaPredictV2 : INotifyPropertyChanged
    {
        private readonly ImageProcessBLL _imageProcessBLL;
        private InferenceSession _session;
        private readonly int _inputSize = 640;  // 根据实际模型调整

        public SimpleSegmentation Prediction { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public CircularAreaPredictV2(ImageProcessBLL imageProcessBLL)
        {
            _imageProcessBLL = imageProcessBLL;

            // 初始化ONNX Runtime
            var options = new SessionOptions
            {
                GraphOptimizationLevel = GraphOptimizationLevel.ORT_ENABLE_ALL,
                ExecutionMode = ExecutionMode.ORT_SEQUENTIAL
            };

            if (File.Exists("./Resources/Circular/v2.onnx"))
            {
                _session = new InferenceSession("./Resources/Circular/v2.onnx", options);
            }
        }

        public void Predict(CalculateScale currentScale)
        {
            if (_session == null) return;

            using var image = new Image<Bgr, byte>(_imageProcessBLL.OriginImagePath);

            // 1. 预处理
            var (inputTensor, scaleFactor) = PreprocessImage(image);

            var results = _session.Run(new List<NamedOnnxValue>
            // 2. 推理
            {
                NamedOnnxValue.CreateFromTensor("images", inputTensor)
            });

            // 3. 后处理
            var mask = ProcessOutput(results, image.Size, scaleFactor);

            // 4. 边缘优化
            var optimizedMask = RefineMaskEdges(mask);

            // 5. 可视化
            var rendered = DrawResults(image, optimizedMask, currentScale);

            //_imageProcessBLL.SaveRenderImage(rendered);
        }

        private (Tensor<float>, float) PreprocessImage(Image<Bgr, byte> image)
        {
            // 缩放到模型输入尺寸
            var scaleFactor = Math.Min((float)_inputSize / image.Width, (float)_inputSize / image.Height);
            var scaledWidth = (int)(image.Width * scaleFactor);
            var scaledHeight = (int)(image.Height * scaleFactor);

            using var resized = image.Resize(scaledWidth, scaledHeight, Inter.Linear);

            // 填充到正方形
            var padded = new Image<Bgr, byte>(_inputSize, _inputSize);
            resized.CopyTo(padded.GetSubRect(new Rectangle(0, 0, scaledWidth, scaledHeight)));

            var tensorData = new float[3 * _inputSize * _inputSize];
            var index = 0;
            for (int c = 0; c < 3; c++) // 通道
            {
                for (int y = 0; y < _inputSize; y++)
                {
                    for (int x = 0; x < _inputSize; x++)
                    {
                        tensorData[index++] = padded.Data[y, x, c] / 255.0f; // 归一化到 [0,1]
                    }
                }
            }
            var inputTensor = new DenseTensor<float>(tensorData, new[] { 1, 3, _inputSize, _inputSize });

            return (inputTensor, scaleFactor);
        }

        private Mat ProcessOutput(IReadOnlyCollection<DisposableNamedOnnxValue> results, Size originalSize, float scaleFactor)
        {
            // 1. 获取 `output1`
            var maskTensor = results.First(x => x.Name == "output1").AsTensor<float>();

            // 2. 确保输出维度是 (1,32,160,160)
            if (maskTensor.Dimensions.Length != 4 || maskTensor.Dimensions[1] != 32)
            {
                throw new InvalidOperationException($"ONNX 输出尺寸不匹配: 预期 (1,32,160,160)，实际 {string.Join("x", maskTensor.Dimensions.ToString())}");
            }

            int channels = maskTensor.Dimensions[1]; // 32个类别
            int height = maskTensor.Dimensions[2];   // 160
            int width = maskTensor.Dimensions[3];    // 160

            float[] maskData = maskTensor.ToArray();

            // 3. 选择某个类别通道（比如取第一个通道）
            int selectedChannel = 0; // 你可以调整这个数值来选择不同类别的掩码
            float[] singleChannelMask = new float[width * height];
            Array.Copy(maskData, selectedChannel * width * height, singleChannelMask, 0, width * height);

            // 4. 创建 `Mat` 并拷贝数据
            Mat mask = new Mat(height, width, DepthType.Cv32F, 1);
            System.Runtime.InteropServices.Marshal.Copy(singleChannelMask, 0, mask.DataPointer, singleChannelMask.Length);

            // 5. 恢复到原始尺寸
            CvInvoke.Resize(mask, mask, originalSize, 0, 0, Inter.Cubic);

            return mask;
        }

        private Mat RefineMaskEdges(Mat mask)
        {
            // 1. 二值化
            Mat binary = new Mat();
            CvInvoke.Threshold(mask, binary, 0.9, 255, ThresholdType.Binary);
            binary.ConvertTo(binary, DepthType.Cv8U);  // 确保为 CV_8U 格式

            // 2. 形态学优化
            Mat kernel = CvInvoke.GetStructuringElement(ElementShape.Ellipse, new Size(3, 3), new Point(-1, -1));
            CvInvoke.MorphologyEx(binary, binary, MorphOp.Close, kernel, new Point(-1, -1), 2, BorderType.Default, new MCvScalar(0));

            // 3. 轮廓优化
            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            Mat hierarchy = new Mat();  // 确保 OpenCV 4.x 兼容
            CvInvoke.FindContours(binary, contours, hierarchy, RetrType.External, ChainApproxMethod.ChainApproxNone);

            Mat optimized = new Mat(binary.Size, DepthType.Cv8U, 1);
            optimized.SetTo(new MCvScalar(0));

            for (int i = 0; i < contours.Size; i++)
            {
                var contour = contours[i];
                var epsilon = 0.005 * CvInvoke.ArcLength(contour, true);
                var approx = new VectorOfPoint();
                CvInvoke.ApproxPolyDP(contour, approx, epsilon, true);
                CvInvoke.DrawContours(optimized, new VectorOfVectorOfPoint(approx), -1, new MCvScalar(255), -1);
            }

            return optimized;
        }

        private Image<Bgr, byte> DrawResults(Image<Bgr, byte> image, Mat mask, CalculateScale currentScale)
        {
            // 创建可视化图像
            var result = image.Copy();

            // 应用半透明掩膜
            var colorMask = new Image<Bgra, byte>(image.Size);
            CvInvoke.CvtColor(mask, colorMask, ColorConversion.Gray2Bgra);
            colorMask.SetValue(new Bgra(0, 255, 0, 50));

            result = result.AddWeighted(colorMask.Convert<Bgr, byte>(), 0.5, 0.5, 0);

            // 绘制边界框
            var contours = new VectorOfVectorOfPoint();
            CvInvoke.FindContours(mask, contours, null, RetrType.External, ChainApproxMethod.ChainApproxSimple);

            foreach (var contour in contours.ToArrayOfArray())
            {
                var rect = CvInvoke.BoundingRectangle(contour);
                result.Draw(rect, new Bgr(Color.Red), 2);
            }

            return result;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
