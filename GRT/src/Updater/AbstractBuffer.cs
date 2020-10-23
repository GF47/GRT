using System;

namespace GRT.Updater
{
    public abstract class AbstractBuffer<T> : IValueBuffer<T>
    {
        public Action<T> OnBuffering { get; set; }
        public Action<T> OnStart { get; set; }
        public Action<T> OnStop { get; set; }

        public bool IsBuffering
        {
            get => _isBuffering;
            set
            {
                if (_isBuffering == value) { return; }


                _isBuffering = value;

                if (_isBuffering)
                {
                    _updateNode.Start();
                    OnBuffering?.Invoke(Value);
                }
                else
                {
                    _updateNode.Stop();
                    OnStop?.Invoke(Value);
                }
            }
        }
        private bool _isBuffering;

        public T From
        {
            get => _from;
            set
            {
                _from = value;
                _gap = Subtraction(_to, _from);
            }
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
                _percent = 0f;
                IsBuffering = IsValueGreaterThanTMin(_gap);
            }
        }
        private T _to;
        private T _gap;

        public T Value
        {
            get => _value;
            set
            {
                if (Division(Subtraction(_value, _from), _gap, out float result))
                {
                    Percent = result;
                }
            }
        }
        private T _value;

        public float Duration { get => _duration; set => _duration = Math.Max(value, 0.01f); }
        private float _duration;

        public float Percent 
        {
            get => _percent;
            set
            {
                _percent = Math.Max(Math.Min(value, 1f), 0f);

                _value = Project01(_percent);

                OnBuffering?.Invoke(_value);

                if (value >= 1f) { IsBuffering = false; }
            }
        }
        private float _percent;

        private PerFrameUpdateNode _updateNode;

        protected AbstractBuffer(T from, Action<T> onBuffering, float duration = 1f)
        {
            _from = from;
            _value = from;
            To = from;

            _updateNode = new PerFrameUpdateNode(Update);

            OnBuffering = onBuffering;
            Duration = duration;
        }

        public void Update(float delta) { Percent += delta / _duration; }

        public void Clear()
        {
            OnBuffering = null;
            OnStart = null;
            OnStop = null;
        }

        T IProjecter01<T>.Project(float percent)
        {
            return Project01(percent);
        }
        protected T Project01(float percent) { return Addition(Multiplication(percent, _gap), _from); }

        protected abstract T Addition(T a, T b);
        protected abstract T Subtraction(T a, T b);
        protected abstract T Multiplication(float m, T v);
        protected abstract bool Division(T v, T d, out float result);
        protected abstract bool IsValueGreaterThanTMin(T v);
    }
}
