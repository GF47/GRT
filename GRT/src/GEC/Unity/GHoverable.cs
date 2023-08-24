using GRT.Events;
using UnityEngine;

namespace GRT.GEC.Unity
{
    public class GHoverable<T> : IGComponent<GameObject>, IUser<GameObject>
        where T : GHoverTrigger<T>
    {
        public IGEntity<GameObject> GEntity { get; set; }

        public T Trigger { get; protected set; }

        public IProvider<GameObject> Provider { get; private set; }

        public bool Use(IProvider<GameObject> provider)
        {
            var collider = (provider as IGEntity<GameObject>)?.GetComponent<GameObject, GCollider>();
            var deal = collider != null;

            if (deal)
            {
                Provider = provider;

                Trigger = collider.Collider.gameObject.AddComponent<T>();
                Trigger.Connect(this);
            }

            return deal;
        }

        public void Release()
        {
            if (Trigger != null)
            {
                GHoverTrigger<T>.Destroy(Trigger);
            }

            Provider = null;
        }
    }

    public abstract class GHoverTrigger<T> : UBehaviour<GHoverable<T>>, IPointerEnter, IPointerExit, IPointerHover
        where T : GHoverTrigger<T>
    {
        public abstract void OnPointerEnter(Camera camera, RaycastHit hit, Vector2 pos);

        public abstract void OnPointerExit(Camera camera, RaycastHit hit, Vector2 pos);

        public abstract void OnPointerHover(Camera camera, RaycastHit hit, Vector2 pos);
    }
}