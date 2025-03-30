using AI_Assistant_Win.Utils;
using Emgu.CV.Structure;
using System;
using System.Drawing;
using System.Linq;

namespace AI_Assistant_Win.Models.Middle
{
    public class QuadrilateralSegmentation : SimpleSegmentation
    {
        public Quadrilateral Quadrilateral { get; set; }
    }

    public class Quadrilateral()
    {
        public Quadrilateral(PointF topLeft, PointF topRight, PointF bottomLeft, PointF bottomRight) : this()
        {
            TopLeft = topLeft;
            TopRight = topRight;
            BottomLeft = bottomLeft;
            BottomRight = bottomRight;
        }

        public PointF TopLeft { get; set; }
        public PointF TopRight { get; set; }
        public PointF BottomLeft { get; set; }
        public PointF BottomRight { get; set; }

        // 新增从RotatedRect构造的方法
        public Quadrilateral(RotatedRect rect) : this()
        {
            PointF[] vertices = rect.GetVertices();
            vertices = SortVertices(vertices); // 顶点排序
            (TopLeft, TopRight, BottomRight, BottomLeft) =
                (vertices[0], vertices[1], vertices[2], vertices[3]);
        }

        // 新增尺寸属性
        public double Width => ShapeHelper.CalculateDistance(TopLeft, TopRight);
        public double Height => ShapeHelper.CalculateDistance(TopLeft, BottomLeft);

        // 核心顶点处理方法
        private static PointF[] SortVertices(PointF[] vertices)
        {
            // 步骤1：计算质心
            PointF centroid = new PointF(
                vertices.Average(p => p.X),
                vertices.Average(p => p.Y)
            );

            // 步骤2：按极角排序（顺时针）
            Array.Sort(vertices, (a, b) =>
            {
                double angleA = Math.Atan2(a.Y - centroid.Y, a.X - centroid.X);
                double angleB = Math.Atan2(b.Y - centroid.Y, b.X - centroid.X);
                return angleA.CompareTo(angleB);
            });

            // 步骤3：确保左上角为首点（适用于倾斜矩形）
            int startIndex = FindStartIndex(vertices);
            return vertices
                .Skip(startIndex)
                .Concat(vertices.Take(startIndex))
                .ToArray();
        }

        // 寻找左上角起始点（考虑旋转）
        private static int FindStartIndex(PointF[] vertices)
        {
            int minSumIndex = 0;
            float minSum = float.MaxValue;

            for (int i = 0; i < vertices.Length; i++)
            {
                float sum = vertices[i].X + vertices[i].Y;
                if (sum < minSum)
                {
                    minSum = sum;
                    minSumIndex = i;
                }
            }
            return minSumIndex;
        }
    }
}
