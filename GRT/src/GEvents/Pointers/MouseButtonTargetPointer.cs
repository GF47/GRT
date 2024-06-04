using GRT.GEvents.Triggers;
using UnityEngine;

namespace GRT.GEvents.Pointers
{
    public class MouseButtonTargetPointer<T> : TargetPointer<T>
    {
        public int mouseButton;

        public override bool Downing => Input.GetMouseButtonDown(mouseButton);

        public override bool Upping => Input.GetMouseButtonUp(mouseButton);

        public override bool Holding => Input.GetMouseButton(mouseButton);

        protected override bool IsInterestedIn(Component com)
        {
            return com is ITrigger<T> trigger
                && trigger.HasInnerTrigger<T, MouseButtonTrigger<T>>(out var inner)
                && inner.button == mouseButton;
        }
    }
}