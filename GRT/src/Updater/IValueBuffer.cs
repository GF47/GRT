using System;

namespace GRT.Updater
{
    public interface IValueBuffer<T> : IProjecter01<T>, IPercent
    {
        event Action<T> Buffering;

        event Action<T> Starting;

        event Action<T> Stopping;

        bool IsBuffering { get; set; }

        T Value { get; set; }

        float Duration { get; set; }

        void Clear();
    }
}