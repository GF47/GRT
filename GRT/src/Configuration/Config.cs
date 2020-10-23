using System.Collections.Generic;
using System.Xml;
using GRT.Data;
using UnityEngine;

namespace GRT.Configuration
{
    public class Config
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

        public T Get<T>(string name, T defaultValue)
        {
            if (_config == null)
            {
                Debug.LogWarningFormat("{0} is not found, use default value {1}", name, defaultValue);
                return defaultValue;
            }
            if (_config.ContainsKey(name))
            {
                return (T)_config[name];
            }
            return defaultValue;
        }

        public T Get<T>(string name)
        {
            return Get(name, default(T));
        }
    }
}
