using UnityEngine;

namespace GRT.Events
{
    public class KeyNoneTargetPointer : NoneTargetPointer
    {
        public KeyCode key;

        public override bool Downing => Input.GetKeyDown(key);

        public override bool Upping => Input.GetKeyUp(key);

        public override bool Holding => Input.GetKey(key);
    }
}