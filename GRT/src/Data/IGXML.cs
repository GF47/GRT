using System;
using System.Collections.Generic;

namespace GRT.Data
{
    public interface IGXML<T>
    {
        string NameOf(T node);

        #region has inner

        bool HasInnerString(T node, out string value);

        bool HasInnerBoolean(T node, out bool value);

        bool HasInnerInteger(T node, out int value);

        bool HasInnerFloat(T node, out float value);

        bool HasInner<V>(T node, out V value, Func<string, (bool, V)> parser, V @default = default);

        #endregion has inner

        #region get inner

        string GetInnerString(T node, string @default = default);

        bool GetInnerBoolean(T node, bool @default = default);

        int GetInnerInteger(T node, int @default = default);

        float GetInnerFloat(T node, float @default = default);

        V GetInner<V>(T node, Func<string, (bool, V)> parser, V @default = default);

        #endregion get inner

        #region has attribute

        bool HasAttribute(T node, string name, out string value);

        bool HasAttribute(T node, Predicate<string> predicate, out string value);

        bool HasAttributeBoolean(T node, string name, out bool value);

        bool HasAttributeInteger(T node, string name, out int value);

        bool HasAttributeFloat(T node, string name, out float value);

        bool HasAttribute<V>(T node, string name, out V value, Func<string, (bool, V)> parser, V @default = default);

        bool HasAttribute<V>(T node, Predicate<string> predicate, out V value, Func<string, (bool, V)> parser, V @default = default);

        #endregion has attribute

        IEnumerable<KeyValuePair<string, string>> GetAttributes(T node);

        #region get attribute

        string GetAttribute(T node, string name, string @default = default);

        string GetAttribute(T node, Predicate<string> predicate, string @default = default);

        bool GetAttributeBoolean(T node, string name, bool @default = default);

        int GetAttributeInteger(T node, string name, int @default = default);

        float GetAttributeFloat(T node, string name, float @default = default);

        V GetAttribute<V>(T node, string name, Func<string, (bool, V)> parser, V @default = default);

        V GetAttribute<V>(T node, Predicate<string> predicate, Func<string, (bool, V)> parser, V @default = default);

        #endregion get attribute

        #region child

        bool HasChild(T node, string name, out T child);

        bool HasChild(T node, Predicate<T> predicate, out T child);

        T GetChild(T node, string name);

        T GetChild(T node, Predicate<T> predicate);

        IEnumerable<T> GetChildren(T node);

        IEnumerable<T> GetChildren(T node, string name);

        IEnumerable<T> GetChildren(T node, Predicate<T> predicate);

        #endregion child

        #region serialize

        T CreateRoot(string name);

        T CreateChild(T node, string childName);

        #region inner

        void SetInnerString(T node, string value);

        void SetInnerBoolean(T node, bool value);

        void SetInnerInteger(T node, int value);

        void SetInnerFloat(T node, float value, int @decimal = 2);

        void SetInner<V>(T node, V value);

        #endregion inner

        #region attribute

        void SetAttribute(T node, string name, string value);

        void SetAttributeBoolean(T node, string name, bool value);

        void SetAttributeInteger(T node, string name, int value);

        void SetAttributeFloat(T node, string name, float value, int @decimal = 2);

        void SetAttribute<V>(T node, string name, V value);

        #endregion attribute

        #endregion serialize
    }
}