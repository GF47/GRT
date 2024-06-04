using UnityEngine;

namespace GRT.GEvents.Pointers
{
    public class MouseButtonNoneTargetPointer<T> : NoneTargetPointer<T>
    {
        public int mouseButton;

        public override bool Downing => Input.GetMouseButtonDown(mouseButton);

        public override bool Upping => Input.GetMouseButtonUp(mouseButton);

        public override bool Holding => Input.GetMouseButton(mouseButton);
    }
}