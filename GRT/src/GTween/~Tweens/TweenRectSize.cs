using UnityEngine;

namespace GRT.GTween
{
    public class TweenRectSize : GTween<Vector2>
    {
        public override Vector2 From { get; set; }
        public override Vector2 To { get; set; }

        public RectTransform Target { get; }

        public override Vector2 Interpolate(float percent) => Target.sizeDelta = Vector2.Lerp(From, To, percent);
    }
}