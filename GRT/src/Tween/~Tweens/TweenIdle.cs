using UnityEngine;

namespace GRT.Tween
{
    public class TweenIdle : Tween<float>
    {
        public override float From { get => 0f; set => from = 0f; }
        public override float To { get => to; set => to = value; }
        public override float Current { get; protected set; }

        public override float Project(float percent)
        {
            Current = Mathf.Lerp(From, To, percent);
            return Current;
        }
    }
}