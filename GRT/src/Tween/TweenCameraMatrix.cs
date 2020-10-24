//************************************************************//
//      Author      :       GF47
//      DataTime    :       2014/1/9 星期四 10:14:11
//      Edited      :       2014/1/9 星期四 10:14:11
//************************************************************//
namespace GRT.Tween
{
    using UnityEngine;

    /// <summary>
    /// Author          :GF47
    /// DataTime        :2014/1/9 星期四 10:14:11
    /// [TweenCameraMatrix] Introduction  :Nothing to introduce
    /// </summary>
    public class TweenCameraMatrix : Tween<float>
    {
        public Camera target;

        [Range(0, 3)]
        public int row;
        [Range(0, 3)]
        public int column;

        void Awake()
        {
            if (target == null)
            {
                target = GetComponent<Camera>();
            }
            if (target == null)
            {
                Destroy(this);
            }
            else
            {
                setValue = delegate(float f) 
                {
                    Matrix4x4 m = target.projectionMatrix;
                    f = Mathf.Lerp(from, to, f);
                    m[Mathf.Clamp(row, 0, 3), Mathf.Clamp(column, 0, 3)] = f;
                    target.projectionMatrix = m;
                    return f;
                };
            }
        }

        public static TweenCameraMatrix Begin(float duration, float from, float to, GameObject root)
        {
            TweenBase tb = TweenBase.Begin<float,TweenCameraMatrix>(duration,from, to, root, root);
            if (tb.targets != null && tb.targets.Count > 0)
            {
                return tb.targets[0] as TweenCameraMatrix;
            }
            return null;
        }
    }
}
