//************************************************************//
//      Author      :       GF47
//      DataTime    :       2014/2/14 星期五 13:36:32
//      Edited      :       2014/2/14 星期五 13:36:32
//************************************************************//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace GF47Editor
{
    /// <summary>
    /// Author          :GF47
    /// DataTime        :2014/2/14 星期五 13:36:32
    /// [GF47SortArray] Introduction  :重新排列 Unity3D Inspector 面板中可视的具有IList接口的类型，例如数组，List
    /// </summary>
    public class GF47SortArray : EditorWindow
    {
        private MonoBehaviour _component;
        private int _index;
        private bool _isPublic;

        [MenuItem("Tools/GF47 Editor/Sort Array")]
        static void Init()
        {
            GF47SortArray window = GetWindow<GF47SortArray>();
            window.position = new Rect(200.0f, 200.0f, 400.0f, 400.0f);
            window._isPublic = true;
            window.Show();
        }

        void OnGUI()
        {
            _component = (MonoBehaviour)EditorGUILayout.ObjectField("组件", _component, typeof(MonoBehaviour), true);
            _isPublic = EditorGUILayout.Toggle("Public ?", _isPublic);
            if (_component != null)
            {
                List<FieldInfo> infos = new List<FieldInfo>();
                BindingFlags flags = BindingFlags.Instance | BindingFlags.Public;
                if (!_isPublic)
                {
                    flags |= BindingFlags.NonPublic;
                }
                FieldInfo[] reflectedFields = _component.GetType().GetFields(flags);
                for (int i = 0; i < reflectedFields.Length; i++)
                {
                    FieldInfo info = reflectedFields[i];
                    if (InheritedFromIEnumerable(info.FieldType))
                    {
                        infos.Add(info);
                    }
                }

                string[] infoNames = new string[infos.Count];
                for (int i = 0; i < infoNames.Length; i++)
                {
                    infoNames[i] = infos[i].Name;
                }
                _index = LimitIn(_index, 0, infoNames.Length - 1);
                _index = EditorGUILayout.Popup(_index, infoNames);
                if (GUILayout.Button("Sort"))
                {
                    try
                    {
                        //*/ 
                        infos[_index].SetValue(_component, Sort((IList)infos[_index].GetValue(_component)));
                        /*/
                        // TODO 可以使用其他的排序方法
                        //*/

                    }
                    catch (Exception e)
                    {
                        Debug.Log(e);
                    }
                }
            }
        }

        private static bool InheritedFromIEnumerable(Type type)
        {
            Type baseType = type.GetInterface("System.Collections.IList");
            if (baseType != null)
            {
                return true;
            }
            return false;
        }
        private static int LimitIn(int t, int min, int max)
        {
            if (min > max)
            {
                return t;
            }
            if (t < min)
            {
                return min;
            }
            if (t > max)
            {
                return max;
            }
            return t;
        }

        private static IList Sort(IList array)
        {
            for (int lengthI = array.Count - 1, i = 0; i < lengthI; i++)
            {
                for (int lengthJ = lengthI - i, j = 0; j < lengthJ; j++)
                {
                    if (string.CompareOrdinal(((UnityEngine.Object)array[j + 1]).name, ((UnityEngine.Object)array[j]).name) < 0)
                    {
                        object temp = array[j + 1];
                        array[j + 1] = array[j];
                        array[j] = temp;
                    }
                }
            }
            return array;
        }
    }
}
