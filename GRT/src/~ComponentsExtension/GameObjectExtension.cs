using System;

namespace GRT
{
    using UnityEngine;

    public static class GameObjectExtension
    {
        /// <summary> 返回目标物体的完整层级
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetHierarchy(this GameObject obj)
        {
            string path = obj.name;

            while (obj.transform.parent != null)
            {
                obj = obj.transform.parent.gameObject;
                path = string.Format(@"{0}\{1}", obj.name, path);
            }
            return string.Format(@"\{0}\", path);
        }

        /// <summary> 在目标物体上添加一个子物体
        /// </summary>
        /// <param name="parent">目标物体</param>
        /// <returns>子物体</returns>
        public static GameObject AddChild(this GameObject parent)
        {
            GameObject go = new GameObject();

            if (parent != null)
            {
                Transform t = go.transform;
                t.parent = parent.transform;
                t.localPosition = Vector3.zero;
                t.localRotation = Quaternion.identity;
                t.localScale = Vector3.one;
                go.layer = parent.layer;
            }
            return go;
        }

        /// <summary> 在目标物体上添加一个实例物体的复制作为子物体
        /// </summary>
        /// <param name="parent">目标物体</param>
        /// <param name="prefab">实例物体</param>
        /// <returns>子物体</returns>
        public static GameObject AddChild(this GameObject parent, GameObject prefab)
        {
            GameObject go = Object.Instantiate(prefab);

            if (go != null && parent != null)
            {
                Transform t = go.transform;
                t.parent = parent.transform;
                t.localPosition = Vector3.zero;
                t.localRotation = Quaternion.identity;
                t.localScale = Vector3.one;
                go.layer = parent.layer;
            }
            return go;
        }

        /// <summary> 在目标物体上添加一个带有 T 组件的物体作为子物体
        /// </summary>
        /// <typeparam name="T">组件类</typeparam>
        /// <param name="parent">目标物体</param>
        /// <returns>子物体的T组件实例</returns>
        public static T AddChild<T>(this GameObject parent) where T : Component
        {
            GameObject go = AddChild(parent);
            go.name = typeof(T).ToString();
            return go.AddComponent<T>();
        }

        /// <summary> 在父层级上查找离目标物体关系最近的 T 组件
        /// </summary>
        /// <typeparam name="T">组件类</typeparam>
        /// <param name="target">目标物体</param>
        /// <returns>查找到的 T 组件</returns>
        public static T FindInParent<T>(this GameObject target) where T : Component
        {
            if (target == null) return null;
            T component = target.GetComponent<T>();

            if (component == null)
            {
                Transform t = target.transform.parent;

                while (t != null && component == null)
                {
                    component = t.gameObject.GetComponent<T>();
                    t = t.parent;
                }
            }
            return component;
        }

        /// <summary> 设置物体的层级，包括子物体
        /// </summary>
        /// <param name="target">目标物体</param>
        /// <param name="layer">层级</param>
        public static void Setlayer(this GameObject target, int layer)
        {
            target.layer = layer;

            Transform t = target.transform;
            for (int i = 0, iMax = t.childCount; i < iMax; ++i)
            {
                Transform child = t.GetChild(i);
                Setlayer(child.gameObject, layer);
            }
        }

        public static T GetInterface<T>(this GameObject target) where T : class
        {
            MonoBehaviour[] scripts = target.GetComponents<MonoBehaviour>();
            if (scripts != null)
            {
                for (int i = 0; i < scripts.Length; i++)
                {
                    if (scripts[i] is T)
                    {
                        return scripts[i] as T;
                    }
                }
            }
            return null;
        }
        public static T[] GetInterfaces<T>(this GameObject target) where T : class
        {
            MonoBehaviour[] scripts = target.GetComponents<MonoBehaviour>();
            if (scripts != null)
            {
                T[] results = new T[scripts.Length];
                int n = 0;
                for (int i = 0; i < scripts.Length; i++)
                {
                    T tmp = scripts[i] as T;
                    if (tmp != null)
                    {
                        results[n] = tmp;
                        n++;
                    }
                }
                Array.Resize(ref results, n);
                return results;
            }
            return null;
        }
    }
}