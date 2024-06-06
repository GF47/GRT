using System;

namespace GRT
{
    public static class ValueRangeExtensions
    {
        public static T Clamp<T>(this T v, T min, T max) where T : IComparable<T>
        {
            if (v.CompareTo(min) < 0) { return min; }
            else if (v.CompareTo(max) > 0) { return max; }
            else { return v; }
        }

        public static int Cycle(this int v, int min, int max)
        {
            var period = max - min;
            var r = (v - min) % period;
            if (r < 0)
            {
                r += period;
            }
            return r + min;
        }

        public static float Cycle(this float v, float min, float max)
        {
            var period = max - min;
            var r = (v - min) % period;
            if (r < 0f)
            {
                r += period;
            }
            return r + min;
        }

        public static double Cycle(this double v, double min, double max)
        {
            var period = max - min;
            var r = (v - min) % period;
            if (r < 0d)
            {
                r += period;
            }

            return r + min;
        }
    }
}