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
        T Serialize(IGXML<T> xml, object @object, T parentNode = default);

        object Deserialize(IGXML<T> xml, T data);
    }

    public interface IGXSerializableFactory<T, TO> : IGXSerializableFactory<T>
    {
        T Serialize(IGXML<T> xml, TO @object, T parentNode = default);

        TO DeserializeExplicitly(IGXML<T> xml, T data);
    }

#if GXML_GEN_CODE

    public class GXMLSerializer<T>
    {
        public IGXML<T> XML { get; private set; }

        public Dictionary<Type, IGXSerializableFactory<T>> Factorys { get; private set; }

        public GXMLSerializer(IGXML<T> xml, Dictionary<Type, IGXSerializableFactory<T>> factorys = null)
        {
            XML = xml;
            Factorys = factorys ?? new Dictionary<Type, IGXSerializableFactory<T>>();
        }

        public object Read(T node, Type typeInfo) =>
            Factorys.TryGetValue(typeInfo, out var factory) ? factory.Deserialize(XML, node) : default;

        public T Write(object obj, T parentNode) =>
            Factorys.TryGetValue(obj.GetType(), out var factory) ? factory.Serialize(XML, obj, parentNode) : default;
    }

#else
    public class GXMLSerializer<T>
    {
        private const BindingFlags FLAGS = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        public Dictionary<Type, GXConverter> CustomConverters { get; private set; }

        public IGXML<T> XML { get; private set; }

        public GXMLSerializer(IGXML<T> xml, Dictionary<Type, GXConverter> convertor = null)
        {
            XML = xml;

            CustomConverters = convertor ?? new Dictionary<Type, GXConverter>()
            {
                { typeof(bool), new GXConverter((vb,d,de) => vb?.ToString()??de, sb => Convert.ToBoolean(sb)) },
                { typeof(int), new GXConverter((vi,d,de) => vi?.ToString()??de, si => Convert.ToInt32(si)) },
                { typeof(float), new GXConverter((vf,d,de) => vf?.ToString()??de, sf => Convert.ToSingle(sf)) },
                { typeof(string), new GXConverter((vs,d,de) => vs?.ToString()??de, ss => ss) }
            };
        }

        #region read

        public object Read(T node, Type typeInfo, IGXAttribute refAttribute = default, Func<object> constructor = default)
        {
            if (Attribute.GetCustomAttribute(typeInfo, typeof(GXNodeAttribute)) is GXNodeAttribute defAttribute)
            {
                if (XML.NameOf(node) == GetValidName(refAttribute?.Name, defAttribute.Name, typeInfo.Name))
                {
                    var obj = constructor?.Invoke() ?? Activator.CreateInstance(typeInfo);

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
                return Construct(XML.GetInnerString(node, refAttribute.Default), typeInfo);
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
                SetMemberValue(info, obj, Construct(strValue, GetMemberType(info)));
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
                foreach (var item in XML.GetChildren(node, GetValidName(itemRefAttribute?.Name, defAttribute?.Name, elementType.Name)))
                {
                    list.Add(Read(item, elementType, itemRefAttribute));
                }

                var array = Array.CreateInstance(elementType, list.Count);
                list.CopyTo(array, 0);
                SetMemberValue(info, obj, array);
            }
            else if (IsList(memberType))
            {
                var argType = memberType.GenericTypeArguments[0];
                var list = Activator.CreateInstance(memberType) as IList;
                var defAttribute = Attribute.GetCustomAttribute(argType, typeof(GXNodeAttribute)) as GXNodeAttribute;
                foreach (var item in XML.GetChildren(node, GetValidName(itemRefAttribute?.Name, defAttribute?.Name, argType.Name)))
                {
                    list.Add(Read(item, argType, itemRefAttribute));
                }
                SetMemberValue(info, obj, list);
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

        private static string GetValidName(params string[] names)
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

        private static void SetMemberValue(MemberInfo info, object obj, object value)
        {
            switch (info)
            {
                case FieldInfo fieldInfo: fieldInfo.SetValue(obj, value); break;
                case PropertyInfo propertyInfo: propertyInfo.SetValue(obj, value); break;
                default: break;
            }
        }

        private static object GetMemberValue(MemberInfo info, object obj)
        {
            switch (info)
            {
                case FieldInfo fieldInfo: return fieldInfo.GetValue(obj);
                case PropertyInfo propertyInfo: return propertyInfo.GetValue(obj);
                default: return null;
            }
        }

        private static Type GetMemberType(MemberInfo info)
        {
            switch (info)
            {
                case FieldInfo fieldInfo: return fieldInfo.FieldType;
                case PropertyInfo propertyInfo: return propertyInfo.PropertyType;
                default: return null;
            }
        }

        public class GXConverter
        {
            public Func<object, int, string, string> Stringifier { get; private set; }
            public Func<string, object> Constructor { get; private set; }

            public GXConverter(Func<object, int, string, string> stringifier, Func<string, object> constructor)
            {
                Stringifier = stringifier;
                Constructor = constructor;
            }

            public string Stringify(object obj, int @decimal = 2, string @default = default) => Stringifier?.Invoke(obj, @decimal, @default);

            public object Construct(string str) => Constructor?.Invoke(str);
        }

        private string Stringify(object obj, int @decimal = 2, string @default = default)
        {
            if (obj == null) { return @default; }

            return CustomConverters.TryGetValue(obj.GetType(), out var converter)
                ? converter.Stringify(obj, @decimal, @default)
                : obj.ToString();
        }

        private object Construct(string str, Type type, object @default = default)
        {
            return (!string.IsNullOrWhiteSpace(str) && CustomConverters.TryGetValue(type, out var converter))
                ? converter.Construct(str)
                : @default;
        }

        #endregion utils
    }

#endif
}