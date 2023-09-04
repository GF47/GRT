using System.Collections.Generic;

namespace GRT.GEC
{
    public interface IGEntity<T, TE> : IProvider<T>
        where T : class
        where TE : IGEntity<T, TE>
    {
        string Location { get; }

        IList<IGComponent<T, TE>> Components { get; }
    }
}