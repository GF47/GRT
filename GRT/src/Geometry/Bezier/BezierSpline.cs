using System;
using System.Collections.Generic;
using UnityEngine;

namespace GRT.Geometry.Bezier 
{
    [CreateAssetMenu]
    [Serializable]
    public class BezierSpline : ScriptableObject
    {
        public List<BezierPoint> points;
        public void Add(BezierPoint item)
        {
            points.Add(item);
        }

        public int Count { get { return points.Count; } }

        public void Insert(int index, BezierPoint item) { points.Insert(index, item); }

        public void RemoveAt(int index) { points.RemoveAt(index); }

        public BezierPoint this[int index]
        {
            get { return points[index]; }
            set
            {
                if (index < points.Count)
                {
                    points[index] = value;
                }
            }
        }

        public BezierSpline() : this(4) { }
        public BezierSpline(int capacity)
        {
            points = new List<BezierPoint>(capacity);
        }

        public BezierResult GetResult(float t)
        {
            if (points.Count == 0)
            {
                return new BezierResult(Vector3.zero, Vector3.forward);
            }
            if (points.Count == 1)
            {
                return new BezierResult(points[0].Point, points[0].HandleR - points[0].Point);
            }
            t = Mathf.Clamp01(t);
            float step = 1f / (points.Count - 1);
            t = t / step;
            int i = Mathf.FloorToInt(t);
            if (i == points.Count - 1)
            {
                i--;
                t = 1f;
            }
            else
            {
                t -= i;
            }
            return _getResult(points[i], points[i + 1], t);
        }

        private static BezierResult _getResult(BezierPoint left, BezierPoint right, float t)
        {
            Vector3 v0 = left.Point;
            Vector3 v1 = left.HandleR;
            Vector3 v2 = right.HandleL;
            Vector3 v3 = right.Point;
            t = Mathf.Clamp01(t);
            float s = 1f - t;

            Vector3 p = s * s * s * v0 +
                        3f * s * s * t * v1 +
                        3f * s * t * t * v2 +
                        t * t * t * v3;
            Vector3 v = 3f * s * s * (v1 - v0) +
                        6f * s * t * (v2 - v1) +
                        3f * t * t * (v3 - v2);
            return new BezierResult(p, v);
        }
    }
}
