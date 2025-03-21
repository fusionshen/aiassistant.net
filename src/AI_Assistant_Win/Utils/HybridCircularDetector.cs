using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AI_Assistant_Win.Utils
{
    public class HybridCircularDetector
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

        public static (List<PointF> dominantContour, double area) DetectCircular(
          List<PointF> yoloPoints,
          Mat originalImage,
          DetectionParams param = null)
        {
            param ??= new DetectionParams();

            // 阶段1：生成YOLO区域掩码
            using var yoloMask = CreateYoloRegionMask(yoloPoints, originalImage.Size, param.RegionMargin);

            // 阶段2：在YOLO区域内提取主轮廓
            var mainContour = ExtractDominantContour(originalImage, yoloMask, param);

            // 阶段3：几何校正与最外层类圆轮廓查找
            return RefineCircular(mainContour);
        }

        private static (List<PointF> dominantContour, double area) RefineCircular(VectorOfPoint mainContour)
        {
            if (mainContour == null || mainContour.Size == 0)
                return (new List<PointF>(), 0);

            // 直接使用原始轮廓点集
            List<PointF> rawPoints = mainContour.ToArray()
                .Select(p => new PointF(p.X, p.Y))
                .ToList();

            // 计算原始轮廓面积（绝对值处理）
            double area = Math.Abs(CvInvoke.ContourArea(mainContour));

            return (rawPoints, area);
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

        private static float Cross(PointF a, PointF b, PointF c)
        {
            return (b.X - a.X) * (c.Y - a.Y) - (b.Y - a.Y) * (c.X - a.X);
        }
    }
}