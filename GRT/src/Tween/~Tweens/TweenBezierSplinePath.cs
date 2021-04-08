using GRT.Geometry;
using UnityEngine;

namespace GRT.Tween
{
    public class TweenBezierSplinePath : Tween<float>
    {
        [SerializeField] private BezierSpline _bSplineAsset;
        [SerializeField] private Transform _target;
        [SerializeField] private bool _useDirection;

        public BezierSpline BSplineAsset { get => _bSplineAsset; set => _bSplineAsset = value; }
        public Transform Target { get => _target; set => _target = value; }
        public bool UseDirection { get => _useDirection; set => _useDirection = value; }

        public override float Current
        {
            get => Percent;
            protected set
            {
                var r = BSplineAsset.GetResult(value);
                Target.position = r.position;
                if (UseDirection)
                {
                    Target.forward = r.Direction;
                }
            }
        }

        public override float Project(float percent)
        {
            Current = percent;
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