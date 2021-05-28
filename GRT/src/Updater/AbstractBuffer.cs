using System;

namespace GRT.Updater
{
    public abstract class AbstractBuffer<T> : IValueBuffer<T>
    {
        public event Action<T> Buffering;

        public event Action<T> Starting;

        public event Action<T> Stopping;

        protected event Action<float> updating;

        event Action<float> IUpdateNode.Updating
        {
            add { updating += value; }
            remove { updating -= value; }
        }

        UpdateType IUpdateNode.Type => UpdateType.PerFrame;

        public bool IsActive
        {
            get => isActive;
            set
            {
                if (isActive != value)
                {
                    if (value) { Start(); }
                    else { Stop(); }
                }
#if UNITY_EDITOR
                else
                {
                    UnityEngine.Debug.LogWarning($"{GetType()} is {(isActive ? "updating" : "stopped")}, Start failed");
                }
#endif
            }
        }

        protected bool isActive;

        T IProjecter01<T>.From
        {
            get => _from;
            set { _from = value; _difference = Subtraction(_to, _from); }
        }

        private T _from;

        public T To
        {
            get => _to;
            set
            {
                _from = _value;
                _to = value;
                _difference = Subtraction(_to, _from);
                Update(0f);
                IsActive = IsValidValue(_difference);
            }
        }

        private T _to;
        private T _difference;

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
            get => IsValidValue(_difference) ? Division(Subtraction(_value, _from), _difference) : 1f;
            set
            {
                Value = Project01(Math.Max(Math.Min(value, 1f), 0f));

                if (value >= 1f) { IsActive = false; }
            }
        }

        protected AbstractBuffer(T from, Action<T> buffering, float duration = 1f)
        {
            _from = from;
            _to = from;
            _difference = Subtraction(_to, _from);

            Buffering = buffering;
            Duration = duration;
        }

        public virtual void Start()
        {
            isActive = MonoUpdater.Add(this);
            Starting?.Invoke(_value);
        }

        public void Update(float delta)
        {
            updating?.Invoke(delta);
            Percent += delta / _duration;
        }

        public virtual void Stop()
        {
            isActive = MonoUpdater.Remove(this);
            Stopping?.Invoke(_value);
        }

        public void Clear()
        {
            Stop();

            Buffering = null;
            Starting = null;
            Stopping = null;

            updating = null;
        }

        T IProjecter01<T>.Project(float percent) => Project01(percent);

        protected T Project01(float percent) => Addition(Multiplication(percent, _difference), _from);

        protected abstract T Addition(T a, T b);

        protected abstract T Subtraction(T a, T b);

        protected abstract T Multiplication(float m, T v);

        protected abstract float Division(T v, T d);

        protected abstract bool IsValidValue(T v);
    }
}