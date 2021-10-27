using System;
using UnityEngine;

namespace GRT.Updater
{
    public class UpdateDriver : MonoBehaviour
    {
        public static void Init()
        {
            _instance = RootGameObject.AddComponent<UpdateDriver>();
            Debug.Log($"{nameof(UpdateDriver)} loaded on {_instance.name}");
        }

        private static UpdateDriver _instance;

        public static void Add(IUpdater updater) => _instance.Add_(updater);

        public static void Remove(IUpdater updater) => _instance.Remove_(updater);


        /******************************************************************/


        private Action<float> _frameUpdaters;
        private Action<float> _fixedFrameUpdaters;
        private Action<float> _afterFrameUpdaters;
        private Action<float> _customFrameUpdaters;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Debug.LogWarning($"do not init another {nameof(UpdateDriver)}");
                Destroy(this);
            }
        }

        private void Update()
        {
            _frameUpdaters?.Invoke(Time.deltaTime);
            _customFrameUpdaters?.Invoke(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            _fixedFrameUpdaters?.Invoke(Time.fixedDeltaTime);
        }

        private void LateUpdate()
        {
            _afterFrameUpdaters?.Invoke(Time.deltaTime);
        }

        private void Add_(IUpdater updater)
        {
            switch (updater.Type)
            {
                case UpdateType.Frame:
                    _frameUpdaters += updater.Update;
                    break;

                case UpdateType.FixedFrame:
                    _fixedFrameUpdaters += updater.Update;
                    break;

                case UpdateType.AfterFrame:
                    _afterFrameUpdaters += updater.Update;
                    break;

                case UpdateType.CustomFrame:
                    _customFrameUpdaters += updater.Update;
                    break;

                default:
                    _frameUpdaters += updater.Update;
                    break;
            }
        }

        private void Remove_(IUpdater updater)
        {
            switch (updater.Type)
            {
                case UpdateType.Frame:
                    _frameUpdaters -= updater.Update;
                    break;

                case UpdateType.FixedFrame:
                    _fixedFrameUpdaters -= updater.Update;
                    break;

                case UpdateType.AfterFrame:
                    _afterFrameUpdaters -= updater.Update;
                    break;

                case UpdateType.CustomFrame:
                    _customFrameUpdaters -= updater.Update;
                    break;

                default:
                    _frameUpdaters -= updater.Update;
                    break;
            }
        }
    }
}