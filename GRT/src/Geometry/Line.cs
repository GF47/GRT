/* ****************************************************************
 * @File Name   :   Line
 * @Author      :   GF47
 * @Date        :   2015/3/11 10:56:59
 * @Description :   定义了一条[直线、射线、线段]
 * @Edit        :   2015/3/11 10:56:59
 * ***************************************************************/

using UnityEngine;

namespace GRT.Geometry
{
    /// <summary>
    /// 定义了一条[直线、射线、线段]
    /// </summary>
    public struct Line
    {
        public enum LineType { Straight = 0, Ray = 1, Segment = 2 }
        public LineType type;

        public bool Logical
        {
            get
            {
                if (_points == null) return false;
                return _points[0] != _points[1];
            }
        }

        public Vector3 A
        {
            get
            {
                if (_points == null) return Vector3.zero;
                return _points[0];
            }
            set
            {
                if (_points == null) return;
                _points[0] = value;
            }
        }

        public Vector3 B
        {
            get
            {
                if (_points == null) return Vector3.zero;
                return _points[1];
            }
            set
            {
                if (_points == null) return;
                _points[1] = value;
            }
        }
        private Vector3[] _points;

        public Vector3 Normal { get { return Vector3.Normalize(_points[1] - _points[0]); } }

        public Line(LineType type, Vector3 a, Vector3 b)
        {
            this.type = type;
            _points = new Vector3[2];
            _points[0] = a;
            _points[1] = b;
        }

        public Vector3 GetPoint(float distance)
        {
            return A + distance * Normal;
        }

        public static Vector3 GetPoint(Line a, float distance)
        {
            return a.A + distance * a.Normal;
        }
    }
}
