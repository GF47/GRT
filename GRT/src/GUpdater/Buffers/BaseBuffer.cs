using System;

namespace GRT.GUpdater.Buffers
{
    public abstract class BaseBuffer<T> : IBuffer<T>
    {
        private float _duration;

        private T _value;
        private T _to;

        private float _percent;

        public event Action<T> Starting;

        public event Action<T> Updating;

        public event Action<T> Stopping;

        public T Value
        {
            get => _value; set
            {
                _value = value;
                Updating?.Invoke(_value);
            }
        }

        public float Duration { get => _duration; set => _duration = Math.Max(0.02f, value); }

        public UpdateMode UpdateMode => UpdateMode.PerFrame;

        public bool IsAlive { get; protected set; }

        public T From { get; set; }

        public T To
        {
            get => _to; set
            {
                From = _value;
                _to = value;

                if (IsEqual(_to, From))
                {
                    Percent = 1f;
                    // if (IsAlive) { Stop(); }
                }
                else
                {
                    Percent = 0f;
                    if (!IsAlive) { Start(); }
                }
            }
        }

        public float Percent
        {
            get => _percent; set
            {
                _percent = Math.Max(Math.Min(value, 1f), 0f);
                Value = Project(_percent);

                if (value >= 1f) { Stop(); }
            }
        }

        protected BaseBuffer(T from, Action<T> updating, float duration = 1f)
        {
            From = from;
            _to = from;
            _value = from;

            Updating = updating;

            Duration = duration;
        }

        public abstract T Project(float percent);

        protected abstract bool IsEqual(T a, T b);

        public void Start()
        {
            if (IsAlive) { return; }

            GUpdaterDriver.Add(this);
            IsAlive = true;

            Starting?.Invoke(Value);
        }

        public void Update(float delta)
        {
            Percent += delta / Duration;
        }

        public void Stop()
        {
            if (!IsAlive) { return; }

            GUpdaterDriver.Remove(this);
            IsAlive = false;

            Stopping?.Invoke(Value);
        }
    }
}