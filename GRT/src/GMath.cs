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
        public static float Mod(this float a, float b) { return (float)(a - System.Math.Floor(a / b) * b); }
        /// <summary>
        /// 取余数
        /// </summary>
        public static double Mod(this double a, double b) { return a - System.Math.Floor(a / b) * b; }

        /// <summary>
        /// 取小数值
        /// </summary>
        public static float Mod1(this float a) { return (float)(a - System.Math.Floor(a)); }
        /// <summary>
        /// 取小数值
        /// </summary>
        public static double Mod1(this double a) { return a - System.Math.Floor(a); }

        #region matlib

        /// <summary>
        /// 正态分布概率密度函数
        /// </summary>
        public static double normpdf(double x, double mu, double sigma)
        {
            return System.Math.Pow(System.Math.E, (mu - x) * (x - mu) / (2 * sigma * sigma)) / (System.Math.Sqrt(2 * System.Math.PI) * sigma);
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

                if (System.Math.Abs(sum - x) <= prec)
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
            double t3 = System.Math.Pow(System.Math.E, t2) / o;
            return t3 / System.Math.Sqrt(2 * System.Math.PI);
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
    }
}
