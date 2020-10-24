namespace GRT
{
    using UnityEngine;

    public static class QuaternionExtension
    {
        /// <summary>
        /// 将一个旋转按轴向限制角度
        /// </summary>
        /// <param name="q">旋转</param>
        /// <param name="minEulerAngle">角度最小值</param>
        /// <param name="maxEulerAngle">角度最大值</param>
        /// <param name="axis">轴向 [0：x轴] [1：y轴] [2：z轴]</param>
        /// <returns></returns>
        public static Quaternion ClampRotationAroundAxis(this Quaternion q, float minEulerAngle, float maxEulerAngle, int axis)
        {
            float w = q.w;
            q.x /= w;
            q.y /= w;
            q.z /= w;
            q.w = 1f;

            float angle = 0;
            
            switch (axis)
            {
                case 0:
                    angle = 2f * Mathf.Rad2Deg * Mathf.Atan(q.x);
                    break;
                case 1:
                    angle = 2f * Mathf.Rad2Deg * Mathf.Atan(q.y);
                    break;
                case 2:
                    angle = 2f * Mathf.Rad2Deg * Mathf.Atan(q.z);
                    break;
            }

            angle = Mathf.Clamp(angle, minEulerAngle, maxEulerAngle);

            switch (axis)
            {
                case 0:
                    q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angle);
                    break;
                case 1:
                    q.y = Mathf.Tan(0.5f * Mathf.Deg2Rad * angle);
                    break;
                case 2:
                    q.z = Mathf.Tan(0.5f * Mathf.Deg2Rad * angle);
                    break;
            }
            return q;
        }
    }
}
