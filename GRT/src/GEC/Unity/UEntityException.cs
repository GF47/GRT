using System;
using UnityEngine;

namespace GRT.GEC.Unity
{
    public class UEntityException : GEntityException<GameObject, UEntity>
    {
        public UEntityException(string path, string message) : base(path, message)
        {
        }

        public UEntityException(UEntity entity, string message) : base(entity, message)
        {
        }

        public UEntityException(string path, string message, Exception innerException) : base(path, message, innerException)
        {
        }

        public UEntityException(UEntity entity, string message, Exception innerException) : base(entity, message, innerException)
        {
        }
    }
}