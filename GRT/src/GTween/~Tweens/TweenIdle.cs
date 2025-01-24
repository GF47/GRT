using UnityEngine;

namespace GRT.GTween
{
    public class TweenIdle : GTween<float>
    {
        public override float From { get => 0f; set { } }

        public override float To { get; set; }

        public override float Interpolate(float percent)
        {
            return Mathf.Lerp(From, To, percent);
        }
    }
}