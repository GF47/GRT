using UnityEngine;

namespace GRT.Tween
{
    public class TweenPosition : Tween<Vector3>
    {
        [SerializeField] private Transform _target;

        public override Vector3 Current { get => _target.position; protected set => _target.position = value; }

        public override Vector3 Project(float percent)
        {
            Current = Vector3.Lerp(From, To, percent);
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