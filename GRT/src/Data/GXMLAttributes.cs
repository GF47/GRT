using System;

namespace GRT.Data
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
    public class GXNodeAttribute : Attribute
    {
        public string Name { get; set; }

        public string Default { get; set; }

        public int Decimal { get; set; }

        public int Priority { get; set; } = -1; // not implemented
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true)]
    public class GXAttributeAttribute : Attribute
    {
        public string Name { get; set; }

        public string Default { get; set; }

        public int Decimal { get; set; }

        public int Priority { get; set; } = -1; // not implemented
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true)]
    public class GXArrayAttribute : Attribute
    {
        public string Container { get; set; }

        public string ItemName { get; set; }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true)]
    public class GXIgnoreAttribute : Attribute
    {
    }
}