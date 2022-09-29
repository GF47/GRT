using UnityEngine;

namespace GRT.GInventory
{
    public class BaseWorldObject : MonoBehaviour, IWorldObject<BaseWorldObject>
    {
        private IStack _stack;

        public IStack Stack
        {
            get => _stack; set
            {
                if (_stack != value)
                {
                    _stack = value;

                    _stack.PickingUp += OnStackPickUp;
                    _stack.Destroying += OnStackDestroy;
                }
            }
        }

        public void OnStackPickUp(IOwner owner, IStack stack)
        {
            _stack.PickingUp -= OnStackPickUp;
            _stack.Destroying -= OnStackDestroy;

            Destroy(gameObject);
        }

        public void OnStackDestroy(IStack stack)
        {
            _stack.PickingUp -= OnStackPickUp;
            _stack.Destroying -= OnStackDestroy;

            Destroy(gameObject);
        }
    }
}