using System;
using UnityEngine;

namespace GRT
{
    public enum OverstepMode
    {
        Clamp,
        Cycle,
        PingPong
    }

    public struct Int32Range : IEquatable<Int32Range>
    {
        private readonly int _a;
        private readonly int _b;

        private int _value;

        public OverstepMode Mode { get; set; }

        public int A => _a;
        public int B => Mode == OverstepMode.Cycle ? _b - 1 : _b;

        public int Value => _value;

        public Int32Range(int a = 0, int b = 100, OverstepMode mode = OverstepMode.Clamp)
        {
            Mode = mode;

            _a = a;
            _b = Mode == OverstepMode.Cycle ? b + 1 : b;

            _value = a;
        }

        public int Set(int i)
        {
            switch (Mode)
            {
                case OverstepMode.Cycle:
                    _value = i.Cycle(_a, _b);
                    break;

                case OverstepMode.PingPong:
                    _value = (int)Mathf.PingPong(i, _b - _a);
                    break;

                case OverstepMode.Clamp:
                default:
                    _value = i.Clamp(_a, _b);
                    break;
            }

            return _value;
        }

        public bool Equals(Int32Range other)
        {
            return _value == other._value
                && _a == other._a
                && _b == other._b;
        }

        public override bool Equals(object obj) => obj is Int32Range range && Equals(range);

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = (int)2166136261;
                hash = (hash * 16777619) ^ _value.GetHashCode();
                hash = (hash * 16777619) ^ _a.GetHashCode();
                hash = (hash * 16777619) ^ _b.GetHashCode();
                return hash;
            }
        }

        public static implicit operator int(Int32Range range) => range._value;

        public static bool operator ==(Int32Range a, Int32Range b) => a.Equals(b);

        public static bool operator !=(Int32Range a, Int32Range b) => !a.Equals(b);
    }

    public struct SingleRange : IEquatable<SingleRange>
    {
        private readonly float _a;
        private readonly float _b;

        private float _value;

        public OverstepMode Mode { get; set; }

        public float A => _a;
        public float B => _b;

        public float Value => _value;

        public SingleRange(float a = 0f, float b = 1f, OverstepMode mode = OverstepMode.Clamp)
        {
            _a = a;
            _b = b;

            Mode = mode;

            _value = a;
        }

        public float Set(float i)
        {
            switch (Mode)
            {
                case OverstepMode.Cycle:
                    _value = i.Cycle(_a, _b);
                    break;

                case OverstepMode.PingPong:
                    _value = Mathf.PingPong(i, _b - _a);
                    break;

                case OverstepMode.Clamp:
                default:
                    _value = i.Clamp(_a, _b);
                    break;
            }

            return _value;
        }

        public bool Equals(SingleRange other)
        {
            return _value == other._value
                && _a == other._a
                && _b == other._b;
        }

        public override bool Equals(object obj) => obj is SingleRange range && Equals(range);

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = (int)2166136261;
                hash = (hash * 16777619) ^ _value.GetHashCode();
                hash = (hash * 16777619) ^ _a.GetHashCode();
                hash = (hash * 16777619) ^ _b.GetHashCode();
                return hash;
            }
        }

        public static implicit operator float(SingleRange range) => range._value;

        public static bool operator ==(SingleRange a, SingleRange b) => a.Equals(b);

        public static bool operator !=(SingleRange a, SingleRange b) => !a.Equals(b);
    }
}