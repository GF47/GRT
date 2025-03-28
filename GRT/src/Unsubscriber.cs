using System;

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
}