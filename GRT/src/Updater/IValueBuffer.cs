using System;

namespace GRT.Updater
{
    public interface IValueBuffer<T> : IProjecter01<T>, IPercent
    {
        Action<T> OnBuffering { get; set; }
        Action<T> OnStart { get; set; }
        Action<T> OnStop { get; set; }

        bool IsBuffering { get; set; }

        T Value { get; set; }

        float Duration { get; set; }

        void Clear();
    }
}
