using System;
using System.Collections.Generic;
using UnityEngine;

namespace GRT.GEC.Unity
{
    public class UEntity : IGEntity<GameObject>
    {
        public WeakReference<GameObject> Reference { get; private set; }

        public string Location { get; private set; }

        public IList<IGComponent<GameObject>> Components { get; } = new List<IGComponent<GameObject>>();

        public void Load()
        {
            GameObject go = Location.CanBeSplitBy(':', out var scene, out var path)
                ? GameObjectExtension.FindIn(scene, path)
                : GameObjectExtension.FindIn(UnityEngine.SceneManagement.SceneManager.GetActiveScene(), path);

            Reference = new WeakReference<GameObject>(go);

            foreach (var com in Components)
            {
                if (com is ILoadable<GameObject> loadable)
                {
                    loadable.Load(go);
                }
            }
        }

        // 想了想好像 unload 不太需要
    }
}