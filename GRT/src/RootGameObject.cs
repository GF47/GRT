namespace GRT
{
    using UnityEngine;

    public static class RootGameObject
    {
        /// <summary>
        /// Unity根物体，请勿销毁，附加了许多单例Unity脚本
        /// </summary>
        public static GameObject Root
        {
            get
            {
                if (_root == null)
                {
                    _root = new GameObject("[GRT Root]");
                    Object.DontDestroyOnLoad(_root);
                    Root.hideFlags = HideFlags.HideAndDontSave;
                    GRTInitializing?.Invoke();
                    GRTInitializing = null;
                    GRTInitialized?.Invoke();
                    GRTInitialized = null;
                }
                return _root;
            }
        }
        private static GameObject _root;

        /// <summary>
        /// 第一次实例化时调用
        /// </summary>
        public static event System.Action GRTInitializing;
        /// <summary>
        /// 第一次实例化时调用
        /// </summary>
        public static event System.Action GRTInitialized;

        /// <summary>
        /// 获取根物体上的某脚本
        /// </summary>
        /// <typeparam name="T">脚本类型</typeparam>
        public static T GetComponent<T>() where T : Component
        {
            return Root.GetComponent<T>();
        }
        /// <summary>
        /// 向根物体上附加某脚本
        /// </summary>
        /// <typeparam name="T">脚本类型</typeparam>
        public static T AddComponent<T>() where T :Component
        {
            return Root.AddComponent<T>();
        }
    }
}
