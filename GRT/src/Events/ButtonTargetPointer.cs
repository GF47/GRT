using GRT.Events.Triggers;
using UnityEngine;

namespace GRT.Events
{
    public class ButtonTargetPointer : TargetPointer
    {
        public string buttonName;

        public override bool Downing => Input.GetButtonDown(buttonName);

        public override bool Upping => Input.GetButtonUp(buttonName);

        public override bool Holding => Input.GetButton(buttonName);

        protected override bool IsInterestedIn(Component com)
        {
            var trigger = com as ITrigger;
            return trigger != null
                && trigger.HasInnerTrigger<ButtonTrigger>(out var inner)
                && inner.buttonName == buttonName;
        }
    }
}