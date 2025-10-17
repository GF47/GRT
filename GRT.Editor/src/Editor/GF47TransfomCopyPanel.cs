using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace GRT.Editor
{
    public class GF47TransformCopyPanel : EditorWindow
    {
        private Component _component;
        private FieldInfo[] _fields;

        private static Vector3 _value;
        private static Vector3 _position;
        private static Quaternion _rotation;
        private static Vector3 _scale;

        [MenuItem("Tools/GF47 Editor/Transform Copy/Copy Local &%c")]
        private static void TransformRecord()
        {
            _position = Selection.activeTransform.localPosition;
            _rotation = Selection.activeTransform.localRotation;
            _scale = Selection.activeTransform.localScale;
            string str = string.Format("position={0}, eulerAngles={1}, scale={2}", _position.ToString("F4"), _rotation.eulerAngles.ToString("F4"), _scale.ToString("F4"));
            Debug.Log(str);
            EditorGUIUtility.systemCopyBuffer = str;
        }

        [MenuItem("Tools/GF47 Editor/Transform Copy/Paste &%v")]
        private static void TransformApply()
        {
            Selection.activeTransform.localPosition = _position;
            Selection.activeTransform.localRotation = _rotation;
            Selection.activeTransform.localScale = _scale;
        }

        [MenuItem("Tools/GF47 Editor/Transform Copy/Copy World")]
        private static void WorldTransformRecord()
        {
            _position = Selection.activeTransform.position;
            _rotation = Selection.activeTransform.rotation;
            _scale = Selection.activeTransform.lossyScale;
            string str = string.Format("position={0}, eulerAngles={1}, scale={2}", _position.ToString("F4"), _rotation.eulerAngles.ToString("F4"), _scale.ToString("F4"));
            Debug.Log(str);
            EditorGUIUtility.systemCopyBuffer = str;
        }

        [MenuItem("Tools/GF47 Editor/Transform Copy/Copy View Camera")]
        private static void GetViewCamera()
        {
            var camera = SceneView.lastActiveSceneView.camera.transform;
            _position = camera.position;
            _rotation = camera.rotation;
            _scale = camera.lossyScale;
            string str = string.Format("position={0}, eulerAngles={1}, scale={2}", _position.ToString("F4"), _rotation.eulerAngles.ToString("F4"), _scale.ToString("F4"));
            Debug.Log(str);
            EditorGUIUtility.systemCopyBuffer = str;
        }

        [MenuItem("Tools/GF47 Editor/Transform Copy/Copy View Target")]
        private static void GetViewCameraTarget()
        {
            _position = SceneView.lastActiveSceneView.pivot;
            string str = string.Format("target={0}", _position.ToString("F4"));
            Debug.Log(str);
            EditorGUIUtility.systemCopyBuffer = str;
        }

        [MenuItem("Tools/GF47 Editor/Transform Copy/Copy View Camera And Target")]
        private static void GetView()
        {
            var camerar = SceneView.lastActiveSceneView.camera.transform;
            _position = camerar.position;
            _rotation = camerar.rotation;
            _scale = camerar.lossyScale;
            var target = SceneView.lastActiveSceneView.pivot;
            var str = $"camera=\"{_position.x:F4},{_position.y:F4},{_position.z:F4}\" target=\"{target.x:F4},{target.y:F4},{target.z:F4}\"";
            Debug.Log(str);
            EditorGUIUtility.systemCopyBuffer = str;
        }

        [MenuItem("Tools/GF47 Editor/Transform Copy/Panel")]
        private static void Init()
        {
            var w = GetWindow<GF47TransformCopyPanel>();
            w.position = new Rect(200f, 200f, 400f, 400f);
            w.Show();
        }

        private void OnGUI()
        {
            var component = EditorGUILayout.ObjectField("Component", _component, typeof(Component), true) as Component;
            if (_component != component)
            {
                _component = component;

                if (_component != null)
                {
                    _fields = Array.FindAll(_component.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance),
                        f => f.FieldType == typeof(Vector3) && (f.IsPublic || f.GetCustomAttribute<SerializeField>() != null));
                }
                else
                {
                    _fields = null;
                }
            }

            if (_fields != null)
            {
                foreach (var f in _fields)
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.BeginVertical(EditorStyles.textArea);
                    {
                        EditorGUILayout.BeginHorizontal();
                        {
                            GUILayout.Label(f.Name, EditorStyles.boldLabel);

                            if (GUILayout.Button("Copy", EditorStyles.miniButtonLeft))
                            {
                                _value = (Vector3)f.GetValue(_component);
                            }

                            if (GUILayout.Button("Paste", EditorStyles.miniButtonRight))
                            {
                                Undo.RecordObject(_component, $"Set Value {f.Name}");
                                f.SetValue(_component, _value);
                                EditorUtility.SetDirty(_component);
                            }
                        }
                        EditorGUILayout.EndHorizontal();

                        Color bg = GUI.color;
                        GUI.color = Color.yellow;
                        GUILayout.Label("Copy From Clipboard", EditorStyles.miniLabel);
                        GUI.color = bg;

                        EditorGUILayout.BeginHorizontal();
                        {
                            if (GUILayout.Button("Position", EditorStyles.miniButtonLeft))
                            {
                                Undo.RecordObject(_component, $"Set Value {f.Name}");
                                f.SetValue(_component, _position);
                                EditorUtility.SetDirty(_component);
                            }

                            if (GUILayout.Button("Rotation", EditorStyles.miniButtonMid))
                            {
                                Undo.RecordObject(_component, $"Set Value {f.Name}");
                                f.SetValue(_component, _rotation.eulerAngles);
                                EditorUtility.SetDirty(_component);
                            }

                            if (GUILayout.Button("Scale", EditorStyles.miniButtonRight))
                            {
                                Undo.RecordObject(_component, $"Set Value {f.Name}");
                                f.SetValue(_component, _scale);
                                EditorUtility.SetDirty(_component);
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    EditorGUILayout.EndVertical();
                }
            }
        }
    }
}