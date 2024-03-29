﻿using GRT.Events.Triggers;
using UnityEngine;

namespace GRT.Events
{
    public class MouseButtonTargetPointer : TargetPointer
    {
        public int mouseButton;

        public override bool Downing => Input.GetMouseButtonDown(mouseButton);

        public override bool Upping => Input.GetMouseButtonUp(mouseButton);

        public override bool Holding => Input.GetMouseButton(mouseButton);

        protected override bool IsInterestedIn(Component com)
        {
            return com is ITrigger trigger
                && trigger.HasInnerTrigger<MouseButtonTrigger>(out var inner)
                && inner.button == mouseButton;
        }
    }
}