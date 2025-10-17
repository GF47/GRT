using GRT.GEvents;
using UnityEngine;

namespace GRT.GEC.Unity
{
    public class UHoverable<T, TS> : IGComponent<GameObject, UEntity>, IConsumer<UEntity>
        where T : UHoverTrigger<T, TS>
    {
        public UEntity Entity { get; set; }

        public T Trigger { get; protected set; }

        public IProvider<UEntity> Provider { get; private set; }

        public bool Use(IProvider<UEntity> provider)
        {
            if (provider.Ware != null && provider.Ware.TryGetComponent(out UCollider collider))
            {
                Provider = provider;

                Trigger = collider.RawCollider.GetRealGameObject().AddComponent<T>();
                Trigger.Connect(this);
                return true;
            }
            return false;
        }

        public void Release()
        {
            if (Trigger != null)
            {
                UHoverTrigger<T, TS>.Destroy(Trigger);
            }

            Provider = null;
        }
    }

    public abstract class UHoverTrigger<T, TS> : UBehaviour<UHoverable<T, TS>>, IGPointerEnter<TS>, IGPointerExit<TS>, IGPointerStay<TS>
        where T : UHoverTrigger<T, TS>
    {
        public abstract void OnPointerEnter(TS sender, RaycastHit hit);

        public abstract void OnPointerExit(TS sender, RaycastHit hit);

        public abstract void OnPointerStay(TS sender, RaycastHit hit);
    }
}