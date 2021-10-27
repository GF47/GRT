using System;

namespace GRT.Updater
{
    public class FloatBuffer : AbstractValueBuffer<float>
    {
        public FloatBuffer(float from, Action<float> updating, float duration = 1) : base(from, updating, duration)
        {
        }

        protected override float Addition(float a, float b)
        {
            return a + b;
        }

        protected override bool IsValid(float v)
        {
            return Math.Abs(v) > 1e-6f;
        }

        protected override float Multiplication(float m, float v)
        {
            return m * v;
        }

        protected override float Subtraction(float a, float b)
        {
            return a - b;
        }
    }
}