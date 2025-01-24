using GRT.Geometry;
using UnityEngine;

namespace GRT.GTween
{
    public class TweenBezierSplinePath : GTween<float>
    {
        public override float From { get => 0f; set { } }

        public override float To { get => 1f; set { } }

        public BezierSpline BSplineAsset { get; set; }
        public Transform Target { get; set; }
        public bool UseDirection { get; set; }

        public override float Interpolate(float percent)
        {
            var result = BSplineAsset.GetResult(percent);
            Target.position = result.position;
            if (UseDirection)
            {
                Target.forward = result.Direction;
            }

            return percent;
        }
    }
}