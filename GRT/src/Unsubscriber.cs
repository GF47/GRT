using System;
using System.Collections.Generic;

namespace GRT
{
    public class Unsubscriber<T> : IDisposable
    {
        private readonly IObserver<T> _observer;
        private readonly Action<IObserver<T>> _action;

        public Unsubscriber(IObserver<T> observer, Action<IObserver<T>> action)
        {
            _observer = observer;
            _action = action;
        }

        public void Dispose() => _action?.Invoke(_observer);
    }

    public readonly struct CollectionUnsubscriber<T> : IDisposable
    {
        private readonly WeakReference<ICollection<IObserver<T>>> _collection;

        private readonly IObserver<T> _observer;

        public CollectionUnsubscriber(ICollection<IObserver<T>> collection, IObserver<T> observer)
        {
            _collection = new WeakReference<ICollection<IObserver<T>>>(collection);
            _observer = observer;
        }

        public void Dispose()
        {
            if (_observer != null && _collection.TryGetTarget(out var observers))
            {
                observers?.Remove(_observer);
            }
        }
    }
}