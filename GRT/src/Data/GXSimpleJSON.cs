/*
 * 将Json中的数组看作多个同名的key-value
 */

using SimpleJSON;
using System;
using System.Collections.Generic;

namespace GRT.Data
{
    public static class GXSimpleJSONExtensions
    {
        public static IEnumerable<JSONNode> Each(this JSONNode node, string name)
        {
            var n = node.GetValueOrDefault(name, null);
            if (n != null)
            {
                if (n.IsArray)
                {
                    foreach (var child in n.Children)
                    {
                        yield return child;
                    }
                }
                else
                {
                    yield return n;
                }
            }
        }

        public static IEnumerable<JSONNode> Each(this JSONNode node, Predicate<string> predicate)
        {
            if (node.IsObject)
            {
                foreach (var pair in node)
                {
                    if (predicate.Invoke(pair.Key))
                    {
                        if (pair.Value.IsArray)
                        {
                            foreach (var child in pair.Value.Children)
                            {
                                yield return child;
                            }
                        }
                        else
                        {
                            yield return pair.Value;
                        }
                    }
                }
            }
        }

        public static IEnumerable<JSONNode> Each(this JSONNode node, Predicate<JSONNode> predicate = default)
        {
            if (node.IsObject)
            {
                foreach (var pair in node)
                {
                    if (pair.Value.IsArray)
                    {
                        foreach (var child in pair.Value.Children)
                        {
                            if (predicate == null || predicate.Invoke(child))
                            {
                                yield return child;
                            }
                        }
                    }
                    else
                    {
                        if (predicate == null || predicate.Invoke(pair.Value))
                        {
                            yield return pair.Value;
                        }
                    }
                }
            }
        }

        public static bool Has(this JSONNode node, string name, out JSONNode result)
        {
            var n = node.GetValueOrDefault(name, null);
            if (n != null)
            {
                if (n.IsArray)
                {
                    if (n.Count > 0)
                    {
                        result = n[0];
                        return true;
                    }
                    else
                    {
                        result = default;
                        return false;
                    }
                }
                else
                {
                    result = n;
                    return true;
                }
            }
            else
            {
                result = default;
                return false;
            }
        }

        public static bool Has(this JSONNode node, Predicate<string> predicate, out JSONNode result)
        {
            if (node.IsObject)
            {
                foreach (var pair in node)
                {
                    if (predicate.Invoke(pair.Key))
                    {
                        var n = pair.Value;
                        if (n.IsArray)
                        {
                            if (n.Count > 0)
                            {
                                result = n[0];
                                return true;
                            }
                        }
                        else
                        {
                            result = n;
                            return true;
                        }
                    }
                }
            }

            result = default;
            return false;
        }

        public static bool Has(this JSONNode node, Predicate<JSONNode> predicate, out JSONNode result)
        {
            if (node.IsObject)
            {
                foreach (var pair in node)
                {
                    if (pair.Value.IsArray)
                    {
                        foreach (var child in pair.Value.Children)
                        {
                            if (predicate.Invoke(child))
                            {
                                result = child;
                                return true;
                            }
                        }
                    }
                    else
                    {
                        if (predicate.Invoke(pair.Value))
                        {
                            result = pair.Value;
                            return true;
                        }
                    }
                }
            }

            result = default;
            return false;
        }

        public static bool Stringifiable(this JSONNode node, out string value, string @default = default, bool jsonNull = true)
        {
            if (node.IsObject || node.IsArray || (node.IsNull && !jsonNull))
            {
                value = @default;
                return false;
            }
            else
            {
                value = node.Value;
                return true;
            }
        }
    }

    public class GXSimpleJSON : IGX<JSONNode>
    {
        public const string JNAME = "jname";
        public const string JVALUE = "jvalue";

        public JSONNode Parse(string str) => JSONNode.Parse(str);

        public string Stringify(JSONNode node) => node.ToString(2);

        public string NameOf(JSONNode node) => node[JNAME]?.Value;

        public bool HasChild(JSONNode node, string name, out JSONNode child) => node.Has(name, out child);

        public bool HasChild(JSONNode node, Predicate<JSONNode> predicate, out JSONNode child) => node.Has(predicate, out child);

        public JSONNode GetChild(JSONNode node, string name) => HasChild(node, name, out var child) ? child : default;

        public JSONNode GetChild(JSONNode node, Predicate<JSONNode> predicate) => HasChild(node, predicate, out var child) ? child : default;

        public IEnumerable<JSONNode> GetChildren(JSONNode node) => node.Each();

        public IEnumerable<JSONNode> GetChildren(JSONNode node, string name) => node.Each(name);

        public IEnumerable<JSONNode> GetChildren(JSONNode node, Predicate<JSONNode> predicate) => node.Each(predicate);

        public bool HasValue(JSONNode node, out string value) => node.GetValueOrDefault(JVALUE, node).Stringifiable(out value);

        public bool HasBooleanValue(JSONNode node, out bool value)
        {
            var vn = node.GetValueOrDefault(JVALUE, node);
            var has = vn.IsBoolean;
            value = has ? vn.AsBool : default;
            return has;
        }

        public bool HasIntegerValue(JSONNode node, out int value)
        {
            var vn = node.GetValueOrDefault(JVALUE, node);
            var has = vn.IsNumber;
            value = has ? vn.AsInt : default;
            return has;
        }

        public bool HasFloatValue(JSONNode node, out float value)
        {
            var vn = node.GetValueOrDefault(JVALUE, node);
            var has = vn.IsNumber;
            value = has ? vn.AsFloat : default;
            return has;
        }

        public bool HasValue<V>(JSONNode node, out V value, Func<string, (bool, V)> parser, V @default = default)
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

        public string GetValue(JSONNode node, string @default = default) => HasValue(node, out var value) ? value : @default;

        public bool GetBooleanValue(JSONNode node, bool @default = default) => HasBooleanValue(node, out var value) ? value : @default;

        public int GetIntegerValue(JSONNode node, int @default = default) => HasIntegerValue(node, out var value) ? value : @default;

        public float GetFloatValue(JSONNode node, float @default = default) => HasFloatValue(node, out var value) ? value : @default;

        public V GetValue<V>(JSONNode node, Func<string, (bool, V)> parser, V @default = default) => HasValue(node, out V value, parser) ? value : @default;

        public bool HasKVPair(JSONNode node, string name, out string value)
        {
            if (node.Has(name, out var child))
            {
                return child.Stringifiable(out value);
            }
            else
            {
                value = default;
                return false;
            }
        }

        public bool HasKVPair(JSONNode node, Predicate<string> predicate, out string value)
        {
            if (node.Has(predicate, out var child))
            {
                return child.Stringifiable(out value);
            }
            else
            {
                value = default;
                return false;
            }
        }

        public bool HasBooleanKVPair(JSONNode node, string name, out bool value)
        {
            // foreach (var n in node.Each(name))
            // {
            //     if (n.IsBoolean)
            //     {
            //         value = n.AsBool;
            //         return true;
            //     }
            // }
            // value = default;
            // return false;

            if (node.Has(name, out var child))
            {
                value = child.AsBool;
                return child.IsBoolean;
            }
            else
            {
                value = default;
                return false;
            }
        }

        public bool HasIntegerKVPair(JSONNode node, string name, out int value)
        {
            if (node.Has(name, out var child))
            {
                value = child.AsInt;
                return child.IsNumber;
            }
            else
            {
                value = default;
                return false;
            }
        }

        public bool HasFloatKVPair(JSONNode node, string name, out float value)
        {
            if (node.Has(name, out var child))
            {
                value = child.AsFloat;
                return child.IsNumber;
            }
            else
            {
                value = default;
                return false;
            }
        }

        public bool HasKVPair<V>(JSONNode node, string name, out V value, Func<string, (bool, V)> parser, V @default = default)
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

        public bool HasKVPair<V>(JSONNode node, Predicate<string> predicate, out V value, Func<string, (bool, V)> parser, V @default = default)
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

        public IEnumerable<KeyValuePair<string, string>> GetKVPairs(JSONNode node)
        {
            if (node.IsObject)
            {
                foreach (var pair in node)
                {
                    if (pair.Value.IsArray)
                    {
                        foreach (var child in pair.Value.Children)
                        {
                            if (child.Stringifiable(out var str))
                            {
                                yield return new KeyValuePair<string, string>(pair.Key, str);
                            }
                        }
                    }
                    else
                    {
                        if (pair.Value.Stringifiable(out var str))
                        {
                            yield return new KeyValuePair<string, string>(pair.Key, str);
                        }
                    }
                }
            }
        }

        public string GetKVPair(JSONNode node, string name, string @default = default) => HasKVPair(node, name, out var value) ? value : @default;

        public string GetKVPair(JSONNode node, Predicate<string> predicate, string @default = default) => HasKVPair(node, predicate, out var value) ? value : @default;

        public bool GetBooleanKVPair(JSONNode node, string name, bool @default = default) => HasBooleanKVPair(node, name, out var value) ? value : @default;

        public int GetIntegerKVPair(JSONNode node, string name, int @default = default) => HasIntegerKVPair(node, name, out var value) ? value : @default;

        public float GetFloatKVPair(JSONNode node, string name, float @default = default) => HasFloatKVPair(node, name, out var value) ? value : @default;

        public V GetKVPair<V>(JSONNode node, string name, Func<string, (bool, V)> parser, V @default = default) => HasKVPair(node, name, out var value, parser) ? value : @default;

        public V GetKVPair<V>(JSONNode node, Predicate<string> predicate, Func<string, (bool, V)> parser, V @default = default) => HasKVPair(node, predicate, out var value, parser) ? value : @default;

        public JSONNode CreateRoot(string name)
        {
            var root = new JSONObject();
            root.Add(JNAME, name);
            return root;
        }

        public JSONNode CreateChild(JSONNode node, string childName)
        {
            var child = new JSONObject();
            child.Add(JNAME, childName);

            if (node.HasKey(childName))
            {
                if (node[childName] is JSONArray children)
                {
                    children.Add(child);
                }
                else
                {
                    var original = node[childName];
                    var array = new JSONArray();
                    array.Add(original);
                    array.Add(child);
                    node[childName] = array;
                }
            }
            else
            {
                node.Add(childName, child);
            }

            return child;
        }

        public void Add(JSONNode parent, JSONNode child)
        {
            parent.Add(child);
        }

        public void Remove(JSONNode parent, JSONNode child)
        {
            parent.Remove(child);
        }

        public void SetValue(JSONNode node, string value) => node[JVALUE] = value;

        public void SetBooleanValue(JSONNode node, bool value) => node[JVALUE] = value;

        public void SetIntegerValue(JSONNode node, int value) => node[JVALUE] = value;

        public void SetFloatValue(JSONNode node, float value, int @decimal = 2) => node[JVALUE] = Math.Round(value, @decimal);

        public void SetValue<V>(JSONNode node, V value, Func<V, string> stringifier = null) => node[JVALUE] = stringifier?.Invoke(value) ?? value.ToString();

        public void SetKVPair(JSONNode node, string name, string value) => node[name] = value;

        public void SetBooleanKVPair(JSONNode node, string name, bool value) => node[name] = value;

        public void SetIntegerKVPair(JSONNode node, string name, int value) => node[name] = value;

        public void SetFloatKVPair(JSONNode node, string name, float value, int @decimal = 2) => node[name] = Math.Round(value, @decimal);

        public void SetKVPair<V>(JSONNode node, string name, V value, Func<V, string> stringifier = null) => node[name] = stringifier?.Invoke(value) ?? value.ToString();
    }
}
