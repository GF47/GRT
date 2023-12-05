namespace GRT.GUpdater.Buffers
{
    public interface IBuffer<T> : IGUpdater, IProjecter01<T>, IPercent
    {
        T Value { get; set; }

        float Duration { get; set; }
    }
}