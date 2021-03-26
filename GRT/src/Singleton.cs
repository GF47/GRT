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
    public class Singleton<T> where T : Singleton<T>, new()
    {
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    if (ConstructFunc == null)
                    {
                        throw new NullReferenceException(nameof(T) + "的ConstructFunc为空，请先指定构造方法");
                    }
                    instance = ConstructFunc();
                }
                return instance;
            }
        }
        protected static T instance;

        public static Func<T> ConstructFunc = () => new T();
    }

    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    if (ConstructFunc == null)
                    {
                        throw new NullReferenceException(nameof(T) + "的ConstructFunc为空，请先指定构造方法");
                    }
                    instance = ConstructFunc();
                }
                return instance;
            }
        }

        protected static T instance;

        public static Func<T> ConstructFunc;
    }
}
