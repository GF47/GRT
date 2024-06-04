using UnityEngine;

namespace GRT.GEvents.Pointers
{
    public class KeyNoneTargetPointer<T> : NoneTargetPointer<T>
    {
        public KeyCode key;

        public override bool Downing => Input.GetKeyDown(key);

        public override bool Upping => Input.GetKeyUp(key);

        public override bool Holding => Input.GetKey(key);
    }
}