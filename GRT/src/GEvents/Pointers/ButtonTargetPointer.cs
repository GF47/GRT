using GRT.GEvents.Triggers;
using UnityEngine;

namespace GRT.GEvents.Pointers
{
    public class ButtonTargetPointer<T> : TargetPointer<T>
    {
        public string buttonName;

        public override bool Downing => Input.GetButtonDown(buttonName);

        public override bool Upping => Input.GetButtonUp(buttonName);

        public override bool Holding => Input.GetButton(buttonName);

        protected override bool IsInterestedIn(Component com)
        {
            return com is ITrigger<T> trigger
                && trigger.HasInnerTrigger<T, ButtonTrigger<T>>(out var inner)
                && inner.buttonName == buttonName;
        }
    }
}