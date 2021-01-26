using GRT.Data;
using System.Collections.Generic;
using System.Xml;

namespace GRT.Configuration
{
    public class Config : IBlackBoard
    {
        private Dictionary<string, object> _config;

        public void Initialize_XmlString(string xmlString)
        {
            _config = new Dictionary<string, object>();

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlString);
            XmlNode root = doc.LastChild;

            Initialize(root);
        }
        public void Initialize_XmlFileName(string configPath)
        {
            _config = new Dictionary<string, object>();

            XmlDocument doc = new XmlDocument();
            doc.Load(configPath);
            XmlNode root = doc.LastChild;

            Initialize(root);
        }

        public void Initialize(XmlNode root)
        {
            if (root != null)
            {
                XmlNodeList list = root.SelectNodes(ConstValues.NODE);
                if (list != null)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        string name = string.Empty;
                        if (!list[i].HasAttribute(ConstValues.NAME, ref name)) { continue; }

                        string type = string.Empty;
                        if (!list[i].HasAttribute(ConstValues.TYPE, ref type)) { continue; }

                        string valueStr = string.Empty;
                        if (!list[i].HasAttribute(ConstValues.VALUE, ref valueStr)) { continue; }
                        object value = Convert.ConvertTo(type, valueStr);

                        _config.Add(name, value);
                    }
                }
            }
        }

        public T Get<T>(string name, T @default = default)
        {
            var result = _config != null && _config.ContainsKey(name);
            return result ? (T)_config[name] : @default;
        }

        public bool Get<T>(string name, out T value, T @default = default)
        {
            var result = _config != null && _config.ContainsKey(name);
            value = result ? (T)_config[name] : @default;
            return result;
        }

        void IBlackBoard.Set<T>(string name, T value)
        {
            throw new System.NotImplementedException();
        }
    }
}
