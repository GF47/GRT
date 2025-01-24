using UnityEngine;

namespace GRT.GTween
{
    public class TweenLocalScale : GTween<Vector3>
    {
        public override Vector3 From { get; set; }

        public override Vector3 To { get; set; }

        public Transform Target { get; set; }

        public override Vector3 Interpolate(float percent)
        {
            Target.localScale = Vector3.Lerp(From, To, percent);
            return Target.localScale;
        }
    }
}