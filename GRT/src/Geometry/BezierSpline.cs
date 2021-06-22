using System;
using System.Collections.Generic;
using UnityEngine;

namespace GRT.Geometry
{
    [CreateAssetMenu]
    [Serializable]
    public class BezierSpline : ScriptableObject
    {
        public List<BezierPoint> points;

        public int Count => points.Count;

        public void Add(BezierPoint item) => item.AddToList(points);

        public void Insert(int index, BezierPoint item) => item.InsertToList(index, points);

        public void RemoveAt(int index) => BezierPoint.RemoveFromList(index, points);

        public void Sort() => points.Sort((a, b) => Math.Sign(a.Percent - b.Percent));

        public BezierPoint this[int index]
        {
            get => points[index];
            set
            {
                if (index > -1 && index < points.Count)
                {
                    points[index] = value;
                }
            }
        }

        public BezierSpline() : this(4)
        {
        }

        public BezierSpline(int capacity)
        {
            points = new List<BezierPoint>(capacity);
            new BezierPoint().InsertToList(0, points);
            new BezierPoint(Vector3.forward).InsertToList(1, points);
        }

        public Point GetResult(float t)
        {
            if (points.Count == 0)
            {
                return new Point(Vector3.zero, Vector3.forward);
            }

            if (points.Count == 1)
            {
                var bp = points[0];
                return new Point(bp.Position, bp.HandleR - bp.Position);
            }

            t = Math.Min(Math.Max(0f, t), 1);
            var i = 0;
            while (i < points.Count - 1)
            {
                var pi = points[i];
                var pi1 = points[i + 1];
                if (pi.Percent <= t && t < pi1.Percent)
                {
                    return GetResult_(pi, pi1, (t - pi.Percent) / (pi1.Percent - pi.Percent));
                }
                i++;
            }

            return GetResult_(points[i - 1], points[i], 1f);
        }

        private static Point GetResult_(BezierPoint left, BezierPoint right, float t)
        {
            var v0 = left.Position;
            var v1 = left.HandleR;
            var v2 = right.HandleL;
            var v3 = right.Position;

            t = Mathf.Clamp01(t);
            var s = 1f - t;

            var p = s * s * s * v0 +
               3f * s * s * t * v1 +
               3f * s * t * t * v2 +
                    t * t * t * v3;
            var v = 3f * s * s * (v1 - v0) +
                    6f * s * t * (v2 - v1) +
                    3f * t * t * (v3 - v2);
            return new Point(p, v);
        }
    }
}