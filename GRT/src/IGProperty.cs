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

    public interface IGPropertyDecorator<T> : IGProperty<T>
    {
        IGProperty<T> RawProperty { get; }
    }

    public static class GPropertyExtensions
    {
        public static T Get<T>(this IGProperty<T> property, T @default = default) => property.TryGet(out var t) ? t : @default;

        public static bool ValueEqual<T>(T a, T b, IEqualityComparer<T> comparer)
        {
            return comparer == null
                ? a == null
                    ? b == null || b.Equals(a)
                    : a.Equals(b)
                : comparer.Equals(a, b);
        }

        public static IDisposable Subscribe<T>(ICollection<IObserver<(IGProperty<T>, T)>> observers, IObserver<(IGProperty<T>, T)> observer)
        {
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
            }

            // 重复取消注册，不会出问题
            return new CollectionUnsubscriber<(IGProperty<T>, T)>(observers, observer);
        }
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

    public class Predicated<T> : IGProperty<T>
    {
        public string Name { get; private set; }

        private readonly Predicate<T> _getPredication;
        private readonly Predicate<T> _setPredication;

        private T _value;

        public Predicated(string name, Predicate<T> get, Predicate<T> set)
        {
            _getPredication = get;
            _setPredication = set;
            _value = default;
            Name = name;
        }

        public bool TryGet(out T value)
        {
            if (_getPredication == null || _getPredication.Invoke(_value))
            {
                value = _value;
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }

        public bool TrySet(T value)
        {
            if (_setPredication == null || _setPredication.Invoke(value))
            {
                _value = value;
                return true;
            }
            else
            {
                return false;
            }
        }

        public static implicit operator T(Predicated<T> property) => property.Get();
    }

    public class PredicatedDecorator<T> : IGPropertyDecorator<T>
    {
        private readonly IGProperty<T> _property;

        private readonly Predicate<T> _getPredicate;
        private readonly Predicate<T> _setPredicate;

        public string Name => _property.Name;

        public IGProperty<T> RawProperty => _property;

        public PredicatedDecorator(IGProperty<T> property, Predicate<T> get, Predicate<T> set)
        {
            _property = property;
            _getPredicate = get;
            _setPredicate = set;
        }

        public bool TryGet(out T value)
        {
            if (_property == null)
            {
                value = default;
                return false;
            }
            else
            {
                // 内部已经触发了get，不好
                return _property.TryGet(out value) && (_getPredicate == null || _getPredicate.Invoke(value));
            }
        }

        public bool TrySet(T value) =>
            _property != null
            && (_setPredicate == null || _setPredicate.Invoke(value))
            && _property.TrySet(value);

        public static implicit operator T(PredicatedDecorator<T> property) => property.Get();
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

    public class ReadonlyDecorator<T> : IGPropertyDecorator<T>
    {
        private readonly IGProperty<T> _property;

        private bool _initialized;

        public string Name => _property.Name;

        public IGProperty<T> RawProperty => _property;

        public ReadonlyDecorator(IGProperty<T> property)
        {
            _property = property;
            _initialized = default;
        }

        public bool TryGet(out T value) => _property.TryGet(out value);

        public bool TrySet(T value) =>
            _property != null
            && !_initialized
            && (_initialized = _property.TrySet(value));

        public static implicit operator T(ReadonlyDecorator<T> property) => property.Get();
    }

    public class Observable<T> : IGProperty<T>, IObservable<(IGProperty<T>, T)>, IDisposable
    {
        private T _value;

        private readonly IEqualityComparer<T> _equalityComparer;

        private readonly IList<IObserver<(IGProperty<T>, T)>> _observers;

        public string Name { get; private set; }

        public Observable(string name, IEqualityComparer<T> equalityComparer = null)
        {
            Name = name;
            _equalityComparer = equalityComparer ?? EqualityComparer<T>.Default;
            _observers = new List<IObserver<(IGProperty<T>, T)>>();
        }

        public bool TryGet(out T value)
        {
            value = _value;
            return true;
        }

        public bool TrySet(T value)
        {
            if (!GPropertyExtensions.ValueEqual(_value, value, _equalityComparer))
            {
                var temp = _value;
                _value = value;

                foreach (var observer in _observers)
                {
                    observer.OnNext((this, temp));
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public IDisposable Subscribe(IObserver<(IGProperty<T>, T)> observer)
        {
            return GPropertyExtensions.Subscribe(_observers, observer);
        }

        public void Dispose()
        {
            for (int i = _observers.Count - 1; -1 < i; i--)
            {
                var observer = _observers[i];
                _observers.RemoveAt(i);
                observer.OnCompleted();
            }
        }

        public static implicit operator T(Observable<T> property) => property.Get();
    }

    public class ObservableDecorator<T> : IGPropertyDecorator<T>, IObservable<(IGProperty<T>, T)>, IDisposable
    {
        private readonly IGProperty<T> _property;

        private readonly IEqualityComparer<T> _equalityComparer;

        private readonly IList<IObserver<(IGProperty<T>, T)>> _observers;

        public string Name => _property.Name;

        public IGProperty<T> RawProperty => _property;

        public ObservableDecorator(IGProperty<T> property, IEqualityComparer<T> equalityComparer = null)
        {
            _property = property;
            _equalityComparer = equalityComparer ?? EqualityComparer<T>.Default;
            _observers = new List<IObserver<(IGProperty<T>, T)>>();
        }

        public bool TryGet(out T value)
        {
            return _property.TryGet(out value);
        }

        public bool TrySet(T value)
        {
            var temp = _property.Get();
            var done = _property.TrySet(value);

            if (!GPropertyExtensions.ValueEqual(_property.Get(), temp, _equalityComparer))
            {
                foreach (var observer in _observers)
                {
                    observer.OnNext((this, temp));
                }
            }

            return done;
        }

        public IDisposable Subscribe(IObserver<(IGProperty<T>, T)> observer)
        {
            return GPropertyExtensions.Subscribe(_observers, observer);
        }

        public void Dispose()
        {
            for (int i = _observers.Count - 1; -1 < i; i--)
            {
                var observer = _observers[i];
                _observers.RemoveAt(i);
                observer.OnCompleted();
            }
        }

        public static implicit operator T(ObservableDecorator<T> property) => property.Get();
    }
}