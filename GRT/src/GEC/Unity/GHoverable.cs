using GRT.Events;
using UnityEngine;

namespace GRT.GEC.Unity
{
    public class GHoverable<T> : IGComponent<GameObject>, IBorrower<UEntity>
        where T : GHoverTrigger<T>
    {
        public IGEntity<GameObject> GEntity { get; set; }

        public ILender<UEntity> Lender { get; private set; }

        public T Trigger { get; protected set; }

        public bool Borrow(ILender<UEntity> lender)
        {
            var collider = lender.Wares.GetComponent<GameObject, GCollider>();
            var deal = collider != null;

            if (deal)
            {
                Lender = lender;

                Trigger = collider.Collider.gameObject.AddComponent<T>();
                Trigger.Connect(this);
            }

            return deal;
        }

        public void Repay()
        {
            if (Trigger != null)
            {
                GHoverTrigger<T>.Destroy(Trigger);
            }

            Lender = null;
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