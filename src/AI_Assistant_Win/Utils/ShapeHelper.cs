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
            // TODO: optimize 排序四个点，以确保左上、右上、左下、右下顺序
            var maybeFake = SortPointsToRectangle(GetVertices(convexHull));
            // 校验并替换四个顶点
            var correctedQuad = ReplaceWithTrueVertices(convexHull, maybeFake);
            // 返回四个顶点
            return correctedQuad;
        }

        // 校验并替换四个顶点
        public static Quadrilateral ReplaceWithTrueVertices(List<PointF> allPoints, Quadrilateral quad)
        {
            // 初始化四个顶点
            PointF topLeft = quad.TopLeft;
            PointF topRight = quad.TopRight;
            PointF bottomLeft = quad.BottomLeft;
            PointF bottomRight = quad.BottomRight;

            // 遍历所有点集，找到更合适的四个顶点
            foreach (var point in allPoints)
            {
                // 更新左上点：更左且更上
                if (point.X <= topLeft.X && point.Y <= topLeft.Y)
                    topLeft = point;

                // 更新右上点：更右且更上
                if (point.X >= topRight.X && point.Y <= topRight.Y)
                    topRight = point;

                // 更新左下点：更左且更下
                if (point.X <= bottomLeft.X && point.Y >= bottomLeft.Y)
                    bottomLeft = point;

                // 更新右下点：更右且更下
                if (point.X >= bottomRight.X && point.Y >= bottomRight.Y)
                    bottomRight = point;
            }

            // 创建并返回更新后的 Quadrilateral 对象
            return new Quadrilateral(topLeft, topRight, bottomLeft, bottomRight);
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

        public static List<PointF> GetVertices(List<PointF> convexHull)
        {
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

        public static List<PointF> ComputeConvexHull(List<PointF> points)
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

        private struct Vector2D(double x, double y)
        {
            public double X = x;
            public double Y = y;
        }

        public static float CalculateDistance(PointF p1, PointF p2)
        {
            // 计算两点之间的欧几里得距离
            return (float)Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
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
