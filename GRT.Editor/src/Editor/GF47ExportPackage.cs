using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using UObject = UnityEngine.Object;

namespace GRT.Editor
{
    public class GF47ExportPackage : EditorWindow
    {
        [MenuItem("Tools/GF47 Editor/Export Package")]
        private static void Init()
        {
            var window = GetWindow<GF47ExportPackage>();
            window.position = new Rect(200f, 200f, 400f, 400f);
            window.Show();
        }

        [SerializeField]
        private List<UObject> _objects;

        private string _listFilePath;

        private string _packageName;

        private SerializedObject _serializedObject;
        private SerializedProperty _objectsProperty;

        private void OnEnable()
        {
            _objects = new List<UObject>();
            _serializedObject = new SerializedObject(this);
            _objectsProperty = _serializedObject.FindProperty("_objects");
        }

        private void OnGUI()
        {
            _serializedObject.Update();

            EditorGUI.BeginChangeCheck();
            _packageName = EditorGUILayout.TextField("Package Name", _packageName);
            EditorGUILayout.BeginHorizontal();
            {
                _listFilePath = EditorGUILayout.TextField("List File", _listFilePath);
                if (EditorGUI.EndChangeCheck())
                {
                    _serializedObject.ApplyModifiedProperties();
                }

                if (GUILayout.Button("Read", GUILayout.Width(50f)))
                {
                    _listFilePath = EditorUtility.OpenFilePanel("Read List File", Application.dataPath, "txt,csv");
                    if (_listFilePath != null)
                    {
                        ReadAssetsListFile(_listFilePath);
                    }
                }

                if (GUILayout.Button("Write", GUILayout.Width(50f)))
                {
                    _listFilePath = EditorUtility.SaveFilePanel("Write List File", Application.dataPath, "assets list", "txt,csv");
                    if (_listFilePath != null)
                    {
                        WriteAssetsListFile(_listFilePath);
                    }
                }
            }
            EditorGUILayout.EndHorizontal();

            var color = GUI.color;
            GUI.color = Color.red;
            if (GUILayout.Button("Export", GUILayout.Height(64)))
            {
                Export();
            }
            GUI.color = color;

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(_objectsProperty, true);
            if (EditorGUI.EndChangeCheck())
            {
                _serializedObject.ApplyModifiedProperties();
            }
        }

        private void ReadAssetsListFile(string path)
        {
            EditorGUI.BeginChangeCheck();

            if (File.Exists(path))
            {
                foreach (var assetPath in File.ReadLines(path))
                {
                    UObject obj = AssetDatabase.LoadAssetAtPath(assetPath, typeof(UObject));
                    if (obj != null && !_objects.Contains(obj))
                    {
                        _objects.Add(obj);
                    }
                }
            }

            if (EditorGUI.EndChangeCheck())
            {
                _serializedObject.ApplyModifiedProperties();
            }
        }

        private void WriteAssetsListFile(string path)
        {
            try
            {
                using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    var sb = new StringBuilder();
                    foreach (var asset in _objects)
                    {
                        var assetPath = AssetDatabase.GetAssetPath(asset);
                        sb.AppendLine(assetPath);
                    }

                    var data = Encoding.UTF8.GetBytes(sb.ToString());
                    fs.Write(data, 0, data.Length);
                }

                AssetDatabase.Refresh();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private void Export()
        {
            var list = new List<string>();
            foreach (var asset in _objects)
            {
                list.Add(AssetDatabase.GetAssetPath(asset));
            }

            AssetDatabase.ExportPackage(list.ToArray(),
                string.IsNullOrEmpty(_packageName) ? "Exported Package.unitypackage" : $"{_packageName}.unitypackage",
                ExportPackageOptions.Interactive | ExportPackageOptions.Recurse);
        }
    }
}