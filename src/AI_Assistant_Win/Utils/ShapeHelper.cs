using AI_Assistant_Win.Models.Middle;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AI_Assistant_Win.Utils
{
    public class ShapeHelper
    {
        // 获取四边形的四个顶点
        public static Quadrilateral GetRectangleVertices(List<PointF> points)
        {
            // 计算凸包
            List<PointF> convexHull = ComputeConvexHull(points);
            if (convexHull.Count == 4)
            {
                return SortPointsToRectangle(convexHull);
            }
            else if (convexHull.Count > 4)
            {
                return SortPointsToRectangle(FindBestQuadrilateral(convexHull));
            }
            else
            {
                throw new ArgumentException(LocalizeHelper.NOT_A_QUADRILATERAL);
            }
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

        // 计算凸包
        public static List<PointF> ComputeConvexHull(List<PointF> points)
        {
            if (points.Count <= 1)
                return points;

            points.Sort((a, b) => a.X.CompareTo(b.X) != 0 ? a.X.CompareTo(b.X) : a.Y.CompareTo(b.Y));

            List<PointF> lower = new List<PointF>();
            foreach (PointF p in points)
            {
                while (lower.Count >= 2 && Cross(lower[lower.Count - 2], lower[lower.Count - 1], p) <= 0)
                    lower.RemoveAt(lower.Count - 1);
                lower.Add(p);
            }

            List<PointF> upper = new List<PointF>();
            for (int i = points.Count - 1; i >= 0; i--)
            {
                PointF p = points[i];
                while (upper.Count >= 2 && Cross(upper[upper.Count - 2], upper[upper.Count - 1], p) <= 0)
                    upper.RemoveAt(upper.Count - 1);
                upper.Add(p);
            }

            lower.RemoveAt(lower.Count - 1);
            upper.RemoveAt(upper.Count - 1);

            lower.AddRange(upper);
            return lower;
        }

        private static double Cross(PointF o, PointF a, PointF b)
        {
            return (a.X - o.X) * (b.Y - o.Y) - (a.Y - o.Y) * (b.X - o.X);
        }

        public static double CalculateDistance(PointF p1, PointF p2)
        {
            // 计算两点之间的欧几里得距离
            return Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
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

    }
}
