using UnityEngine;

namespace GRT.Geometry
{
    public struct Line
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
    }
}
