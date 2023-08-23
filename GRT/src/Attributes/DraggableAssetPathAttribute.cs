using System;
using UnityEngine;

namespace GRT
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class DraggableAssetPathAttribute : PropertyAttribute
    {
        public string Name { get; }
        public string Tips { get; }

        public DraggableAssetPathAttribute(string name = null, string tips = null)
        {
            Name = name;
            Tips = tips;
        }
    }
}