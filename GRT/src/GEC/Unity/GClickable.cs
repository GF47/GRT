using GRT.Events.Triggers;
using UnityEngine;

namespace GRT.GEC.Unity
{
    public abstract class GClickable : IGComponent<GameObject>, IBorrower<UEntity>
    {
        public IGEntity<GameObject> GEntity { get; set; }

        public ILender<UEntity> Lender { get; private set; }

        private PointerClickTrigger _trigger;

        public bool Borrow(ILender<UEntity> lender)
        {
            var collider = lender.Wares.GetComponent<GameObject, GCollider>();
            var deal = collider != null;
            if (deal)
            {
                Lender = lender;

                _trigger = collider.Collider.gameObject.AddComponent<PointerClickTrigger>();
                _trigger.InnerTrigger = new MouseButtonTrigger() { button = 0 };
                _trigger.Event.AddListener(OnClick);
            }

            return deal;
        }

        public void Return()
        {
            if (_trigger != null)
            {
                _trigger.Event.RemoveListener(OnClick);
                PointerClickTrigger.Destroy(_trigger);
            }

            Lender = null;
        }

        public abstract void OnClick(Camera camera, RaycastHit hit, Vector2 position);
    }
}