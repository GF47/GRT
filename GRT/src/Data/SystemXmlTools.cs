using GRT.Data;
using System.Collections.Generic;
using System.Xml;

namespace Modules
{
    public class SystemXmlTools : GXML<XmlNode>
    {
        public override string GetAttribute(XmlNode node, string name) => node?.SelectSingleNode($"@{name}")?.Value; // node?.attributes[name]?.Value;

        public override IEnumerable<KeyValuePair<string, string>> GetAttributes(XmlNode node)
        {
            var attributes = node?.Attributes;
            if (attributes != null && attributes.Count > 0)
            {
                foreach (XmlAttribute attribute in attributes)
                {
                    yield return new KeyValuePair<string, string> (attribute.Name, attribute.Value);
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

        public override string GetInnerString(XmlNode node) => node?.InnerText;

        public override string NameOf(XmlNode node) => node?.Name;
    }
}