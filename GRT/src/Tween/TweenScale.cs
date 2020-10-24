//************************************************************//
//      Author      :       GF47
//      DataTime    :       2013/9/17 星期二 14:44:27
//      Edited      :       2013/9/17 星期二 14:44:27
//************************************************************//
namespace GRT.Tween
{
    using UnityEngine;

    /// <summary>
    /// Author          :GF47
    /// DataTime        :2013/9/17 星期二 14:44:27
    /// [TweenScale] Introduction  :Nothing to introduce
    /// </summary>
    public class TweenScale : Tween<Vector3>
    {
        public Transform target;

        void Awake()
        {
            if (target == null)
            {
                target = transform;
            }
            setValue = delegate (float f)
             {
                 target.localScale = Vector3.Lerp(from, to, f);
                 return target.localScale;
             };
        }
    }
}
