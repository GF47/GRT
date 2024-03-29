﻿using GRT.Events.Triggers;
using UnityEngine;

namespace GRT.GEC.Unity
{
    public abstract class UClickable : IGComponent<GameObject, UEntity>, IConsumer<UEntity>
    {
        public UEntity Entity { get; set; }

        public IProvider<UEntity> Provider { get; private set; }

        private PointerClickTrigger _trigger;

        public bool Use(IProvider<UEntity> provider)
        {
            if (provider.Ware != null && provider.Ware.TryGetComponent(out UCollider collider))
            {
                Provider = provider;

                _trigger = collider.RawCollider.gameObject.AddComponent<PointerClickTrigger>();
                _trigger.InnerTrigger = new MouseButtonTrigger() { button = 0 };
                _trigger.Event.AddListener(OnClick);
                return true;
            }
            return false;
        }

        public void Release()
        {
            if (_trigger != null)
            {
                _trigger.Event.RemoveListener(OnClick);
                PointerClickTrigger.Destroy(_trigger);
            }

            Provider = null;
        }

        public abstract void OnClick(Camera camera, RaycastHit hit, Vector2 position);
    }
}