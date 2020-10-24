/***************************************************************
 * @File Name       : InheritFromAttributeDrawer
 * @Author          : GF47
 * @Description     : 指定继承关系属性的编辑器显示效果
 * @Date            : 2017/6/8/星期四 15:04:04
 * @Edit            : none
 **************************************************************/

using GRT;
using UnityEditor;
using UnityEngine;

namespace GF47Editor.Inspectors
{
    [CustomPropertyDrawer(typeof(InheritFromAttribute))]
    public class InheritFromAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            InheritFromAttribute inheritFrom = attribute as InheritFromAttribute;
            if (inheritFrom != null)
            {
                Object o = EditorGUI.ObjectField(position, label, property.objectReferenceValue, typeof(Component), true);

                if (o == null)
                {
                    Undo.RecordObject(property.serializedObject.targetObject, "change " + label);
                    property.objectReferenceValue = null;
                    EditorUtility.SetDirty(property.serializedObject.targetObject);
                    return;
                }
                if (inheritFrom.baseType.IsInstanceOfType(o))
                {
                    if (!ReferenceEquals(o, property.objectReferenceValue))
                    {
                        Undo.RecordObject(property.serializedObject.targetObject, "change " + label);
                        property.objectReferenceValue = o;
                        EditorUtility.SetDirty(property.serializedObject.targetObject);
                    }
                    return;
                }

                Component co = o as Component;
                if (co != null)
                {
                    Component[] behaviours = co.GetComponents<Component>();
                    if (behaviours != null)
                    {
                        for (int i = 0; i < behaviours.Length; i++)
                        {
                            if (inheritFrom.baseType.IsInstanceOfType(behaviours[i]))
                            {
                                if (!ReferenceEquals(behaviours[i], property.objectReferenceValue))
                                {
                                    Undo.RecordObject(property.serializedObject.targetObject, "change " + label);
                                    property.objectReferenceValue = behaviours[i];
                                    EditorUtility.SetDirty(property.serializedObject.targetObject);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
