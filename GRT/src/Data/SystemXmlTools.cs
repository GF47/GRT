using GRT.Data;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Modules
{
    public class SystemXmlTools : GXML<XmlNode>
    {
        // public override string GetAttribute(XmlNode node, string name) => node?.SelectSingleNode($"@{name}")?.Value; // node?.attributes[name]?.Value;

        public override string NameOf(XmlNode node) => node?.Name;

        public override bool HasInnerString(XmlNode node, out string value)
        {
            value = node?.InnerText;
            return !string.IsNullOrEmpty(value);
        }

        public override bool HasAttribute(XmlNode node, string name, out string value)
        {
            var attributes = node?.Attributes;
            if (attributes != null && attributes.Count > 0)
            {
                var attribute = attributes[name];
                if (attribute == null)
                {
                    value = null;
                    return false;
                }
                else
                {
                    value = attribute.Value;
                    return !string.IsNullOrEmpty(value);
                }
            }
            else
            {
                value = null;
                return false;
            }
        }

        public override bool HasAttribute(XmlNode node, Predicate<string> predicate, out string value)
        {
            var attributes = node?.Attributes;
            if (attributes != null && attributes.Count > 0)
            {
                foreach (XmlAttribute attribute in attributes)
                {
                    if (predicate(attribute.Name))
                    {
                        value = attribute.Value;
                        return !string.IsNullOrEmpty(value);
                    }
                }
            }

            value = null;
            return false;
        }

        public override IEnumerable<KeyValuePair<string, string>> GetAttributes(XmlNode node)
        {
            var attributes = node?.Attributes;
            if (attributes != null && attributes.Count > 0)
            {
                foreach (XmlAttribute attribute in attributes)
                {
                    yield return new KeyValuePair<string, string>(attribute.Name, attribute.Value);
                }
            }
            else
            {
                yield break;
            }
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
    }
}