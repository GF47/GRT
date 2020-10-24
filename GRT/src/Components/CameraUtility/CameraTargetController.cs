//************************************************************//
//      Author      :       GF47
//      DataTime    :       2013/9/22 星期日 10:14:24
//      Edited      :       2013/9/22 星期日 10:14:24
//************************************************************//

using GRT.Tween;
using UnityEngine;

namespace GRT.Components.CameraUtility
{
    /// <summary>
    /// Author          :GF47
    /// DataTime        :2013/9/22 星期日 10:14:24
    /// [CameraTargetController] Introduction  :Nothing to introduce
    /// </summary>
    public class CameraTargetController : MonoBehaviour
    {
        public float duration = 1f;
        public TweenEase easeType = TweenEase.Linear;
        public TweenLoop loopType = TweenLoop.Once;
        [HideInInspector]
        public Translater theTranslater;

        public Vector3 TranslateTo
        {
            get
            {
                return _translateTo;
            }
            set
            {
                _translateTo = value;
                theTranslater = Translater.Begin(gameObject, duration, transform.localPosition, _translateTo, easeType, loopType);
            }
        }
        private Vector3 _translateTo;
    }
}
