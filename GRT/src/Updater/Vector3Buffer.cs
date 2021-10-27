using System;
using UnityEngine;

namespace GRT.Updater
{
    public class Vector3Buffer : AbstractValueBuffer<Vector3>
    {
        public Vector3Buffer(Vector3 from, Action<Vector3> updating, float duration = 1f) : base(from, updating, duration)
        {
        }

        protected override Vector3 Addition(Vector3 a, Vector3 b)
        {
            return a + b;
        }

        protected override bool IsValid(Vector3 v)
        {
            return v.magnitude > 1e-6f;
        }

        protected override Vector3 Multiplication(float m, Vector3 v)
        {
            return m * v;
        }

        protected override Vector3 Subtraction(Vector3 a, Vector3 b)
        {
            return a - b;
        }
    }
}