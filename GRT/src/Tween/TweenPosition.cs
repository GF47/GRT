//************************************************************//
//      Author      :       GF47
//      DataTime    :       2013/9/17 星期二 14:21:48
//      Edited      :       2013/9/17 星期二 14:21:48
//************************************************************//
namespace GRT.Tween
{
    using UnityEngine;

    /// <summary>
    /// Author          :GF47
    /// DataTime        :2013/9/17 星期二 14:21:48
    /// [TweenPosition] Introduction  :Nothing to introduce
    /// </summary>
    public class TweenPosition : Tween<Vector3>
    {
        public Transform target;

        void Awake()
        {
            if (target == null)
            {
                target = transform;
            }
            setValue = delegate(float f)
            {
                target.localPosition = Vector3.Lerp(from, to, f);
                return target.localPosition;
            };
        }
    }
}
