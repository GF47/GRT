using UnityEditor;
using UnityEngine;

namespace GRT.Editor.Inspectors
{
    [CustomPropertyDrawer(typeof(DraggableAssetPathAttribute))]
    public class DraggableAssetPathDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.String)
            {
                EditorGUI.BeginProperty(position, label, property);
                var e = Event.current;
                if (position.Contains(e.mousePosition))
                {
                    // if ((e.type & EventType.DragUpdated) > 0)
                    // {
                    //     DragAndDrop.visualMode = DragAndDropVisualMode.Link;
                    //     var paths = DragAndDrop.paths;
                    //     if (paths != null && paths.Length > 0)
                    //     {
                    //         var path = paths[0];
                    //         if (property.stringValue != path)
                    //         {
                    //             property.stringValue = path;
                    //             property.serializedObject.ApplyModifiedProperties();
                    //         }
                    //     }
                    //     DragAndDrop.AcceptDrag();
                    // }
                    // else
                    if ((e.type & EventType.DragPerform) > 0)
                    {
                        DragAndDrop.visualMode = DragAndDropVisualMode.Link;
                        var objects = DragAndDrop.objectReferences;
                        if (objects != null && objects.Length > 0)
                        {
                            var obj = objects[0];
                            if (EditorUtility.IsPersistent(obj))
                            {
                                property.stringValue = AssetDatabase.GetAssetPath(obj);
                            }
                            else if (obj is GameObject go)
                            {
                                property.stringValue = GameObjectExtension.GetPath(go, true);
                            }
                            property.serializedObject.ApplyModifiedProperties();
                        }
                        DragAndDrop.AcceptDrag();
                    }
                }

                var draggable = attribute as DraggableAssetPathAttribute;
                label.text = draggable.Name;
                label.tooltip = draggable.Tips;

                EditorGUI.PropertyField(position, property, label, true);

                EditorGUI.EndProperty();
            }
            else
            {
                EditorGUI.LabelField(position, $"{property.name} must be a string");
            }
        }
    }
}