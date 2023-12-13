using GRT.Events.Triggers;
using UnityEngine;

namespace GRT.Events
{
    public class KeyTargetPointer : TargetPointer
    {
        public KeyCode key;

        public override bool Downing => Input.GetKeyDown(key);

        public override bool Upping => Input.GetKeyUp(key);

        public override bool Holding => Input.GetKey(key);

        protected override bool IsInterestedIn(Component com)
        {
            return com is ITrigger trigger
                && trigger.HasInnerTrigger<KeyTrigger>(out var inner)
                && inner.key == key;
        }
    }
}