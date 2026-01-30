using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace GRT.Data
{
    public class GXSystemXmlImpl : GXBase<XmlNode>
    {
        public override XmlNode Parse(string str)
        {
            var doc = new XmlDocument();
            doc.LoadXml(str);
            return doc.LastChild;
        }

        public override string Stringify(XmlNode node)
        {
            if (node is XmlDocument doc)
            {
                using (StringWriter sw = new StringWriter())
                {
                    using (var tx = XmlWriter.Create(sw))
                    {
                        doc.WriteTo(tx);
                        return tx.ToString();
                    }
                }
            }
            else
            {
                return node.OuterXml;
            }
        }

        public override string NameOf(XmlNode node)
        {
            return node?.Name;
        }

        public override IEnumerable<XmlNode> GetChildren(XmlNode node)
        {
            if (node != null)
            {
                foreach (XmlNode child in node.ChildNodes)
                {
                    yield return child;
                }
            }
            else
            {
                yield break;
            }
        }

        public override bool HasValue(XmlNode node, out string value)
        {
            value = node?.InnerText;
            return !string.IsNullOrEmpty(value);
        }

        public override bool HasKVPair(XmlNode node, string name, out string value)
        {
            var pairs = node?.Attributes;
            if (pairs != null && pairs.Count > 0)
            {
                var pair = pairs[name];
                if (pair == null)
                {
                    value = null;
                    return false;
                }
                else
                {
                    value = pair.Value;
                    return true; // ??? 原来是 !string.IsNullOrEmpty(value) 哪个好
                }
            }
            else
            {
                value = null;
                return false;
            }
        }

        public override bool HasKVPair(XmlNode node, Predicate<string> predicate, out string value)
        {
            var pairs = node?.Attributes;
            if (pairs != null && pairs.Count > 0)
            {
                foreach (XmlAttribute pair in pairs)
                {
                    if (predicate(pair.Name))
                    {
                        value = pair.Value;
                        return true; // ??? 原来是 !string.IsNullOrEmpty(value) 哪个好
                    }
                }
            }

            value = null;
            return false;
        }

        public override IEnumerable<KeyValuePair<string, string>> GetKVPairs(XmlNode node)
        {
            var pairs = node?.Attributes;
            if (pairs != null && pairs.Count != 0)
            {
                foreach (XmlAttribute pair in pairs)
                {
                    yield return new KeyValuePair<string, string>(pair.Name, pair.Value);
                }
            }
            else
            {
                yield break;
            }
        }

        public override XmlNode CreateRoot(string name)
        {
            var doc = new XmlDocument();
            var root = doc.CreateElement(name);
            doc.PrependChild(root);
            return root;
        }

        public override XmlNode CreateChild(XmlNode node, string childName)
        {
            var child = node.OwnerDocument.CreateElement(childName);
            node.AppendChild(child);
            return child;
        }

        public override void Add(XmlNode parent, XmlNode child)
        {
            parent.AppendChild(child);
        }

        public override void Remove(XmlNode parent, XmlNode child)
        {
            parent.RemoveChild(child);
        }

        public override void SetValue(XmlNode node, string value)
        {
            node.InnerText = value;
        }

        public override void SetKVPair(XmlNode node, string name, string value)
        {
            var pair = node.Attributes[name];
            if (pair != null)
            {
                pair.Value = value;
            }
            else
            {
                var doc = node.OwnerDocument;
                pair = doc.CreateAttribute(name);
                pair.Value = value;
                node.Attributes.Append(pair);
            }
        }
    }
}