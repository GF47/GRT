using UnityEngine;

namespace GRT.Tween
{
    public class TweenRotation : Tween<Vector3>
    {
        [SerializeField] private Transform _target;

        public override Vector3 From { get => from; set { from = value; _from = Quaternion.Euler(value); } }
        public override Vector3 To { get => to; set { to = value; _to = Quaternion.Euler(value); } }

        public override Vector3 Current { get => _target.eulerAngles; protected set => _target.eulerAngles = value; }

        public override Vector3 Interpolate(float percent)
        {
            _target.rotation = Quaternion.Slerp(_from, _to, percent);
            return Current;
        }

        private Quaternion _from;
        private Quaternion _to;

        private void Awake()
        {
            if (_target == null)
            {
                _target = transform;
            }

            _from = Quaternion.Euler(from);
            _to = Quaternion.Euler(to);
        }
    }
}