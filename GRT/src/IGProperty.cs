using System;
using System.Collections.Generic;

namespace GRT
{
    public interface IGProperty<T>
    {
        string Name { get; }

        bool TryGet(out T value);

        bool TrySet(T value);
    }

    public static class GPropertyExtensions
    {
        public static T Get<T>(this IGProperty<T> property, T @default = default) => property.TryGet(out var t) ? t : @default;
    }

    public class MiniGProperty<T> : IGProperty<T>
    {
        public string Name { get; private set; }

        private T _value;

        public MiniGProperty(string name)
        {
            _value = default;
            Name = name;
        }

        public bool TryGet(out T value)
        {
            value = _value;
            return true;
        }

        public bool TrySet(T value)
        {
            _value = value;
            return true;
        }

        public static implicit operator T(MiniGProperty<T> property) => property.Get();
    }

    public class Readonly<T> : IGProperty<T>
    {
        private bool _initialized;

        private T _value;

        public string Name { get; private set; }

        public Readonly(string name)
        {
            _initialized = default;
            _value = default;

            Name = name;
        }

        public bool TryGet(out T value)
        {
            value = _value;
            return _initialized;
        }

        public bool TrySet(T value)
        {
            if (_initialized)
            {
                return false;
            }
            else
            {
                _value = value;
                _initialized = true;
                return true;
            }
        }

        public static implicit operator T(Readonly<T> property) => property.Get();
    }

    public class ObservableGProperty<T> : IGProperty<T>, IGLife<T>, IGStartable<T>, IGDisposable<T>, IObservable<(IGProperty<T>, T)>
    {
        private readonly ICollection<IObserver<(IGProperty<T>, T)>> _observers;

        private readonly IGProperty<T> _property;

        private readonly Func<T, T, bool> _equalFunction;

        private bool TEqual(T a, T b) => _equalFunction == null ? a.Equals(b) : _equalFunction.Invoke(a, b);

        public string Name => _property.Name;

        public T Object
        {
            get => _property.Get();
            set => TrySet(value);
        }

        public bool IsAlive { get; private set; }

        public IGScope Scope { get; set; }

        public ObservableGProperty(IGProperty<T> property) : this(property, null)
        {
        }

        public ObservableGProperty(IGProperty<T> property, Func<T, T, bool> equalFunc)
        {
            _observers = new List<IObserver<(IGProperty<T>, T)>>();
            _equalFunction = equalFunc;
            _property = property;
        }

        public IDisposable Subscribe(IObserver<(IGProperty<T>, T)> observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }

            // 重复取消注册，不会出问题
            return new Unsubscriber(this, observer);
        }

        public bool TryGet(out T value) => _property.TryGet(out value);

        public bool TrySet(T value)
        {
            var temp = _property.Get();
            var done = _property.TrySet(value);

            if (!TEqual(temp, _property.Get()))
            {
                foreach (var observer in _observers)
                {
                    observer.OnNext((this, temp));
                }
            }

            return done;
        }

        public void GStart()
        {
            IsAlive = true;
        }

        public void GDispose()
        {
            IsAlive = false;
            foreach (var observer in _observers)
            {
                observer.OnCompleted();
            }

            _observers.Clear();
        }

        public static implicit operator T(ObservableGProperty<T> property) => property.Get();

        private readonly struct Unsubscriber : IDisposable
        {
            private readonly ObservableGProperty<T> _observable;
            private readonly IObserver<(IGProperty<T>, T)> _observer;

            public Unsubscriber(ObservableGProperty<T> observable, IObserver<(IGProperty<T>, T)> observer)
            {
                _observable = observable;
                _observer = observer;
            }

            public void Dispose()
            {
                if (_observer != null)
                {
                    // 检测IsAlive是防止在ObservableGProperty主动GDispose的时候多次remove
                    if (_observable != null && _observable.IsAlive && _observable._observers != null)
                    {
                        _observable._observers.Remove(_observer);
                    }
                }
            }
        }
    }
}