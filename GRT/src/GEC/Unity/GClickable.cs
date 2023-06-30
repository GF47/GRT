using GRT.Events.Triggers;
using UnityEngine;

namespace GRT.GEC.Unity
{
    public abstract class GClickable : IGComponent<GameObject>, IBorrower<UEntity>
    {
        public IGEntity<GameObject> GEntity { get; set; }

        public ILender<UEntity> Lender { get; private set; }

        private PointerClickTrigger _trigger;

        public void Borrow(ILender<UEntity> lender)
        {
            Lender = lender;

            var collider = Lender.Wares.GetComponent<GameObject, GCollider>();
            if (collider != null)
            {
                _trigger = collider.Collider.gameObject.AddComponent<PointerClickTrigger>();
                _trigger.InnerTrigger = new MouseButtonTrigger() { button = 0 };
                _trigger.Event.AddListener(OnClick);
            }
        }

        public UEntity Return()
        {
            if (_trigger != null)
            {
                _trigger.Event.RemoveListener(OnClick);
                PointerClickTrigger.Destroy(_trigger);
            }

            var wares = Lender.Wares;
            Lender = null;
            return wares;
        }

        public abstract void OnClick(Camera camera, RaycastHit hit, Vector2 position);
    }
}