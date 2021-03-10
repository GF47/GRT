using System;

namespace GRT.Updater
{
    public abstract class AbstractBuffer<T> : IValueBuffer<T>
    {
        public event Action<T> Buffering;

        public event Action<T> Starting;

        public event Action<T> Stopping;

        public bool IsBuffering
        {
            get => _updateNode.IsUpdating;
            set
            {
                if (_updateNode.IsUpdating == value) { return; }

                if (_updateNode.IsUpdating)
                {
                    _updateNode.Stop();
                    Stopping?.Invoke(Value);
                }
                else
                {
                    _updateNode.Start();
                    Starting?.Invoke(Value);
                }
            }
        }

        T IProjecter01<T>.From
        {
            get => _from;
            set { _from = value; _gap = Subtraction(_to, _from); }
        }
        private T _from;

        public T To
        {
            get => _to;
            set
            {
                _from = _value;
                _to = value;
                _gap = Subtraction(_to, _from);
                IsBuffering = IsValidValue(_gap);
            }
        }

        private T _to;
        private T _gap;

        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                Buffering?.Invoke(_value);
            }
        }

        private T _value;

        public float Duration { get => _duration; set => _duration = Math.Max(value, 0.02f); }
        private float _duration;

        public float Percent
        {
            get => IsValidValue(_gap) ? Division(Subtraction(_value, _from), _gap) : 1f;
            set
            {
                Value = Project01(Math.Max(Math.Min(value, 1f), 0f));

                if (value >= 1f) { IsBuffering = false; }
            }
        }

        private PerFrameUpdateNode _updateNode;

        protected AbstractBuffer(T from, Action<T> buffering, float duration = 1f)
        {
            _from = from;
            _value = from;
            To = from;

            _updateNode = new PerFrameUpdateNode(Update);

            Buffering = buffering;
            Duration = duration;
        }

        public void Update(float delta)
        {
            Percent += delta / _duration;
        }

        public void Clear()
        {
            Buffering = null;
            Starting = null;
            Stopping = null;
        }

        T IProjecter01<T>.Project(float percent)
        {
            return Project01(percent);
        }

        protected T Project01(float percent)
        {
            return Addition(Multiplication(percent, _gap), _from);
        }

        protected abstract T Addition(T a, T b);

        protected abstract T Subtraction(T a, T b);

        protected abstract T Multiplication(float m, T v);

        protected abstract float Division(T v, T d);

        protected abstract bool IsValidValue(T v);
    }
}
