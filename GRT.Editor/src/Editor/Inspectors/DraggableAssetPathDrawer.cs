using UnityEditor;
using UnityEngine;

namespace GRT.Editor.Inspectors
{
    [CustomPropertyDrawer(typeof(DraggableAssetPathAttribute))]
    public class DraggableAssetPathDrawer : PropertyDrawer
    {
        GUIContent _customLabel;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.String)
            {
                EditorGUI.BeginProperty(position, label, property);
                var e = Event.current;
                if (position.Contains(e.mousePosition) && (e.type & EventType.DragUpdated) > 0)
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Link;
                    var paths = DragAndDrop.paths;
                    if (paths != null && paths.Length > 0)
                    {
                        var path = paths[0];
                        if (property.stringValue != path)
                        {
                            property.stringValue = path;
                            property.serializedObject.ApplyModifiedProperties();
                        }
                    }
                    DragAndDrop.AcceptDrag();
                }

                if (_customLabel == null)
                {
                    var customName = (attribute as DraggableAssetPathAttribute).Name;
                    _customLabel = string.IsNullOrEmpty(customName) ? label : new GUIContent(customName);
                }
                EditorGUI.PropertyField(position, property, _customLabel, true);

                EditorGUI.EndProperty();
            }
            else
            {
                EditorGUI.LabelField(position, $"{property.name} must be a string");
            }
        }
    }
}