/***************************************************************
 * @File Name       : MonoSingleton
 * @Author          : GF47
 * @Description     : 单例，然而单例还特么的要继承，我也是醉了……
 * @Date            : 2017/7/17/星期一 11:51:07
 * @Edit            : none
 **************************************************************/

using System;
using UnityEngine;

namespace GRT
{
    /// <summary>
    /// 单例基类，没啥卵用
    /// </summary>
    /// <typeparam name="T">自己</typeparam>
    public class Singleton<T> where T : Singleton<T>
    {
        /// <summary>
        /// 单例
        /// </summary>
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Activator.CreateInstance<T>();
                    // if (ConstructFunc == null)
                    // {
                    //     throw new NullReferenceException(nameof(T) + "的ConstructFunc为空，请先指定构造方法");
                    // }
                    // instance = ConstructFunc();
                }
                return instance;
            }
        }

        protected static T instance;

        /// <summary>
        /// 这就比较sb了，外部还能实例化一个出来2333
        /// </summary>
        // public static Func<T> ConstructFunc = () => new T();
    }

    /// <summary>
    /// Unity脚本单例
    /// </summary>
    /// <typeparam name="T">自己</typeparam>
    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        /// <summary>
        /// 单例
        /// </summary>
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    var go = new GameObject(nameof(T));
                    DontDestroyOnLoad(go);
                    instance = go.AddComponent<T>();
                    Debug.Log($"{nameof(T)} loaded");
                }
                return instance;
            }
        }

        protected static T instance;

        /// <summary>
        /// 子类最好还是继承一下，防止多次实例化
        /// </summary>
        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
            }
            else
            {
                if (instance != this)
                {
                    throw new UnityException($"Do not init another {nameof(T)}");
                }
            }
        }
    }
}