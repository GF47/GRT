/***************************************************************
 * @File Name       : TweenBezierSplinePath
 * @Author          : GF47
 * @Description     : 贝塞尔曲线的路径
 * @Date            : 2017/6/8/星期四 12:00:08
 * @Edit            : none
 **************************************************************/

using GRT.Geometry.Bezier;
using UnityEngine;

namespace GRT.Tween
{
    public class TweenBezierSplinePath : MonoBehaviour , IPercent
    {
        /// <summary>
        /// 序列化的贝塞尔曲线
        /// </summary>
        public BezierSpline BSplineAsset;
        /// <summary>
        /// 需要沿路径移动的[Transform]
        /// </summary>
        public Transform target;
        /// <summary>
        /// 是否影响朝向
        /// </summary>
        public bool affectDirection;
        /// <summary>
        /// 坐标系，默认世界坐标
        /// </summary>
        public Space space;

        private float _percent;

        public float Percent
        {
            get { return _percent; }
            set
            {
                _percent = value;

                BezierResult r = BSplineAsset.GetResult(_percent);
                if (space == Space.Self)
                {
                    target.localPosition = r.position;
                    if (affectDirection)
                    {
                        // TODO 修改局部坐标的forward
                        // target.forward = r.Direction;
                    }
                }
                else
                {
                    target.position = r.position;
                    if (affectDirection)
                    {
                        target.forward = r.Direction;
                    }
                }
            }
        }

        void Awake()
        {
            if (target == null) { target = transform; }
        }
    }
}
