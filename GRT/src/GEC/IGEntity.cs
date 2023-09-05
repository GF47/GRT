using System.Collections.Generic;

namespace GRT.GEC
{
    public interface IGEntity<T, TE>
        where T : class
        where TE : IGEntity<T, TE>
    {
        string Location { get; }

        T Puppet { get; }

        IList<IGComponent<T, TE>> Components { get; }
    }
}