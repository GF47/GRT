using UnityEngine;

namespace GRT
{
    public static class Vector2Extension
    {
        public static void Deconstruct(this Vector2 v2, out float x, out float y)
        {
            x = v2.x;
            y = v2.y;
        }

        public static void Deconstruct(this Vector2Int v2, out int x, out int y)
        {
            x = v2.x;
            y = v2.y;
        }
    }
}