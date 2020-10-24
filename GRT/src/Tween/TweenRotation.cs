//************************************************************//
//      Author      :       GF47
//      DataTime    :       2013/9/17 星期二 14:41:26
//      Edited      :       2013/9/17 星期二 14:41:26
//************************************************************//
namespace GRT.Tween
{
    using UnityEngine;

    /// <summary>
    /// Author          :GF47
    /// DataTime        :2013/9/17 星期二 14:41:26
    /// [TweenRotation] Introduction  :Nothing to introduce
    /// </summary>
    public class TweenRotation : Tween<Vector3>
    {
        public Transform target;

        private Quaternion _from;
        private Quaternion _to;

        void Awake()
        {
            if (target == null)
            {
                target = transform;
            }

            _from = Quaternion.Euler(from);
            _to = Quaternion.Euler(to);

            setValue = delegate (float f)
            {
                target.localRotation = Quaternion.Slerp(_from, _to, f);
                 return target.localEulerAngles;
             };
        }
    }
}
