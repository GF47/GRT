using UnityEngine;

namespace GRT.GTween
{
    public class TweenCameraFOV : GTween<float>
    {
        public override float From { get; set; }

        public override float To { get; set; }

        public Camera Camera { get; set; }

        public override float Interpolate(float percent)
        {
            Camera.fieldOfView = Mathf.Lerp(From, To, percent);
            return percent;
        }
    }
}