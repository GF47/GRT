using GRT.Events;
using System;
using UnityEngine;

namespace GRT.GEC.Unity
{
    public class GHoverable<T> : IGComponent<GameObject>, ILoadable<GameObject>
        where T : GHoverTrigger<T>
    {
        public IGEntity<GameObject> GEntity { get; set; }

        private WeakReference<T> _triggerRef;

        public void Load(GameObject target)
        {
            var collider = GEntity.GetComponent<GameObject, GCollider>();
            if (collider != null)
            {
                var trigger = collider.Collider.gameObject.AddComponent<T>();

                _triggerRef = new WeakReference<T>(trigger);
            }
        }

        public GameObject Unload()
        {
            if (_triggerRef != null && _triggerRef.TryGetTarget(out var trigger))
            {
                var go = trigger.gameObject;
                UnityEngine.Object.Destroy(trigger);
                return go;
            }
            else
            {
                return null;
            }
        }
    }

    public abstract class GHoverTrigger<T> : UComponent<GameObject, GHoverable<T>>, IPointerEnter, IPointerExit, IPointerHover
        where T : GHoverTrigger<T>
    {
        public abstract void OnPointerEnter(Camera camera, RaycastHit hit, Vector2 pos);

        public abstract void OnPointerExit(Camera camera, RaycastHit hit, Vector2 pos);

        public abstract void OnPointerHover(Camera camera, RaycastHit hit, Vector2 pos);
    }
}