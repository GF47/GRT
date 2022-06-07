using System.Collections.Generic;
using UnityEngine;

namespace GRT.Serialization
{
    public interface ISerializer<Node>
    {
        string Serialize(Node node);

        Node Deserialize(string src);

        Node GetChild(Node parent);

        IList<Node> GetChildren(Node parent);

        bool HasAttribute(Node node);

        bool GetAttribute(Node node, out string value);

        bool GetAttributeBool(Node node, out bool value);

        bool GetAttributeInt(Node node, out int value);

        bool GetAttributeFloat(Node node, out float value);

        bool GetAttributeVector2(Node node, out Vector2 value);

        bool GetAttributeVector3(Node node, out Vector3 value);

        bool GetAttributeColor(Node node, out Color value);

        bool GetAttributeHtmlColor(Node node, out Color value);
    }
}