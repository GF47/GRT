namespace GRT.GEC
{
    public interface IGComponent<T, TE> where T : class where TE : IGEntity<T, TE>
    {
        TE Entity { get; set; }
    }
}
