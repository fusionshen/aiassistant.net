using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.OCR;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Yolov8.Net;


namespace AI_Assistant_Win.Utils
{
    // TODO: Simple Hanzi works not well
    public class OcrBasedMatcher : IDisposable
    {
        // 添加关键词配置
        private static readonly string[] TargetKeywords = ["表面", "里面", "OP", "CE", "DR"];
        private bool _disposed;
        // 添加图像处理缓存
        private Image<Gray, byte> _cachedGray;
        private Image<Gray, byte> _cachedProcessed;
        public OcrBasedMatcher()
        {
        }

        ///// <summary>
        ///// 匹配物体到最近区域
        ///// </summary>
        public Prediction[] MatchRegions(
            Image<Bgr, byte> image, Prediction[] yoloPredictions)
        {
            var markers = DetectRegionMarkers(image);
            // 初始化6个位置的结果数组
            Prediction[] result = new Prediction[6];
            // 遍历所有YOLO预测结果
            foreach (var prediction in yoloPredictions)
            {
                var nearest = markers
                    .Select((m, i) => new
                    {
                        Index = i,
                        Marker = m,
                        Distance = CalculateDistance(m.Center, prediction.Rectangle)
                    })
                    .OrderBy(x => x.Distance)
                    .FirstOrDefault();

                // 有效索引处理
                if (nearest.Index >= 0 && nearest.Index < 6)
                {
                    result[nearest.Index] = prediction;
                }
            }
            return result;
        }

        /// <summary>
        /// 识别图像中的区域标记
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public List<RegionMarker> DetectRegionMarkers(Image<Bgr, byte> image)
        {
            // 复用预处理结果
            if (_cachedGray == null || _cachedProcessed == null)
            {
                _cachedGray = image.Convert<Gray, byte>();
                _cachedProcessed = new Image<Gray, byte>(_cachedGray.Size);

                // 使用高斯模糊替代中值模糊
                CvInvoke.GaussianBlur(_cachedGray, _cachedProcessed, new Size(3, 3), 1.5);

                // 调整自适应阈值参数
                CvInvoke.AdaptiveThreshold(
                    _cachedProcessed,
                    _cachedProcessed,
                    255,
                    AdaptiveThresholdType.GaussianC,
                    ThresholdType.Binary,
                    25,  // 增大块尺寸
                    7    // 调整常数
                );

                // 添加膨胀操作增强文字连接
                using (var kernel = CvInvoke.GetStructuringElement(
                    ElementShape.Rectangle,
                    new Size(2, 2),
                    new Point(-1, -1)))
                {
                    CvInvoke.Dilate(_cachedProcessed, _cachedProcessed, kernel, new Point(-1, -1), 1, BorderType.Default, default);
                }

                CvInvoke.Imwrite("3-RegionMarker.png", _cachedProcessed);
            }

            // 优化版轮廓检测
            var regions = new List<Rectangle>();
            using (var contours = new VectorOfVectorOfPoint())
            {
                CvInvoke.FindContours(
                    _cachedProcessed,
                    contours,
                    null,
                    RetrType.List,
                    ChainApproxMethod.ChainApproxSimple
                );

                int contourCount = contours.Size;
                for (int i = 0; i < contourCount; i++)
                {
                    using var contour = contours[i];
                    if (contour.Size < 30) continue; // 忽略小轮廓

                    var rect = CvInvoke.BoundingRectangle(contour);
                    if (IsValidTextRegion(rect))
                    {
                        // 合并重叠区域
                        bool merged = false;
                        for (int j = 0; j < regions.Count; j++)
                        {
                            if (regions[j].IntersectsWith(rect))
                            {
                                regions[j] = Rectangle.Union(regions[j], rect);
                                merged = true;
                                break;
                            }
                        }
                        if (!merged) regions.Add(rect);
                    }
                }
            }

            // 新增合并区域可视化调试
            using (var debugImage = _cachedProcessed.Copy())
            {
                foreach (var rect in regions)
                {
                    CvInvoke.Rectangle(debugImage, rect, new MCvScalar(0, 255, 0), 2);
                }
                CvInvoke.Imwrite("4-DebugRegions.png", debugImage);
            }

            // 改进的区域合并流程
            var mergedRegions = MergeAdjacentTextRegions(regions, image.Width);

            // 新增合并区域可视化调试
            using (var debugImage = _cachedProcessed.Copy())
            {
                foreach (var rect in mergedRegions)
                {
                    CvInvoke.Rectangle(debugImage, rect, new MCvScalar(0, 255, 0), 2);
                }
                CvInvoke.Imwrite("5-MergedRegions.png", debugImage);
            }

            // 批量OCR处理
            var result = new ConcurrentBag<RegionMarker>();
            var batches = mergedRegions
               .Select((r, i) => new { Index = i, Value = r })
               .GroupBy(x => x.Index / 20) // 减少批次数量
               .Select(g => g.Select(x => x.Value).ToList());


            Parallel.ForEach(batches, batch =>
            {
                using var localOcr = new Tesseract( // 每个线程使用独立实例
                  "./Resources/tessdata/",
                  "chi_sim+eng",  // 同时使用中英文
                OcrEngineMode.LstmOnly)
                {
                    PageSegMode = PageSegMode.SingleLine  // 明确单行模式
                };

                // 扩展白名单包含必要符号
                localOcr.SetVariable("tessedit_char_whitelist", "表面里面OPCEDR");

                foreach (var rect in batch)
                {
                    using var cropped = _cachedProcessed.Copy(rect);
                    string text = NormalizeText(RecognizeText(cropped, localOcr));

                    if (!string.IsNullOrEmpty(text))
                    {
                        result.Add(new RegionMarker { Name = text, BoundingBox = rect });
                    }
                }
            });
            // 添加后处理
            return PostProcessResults([.. result], image.Size);
        }

        /// <summary>
        /// 新增智能区域合并方法
        /// </summary>
        /// <param name="regions"></param>
        /// <param name="imageWidth"></param>
        /// <returns></returns>
        private List<Rectangle> MergeAdjacentTextRegions(List<Rectangle> regions, int imageWidth)
        {
            var mergedRegions = new List<Rectangle>();
            regions = regions
                .Where(r => r.X < imageWidth / 2)
                .OrderBy(r => r.Y)
                .ThenBy(r => r.X)
                .ToList();

            for (int i = 0; i < regions.Count; i++)
            {
                var current = regions[i];
                bool merged = false;

                // 查找后续可能合并的区域
                for (int j = i + 1; j < regions.Count; j++)
                {
                    var next = regions[j];

                    // 计算水平合并条件
                    float verticalOverlap = Math.Min(current.Bottom, next.Bottom) - Math.Max(current.Top, next.Top);
                    bool isSameLine = verticalOverlap > current.Height * 0.7f;

                    // 计算字符间距条件
                    float horizontalGap = next.X - current.Right;
                    float avgWidth = (current.Width + next.Width) / 2f;

                    // 合并条件：同一行且间距小于平均宽度的1.2倍
                    if (isSameLine && horizontalGap < avgWidth * 1.2f)
                    {
                        current = Rectangle.Union(current, next);
                        regions.RemoveAt(j);
                        j--;
                        merged = true;
                    }
                }

                mergedRegions.Add(current);
            }

            return mergedRegions.Where(r => r.Width > 30).ToList();
        }

        /// <summary>
        /// 修改文本识别方法
        /// </summary>
        /// <param name="roi"></param>
        /// <param name="localOcr"></param>
        /// <returns></returns>
        private string RecognizeText(Image<Gray, byte> roi, Tesseract localOcr)
        {
            // 调整图像尺寸和对比度
            using var resized = roi.Resize(2.0, Inter.Linear);
            using var enhanced = new Image<Gray, byte>(resized.Size);
            CvInvoke.EqualizeHist(resized, enhanced);

            // 添加10像素白色边框
            using var padded = new Image<Gray, byte>(
                enhanced.Width + 20,
                enhanced.Height + 20,
                new Gray(255));
            padded.ROI = new Rectangle(10, 10, enhanced.Width, enhanced.Height);
            enhanced.CopyTo(padded);
            padded.ROI = Rectangle.Empty;

            localOcr.SetImage(padded);
            localOcr.Recognize();
            return localOcr.GetUTF8Text().Trim();
        }

        private List<RegionMarker> PostProcessResults(List<RegionMarker> rawMarkers, Size imageSize)
        {
            var finalMarkers = new List<RegionMarker>();

            // 按区域位置排序
            var ordered = rawMarkers
                .OrderBy(m => m.BoundingBox.Y)
                .ThenBy(m => m.BoundingBox.X)
                .ToList();

            foreach (var marker in ordered)
            {
                // 文本修正
                string text = RefineText(marker.Name);

                // 验证是否为有效关键词
                if (TargetKeywords.Any(k => text.Contains(k)))
                {
                    // 处理重复项
                    var existing = finalMarkers.FirstOrDefault(m =>
                        m.Name == text &&
                        m.BoundingBox.IntersectsWith(marker.BoundingBox));

                    if (existing == null)
                    {
                        finalMarkers.Add(new RegionMarker
                        {
                            Name = text,
                            BoundingBox = marker.BoundingBox
                        });
                    }
                }
            }

            // 新增文本识别结果可视化调试
            using (var debugImage = _cachedProcessed.Copy())
            {
                foreach (var item in finalMarkers)
                {
                    CvInvoke.Rectangle(debugImage, item.BoundingBox, new MCvScalar(0, 255, 0), 2);
                }
                CvInvoke.Imwrite("6-PostRegionMarkers.png", debugImage);
            }

            // 确保关键标记数量
            return EnsureRequiredMarkers(finalMarkers, imageSize);
        }

        /// <summary>
        /// 确保必需标记存在（精确到表面/里面定位）
        /// </summary>
        private List<RegionMarker> EnsureRequiredMarkers(List<RegionMarker> markers, Size imageSize)
        {
            var surfaceAreaHeight = CalculateSurfaceBoundary(markers);
            var required = new Dictionary<string, (int Surface, int Inside)>
            {
                { "OP", (1, 1) },
                { "CE", (1, 1) },
                { "DR", (1, 1) }
            };

            // 统计表面/里面现有标记数量
            var surfaceMarkers = markers
                .Where(m => m.BoundingBox.Y < surfaceAreaHeight)
                .GroupBy(m => m.Name)
                .ToDictionary(g => g.Key, g => g.Count());

            var insideMarkers = markers
                .Where(m => m.BoundingBox.Y >= surfaceAreaHeight)
                .GroupBy(m => m.Name)
                .ToDictionary(g => g.Key, g => g.Count());

            // 生成需要补充的虚拟标记
            var virtualMarkers = new List<RegionMarker>();

            foreach (var key in required.Keys)
            {
                // 计算表面缺失数量
                int surfaceNeed = required[key].Surface - surfaceMarkers.GetValueOrDefault(key, 0);
                for (int i = 0; i < surfaceNeed; i++)
                {
                    virtualMarkers.Add(CreateVirtualMarker(key, imageSize, isSurface: true));
                }

                // 计算里面缺失数量
                int insideNeed = required[key].Inside - insideMarkers.GetValueOrDefault(key, 0);
                for (int i = 0; i < insideNeed; i++)
                {
                    virtualMarkers.Add(CreateVirtualMarker(key, imageSize, isSurface: false));
                }
            }

            markers.AddRange(virtualMarkers);
            return markers.OrderBy(m => m.BoundingBox.Y).ToList();
        }

        /// <summary>
        /// 动态计算表面区域分界阈值
        /// </summary>
        private int CalculateSurfaceBoundary(List<RegionMarker> markers)
        {
            // 步骤1：按名称分组并筛选有效组
            var validGroups = markers
                .GroupBy(m => m.Name)
                .Where(g => g.Count() == 2) // 只保留包含两个元素的组
                .ToList();

            if (validGroups.Count == 0)
            {
                // 无有效数据时返回图像中线（示例默认值）
                return _cachedProcessed?.Height / 2 ?? 0;
            }

            // 步骤2：收集表面和里面区域的Y坐标
            var surfaceYValues = new List<int>();
            var insideYValues = new List<int>();

            foreach (var group in validGroups)
            {
                // 按Y坐标排序，确定表面和里面位置
                var orderedMarkers = group
                    .OrderBy(m => m.BoundingBox.Top)
                    .ToList();

                // 第一个为表面位置，第二个为里面位置
                surfaceYValues.Add(orderedMarkers[0].BoundingBox.Top);
                insideYValues.Add(orderedMarkers[1].BoundingBox.Top);
            }

            // 步骤3：计算平均值
            double avgSurfaceY = surfaceYValues.Average();
            double avgInsideY = insideYValues.Average();

            // 步骤4：计算分界阈值（取两者中间值）
            return (int)((avgSurfaceY + avgInsideY) / 2);
        }

        /// <summary>
        /// 动态创建虚拟标记位置
        /// </summary>
        private RegionMarker CreateVirtualMarker(string name, Size imageSize, bool isSurface)
        {
            // 根据实际样板布局参数化坐标
            var basePositions = new Dictionary<string, (int X, int SurfaceY, int InsideY, int Width, int Height)>
            {
                { "OP", (1238, 346, 1754, 67, 40) },
                { "CE", (1227, 812, 2217, 65, 40) },
                { "DR", (1214, 1275, 2700, 66, 41) }
            };

            var pos = basePositions[name];
            return new RegionMarker
            {
                Name = name,
                BoundingBox = new Rectangle(
                    pos.X,
                    isSurface ? pos.SurfaceY : pos.InsideY,
                    pos.Width,
                    pos.Height
                )
            };
        }

        // 改进文本修正方法
        private string RefineText(string rawText)
        {
            var text = rawText
                .Replace("O", "0") // 统一字符格式
                .Replace("I", "1")
                .Replace(" ", "")
                .Replace("\n", "")
                .Replace("表而", "表面")
                .Replace("裏面", "里面")
                .Replace("0P", "OP")
                .Replace("CЕ", "CE")
                .Replace("DЯ", "DR")
                .Trim();

            // 动态匹配关键词
            var patterns = new Dictionary<string, string>
            {
                { @"^表[面]$", "表面" },
                { @"^.*OP.*$", "OP" },
                { @"^.*CE.*$", "CE" },
                { @"^.*DR.*$", "DR" },
                { @"^里[面]$", "里面" }
            };

            foreach (var pattern in patterns)
            {
                if (Regex.IsMatch(text, pattern.Key))
                {
                    return pattern.Value;
                }
            }
            return text;
        }

        public void Dispose()
        {
            if (_disposed) return;
            _cachedGray?.Dispose();
            _cachedProcessed?.Dispose();
            _disposed = true;
        }

        private bool IsValidTextRegion(Rectangle rect)
        {
            // 放宽尺寸限制
            return rect.Width > 20 && rect.Height > 10
                   && rect.Width < 400 && rect.Height < 200
                   && (float)rect.Width / rect.Height > 0.3
                   && (float)rect.Width / rect.Height < 5;
        }

        private string NormalizeText(string text)
        {
            // 去除空格和特殊字符
            return text.Replace(" ", "").Replace("\n", "")
                       .Replace("_", "").Replace("\r", "").Trim();
        }

        private float CalculateDistance(Point markerCenter, SixLabors.ImageSharp.RectangleF objBox)
        {
            var objCenter = new Point(
                (int)(objBox.X + objBox.Width / 2),
                (int)(objBox.Y + objBox.Height / 2));

            // 带方向的距离计算（优先考虑垂直对齐）
            float verticalWeight = 0.7f;
            float horizontalWeight = 0.3f;

            return verticalWeight * Math.Abs(markerCenter.Y - objCenter.Y) +
                   horizontalWeight * Math.Abs(markerCenter.X - objCenter.X);
        }
    }
    public class RegionMarker
    {
        public string Name { get; set; }
        public Rectangle BoundingBox { get; set; }
        public Point Center => new Point(
            BoundingBox.X + BoundingBox.Width / 2,
            BoundingBox.Y + BoundingBox.Height / 2);
    }
}
