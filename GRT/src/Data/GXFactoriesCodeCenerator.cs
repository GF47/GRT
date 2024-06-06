using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace GRT.Data
{
    public static class GXFactoriesCodeCenerator
    {
        private const BindingFlags FLAGS = BindingFlags.Instance | BindingFlags.Public; // | BindingFlags.NonPublic;

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

        /**************************************************************/

        public static void GenerateAll(string path)
        {
            var dirPath = path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            if (Directory.Exists(dirPath))
            {
                Directory.Delete(dirPath, true);
            }
            Directory.CreateDirectory(dirPath);

            var types = from assembly in AppDomain.CurrentDomain.GetAssemblies()
                        where !(assembly.ManifestModule is ModuleBuilder)
                        from type in assembly.GetTypes()
                        where type.IsDefined(typeof(GXNodeAttribute), false)
                        select type;

            var filePath = $"{dirPath}/GXRegister.cs".Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            using (var fs = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write))
            {
                using (var sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    sw.Write(GXREGISTER(types));
                }
            }

            foreach (var type in types)
            {
                GenerateFactory(type, dirPath);
            }
        }

        public static void GenerateFactory(Type type, string path)
        {
            var filePath = $"{path}/{type.FULL_NAME().Replace('.', '_')}_GXFactory.cs".Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                using (var sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    sw.Write(GXFACTORY(type));
                }
            }
        }

        /**************************************************************/

        private static string GXREGISTER(IEnumerable<Type> types)
        {
            var sb = new StringBuilder();
            sb.Append(TEMPLATE_USING);
            sb.Append(TEMPLATE_NAMESPACE_A);
            {
                sb.Append(REGISTER_CLASS_A);
                {
                    sb.Append(GET_FACTORIES_METHOD_A);
                    {
                        foreach (var type in types)
                        {
                            sb.Append(FACTORY_KV_PAIR(type.FULL_NAME()));
                        }
                    }
                    sb.Append(GET_FACTORIES_METHOD_B);
                }
                sb.Append(REGISTER_CLASS_B);
            }
            sb.Append(TEMPLATE_NAMESPACE_B);

            return sb.ToString();
        }

        private const string REGISTER_CLASS_A = @"
    public static class GXRegister<T>
    {";

        private const string REGISTER_CLASS_B = @"
    }";

        private const string GET_FACTORIES_METHOD_A = @"
        public static Dictionary<Type, IGXSerializableFactory<T>> GetFactories() => new Dictionary<Type, IGXSerializableFactory<T>>()
        {";

        private const string GET_FACTORIES_METHOD_B = @"
        };";

        private static string FACTORY_KV_PAIR(string fullName) => $@"
            {{ typeof({fullName}), new {fullName.Replace('.', '_')}_GXFactory<T>() }},";

        /**************************************************************/

        private static string GXFACTORY(Type type)
        {
            var sb = new StringBuilder();
            sb.Append(TEMPLATE_USING);
            sb.Append(TEMPLATE_NAMESPACE_A);
            {
                sb.Append(FACTORY_CLASS_A(type));
                {
                    sb.Append(SERIALIZE_METHOD(type));

                    sb.Append(SERIALIZE_METHOD_EXPLICITLY_A(type));
                    {
                        var propertyInfos = type.GetProperties(FLAGS);
                        foreach (var info in propertyInfos)
                        {
                            sb.Append(SERIALIZE_MEMBER(info));
                        }

                        var fieldInfos = type.GetFields(FLAGS);
                        foreach (var info in fieldInfos)
                        {
                            sb.Append(SERIALIZE_MEMBER(info));
                        }
                    }
                    sb.Append(SERIALIZE_METHOD_EXPLICITLY_B);

                    sb.Append(DESERIALIZE_METHOD_EXPLICITLY_A(type));
                    {
                        var propertyInfos = type.GetProperties(FLAGS);
                        foreach (var info in propertyInfos)
                        {
                            sb.Append(DESERIALIZE_MEMBER(info));
                        }

                        var fieldInfos = type.GetFields(FLAGS);
                        foreach (var info in fieldInfos)
                        {
                            sb.Append(DESERIALIZE_MEMBER(info));
                        }
                    }
                    sb.Append(DESERIALIZE_METHOD_EXPLICITLY_B);
                }
                sb.Append(FACTORY_CLASS_B);
            }
            sb.Append(TEMPLATE_NAMESPACE_B);

            return sb.ToString();
        }

        private static string FACTORY_CLASS_A(Type type) => $@"
    public class {type.FULL_NAME().Replace('.', '_')}_GXFactory<T> : IGXSerializableFactory<T, {type.FULL_NAME()}>
    {{";

        private const string FACTORY_CLASS_B = @"
    }";

        private static string SERIALIZE_METHOD(Type type) => $@"
        public T Serialize(IGX<T> gx, object obj, T parent, IGXAttribute refAttr = default) =>
            Serialize(gx, obj as {type.FULL_NAME()}, parent, refAttr);

        public object Deserialize(IGX<T> gx, T node, Func<object> constructor = default) =>
            DeserializeExplicitly(gx, node, constructor);
";

        private static string SERIALIZE_METHOD_EXPLICITLY_A(Type type) => $@"
        public T Serialize(IGX<T> gx, {type.FULL_NAME()} obj, T parent, IGXAttribute refAttr = default)
        {{
            if (obj == null) {{ return default; }}

            var serializer = GXSerializer<T>.Instance;

            var node = gx.CreateChild(parent, GXExtensions.GetValidName(refAttr?.DefaultName(), ""{type.DEFAULT_NAME()}""));
";

        private const string SERIALIZE_METHOD_EXPLICITLY_B = @"
            return node;
        }
";

        private static string SERIALIZE_MEMBER(MemberInfo member)
        {
            var mName = member.Name;
            if (member.HasAttribute<GXArrayAttribute>(out var arrayAttr))
            {
                var itemType = member.GetChildItemType();
                var itemRefAttr = member.GetAttribute<GXArrayItemAttribute>();
                var itemDefAttr = Attribute.GetCustomAttribute(itemType, typeof(GXNodeAttribute)) as GXNodeAttribute;

                var containerNodeName = arrayAttr.DefaultName();
                return $@"
            if (obj.{mName} != null)
            {{
                var containerNode_{mName} = {(string.IsNullOrWhiteSpace(containerNodeName) ? "node" : $@"gx.CreateChild(node, ""{containerNodeName}"")")};
                var refAttr_{mName} = new GXMockAttribute()
                {{
                    Name = ""{itemType.DEFAULT_NAME(itemRefAttr, itemDefAttr)}"",
                    Default = {GetDefault(itemRefAttr?.Default, itemDefAttr?.Default)},
                }};
                foreach (var item in obj.{mName})
                {{
                    GXSerializer<T>.Instance.Serialize(item, containerNode_{mName}, refAttr_{mName}, {itemRefAttr?.Stringifier ?? "default"});
                }}
            }}
";
            }
            else if (member.HasAttribute<GXNodeAttribute>(out var refAttr))
            {
                return $@"
            {(GetMemberType(member).IsValueType ? "// " : string.Empty)}if (obj.{mName} != null)
            {{
                var node_{mName} = GXSerializer<T>.Instance.Serialize(obj.{mName}, node, new GXMockAttribute()
                {{
                    Name = {GetDefault(refAttr?.DefaultName(), mName)},
                    Default = {GetDefault(refAttr?.Default)},
                }});
            }}
";
            }
            else if (member.HasAttribute<GXKVPairAttribute>(out var attr))
            {
                var mType = GetMemberType(member);
                if (IsNullable(mType, out _))
                {
                    return $@"
            if (obj.{mName} != null)
            {{
                gx.SetKVPair(node, {GetDefault(attr?.DefaultName(), mName)}, serializer.Stringify(obj.{mName}, {GetDefault(attr?.Default)}, {attr?.Stringifier ?? "default"}));
            }}
";
                }
                else
                {
                    return $@"
            gx.SetKVPair(node, {GetDefault(attr?.DefaultName(), mName)}, serializer.Stringify(obj.{mName}, {GetDefault(attr?.Default)}, {attr?.Stringifier ?? "default"}));
";
                }
            }
            else
            {
                return string.Empty;
            }
        }

        private static string DESERIALIZE_METHOD_EXPLICITLY_A(Type type)
        {
            var name = type.FULL_NAME();
            return $@"
        public {name} DeserializeExplicitly(IGX<T> gx, T node, Func<object> constructor = default)
        {{
            var serializer = GXSerializer<T>.Instance;

            var obj = (constructor?.Invoke() as {name}) ?? new {name}();
";
        }

        private const string DESERIALIZE_METHOD_EXPLICITLY_B = @"
            return obj;
        }";

        private static string DESERIALIZE_MEMBER(MemberInfo member)
        {
            var mName = member.Name;
            var mType = GetMemberType(member);
            if (member.HasAttribute<GXArrayAttribute>(out var arrayAttr))
            {
                var itemType = member.GetChildItemType();
                var itemRefAttr = member.GetAttribute<GXArrayItemAttribute>();
                var itemDefAttr = Attribute.GetCustomAttribute(itemType, typeof(GXNodeAttribute)) as GXNodeAttribute;
                var itemTypeFullName = itemType.FULL_NAME();

                var (cn1, cn2, cn3) = arrayAttr.Names(); // container 节点的可选名称

                var (in1, in2, in3) = itemRefAttr == null ? (default, default, default) : itemRefAttr.Names(); // 列表项节点的可选名称
                var (in4, in5, in6) = itemDefAttr == null ? (default, default, default) : itemDefAttr.Names();

                return $@"
            var containerNode_{mName} = {(string.IsNullOrWhiteSpace(arrayAttr.Container) ? "node" : $@"gx.GetChild(node, {GetNodeNamePredicate(cn1, cn2, cn3, mName)})")};
            if (containerNode_{mName} != null)
            {{
                var itemNodes_{mName} = gx.GetChildren(containerNode_{mName}, {GetNodeNamePredicate(in1, in2, in3, in4, in5, in6, itemType.Name)});
                if (itemNodes_{mName} != null)
                {{
                    var list_{mName} = new List<{itemTypeFullName}>();
                    foreach (var item in itemNodes_{mName})
                    {{
                        list_{mName}.Add(({itemTypeFullName})serializer.Deserialize(item, typeof({itemTypeFullName})));
                    }}

                    obj.{mName} = list_{mName}{(mType.IsArray ? ".ToArray()" : string.Empty)};
                }}
            }}
";
            }
            else if (member.HasAttribute<GXNodeAttribute>(out var refAttr))
            {
                var mTypeFullName = mType.FULL_NAME();

                var (n1, n2, n3) = refAttr.Names();

                return $@"
            var node_{mName} = gx.GetChild(node, {GetNodeNamePredicate(n1, n2, n3, mName)});
            if (node_{mName} != null)
            {{
                obj.{mName} = ({mTypeFullName})serializer.Deserialize(node_{mName}, typeof({mTypeFullName}));
            }}
";
            }
            else if (member.HasAttribute<GXKVPairAttribute>(out var attr))
            {
                var (n1, n2, n3) = attr.Names();

                if (IsNullable(mType, out var gType))
                {
                    mType = gType;
                    return $@"
            if (gx.HasKVPair(node, {GetNamePredicate(n1, n2, n3, mName)}, out var str_{mName}))
            {{
                obj.{mName} = serializer.Construct<{mType.FULL_NAME()}>(str_{mName});
            }}
";
                }
                else
                {
                    return $@"
            obj.{mName} = serializer.Construct<{mType.FULL_NAME()}>(gx.GetKVPair(node, {GetNamePredicate(n1, n2, n3, mName)}, {GetDefault(attr?.Default)}));
";
                }
            }
            else
            {
                return string.Empty;
            }
        }

        private static string FULL_NAME(this Type type)
        {
            var parent = type.DeclaringType;
            var relationShip = "";
            while (parent != null)
            {
                relationShip = $"{parent.NAME()}.";
                parent = parent.DeclaringType;
            }

            return $"{type.Namespace}.{relationShip}{type.NAME()}";
        }

        private static string NAME(this Type type)
        {
            var name = type.Name;
            if (type.IsGenericType)
            {
                name += "<";

                var addComma = false;
                foreach (var arg in type.GetGenericArguments())
                {
                    if (addComma) { name += ","; } else { addComma = true; }
                    name += arg.FULL_NAME();
                }

                name += ">";
            }

            return name;
        }

        private static string DEFAULT_NAME(this Type type, IGXAttribute refAttr = default, GXNodeAttribute defAttr = default)
        {
            return refAttr?.DefaultName() ??
                (defAttr ?? Attribute.GetCustomAttribute(type, typeof(GXNodeAttribute)) as GXNodeAttribute)?.DefaultName() ??
                type.Name;
        }

        #region type utilities

        private static bool IsList(this Type type)
        {
            if (typeof(IList).IsAssignableFrom(type))
            {
                return true;
            }
            else
            {
                foreach (var i in type.GetInterfaces())
                {
                    if (i.IsGenericType && typeof(IList<>) == i.GetGenericTypeDefinition())
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        private static bool IsNullable(this Type type, out Type genericType)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                genericType = type.GenericTypeArguments[0];
                return true;
            }
            else
            {
                genericType = default;
                return false;
            }
        }

        private static bool HasAttribute<T>(this MemberInfo info, out T attr) where T : Attribute
        {
            if (Attribute.GetCustomAttribute(info, typeof(T)) is T attribute)
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

        private static T GetAttribute<T>(this MemberInfo info) where T : Attribute
        {
            return Attribute.GetCustomAttribute(info, typeof(T)) as T;
        }

        private static Type GetChildItemType(this MemberInfo info)
        {
            var mType = GetMemberType(info);
            if (mType.IsArray)
            {
                return mType.GetElementType();
            }
            else if (mType.IsList())
            {
                return mType.GenericTypeArguments[0];
            }

            throw new ArgumentException("member is not array or list", nameof(info));
        }

        private static Type GetMemberType(MemberInfo info)
        {
            switch (info)
            {
                case FieldInfo fieldInfo: return fieldInfo.FieldType;
                case PropertyInfo propertyInfo: return propertyInfo.PropertyType;
                default: return default;
            }
        }

        #endregion type utilities

        private static string GetDefault(params string[] strings)
        {
            foreach (var s in strings)
            {
                if (!string.IsNullOrEmpty(s))
                {
                    return $@"""{s}""";
                }
            }

            return "default";
        }

        private static string GetNamePredicate(params string[] strings)
        {
            var sb = new StringBuilder("s => ");
            for (int i = 0; i < strings.Length; i++)
            {
                var si = strings[i];
                if (!string.IsNullOrWhiteSpace(si))
                {
                    sb.Append("s == \"");
                    sb.Append(si);
                    sb.Append("\" || ");
                }
            }
            sb.Append("false");

            return sb.ToString();
        }

        private static string GetNodeNamePredicate(params string[] strings)
        {
            var sb = new StringBuilder("n => { var s = gx.NameOf(n); return ");
            for (int i = 0; i < strings.Length; i++)
            {
                var si = strings[i];
                if (!string.IsNullOrWhiteSpace(si))
                {
                    sb.Append("s == \"");
                    sb.Append(si);
                    sb.Append("\" || ");
                }
            }
            sb.Append("false; }");

            return sb.ToString();
        }
    }
}