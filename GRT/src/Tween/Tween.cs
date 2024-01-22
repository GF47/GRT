using UnityEngine;

namespace GRT.Tween
{
    public abstract class Tween<T> : MonoBehaviour, IInterpolable<T>, IPercent
    {
        [SerializeField] protected T from;
        [SerializeField] protected T to;

        private float _percent;

        public virtual T From { get => from; set => from = value; }

        public virtual T To { get => to; set => to = value; }

        public float Percent
        {
            get => _percent;
            set
            {
                Interpolate(_percent = value);
            }
        }

        public abstract T Current { get; protected set; }

        public abstract T Interpolate(float percent);
    }
}