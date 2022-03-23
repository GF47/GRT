using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Text.RegularExpressions;

namespace GRT.Editor
{
    public class GF47Empty2Prefab : EditorWindow
    {
        private string _targetName;
        private GameObject _prefab;

        private bool _renamePrefab;
        private bool _deleteChildren;

        [MenuItem("Tools/GF47 Editor/Empty to Prefab")]
        private static void Init()
        {
            var window = GetWindow<GF47Empty2Prefab>();
            window.position = new Rect(200f, 200f, 400f, 400f);
            window.Show();
        }

        private void OnGUI()
        {
            _targetName = EditorGUILayout.TextField("被替换物体名称", _targetName);
            _prefab = EditorGUILayout.ObjectField("替换为", _prefab, typeof(GameObject), true) as GameObject;

            var root = Selection.activeTransform == null ? EditorSceneManager.GetActiveScene().name : Selection.activeTransform.name;
            EditorGUILayout.LabelField($"查找根节点 ==> {root} <==");

            _renamePrefab = EditorGUILayout.Toggle("修改 Prefab 名称", _renamePrefab);
            _deleteChildren = EditorGUILayout.Toggle("删除原有子节点", _deleteChildren);

            var defaultColor = GUI.color;
            GUI.color = Color.red;
            if (GUILayout.Button("开始替换 DANGER!!!"))
            {
                Replace();
            }
            GUI.color = defaultColor;
        }

        private void Replace()
        {
            var regex = new Regex(_targetName);

            var roots = Selection.transforms;
            if (roots != null && roots.Length > 0)
            {
                foreach (var root in roots)
                {
                    if (root==null) { continue; }
                    var list = root.GetComponentsInChildren<Transform>();

                    foreach (var t in list)
                    {
                        if (t == null) { continue; }
                        if (regex.IsMatch(t.name))
                        {
                            if (_deleteChildren)
                            {
                                var children = new List<Transform>(t.childCount);
                                for (int i = 0; i < t.childCount; i++)
                                {
                                    children.Add(t.GetChild(i));
                                }
                                foreach (var child in children)
                                {
                                    Undo.DestroyObjectImmediate(child.gameObject);
                                }
                            }

                            var instance = GameObject.Instantiate(_prefab, t.position, t.rotation, t);
                            instance.transform.localScale = Vector3.one;
                            if (_renamePrefab) { instance.name = t.name; }
                            Undo.RegisterCreatedObjectUndo(instance, "replace empty node by prefab");
                        }
                    }
                    EditorUtility.SetDirty(root);
                }
            }
            else
            {
                foreach (var sceneRoot in EditorSceneManager.GetActiveScene().GetRootGameObjects())
                {
                    if (sceneRoot == null) { continue; }
                    var list = sceneRoot.GetComponentsInChildren<Transform>();

                    foreach (var t in list)
                    {
                        if (t == null) { continue; }
                        if (regex.IsMatch(t.name))
                        {
                            if (_deleteChildren)
                            {
                                var children = new List<Transform>(t.childCount);
                                for (int i = 0; i < t.childCount; i++)
                                {
                                    children.Add(t.GetChild(i));
                                }
                                foreach (var child in children)
                                {
                                    Undo.DestroyObjectImmediate(child.transform);
                                }
                            }

                            var instance = GameObject.Instantiate(_prefab, t.position, t.rotation, t);
                            instance.transform.localScale = Vector3.one;
                            if (_renamePrefab) { instance.name = t.name; }
                            Undo.RegisterCreatedObjectUndo(instance, "replace empty node by prefab");
                        }
                    }
                }
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }
        }
    }
}
