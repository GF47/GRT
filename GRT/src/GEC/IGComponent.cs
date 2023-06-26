namespace GRT.GEC
{
    public interface IGComponent<T> where T : class
    {
        IGEntity<T> GEntity { get; set; }
    }
}
