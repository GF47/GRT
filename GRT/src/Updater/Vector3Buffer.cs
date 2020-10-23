using System;
using UnityEngine;

namespace GRT.Updater
{
    public class Vector3Buffer : AbstractBuffer<Vector3>
    {
        public Vector3Buffer(Vector3 from, Action<Vector3> onBuffering, float duration = 1f) : base(from, onBuffering, duration) { }

        protected override bool Division(Vector3 v, Vector3 d, out float result)
        {
            if (d.magnitude < 1e-6f) 
            {
                result = 1f;
                return false; 
            }
            var p = Vector3.Project(v, d);

            int flag = 0; float max = d.x;
            if (Math.Abs(max) < Math.Abs(d.y)) { flag = 1; max = d.y; }
            if (Math.Abs(max) < Math.Abs(d.z)) { flag = 2; max = d.z; }

            switch (flag)
            {
                case 0: result = p.x / max; return true;
                case 1: result = p.y / max; return true;
                case 2: result = p.z / max; return true;
            }

            result = 1f;
            return false;
        }

        protected override bool IsValueGreaterThanTMin(Vector3 v) { return v.magnitude > 1e-6f; }
        protected override Vector3 Multiplication(float m, Vector3 v) { return m * v; }
        protected override Vector3 Subtraction(Vector3 a, Vector3 b) { return a - b; }
        protected override Vector3 Addition(Vector3 a, Vector3 b) { return a + b; }

    }
}
