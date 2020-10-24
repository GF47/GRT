using System.Collections.Generic;

namespace GRT
{
    using UnityEngine;

    public static class TransformExtension
    {
        /// <summary> 判断 child 是否是 parent 的子物体
        /// </summary>
        /// <param name="parent">假定的父物体</param>
        /// <param name="child">假定的子物体</param>
        /// <returns>是否是子物体</returns>
        public static bool IsTheParentOf(this Transform parent, Transform child)
        {
            if (parent == null || child == null) return false;

            while (child != null)
            {
                if (child == parent) return true;
                child = child.parent;
            }
            return false;
        }

        public static List<int> GetIndexRelativeTo(this Transform t, Transform root, bool isChildToRoot = false)
        {
            List<int> list = new List<int>();
            Transform parent = null;
            while (parent != root)
            {
                parent = t.parent;
                list.Add(t.GetSiblingIndex());
                t = parent;
            }
            if (!isChildToRoot)
            {
                list.Reverse();
            }
            return list;
        }

        public static Transform GetChildByIndexList(this Transform root, IList<int> index, bool isChildToRoot = false)
        {
            if (isChildToRoot)
            {
                for (int i = index.Count -1; i > -1; i--)
                {
                    root = root.GetChild(index[i]);
                }
            }
            else
            {
                for (int i = 0; i < index.Count; i++)
                {
                    root = root.GetChild(index[i]);
                }
            }
            return root;
        }
    }
}