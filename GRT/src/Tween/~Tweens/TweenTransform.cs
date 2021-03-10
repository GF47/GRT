using UnityEngine;

namespace GRT.Tween
{
    public class TweenTransform : Tween<Transform>
    {
        [SerializeField] private Transform _target;
        [SerializeField] private bool _position = true;
        [SerializeField] private bool _rotation = true;
        [SerializeField] private bool _scale;

        public override Transform Current { get => _target; protected set { } }

        public override Transform Project(float percent)
        {
            if (to != null)
            {
                var from_ = from == null ? _target : from; // 不能用 from ?? _target 因为 ?? 只能用于没有重写过 == 的类

                if (_position) _target.position = Vector3.Lerp(from_.position, to.position, percent);
                if (_rotation) _target.rotation = Quaternion.Slerp(from_.rotation, to.rotation, percent);
                if (_scale) _target.localScale = Vector3.Lerp(from_.localScale, to.localScale, percent);
            }
            return _target;
        }

        private void Awake()
        {
            if (_target == null)
            {
                _target = transform;
            }
        }

        public static TweenTransform Play(GameObject go, float duration, Transform to)
        {
            return Play(go, duration, null, to);
        }

        public static TweenTransform Play(GameObject go, float duration, Transform from, Transform to)
        {
            var td = TweenDriver.Play<Transform, TweenTransform>(go, duration, from, to, go);
            if (td.Targets != null && td.Targets.Count > 0)
            {
                return td.Targets[0] as TweenTransform;
            }
            return null;
        }
    }
}