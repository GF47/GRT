using System;
using System.Xml;

namespace GRT.Data
{
    public static class XmlTools
    {
        public delegate bool TryParse<T>(string text, ref T value);

        public static XmlNode GetRootNode(string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            return doc.LastChild;
        }
        public static bool HasInnerText(this XmlNode node, ref string text)
        {
            if (node == null) return false;

            text = node.InnerText;
            return true;
        }
        public static bool CanGetInnerTextAsInteger(this XmlNode node, ref int value)
        {
            string text = string.Empty;
            if (node.HasInnerText(ref text))
            {
                if (int.TryParse(text, out value))
                {
                    return true;
                }
            }
            return false;
        }
        public static bool CanGetInnerTextAsFloat(this XmlNode node, ref float value)
        {
            string text = string.Empty;
            if (node.HasInnerText(ref text))
            {
                if (float.TryParse(text, out value))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool CanGetInnerTextAsBool(this XmlNode node, ref bool value)
        {
            string text = string.Empty;
            if (node.HasInnerText(ref text))
            {
                if (bool.TryParse(text, out value))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool CanGetInnerTextAs<T>(this XmlNode node, ref T value, TryParse<T> tryParse)
        {
            string text = string.Empty;
            if (node.HasInnerText(ref text))
            {
                if (tryParse(text, ref value))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool HasAttribute(this XmlNode node, string attributeName, ref string value)
        {
            if (node == null) return false;
            if (node.Attributes == null) return false;
            XmlAttribute attribute = node.Attributes[attributeName];
            if (attribute == null) return false;
            value = attribute.Value;
            return true;
        }
        public static bool CanGetAttributeValueAsInteger(this XmlNode node, string attributeName, ref int value)
        {
            string text = string.Empty;
            if (node.HasAttribute(attributeName, ref text))
            {
                if (int.TryParse(text, out value))
                {
                    return true;
                }
            }
            return false;
        }
        public static bool CanGetAttributeValueAsFloat(this XmlNode node, string attributeName, ref float value)
        {
            string text = string.Empty;
            if (node.HasAttribute(attributeName, ref text))
            {
                if (float.TryParse(text, out value))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool CanGetAttributeValueAsBool(this XmlNode node, string attributeName, ref bool value)
        {
            string text = string.Empty;
            if (node.HasAttribute(attributeName, ref text))
            {
                if (bool.TryParse(text, out value))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool CanGetAttributeValueAs<T>(this XmlNode node, string attributeName, ref T value, TryParse<T> tryParse)
        {
            string text = string.Empty;
            if (node.HasAttribute(attributeName, ref text))
            {
                if (tryParse(text, ref value))
                {
                    return true;
                }
            }
            return false;
        }

        public static XmlNode GetXmlNodeByAttribute(this XmlNodeList list, string attributeName, string value)
        {
            if (list == null || list.Count <= 0) { return null; }

            for (int i = 0; i < list.Count; i++)
            {
                XmlAttributeCollection attributes = list[i].Attributes;
                if (attributes == null) { return null;}

                XmlAttribute tmp = attributes[attributeName];
                if (tmp == null) { return null;}

                if (string.Equals(tmp.Value, value)) { return list[i]; }
            }
            return null;
        }
    }
}
