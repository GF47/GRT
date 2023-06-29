using System;
using UnityEngine;

namespace GRT
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class DraggableAssetPathAttribute : PropertyAttribute
    {
        public string Name { get; }

        public DraggableAssetPathAttribute(string name = null)
        {
            Name = name;
        }
    }
}