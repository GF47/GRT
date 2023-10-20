using System;
using System.Collections;
using System.Reflection;

namespace GRT.Data
{
    public class GXMLSerializer<T>
    {
        private const BindingFlags FLAGS = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        public IGXML<T> XML { get; private set; }

        public GXMLSerializer(IGXML<T> xml) => XML = xml;

        public object Read(T node, Type typeInfo)
        {
            if (Attribute.GetCustomAttribute(typeInfo, typeof(GXNodeAttribute)) is GXNodeAttribute nodeAttribute
                && (string.IsNullOrWhiteSpace(nodeAttribute.Name) ? typeInfo.Name : nodeAttribute.Name) == XML.NameOf(node))
            {
                var obj = Activator.CreateInstance(typeInfo);

                var propertyInfos = typeInfo.GetProperties(FLAGS);
                foreach (var info in propertyInfos)
                {
                    ReadProperty(obj, node, info);
                }

                var fieldInfos = typeInfo.GetFields(FLAGS);
                foreach(var info in fieldInfos)
                {
                    ReadField(obj, node, info);
                }

                return obj;
            }
            else
            {
                throw new ArgumentException($"<{XML.NameOf(node)}/> is not a {typeInfo.FullName}", nameof(node));
            }
        }

        private void ReadField(object obj, T node, FieldInfo info)
        {
            if (Attribute.GetCustomAttribute(info, typeof(GXIgnoreAttribute)) != null)
            {
                return;
            }
            else if (Attribute.GetCustomAttribute(info, typeof(GXNodeAttribute)) is GXNodeAttribute noa)
            {
                var strValue = XML.HasChild(node, string.IsNullOrWhiteSpace(noa.Name) ? info.Name : noa.Name, out var n)
                            && XML.HasInnerString(n, out var vout)
                    ? vout : noa.Default;
                info.SetValue(obj, strValue.ConvertTo(info.FieldType));
            }
            else if (Attribute.GetCustomAttribute(info, typeof(GXAttributeAttribute)) is GXAttributeAttribute ata)
            {
                var strValue = XML.HasAttribute(node, string.IsNullOrWhiteSpace(ata.Name) ? info.Name : ata.Name, out var vout)
                    ? vout : ata.Default;
                info.SetValue(obj, strValue.ConvertTo(info.FieldType));
            }
            else if (Attribute.GetCustomAttribute(info, typeof(GXArrayAttribute)) is GXArrayAttribute ara)
            {
                ReadArray(obj, string.IsNullOrWhiteSpace(ara.Container) ? node : XML.GetChild(node, ara.Container), info);
            }
        }

        private void ReadArray(object obj, T node, FieldInfo info)
        {
            if (typeof(IList).IsAssignableFrom(info.FieldType))
            {
                var argType = info.FieldType.GenericTypeArguments[0]; // only support one level list, not array
                var list = Activator.CreateInstance(info.FieldType) as IList;
                var name = (Attribute.GetCustomAttribute(argType, typeof(GXNodeAttribute)) is GXNodeAttribute noa
                    && !string.IsNullOrWhiteSpace(noa.Name))
                    ? noa.Name : argType.Name;
                foreach (var item in XML.GetChildren(node, name))
                {
                    list.Add(Read(item, argType));
                }
                info.SetValue(obj, list);
            }
        }

        private void ReadProperty<TC>(TC obj, T node, PropertyInfo info)
        {
            throw new NotImplementedException();
        }

        #region write

        public T Write(object obj, T parentNode, string nodeName = null)
        {
            var typeInfo = obj.GetType();
            var nodeAttribute = (Attribute.GetCustomAttribute(typeInfo, typeof(GXNodeAttribute))
                ?? throw new ArgumentException($"{typeInfo.FullName} must have a {nameof(GXNodeAttribute)} attribute", nameof(obj)))
                as GXNodeAttribute;
            var node = XML.CreateChild(parentNode, GetValidName(nodeName, nodeAttribute.Name, typeInfo.Name));

            var fieldInfos = typeInfo.GetFields(FLAGS);
            foreach (var info in fieldInfos)
            {
                WriteField(obj, node, info);
            }

            var propertyInfos = typeInfo.GetProperties(FLAGS);
            foreach (var info in propertyInfos)
            {
                WriteProperty(obj, node, info);
            }

            return node;
        }

        private void WriteField(object obj, T node, FieldInfo info)
        {
            if (Attribute.GetCustomAttribute(info, typeof(GXIgnoreAttribute)) != null)
            {
                return;
            }
            else if (Attribute.GetCustomAttribute(info, typeof(GXNodeAttribute)) is GXNodeAttribute noa)
            {
                var v = info.GetValue(obj);
                if (Attribute.GetCustomAttribute(info.FieldType, typeof(GXNodeAttribute)) is GXNodeAttribute fnoa)
                {
                    if (v != null)
                    {
                        Write(v, node, fnoa.Name);
                    }
                    return;
                }

                var n = XML.CreateChild(node, GetValidName(noa.Name, info.Name));
                if (v != null)
                {
                    XML.SetInnerString(n, v.ToString()); // ??? how to stringify with custom decimal
                }
                else if (!string.IsNullOrEmpty(noa.Default))
                {
                    XML.SetInnerString(n, noa.Default);
                }
                else
                {
                    // pass
                }
            }
            else if (Attribute.GetCustomAttribute(info, typeof(GXAttributeAttribute)) is GXAttributeAttribute ata)
            {
                var v = info.GetValue(obj);
                XML.SetAttribute(node, GetValidName(ata.Name, info.Name), v?.ToString() ?? ata.Default); // ??? how to stringify with
            }
            else if (Attribute.GetCustomAttribute(info, typeof(GXArrayAttribute)) is GXArrayAttribute ara)
            {
                WriteArray(obj, string.IsNullOrEmpty(ara.Container) ? node : XML.CreateChild(node, ara.Container), info);
            }
        }

        private void WriteArray(object obj, T node, FieldInfo info)
        {
            if (typeof(IEnumerable).IsAssignableFrom(info.FieldType) && info.GetValue(obj) is IEnumerable collection)
            {
                foreach (var item in collection)
                {
                    Write(item, node);
                }
            }
        }

        private void WriteProperty(object obj, T node, PropertyInfo info)
        {
            if (Attribute.GetCustomAttribute(info, typeof(GXIgnoreAttribute)) != null)
            {
                return;
            }
            else if (Attribute.GetCustomAttribute(info, typeof(GXNodeAttribute)) is GXNodeAttribute noa)
            {
                var v = info.GetValue(obj);
                if (Attribute.GetCustomAttribute(info.PropertyType, typeof(GXNodeAttribute)) is GXNodeAttribute)
                {
                    if (v != null)
                    {
                        Write(v, node);
                    }
                    return;
                }

                var n = XML.CreateChild(node, string.IsNullOrWhiteSpace(noa.Name) ? info.Name : noa.Name);
                if (v != null)
                {
                    XML.SetInnerString(n, v.ToString()); // ??? how to stringify with custom decimal
                }
                else if (!string.IsNullOrEmpty(noa.Default))
                {
                    XML.SetInnerString(n, noa.Default);
                }
                else
                {
                    // pass
                }
            }
            else if (Attribute.GetCustomAttribute(info, typeof(GXAttributeAttribute)) is GXAttributeAttribute ata)
            {
                var v = info.GetValue(obj);
                XML.SetAttribute(node, string.IsNullOrWhiteSpace(ata.Name) ? info.Name : ata.Name, v?.ToString() ?? ata.Default); // ??? how to stringify with
            }
            else if (Attribute.GetCustomAttribute(info, typeof(GXArrayAttribute)) is GXArrayAttribute ara)
            {
                WriteArray(obj, string.IsNullOrEmpty(ara.Container) ? node : XML.CreateChild(node, ara.Container), info);
            }
        }

        private void WriteArray(object obj, T node, PropertyInfo info)
        {
            if (typeof(IEnumerable).IsAssignableFrom(info.PropertyType) && info.GetValue(obj) is IEnumerable collection)
            {
                foreach (var item in collection)
                {
                    Write(item, node);
                }
            }
        }

        #endregion write

        #region utils

        public static string GetValidName(params string[] names)
        {
            foreach(var name in names)
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    continue;
                }

                return name;
            }

            throw new ArgumentException("no valid name", nameof(names));
        }

        #endregion
    }
}