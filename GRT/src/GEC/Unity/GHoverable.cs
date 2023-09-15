using GRT.Events;
using UnityEngine;

namespace GRT.GEC.Unity
{
    public class GHoverable<T> : IGComponent<GameObject, UEntity>, IConsumer<UEntity>
        where T : GHoverTrigger<T>
    {
        public UEntity GEntity { get; set; }

        public T Trigger { get; protected set; }

        public IProvider<UEntity> Provider { get; private set; }

        public bool Use(IProvider<UEntity> provider)
        {
            if (provider.Ware != null & provider.Ware.TryGetComponent(out GCollider collider))
            {
                Provider = provider;

                Trigger = collider.Collider.gameObject.AddComponent<T>();
                Trigger.Connect(this);
                return true;
            }
            return false;
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