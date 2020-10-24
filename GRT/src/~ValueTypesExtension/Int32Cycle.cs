/***************************************************************
 * @File Name       : Int32Cycle
 * @Author          : GF47
 * @Description     : 一个由指定初始位置和长度的环形数组
 * @Date            : 2017/8/16/星期三 15:27:30
 * @Edit            : none
 **************************************************************/

namespace GRT
{
    public struct Int32Cycle
    {
        private readonly int _origin;
        private readonly int _step;
        private readonly int _length;

        private int _value;

        public int Previous(int i)
        {
            int previous = _value - i * _step;
            Cycle(ref previous, _origin, _length);
            return previous;
        }

        public int Value
        {
            get { return _value; }
        }

        public int Following(int i)
        {
            int following = _value + i * _step;
            Cycle(ref following, _origin, _length);
            return following;
        }

        public Int32Cycle(int origin, int count, int step)
        {
            _origin = origin;
            _step = step;
            _length = count * _step;

            _value = _origin;
        }

        public void Step()
        {
            _value += _step;
            Cycle(ref _value, _origin, _length);
        }

        private static void Cycle(ref int value, int origin, int lenght)
        {
            value = (value - origin) % lenght + origin;
            if (value < origin) { value += lenght; }
        }

        public static implicit operator int(Int32Cycle c)
        {
            return c.Value;
        }
    }
}
