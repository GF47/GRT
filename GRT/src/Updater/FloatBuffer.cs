using System;

namespace GRT.Updater
{
    public class FloatBuffer : AbstractBuffer<float>
    {
        public FloatBuffer(float from, Action<float> buffering, float duration = 1f) : base(from, buffering, duration) { }

        protected override float Addition(float a, float b)
        {
            return a + b;
        }

        protected override float Division(float v, float d)
        {
            return v / d;
        }

        protected override bool IsValidValue(float v)
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