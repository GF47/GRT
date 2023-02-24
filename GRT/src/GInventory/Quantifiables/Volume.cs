using System;
using System.Runtime.InteropServices;

namespace GRT.GInventory.Quantifiables
{
    public class Volume : IQuantifiable
    {
        public const string TYPE = Keywords.VOLUME;

        public const int MAX_VOLUME = 100;

        private int _value;

        public string Type => TYPE;

        public int Value => _value;

        public int Max { get; set; } = MAX_VOLUME;

        public event Action<IStack, int, int> ValueChanging;

        // public void ClearValueChangingEvents() => ValueChanging = null;

        public Volume(int value)
        {
            _value = value;
        }

        public IQuantifiable Clone(int value) => new Volume(value) { Max = Max };

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