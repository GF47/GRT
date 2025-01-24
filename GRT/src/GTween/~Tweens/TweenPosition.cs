using UnityEngine;

namespace GRT.GTween
{
    public class TweenPosition : GTween<Vector3>
    {
        public override Vector3 From { get; set; }
        public override Vector3 To { get; set; }

        public Transform Target { get; set; }

        public bool IsLocal { get; set; }

        public override Vector3 Interpolate(float percent)
        {
            var pos = Vector3.Lerp(From, To, percent);
            if (IsLocal)
            {
                Target.localPosition = pos;
            }
            else
            {
                Target.position = pos;
            }
            return pos;
        }
    }
}