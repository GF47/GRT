using GRT.Data;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GRT.Editor.GXSerialization
{
    public class Actor_GXFactory : IGXSerializableFactory<XmlNode, Actor>
    {
        #region const

        private const string CLASS_Actor_NODE_NAME = "actor";

        private const string FIELD__name_ATTRIBUTE_NAME = "name";
        private const string FIELD__name_ATTRIBUTE_DEFAULT = "GF47";
        private const int FIELD__name_ATTRIBUTE_DECIMAL = 2;

        private const string FIELD__age_ATTRIBUTE_NAME = "age";
        private const string FIELD__age_ATTRIBUTE_DEFAULT = "3";
        private const int FIELD__age_ATTRIBUTE_DECIMAL = 2;

        private const string FIELD__sex_ATTRIBUTE_NAME = "sex";
        private const string FIELD__sex_ATTRIBUTE_DEFAULT = "false";
        private const int FIELD__sex_ATTRIBUTE_DECIMAL = 2;

        private const string PROP_Mood_NODE_NAME = "mood";
        private const string PROP_Mood_NODE_DEFAULT = "happy";
        private const int PROP_Mood_NODE_DECIMAL = 2;

        private const string PROP_Magic_NODE_NAME = "magic";
        private const string PROP_Magic_NODE_DEFAULT = "100.00";
        private const int PROP_Magic_NODE_DECIMAL = 1;

        private const string PROP_Spouse_REF_NODE_NAME = "spouse";

        private const string PROP_Friends_ARRAY_CONTAINER = "friends";
        private const string PROP_Friends_REF_NODE_NAME = "friend";

        #endregion

        public XmlNode Serialize(IGXML<XmlNode> xml, object @object, XmlNode parentNode, IGXAttribute refAttribute = default) =>
            Serialize(xml, @object as Actor, parentNode, refAttribute);

        public object Deserialize(IGXML<XmlNode> xml, XmlNode data, IGXAttribute refAttribute = default, Func<object> constructor = default) =>
            DeserializeExplicitly(xml, data, refAttribute, constructor);

        public XmlNode Serialize(IGXML<XmlNode> xml, Actor @object, XmlNode parentNode, IGXAttribute refAttribute = default)
        {
            var node = xml.CreateChild(parentNode, CLASS_Actor_NODE_NAME);
            xml.SetAttribute(node, FIELD__name_ATTRIBUTE_NAME, @object.Name);
            xml.SetAttributeInteger(node, FIELD__age_ATTRIBUTE_NAME, @object.Age);
            xml.SetAttributeBoolean(node, FIELD__sex_ATTRIBUTE_NAME, @object.Sex);
            var node_Mood = xml.CreateChild(node, PROP_Mood_NODE_NAME);
            xml.SetInnerString(node_Mood, @object.Mood);
            var node_Magic = xml.CreateChild(node, PROP_Magic_NODE_NAME);
            xml.SetInnerFloat(node_Magic, @object.Magic);
            var node_Spouse = GXMLSerializer<XmlNode>.Instance.Write(@object.Spouse, node, new GXAttributeMock()
            {
                Name = PROP_Spouse_REF_NODE_NAME,
            });
            var containerNode_Friends = xml.CreateChild(node, PROP_Friends_ARRAY_CONTAINER);
            var refAttribute_Friends = new GXAttributeMock()
            {
                Name = PROP_Friends_REF_NODE_NAME,
            };
            foreach (var item in @object.Friends)
            {
                GXMLSerializer<XmlNode>.Instance.Write(item, containerNode_Friends, refAttribute_Friends);
            }
            return node;
        }

        public Actor DeserializeExplicitly(IGXML<XmlNode> xml, XmlNode data, IGXAttribute refAttribute = default, Func<object> constructor = default)
        {
            throw new NotImplementedException();
        }
    }
}
