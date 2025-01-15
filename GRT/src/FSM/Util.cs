using System;
using System.Collections;
using System.Collections.Generic;

namespace GRT.FSM
{
    public static class Util
    {
        /// <summary>
        /// 当 ID 为此值时，没有对应的状态
        /// </summary>
        public const int NullStateID = int.MinValue;

        public const int EntryStateID = int.MinValue;
        public const int ExitStateID = int.MaxValue;

        /// <summary>
        /// 判断是否为合法的ID
        /// </summary>
        public static bool IsValid(int anyID) => anyID != NullStateID;

        public static void DeepReset(this IResetable resetable, bool resetSelf = false, Action<IResetable> extraAction = null)
        {
            if (resetSelf)
            {
                resetable.Reset();
                extraAction?.Invoke(resetable);
            }

            if (resetable is IEnumerable<IResetable> enumerable)
            {
                foreach (var a in enumerable)
                {
                    if (a != null)
                    {
                        DeepReset(a, true, extraAction);
                    }
                }
            }
        }

        public static IEnumerator<IResetable> CombineIResetables(params IEnumerable[] collections)
        {
            foreach (var enumerable in collections)
            {
                if (enumerable != null)
                {
                    foreach (var a in enumerable)
                    {
                        if (a is IResetable resetable)
                        {
                            yield return resetable;
                        }
                    }
                }
            }
        }
    }
}