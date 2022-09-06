/***************************************************************
 * @File Name       : GFMath
 * @Author          : GF47
 * @Description     : 一些数学运算
 * @Date            : 2017/5/11/星期四 14:46:18
 * @Edit            : none
 **************************************************************/

#define UNITY
// #undef UNITY

namespace GRT
{
    using System;

#if UNITY

    using UnityEngine;

#endif

    /// <summary>
    /// 一些数学运算
    /// </summary>
    public static class GMath
    {
        /// <summary>
        /// 取余数
        /// </summary>
        public static int Mod(this int a, int b) { return a % b; }

        /// <summary>
        /// 取余数
        /// </summary>
        public static float Mod(this float a, float b) { return (float)(a - Math.Floor(a / b) * b); }

        /// <summary>
        /// 取余数
        /// </summary>
        public static double Mod(this double a, double b) { return a - Math.Floor(a / b) * b; }

        /// <summary>
        /// 取小数值
        /// </summary>
        public static float Mod1(this float a) { return (float)(a - Math.Floor(a)); }

        /// <summary>
        /// 取小数值
        /// </summary>
        public static double Mod1(this double a) { return a - Math.Floor(a); }

        #region matlib

        /// <summary>
        /// 正态分布概率密度函数
        /// </summary>
        public static double normpdf(double x, double mu, double sigma)
        {
            return Math.Pow(Math.E, (mu - x) * (x - mu) / (2 * sigma * sigma)) / (Math.Sqrt(2 * Math.PI) * sigma);
        }

        /// <summary>
        /// <see cref="https://bbs.csdn.net/topics/390164216"/>
        /// </summary>
        public static double norminv(double x, double mu, double sigma)
        {
            const double min = -10d;
            const double max = 10d;
            const double delta = 0.01d;
            const double prec = 0.001d;

            double sum = 0d;
            double r = -100000000d;

            for (double i = min; i + delta <= max; i += delta)
            {
                sum += norminv_inner(i, mu, sigma) * delta;

                if (Math.Abs(sum - x) <= prec)
                {
                    r = i;
                    break;
                }
            }
            return r;
        }

        private static double norminv_inner(double x, double mu, double o)
        {
            double t1 = (x - mu) / o;
            double t2 = -1 * t1 * t1 / 2d;
            double t3 = Math.Pow(Math.E, t2) / o;
            return t3 / Math.Sqrt(2 * Math.PI);
        }

        #endregion matlib

#if UNITY

        #region 贝塞尔曲线

        /// <summary>
        /// 二阶贝塞尔曲线
        /// </summary>
        public static Vector3 QuadBezier(Vector3 p1, Vector3 c, Vector3 p2, float t)
        {
            var d = 1f - t;
            return d * d * p1 + 2f * d * t * c + t * t * p2;
        }

        /// <summary>
        /// 三阶贝塞尔曲线
        /// </summary>
        public static Vector3 QubicBezier(Vector3 p1, Vector3 c1, Vector3 c2, Vector3 p2, float t)
        {
            var d = 1f - t;
            return d * d * d * p1 + 3f * d * d * t * c1 + 3f * d * t * t * c2 + t * t * t * p2;
        }

        public static Vector3 CatmullRoom(Vector3 c1, Vector3 p1, Vector3 p2, Vector3 c2, float t)
        {
            return 0.5f *
            (
                (-c1 + 3f * p1 - 3f * p2 + c2) * (t * t * t)
                + (2f * c1 - 5f * p1 + 4f * p2 - c2) * (t * t)
                + (-c1 + p2) * t
                + 2f * p1
            );
        }

        #endregion 贝塞尔曲线

#endif

        #region matrix

        /// <summary>
        /// 生成单位矩阵
        /// </summary>
        public static float[,] GetIdentityMatrix(int n)
        {
            var matrix = new float[n, n];
            for (int i = 0; i < n; i++)
            {
                matrix[i, i] = 1f;
            }
            return matrix;
        }

        /// <summary>
        /// 矩阵复制
        /// </summary>
        private static float[,] MatrixCopy(float[,] ma)
        {
            var row = ma.GetLength(0);
            var col = ma.GetLength(1);

            var mb = new float[row, col];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    mb[i, j] = ma[i, j];
                }
            }
            return mb;
        }

        /// <summary>
        /// 矩阵相加
        /// </summary>
        public static float[,] MatrixAddition(this float[,] ma, float[,] mb)
        {
            if (ma == null) { return mb; }
            if (mb == null) { return ma; }

            var row = ma.GetLength(0);
            var col = ma.GetLength(1);
            if (row != mb.GetLength(0)) { throw new Exception("ma row != mb row"); }
            if (col != mb.GetLength(1)) { throw new Exception("ma column != mb column"); }

            var matrix = new float[row, col];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    matrix[i, j] = ma[i, j] + mb[i, j];
                }
            }
            return matrix;
        }

        /// <summary>
        /// 矩阵相减
        /// </summary>
        public static float[,] MatrixSubtraction(this float[,] ma, float[,] mb)
        {
            if (ma == null) { return mb.MatrixMultiply(-1f); }
            if (mb == null) { return ma; }

            var row = ma.GetLength(0);
            var col = ma.GetLength(1);
            if (row != mb.GetLength(0)) { throw new Exception("ma row != mb row"); }
            if (col != mb.GetLength(1)) { throw new Exception("ma column != mb column"); }

            var matrix = new float[row, col];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    matrix[i, j] = ma[i, j] - mb[i, j];
                }
            }
            return matrix;
        }

        /// <summary>
        /// 矩阵数乘
        /// </summary>
        public static float[,] MatrixMultiply(this float[,] ma, float b)
        {
            if (ma == null) { throw new NullReferenceException("left matrix ma is null"); }

            var row = ma.GetLength(0);
            var col = ma.GetLength(1);
            var matrix = new float[row, col];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    matrix[i, j] = b * ma[i, j];
                }
            }
            return matrix;
        }

        /// <summary>
        /// 矩阵相乘
        /// </summary>
        public static float[,] MatrixMultiply(this float[,] ma, float[,] mb)
        {
            // if (ma == null) { throw new NullReferenceException("left matrix ma is null"); }
            if (ma == null) { return mb; }
            if (mb == null) { return ma; }

            var marow = ma.GetLength(0);
            var macol = ma.GetLength(1);
            var mbcol = mb.GetLength(1);

            if (macol != mb.GetLength(0)) { throw new Exception("ma column != mb row"); }

            var matrix = new float[marow, mbcol];
            for (int i = 0; i < marow; i++)
            {
                for (int j = 0; j < mbcol; j++)
                {
                    var v = 0f;
                    for (int k = 0; k < macol; k++)
                    {
                        v += ma[i, k] * mb[k, j];
                    }
                    matrix[i, j] = v;
                }
            }
            return matrix;
        }

        /// <summary>
        /// 矩阵乘单列矩阵
        /// </summary>
        /// <param name="ma">原矩阵</param>
        /// <param name="mb">单列矩阵</param>
        /// <returns>结果也是单列矩阵，但行数为原矩阵的行数</returns>
        public static float[] MatrixMultiply(this float[,] ma, float[] mb)
        {
            if (ma == null) { throw new NullReferenceException("left matrix ma is null"); }
            if (mb == null) { throw new NullReferenceException("right single column matrix mb is null"); }

            var marow = ma.GetLength(0);
            var macol = ma.GetLength(1);

            if (macol != mb.Length) { throw new Exception("ma column != mb length"); }

            var matrix1 = new float[marow];
            for (int i = 0; i < marow; i++)
            {
                var v = 0f;
                for (int k = 0; k < macol; k++)
                {
                    v += ma[i, k] * mb[k];
                }
                matrix1[i] = v;
            }
            return matrix1;
        }

        /// <summary>
        /// 矩阵转置
        /// </summary>
        /// <param name="ma">原矩阵</param>
        /// <returns>转置后的矩阵</returns>
        public static float[,] MatrixTranspose(this float[,] ma)
        {
            if (ma == null) { throw new NullReferenceException("ma is null"); }

            var row = ma.GetLength(0);
            var col = ma.GetLength(1);
            var matrix = new float[col, row];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    matrix[j, i] = ma[i, j];
                }
            }
            return matrix;
        }

        /// <summary>
        /// 矩阵原地转置
        /// </summary>
        /// <param name="ma">原矩阵</param>
        public static float[,] MatrixTransposeSelf(this float[,] ma)
        {
            if (ma == null) { throw new NullReferenceException("ma is null"); }

            var row = ma.GetLength(0);
            var col = ma.GetLength(1);

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    var temp = ma[i, j];
                    ma[i, j] = ma[j, i];
                    ma[j, i] = temp;
                }
            }
            return ma;
        }

        /// <summary>
        /// 矩阵求逆
        /// </summary>
        public static float[,] MatrixInverse(this float[,] ma)
        {
            var row = ma.GetLength(0);
            var col = ma.GetLength(1);

            var mac = MatrixCopy(ma);

            var mb = GetIdentityMatrix(Math.Min(row, col));

            for (int j = 0; j < col; j++)
            {
                // swap row in case our pivot point is not working
                if (mac[j, j] == 0f)
                {
                    var big = j;
                    for (int i = 0; i < row; i++)
                    {
                        if (Math.Abs(mac[i, j]) > Math.Abs(mac[big, j]))
                        {
                            big = i;
                        }
                    }
                    if (big == j)
                    {
                        // this is a singular matrix
                        throw new Exception("singular matrix");
                    }
                    else // swap rows
                    {
                        for (int k = 0; k < col; k++)
                        {
                            (mac[j, k], mac[big, k]) = (mac[big, k], mac[j, k]);
                            (mb[j, k], mb[big, k]) = (mb[big, k], mb[j, k]);
                        }
                    }
                }

                // set each row in the column to 0
                for (int i = 0; i < row; i++)
                {
                    if (i != j)
                    {
                        var coeff = mac[i, j] / mac[j, j];
                        if (coeff != 0f)
                        {
                            for (int k = 0; k < col; k++)
                            {
                                mac[i, k] -= coeff * ma[j, k];
                                mb[i, k] -= coeff * mb[j, k];
                            }
                            // set the element to 0 for seafty
                            mac[i,j] = 0f;
                        }
                    }
                }
            }

            for (int i = 0; i < row; i++)
            {
                var maci = mac[i, i];
                for (int j = 0; j < col; j++)
                {
                    mb[i, j] /= maci;
                }
            }
            return mb;
        }

        // /// <summary>
        // /// 矩阵求逆
        // /// <para>通过解线性方程组的方式求逆矩阵，mb分别取单位阵的各个列向量</para>
        // /// <para>所得到的解向量就是逆矩阵的各个列向量，拼成逆矩阵即可</para>
        // /// </summary>
        // public static float[,] MatrixInverse(this float[,] ma)
        // {
        //     var n = ma.GetLength(1);
        //     var mi = new float[n, n];
        //     var mb = GetIdentityMatrix(n);

        //     // 求单列向量，这里可以并行，所以康皮特界一般用LUP来计算
        //     for (int i = 0; i < n; i++)
        //     {
        //         var ml = new float[n, n];
        //         var mu = new float[n, n];
        //         var p = new int[n];

        //         // LUP分解会改变原矩阵，所以复制一份来保证原矩阵不变
        //         var ma_copy = MatrixCopy(ma);

        //         MatrixLUPDecompose(ma_copy, ml, mu, p);
        //         MatrixLUPSolve(mi, i, mb, ml, mu, p);
        //     }
        //     return mi.MatrixTransposeSelf();
        // }

        // /// <summary>
        // /// 求解线性方程组
        // /// <para> Ax = b ==> PAx = Pb ==> LUx= Pb ==> Ly = Pb ==> Ux = y</para>
        // /// </summary>
        // private static void MatrixLUPSolve(float[,] inv, int row, float[,] mb, float[,] ml, float[,] mu, int[] p)
        // {
        //     var n = ml.GetLength(1);

        //     var y = new float[n];

        //     // 正向替换
        //     for (int i = 0; i < n; i++)
        //     {
        //         y[i] = mb[row, p[i]];
        //         for (int j = 0; j < i; j++)
        //         {
        //             y[i] = y[i] - ml[i, j] * y[j];
        //         }
        //     }

        //     // 反向替换
        //     for (int i = n - 1; i >= 0; i--)
        //     {
        //         var v = y[i];
        //         for (int j = n - 1; j > i; j--)
        //         {
        //             v -= mu[i, j] * inv[row, j];
        //         }
        //         v /= mu[i, i];

        //         inv[row, i] = v;
        //     }
        // }

        // /// <summary>
        // /// LUP分解
        // /// </summary>
        // private static void MatrixLUPDecompose(float[,] ma, float[,] ml, float[,] mu, int[] p)
        // {
        //     var n = ma.GetLength(1);

        //     var row = 0;

        //     for (int i = 0; i < n; i++) { p[i] = i; }
        //     for (int i = 0; i < n - 1; i++)
        //     {
        //         var pv = 0f;
        //         for (int j = i; j < n; j++)
        //         {
        //             var abs = Math.Abs(ma[j, i]);
        //             if (abs > pv)
        //             {
        //                 pv = abs;
        //                 row = j;
        //             }
        //         }

        //         if (pv < 1e-8f) { throw new Exception("pv is too small, matrix maybe singular"); }

        //         // 交换 p[i] 和 p[row]
        //         var tmp = p[i];
        //         p[i] = p[row];
        //         p[row] = tmp;

        //         var tmp2 = 0f;
        //         for (int j = 0; j < n; j++)
        //         {
        //             // 交换 ma[i, j] 和 ma[row, j]
        //             tmp2 = ma[i, j];
        //             ma[i, j] = ma[row, j];
        //             ma[row, j] = tmp2;
        //         }

        //         // LU
        //         var u = ma[i, i];
        //         for (int j = i + 1; j < n; j++)
        //         {
        //             var l = ma[j, i] / u;
        //             ma[j, i] = l;
        //             // 更新 ma
        //             for (int k = i + 1; k < n; k++)
        //             {
        //                 ma[j, k] = ma[j, k] - ma[i, k] * l;
        //             }
        //         }
        //     }

        //     // 组合 ml 和 mu
        //     for (int i = 0; i < n; i++)
        //     {
        //         for (int j = 0; j <= i; j++)
        //         {
        //             if (i != j)
        //             {
        //                 ml[i, j] = ma[i, j];
        //             }
        //             else
        //             {
        //                 ml[i, j] = 1f;
        //             }
        //         }
        //         for (int k = i; k < n; k++)
        //         {
        //             mu[i, k] = ma[i, k];
        //         }
        //     }
        // }

        #endregion matrix
    }
}