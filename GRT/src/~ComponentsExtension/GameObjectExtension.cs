namespace GRT
{
    using System;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public static class GameObjectExtension
    {
        public static (string, string) SplitRootAndSubPath(string path) =>
            path.CanBeSplitBy('/', out var left, out var right, true) ? (left, right) : (path, null);

        public static bool IsSameLocation(string locationA, string locationB)
        {
            var sceneASpecified = locationA.CanBeSplitBy(':', out var sceneA, out var pathA);
            var sceneBSpecified = locationB.CanBeSplitBy(':', out var sceneB, out var pathB);

            var activeScene = SceneManager.GetActiveScene().name;

            return pathA == pathB
                && (sceneA == sceneB
                    || (!sceneASpecified && sceneB == activeScene)
                    || (sceneA == activeScene && !sceneBSpecified));
        }

        public static GameObject FindByLocation(string location)
        {
            var scene = location.CanBeSplitBy(':', out var sceneName, out var path)
                ? SceneManager.GetSceneByName(sceneName)
                : SceneManager.GetActiveScene();
            return scene.Find(path);
        }

        public static GameObject Find(string sceneName, string path) => SceneManager.GetSceneByName(sceneName).Find(path);

        public static GameObject Find(this Scene scene, string path)
        {
            if (scene.IsValid())
            {
                var (rootName, subPath) = SplitRootAndSubPath(path);

                var rootGameObjects = scene.GetRootGameObjects();
                var root = Array.Find(rootGameObjects, go => go.name == rootName);
                if (root != null)
                {
                    if (string.IsNullOrEmpty(subPath))
                    {
                        return root;
                    }
                    else
                    {
                        var target = root.transform.Find(subPath);
                        if (target != null)
                        {
                            return target.gameObject;
                        }
                    }
                }
            }
            Debug.LogWarning($"there is not a game object called [{path}] in scene [{scene.name}]");
            return null;
        }

        /// <summary> ����Ŀ������������㼶
        /// </summary>
        public static string GetPath(this GameObject obj, bool addSceneName = false)
        {
            string path = obj.name;

            Transform t = obj.transform.parent;

            while (t != null)
            {
                path = $"{t.name}/{path}";
                t = t.parent;
            }
            return addSceneName ? $"{obj.scene.name}:{path}" : path;
        }

        /// <summary> ����Ŀ���������Բ㼶
        /// </summary>
        /// <param name="obj">Ŀ������</param>
        /// <param name="root">������</param>
        public static string GetRelativePath(this GameObject obj, GameObject root)
        {
            string path = obj.name;
            Transform t = obj.transform.parent;
            Transform rootTrans = root.transform;
            while (t != null && t != rootTrans)
            {
                path = $"{t.name}/{path}";
                t = t.parent;
            }
            return path;
        }

        /// <summary> ��Ŀ�����������һ��������
        /// </summary>
        /// <param name="parent">Ŀ������</param>
        /// <returns>������</returns>
        public static GameObject AddChild(this GameObject parent)
        {
            GameObject go = new GameObject();

            if (parent != null)
            {
                Transform t = go.transform;
                t.SetParent(parent.transform, false);
                // t.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                // t.localScale = Vector3.one;
                go.layer = parent.layer;
            }
            return go;
        }

        /// <summary> ��Ŀ�����������һ��ʵ������ĸ�����Ϊ������
        /// </summary>
        /// <param name="parent">Ŀ������</param>
        /// <param name="prefab">ʵ������</param>
        /// <returns>������</returns>
        public static GameObject AddChild(this GameObject parent, GameObject prefab)
        {
            GameObject go = UnityEngine.Object.Instantiate(prefab);

            if (go != null && parent != null)
            {
                Transform t = go.transform;
                t.SetParent(parent.transform, false);
                go.layer = parent.layer;
            }
            return go;
        }

        /// <summary> ��Ŀ�����������һ������ T �����������Ϊ������
        /// </summary>
        /// <typeparam name="T">�����</typeparam>
        /// <param name="parent">Ŀ������</param>
        /// <returns>�������T���ʵ��</returns>
        public static T AddChild<T>(this GameObject parent) where T : Component
        {
            GameObject go = AddChild(parent);
            go.name = typeof(T).Name;
            return go.AddComponent<T>();
        }

        /// <summary> �ڸ��㼶�ϲ�����Ŀ�������ϵ����� T ���
        /// </summary>
        /// <typeparam name="T">�����</typeparam>
        /// <param name="target">Ŀ������</param>
        /// <returns>���ҵ��� T ���</returns>
        public static T FindInParent<T>(this GameObject target) where T : Component
        {
            T com = null;

            var t = target == null ? null : target.transform;
            while (t != null && !t.gameObject.TryGetComponent(out com))
            {
                t = t.parent;
            }

            return com;
        }

        /// <summary> ��������Ĳ㼶������������
        /// </summary>
        /// <param name="target">Ŀ������</param>
        /// <param name="layer">�㼶</param>
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

        public static int StringToLayer(string str)
        {
            var layer = int.TryParse(str, out var layerOut) ? layerOut : LayerMask.NameToLayer(str);
            if (layer < 0 || 31 < layer)
            {
                layer = 0;
            }

            return layer;
        }

        public static Bounds WrapChildren(this GameObject go, bool isLocal = true)
        {
            var max = Vector3.negativeInfinity;
            var min = Vector3.positiveInfinity;

            var renderers = go.GetComponentsInChildren<Renderer>();
            if (renderers == null || renderers.Length == 0)
            {
                var colliders = go.GetComponentsInChildren<Collider>();
                if (colliders == null || colliders.Length == 0)
                {
                    throw new UnityException($"{go.GetPath(true)} has no renderer/collider, can not be wrapped");
                }
                else
                {
                    for (int i = 0; i < colliders.Length; i++)
                    {
                        max = Vector3.Max(max, colliders[i].bounds.max);
                        min = Vector3.Min(min, colliders[i].bounds.min);
                    }
                }
            }
            else
            {
                for (int i = 0; i < renderers.Length; i++)
                {
                    // bounds.Encapsulate(renderers[i].bounds);
                    max = Vector3.Max(max, renderers[i].bounds.max);
                    min = Vector3.Min(min, renderers[i].bounds.min);
                }
            }

            var matrix = go.transform.worldToLocalMatrix;

            var center = (max + min) / 2f;
            var size = (max - min);
            if (isLocal)
            {
                center = matrix.MultiplyPoint(center);
                size = matrix.MultiplyVector(size);
            }
            size = new Vector3(Mathf.Abs(size.x), Mathf.Abs(size.y), Mathf.Abs(size.z));

            return new Bounds(center, size);
        }

        public static T GetInterface<T>(this GameObject target) where T : class
        {
            Component[] coms = target.GetComponents<Component>();
            if (coms != null)
            {
                for (int i = 0; i < coms.Length; i++)
                {
                    if (coms[i] is T temp)
                    {
                        return temp;
                    }
                }
            }
            return null;
        }

        public static T[] GetInterfaces<T>(this GameObject target) where T : class
        {
            Component[] scripts = target.GetComponents<Component>();
            if (scripts != null)
            {
                T[] results = new T[scripts.Length];
                int n = 0;
                for (int i = 0; i < scripts.Length; i++)
                {
                    if (scripts[i] is T tmp)
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