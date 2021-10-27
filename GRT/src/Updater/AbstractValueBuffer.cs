using System;

namespace GRT.Updater
{
    public abstract class AbstractValueBuffer<T> : IValueBuffer<T>
    {
        private float _duration;
        private bool _isAlive;
        private T _value;
        private T _from;
        private T _to;
        private T _diff;
        private float _percent;

        public event Action<T> Updating;
        public event Action<T> Starting;
        public event Action<T> Stopping;

        UpdateType IUpdater.Type => UpdateType.Frame;

        public float Duration { get => _duration; set => _duration = Math.Max(0.02f, value); }

        public bool IsAlive => _isAlive;

        public T Value { get => _value; set => Updating?.Invoke(_value = value); }

        T IProjecter01<T>.From
        {
            get => _from; set
            {
                _from = value;
                _diff = Subtraction(_to, _from);
            }
        }

        public T To
        {
            get => _to; set
            {
                _from = _value;
                _to = value;
                _diff = Subtraction(_to, _from);
                Update(_percent = 0f);
                if (IsValid(_diff)) { Start(); }
                else { Stop(); }
            }
        }

        public float Percent
        {
            get => _percent; set
            {
                _percent = Math.Max(Math.Min(value, 1f), 0f);
                Value = Project01(_percent);

                if (value >= 1f) { Stop(); }
            }
        }

        protected AbstractValueBuffer(T from, Action<T> updating, float duration = 1f)
        {
            _from = from;
            _to = from;
            _value = from;
            _diff = Subtraction(_to, _from);

            Updating = updating;
            Duration = duration;
        }

        public void Start()
        {
            if (_isAlive) { return; }

            UpdateDriver.Add(this);
            _isAlive = true;

            Starting?.Invoke(_value);
        }

        public void Update(float delta)
        {
            Percent += delta / _duration;
        }

        public void Stop()
        {
            if (!_isAlive) { return; }

            UpdateDriver.Remove(this);
            _isAlive = false;

            Stopping?.Invoke(_value);
        }

        T IProjecter01<T>.Project(float percent) => Project01(percent);

        protected T Project01(float p) => Addition(Multiplication(p, _diff), _from);

        protected abstract T Addition(T a, T b);

        protected abstract T Subtraction(T a, T b);

        protected abstract T Multiplication(float m, T v);

        protected abstract bool IsValid(T v);
    }
}