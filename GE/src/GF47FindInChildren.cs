//************************************************************//
//      Author      :       GF47
//      DataTime    :       2014/2/12 星期三 20:10:37
//      Edited      :       2014/2/12 星期三 20:10:37
//************************************************************//

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GF47Editor
{
    /// <summary>
    /// Author          :GF47
    /// DataTime        :2014/2/12 星期三 20:10:37
    /// [GF47FindInChildren] Introduction  :Nothing to introduce
    /// </summary>
    public class GF47FindInChildren : EditorWindow
    {
        private List<GameObject> _targets = new List<GameObject>();
        private string _name;
        private bool _includeInActive;
        private enum Method
        {
            NameInclude,
            CompleteName,
            ComponentName
        }

        private Method _method = Method.CompleteName;

        [MenuItem("Tools/GF47 Editor/GF47FindInChildren")]
        static void Init()
        {
            GF47FindInChildren window = GetWindow<GF47FindInChildren>();
            window.position = new Rect(200.0f, 200.0f, 400.0f, 250.0f);
            window.minSize = new Vector2(300.0f, 25.0f);
            window.autoRepaintOnSceneChange = true;
            window.titleContent = new GUIContent("查找子物体");
            window.Show();
        }

        void OnGUI()
        {
            if (Selection.gameObjects.Length == 0)
            {
                EditorGUILayout.LabelField("You have selected nothing");
                return;
            }

            EditorGUILayout.BeginHorizontal();
            _method = (Method)EditorGUILayout.EnumPopup(_method, GUILayout.Width(110.0f));
            _name = EditorGUILayout.TextField(_name);
            EditorGUILayout.LabelField("包含不激活物体", GUILayout.Width(85.0f));
            _includeInActive = EditorGUILayout.Toggle(_includeInActive, GUILayout.Width(20.0f));
            if (GUILayout.Button("F", GUILayout.Width(20.0f)))
            {
                foreach (GameObject s in Selection.gameObjects)
                {
                    foreach (Component c in s.GetComponentsInChildren(typeof(Transform), _includeInActive))
                    {
                        switch (_method)
                        {
                            case Method.CompleteName:
                                if (c.name == _name)
                                {
                                    _targets.Add(c.gameObject);
                                }
                                break;
                            case Method.NameInclude:
                                if (c.name.Contains(_name))
                                {
                                    _targets.Add(c.gameObject);
                                }
                                break;
                            case Method.ComponentName:
                                if (c.gameObject.GetComponent(_name) != null)
                                {
                                    _targets.Add(c.gameObject);
                                }
                                break;
                        }
                    }
                }
                Debug.Log(_targets.Count);
                if (_targets.Count != 0)
                {
// ReSharper disable CoVariantArrayConversion
                    Selection.objects = _targets.ToArray();
// ReSharper restore CoVariantArrayConversion
                }
                _targets.Clear();
            }

            EditorGUILayout.EndHorizontal();
        }
    }
}
