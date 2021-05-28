using System;

namespace GRT.Updater
{
    public interface IValueBuffer<T> : IUpdateNode, IProjecter01<T>, IPercent
    {
        event Action<T> Buffering;

        event Action<T> Starting;

        event Action<T> Stopping;

        T Value { get; set; }

        float Duration { get; set; }
    }
}