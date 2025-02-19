using System;

namespace GRT.GUpdater.Buffers
{
    public abstract class BaseBuffer<T> : MiniGLifecycleWithTick<T>, IInterpolable<T>, IPercent
    {
        private float _duration;

        private T _to;

        private float _percent;

        public event Action<T> Updating;

        public event Action<IGStartable<T>> StartingOneShot;

        public event Action<IGDisposable<T>> DisposingOneShot;

        public T Value
        {
            get => Object; set
            {
                Object = value;
                Updating?.Invoke(Object);
            }
        }

        public float Duration { get => _duration; set => _duration = Math.Max(0.02f, value); }

        public T From { get; set; }

        public T To
        {
            get => _to; set
            {
                From = Object;
                _to = value;

                Percent = 0f;
            }
        }

        public float Percent
        {
            get => _percent; set
            {
                _percent = Math.Max(Math.Min(value, 1f), 0f);
                Value = Interpolate(_percent);

                if (value >= 1f) { this.DetachFromScope(true); }
            }
        }

        protected BaseBuffer(T from, Action<T> updating, float duration = 1f) : base(from)
        {
            From = from;
            _to = from;

            Updating = updating;

            Duration = duration;
        }

        public abstract T Interpolate(float percent);

        protected abstract bool IsEqual(T a, T b);

        public override void GStart()
        {
            base.GStart();
            StartingOneShot?.Invoke(this);
            StartingOneShot = null;
        }

        public override void GTick(float delta)
        {
            Percent += delta / Duration;

            base.GTick(delta);
        }

        public override void GDispose()
        {
            base.GDispose();
            DisposingOneShot?.Invoke(this);
            DisposingOneShot = null;
        }

        public void ClearOneShotEvents()
        {
            StartingOneShot = null;
            DisposingOneShot = null;
        }
    }
}