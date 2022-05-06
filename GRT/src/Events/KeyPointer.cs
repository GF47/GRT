using GRT.Events.Triggers;
using UnityEngine;

namespace GRT.Events
{
    public class KeyPointer : BasePointer
    {
        public KeyCode key;

        public override bool Downing => Input.GetKeyDown(key);

        public override bool Upping => Input.GetKeyUp(key);

        public override bool Holding => Input.GetKey(key);

        protected override bool IsInterestedIn(Component com)
        {
            var trigger = com as ITrigger;
            return trigger != null
                && trigger.HasInnerTrigger(typeof(KeyTrigger), out var inner)
                && (inner as KeyTrigger).key == key;
        }
    }
}