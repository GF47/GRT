namespace GRT
{
    public interface IInterpolable<T>
    {
        T From { get; set; }
        T To { get; set; }
        T Interpolate(float percent);
    }
}
