namespace GRT
{
    using System;
    using System.Collections.Generic;
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
                for (int i = index.Count - 1; i > -1; i--)
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

        public static Transform GetChildByIndexList(this Transform root, bool isChildToRoot, params int[] index)
        {
            return root.GetChildByIndexList(index, isChildToRoot);
        }

        public static Transform[] FindChildren(this Transform root, Predicate<Transform> predicate = null)
        {
            var children = new List<Transform>(root.childCount);
            foreach (Transform child in root)
            {
                if (predicate == null || predicate.Invoke(child))
                {
                    children.Add(child);
                }
            }

            return children.ToArray();
        }

        public static Transform FindChildExt(this Transform root, string path, char splitChar = '/', char serialChar = ':')
        {
            var split = path.Split(splitChar);
            var child = root;
            for (int i = 0; i < split.Length; i++)
            {
                child = child.FindChildAvoidSameName(split[i], serialChar);
            }
            return child;
        }

        public static Transform FindChildAvoidSameName(this Transform root, string fullName, char serialChar = ':')
        {
            var k = fullName.LastIndexOf(serialChar);
            if (k > -1)
            {
                var name = fullName.Substring(0, k);
                var n = int.Parse(fullName.Substring(k + 1));

                var m = -1;
                foreach (Transform t in root)
                {
                    if (t.name == name)
                    {
                        m++;
                        if (m == n) { return t; }
                    }
                }
            }
            else
            {
                return root.Find(fullName);
            }

            return null;
        }
    }
}