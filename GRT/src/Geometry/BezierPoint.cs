using System;
using UnityEngine;

namespace GRT.Geometry
{
    /// <summary>
    /// 贝塞尔曲线的节点
    /// </summary>
    [Serializable]
    public class BezierPoint
    {
        public enum PointType
        {
            // Smooth,
            // Corner,
            BezierCorner,
            Bezier,
        }
        public PointType type;

        public Vector3 Point
        {
            get => _point;
            set
            {
                Vector3 offset = value - _point;
                _point = value;
                _handleL += offset;
                _handleR += offset;
            }
        }
        [SerializeField]
        private Vector3 _point;

        public Vector3 HandleL
        {
            get => _handleL;
            set
            {
                switch (type)
                {
                    // case PointType.Smooth:
                    //     break;
                    // case PointType.Corner:
                    //     break;
                    case PointType.BezierCorner:
                        _handleL = value;
                        break;
                    case PointType.Bezier:
                    default:
                        float lengthL = Vector3.Distance(_handleL, _point);
                        float lengthR = Vector3.Distance(_handleR, _point);

                        _handleL = value;
                        Vector3 v = _handleL - _point;
                        if (lengthL > 0f)
                        {
                            _handleR = _point - lengthR / lengthL * v;
                        }
                        break;
                }
            }
        }
        [SerializeField]
        private Vector3 _handleL;

        public Vector3 HandleR
        {
            get => _handleR;
            set
            {
                switch (type)
                {
                    // case PointType.Smooth:
                    //     break;
                    // case PointType.Corner:
                    //     break;
                    case PointType.BezierCorner:
                        _handleR = value;
                        break;
                    case PointType.Bezier:
                    default:
                        float lengthL = Vector3.Distance(_handleL, _point);
                        float lengthR = Vector3.Distance(_handleR, _point);

                        _handleR = value;
                        Vector3 v = _handleR - _point;
                        if (lengthR > 0f)
                        {
                            _handleL = _point - lengthL / lengthR * v;
                        }
                        break;
                }
            }
        }
        [SerializeField]
        private Vector3 _handleR;

        public BezierPoint() : this(Vector3.zero, 1f, PointType.Bezier) { }
        public BezierPoint(Vector3 position) : this(position, 1f, PointType.Bezier) { }
        public BezierPoint(Vector3 position, float handleLength, PointType type)
        {
            this.type = type;
            _point = position;
            _handleL = _point + handleLength * Vector3.left;
            _handleR = _point + handleLength * Vector3.right;
        }
    }
}
