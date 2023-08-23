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

        public int Origin => _origin;
        public int Length => _length;

        public Int32Cycle(int origin, int count, int step = 1)
        {
            if (count == 0)
            {
                throw new ArgumentException(nameof(count), "count can not be zero");
            }

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

        public int Offset(int count)
        {
            var value = _value + count * _step;
            Cycle(ref value, _origin, _length);
            return value;
        }

        private static void Cycle(ref int value, int origin, int lenght)
        {
            value = (value - origin) % lenght + origin;
            if (value < origin) { value += lenght; }
        }

        public bool Equals(Int32Cycle other)
        {
            return _value == other._value
                && _origin == other._origin
                && _length == other._length
                && _step == other._step;
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