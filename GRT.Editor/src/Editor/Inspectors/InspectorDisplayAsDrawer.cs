/***************************************************************
 *@File Name     : InspectorDisplayAsDrawer.cs
 *@Author        : GF47
 *@Description   : 字段自定义 Inspector 显示
 *@Data          : 2020-11-10 13:54
 *@Edit          : none
 **************************************************************/

namespace GRT.Editor.Inspectors
{
    using GRT;
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(InspectorDisplayAsAttribute))]
    public class InspectorDisplayAsDrawer : PropertyDrawer
    {
        private InspectorDisplayAsAttribute _realAttribute;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (_realAttribute == null)
            {
                _realAttribute = attribute as InspectorDisplayAsAttribute;
            }
            label.text = _realAttribute.Name;
            label.tooltip = _realAttribute.Tips;
            EditorGUI.PropertyField(position, property, label);
        }
    }
}