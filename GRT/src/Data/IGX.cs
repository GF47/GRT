using System;
using System.Collections.Generic;

namespace GRT.Data
{
    public interface IGX<T>
    {
        T Parse(string str);

        string NameOf(T node);

        #region child

        bool HasChild(T node, string name, out T child);

        bool HasChild(T node, Predicate<T> predicate, out T child);

        T GetChild(T node, string name);

        T GetChild(T node, Predicate<T> predicate);

        IEnumerable<T> GetChildren(T node);

        IEnumerable<T> GetChildren(T node, string name);

        IEnumerable<T> GetChildren(T node, Predicate<T> predicate);

        #endregion child

        #region has value

        bool HasValue(T node, out string value);

        bool HasBooleanValue(T node, out bool value);

        bool HasIntegerValue(T node, out int value);

        bool HasFloatValue(T node, out float value);

        bool HasValue<V>(T node, out V value, Func<string, (bool, V)> parser, V @default = default);

        #endregion has value

        #region get value

        string GetValue(T node, string @default = default);

        bool GetBooleanValue(T node, bool @default = default);

        int GetIntegerValue(T node, int @default = default);

        float GetFloatValue(T node, float @default = default);

        V GetValue<V>(T node, Func<string, (bool, V)> parser, V @default = default);

        #endregion get value

        #region has kvpair

        bool HasKVPair(T node, string name, out string value);

        bool HasKVPair(T node, Predicate<string> predicate, out string value);

        bool HasBooleanKVPair(T node, string name, out bool value);

        bool HasIntegerKVPair(T node, string name, out int value);

        bool HasFloatKVPair(T node, string name, out float value);

        bool HasKVPair<V>(T node, string name, out V value, Func<string, (bool, V)> parser, V @default = default);

        bool HasKVPair<V>(T node, Predicate<string> predicate, out V value, Func<string, (bool, V)> parser, V @default = default);

        #endregion has kvpair

        #region get kvpair

        IEnumerable<KeyValuePair<string, string>> GetKVPairs(T node);

        string GetKVPair(T node, string name, string @default = default);

        string GetKVPair(T node, Predicate<string> predicate, string @default = default);

        bool GetBooleanKVPair(T node, string name, bool @default = default);

        int GetIntegerKVPair(T node, string name, int @default = default);

        float GetFloatKVPair(T node, string name, float @default = default);

        V GetKVPair<V>(T node, string name, Func<string, (bool, V)> parser, V @default = default);

        V GetKVPair<V>(T node, Predicate<string> predicate, Func<string, (bool, V)> parser, V @default = default);

        #endregion get kvpair

        #region serialize

        T CreateRoot(string name);

        T CreateChild(T node, string childName);

        void SetValue(T node, string value);

        void SetBooleanValue(T node, bool value);

        void SetIntegerValue(T node, int value);

        void SetFloatValue(T node, float value, int @decimal = 2);

        void SetValue<V>(T node, V value, Func<V, string> stringifier = null);

        void SetKVPair(T node, string name, string value);

        void SetBooleanKVPair(T node, string name, bool value);

        void SetIntegerKVPair(T node, string name, int value);

        void SetFloatKVPair(T node, string name, float value, int @decimal = 2);

        void SetKVPair<V>(T node, string name, V value, Func<V, string> stringifier = null);

        #endregion serialize
    }
}