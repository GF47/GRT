namespace GRT
{
    public static class SingleExtension
    {
        /// <summary> 在浮点数 min 和 max 之间取一个随机数
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns></returns>
        public static float RandomRange(float min, float max)
        {
            if (System.Math.Abs(min - max) < 1e-6f)
            {
                return min;
            }
            return UnityEngine.Random.Range(min, max);
        }
    }
}