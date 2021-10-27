using System;

namespace GRT.Updater
{
    public interface IValueBuffer<T> : IUpdater, IProjecter01<T>, IPercent
    {
        T Value { get; set; }

        float Duration { get; set; }
    }
}