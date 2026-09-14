namespace GRT
{
    using UnityEngine;

    public static class Vector3Extension
    {
        /// <summary>返回一个向量的整数值
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

        public static void Deconstruct(this Vector3 v3, out float x, out float y, out float z)
        {
            x = v3.x;
            y = v3.y;
            z = v3.z;
        }

        public static void Deconstruct(this Vector3Int v3, out int x, out int y, out int z)
        {
            x = v3.x;
            y = v3.y;
            z = v3.z;
        }
    }
}