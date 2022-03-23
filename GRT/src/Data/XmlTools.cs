using System.Xml;

namespace GRT.Data
{
    public static class XmlTools
    {
        public delegate bool TryParse<T>(string text, out T value);

        public static XmlNode GetRootNode(string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            return doc.LastChild;
        }

        public static bool HasInnerText(this XmlNode node, out string text)
        {
            if (node == null) { text = null; return false; }

            if (string.IsNullOrEmpty(node.InnerText)) { text = null; return false; }

            text = node.InnerText; return true;
        }

        public static bool HasInnerTextAsInteger(this XmlNode node, out int value)
        {
            if (node.HasInnerText(out string text))
            {
                return int.TryParse(text, out value);
            }

            value = 0; return false;
        }

        public static bool HasInnerTextAsFloat(this XmlNode node, out float value)
        {
            if (node.HasInnerText(out string text))
            {
                return float.TryParse(text, out value);
            }

            value = 0f; return false;
        }

        public static bool HasInnerTextAsBoolean(this XmlNode node, out bool value)
        {
            if (node.HasInnerText(out string text))
            {
                return bool.TryParse(text, out value);
            }

            value = false; return false;
        }

        public static bool HasInnerTextAs<T>(this XmlNode node, out T value, TryParse<T> tryParse, T @default = default)
        {
            if (node.HasInnerText(out string text))
            {
                return tryParse(text, out value);
            }

            value = @default; return false;
        }

        public static bool HasAttribute(this XmlNode node, string attributeName, out string value)
        {
            if (node == null) { value = null; return false; }

            var attribute = node.SelectSingleNode($"@{attributeName}");
            if (attribute == null) { value = null; return false; }

            value = attribute.Value; return true;
        }

        public static bool HasAttributeAsInteger(this XmlNode node, string attributeName, out int value)
        {
            if (node.HasAttribute(attributeName, out string text))
            {
                return int.TryParse(text, out value);
            }

            value = 0; return false;
        }

        public static bool HasAttributeAsFloat(this XmlNode node, string attributeName, out float value)
        {
            if (node.HasAttribute(attributeName, out string text))
            {
                return float.TryParse(text, out value);
            }

            value = 0f; return false;
        }

        public static bool HasAttributeAsBoolean(this XmlNode node, string attributeName, out bool value)
        {
            if (node.HasAttribute(attributeName, out string text))
            {
                value = Convert.ToBool(text);
                return true;
            }

            value = false; return false;
        }

        public static bool HasAttributeAs<T>(this XmlNode node, string attributeName, out T value, TryParse<T> tryParse, T @default = default)
        {
            if (node.HasAttribute(attributeName, out string text))
            {
                return (tryParse(text, out value));
            }

            value = @default; return false;
        }

        public static XmlNode GetXmlNodeByAttribute(this XmlNodeList list, string attributeName, string value)
        {
            if (list == null || list.Count < 1) { return null; }

            for (int i = 0; i < list.Count; i++)
            {
                var attribute = list[i].SelectSingleNode($"{attributeName}");

                if (attribute == null) { continue; }

                if (string.Equals(attribute.Value, value)) { return list[i]; }
            }
            return null;
        }
    }
}