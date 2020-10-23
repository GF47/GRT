/* ****************************************************************
 * @File Name   :   Utilities
 * @Author      :   GF47
 * @Date        :   2015/3/11 13:34:56
 * @Description :   [Geometry]的一些辅助方法
 * @Edit        :   2015/3/11 13:34:56
 * ***************************************************************/

using UnityEngine;

namespace GRT.Geometry
{
    /// <summary>
    /// [Geometry]的一些辅助方法
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// 获取点在直线上的投影点
        /// </summary>
        /// <param name="p">目标点</param>
        /// <param name="line">直线</param>
        /// <returns>投影点</returns>
        public static Vector3 GetPointProjection(Vector3 p, Line line)
        {
            if (line.A == line.B) return line.A;
            Vector3 vLine = line.B - line.A;
            float n = (p.x * vLine.x + p.y * vLine.y + p.z * vLine.z - line.A.x * vLine.x - line.A.y * vLine.y - line.A.z * vLine.z) / (vLine.x * vLine.x + vLine.y * vLine.y + vLine.z * vLine.z);
            return n * vLine + line.A;
        }

        public static Vector3 GetPointProjection(Vector3 p, Vector3 a, Vector3 b, Vector3 c)
        {
//          float fa =
//              p.x * a.x +
//              p.y * a.y +
//              p.z * a.z;
//          float fb =
//              p.x * b.x +
//              p.y * b.y +
//              p.z * b.z;
//          float fc =
//              p.x * c.x +
//              p.y * c.y +
//              p.z * c.z;
//          Matrix4x4 m = new Matrix4x4();
//          m.SetRow(0, new Vector4(a.x - b.x, a.y - b.y, a.z - b.z, 0f));
//          m.SetRow(1, new Vector4(a.x - c.x, a.y - c.y, a.z - c.z, 0f));
//          m.SetRow(2, new Vector4(b.x - c.x, b.y - c.y, b.z - c.z, 0f));
//          m.SetRow(3, new Vector4(0f, 0f, 0f, 0f));
//          Matrix4x4 mt = m.inverse;
//          Vector4 v = mt * new Vector4(fa - fb, fa - fc, fb - fc, 0f);
//          return new Vector3(v.x, v.y, v.z);
            return a;
        }

        /// <summary>
        /// 获取两条直线之间的公垂线
        /// <remarks>return 0：两条直线平行</remarks>
        /// <remarks>return 1：两条直线相交，则[out Line]的点[A]为交点，点[B]为法线</remarks>
        /// <remarks>return 2：两条直线异面，则[out Line]为公垂线</remarks>
        /// </summary>
        /// <param name="a">直线</param>
        /// <param name="b">直线</param>
        /// <param name="c">公垂线</param>
        /// <returns>结果类型</returns>
        public static int GetCommonPerpendicular(Line a, Line b, out Line c)
        {
            Vector3 vA = a.B - a.A;
            Vector3 vB = b.B - b.A;

            float f1 = vA.x * vA.x + vA.y * vA.y + vA.z * vA.z;
            float f2 = vA.x * a.A.x + vA.y * a.A.y + vA.z * a.A.z;
            float f3 = vA.x * b.A.x + vA.y * b.A.y + vA.z * b.A.z;
            float f4 = vB.x * vB.x + vB.y * vB.y + vB.z * vB.z;
            float f5 = vB.x * vA.x + vB.y * vA.y + vB.z * vA.z;
            float f6 = vB.x * a.A.x + vB.y * a.A.y + vB.z * a.A.z;
            float f7 = vB.x * b.A.x + vB.y * b.A.y + vB.z * b.A.z;
            float f8 = vA.x * vB.x + vA.y * vB.y + vA.z * vB.z;

            float denominator = f4 * f1 - f8 * f5;
            if (Mathf.Abs(denominator) < Mathf.Epsilon)
            {
                c = new Line(Line.LineType.Segment, a.A, b.A);
                return 0;
            }
            float m = (f8 * f6 + f4 * f3 - f4 * f2 - f8 * f7) / denominator;
            float n = (f3 * f5 + f1 * f6 - f2 * f5 - f1 * f7) / denominator;
            Vector3 pM = m * vA + a.A;
            Vector3 pN = n * vB + b.A;
            if (pM == pN)
            {
                c = new Line(Line.LineType.Segment, pM, Vector3.Normalize(pM + Vector3.Cross(vA, vB)));
                return 1;
            }
            c = new Line(Line.LineType.Segment, pM, pN);
            return 2;
        }

        /// <summary>
        /// 获取三个点所组成的三角形的重心
        /// </summary>
        /// <param name="a">三角形的顶点</param>
        /// <param name="b">三角形的顶点</param>
        /// <param name="c">三角形的顶点</param>
        /// <returns>三角形的重心</returns>
        public static Vector3 GetCenterOfGravity(Vector3 a, Vector3 b, Vector3 c)
        {
            return (a + b + c) / 3;
        }

        /// <summary>
        /// 获取三个点组成的三角形内切圆圆心
        /// </summary>
        /// <param name="a">三角形的顶点</param>
        /// <param name="b">三角形的顶点</param>
        /// <param name="c">三角形的顶点</param>
        /// <returns>三角形的内切圆圆心</returns>
        public static Vector3 GetCenterOfIncircle(Vector3 a, Vector3 b, Vector3 c)
        {
            float ab = Vector3.Distance(b, a);
            float ac = Vector3.Distance(c, a);
            float bc = Vector3.Distance(c, b);
            float denominator = ab + ac + bc;
            if (Mathf.Abs(denominator) < Mathf.Epsilon) return Vector3.zero;
            return new Vector3(
                bc * a.x + ac * b.x + ab * c.x,
                bc * a.y + ac * b.y + ab * c.y,
                bc * a.z + ac * b.z + ab * c.z) / denominator;
        }

        /// <summary>
        /// 获取三个点组成的三角形外接圆圆心
        /// </summary>
        /// <param name="a">三角形的顶点</param>
        /// <param name="b">三角形的顶点</param>
        /// <param name="c">三角形的顶点</param>
        /// <returns>三角形外接圆圆心</returns>
        public static Vector3 GetCenterOfCircumcircle(Vector3 a, Vector3 b, Vector3 c)
        {
            Vector3 vab = b - a;
            Vector3 vac = c - a;
            Vector3 normal = Vector3.Cross(vab, vac);

            Vector3 nab = Vector3.Cross(vab, normal);
            Vector3 mpab = (a + b) / 2f;
            Vector3 nac = Vector3.Cross(vac, normal);
            Vector3 mpac = (a + c) / 2f;

            Line lab = new Line(Line.LineType.Straight, mpab, mpab + nab);
            Line lac = new Line(Line.LineType.Straight, mpac, mpac + nac);
            Line line;
            int i = GetCommonPerpendicular(lab, lac, out line);
            if (i == 0)
            {
                return Vector3.zero;
            }
            return line.A;
        }
    }
}
