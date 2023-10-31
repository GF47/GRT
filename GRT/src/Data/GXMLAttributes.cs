using System;

namespace GRT.Data
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
    public class GXNodeAttribute : Attribute, IGXAttribute
    {
        public string Name { get; set; }

        public string Default { get; set; }

        public int Decimal { get; set; }

        public int Priority { get; set; } = -1; // not implemented
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true)]
    public class GXAttributeAttribute : Attribute, IGXAttribute
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
    }

    public interface IGXAttribute
    {
        string Name { get; set; }

        string Default { get; set; }

        int Decimal { get; set; }
    }

    public struct GXAttributeMock : IGXAttribute
    {
        public string Name { get; set; }

        public string Default { get; set; }

        public int Decimal { get; set; }
    }

    public static class GXAttributeExtensions
    {
        public const string NULL = "null";

        public static (string, string, int) GetProperties(this IGXAttribute attribute, string defaultName = default, string defaultDefault = default, int defaultDecimal = 2)
        {
            if (attribute == null)
            {
                return (defaultName, defaultDefault, defaultDecimal);
            }
            else
            {
                return (GetValidName(attribute.Name, defaultName), attribute.Default ?? defaultDefault, attribute.Decimal);
            }
        }

        public static string GetValidName(params string[] names)
        {
            foreach (var name in names)
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    continue;
                }

                return name;
            }

            throw new ArgumentException("no valid name", nameof(names));
        }
    }
}