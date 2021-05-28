// TODO 考虑将持续时间很短的node单独存放，node自身去标记持续时间的长短
namespace GRT.Updater
{
    using System.Collections.Generic;
    using UnityEngine;

    public class MonoUpdater : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            RootGameObject.GRTInitializing += () =>
            {
                _instance = RootGameObject.AddComponent<MonoUpdater>();
                Debug.Log($"{nameof(MonoUpdater)} loaded on {_instance.name}");
            };
        }

        private static MonoUpdater _instance;

        private static readonly List<IUpdateNode> _perFrameList = new List<IUpdateNode>();
        private static readonly List<IUpdateNode> _perFixedFrameList = new List<IUpdateNode>();
        private static readonly List<IUpdateNode> _perAfterFrameList = new List<IUpdateNode>();
        private static readonly List<IUpdateNode> _perCustomFrameList = new List<IUpdateNode>();

        private static readonly List<(bool, List<IUpdateNode>, IUpdateNode)> _cache = new List<(bool, List<IUpdateNode>, IUpdateNode)>();

        private void Awake()
        {
            if (_instance != null && _instance != this) // 防止多个实例
            {
                Debug.LogWarning("Do not init another MonoUpdater");
                Destroy(this);
            }
        }

        private void Update()
        {
            if (_cache.Count > 0)
            {
                for (int i = 0; i < _cache.Count; i++)
                {
                    var (isAdd, dict, node) = _cache[i];
                    if (isAdd) { dict.Add(node); }
                    else { dict.Remove(node); }
                }
                _cache.Clear();
            }

            foreach (var item in _perFrameList) { item.Update(Time.deltaTime); }

            foreach (var item in _perCustomFrameList) { item.Update(Time.deltaTime); }
        }

        private void FixedUpdate()
        {
            foreach (var item in _perFixedFrameList) { item.Update(Time.fixedDeltaTime); }
        }

        private void LateUpdate()
        {
            foreach (var item in _perAfterFrameList) { item.Update(Time.deltaTime); }
        }

        public static bool Add(IUpdateNode node)
        {
            List<IUpdateNode> list;
            switch (node.Type)
            {
                case UpdateType.PerFixedFrame:
                    list = _perFixedFrameList;
                    break;

                case UpdateType.PerAfterFrame:
                    list = _perAfterFrameList;
                    break;

                case UpdateType.PerCustomFrame:
                    list = _perCustomFrameList;
                    break;

                case UpdateType.PerFrame:
                default:
                    list = _perFrameList;
                    break;
            }

            if (!list.Contains(node))
            {
                if (_cache.FindIndex(n => n.Item1 == true && n.Item2 == list && n.Item3 == node) < 0)
                {
                    _cache.Add((true, list, node));
                }
            }
            return true;
        }

        public static bool Remove(IUpdateNode node)
        {
            List<IUpdateNode> list;
            switch (node.Type)
            {
                case UpdateType.PerFixedFrame:
                    list = _perFixedFrameList;
                    break;

                case UpdateType.PerAfterFrame:
                    list = _perAfterFrameList;
                    break;

                case UpdateType.PerCustomFrame:
                    list = _perCustomFrameList;
                    break;

                case UpdateType.PerFrame:
                default:
                    list = _perFrameList;
                    break;
            }
            if (list.Contains(node))
            {
                if (_cache.FindIndex(n => n.Item1 == false && n.Item2 == list && n.Item3 == node) < 0)
                {
                    _cache.Add((false, list, node));
                }
            }
            return false;
        }
    }
}