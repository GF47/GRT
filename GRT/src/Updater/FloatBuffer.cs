using System;

namespace GRT.Updater
{
    public class FloatBuffer : AbstractBuffer<float>
    {
        public FloatBuffer(float from, Action<float> onBuffering, float duration = 1f) : base(from, onBuffering, duration) { }

        protected override float Addition(float a, float b) { return a + b; }

        protected override bool Division(float v, float d, out float result)
        {
            if (Math.Abs(d) < 1e-6f)
            {
                result = default;
                return false;
            }
            result = v / d;
            return true;
        }


        protected override bool IsValueGreaterThanTMin(float v) { return Math.Abs(v) > 1e-6f; }

        protected override float Multiplication(float m, float v) { return m * v; }

        protected override float Subtraction(float a, float b) { return a - b; }
    }
}
