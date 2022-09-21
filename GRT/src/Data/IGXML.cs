using System.Collections.Generic;
using System;

namespace GRT.Data
{
    public interface IGXML<T>
    {
        string NameOf(T node);

        bool HasInnerString(T node, out string value);
        bool HasInnerBoolean(T node, out bool value);
        bool HasInnerInteger(T node, out int value);
        bool HasInnerFloat(T node, out float value);
        bool HasInner<V>(T node, out V value, Func<string, (bool, V)> parser, V @default = default);

        bool HasAttribute(T node, string name, out string value);
        bool HasAttributeBoolean(T node, string name, out bool value);
        bool HasAttributeInteger(T node, string name, out int value);
        bool HasAttributeFloat(T node, string name, out float value);
        bool HasAttribute<V>(T node, string name, out V value, Func<string, (bool, V)> parser, V @default = default);

        bool HasChild(T node, string name, out T child);
        bool HasChild(T node, Predicate<T> predicate, out T child);

        string GetInnerString(T node);
        bool GetInnerBoolean(T node);
        int GetInnerInteger(T node);
        float GetInnerFloat(T node);
        V GetInner<V>(T node, Func<string, (bool, V)> parser, V @default = default);

        IDictionary<string, string> GetAttributes(T node);
        string GetAttribute(T node, string name);
        bool GetAttributeBoolean(T node, string name);
        int GetAttributeInteger(T node, string name);
        float GetAttributeFloat(T node, string name);
        V GetAttribute<V>(T node, string name, Func<string, (bool, V)> parser, V @default = default);

        T GetChild(T node, string name);
        T GetChild(T node, Predicate<T> predicate);

        IList<T> GetChildren(T node);
        IList<T> GetChildren(T node, string name);
        IList<T> GetChildren(T node, Predicate<T> predicate);
    }
}
