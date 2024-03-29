﻿using UnityEngine;

namespace GRT.Tween
{
    public class TweenFocusTranslate : Tween<Vector3>
    {
        [SerializeField] private Transform _target;
        [SerializeField] private Transform _self;
        [SerializeField] private bool _useInterpolation = true;

        public override Vector3 From
        {
            get => from; set
            {
                from = value;
            }
        }
        public override Vector3 To
        {
            get => to; set
            {
                to = value;
            }
        }

        public Transform Target { get => _target; set => SetTarget(value); }
        public Transform Self { get => _self; set => _self = value; }
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

        public override Vector3 Current { get => _self.position; protected set => _self.position = value; }

        public override Vector3 Interpolate(float percent)
        {
            if (_useInterpolation) { InterpolatedTranslate(percent); }
            else { LinearTranslate(percent); }
            return Current;
        }

        private void Awake()
        {
            if (_self == null) { _self = transform; }
            if (_useInterpolation) { CalculateInterpolation(); }
        }

        private void LinearTranslate(float p)
        {
            Current = Vector3.Lerp(From, To, p);
            if (_target != null) { _self.LookAt(_target); }
        }

        private void InterpolatedTranslate(float p)
        {
            if (_target == null) { LinearTranslate(p); return; }

            var current = Mathf.Lerp(_fromRadius, _toRadius, p);
            _self.rotation = Quaternion.Slerp(_fromQuaternion, _toQuaternion, p);
            Current = _self.rotation * new Vector3(0f, 0f, -current) + _target.position;
        }

        public void SetTarget(Transform target)
        {
            _target = target;
        }

        /// <summary>
        /// 最终还是需要重新计算一下
        /// </summary>
        public void CalculateInterpolation()
        {
            if (_target == null)
            {
                Debug.LogWarning($"{nameof(TweenFocusTranslate)} at {name}: Target is null");
                return;
            }

            var tt = _target.GetComponent<TweenPosition>();

            Vector3 vtf, vtt;

            if (tt != null)
            {
                vtf = tt.From;
                vtt = tt.To;
            }
            else
            {
                vtf = _target.position;
                vtt = _target.position;
            }

            var f = vtf - from; _fromRadius = f.magnitude; _fromQuaternion = Quaternion.LookRotation(f);
            var t = vtt - to; _toRadius = t.magnitude; _toQuaternion = Quaternion.LookRotation(t);
        }
    }
}