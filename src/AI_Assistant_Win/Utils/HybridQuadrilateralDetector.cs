using AI_Assistant_Win.Models.Middle;
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
    /// <summary> 
    /// 量块(表面设计会吸收顺光源放置的采光，导致表面太黑)受到数据集和斜放物品的分割扫描头影响，会在边缘产生波浪形毛刺。量块正置放置，优化还行，前提：采光充足。
    /// 两种方式去获得顶点：1.yolo11的实例分割，2.CV的轮廓识别。
    /// 各种情况下，两者会互补去取顶点，现在黑度工作台应该没有啥大问题了，圆片面积主要受采光的影响。(如果量块表面光滑反光一些也都没太大问题)
    /// 问题到这里：导致量块分割波浪形毛刺也可能主要是量块表面设计特质。
    /// </summary>
    public class HybridQuadrilateralDetector
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

            // +++ 新增代码：绘制轮廓并保存 +++
            if (mainContour != null)
            {
                // 创建原始图像的副本进行绘制操作
                using Mat imageWithContour = originalImage.Clone();
                // 将轮廓转换为绘制所需的格式
                Point[][] contoursArray = [mainContour.ToArray()];
                using (var contours = new VectorOfVectorOfPoint(contoursArray))
                {
                    // 绘制绿色轮廓（BGR颜色空间，线宽2）
                    CvInvoke.DrawContours(
                        imageWithContour,
                        contours,
                        contourIdx: -1,  // -1表示绘制所有轮廓
                        new MCvScalar(0, 255, 0), // 绿色
                        thickness: 2);
                }
                // 保存带轮廓的图像
                CvInvoke.Imwrite("Gauge_4_Contour.png", imageWithContour);
            }

            // 当主轮廓有效时，使用旋转矩形
            if (mainContour != null)
            {
                var rotatedRect = CvInvoke.MinAreaRect(mainContour);
                return new Quadrilateral(rotatedRect);
            }

            // 降级使用YOLO点
            return SortPointsToRectangle(FindBestQuadrilateral(
                ShapeHelper.ComputeConvexHull(yoloPoints)));
        }
        private static Mat CreateYoloRegionMask(List<PointF> points, Size size, int margin)
        {
            var mask = new Mat(size, DepthType.Cv8U, 1);
            mask.SetTo(new MCvScalar(0));

            // 生成带边距的凸包区域
            var expandedHull = ShapeHelper.ComputeConvexHull(points)
                .Select(p => new PointF(
                    p.X + margin * Math.Sign(p.X - size.Width / 2f),
                    p.Y + margin * Math.Sign(p.Y - size.Height / 2f)
                )).ToList();

            using var vop = new VectorOfPoint(expandedHull.Select(p => new Point((int)p.X, (int)p.Y)).ToArray());
            CvInvoke.FillConvexPoly(mask, vop, new MCvScalar(255));
            CvInvoke.Imwrite("Gauge_1_Yolo-Mask.png", mask);
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
            // 自动Canny阈值
            double median = ShapeHelper.ComputeMedian(processed);
            //CvInvoke.Canny(processed, processed, (int)Math.Max(0, median * 0.4), (int)Math.Min(255, median * 1.2));
            CvInvoke.Canny(processed, processed, param.CannyThreshold1, param.CannyThreshold2);
            CvInvoke.Imwrite("Gauge_2_Canny-Improved.png", processed);

            // 形态学闭合填充
            var kernel = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(3, 3), new Point(-1, -1));
            CvInvoke.MorphologyEx(processed, processed, MorphOp.Close, kernel, new Point(-1, -1), iterations: 2, BorderType.Default, default);

            CvInvoke.Imwrite("Gauge_3_Morphology.png", processed);

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
    }
}