using System;

namespace GRT.GInventory.Quantifiables
{
    public class Volume : IQuantifiable
    {
        public const int MAX = 100;

        private int _volume;

        public string Type => Keywords.VOLUME;

        public int Value => _volume;

        public int Max { get; set; } = MAX;

        public int Dose { get; set; } = MAX;

        public event Action<IStack, int, int> Changing;

        public Volume(int volume) => _volume = volume;

        public IQuantifiable Clone(int volume) => new Volume(volume) { Max = Max, Dose = Dose, };

        public void SetValue(IStack stack, int volume)
        {
            if (_volume != volume)
            {
                var old = _volume;
                _volume = volume;

                Changing?.Invoke(stack, _volume, old);

                if (_volume <= 0)
                {
                    stack.Destroy();
                }
            }
        }
    }
}