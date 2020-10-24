//************************************************************//
//      Author      :       GF47
//      DataTime    :       2013/9/7 18:44:08
//      Edited      :       2013/9/7 18:44:08
//************************************************************//

using GRT.Tween;
using UnityEngine;

namespace GRT.Components.CameraUtility
{
    /// <summary>
    /// Author          :GF47
    /// DataTime        :2013/9/7 18:44:08
    /// CameraController Introduction    :nothing to say
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        public float duration = 1.0f;
        public TweenEase easeType = TweenEase.Linear;
        public TweenLoop loopType = TweenLoop.Once;
        public CameraTranslater.TranslateMode translateMode = CameraTranslater.TranslateMode.Linear;
        public CameraTargetController cameraTargetController;
        [HideInInspector]
        public CameraTranslater theTranslater;

        public Vector3 TranslateTo
        {
            get { return _translateTo; }
            set
            {
                _translateTo = value;
                CameraTranslater.CameraTargetPositions targetPositions = new CameraTranslater.CameraTargetPositions(cameraTargetController.transform, cameraTargetController.transform.localPosition, cameraTargetController.TranslateTo);
                theTranslater = CameraTranslater.Begin(gameObject, duration, transform.localPosition, _translateTo, targetPositions, easeType, loopType, translateMode);
            }
        }
        private Vector3 _translateTo;
    }
}
