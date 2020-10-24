//************************************************************//
//      Author      :       GF47
//      DataTime    :       2014/2/13 星期四 16:37:34
//      Edited      :       2014/2/13 星期四 16:37:34
//************************************************************//

namespace GRT.Tween
{
    using UnityEngine;

    /// <summary>
    /// Author          :GF47
    /// DataTime        :2014/2/13 星期四 16:37:34
    /// [TweenCameraFOV] Introduction  :Nothing to introduce
    /// </summary>
    public class TweenCameraFOV : Tween<float>
    {
        public Camera target;

        void Awake()
        {
            if (target == null) { target = GetComponent<Camera>(); }

            if (target == null)
            {
                Destroy(this);
            }
            else
            {
                setValue = delegate (float f)
                {
                    target.fieldOfView = Mathf.Lerp(from, to, f);
                    return target.fieldOfView;
                };
            }
        }
    }
}
