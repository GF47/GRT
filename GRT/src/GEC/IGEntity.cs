using System;
using System.Collections.Generic;

namespace GRT.GEC
{
    public interface IGEntity<T> : IProvider<T> where T : class
    {
        string Location { get; }

        IList<IGComponent<T>> Components { get; }
    }
}