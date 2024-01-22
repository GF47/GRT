using UnityEngine;

namespace GRT.Tween
{
    public class TweenLocalScale : Tween<Vector3>
    {
        [SerializeField] private Transform _target;

        public override Vector3 Current { get => _target.localScale; protected set => _target.localScale = value; }

        public override Vector3 Interpolate(float percent)
        {
            Current = Vector3.Lerp(from, to, percent);
            return Current;
        }

        private void Awake()
        {
            if (_target == null)
            {
                _target = transform;
            }
        }
    }
}