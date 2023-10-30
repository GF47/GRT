#if UNITY_WEBGL
#define GXML_GEN_CODE
#endif

// #define GXML_GEN_CODE
// #undef GXML_GEN_CODE

using System;
using System.Collections.Generic;

#if !GXML_GEN_CODE

using System.Collections;
using System.Reflection;

#endif

namespace GRT.Data
{
    public interface IGXSerializableFactory<T>
    {
        T Serialize(IGXML<T> xml, object @object, T parentNode, IGXAttribute refAttribute = default);

        object Deserialize(IGXML<T> xml, T data, IGXAttribute refAttribute = default, Func<object> constructor = default);
    }

    public interface IGXSerializableFactory<T, TO> : IGXSerializableFactory<T>
    {
        T Serialize(IGXML<T> xml, TO @object, T parentNode, IGXAttribute refAttribute = default);

        TO DeserializeExplicitly(IGXML<T> xml, T data, IGXAttribute refAttribute = default, Func<object> constructor = default);
    }

    public class GXConverter
    {
        public struct Result
        {
            public bool success;
            public object value;

            public Result(bool success, object value)
            {
                this.success = success;
                this.value = value;
            }
        }

        private readonly Func<object, int, string, string> _stringifier;
        private readonly Func<string, Result> _constructor;

        public GXConverter(Func<object, int, string, string> stringifier, Func<string, Result> constructor)
        {
            _stringifier = stringifier;
            _constructor = constructor;
        }

        public string Stringify(object obj, int @decimal = 2, string @default = default)
        {
            return _stringifier == null ? @default : _stringifier.Invoke(obj, @decimal, @default);
        }

        public bool Construct(string str, out object value)
        {
            if (_constructor == null)
            {
                value = default;
                return false;
            }
            else
            {
                var result = _constructor.Invoke(str);
                value = result.value;
                return result.success;
            }
        }

        public TO ConstructExplicitly<TO>(string str, TO @default) => Construct(str, out var value) ? (TO)value : @default;
    }

#if GXML_GEN_CODE

    public class GXMLSerializer<T>
    {
        public static GXMLSerializer<T> Instance { get; private set; }

        public IGXML<T> XML { get; private set; }

        public Dictionary<Type, IGXSerializableFactory<T>> Factories { get; private set; }

        public Dictionary<Type, GXConverter> CustomConverters { get; private set; }

        public GXMLSerializer(IGXML<T> xml, Dictionary<Type, GXConverter> converters = null, Dictionary<Type, IGXSerializableFactory<T>> factories = null)
        {
            XML = xml;
            CustomConverters = converters ?? new Dictionary<Type, GXConverter>()
            {
                { typeof(bool),   new GXConverter((vb, d, de) => vb?.ToString() ?? de, sb => new GXConverter.Result(bool.TryParse(sb,  out var result), result)) },
                { typeof(int),    new GXConverter((vi, d, de) => vi?.ToString() ?? de, si => new GXConverter.Result(int.TryParse(si,   out var result), result)) },
                { typeof(float),  new GXConverter((vf, d, de) => vf?.ToString() ?? de, sf => new GXConverter.Result(float.TryParse(sf, out var result), result)) },
                { typeof(string), new GXConverter((vs, d, de) => vs?.ToString() ?? de, ss => new GXConverter.Result(true, ss)) }
            };
            Factories = factories ?? new Dictionary<Type, IGXSerializableFactory<T>>();

            Instance = this;
        }

        public object Read(T node, Type typeInfo, IGXAttribute refAttribute = default, Func<object> constructor = default) =>
            Factories.TryGetValue(typeInfo, out var factory) ? factory.Deserialize(XML, node, refAttribute, constructor) : default;

        public T Write(object obj, T parentNode, IGXAttribute refAttribute = default) =>
            Factories.TryGetValue(obj.GetType(), out var factory) ? factory.Serialize(XML, obj, parentNode, refAttribute) : default;

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

        public string Stringify(object obj, int @decimal = 2, string @default = default)
        {
            if (obj == null) { return @default; }

            return CustomConverters.TryGetValue(obj.GetType(), out var converter)
                ? converter.Stringify(obj, @decimal, @default)
                : obj.ToString();
        }

        public bool Construct(string str, Type type, out object result)
        {
            if (!string.IsNullOrWhiteSpace(str)
                && CustomConverters.TryGetValue(type, out var converter)
                && converter.Construct(str, out result))
            {
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

        public TO Construct<TO>(string str, TO @default) => Construct(str, typeof(TO), out var result) ? (TO)result : @default;
    }

#else

    public class GXMLSerializer<T>
    {
        public static GXMLSerializer<T> Instance { get; private set; }

        private const BindingFlags FLAGS = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        public Dictionary<Type, GXConverter> CustomConverters { get; private set; }

        public IGXML<T> XML { get; private set; }

        public GXMLSerializer(IGXML<T> xml, Dictionary<Type, GXConverter> converters = null)
        {
            XML = xml;

            CustomConverters = converters ?? new Dictionary<Type, GXConverter>()
            {
                { typeof(bool),   new GXConverter((vb, d, de) => vb?.ToString() ?? de, sb => new GXConverter.Result(bool.TryParse(sb,  out var result), result)) },
                { typeof(int),    new GXConverter((vi, d, de) => vi?.ToString() ?? de, si => new GXConverter.Result(int.TryParse(si,   out var result), result)) },
                { typeof(float),  new GXConverter((vf, d, de) => vf?.ToString() ?? de, sf => new GXConverter.Result(float.TryParse(sf, out var result), result)) },
                { typeof(string), new GXConverter((vs, d, de) => vs?.ToString() ?? de, ss => new GXConverter.Result(true, ss)) }
            };

            Instance = this;
        }

        #region read

        public object Read(T node, Type typeInfo, IGXAttribute refAttribute = default, Func<object> constructor = default)
        {
            if (Attribute.GetCustomAttribute(typeInfo, typeof(GXNodeAttribute)) is GXNodeAttribute defAttribute)
            {
                if (XML.NameOf(node) == GetValidName(refAttribute?.Name, defAttribute.Name, typeInfo.Name))
                {
                    var obj = constructor == null ? Activator.CreateInstance(typeInfo) : constructor.Invoke();

                    var propertyInfos = typeInfo.GetProperties(FLAGS);
                    foreach (var info in propertyInfos)
                    {
                        ReadMember(obj, node, info);
                    }

                    var fieldInfos = typeInfo.GetFields(FLAGS);
                    foreach (var info in fieldInfos)
                    {
                        ReadMember(obj, node, info);
                    }

                    return obj;
                }
            }
            else if (XML.NameOf(node) == GetValidName(refAttribute.Name, typeInfo.Name))
            {
                return Construct(XML.GetInnerString(node, refAttribute.Default), typeInfo, out var result)
                    ? result
                    : (constructor == null ? Activator.CreateInstance(typeInfo) : constructor.Invoke());
            }

            throw new ArgumentException($"<{XML.NameOf(node)}/> is not a {typeInfo.FullName}", nameof(node));
        }

        private void ReadMember(object obj, T node, MemberInfo info)
        {
            if (Attribute.GetCustomAttribute(info, typeof(GXArrayAttribute)) is GXArrayAttribute ara)
            {
                var itemRefAttribute = Attribute.GetCustomAttribute(info, typeof(GXNodeAttribute)) as GXNodeAttribute;
                ReadArray(obj, string.IsNullOrWhiteSpace(ara.Container) ? node : XML.GetChild(node, ara.Container), info, itemRefAttribute);
            }
            else if (Attribute.GetCustomAttribute(info, typeof(GXNodeAttribute)) is GXNodeAttribute refAttribute)
            {
                if (XML.HasChild(node, GetValidName(refAttribute.Name, info.Name), out var child))
                {
                    SetMemberValue(info, obj, Read(child, GetMemberType(info), refAttribute));
                }
            }
            else if (Attribute.GetCustomAttribute(info, typeof(GXAttributeAttribute)) is GXAttributeAttribute ata)
            {
                var strValue = XML.HasAttribute(node, GetValidName(ata.Name, info.Name), out var vout)
                    ? vout
                    : ata.Default;

                var value = Construct(strValue, GetMemberType(info), out var result) ? result : default;
                SetMemberValue(info, obj, value);
            }
        }

        private void ReadArray(object obj, T node, MemberInfo info, IGXAttribute itemRefAttribute = default)
        {
            var memberType = GetMemberType(info);
            if (memberType.IsArray)
            {
                var elementType = memberType.GetElementType();
                var list = Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType)) as IList;
                var defAttribute = Attribute.GetCustomAttribute(elementType, typeof(GXNodeAttribute)) as GXNodeAttribute;
                var itemNodes = XML.GetChildren(node, GetValidName(itemRefAttribute?.Name, defAttribute?.Name, elementType.Name));
                if (itemNodes != null)
                {
                    foreach (var item in itemNodes)
                    {
                        list.Add(Read(item, elementType, itemRefAttribute));
                    }

                    var array = Array.CreateInstance(elementType, list.Count);
                    list.CopyTo(array, 0);
                    SetMemberValue(info, obj, array);
                }
            }
            else if (IsList(memberType))
            {
                var argType = memberType.GenericTypeArguments[0];
                var list = Activator.CreateInstance(memberType) as IList;
                var defAttribute = Attribute.GetCustomAttribute(argType, typeof(GXNodeAttribute)) as GXNodeAttribute;
                var itemNodes = XML.GetChildren(node, GetValidName(itemRefAttribute?.Name, defAttribute?.Name, argType.Name));
                if (itemNodes != null)
                {
                    foreach (var item in itemNodes)
                    {
                        list.Add(Read(item, argType, itemRefAttribute));
                    }
                    SetMemberValue(info, obj, list);
                }
            }
        }

        #endregion read

        #region write

        public T Write(object obj, T parentNode, IGXAttribute refAttribute = default)
        {
            if (obj == null) { return default; }

            var typeInfo = obj.GetType();

            if (Attribute.GetCustomAttribute(typeInfo, typeof(GXNodeAttribute)) is GXNodeAttribute defAttribute)
            {
                // 可序列化

                var node = XML.CreateChild(parentNode, GetValidName(refAttribute?.Name, defAttribute.Name, typeInfo.Name));

                var fieldInfos = typeInfo.GetFields(FLAGS);
                foreach (var info in fieldInfos)
                {
                    WriteMember(obj, node, info);
                }

                var propertyInfos = typeInfo.GetProperties(FLAGS);
                foreach (var info in propertyInfos)
                {
                    WriteMember(obj, node, info);
                }

                return node;
            }
            else
            {
                // 普通数据类型

                var refName = GetValidName(refAttribute?.Name, typeInfo.Name);
                var refDecimal = refAttribute?.Decimal ?? 2;
                var refDefaultValue = refAttribute?.Default;

                var node = XML.CreateChild(parentNode, refName);
                XML.SetInnerString(node, Stringify(obj, refDecimal, refDefaultValue));

                return node;
            }
        }

        private void WriteMember(object obj, T node, MemberInfo info)
        {
            if (Attribute.GetCustomAttribute(info, typeof(GXArrayAttribute)) is GXArrayAttribute ara)
            {
                var itemRefAttribute = Attribute.GetCustomAttribute(info, typeof(GXNodeAttribute)) as GXNodeAttribute;
                WriteArray(obj, string.IsNullOrWhiteSpace(ara.Container) ? node : XML.CreateChild(node, ara.Container), info, itemRefAttribute);
            }
            else if (Attribute.GetCustomAttribute(info, typeof(GXNodeAttribute)) is GXNodeAttribute refAttribute)
            {
                var v = GetMemberValue(info, obj);
                Write(v, node, refAttribute);
            }
            else if (Attribute.GetCustomAttribute(info, typeof(GXAttributeAttribute)) is GXAttributeAttribute ata)
            {
                var v = GetMemberValue(info, obj);
                XML.SetAttribute(node, GetValidName(ata.Name, info.Name), Stringify(v, ata.Decimal, ata.Default));
            }
        }

        private void WriteArray(object obj, T node, MemberInfo info, IGXAttribute itemRefAttribute = default)
        {
            if (typeof(ICollection).IsAssignableFrom(GetMemberType(info)) && GetMemberValue(info, obj) is ICollection collection)
            {
                foreach (var item in collection)
                {
                    Write(item, node, itemRefAttribute);
                }
            }
        }

        #endregion write

        #region utils

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

        private static void SetMemberValue(MemberInfo info, object obj, object value)
        {
            switch (info)
            {
                case FieldInfo fieldInfo: fieldInfo.SetValue(obj, value); break;
                case PropertyInfo propertyInfo: propertyInfo.SetValue(obj, value); break;
                default: break;
            }
        }

        private static object GetMemberValue(MemberInfo info, object obj) => info switch
        {
            FieldInfo fieldInfo => fieldInfo.GetValue(obj),
            PropertyInfo propertyInfo => propertyInfo.GetValue(obj),
            _ => null,
        };

        private static Type GetMemberType(MemberInfo info) => info switch
        {
            FieldInfo fieldInfo => fieldInfo.FieldType,
            PropertyInfo propertyInfo => propertyInfo.PropertyType,
            _ => null,
        };

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

        public string Stringify(object obj, int @decimal = 2, string @default = default)
        {
            if (obj == null) { return @default; }

            return CustomConverters.TryGetValue(obj.GetType(), out var converter)
                ? converter.Stringify(obj, @decimal, @default)
                : obj.ToString();
        }

        public bool Construct(string str, Type type, out object result)
        {
            if (!string.IsNullOrWhiteSpace(str)
                && CustomConverters.TryGetValue(type, out var converter)
                && converter.Construct(str, out result))
            {
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

        public TO Construct<TO>(string str, TO @default) => Construct(str, typeof(TO), out var result) ? (TO)result : @default;

        #endregion utils
    }

#endif
}