using System;
using UnityEngine;

namespace GRT.Updater
{
    public class Vector3Buffer : AbstractBuffer<Vector3>
    {
        public Vector3Buffer(Vector3 from, Action<Vector3> onBuffering, float duration = 1f) : base(from, onBuffering, duration) { }

        protected override float Division(Vector3 v, Vector3 d)
        {
            var p = Vector3.Project(v, d);

            int flag = 0; float max = d.x;
            if (Math.Abs(max) < Math.Abs(d.y)) { flag = 1; max = d.y; }
            if (Math.Abs(max) < Math.Abs(d.z)) { flag = 2; max = d.z; }

            switch (flag)
            {
                case 2: return p.z / max; 
                case 1: return p.y / max; 
                case 0:
                default: return p.x / max;
            }
        }

        protected override bool IsValidValue(Vector3 v)
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

        protected override Vector3 Addition(Vector3 a, Vector3 b)
        {
            return a + b;
        }
    }
}
