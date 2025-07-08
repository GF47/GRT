using GRT.GEvents.Triggers;
using UnityEngine;
using UComponent = UnityEngine.Component;

namespace GRT.GEC.Unity
{
    public abstract class UDraggable<T> : IGComponent<GameObject, UEntity>, IConsumer<UEntity>
    {
        public IProvider<UEntity> Provider { get; private set; }

        public UEntity Entity { get; set; }

        private ITrigger<T> _dragTrigger;
        private ITrigger<T> _dragStartTrigger;
        private ITrigger<T> _dragStopTrigger;

        public virtual bool Use(IProvider<UEntity> provider)
        {
            if (provider.Ware != null && provider.Ware.TryGetComponent(out UCollider collider))
            {
                Provider = provider;

                (_dragTrigger, _dragStartTrigger, _dragStopTrigger) = AddTriggers(collider.RawCollider.gameObject);
                if (_dragTrigger != null) { _dragTrigger.Event.AddListener(OnDrag); }
                if (_dragStartTrigger != null) { _dragStartTrigger.Event.AddListener(OnDragStart); }
                if (_dragStopTrigger != null) { _dragStopTrigger.Event.AddListener(OnDragStop); }

                return true;
            }

            return false;
        }

        public virtual void Release()
        {
            if (_dragTrigger != null)
            {
                _dragTrigger.Event.RemoveListener(OnDrag);
                if (_dragTrigger is UComponent com)
                {
                    UComponent.Destroy(com);
                }
            }

            if (_dragStartTrigger != null)
            {
                _dragStartTrigger.Event.RemoveListener(OnDragStart);
                if (_dragStartTrigger is UComponent com)
                {
                    UComponent.Destroy(com);
                }
            }

            if (_dragStopTrigger != null)
            {
                _dragStopTrigger.Event.RemoveListener(OnDragStop);
                if (_dragStopTrigger is UComponent com)
                {
                    UComponent.Destroy(com);
                }
            }

            Provider = null;
        }

        public abstract void OnDrag(T sender, RaycastHit hit);

        public abstract void OnDragStart(T sender, RaycastHit hit);

        public abstract void OnDragStop(T sender, RaycastHit hit);

        public abstract (ITrigger<T>, ITrigger<T>, ITrigger<T>) AddTriggers(GameObject go);
    }
}