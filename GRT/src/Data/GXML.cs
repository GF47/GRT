using System;
using System.Collections.Generic;

namespace GRT.Data
{
    public abstract class GXML<T> : IGXML<T>
    {
        public abstract string NameOf(T node);

        #region has inner

        public abstract bool HasInnerString(T node, out string value);

        public bool HasInnerBoolean(T node, out bool value)
        {
            if (HasInnerString(node, out var str))
            {
                return bool.TryParse(str, out value);
            }
            else
            {
                value = default;
                return false;
            }
        }

        public bool HasInnerInteger(T node, out int value)
        {
            if (HasInnerString(node, out var str))
            {
                return int.TryParse(str, out value);
            }
            else
            {
                value = default;
                return false;
            }
        }

        public bool HasInnerFloat(T node, out float value)
        {
            if (HasInnerString(node, out var str))
            {
                return float.TryParse(str, out value);
            }
            else
            {
                value = default;
                return false;
            }
        }

        public bool HasInner<V>(T node, out V value, Func<string, (bool, V)> parser, V @default = default)
        {
            if (HasInnerString(node, out var str))
            {
                var (has, value2) = parser(str);
                value = has ? value2 : @default;
                return has;
            }
            else
            {
                value = @default;
                return false;
            }
        }

        #endregion has inner

        #region get inner

        public virtual string GetInnerString(T node, string @default = default) =>
            HasInnerString(node, out var value) ? value : @default;

        public bool GetInnerBoolean(T node, bool @default = default) =>
            HasInnerBoolean(node, out var value) ? value : @default;

        public int GetInnerInteger(T node, int @default = default) =>
            HasInnerInteger(node, out var value) ? value : @default;

        public float GetInnerFloat(T node, float @default = default) =>
            HasInnerFloat(node, out var value) ? value : @default;

        public V GetInner<V>(T node, Func<string, (bool, V)> parser, V @default = default) =>
            HasInner(node, out var value, parser) ? value : @default;

        #endregion get inner

        #region has attribute

        public abstract bool HasAttribute(T node, string name, out string value);

        public abstract bool HasAttribute(T node, Predicate<string> predicate, out string value);

        public bool HasAttributeBoolean(T node, string name, out bool value)
        {
            if (HasAttribute(node, name, out var str))
            {
                return bool.TryParse(str, out value);
            }
            else
            {
                value = default;
                return false;
            }
        }

        public bool HasAttributeInteger(T node, string name, out int value)
        {
            if (HasAttribute(node, name, out var str))
            {
                return int.TryParse(str, out value);
            }
            else
            {
                value = default;
                return false;
            }
        }

        public bool HasAttributeFloat(T node, string name, out float value)
        {
            if (HasAttribute(node, name, out var str))
            {
                return float.TryParse(str, out value);
            }
            else
            {
                value = default;
                return false;
            }
        }

        public bool HasAttribute<V>(T node, string name, out V value, Func<string, (bool, V)> parser, V @default = default)
        {
            if (HasAttribute(node, name, out var str))
            {
                var (has, value2) = parser(str);
                value = has ? value2 : @default;
                return has;
            }
            else
            {
                value = @default;
                return false;
            }
        }

        public bool HasAttribute<V>(T node, Predicate<string> predicate, out V value, Func<string, (bool, V)> parser, V @default = default)
        {
            if (HasAttribute(node, predicate, out var str))
            {
                var (has, value2) = parser(str);
                value = has ? value2 : @default;
                return has;
            }
            else
            {
                value = @default;
                return false;
            }
        }

        #endregion has attribute

        public abstract IEnumerable<KeyValuePair<string, string>> GetAttributes(T node);

        #region get attribute

        public virtual string GetAttribute(T node, string name, string @default = default) =>
            HasAttribute(node, name, out var str) ? str : @default;

        public virtual string GetAttribute(T node, Predicate<string> predicate, string @default = default) =>
            HasAttribute(node, predicate, out var value) ? value : @default;

        public bool GetAttributeBoolean(T node, string name, bool @default = default) =>
            HasAttributeBoolean(node, name, out var value) ? value : @default;

        public int GetAttributeInteger(T node, string name, int @default = default) =>
            HasAttributeInteger(node, name, out var value) ? value : @default;

        public float GetAttributeFloat(T node, string name, float @default = default) =>
            HasAttributeFloat(node, name, out var value) ? value : @default;

        public V GetAttribute<V>(T node, string name, Func<string, (bool, V)> parser, V @default = default) =>
            HasAttribute(node, name, out var value, parser) ? value : @default;

        public V GetAttribute<V>(T node, Predicate<string> predicate, Func<string, (bool, V)> parser, V @default = default) =>
            HasAttribute(node, predicate, out var value, parser) ? value : @default;

        #endregion get attribute

        #region child

        public virtual bool HasChild(T node, string name, out T child)
        {
            var children = GetChildren(node);
            if (children != null)
            {
                foreach (var child_ in children)
                {
                    if (NameOf(child_) == name)
                    {
                        child = child_;
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
                foreach (var child_ in children)
                {
                    if (predicate(child_))
                    {
                        child = child_;
                        return true;
                    }
                }
            }
            child = default;
            return false;
        }

        public virtual T GetChild(T node, string name) =>
            HasChild(node, name, out var child) ? child : default;

        public virtual T GetChild(T node, Predicate<T> predicate) =>
            HasChild(node, predicate, out var child) ? child : default;

        public IEnumerable<T> GetChildren(T node, string name)
        {
            var children = GetChildren(node);
            if (children != null)
            {
                foreach (var child in children)
                {
                    if (NameOf(child) == name)
                    {
                        yield return child;
                    }
                }
            }
            else
            {
                yield break;
            }
        }

        public IEnumerable<T> GetChildren(T node, Predicate<T> predicate)
        {
            var children = GetChildren(node);
            if (children != null)
            {
                foreach (var child in children)
                {
                    if (predicate(child))
                    {
                        yield return child;
                    }
                }
            }
            else
            {
                yield break;
            }
        }

        public abstract IEnumerable<T> GetChildren(T node);

        #endregion child

        #region serialize

        public abstract T CreateRoot(string name);

        public abstract T CreateChild(T node, string childName);

        #region inner

        public abstract void SetInnerString(T node, string value);

        public void SetInnerBoolean(T node, bool value) =>
            SetInnerString(node, value ? bool.TrueString : bool.FalseString);

        public void SetInnerInteger(T node, int value) =>
            SetInnerString(node, value.ToString());

        public void SetInnerFloat(T node, float value, int @decimal = 2) =>
            SetInnerString(node, value.ToString($"F{@decimal}"));

        public virtual void SetInner<V>(T node, V value) =>
            SetInnerString(node, value.ToString());

        #endregion inner

        #region attribute

        public abstract void SetAttribute(T node, string name, string value);

        public void SetAttributeBoolean(T node, string name, bool value) =>
            SetAttribute(node, name, value ? bool.TrueString : bool.FalseString);

        public void SetAttributeInteger(T node, string name, int value) =>
            SetAttribute(node, name, value.ToString());

        public void SetAttributeFloat(T node, string name, float value, int @decimal = 2) =>
            SetAttribute(node, name, value.ToString($"F{@decimal}"));

        public virtual void SetAttribute<V>(T node, string name, V value) =>
            SetAttribute(node, name, value.ToString());

        #endregion attribute

        #endregion serialize
    }
}