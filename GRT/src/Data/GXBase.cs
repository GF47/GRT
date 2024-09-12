using System;
using System.Collections.Generic;

namespace GRT.Data
{
    public abstract class GXBase<T> : IGX<T>
    {
        public abstract T Parse(string str);

        public abstract string NameOf(T node);

        #region child

        public virtual bool HasChild(T node, string name, out T child)
        {
            var children = GetChildren(node);
            if (children != null)
            {
                foreach (var c in children)
                {
                    if (NameOf(c) == name)
                    {
                        child = c;
                        return true;
                    }
                }
            }

            child = default;
            return false;
        }

        public virtual bool HasChild(T node, Predicate<T> predicate, out T child)
        {
            var children = GetChildren(node);
            if (children != null)
            {
                foreach (var c in children)
                {
                    if (predicate(c))
                    {
                        child = c;
                        return true;
                    }
                }
            }

            child = default;
            return false;
        }

        public virtual T GetChild(T node, string name) => HasChild(node, name, out var child) ? child : default;

        public virtual T GetChild(T node, Predicate<T> predicate) => HasChild(node, predicate, out var child) ? child : default;

        public abstract IEnumerable<T> GetChildren(T node);

        public virtual IEnumerable<T> GetChildren(T node, string name)
        {
            var children = GetChildren(node);
            if (children != null)
            {
                foreach (var c in children)
                {
                    if (NameOf(c) == name)
                    {
                        yield return c;
                    }
                }
            }
            else
            {
                yield break;
            }
        }

        public virtual IEnumerable<T> GetChildren(T node, Predicate<T> predicate)
        {
            var children = GetChildren(node);
            if (children != null)
            {
                foreach (var c in children)
                {
                    if (predicate(c))
                    {
                        yield return c;
                    }
                }
            }
            else
            {
                yield break;
            }
        }

        #endregion child

        #region has value

        public abstract bool HasValue(T node, out string value);

        public bool HasBooleanValue(T node, out bool value)
        {
            if (HasValue(node, out var str))
            {
                return bool.TryParse(str, out value);
            }
            else
            {
                value = default;
                return false;
            }
        }

        public bool HasIntegerValue(T node, out int value)
        {
            if (HasValue(node, out var str))
            {
                return int.TryParse(str, out value);
            }
            else
            {
                value = default;
                return false;
            }
        }

        public bool HasFloatValue(T node, out float value)
        {
            if (HasValue(node, out var str))
            {
                return float.TryParse(str, out value);
            }
            else
            {
                value = default;
                return false;
            }
        }

        public bool HasValue<V>(T node, out V value, Func<string, (bool, V)> parser, V @default = default)
        {
            if (HasValue(node, out var str))
            {
                var (has, v) = parser(str);
                value = has ? v : @default;
                return has;
            }
            else
            {
                value = @default;
                return false;
            }
        }

        #endregion has value

        #region get value

        public virtual string GetValue(T node, string @default = default) => HasValue(node, out var value) ? value : @default;

        public bool GetBooleanValue(T node, bool @default = default) => HasBooleanValue(node, out var value) ? value : @default;

        public int GetIntegerValue(T node, int @default = default) => HasIntegerValue(node, out var value) ? value : @default;

        public float GetFloatValue(T node, float @default = default) => HasFloatValue(node, out var value) ? value : @default;

        public V GetValue<V>(T node, Func<string, (bool, V)> parser, V @default = default) => HasValue(node, out V value, parser) ? value : @default;

        #endregion get value

        #region has kvpair

        public abstract bool HasKVPair(T node, string name, out string value);

        public abstract bool HasKVPair(T node, Predicate<string> predicate, out string value);

        public bool HasBooleanKVPair(T node, string name, out bool value)
        {
            if (HasKVPair(node, name, out var str))
            {
                return bool.TryParse(str, out value);
            }
            else
            {
                value = default;
                return false;
            }
        }

        public bool HasIntegerKVPair(T node, string name, out int value)
        {
            if (HasKVPair(node, name, out var str))
            {
                return int.TryParse(str, out value);
            }
            else
            {
                value = default;
                return false;
            }
        }

        public bool HasFloatKVPair(T node, string name, out float value)
        {
            if (HasKVPair(node, name, out var str))
            {
                return float.TryParse(str, out value);
            }
            else
            {
                value = default;
                return false;
            }
        }

        public bool HasKVPair<V>(T node, string name, out V value, Func<string, (bool, V)> parser, V @default = default)
        {
            if (HasKVPair(node, name, out var str))
            {
                var (has, v) = parser(str);
                value = has ? v : @default;
                return has;
            }
            else
            {
                value = @default;
                return false;
            }
        }

        public bool HasKVPair<V>(T node, Predicate<string> predicate, out V value, Func<string, (bool, V)> parser, V @default = default)
        {
            if (HasKVPair(node, predicate, out var str))
            {
                var (has, v) = parser(str);
                value = has ? v : @default;
                return has;
            }
            else
            {
                value = @default;
                return false;
            }
        }

        #endregion has kvpair

        #region get kvpair

        public abstract IEnumerable<KeyValuePair<string, string>> GetKVPairs(T node);

        public virtual string GetKVPair(T node, string name, string @default = default) => HasKVPair(node, name, out var value) ? value : @default;

        public virtual string GetKVPair(T node, Predicate<string> predicate, string @default = default) => HasKVPair(node, predicate, out var value) ? value : @default;

        public bool GetBooleanKVPair(T node, string name, bool @default = default) => HasBooleanKVPair(node, name, out var value) ? value : @default;

        public int GetIntegerKVPair(T node, string name, int @default = default) => HasIntegerKVPair(node, name, out var value) ? value : @default;

        public float GetFloatKVPair(T node, string name, float @default = default) => HasFloatKVPair(node, name, out var value) ? value : @default;

        public V GetKVPair<V>(T node, string name, Func<string, (bool, V)> parser, V @default = default) => HasKVPair(node, name, out var value, parser) ? value : @default;

        public V GetKVPair<V>(T node, Predicate<string> predicate, Func<string, (bool, V)> parser, V @default = default) => HasKVPair(node, predicate, out var value, parser) ? value : @default;

        #endregion get kvpair

        #region serialize

        public abstract T CreateRoot(string name);

        public abstract T CreateChild(T node, string childName);

        public abstract void SetValue(T node, string value);

        public void SetBooleanValue(T node, bool value) => SetValue(node, value ? bool.TrueString : bool.FalseString);

        public void SetIntegerValue(T node, int value) => SetValue(node, value.ToString());

        public void SetFloatValue(T node, float value, int @decimal = 2) => SetValue(node, value.ToString($"F{@decimal}"));

        public virtual void SetValue<V>(T node, V value, Func<V, string> stringifier = null) => SetValue(node, stringifier?.Invoke(value) ?? value.ToString());

        public abstract void SetKVPair(T node, string name, string value);

        public void SetBooleanKVPair(T node, string name, bool value) => SetKVPair(node, name, value ? bool.TrueString : bool.FalseString);

        public void SetIntegerKVPair(T node, string name, int value) => SetKVPair(node, name, value.ToString());

        public void SetFloatKVPair(T node, string name, float value, int @decimal = 2) => SetKVPair(node, name, value.ToString($"F{@decimal}"));

        public virtual void SetKVPair<V>(T node, string name, V value, Func<V, string> stringifier = null) => SetKVPair(node, name, stringifier?.Invoke(value) ?? value.ToString());

        #endregion serialize
    }
}