﻿using System;
using System.Collections.Generic;

namespace GRT.GEC
{
    public interface IGEntity<T> where T : class
    {
        WeakReference<T> Reference { get; }

        T Target { get; }

        string Location { get; }

        IList<IGComponent<T>> Components { get; }
    }
}