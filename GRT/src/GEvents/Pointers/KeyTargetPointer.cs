using GRT.GEvents.Triggers;
using UnityEngine;

namespace GRT.GEvents.Pointers
{
    public class KeyTargetPointer<T> : TargetPointer<T>
    {
        public KeyCode key;

        public override bool Downing => Input.GetKeyDown(key);

        public override bool Upping => Input.GetKeyUp(key);

        public override bool Holding => Input.GetKey(key);

        protected override bool IsInterestedIn(Component com)
        {
            return com is ITrigger<T> trigger
                && trigger.HasInnerTrigger<T, KeyTrigger<T>>(out var inner)
                && inner.key == key;
        }
    }
}