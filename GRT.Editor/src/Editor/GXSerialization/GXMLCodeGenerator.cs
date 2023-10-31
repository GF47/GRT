﻿using GRT.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEditor;

namespace GRT.Editor.GXSerialization
{
    public static class GXMLCodeGenerator
    {
        private const BindingFlags FLAGS = BindingFlags.Instance | BindingFlags.Public; // | BindingFlags.NonPublic;

        [MenuItem("Assets/Project/Generate GXML Factories")]
        private static void Generate()
        {
        }

        private static string Generate(Type type)
        {
            var sb = new StringBuilder();
            sb.Append(TEMPLATE_USING);
            sb.Append(TEMPLATE_NAMESPACE_A);
            {
                sb.Append(TEMPLATE_CLASS_A(type));
                {
                    sb.Append(TEMPLATE_SERIALIZE(type));

                    sb.Append(TEMPLATE_SERIALIZE_EXPLICITLY_A(type));
                    {
                        var propertyInfos = type.GetProperties(FLAGS);
                        foreach (var info in propertyInfos)
                        {
                            sb.Append(TEMPLATE_SERIALIZE_MEMBER(info));
                        }

                        var fieldInfos = type.GetFields(FLAGS);
                        foreach (var info in fieldInfos)
                        {
                            sb.Append(TEMPLATE_SERIALIZE_MEMBER(info));
                        }
                    }
                    sb.Append(TEMPLATE_SERIALIZE_EXPLICITLY_B);

                    sb.Append(TEMPLATE_DESERIALIZE_EXPLICITLY_A(type));
                    {
                        var propertyInfos = type.GetProperties(FLAGS);
                        foreach (var info in propertyInfos)
                        {
                            sb.Append(TEMPLATE_DESERIALIZE_MEMBER(info));
                        }

                        var fieldInfos = type.GetFields(FLAGS);
                        foreach (var info in fieldInfos)
                        {
                            sb.Append(TEMPLATE_DESERIALIZE_MEMBER(info));
                        }
                    }
                    sb.Append(TEMPLATE_DESERIALIZE_EXPLICITLY_B);
                }
                sb.Append(TEMPLATE_CLASS_B);
            }
            sb.Append(TEMPLATE_NAMESPACE_B);

            return sb.ToString();
        }

        // private const string TAB = "    ";

        private const string TEMPLATE_USING = @"// auto generated code, do not change

using GRT.Data;
using System;
using System.Collections.Generic;
";

        private const string TEMPLATE_NAMESPACE_A = @"
namespace GXFactories
{";

        private const string TEMPLATE_NAMESPACE_B = @"
}";

        private static string TEMPLATE_CLASS_A(Type type) => $@"
    public class {type.FULL_NAME().Replace('.', '_')}_GXFactory<T> : IGXSerializableFactory<T, {type.FULL_NAME()}>
    {{";

        private const string TEMPLATE_CLASS_B = @"
    }";

        private static string TEMPLATE_SERIALIZE(Type type) => $@"
        public T Serialize(IGXML<T> xml, object obj, T parentNode, IGXAttribute refAttribute = default) =>
            Serialize(xml, obj as {type.FULL_NAME()}, parentNode, refAttribute);

        public object Deserialize(IGXML<T> xml, T node, IGXAttribute refAttribute = default, Func<object> constructor = default) =>
            DeserializeExplicitly(xml, node, refAttribute, constructor);
";

        private static string TEMPLATE_SERIALIZE_EXPLICITLY_A(Type type) => $@"
        public T Serialize(IGXML<T> xml, {type.FULL_NAME()} obj, T parentNode, IGXAttribute refAttribute = default)
        {{
            if (obj == null) {{ return default; }}

            var serializer = GXMLSerializer<T>.Instance;

            var node = xml.CreateChild(parentNode, GXMLSerializer<T>.GetValidName(refAttribute?.Name, ""{type.CLASS_NODE_NAME()}""));
";

        private const string TEMPLATE_SERIALIZE_EXPLICITLY_B = @"
            return node;
        }
";

        private static string TEMPLATE_SERIALIZE_MEMBER(MemberInfo member)
        {
            var mName = member.Name;
            if (member.HasAttribute<GXArrayAttribute>(out var ara))
            {
                var itemType = member.GetChildItemType();
                var itemRefAttribute = member.GetAttribute<GXNodeAttribute>();
                var itemDefAttribute = Attribute.GetCustomAttribute(itemType, typeof(GXNodeAttribute)) as GXNodeAttribute;
                return $@"
            if (obj.{mName} != null)
            {{
                var containerNode_{mName} = {(string.IsNullOrWhiteSpace(ara.Container) ? "node" : $@"xml.CreateChild(node, ""{ara.Container}"")")};
                var refAttribute_{mName} = new GXAttributeMock()
                {{
                    Name = {GetDefault(itemRefAttribute?.Name, itemDefAttribute?.Name, itemType.Name)},
                    Default = {GetDefault(itemRefAttribute?.Default, itemDefAttribute?.Default)},
                    Decimal = {itemRefAttribute?.Decimal ?? itemDefAttribute?.Decimal ?? 2},
                }};
                foreach (var item in obj.{mName})
                {{
                    GXMLSerializer<T>.Instance.Write(item, containerNode_{mName}, refAttribute_{mName});
                }}
            }}
";
            }
            else if (member.HasAttribute<GXNodeAttribute>(out var refAttribute))
            {
                return $@"
            {(GetMemberType(member).IsValueType ? "// " : string.Empty)}if (obj.{mName} != null)
            {{
                var node_{mName} = GXMLSerializer<T>.Instance.Write(obj.{mName}, node, new GXAttributeMock()
                {{
                    Name = {GetDefault(refAttribute?.Name, mName)},
                    Default = {GetDefault(refAttribute?.Default)},
                    Decimal = {refAttribute?.Decimal ?? 2},
                }});
            }}
";
            }
            else if (member.HasAttribute<GXAttributeAttribute>(out var ata))
            {
                return $@"
            xml.SetAttribute(node, {GetDefault(ata?.Name, mName)}, serializer.Stringify(obj.{mName}, {ata?.Decimal ?? 2}, {GetDefault(ata?.Default)}));
";
            }
            else
            {
                return string.Empty;
            }
        }

        private static string TEMPLATE_DESERIALIZE_EXPLICITLY_A(Type type)
        {
            var name = type.FULL_NAME();
            return $@"
        public {name} DeserializeExplicitly(IGXML<T> xml, T node, IGXAttribute refAttribute = default, Func<object> constructor = default)
        {{
            var serializer = GXMLSerializer<T>.Instance;

            var obj = (constructor?.Invoke() as {name}) ?? new {name}();
";
        }

        private const string TEMPLATE_DESERIALIZE_EXPLICITLY_B = @"
            return obj;
        }";

        private static string TEMPLATE_DESERIALIZE_MEMBER(MemberInfo member)
        {
            var mName = member.Name;
            var mType = GetMemberType(member);
            if (member.HasAttribute<GXArrayAttribute>(out var ara))
            {
                var itemType = member.GetChildItemType();
                var itemRefAttribute = member.GetAttribute<GXNodeAttribute>();
                var itemDefAttribute = Attribute.GetCustomAttribute(itemType, typeof(GXNodeAttribute)) as GXNodeAttribute;
                var itemNodeName = GetDefault(itemRefAttribute?.Name, itemDefAttribute?.Name, itemType.Name);
                var itemTypeFullName = itemType.FULL_NAME();

                return $@"
            var containerNode_{mName} = {(string.IsNullOrWhiteSpace(ara.Container) ? "node" : $@"xml.GetChild(node, ""{ara.Container}"")")};
            if (containerNode_{mName} != null)
            {{
                var itemNodes_{mName} = xml.GetChildren(containerNode_{mName}, {itemNodeName});
                if (itemNodes_{mName} != null)
                {{
                    var refAttribute_{mName} = new GXAttributeMock()
                    {{
                        Name = {itemNodeName},
                        Default = {GetDefault(itemRefAttribute?.Default, itemDefAttribute?.Default)},
                        Decimal = {itemRefAttribute?.Decimal ?? itemDefAttribute?.Decimal ?? 2},
                    }};

                    var list_{mName} = new List<{itemTypeFullName}>();
                    foreach (var item in itemNodes_{mName})
                    {{
                        list_{mName}.Add(serializer.Read(item, typeof({itemTypeFullName}), refAttribute_{mName}) as {itemTypeFullName});
                    }}

                    obj.{mName} = list_{mName}{(mType.IsArray ? ".ToArray()" : string.Empty)};
                }}
            }}
";
            }
            else if (member.HasAttribute<GXNodeAttribute>(out var refAttribute))
            {
                var mTypeFullName = mType.FULL_NAME();

                var nodeName = GetDefault(refAttribute?.Name, mName);

                return $@"
            var node_{mName} = xml.GetChild(node, {nodeName});
            if (node_{mName} != null)
            {{
                obj.{mName} = ({mTypeFullName})serializer.Read(node_{mName}, typeof({mTypeFullName}), new GXAttributeMock()
                {{
                    Name = {nodeName},
                    Default = {GetDefault(refAttribute?.Default)},
                    Decimal = {refAttribute?.Decimal ?? 2},
                }});
            }}
";
            }
            else if (member.HasAttribute<GXAttributeAttribute>(out var ata))
            {
                return $@"
            obj.{mName} = serializer.Construct<{mType.FULL_NAME()}>(xml.GetAttribute(node, {GetDefault(ata?.Name, mName)}, {GetDefault(ata?.Default)}));
";
            }
            else
            {
                return string.Empty;
            }
        }

        private static string FULL_NAME(this Type type)
        {
            var name = $"{type.Namespace}.{type.Name}";
            if (type.IsGenericType)
            {
                name += "<";
                var addComma = false;
                foreach (var arg in type.GetGenericArguments())
                {
                    if (addComma) { name += ", "; } else { addComma = true; }
                    name += arg.FULL_NAME();
                }
                name += ">";
            }
            return name;
        }

        private static string CLASS_NODE_NAME(this Type type) =>
            Attribute.GetCustomAttribute(type, typeof(GXNodeAttribute)) is GXNodeAttribute defAttribute && !string.IsNullOrWhiteSpace(defAttribute.Name)
                ? defAttribute.Name
                : type.Name;

        private static bool IsList(Type type)
        {
            if (typeof(IList).IsAssignableFrom(type))
            {
                return true;
            }

            foreach (var i in type.GetInterfaces())
            {
                if (i.IsGenericType && typeof(IList<>) == i.GetGenericTypeDefinition())
                {
                    return true;
                }
            }

            return false;
        }

        private static bool HasAttribute<T>(this MemberInfo member, out T attr) where T : Attribute
        {
            if (Attribute.GetCustomAttribute(member, typeof(T)) is T attribute)
            {
                attr = attribute;
                return true;
            }
            else
            {
                attr = default;
                return false;
            }
        }

        private static T GetAttribute<T>(this MemberInfo member) where T : Attribute =>
            Attribute.GetCustomAttribute(member, typeof(T)) as T;

        private static Type GetChildItemType(this MemberInfo info)
        {
            var mType = GetMemberType(info);
            if (mType.IsArray)
            {
                return mType.GetElementType();
            }
            else if (IsList(mType))
            {
                return mType.GenericTypeArguments[0];
            }

            throw new ArgumentException("member is not a array or list", nameof(info));
        }

        private static Type GetMemberType(MemberInfo info) => info switch
        {
            FieldInfo fieldInfo => fieldInfo.FieldType,
            PropertyInfo propertyInfo => propertyInfo.PropertyType,
            _ => null,
        };

        public static string GetDefault(params string[] strings)
        {
            foreach (var str in strings)
            {
                if (string.IsNullOrWhiteSpace(str))
                {
                    continue;
                }

                return $@"""{str}""";
            }
            return "default";
        }
    }
}