/* ****************************************************************
 * @File Name   :   EnumFlagsAttributes
 * @Author      :   GF47
 * @Date        :   2015/7/23 14:52:05
 * @Description :   枚举的显示面板效果
 * @Edit        :   2015/7/23 14:52:05
 * ***************************************************************/

using UnityEditor;
using UnityEngine;

namespace GRT.Editor.Inspectors
{
    [CustomPropertyDrawer(typeof(GRT.EnumFlagsAttribute))]
    public class EnumFlagsAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginChangeCheck();
            int value = EditorGUI.MaskField(position, label, property.intValue, property.enumNames);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(property.serializedObject.targetObject, "change " + label.text);
                property.intValue = value;
                EditorUtility.SetDirty(property.serializedObject.targetObject);
            }
        }
    }
}