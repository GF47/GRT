using System;
using System.Collections.Generic;

namespace GRT.Data
{
    public abstract class GXML<T> : IGXML<T>
    {
        public abstract string NameOf(T node);

        public abstract string GetAttribute(T node, string name);

        public V GetAttribute<V>(T node, string name, Func<string, (bool, V)> parser, V @default = default)
        {
            if (HasAttribute(node, name, out var str))
            {
                var (_, value) = parser(str);
                return value;
            }
            else
            {
                return @default;
            }
        }

        public bool GetAttributeBoolean(T node, string name) => (HasAttribute(node, name, out var str) && bool.TryParse(str, out var value)) ? value : default;

        public float GetAttributeFloat(T node, string name) => (HasAttribute(node, name, out var str) && float.TryParse(str, out var value)) ? value : default;

        public int GetAttributeInteger(T node, string name) => (HasAttribute(node, name, out var str) && int.TryParse(str, out var value)) ? value : default;

        public T GetChild(T node, string name)
        {
            var children = GetChildren(node);
            if (children != null)
            {
                foreach (var child in children)
                {
                    if (NameOf(child) == name)
                    {
                        return child;
                    }
                }
            }
            return default;
        }

        public T GetChild(T node, Predicate<T> predicate)
        {
            var children = GetChildren(node);
            if (children != null)
            {
                foreach (var child in children)
                {
                    if (predicate(child))
                    {
                        return child;
                    }
                }
            }
            return default;
        }

        public IList<T> GetChildren(T node, string name)
        {
            var children = GetChildren(node);
            if (children != null)
            {
                var list = new List<T>(children.Count);
                foreach (var child in children)
                {
                    if (NameOf(child) == name)
                    {
                        list.Add(child);
                    }
                }
                list.TrimExcess();
                return list;
            }
            else
            {
                return null;
            }
        }

        public IList<T> GetChildren(T node, Predicate<T> predicate)
        {
            var children = GetChildren(node);
            if (children != null)
            {
                var list = new List<T>(children.Count);
                foreach (var child in children)
                {
                    if (predicate(child))
                    {
                        list.Add(child);
                    }
                }
                list.TrimExcess();
                return list;
            }
            else
            {
                return null;
            }
        }

        public abstract IList<T> GetChildren(T node);

        public V GetInner<V>(T node, Func<string, (bool, V)> parser, V @default = default)
        {
            if (HasInnerString(node, out var str))
            {
                var (_, value) = parser(str);
                return value;
            }
            else
            {
                return default;
            }
        }

        public bool GetInnerBoolean(T node) => (HasInnerString(node, out var str) && bool.TryParse(str, out var value)) ? value : default;

        public float GetInnerFloat(T node) => (HasInnerString(node, out var str) && float.TryParse(str, out var value)) ? value : default;

        public int GetInnerInteger(T node) => (HasInnerString(node, out var str) && int.TryParse(str, out var value)) ? value : default;

        public abstract string GetInnerString(T node);

        public virtual bool HasAttribute(T node, string name, out string value)
        {
            value = GetAttribute(node, name);
            return !string.IsNullOrEmpty(value);
        }

        public bool HasAttribute<V>(T node, string name, out V value, Func<string, (bool, V)> parser, V @default = default)
        {
            if (HasAttribute(node, name, out var str))
            {
                var (result, value2) = parser(str);
                value = value2;
                return result;
            }
            else
            {
                value = default;
                return false;
            }
        }

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

        public bool HasChild(T node, string name, out T child)
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

        public bool HasChild(T node, Predicate<T> predicate, out T child)
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

        public bool HasInner<V>(T node, out V value, Func<string, (bool, V)> parser, V @default = default)
        {
            if (HasInnerString(node, out var str))
            {
                var (result, value2) = parser(str);
                value = value2;
                return result;
            }
            else
            {
                value = @default;
                return false;
            }
        }

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

        public bool HasInnerString(T node, out string value)
        {
            value = GetInnerString(node);
            return !string.IsNullOrEmpty(value);
        }
    }
}