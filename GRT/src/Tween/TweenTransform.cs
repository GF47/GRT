//************************************************************//
//      Author      :       GF47
//      DataTime    :       2013/9/17 星期二 15:48:07
//      Edited      :       2013/9/17 星期二 15:48:07
//************************************************************//
namespace GRT.Tween
{
    using UnityEngine;

    /// <summary>
    /// Author          :GF47
    /// DataTime        :2013/9/17 星期二 15:48:07
    /// [TweenTransform] Introduction  :Nothing to introduce
    /// </summary>
    public class TweenTransform : Tween<Transform>
    {
        private Transform _transform;
        private Vector3 _position;
        private Quaternion _rotation;
        private Vector3 _scale;

        void Awake()
        {
            _transform = transform;
            setValue = delegate(float f)
            {
                if (to != null)
                {
                    if (from != null)
                    {
                        _transform.position = Vector3.Lerp(from.position, to.position, f);
                        _transform.localScale = Vector3.Lerp(from.localScale, to.localScale, f);
                        _transform.rotation = Quaternion.Slerp(from.rotation, to.rotation, f);
                    }
                    else
                    {
                        _transform.position = Vector3.Lerp(_position, to.position, f);
                        _transform.localScale = Vector3.Lerp(_scale, to.localScale, f);
                        _transform.rotation = Quaternion.Slerp(_rotation, to.rotation, f);
                    }
                }
                return _transform;
            };
        }

        public static TweenTransform Begin(GameObject go, float duration, Transform to)
        {
            TweenTransform tt = Begin(go, duration, null, to);
            tt._position = go.transform.localPosition;
            tt._scale = go.transform.localScale;
            tt._rotation = go.transform.localRotation;
            return tt;
        }
        public static TweenTransform Begin(GameObject go, float duration, Transform from, Transform to)
        {
            TweenBase tb = TweenBase.Begin<Transform, TweenTransform>(duration, from, to, go, go);
            if (tb.targets != null && tb.targets.Count > 0)
            {
                return tb.targets[0] as TweenTransform;
            }
            return null;
        }
    }
}
