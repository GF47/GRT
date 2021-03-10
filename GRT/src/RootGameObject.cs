namespace GRT
{
    using UnityEngine;

    public static class RootGameObject
    {
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

        public static System.Action GRTInitializing;
        public static System.Action GRTInitialized;

        public static T GetComponent<T>() where T : Component
        {
            return Root.GetComponent<T>();
        }
        public static T AddComponent<T>() where T :Component
        {
            return Root.AddComponent<T>();
        }
    }
}
