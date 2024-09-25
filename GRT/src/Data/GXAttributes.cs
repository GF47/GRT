using System;

namespace GRT.Data
{
    public interface IGXAttribute
    {
        string Name { get; set; }
        string Default { get; set; }
    }

    /// <summary>
    /// 虚拟的属性
    /// </summary>
    public class GXMockAttribute : IGXAttribute
    {
        public string Name { get; set; }
        public string Default { get; set; }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
    public class GXNodeAttribute : Attribute, IGXAttribute
    {
        public string Name { get; set; }
        public string Default { get; set; }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true)]
    public class GXKVPairAttribute : Attribute, IGXAttribute
    {
        public string Name { get; set; }
        public string Default { get; set; }

        /// <summary>
        /// 其值为所修饰的字段或属性所在类内的某个静态方法名
        /// </summary>
        public string Stringifier { get; set; }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true)]
    public class GXArrayAttribute : Attribute
    {
        public string Container { get; set; }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true)]
    public class GXArrayItemAttribute : Attribute, IGXAttribute
    {
        public string Name { get; set; }
        public string Default { get; set; }

        /// <summary>
        /// 其值为所修饰的字段或属性所在类内的某个静态方法名
        /// </summary>
        public string Stringifier { get; set; }
    }

    public static class GXExtensions
    {
        public const string NULL = "null";

        public const char SEPARATOR = '|';

        public static (string, string, string) Names(this IGXAttribute attribute)
        {
            if (string.IsNullOrWhiteSpace(attribute.Name))
            {
                return default;
            }
            else
            {
                var slices = attribute.Name.Split(SEPARATOR);
                return (slices.Length > 0 ? slices[0] : default,
                    slices.Length > 1 ? slices[1] : default,
                    slices.Length > 2 ? slices[2] : default);
            }
        }

        public static string DefaultName(this IGXAttribute attribute)
        {
            if (string.IsNullOrWhiteSpace(attribute.Name))
            {
                return default;
            }
            else
            {
                var slices = attribute.Name.Split(SEPARATOR);
                return slices.Length > 0 ? slices[0] : default;
            }
        }

        public static (string, string, string) Names(this GXArrayAttribute attribute)
        {
            if (string.IsNullOrWhiteSpace(attribute.Container))
            {
                return default;
            }
            else
            {
                var slices = attribute.Container.Split(SEPARATOR);
                return (slices.Length > 0 ? slices[0] : default,
                    slices.Length > 1 ? slices[1] : default,
                    slices.Length > 2 ? slices[2] : default);
            }
        }

        public static string DefaultName(this GXArrayAttribute attribute)
        {
            if (string.IsNullOrWhiteSpace(attribute.Container))
            {
                return default;
            }
            else
            {
                var slices = attribute.Container.Split(SEPARATOR);
                return slices.Length > 0 ? slices[0] : default;
            }
        }

        public static string GetValidName(params string[] names)
        {
            foreach (string name in names)
            {
                if (!string.IsNullOrWhiteSpace(name))
                {
                    return name;
                }
            }

            throw new ArgumentException("no valid name", nameof(names));
        }
    }
}