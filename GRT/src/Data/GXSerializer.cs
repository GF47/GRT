using System;
using System.Collections.Generic;

namespace GRT.Data
{
    public interface IGXSerializableFactory<T>
    {
        T Serialize(IGX<T> gx, object @object, T parent, IGXAttribute refAttr = default);

        object Deserialize(IGX<T> gx, T node, Func<object> constructor = null);
    }

    public interface IGXSerializableFactory<T, TO> : IGXSerializableFactory<T>
    {
        T Serialize(IGX<T> gx, TO @object, T parent, IGXAttribute refAttr = default);

        TO DeserializeExplicitly(IGX<T> gx, T node, Func<object> constructor = null);
    }

    public class GXConverter
    {
        private readonly Func<object, string, string> _stringifier;
        private readonly Func<string, (bool, object)> _constructor;

        public GXConverter(Func<object, string, string> stringifier, Func<string, (bool, object)> constructor)
        {
            _stringifier = stringifier;
            _constructor = constructor;
        }

        public string Stringify(object obj, string @default = default, Func<object, string> customStringifier = default)
        {
            return customStringifier?.Invoke(obj) ?? _stringifier?.Invoke(obj, @default) ?? @default;
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
                bool result;
                (result, value) = _constructor(str);
                return result;
            }
        }

        public TO ConstructExplicitly<TO>(string str, TO @default = default)
        {
            return Construct(str, out var value) ? (TO)value : @default;
        }
    }

    public class GXSerializer<T>
    {
        public static GXSerializer<T> Instance { get; protected set; }

        public IGX<T> GX { get; private set; }

        /// <summary>
        /// Factories 适用于复杂的类型，比如自定义的用 GXAttribute 修饰的类型
        /// </summary>
        public Dictionary<Type, IGXSerializableFactory<T>> Factories { get; private set; }

        /// <summary>
        /// Converters 适用于简单的类型，比如内置值类型，不需要 GXAttribute 修饰
        /// </summary>
        public Dictionary<Type, GXConverter> Converters { get; private set; }

        public GXSerializer(IGX<T> gx, Dictionary<Type, GXConverter> converters = default, Dictionary<Type, IGXSerializableFactory<T>> factories = default)
        {
            GX = gx;

            Converters = converters ?? new Dictionary<Type, GXConverter>()
            {
                { typeof(bool), new GXConverter((vb, s) => vb?.ToString() ?? s, s => (bool.TryParse(s, out var v), v)) },
                { typeof(int), new GXConverter((vi, s) => vi?.ToString() ?? s, s => (int.TryParse(s, out var v), v)) },
                { typeof(float), new GXConverter((vf, s) => (vf is float f) ? f.ToString("F3") : s, s => (float.TryParse(s, out var v), v)) },
                { typeof(string), new GXConverter((vs, s) => vs?.ToString() ?? s, s => (true, s)) },
            };

            Factories = factories ?? new Dictionary<Type, IGXSerializableFactory<T>>();

            Instance = this;
        }

        public T Serialize(object obj, T parent, IGXAttribute refAttr = default, Func<object, string> customStringifier = default)
        {
            if (Stringify(obj, out var result, refAttr, customStringifier))
            {
                var node = GX.CreateChild(parent, GXExtensions.GetValidName(refAttr?.DefaultName(), obj.GetType().Name));
                GX.SetValue(node, result);
                return node;
            }
            else
            {
                return Factories.TryGetValue(obj.GetType(), out var factory) ? factory.Serialize(GX, obj, parent, refAttr) : default;
            }
        }

        public object Deserialize(T node, Type type, Func<object> construcor = default)
        {
            if (Construct(GX.GetValue(node), type, out var result))
            {
                return result;
            }
            else
            {
                return Factories.TryGetValue(type, out var factory) ? factory.Deserialize(GX, node, construcor) : default;
            }
        }

        /// <summary>
        /// 使用内置的 GXConverter 方式将给定类型的实例转换为字符串
        /// </summary>
        public string Stringify(object obj, string @default = default, Func<object, string> customStringifier = default)
        {
            if (obj == null) { return @default; }

            return Converters.TryGetValue(obj.GetType(), out var converter)
                ? converter.Stringify(obj, @default, customStringifier)
                : customStringifier?.Invoke(obj) ?? obj.ToString();
        }

        /// <summary>
        /// 使用内置的 GXConverter 方式将给定类型的实例转换为字符串
        /// </summary>
        public bool Stringify(object obj, out string value, IGXAttribute attr = default, Func<object, string> customStringifier = default)
        {
            if (obj == null)
            {
                value = attr?.Default;
                return false;
            }

            if (Converters.TryGetValue(obj.GetType(), out var converter))
            {
                value = converter.Stringify(obj, attr?.Default, customStringifier);
                return true;
            }
            else
            {
                value = customStringifier?.Invoke(obj) ?? obj.ToString();
                return false;
            }
        }

        /// <summary>
        /// 使用内置的 GXConverter 方式将字符串转换为特定类型，但需要手动转换 object 类型
        /// </summary>
        public bool Construct(string str, Type type, out object obj)
        {
            if (!string.IsNullOrWhiteSpace(str)
                && Converters.TryGetValue(type, out var converter)
                && converter.Construct(str, out obj))
            {
                return true;
            }
            else
            {
                obj = default;
                return false;
            }
        }

        /// <summary>
        /// 使用内置的 GXConverter 方式将字符串转换为特定类型，泛型方式
        /// </summary>
        public TO Construct<TO>(string str, TO @default = default)
        {
            return Construct(str, typeof(TO), out var obj) ? (TO)obj : @default;
        }
    }
}