using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRT.Updater
{
    public class IntBuffer : AbstractBuffer<int>
    {
        public IntBuffer(int from, Action<int> buffering, float duration = 1) : base(from, buffering, duration)
        {
        }

        protected override int Addition(int a, int b)
        {
            return a + b;
        }

        protected override float Division(int v, int d)
        {
            return v / d;
        }

        protected override bool IsValidValue(int v)
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
