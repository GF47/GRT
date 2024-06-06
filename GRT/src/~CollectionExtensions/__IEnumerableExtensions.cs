using System;
using System.Collections.Generic;

namespace GRT
{
    public static class __IEnumerableExtensions
    {
        public static T FindExt<T>(this IEnumerable<T> collection, Predicate<T> predicate)
        {
            foreach (var item in collection)
            {
                if (predicate(item))
                {
                    return item;
                }
            }

            return default;
        }

        public static T[] GetSpan<T>(this IEnumerable<T> collection, Predicate<T> match, int length, T @default = default)
        {
            var array = new T[length];

            var i = 0;
            var found = false;
            foreach (var item in collection)
            {
                array[i] = item;

                if (!found)
                {
                    found = match(item);
                }

                i++;
                if (i >= length)
                {
                    if (found) { break; }
                    else { i -= length; }
                }
            }

            for (int j = found ? i : 0; j < length; j++)
            {
                array[j] = @default;
            }

            return array;
        }

        public static T[] GetSpan<T>(this IEnumerable<T> collection, int target, int length, T @default = default)
        {
            var array = new T[length];

            int i = 0;
            int n = -1;
            foreach (var item in collection)
            {
                array[i] = item;

                i++;
                n++;
                if (i >= length)
                {
                    if (n >= target) { break; }
                    else { i -= length; }
                }
            }

            for (int j = n >= target ? i : 0; j < length; j++)
            {
                array[j] = @default;
            }

            return array;
        }
    }
}