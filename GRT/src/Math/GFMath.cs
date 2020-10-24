/***************************************************************
 * @File Name       : GFMath
 * @Author          : GF47
 * @Description     : 一些数学运算
 * @Date            : 2017/5/11/星期四 14:46:18
 * @Edit            : none
 **************************************************************/

using System;

namespace GRT
{
    /// <summary>
    /// 一些数学运算
    /// </summary>
    public static class GFMath
    {
        /// <summary>
        /// 取余数
        /// </summary>
        public static int Mod(int a, int b) { return a % b; }
        /// <summary>
        /// 取余数
        /// </summary>
        public static float Mod(float a, float b) { return (float)(a - Math.Floor(a / b) * b); }
        /// <summary>
        /// 取余数
        /// </summary>
        public static double Mod(double a, double b) { return a - Math.Floor(a / b) * b; }

        /// <summary>
        /// 取小数值
        /// </summary>
        public static float Mod1(float a) { return (float)(a - Math.Floor(a)); }
        /// <summary>
        /// 取小数值
        /// </summary>
        public static double Mod1(double a) { return a - Math.Floor(a); }

    }
}
