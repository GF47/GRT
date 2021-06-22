using System;
using UnityEngine;

namespace GRT.Geometry
{
    [Serializable]
    public struct Point : IEquatable<Point>
    {
        public readonly Vector3 position;
        public readonly Vector3 velocity;

        public Vector3 Direction
        {
            get { return velocity.normalized; }
        }

        public Point(Vector3 position, Vector3 velocity)
        {
            this.position = position;
            this.velocity = velocity;
        }

        public bool Equals(Point other) => position.Equals(other.position) && velocity.Equals(other.velocity);

        public override bool Equals(object obj) => (obj is Point that) && Equals(that);

        public override int GetHashCode()
        {
            int hashCode = 2053303756;
            hashCode = hashCode * -1521134295 + position.GetHashCode();
            hashCode = hashCode * -1521134295 + velocity.GetHashCode();
            return hashCode;
        }

        public static implicit operator ValueTuple<Vector3, Vector3>(Point point) => (point.position, point.velocity);
        public static implicit operator Point(ValueTuple<Vector3, Vector3> tuple) => new Point(tuple.Item1, tuple.Item2);
    }
}
