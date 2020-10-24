namespace GRT
{
    public static class Int32Extension
    {
        /// <summary> 在整形数 min 和 max 之间取一个随机数
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns></returns>
        public static int RandomRange(int min, int max)
        {
            if (min == max)
            {
                return min;
            }
            return UnityEngine.Random.Range(min, max + 1);
        }

        /// <summary>
        /// 将整形数值限制在阈值内
        /// </summary>
        public static int Clamp(this int t, int min, int max)
        {
            if (t >= max)
            {
                return max;
            }
            if (t <= min)
            {
                return min;
            }
            return t;
        }
    }
}