namespace GRT
{
    using UnityEngine;

    public static class RootGameObject
    {
        /// <summary>Unity根物体，请勿销毁，附加了许多单例Unity脚本</summary>
        private static GameObject _root;

        public static void Init()
        {
            if (_root == null)
            {
                _root = new GameObject("[GRT Root]");
                _root.hideFlags = HideFlags.HideAndDontSave;
                Object.DontDestroyOnLoad(_root);
            }
        }

        /// <summary>
        /// 获取根物体上的某脚本
        /// </summary>
        /// <typeparam name="T">脚本类型</typeparam>
        public static T GetComponent<T>() where T : Component
        {
            return _root.GetComponent<T>();
        }

        /// <summary>
        /// 向根物体上附加某脚本
        /// </summary>
        /// <typeparam name="T">脚本类型</typeparam>
        public static T AddComponent<T>() where T : Component
        {
            return _root.AddComponent<T>();
        }
    }
}