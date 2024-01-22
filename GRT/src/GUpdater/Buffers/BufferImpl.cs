using System;
using UnityEngine;

namespace GRT.GUpdater.Buffers
{
    public class IntBuffer : BaseBuffer<int>
    {
        public IntBuffer(int from, Action<int> updating, float duration = 1) : base(from, updating, duration)
        {
        }

        public override int Interpolate(float percent) => IsEqual(From, To) ? To : (From + (int)(percent * (To - From)));

        protected override bool IsEqual(int a, int b) => a == b;
    }

    public class SingleBuffer : BaseBuffer<float>
    {
        public SingleBuffer(float from, Action<float> updating, float duration = 1) : base(from, updating, duration)
        {
        }

        public override float Interpolate(float percent) => IsEqual(From, To) ? To : (From + (percent * (To - From)));

        protected override bool IsEqual(float a, float b) => Math.Abs(a - b) < 1e-6f;
    }

    public class Vector2Buffer : BaseBuffer<Vector2>
    {
        public Vector2Buffer(Vector2 from, Action<Vector2> updating, float duration = 1) : base(from, updating, duration)
        {
        }

        public override Vector2 Interpolate(float percent) => Vector2.Lerp(From, To, percent);

        protected override bool IsEqual(Vector2 a, Vector2 b) => (a - b).magnitude < 1e-6f;
    }

    public class Vector3Buffer : BaseBuffer<Vector3>
    {
        public Vector3Buffer(Vector3 from, Action<Vector3> updating, float duration = 1) : base(from, updating, duration)
        {
        }

        public override Vector3 Interpolate(float percent) => Vector3.Lerp(From, To, percent);

        protected override bool IsEqual(Vector3 a, Vector3 b)
        {
            return (a - b).magnitude < 1e-6f;
        }
    }

    public class QuaternionBuffer : BaseBuffer<Quaternion>
    {
        public QuaternionBuffer(Quaternion from, Action<Quaternion> updating, float duration = 1) : base(from, updating, duration)
        {
        }

        public override Quaternion Interpolate(float percent) => Quaternion.Slerp(From, To, percent);

        protected override bool IsEqual(Quaternion a, Quaternion b)
        {
            (a * Quaternion.Inverse(b)).ToAngleAxis(out var angle, out _);
            return Math.Abs(angle) < 1e-6f;
        }
    }
}