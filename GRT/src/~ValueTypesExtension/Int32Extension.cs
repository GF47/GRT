namespace GRT
{
    public static class Int32Extension
    {
        /// <summary> �������� min �� max ֮��ȡһ�������
        /// </summary>
        /// <param name="min">��Сֵ</param>
        /// <param name="max">���ֵ</param>
        /// <returns></returns>
        public static int RandomRange(int min, int max)
        {
            if (min == max)
            {
                return min;
            }
            return UnityEngine.Random.Range(min, max + 1);
        }
    }
}