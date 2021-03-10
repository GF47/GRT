namespace GRT.Updater
{
    using System.Collections.Generic;
    using UnityEngine;

    public class MonoUpdater : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Init()
        {
            RootGameObject.GRTInitializing += () =>
            {
                _instance = RootGameObject.AddComponent<MonoUpdater>();
                Debug.Log($"{nameof(MonoUpdater)} loaded on {_instance.name}");
            };
        }
        private static MonoUpdater _instance;

        private static readonly Dictionary<long, IUpdateNode> _perFrameDict = new Dictionary<long, IUpdateNode>();
        private static readonly Dictionary<long, IUpdateNode> _perFixedFrameDict = new Dictionary<long, IUpdateNode>();
        private static readonly Dictionary<long, IUpdateNode> _perAfterFrameDict = new Dictionary<long, IUpdateNode>();
        private static readonly Dictionary<long, IUpdateNode> _perCustomFrameDict = new Dictionary<long, IUpdateNode>();

        private static readonly List<(bool, Dictionary<long, IUpdateNode>, IUpdateNode)> _cache = new List<(bool, Dictionary<long, IUpdateNode>, IUpdateNode)>();

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
                    if (isAdd) { dict.Add(node.ID, node); }
                    else { dict.Remove(node.ID); }
                }
                _cache.Clear();
            }

            foreach (var item in _perFrameDict)
            {
                item.Value.Update(Time.deltaTime);
            }

            foreach (var item in _perCustomFrameDict)
            {
                item.Value.Update(Time.deltaTime);
            }
        }

        private void FixedUpdate()
        {
            foreach (var item in _perFixedFrameDict)
            {
                item.Value.Update(Time.fixedDeltaTime);
            }
        }

        private void LateUpdate()
        {
            foreach (var item in _perAfterFrameDict)
            {
                item.Value.Update(Time.deltaTime);
            }
        }

        public static void Add(IUpdateNode node)
        {
            Dictionary<long, IUpdateNode> dict;
            switch (node.Type)
            {
                case UpdateType.PerFixedFrame:
                    dict = _perFixedFrameDict;
                    break;
                case UpdateType.PerAfterFrame:
                    dict = _perAfterFrameDict;
                    break;
                case UpdateType.PerCustomFrame:
                    dict = _perCustomFrameDict;
                    break;
                case UpdateType.PerFrame:
                default:
                    dict = _perFrameDict;
                    break;
            }

            if (!dict.ContainsKey(node.ID))
            {
                _cache.Add((true, dict, node));
            }
        }

        public static void Remove(IUpdateNode node)
        {
            Dictionary<long, IUpdateNode> dict;
            switch (node.Type)
            {
                case UpdateType.PerFixedFrame:
                    dict = _perFixedFrameDict;
                    break;
                case UpdateType.PerAfterFrame:
                    dict = _perAfterFrameDict;
                    break;
                case UpdateType.PerCustomFrame:
                    dict = _perCustomFrameDict;
                    break;
                case UpdateType.PerFrame:
                default:
                    dict = _perFrameDict;
                    break;
            }
            if (dict.ContainsKey(node.ID))
            {
                _cache.Add((false, dict, node));
            }
        }
    }
}
