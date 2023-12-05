using System;
using UnityEngine;

namespace GRT.GUpdater
{
    public class GUpdaterDriver : MonoBehaviour
    {
        public static void Init(GameObject root)
        {
            _instance = root.AddComponent<GUpdaterDriver>();
        }

        private static GUpdaterDriver _instance;

        public static void Add(IGUpdater updater)
        {
            switch (updater.UpdateMode)
            {
                case UpdateMode.PerFixedFrame: _instance._perFixedFrameActions += updater.Update; return;
                case UpdateMode.PerAfterFrame: _instance._perAfterFrameActions += updater.Update; return;
                case UpdateMode.CustomInterval: _instance._perCustomIntervalActions += updater.Update; return;
                case UpdateMode.PerFrame: default: _instance._perFrameActions += updater.Update; return;
            }
        }

        public static void Remove(IGUpdater updater)
        {
            switch (updater.UpdateMode)
            {
                case UpdateMode.PerFixedFrame: _instance._perFixedFrameActions -= updater.Update; return;
                case UpdateMode.PerAfterFrame: _instance._perAfterFrameActions -= updater.Update; return;
                case UpdateMode.CustomInterval: _instance._perCustomIntervalActions -= updater.Update; return;
                case UpdateMode.PerFrame: default: _instance._perFrameActions -= updater.Update; return;
            }
        }

        /**************************************************************/

        private Action<float> _perFrameActions;
        private Action<float> _perFixedFrameActions;
        private Action<float> _perAfterFrameActions;
        private Action<float> _perCustomIntervalActions;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Debug.LogWarning($"another {nameof(GUpdaterDriver)} already exists, this will be deleted automatically");
                Destroy(this);
            }
        }

        private void Update()
        {
            var delta = Time.deltaTime;
            _perFrameActions?.Invoke(delta);
            _perCustomIntervalActions?.Invoke(delta);
        }

        private void FixedUpdate()
        {
            _perFixedFrameActions?.Invoke(Time.fixedDeltaTime);
        }

        private void LateUpdate()
        {
            _perAfterFrameActions?.Invoke(Time.deltaTime);
        }
    }
}