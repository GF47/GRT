using GRT.GEvents.Triggers;
using UnityEngine;

namespace GRT.GEC.Unity
{
    public abstract class UClickable<T> : IGComponent<GameObject, UEntity>, IConsumer<UEntity>
    {
        public UEntity Entity { get; set; }

        public IProvider<UEntity> Provider { get; private set; }

        private PointerClickTrigger<T> _trigger;

        public bool Use(IProvider<UEntity> provider)
        {
            if (provider.Ware != null && provider.Ware.TryGetComponent(out UCollider collider))
            {
                Provider = provider;

                _trigger = AddTrigger(collider.RawCollider.gameObject);
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
                PointerClickTrigger<T>.Destroy(_trigger);
            }

            Provider = null;
        }

        public abstract void OnClick(T sender, RaycastHit hit);

        public abstract PointerClickTrigger<T> AddTrigger(GameObject go);
    }
}