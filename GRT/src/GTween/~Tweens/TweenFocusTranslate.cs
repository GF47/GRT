using UnityEngine;

namespace GRT.GTween
{
    public class TweenFocusTranslate : GTween<Vector3>
    {
        private bool _useInterpolation = true;

        public override Vector3 From { get; set; }
        public override Vector3 To { get; set; }

        public Vector3 TargetFrom { get; set; }
        public Vector3 TargetTo { get; set; }

        public Transform Target { get; set; }
        public Transform Self { get; set; }

        public bool UseInterpolation
        {
            get => _useInterpolation;
            set
            {
                _useInterpolation = value;
                if (_useInterpolation) { CalculateInterpolation(); }
            }
        }

        private float _fromRadius, _toRadius;
        private Quaternion _fromQuaternion, _toQuaternion;

        public override Vector3 Interpolate(float percent)
        {
            if (_useInterpolation) { InterpolatedTranslate(percent); }
            else { LinearTranslate(percent); }
            return Self.position;
        }

        private void LinearTranslate(float p)
        {
            Self.position = Vector3.Lerp(From, To, p);
            if (Target != null) { Self.LookAt(Target); }
        }

        private void InterpolatedTranslate(float p)
        {
            if (Target == null) { LinearTranslate(p); return; }

            var current = Mathf.Lerp(_fromRadius, _toRadius, p);
            Self.rotation = Quaternion.Slerp(_fromQuaternion, _toQuaternion, p);
            Self.position = Self.rotation * new Vector3(0f, 0f, -current) + Target.position;
        }

        /// <summary>
        /// 最终还是需要重新计算一下
        /// </summary>
        public void CalculateInterpolation()
        {
            if (Target == null)
            {
                throw new UnityException($"{nameof(TweenFocusTranslate)} at {Self.name}: Target is null");
            }

            var f = TargetFrom - From; _fromRadius = f.magnitude; _fromQuaternion = Quaternion.LookRotation(f);
            var t = TargetTo - To; _toRadius = t.magnitude; _toQuaternion = Quaternion.LookRotation(t);
        }
    }
}