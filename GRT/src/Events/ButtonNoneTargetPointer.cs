using UnityEngine;

namespace GRT.Events
{
    public class ButtonNoneTargetPointer : NoneTargetPointer
    {
        public string buttonName;

        public override bool Downing => Input.GetButtonDown(buttonName);

        public override bool Upping => Input.GetButtonUp(buttonName);

        public override bool Holding => Input.GetButton(buttonName);
    }
}