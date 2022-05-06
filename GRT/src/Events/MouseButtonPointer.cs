using GRT.Events.Triggers;
using UnityEngine;

namespace GRT.Events
{
    public class MouseButtonPointer : BasePointer
    {
        public int mouseButton;

        public override bool Downing => Input.GetMouseButtonDown(mouseButton);

        public override bool Upping => Input.GetMouseButtonUp(mouseButton);

        public override bool Holding => Input.GetMouseButton(mouseButton);

        protected override bool IsInterestedIn(Component com)
        {
            var trigger = com as ITrigger;
            return trigger != null
                && trigger.HasInnerTrigger(typeof(MouseButtonTrigger), out var inner)
                && (inner as MouseButtonTrigger).button == mouseButton;
        }
    }
}