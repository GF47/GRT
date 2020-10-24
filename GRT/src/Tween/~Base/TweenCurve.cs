/***************************************************************
 * @File Name       : TweenCurve
 * @Author          : GF47
 * @Description     : 使用AnimationCurve的Tween装饰器
 * @Date            : 2017/6/7/星期三 9:37:03
 * @Edit            : none
 **************************************************************/

using UnityEngine;

namespace GRT.Tween
{
    public class TweenCurve : MonoBehaviour, IPercent
    {
        public AnimationCurve curve;
        [InheritFrom(typeof(IPercent))]
        public MonoBehaviour _iPercentTarget;
        public IPercent target;

        public float Percent
        {
            get { return _percent; }
            set
            {
                _percent = value;
                target.Percent = curve.Evaluate(_percent);
            }
        }
        private float _percent;

        void Awake()
        {
            target = _iPercentTarget as IPercent;
        }
    }
}
