using System;

namespace GRT
{
    public interface ISubscriber<T>
    {
        void Subscribe(IObservable<T> observable);
    }
}