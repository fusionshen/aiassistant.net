using AI_Assistant_Win.Models.Middle;
using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AI_Assistant_Win.Utils
{
    public class ShapeHelper
    {
        public static double ComputeMedian(Mat image)
        {
            // 获取 Mat 数据（转换为 2D 数组）
            byte[,] pixelValues2D = (byte[,])image.GetData();

            // 将 2D 数组转换为 1D 数组
            int rows = pixelValues2D.GetLength(0);
            int cols = pixelValues2D.GetLength(1);
            byte[] pixelValues = new byte[rows * cols];

            int index = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    pixelValues[index++] = pixelValues2D[i, j];
                }
            }

            // 计算中位数
            Array.Sort(pixelValues);
            return pixelValues[pixelValues.Length / 2];
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
