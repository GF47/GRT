using SimpleJSON;
using System;
using System.Collections.Generic;

namespace GRT.Data
{
    public class GXSimpleJSONImpl : GXBase<JSONNode>
    {
        public const string JNAME = "jname";
        public const string JVALUE = "jvalue";

        public override JSONNode CreateChild(JSONNode node, string childName)
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

        public override JSONNode CreateRoot(string name)
        {
            var root = new JSONObject();
            root.Add(JNAME, name);
            return root;
        }

        public override IEnumerable<JSONNode> GetChildren(JSONNode node)
        {
            foreach (var pair in node)
            {
                if (pair.Value is JSONArray array)
                {
                    foreach (var child in array.Children)
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

        public override IEnumerable<JSONNode> GetChildren(JSONNode node, string name)
        {
            foreach (var pair in node)
            {
                if (pair.Key == name)
                {
                    if (pair.Value is JSONArray array)
                    {
                        foreach (var child in array.Children)
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

        public override IEnumerable<JSONNode> GetChildren(JSONNode node, Predicate<JSONNode> predicate)
        {
            if (predicate == null) { throw new ArgumentNullException(nameof(predicate)); }

            foreach (var pair in node)
            {
                if (predicate.Invoke(pair.Key))
                {
                    if (pair.Value is JSONArray array)
                    {
                        foreach (var child in array.Children)
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

        public override bool HasChild(JSONNode node, string name, out JSONNode child)
        {
            foreach (var pair in node)
            {
                if (pair.Key == name)
                {
                    child = pair.Value;
                    return true;
                }
            }

            child = null;
            return false;
        }

        public override bool HasChild(JSONNode node, Predicate<JSONNode> predicate, out JSONNode child)
        {
            if (predicate == null) { throw new ArgumentNullException(nameof(predicate)); }

            foreach (var pair in node)
            {
                if (predicate.Invoke(pair.Key))
                {
                    child = pair.Value;
                    return true;
                }
            }

            child = null;
            return false;
        }

        public override IEnumerable<KeyValuePair<string, string>> GetKVPairs(JSONNode node)
        {
            foreach (var pair in node)
            {
                yield return new KeyValuePair<string, string>(pair.Key, pair.Value.Value);
            }
        }

        public override bool HasKVPair(JSONNode node, string name, out string value)
        {
            foreach (var child in node)
            {
                if (child.Key == name)
                {
                    value = child.Value;
                    return true;
                }
            }

            value = null;
            return false;
        }

        public override bool HasKVPair(JSONNode node, Predicate<string> predicate, out string value)
        {
            if (predicate == null) { throw new ArgumentNullException(nameof(predicate)); }
            foreach (var child in node)
            {
                if (predicate.Invoke(child.Key))
                {
                    value = child.Value;
                    return true;
                }
            }

            value = null;
            return false;
        }

        public override bool HasValue(JSONNode node, out string value)
        {
            value = node[JVALUE]?.Value;
            return !string.IsNullOrWhiteSpace(value) && value != "null";
        }

        public override string NameOf(JSONNode node) => node.GetValueOrDefault(JNAME, null);

        public override JSONNode Parse(string str) => JSON.Parse(str);

        public override void SetKVPair(JSONNode node, string name, string value)
        {
            if (node.HasKey(name))
            {
                node[name] = value;
            }
            else
            {
                node.Add(name, value);
            }
        }

        public override void SetValue(JSONNode node, string value) => SetKVPair(node, JVALUE, value);
    }
}