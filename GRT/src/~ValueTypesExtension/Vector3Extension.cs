namespace GRT
{
    using UnityEngine;

    public static class Vector3Extension
    {
        /// <summary>����һ������������ֵ
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Vector3 Round(this Vector3 value)
        {
            value.x = Mathf.Round(value.x);
            value.y = Mathf.Round(value.y);
            value.z = Mathf.Round(value.z);
            return value;
        }
    }
}