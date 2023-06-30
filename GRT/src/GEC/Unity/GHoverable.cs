using GRT.Events;
using UnityEngine;

namespace GRT.GEC.Unity
{
    public class GHoverable<T> : IGComponent<GameObject>, IBorrower<UEntity>
        where T : GHoverTrigger<T>
    {
        public IGEntity<GameObject> GEntity { get; set; }

        public ILender<UEntity> Lender { get; private set; }

        private T _trigger;

        public void Borrow(ILender<UEntity> lender)
        {
            Lender = lender;

            var collider = Lender.Wares.GetComponent<GameObject, GCollider>();
            if (collider != null)
            {
                _trigger = collider.Collider.gameObject.AddComponent<T>();
            }
        }

        public UEntity Return()
        {
            if (_trigger != null)
            {
                GHoverTrigger<T>.Destroy(_trigger);
            }

            var ware = Lender.Wares;
            Lender = null;
            return ware;
        }
    }

    public abstract class GHoverTrigger<T> : UComponent<GHoverable<T>>, IPointerEnter, IPointerExit, IPointerHover
        where T : GHoverTrigger<T>
    {
        public abstract void OnPointerEnter(Camera camera, RaycastHit hit, Vector2 pos);

        public abstract void OnPointerExit(Camera camera, RaycastHit hit, Vector2 pos);

        public abstract void OnPointerHover(Camera camera, RaycastHit hit, Vector2 pos);
    }
}