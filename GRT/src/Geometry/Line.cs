using System;
using UnityEngine;

namespace GRT.Geometry
{
    public struct Line : IEquatable<Line>
    {
        private Vector3 a;
        private Vector3 b;
        private Vector3 n;

        public Vector3 A { get => a; set { a = value; n = Vector3.Normalize(b - a); } }
        public Vector3 B { get => b; set { b = value; n = Vector3.Normalize(b - a); } }

        public Vector3 Normal => n;
        public bool Logical => a != b;

        public Line(Vector3 a, Vector3 b)
        {
            this.a = a;
            this.b = b;

            n = Vector3.Normalize(b - a);
        }

        public Vector3 GetPoint(float distance) => a + distance * n;

        public Vector3 Lerp(float f) => Vector3.Lerp(a, b, f);

        public bool Equals(Line other) => a.Equals(other.a) && b.Equals(other.b);

        public override bool Equals(object obj) => (obj is Line line) && Equals(line);

        public override int GetHashCode()
        {
            int hashCode = 2118541809;
            hashCode = hashCode * -1521134295 + a.GetHashCode();
            hashCode = hashCode * -1521134295 + b.GetHashCode();
            return hashCode;
        }

        public static implicit operator ValueTuple<Vector3, Vector3>(Line line) => (line.a, line.b);
        public static implicit operator Line(ValueTuple<Vector3, Vector3> tuple) => new Line(tuple.Item1, tuple.Item2);
    }
}
