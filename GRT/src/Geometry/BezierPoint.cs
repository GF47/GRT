using System;
using System.Collections.Generic;
using UnityEngine;

namespace GRT.Geometry
{
    /// <summary>
    /// 贝塞尔曲线的节点
    /// </summary>
    [Serializable]
    public class BezierPoint
    {
        public enum PointType { Smooth, Bezier, BezierCorner, }

        public PointType type;

        public float Percent { get => _percent; set => _percent = Mathf.Clamp01(value); }

        [SerializeField]
        private float _percent;

        public Vector3 Position
        {
            get => _position;
            set
            {
                Vector3 offset = value - _position;
                _position = value;
                _handleL += offset;
                _handleR += offset;
            }
        }

        [SerializeField]
        private Vector3 _position;

        public Vector3 HandleL
        {
            get => _handleL;
            set
            {
                switch (type)
                {
                    case PointType.Bezier:
                        float ll = Vector3.Distance(_handleL, _position);
                        float lr = Vector3.Distance(_handleR, _position);

                        _handleL = value;
                        Vector3 v = _handleL - _position;
                        if (ll > 0f)
                        {
                            _handleR = _position - lr / ll * v;
                        }
                        break;

                    case PointType.BezierCorner:
                        _handleL = value;
                        break;

                    case PointType.Smooth:
                    default:
                        _handleL = value;
                        _handleR = _position + _position - _handleL;
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
                    case PointType.Bezier:
                        float ll = Vector3.Distance(_handleL, _position);
                        float lr = Vector3.Distance(_handleR, _position);

                        _handleR = value;
                        Vector3 v = _handleR - _position;
                        if (lr > 0f)
                        {
                            _handleL = _position - ll / lr * v;
                        }
                        break;

                    case PointType.BezierCorner:
                        _handleR = value;
                        break;

                    case PointType.Smooth:
                    default:
                        _handleR = value;
                        _handleL = _position + _position - _handleR;
                        break;
                }
            }
        }

        [SerializeField]
        private Vector3 _handleR;

        public BezierPoint() : this(
            Vector3.zero,
            Vector3.left,
            Vector3.right,
            PointType.Smooth) { }

        public BezierPoint(Vector3 position) : this(
            position,
            position + Vector3.left,
            position + Vector3.right,
            PointType.Smooth) { }

        public BezierPoint(Vector3 position, Vector3 handleLeft, Vector3 handleRight, PointType type = PointType.Smooth)
        {
            this.type = type;
            _position = position;
            _handleL = handleLeft;
            _handleR = handleRight;
        }

        public int AddToList(IList<BezierPoint> list)
        {
            int i = 0;
            while (i < list.Count)
            {
                if (Percent < list[i].Percent)
                {
                    if (i == 0)
                    {
                        Percent = 0f;
                    }
                    list.Insert(i, this);
                    return i;
                }
                i++;
            }
            Percent = 1f;
            list.Add(this);
            return i;
        }

        public static void RemoveFromList(int index, IList<BezierPoint> list)
        {
            if (0 <= index && index < list.Count)
            {
                list.RemoveAt(index);
            }
        }

        public void InsertToList(int index, IList<BezierPoint> list)
        {
            index = Math.Min(Math.Max(0, index), list.Count);

            if (index == list.Count)
            {
                if (index > 0)
                {
                    list[index - 1].Percent = list.Count < 2 ? 0f : (list[index - 2].Percent + 1f) / 2f;
                }

                Percent = 1f;
                list.Add(this);
            }
            else if (index == 0)
            {
                list[0].Percent = list.Count < 2 ? 1f : list[1].Percent / 2f;

                Percent = 0f;
                list.Insert(index, this);
            }
            else
            {
                Percent = (list[index - 1].Percent + list[index].Percent) / 2f;
                list.Insert(index, this);
            }
        }
    }
}