using GRT.Data;
using System.Xml;

namespace GRT.Configuration
{
    public class XmlConfig : BlackBoard
    {
        public void Initialize_XmlString(string xmlString)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlString);
            XmlNode root = doc.LastChild;

            Initialize(root);
        }

        public void Initialize_XmlFileName(string configPath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(configPath);
            XmlNode root = doc.LastChild;

            Initialize(root);
        }

        public void Initialize(XmlNode root)
        {
            items.Clear();

            if (root != null)
            {
                XmlNodeList list = root.SelectNodes(ConstValues.NODE);
                if (list != null)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (!list[i].HasAttribute(ConstValues.NAME, out string name)) { continue; }
                        if (!list[i].HasAttribute(ConstValues.TYPE, out string type)) { continue; }
                        if (!list[i].HasAttribute(ConstValues.VALUE, out string valueStr)) { continue; }

                        items.Add(name, Convert.ConvertTo(type, valueStr));
                    }
                }
            }
        }
    }
}