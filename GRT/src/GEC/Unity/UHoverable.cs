using GRT.Events;
using UnityEngine;

namespace GRT.GEC.Unity
{
    public class UHoverable<T> : IGComponent<GameObject, UEntity>, IConsumer<UEntity>
        where T : UHoverTrigger<T>
    {
        public UEntity Entity { get; set; }

        public T Trigger { get; protected set; }

        public IProvider<UEntity> Provider { get; private set; }

        public bool Use(IProvider<UEntity> provider)
        {
            if (provider.Ware != null && provider.Ware.TryGetComponent(out UCollider collider))
            {
                Provider = provider;

                Trigger = collider.RawCollider.gameObject.AddComponent<T>();
                Trigger.Connect(this);
                return true;
            }
            return false;
        }

        public void Release()
        {
            if (Trigger != null)
            {
                UHoverTrigger<T>.Destroy(Trigger);
            }

            Provider = null;
        }
    }

    public abstract class UHoverTrigger<T> : UBehaviour<UHoverable<T>>, IPointerEnter, IPointerExit, IPointerHover
        where T : UHoverTrigger<T>
    {
        public abstract void OnPointerEnter(Camera camera, RaycastHit hit, Vector2 pos);

        public abstract void OnPointerExit(Camera camera, RaycastHit hit, Vector2 pos);

        public abstract void OnPointerHover(Camera camera, RaycastHit hit, Vector2 pos);
    }
}