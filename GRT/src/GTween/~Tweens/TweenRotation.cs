using UnityEngine;

namespace GRT.GTween
{
    public class TweenRotation : GTween<Quaternion>
    {
        public override Quaternion From { get; set; }

        public override Quaternion To { get; set; }

        public Transform Target { get; set; }

        public bool IsLocal { get; set; }

        public override Quaternion Interpolate(float percent)
        {
            var rot = Quaternion.Slerp(From, To, percent);

            if (IsLocal)
            {
                Target.localRotation = rot;
            }
            else
            {
                Target.rotation = rot;
            }

            return rot;
        }
    }
}