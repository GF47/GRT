using System;

namespace GRT.Updater
{
    public class IntBuffer : AbstractValueBuffer<int>
    {
        public IntBuffer(int from,Action<int> updating, float duration = 1) : base(from, updating, duration)
        {
        }

        protected override int Addition(int a, int b)
        {
            return a + b;
        }

        protected override bool IsValid(int v)
        {
            return v != 0;
        }

        protected override int Multiplication(float m, int v)
        {
            return (int)m * v;
        }

        protected override int Subtraction(int a, int b)
        {
            return a - b;
        }
    }
}