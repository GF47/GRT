namespace GRT
{
    public interface IProjecter01<T>
    {
        T From { get; set; }
        T To { get; set; }
        T Project(float percent);
    }
}
