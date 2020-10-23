/***************************************************************
 * @File Name       : Coroutines
 * @Author          : GF47
 * @Description     : 统一的协同程序调用入口
 * @Date            : 2017/7/25/星期二 15:26:51
 * @Edit            : none
 **************************************************************/

using System;
using System.Collections;
using UnityEngine;

namespace GRT
{
    public class Coroutines : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod]
        static void Init() { _instance = RootGameObject.AddComponent<Coroutines>(); }
        private static Coroutines _instance;

        public static void StartACoroutineWithCallback(IEnumerator routine, Action callback)
        {
            _instance.StartCoroutine(__startACoroutineWithCallback(routine, callback));
        }

        public static void StartACoroutine(IEnumerator routine)
        {
            _instance.StartCoroutine(routine);
        }

        public static void DelayInvoke(Action action, float delay)
        {
            _instance.StartCoroutine(__startACoroutineWithCallback(new WaitForSecondsRealtime(delay), action));
        }

        public static void StopACoroutine(IEnumerator routine)
        {
            _instance.StopCoroutine(routine);
        }

        public static void StopAll()
        {
            _instance.StopAllCoroutines();
        }

        private static IEnumerator __startACoroutineWithCallback(IEnumerator routine, Action callback)
        {
            yield return routine;
            callback?.Invoke();
        }
    }
}
