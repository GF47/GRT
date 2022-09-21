using UnityEngine;

namespace GRT.Events
{
    public class MouseButtonNoneTargetPointer : NoneTargetPointer
    {
        public int mouseButton;

        public override bool Downing => Input.GetMouseButtonDown(mouseButton);

        public override bool Upping => Input.GetMouseButtonUp(mouseButton);

        public override bool Holding => Input.GetMouseButton(mouseButton);
    }
}