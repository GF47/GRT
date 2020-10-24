//************************************************************//
//      Author      :       GF47
//      DataTime    :       2013/9/18 星期三 17:39:04
//      Edited      :       2013/9/18 星期三 17:39:04
//************************************************************//
namespace GRT.Tween
{
    using UnityEngine;

    /// <summary>
    /// Author          :GF47
    /// DataTime        :2013/9/18 星期三 17:39:04
    /// [Translater] 目标物体移动
    /// </summary>
    public class Translater : Tween<Vector3>
    {
        void Awake()
        {
            setValue = delegate(float f)
            {
                transform.localPosition = Vector3.Lerp(from, to, f);
                return transform.localPosition;
            };
        }

        public static Translater Begin(GameObject go, float duration, Vector3 from, Vector3 to, TweenEase easeType, TweenLoop loopType)
        {
            TweenBase tb = TweenBase.Begin<Vector3, Translater>(duration, from, to, go, go);
            tb.ResetAlgorithm(easeType, loopType, TweenDirection.Forward);
            if (tb.targets != null && tb.targets.Count > 0)
            {
                return tb.targets[0] as Translater;
            }
            return null;
        }
    }

}
