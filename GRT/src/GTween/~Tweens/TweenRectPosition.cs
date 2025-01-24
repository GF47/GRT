using UnityEngine;

namespace GRT.GTween
{
    public class TweenRectPosition : GTween<Vector3>
    {
        public override Vector3 From { get; set; }

        public override Vector3 To { get; set; }

        public RectTransform Target { get; set; }

        public override Vector3 Interpolate(float percent) => Target.anchoredPosition3D = Vector3.Lerp(From, To, percent);
    }
}