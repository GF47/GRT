namespace GRT.GUpdater.Buffers
{
    public interface IBuffer<T> : IGUpdater, IInterpolable<T>, IPercent
    {
        T Value { get; set; }

        float Duration { get; set; }
    }
}