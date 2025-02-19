using AI_Assistant_Win.Models.Middle;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AI_Assistant_Win.Utils
{
    public class ShapeHelper
    {
        // 获取矩形的四个顶点
        public static Quadrilateral GetRectangleVertices(List<PointF> points)
        {
            // 排序四个点，以确保左上、右上、左下、右下顺序
            var sortedPoints = SortPointsToRectangle(GetVertices(points));

            // 返回四个顶点
            return sortedPoints;
        }

        // 排序点以确保符合矩形的四个角
        public static Quadrilateral SortPointsToRectangle(List<PointF> points)
        {
            // 计算质心（所有点的平均值）
            double centerX = points.Average(p => p.X);
            double centerY = points.Average(p => p.Y);
            Point centroid = new Point((int)centerX, (int)centerY);

            // 计算每个点相对于质心的极角
            var sortedPoints = points.Select(p => new { Point = p, Angle = CalculateAngle(centroid, p) })
                                     .OrderBy(p => p.Angle)
                                     .Select(p => p.Point)
                                     .ToList();

            // 获取排序后的四个点
            var topLeft = sortedPoints[0];      // 最小角度点
            var topRight = sortedPoints[1];     // 第二小角度点
            var bottomLeft = sortedPoints[2];   // 第三小角度点
            var bottomRight = sortedPoints[3];  // 最大角度点

            // 返回四个顶点
            return new Quadrilateral(topLeft, topRight, bottomLeft, bottomRight);
        }

        // 计算两点之间的角度
        private static double CalculateAngle(PointF origin, PointF point)
        {
            return Math.Atan2(point.Y - origin.Y, point.X - origin.X);
        }

        public static List<PointF> GetVertices(List<PointF> points)
        {
            // 计算凸包
            List<PointF> convexHull = ComputeConvexHull(points);

            // 如果凸包顶点数小于等于4，直接返回
            if (convexHull.Count <= 4)
                return convexHull;

            // 使用Douglas-Peucker算法简化到四个顶点
            List<PointF> simplified = SimplifyToFourVertices(convexHull);

            // 如果简化后仍多于四个点，选择四个角度最小的顶点
            if (simplified.Count > 4)
                simplified = SelectFourCornersByAngle(simplified);

            return simplified;
        }

        private static List<PointF> ComputeConvexHull(List<PointF> points)
        {
            if (points.Count <= 1)
                return new List<PointF>(points);

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

        private static float Cross(PointF o, PointF a, PointF b)
        {
            return (a.X - o.X) * (b.Y - o.Y) - (a.Y - o.Y) * (b.X - o.X);
        }

        private static List<PointF> SimplifyToFourVertices(List<PointF> convexHull)
        {
            if (convexHull.Count <= 4)
                return convexHull;

            double low = 0;
            double high = CalculateMaxEpsilon(convexHull);
            double epsilon = 0;
            List<PointF> result = new List<PointF>();
            int iterations = 0;
            const int maxIterations = 100;

            while (iterations++ < maxIterations)
            {
                epsilon = (low + high) / 2;
                result = DouglasPeuckerSimplify(convexHull, epsilon);

                if (result.Count == 4)
                    break;
                else if (result.Count > 4)
                    low = epsilon;
                else
                    high = epsilon;
            }

            return result.Count == 4 ? result : SelectFourCornersByAngle(result);
        }

        private static double CalculateMaxEpsilon(List<PointF> points)
        {
            double maxDistance = 0;
            foreach (PointF p in points)
            {
                double dx = p.X - points[0].X;
                double dy = p.Y - points[0].Y;
                double distance = Math.Sqrt(dx * dx + dy * dy);
                if (distance > maxDistance)
                    maxDistance = distance;
            }
            return maxDistance;
        }

        private static List<PointF> DouglasPeuckerSimplify(List<PointF> points, double epsilon)
        {
            if (points.Count <= 2)
                return new List<PointF>(points);

            int index = 0;
            double dmax = 0;
            int last = points.Count - 1;

            for (int i = 1; i < last; i++)
            {
                double d = PerpendicularDistance(points[i], points[0], points[last]);
                if (d > dmax)
                {
                    index = i;
                    dmax = d;
                }
            }

            List<PointF> result = new List<PointF>();
            if (dmax > epsilon)
            {
                List<PointF> firstLine = points.Take(index + 1).ToList();
                List<PointF> secondLine = points.Skip(index).ToList();
                List<PointF> recResults1 = DouglasPeuckerSimplify(firstLine, epsilon);
                List<PointF> recResults2 = DouglasPeuckerSimplify(secondLine, epsilon);

                result.AddRange(recResults1.Take(recResults1.Count - 1));
                result.AddRange(recResults2);
            }
            else
            {
                result.Add(points[0]);
                result.Add(points[last]);
            }

            return result;
        }

        private static double PerpendicularDistance(PointF point, PointF lineStart, PointF lineEnd)
        {
            double area = Math.Abs((lineEnd.X - lineStart.X) * (lineStart.Y - point.Y) -
                         (lineStart.X - point.X) * (lineEnd.Y - lineStart.Y));
            double lineLength = Math.Sqrt(Math.Pow(lineEnd.X - lineStart.X, 2) + Math.Pow(lineEnd.Y - lineStart.Y, 2));
            return lineLength == 0 ? 0 : area / lineLength;
        }

        private static List<PointF> SelectFourCornersByAngle(List<PointF> convexHull)
        {
            // 计算每个顶点的角度并选择四个最小的
            List<Tuple<PointF, double>> angles = new List<Tuple<PointF, double>>();
            int count = convexHull.Count;

            for (int i = 0; i < count; i++)
            {
                int prev = (i - 1 + count) % count;
                int next = (i + 1) % count;

                PointF a = convexHull[prev];
                PointF b = convexHull[i];
                PointF c = convexHull[next];

                double angle = CalculateAngle(a, b, c);
                angles.Add(Tuple.Create(b, angle));
            }

            // 按角度升序排序并选择前四个点
            var sorted = angles.OrderBy(t => t.Item2).Select(t => t.Item1).Take(4).ToList();

            // 确保四个点按顺时针或逆时针顺序排列
            return OrderVertices(sorted);
        }

        private static double CalculateAngle(PointF a, PointF b, PointF c)
        {
            // 计算向量BA和BC之间的夹角
            Vector2D ba = new Vector2D(a.X - b.X, a.Y - b.Y);
            Vector2D bc = new Vector2D(c.X - b.X, c.Y - b.Y);

            double dotProduct = ba.X * bc.X + ba.Y * bc.Y;
            double magnitudeBA = Math.Sqrt(ba.X * ba.X + ba.Y * ba.Y);
            double magnitudeBC = Math.Sqrt(bc.X * bc.X + bc.Y * bc.Y);

            if (magnitudeBA == 0 || magnitudeBC == 0)
                return 0;

            double cosTheta = dotProduct / (magnitudeBA * magnitudeBC);
            return Math.Acos(Math.Clamp(cosTheta, -1, 1)) * (180 / Math.PI);
        }

        private static List<PointF> OrderVertices(List<PointF> points)
        {
            // 按质心排序顶点
            PointF center = new PointF(
                points.Average(p => p.X),
                points.Average(p => p.Y));

            return points.OrderBy(p => Math.Atan2(p.Y - center.Y, p.X - center.X)).ToList();
        }

        private struct Vector2D
        {
            public double X;
            public double Y;

            public Vector2D(double x, double y)
            {
                X = x;
                Y = y;
            }
        }

    }
}
