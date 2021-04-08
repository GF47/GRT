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
    /// <summary>
    /// 执行Unity携程的公共类
    /// </summary>
    public class Coroutines : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Init() 
        {
            RootGameObject.GRTInitializing += () =>
            {
                _instance = RootGameObject.AddComponent<Coroutines>();
                Debug.Log($"{nameof(Coroutines)} loaded on {_instance.name}");
            };
        }
        private static Coroutines _instance;

        private void Awake()
        {
            if (_instance!= null && _instance != this)
            {
                Debug.LogWarning("Do not init another Coroutines");
                Destroy(this);
            }
        }

        /// <summary>
        /// 在一段携程执行完成后，执行指定的回调
        /// </summary>
        public static void StartACoroutineWithCallback(IEnumerator routine, Action callback)
        {
            _instance.StartCoroutine(__startACoroutineWithCallback(routine, callback));
        }

        /// <summary>
        /// 执行一段携程
        /// </summary>
        public static void StartACoroutine(IEnumerator routine)
        {
            _instance.StartCoroutine(routine);
        }

        /// <summary>
        /// 等待指定时间（秒）后，执行指定的回调
        /// </summary>
        public static void DelayInvoke(Action action, float delay)
        {
            _instance.StartCoroutine(__startACoroutineWithCallback(new WaitForSecondsRealtime(delay), action));
        }

        /// <summary>
        /// 停止一个正在执行的携程
        /// </summary>
        public static void StopACoroutine(IEnumerator routine)
        {
            _instance.StopCoroutine(routine);
        }

        /// <summary>
        /// 停止所有的携程
        /// </summary>
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
