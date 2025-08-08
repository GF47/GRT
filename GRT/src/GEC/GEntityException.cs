using System;

namespace GRT.GEC
{
    public class GEntityException<T, TE> : Exception
        where T : class
        where TE : IGEntity<T, TE>
    {
        public string EntityPath { get; private set; }

        public GEntityException(string path, string message)
            : base($"Entity Error[<color=#ffff00>{path}</color>].\n{message}")
        {
        }

        public GEntityException(IGEntity<T, TE> entity, string message)
            : this(entity.Location, message)
        {
        }

        public GEntityException(string path, string message, Exception innerException)
            : base($"Entity Error[<color=#ffff00>{path}</color>].\n{message}", innerException)
        {
        }

        public GEntityException(IGEntity<T, TE> entity, string message, Exception innerException)
            : this(entity.Location, message, innerException)
        {
        }
    }
}