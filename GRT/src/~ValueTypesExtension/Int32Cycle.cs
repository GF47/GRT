/***************************************************************
 * @File Name       : Int32Cycle
 * @Author          : GF47
 * @Description     : 一个由指定初始位置和长度的环形数组
 * @Date            : 2017/8/16/星期三 15:27:30
 * @Edit            : none
 **************************************************************/

using System;

namespace GRT
{
    public struct Int32Cycle : IEquatable<Int32Cycle>
    {
        private readonly int _origin;
        private readonly int _length;
        private readonly int _step;

        private int _value;
        public int Value => _value;

        public Int32Cycle(int origin, int count, int step)
        {
            _origin = origin;
            _step = step;
            _length = count * _step;

            _value = _origin;
        }

        public void Step(int count = 1)
        {
            _value += count * _step;
            Cycle(ref _value, _origin, _length);
        }

        public void InvertStep(int count = 1)
        {
            _value -= count * _step;
            Cycle(ref _value, _origin, _length);
        }

        public int Previous(int count)
        {
            int previous = _value - count * _step;
            Cycle(ref previous, _origin, _length);
            return previous;
        }

        public int Following(int count)
        {
            int following = _value + count * _step;
            Cycle(ref following, _origin, _length);
            return following;
        }

        private static void Cycle(ref int value, int origin, int lenght)
        {
            value = (value - origin) % lenght + origin;
            if (value < origin) { value += lenght; }
        }

        public bool Equals(Int32Cycle other)
        {
            if (_value != other._value) goto RETURN;
            if (_origin != other._origin) goto RETURN;
            if (_length != other._length) goto RETURN;
            if (_step != other._step) goto RETURN;

            return true;

        RETURN:
            return false;
        }

        public override bool Equals(object obj)
        {
            return obj != null &&
                GetType() == obj.GetType() &&
                Equals((Int32Cycle)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = (int)2166136261;
                hash = (hash * 16777619) ^ _value.GetHashCode();
                hash = (hash * 16777619) ^ _origin.GetHashCode();
                hash = (hash * 16777619) ^ _length.GetHashCode();
                hash = (hash * 16777619) ^ _step.GetHashCode();
                return hash;
            }
        }

        public static implicit operator int(Int32Cycle c) => c.Value;

        public static bool operator ==(Int32Cycle a, Int32Cycle b) => a.Equals(b);

        public static bool operator !=(Int32Cycle a, Int32Cycle b) => !a.Equals(b);
    }
}