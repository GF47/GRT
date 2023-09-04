using System;
using System.Collections.Generic;

namespace GRT
{
    public static class __IListExtensions
    {
        public static T Find<T>(this IList<T> list, Predicate<T> predicate)
        {
            for (int i = 0; i < list.Count; i++)
            {
                var item = list[i];
                if (predicate(item))
                {
                    return item;
                }
            }
            return default;
        }

        public static int IndexOf<T>(this IList<T> list, Predicate<T> predicate)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (predicate(list[i]))
                {
                    return i;
                }
            }
            return -1;
        }
    }
}