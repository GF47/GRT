using UnityEngine;

namespace GRT.GEvents.Pointers
{
    public class ButtonNoneTargetPointer<T> : NoneTargetPointer<T>
    {
        public string buttonName;

        public override bool Downing => Input.GetButtonDown(buttonName);

        public override bool Upping => Input.GetButtonUp(buttonName);

        public override bool Holding => Input.GetButton(buttonName);
    }
}