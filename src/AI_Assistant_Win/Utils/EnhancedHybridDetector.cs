using AI_Assistant_Win.Models.Middle;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;

namespace AI_Assistant_Win.Utils
{
    /// <summary> 
    /// 量块(表面设计会吸收顺光源放置的采光，导致表面太黑)受到数据集和斜放物品的分割扫描头影响，会在边缘产生波浪形毛刺。量块正置放置，优化还行，前提：采光充足。
    /// 两种方式去获得顶点：1.yolo11的实例分割，2.CV的轮廓识别。
    /// 各种情况下，两者会互补去取顶点，现在黑度工作台应该没有啥大问题了，圆片面积主要受采光的影响。(如果量块表面光滑反光一些也都没太大问题)
    /// 问题到这里：导致量块分割波浪形毛刺也可能主要是量块表面设计特质。
    /// </summary>
    public class EnhancedHybridDetector
    {
        public class DetectionParams
        {
            // YOLO区域参数
            public int RegionMargin = 15;           // 边界扩展像素
            public float MinMaskCoverage = 0.7f;     // 最小轮廓覆盖率

            // OpenCV参数
            public int CannyThreshold1 = 30;
            public int CannyThreshold2 = 90;
            public int GaussianSize = 5;            // 高斯模糊核尺寸

            // 几何参数
            public double EpsilonFactor = 0.015;    // 多边形逼近精度系数
        }

        /// <summary>
        /// 1. 扩大 mask 区域，确保完整轮廓 （右侧平移，效果不佳）
        /// 在 CreateYoloRegionMask() 方法中，你给 yoloPoints 计算凸包(Convex Hull) 并增加了 margin，但如果物体不在中央，margin 可能不够。
        /// 你可以增加 margin 的值，或者根据物体的位置动态调整 margin，确保完整轮廓不被裁剪。
        /// 2. 重新计算 ROI，使其包含完整轮廓 (15像素效果不佳)
        /// 在 GetNonZeroBoundingRect() 方法中，你计算的是 mask 的 boundingRect，但如果 mask 过小或位置不对，ROI 可能导致轮廓被截断。可以尝试扩大 ROI 的范围：
        /// 3. 让 ExtractDominantContour() 使用原始图片，而不是裁剪后的 ROI (性能问题)
        /// 目前，你是用 roiMat = new Mat(image, boundingRect) 进行裁剪，这可能会导致部分轮廓被切割。
        /// 你可以改为在整个图像中处理轮廓，然后只在 mask 区域内筛选有效轮廓，这样可以避免裁剪错误：
        /// </summary>
        /// <param name="yoloPoints"></param>
        /// <param name="originalImage"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static Quadrilateral DetectQuadrilateral(
          List<PointF> yoloPoints,
          Mat originalImage,
          DetectionParams param = null)
        {
            param ??= new DetectionParams();

            // 阶段1：生成YOLO区域掩码
            using var yoloMask = CreateYoloRegionMask(yoloPoints, originalImage.Size, param.RegionMargin);

            // 阶段2：在YOLO区域内提取主轮廓
            var mainContour = ExtractDominantContour(originalImage, yoloMask, param);

            // 阶段3：几何校正与顶点优化
            return RefineQuadrilateral(mainContour, yoloPoints, param);
        }

        private static Mat CreateYoloRegionMask(List<PointF> points, Size size, int margin)
        {
            var mask = new Mat(size, DepthType.Cv8U, 1);
            mask.SetTo(new MCvScalar(0));

            // 生成带边距的凸包区域
            var expandedHull = ComputeConvexHull(points)
                .Select(p => new PointF(
                    p.X + margin * Math.Sign(p.X - size.Width / 2f),
                    p.Y + margin * Math.Sign(p.Y - size.Height / 2f)
                )).ToList();

            using var vop = new VectorOfPoint(expandedHull.Select(p => new Point((int)p.X, (int)p.Y)).ToArray());
            CvInvoke.FillConvexPoly(mask, vop, new MCvScalar(255));
            CvInvoke.Imwrite("1-mask.png", mask);
            return mask;
        }

        private static VectorOfPoint ExtractDominantContour(Mat image, Mat mask, DetectionParams param)
        {
            // ROI限定处理
            var boundingRect = GetNonZeroBoundingRect(mask, param);
            using var roiMat = new Mat(image, boundingRect);

            // 预处理流程
            using var processed = new Mat();
            CvInvoke.CvtColor(roiMat, processed, ColorConversion.Bgr2Gray);
            CvInvoke.GaussianBlur(processed, processed, new Size(param.GaussianSize, param.GaussianSize), 1.5);
            CvInvoke.Canny(processed, processed, param.CannyThreshold1, param.CannyThreshold2);

            // 形态学闭合填充
            var kernel = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(3, 3), new Point(-1, -1));
            CvInvoke.MorphologyEx(processed, processed, MorphOp.Close, kernel, new Point(-1, -1), iterations: 2, BorderType.Default, default);

            CvInvoke.Imwrite("2-processed.png", processed);

            // 提取并筛选轮廓
            var contours = new VectorOfVectorOfPoint();
            CvInvoke.FindContours(processed, contours, null, RetrType.List, ChainApproxMethod.ChainApproxNone);

            // 选择与YOLO区域重叠最大的主轮廓
            VectorOfPoint bestContour = null;
            double maxOverlap = 0;
            for (int i = 0; i < contours.Size; i++)
            {
                var contour = contours[i];
                if (ContourValidation(contour, mask, boundingRect, param))
                {
                    var overlap = CalculateContourOverlap(contour, mask, boundingRect);
                    if (overlap > maxOverlap)
                    {
                        maxOverlap = overlap;
                        bestContour = contour;
                    }
                }
            }

            // 坐标转换回原图
            return bestContour != null
                ? OffsetContour(bestContour, boundingRect.Location)
                : null;
        }

        private static Quadrilateral RefineQuadrilateral(VectorOfPoint contour, List<PointF> yoloPoints, DetectionParams param)
        {
            if (contour == null)
            {
                return SortPointsToRectangle(FindBestQuadrilateral(ComputeConvexHull(yoloPoints)));
            }
            // 多边形逼近优化
            var approx = new VectorOfPoint();
            var epsilon = param.EpsilonFactor * CvInvoke.ArcLength(contour, true);
            CvInvoke.ApproxPolyDP(contour, approx, epsilon, true);

            // 顶点数验证与校正
            var vertices = approx.ToArray().Select(p => new PointF(p.X, p.Y)).ToList();
            if (vertices.Count != 4 || !IsQuadrilateralApproximatelyRectangle(vertices))
            {
                // 使用YOLO点生成备用四边形
                vertices = FindBestQuadrilateral(ComputeConvexHull(yoloPoints));
            }
            return SortPointsToQuadrilateral(vertices);
        }
        /// <summary>
        /// 1. 计算四条边的长度，检查是否接近矩形比例
        ///    计算对边长度的比值，要求比值接近 1。
        /// 2. 计算四个角的角度，检查是否接近 90°
        ///    计算每个角的夹角，确保角度在 85° ~ 95° 之间。
        /// 3. 检查对边是否平行
        ///    计算两对对边的方向向量，确保其余弦相似度接近 1。
        /// </summary>
        /// <param name="vertices"></param>
        /// <param name="angleTolerance"></param>
        /// <param name="lengthRatioTolerance"></param>
        /// <returns></returns>
        private static bool IsQuadrilateralApproximatelyRectangle(List<PointF> vertices, double angleTolerance = 5.0, double lengthRatioTolerance = 0.2)
        {
            if (vertices.Count != 4) return false;

            // 计算四条边的向量
            double[] edgeLengths = new double[4];
            for (int i = 0; i < 4; i++)
            {
                int next = (i + 1) % 4;
                edgeLengths[i] = Distance(vertices[i], vertices[next]);
            }

            // 长边和短边
            double length1 = (edgeLengths[0] + edgeLengths[2]) / 2.0;
            double length2 = (edgeLengths[1] + edgeLengths[3]) / 2.0;

            // **1. 检查对边长度比值是否接近 1**
            double ratio1 = edgeLengths[0] / edgeLengths[2];
            double ratio2 = edgeLengths[1] / edgeLengths[3];

            if (Math.Abs(ratio1 - 1) > lengthRatioTolerance || Math.Abs(ratio2 - 1) > lengthRatioTolerance)
                return false;

            // **2. 检查四个角是否接近 90°**
            for (int i = 0; i < 4; i++)
            {
                int prev = (i - 1 + 4) % 4;
                int next = (i + 1) % 4;
                double angle = CalculateAngle(vertices[prev], vertices[i], vertices[next]);
                if (Math.Abs(angle - 90) > angleTolerance) return false;
            }

            // **3. 检查对边是否平行**
            Vector2 v1 = new Vector2(vertices[1].X - vertices[0].X, vertices[1].Y - vertices[0].Y);
            Vector2 v2 = new Vector2(vertices[3].X - vertices[2].X, vertices[3].Y - vertices[2].Y);
            Vector2 v3 = new Vector2(vertices[2].X - vertices[1].X, vertices[2].Y - vertices[1].Y);
            Vector2 v4 = new Vector2(vertices[0].X - vertices[3].X, vertices[0].Y - vertices[3].Y);

            if (!AreVectorsParallel(v1, v2) || !AreVectorsParallel(v3, v4))
                return false;

            return true;
        }

        // 计算两点之间的欧几里得距离
        private static double Distance(PointF p1, PointF p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }

        // 计算向量夹角（单位：度）
        private static double CalculateAngle(PointF a, PointF b, PointF c)
        {
            double abX = a.X - b.X, abY = a.Y - b.Y;
            double cbX = c.X - b.X, cbY = c.Y - b.Y;

            double dotProduct = (abX * cbX) + (abY * cbY);
            double magnitudeAB = Math.Sqrt(abX * abX + abY * abY);
            double magnitudeCB = Math.Sqrt(cbX * cbX + cbY * cbY);

            double cosTheta = dotProduct / (magnitudeAB * magnitudeCB);
            return Math.Acos(cosTheta) * (180.0 / Math.PI);
        }

        // 判断两个向量是否近似平行（余弦相似度接近 1）
        private static bool AreVectorsParallel(Vector2 v1, Vector2 v2, double tolerance = 0.1)
        {
            v1 = Vector2.Normalize(v1);
            v2 = Vector2.Normalize(v2);
            return Math.Abs(Vector2.Dot(v1, v2)) > (1 - tolerance);
        }

        // 从凸包中找出最佳四边形的四个顶点
        private static List<PointF> FindBestQuadrilateral(List<PointF> convexHull)
        {
            double maxArea = 0;
            List<PointF> bestQuadrilateralPoints = null;

            int n = convexHull.Count;
            // 暴力枚举所有可能的四个顶点组合
            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    for (int k = j + 1; k < n; k++)
                    {
                        for (int l = k + 1; l < n; l++)
                        {
                            PointF p1 = convexHull[i];
                            PointF p2 = convexHull[j];
                            PointF p3 = convexHull[k];
                            PointF p4 = convexHull[l];

                            double area = CalculateQuadrilateralArea(p1, p2, p3, p4);
                            if (area > maxArea)
                            {
                                maxArea = area;
                                bestQuadrilateralPoints = new List<PointF> { p1, p2, p3, p4 };
                            }
                        }
                    }
                }
            }

            return bestQuadrilateralPoints;
        }

        // 获取四个点的具体位置（左上、右上、左下、右下）
        public static Quadrilateral SortPointsToRectangle(List<PointF> points)
        {
            // 计算质心
            PointF centroid = new PointF(
                points.Average(p => p.X),
                points.Average(p => p.Y)
            );

            // 计算每个点相对于质心的角度
            var sortedPoints = points
                .Select(p => new
                {
                    Point = p,
                    Angle = Math.Atan2(p.Y - centroid.Y, p.X - centroid.X)  // 计算点相对于质心的角度
                })
                .OrderBy(p => p.Angle)  // 按角度排序
                .Select(p => p.Point)
                .ToList();

            // 按顺时针顺序排列四个点
            PointF topLeft = sortedPoints[0];
            PointF topRight = sortedPoints[1];
            PointF bottomRight = sortedPoints[2];
            PointF bottomLeft = sortedPoints[3];

            // 返回按照左上、右上、左下、右下排序的四个点
            return new Quadrilateral(topLeft, topRight, bottomLeft, bottomRight);
        }

        // 计算四边形的面积
        public static double CalculateQuadrilateralArea(PointF p1, PointF p2, PointF p3, PointF p4)
        {
            // 采用四边形顶点按顺时针或逆时针顺序排列
            double area = 0.0;

            // 使用向量叉积法来计算四边形面积
            area += p1.X * p2.Y - p2.X * p1.Y;
            area += p2.X * p3.Y - p3.X * p2.Y;
            area += p3.X * p4.Y - p4.X * p3.Y;
            area += p4.X * p1.Y - p1.X * p4.Y;

            // 计算绝对值并除以2
            area = Math.Abs(area) / 2.0;

            return area;
        }

        private static Rectangle GetNonZeroBoundingRect(Mat mask, DetectionParams param)
        {
            using Mat nonZeroPoints = new Mat();
            CvInvoke.FindNonZero(mask, nonZeroPoints);
            if (nonZeroPoints.Rows > 0)
            {
                var boundingRect = CvInvoke.BoundingRectangle(nonZeroPoints);
                int expandSize = 2 * param.RegionMargin;  // 例如，扩展 10 像素
                return new Rectangle(
                    Math.Max(0, boundingRect.X - expandSize),
                    Math.Max(0, boundingRect.Y - expandSize),
                    Math.Min(mask.Width - boundingRect.X, boundingRect.Width + 2 * expandSize),
                    Math.Min(mask.Height - boundingRect.Y, boundingRect.Height + 2 * expandSize)
                );
            }

            return new Rectangle(0, 0, mask.Width, mask.Height);
        }

        private static bool ContourValidation(VectorOfPoint contour, Mat mask, Rectangle roi, DetectionParams param)
        {
            // 面积阈值验证
            var area = CvInvoke.ContourArea(contour);
            if (area < 100) return false;

            // 覆盖度验证
            using var contourMask = new Mat(mask.Size, DepthType.Cv8U, 1);
            contourMask.SetTo(new MCvScalar(0));
            CvInvoke.DrawContours(contourMask, new VectorOfVectorOfPoint(contour), -1, new MCvScalar(255), -1, offset: new Point(roi.X, roi.Y));

            CvInvoke.BitwiseAnd(contourMask, mask, contourMask);
            var coverage = CvInvoke.CountNonZero(contourMask) / (double)mask.Size.Width * mask.Size.Height;
            return coverage >= param.MinMaskCoverage;
        }

        private static double CalculateContourOverlap(VectorOfPoint contour, Mat mask, Rectangle roi)
        {
            using var contourMask = new Mat(mask.Size, DepthType.Cv8U, 1);
            contourMask.SetTo(new MCvScalar(0));
            CvInvoke.DrawContours(contourMask, new VectorOfVectorOfPoint(contour), -1, new MCvScalar(255), -1, offset: new Point(roi.X, roi.Y));

            CvInvoke.BitwiseAnd(contourMask, mask, contourMask);
            return CvInvoke.CountNonZero(contourMask) / (double)CvInvoke.CountNonZero(mask);
        }

        private static VectorOfPoint OffsetContour(VectorOfPoint contour, Point offset)
        {
            return new VectorOfPoint(contour.ToArray()
                .Select(p => new Point(p.X + offset.X, p.Y + offset.Y)).ToArray());
        }

        private static List<PointF> ComputeConvexHull(List<PointF> points)
        {
            // 优化后的Andrew算法实现
            points = points.Distinct().ToList();
            if (points.Count < 3) return points;

            var sorted = points.OrderBy(p => p.X).ThenBy(p => p.Y).ToList();

            List<PointF> lower = new List<PointF>();
            foreach (var p in sorted)
            {
                while (lower.Count >= 2 && Cross(lower[lower.Count - 2], lower[lower.Count - 1], p) <= 0)
                    lower.RemoveAt(lower.Count - 1);
                lower.Add(p);
            }

            List<PointF> upper = new List<PointF>();
            foreach (var p in sorted.AsEnumerable().Reverse())
            {
                while (upper.Count >= 2 && Cross(upper[upper.Count - 2], upper[upper.Count - 1], p) <= 0)
                    upper.RemoveAt(upper.Count - 1);
                upper.Add(p);
            }

            lower.RemoveAt(lower.Count - 1);
            upper.RemoveAt(upper.Count - 1);

            return lower.Concat(upper).ToList();
        }

        private static Quadrilateral SortPointsToQuadrilateral(List<PointF> points)
        {
            // 计算质心
            PointF centroid = new PointF(
                points.Average(p => p.X),
                points.Average(p => p.Y)
            );

            // 计算每个点相对于质心的角度
            var sortedPoints = points
                .Select(p => new
                {
                    Point = p,
                    Angle = Math.Atan2(p.Y - centroid.Y, p.X - centroid.X)  // 计算点相对于质心的角度
                })
                .OrderBy(p => p.Angle)  // 按角度排序
                .Select(p => p.Point)
                .ToList();

            // 按顺时针顺序排列四个点
            PointF topLeft = sortedPoints[0];
            PointF topRight = sortedPoints[1];
            PointF bottomRight = sortedPoints[2];
            PointF bottomLeft = sortedPoints[3];

            // 返回按照左上、右上、左下、右下排序的四个点
            return new Quadrilateral(topLeft, topRight, bottomLeft, bottomRight);
        }

        private static float Cross(PointF a, PointF b, PointF c)
        {
            return (b.X - a.X) * (c.Y - a.Y) - (b.Y - a.Y) * (c.X - a.X);
        }
    }
}