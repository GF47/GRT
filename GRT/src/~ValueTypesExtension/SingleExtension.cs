namespace GRT
{
    public static class SingleExtension
    {
        /// <summary> �ڸ����� min �� max ֮��ȡһ�������
        /// </summary>
        /// <param name="min">��Сֵ</param>
        /// <param name="max">���ֵ</param>
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