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
                }
                return _root;
            }
        }
        private static GameObject _root;

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
