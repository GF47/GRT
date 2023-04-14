using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

namespace GRT.Editor
{
    public class GF47SelectFromFile : EditorWindow
    {
        private static GUILayoutOption _height = GUILayout.Height(32f);
        private string _pattern = @"(?<=entity\s+path\s*=\s*"")((.+):)?(.+?)(?="")";
        private int _sceneGroup = 2;
        private int _pathGroup = 3;
        private string _text;

        private IEnumerator<GameObject> _iterator;

        [MenuItem("Tools/GF47 Editor/Select From File")]
        private static void Init()
        {
            var window = GetWindow<GF47SelectFromFile>();
            window.position = new Rect(300f, 300f, 320f, 200f);
            window.minSize = new Vector2(320f, 200f);
            window.Show();
        }

        private void OnGUI()
        {
            _pattern = EditorGUILayout.DelayedTextField("Pattern", _pattern);
            _sceneGroup = EditorGUILayout.DelayedIntField("Scene Group", _sceneGroup);
            _pathGroup = EditorGUILayout.DelayedIntField("Path Group", _pathGroup);

            var loaded = !string.IsNullOrEmpty(_text);

            var defaultColor = GUI.color;
            if (loaded) { GUI.color = Color.green; }
            else { GUI.color = Color.yellow; }
            if (GUILayout.Button(loaded ? "File Loaded" : "Load File", _height))
            {
                var path = EditorUtility.OpenFilePanel("Load File", Application.dataPath, "*");
                Debug.Log(path);
                if (File.Exists(path))
                {
                    _text = Encoding.UTF8.GetString(File.ReadAllBytes(path));
                }
            }

            GUI.color = defaultColor;

            if (loaded)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("Log Info", _height))
                    {
                        if (string.IsNullOrEmpty(_pattern))
                        {
                            Debug.Log(_text.Substring(0, 10000));
                        }
                        else
                        {
                            var sb = new StringBuilder();
                            foreach (Match match in Regex.Matches(_text, _pattern))
                            {
                                var scene = _sceneGroup > -1 ? match.Groups[_sceneGroup].Value : match.Value;
                                var path = _pathGroup > -1 ? match.Groups[_pathGroup].Value : match.Value;
                                sb.AppendFormat("scene:\t{0}\tobject:\t{1}\n", scene, path);

                                if (sb.Length >= 10000)
                                {
                                    sb.Append("...\netc.");
                                    break;
                                }
                            }

                            Debug.Log(sb);
                        }
                    }

                    if (GUILayout.Button("All", _height))
                    {
                        var selected = FindGameObject();

                        Selection.objects = selected.ToArray();
                    }

                    if (GUILayout.Button("Next", _height))
                    {
                        if (_iterator == null)
                        {
                            _iterator = FindGameObject().GetEnumerator();
                        }

                        if (_iterator.MoveNext())
                        {
                            Selection.activeGameObject = _iterator.Current;
                        }
                        else
                        {
                            _iterator.Dispose();
                            _iterator = null;

                            Debug.Log("Over");
                        }
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        private void OnDestroy()
        {
            _iterator?.Dispose();
        }

        private List<GameObject> FindGameObject()
        {
            var selected = new List<GameObject>();

            if (string.IsNullOrEmpty(_pattern))
            {
                var paths = _text.Split('\n');
                foreach (var path in paths)
                {
                    var go = GameObject.Find(path);
                    if (go != null)
                    {
                        selected.Add(go);
                    }
                }
            }
            else
            {
                foreach (Match match in Regex.Matches(_text, _pattern))
                {
                    var scene = _sceneGroup > -1 ? match.Groups[_sceneGroup].Value : match.Value;
                    if (scene == SceneManager.GetActiveScene().name)
                    {
                        var path = _pathGroup > -1 ? match.Groups[_pathGroup].Value : match.Value;
                        var go = GameObject.Find(path);
                        if (go != null)
                        {
                            selected.Add(go);
                        }
                    }
                }
            }

            return selected;
        }
    }
}