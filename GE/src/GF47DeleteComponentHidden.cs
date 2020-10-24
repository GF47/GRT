//************************************************************//
//      Author      :       GF47
//      DataTime    :       2013/11/26 星期二 10:30:12
//      Edited      :       2013/11/26 星期二 10:30:12
//************************************************************//

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GF47Editor
{
    /// <summary>
    /// Author          :GF47
    /// DataTime        :2013/11/26 星期二 10:30:12
    /// [GF47DeleteComponentHidden] Introduction : 清除某个物体上的指定组件
    /// </summary>
    public class GF47DeleteComponentHidden : EditorWindow
    {
        private List<GameObject> _targets = new List<GameObject>();
        private string _componentName;
        private int _count;

        [MenuItem("Tools/GF47 Editor/Delete Component Hidden")]
        private static void Init()
        {
            GF47DeleteComponentHidden window = GetWindow<GF47DeleteComponentHidden>();
            window.position = new Rect(200.0f, 200.0f, 400.0f, 400.0f);
            window.Show();
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("目标:");
            foreach (GameObject target in _targets)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.ObjectField(target, typeof(GameObject), true);
                Component component = target.GetComponent(_componentName);
                EditorGUILayout.LabelField(component != null ? "yes" : "no");
                EditorGUILayout.EndHorizontal();
            }
            _componentName = EditorGUILayout.TextField("组件名称",_componentName);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(new GUIContent("添加选中物体")))
            {
                _targets.AddRange(Selection.gameObjects);
            }

            if (GUILayout.Button(new GUIContent("清空")))
            {
                _targets.Clear();
            }
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button(new GUIContent("删除" + _componentName + "组件", "点击删除隐藏的组件")))
            {
                DeleteComponents();
                Debug.Log(string.Format("删除了{0}个{1}组件", _count, _componentName));
            }
        }

        private void DeleteComponents()
        {
            _count = 0;
            foreach (GameObject t in _targets)
            {
                if (t == null)
                {
                    break;
                }
                Component component = t.GetComponent(_componentName);
                if (component != null)
                {
                    _count++;
                    DestroyImmediate(component);
                }
            }
        }
    }
}
