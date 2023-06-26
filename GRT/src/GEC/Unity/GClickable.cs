using GRT.Events.Triggers;
using System;
using UnityEngine;

namespace GRT.GEC.Unity
{
    public abstract class GClickable : IGComponent<GameObject>, ILoadable<GameObject>
    {
        public IGEntity<GameObject> GEntity { get; set; }

        private WeakReference<PointerClickTrigger> _triggerRef;

        public void Load(GameObject target)
        {
            var collider = GEntity.GetComponent<GameObject, GCollider>();
            if (collider != null)
            {
                var trigger = collider.Collider.gameObject.AddComponent<PointerClickTrigger>();
                trigger.InnerTrigger = new MouseButtonTrigger() { button = 0 };
                trigger.Event.AddListener(OnClick);

                _triggerRef = new WeakReference<PointerClickTrigger>(trigger);
            }
        }

        public GameObject Unload()
        {
            if (_triggerRef != null && _triggerRef.TryGetTarget(out var trigger))
            {
                var go = trigger.gameObject;
                trigger.Event.RemoveListener(OnClick);
                UnityEngine.Object.Destroy(trigger);
                return go;
            }
            else
            {
                return null;
            }
        }

        public abstract void OnClick(Camera camera, RaycastHit hit, Vector2 position);
    }
}