using System;

namespace GRT.GInventory.Quantifiables
{
    public class Count : IQuantifiable
    {
        public const int MAX = 64;

        private int _count;

        public string Type => Keywords.COUNT;

        public int Value => _count;

        public int Max { get; set; } = MAX;

        public int Dose { get; set; } = MAX;

        public event Action<IStack, int, int> Changing;

        public Count(int count) => _count = count;

        public IQuantifiable Clone(int count) => new Count(count) { Max = Max, Dose = Dose, };

        public void SetValue(IStack stack, int count)
        {
            if (_count != count)
            {
                var old = _count;
                _count = count;

                Changing?.Invoke(stack, _count, old);
                if (_count <= 0)
                {
                    stack.Destroy();
                }
            }
        }
    }
}