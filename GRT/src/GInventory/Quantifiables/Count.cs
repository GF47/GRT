using System;

namespace GRT.GInventory.Quantifiables
{
    public class Count : IQuantifiable
    {
        public const string TYPE = InventoryKeyword.COUNT;

        public const int MAX_COUNT = 64;

        private int _value;

        public string Type => TYPE;

        public int Value => _value;

        public int Max { get; set; } = MAX_COUNT;

        public event Action<IStack, int, int> ValueChanging;

        // public void ClearValueChangingEvents() => ValueChanging = null;

        public Count(int value)
        {
            _value = value;
        }

        public IQuantifiable Clone(int value) => new Count(value) { Max = Max };

        public void SetValue(IStack stack, int value)
        {
            if (_value != value)
            {
                var old = _value;
                _value = value;
                ValueChanging?.Invoke(stack, _value, old);
                if (_value <= 0)
                {
                    stack.Destroy();
                }
            }
        }
    }
}