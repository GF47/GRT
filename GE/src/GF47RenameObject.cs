//************************************************************//
//      Author      :       GF47
//      DataTime    :       2013/10/24 星期四 14:57:08
//      Edited      :       2013/10/24 星期四 14:57:08
//************************************************************//

using System.Collections.Generic;
using System.Globalization;
using UnityEditor;
using UnityEngine;

namespace GF47Editor
{
    /// <summary>
    /// Author          :GF47
    /// DataTime        :2013/10/24 星期四 14:57:08
    /// [GF47RenameObject] Introduction  :Nothing to introduce
    /// </summary>
    public class GF47RenameObject : EditorWindow
    {
        private enum RenameType
        {
            Replace,
            Add,
            Delete,
            List,
            Rename
        }
        private RenameType _renameType = RenameType.List;

        private enum Position { Before, After }
        private Position _position = Position.Before;

        private bool _persistOldName;
        private int _startNumber;
        private string _renameString;
        private string _renameStringNew;

        private bool _useMySelection;
        private IList<GameObject> _list;
        [MenuItem("Tools/GF47 Editor/Rename Object")]
        static void Init()
        {
            var window = GetWindow<GF47RenameObject>();
            window.position = new Rect(200.0f, 200.0f, 400.0f, 400.0f);
            window.Show();
            window._list = new List<GameObject>();
        }

        void OnGUI()
        {
            _renameType = (RenameType)EditorGUILayout.EnumPopup("重命名方法", _renameType);
            switch (_renameType)
            {
                case RenameType.List:
                    DrawGUIByList();
                    if (GUILayout.Button(new GUIContent("点我", "点击以重命名")))
                    {
                        RenameByList();
                        ResetAll();
                    }
                    if (_useMySelection && _list != null)
                    {
                        for (int i = 0, iMax = _list.Count; i < iMax; i++)
                        {
                            EditorGUILayout.LabelField(_list[i].name);
                        }
                    }
                    break;
                case RenameType.Add:
                    DrawGUIByAdd();
                    if (GUILayout.Button(new GUIContent("点我", "点击以重命名")))
                    {
                        RenameByAdd();
                        ResetAll();
                    }
                    break;
                case RenameType.Delete:
                    DrawGUIByDelete();
                    if (GUILayout.Button(new GUIContent("点我", "点击以重命名")))
                    {
                        RenameByDelete();
                        ResetAll();
                    }
                    break;
                case RenameType.Replace:
                    DrawGUIByReplace();
                    if (GUILayout.Button(new GUIContent("点我", "点击以重命名")))
                    {
                        RenameByReplace();
                        ResetAll();
                    }
                    break;
                case RenameType.Rename:
                    DrawGUIByRename();
                    if (GUILayout.Button(new GUIContent("点我", "点击以重命名")))
                    {
                        RenameByRename();
                        ResetAll();
                    }
                    break;
            }
        }

        private void DrawGUIByList()
        {
            EditorGUILayout.BeginHorizontal();
            _useMySelection = EditorGUILayout.Toggle("使用自定义的顺序", _useMySelection);
            if (GUILayout.Button(new GUIContent("清除")))
            {
                if (_list != null)
                {
                    _list = new List<GameObject>();
                }
            }
            EditorGUILayout.EndHorizontal();
            _startNumber = EditorGUILayout.IntField("开始于", _startNumber);
            _persistOldName = EditorGUILayout.Toggle("保留旧的名字", _persistOldName);
            if (_persistOldName)
            {
                _position = (Position)EditorGUILayout.EnumPopup("数值位置", _position);
            }
        }
        private void RenameByList()
        {
            if (!_useMySelection)
            {
                _list = Selection.gameObjects;
                for (int i = 0, iMax = _list.Count - 1; i < iMax; i++)
                {
                    for (int j = 0, jMax = iMax - i; j < jMax; j++)
                    {
                        if (string.CompareOrdinal(_list[j + 1].name, _list[j].name) < 0)
                        {
                            GameObject temp = _list[j + 1];
                            _list[j + 1] = _list[j];
                            _list[j] = temp;
                        }
                    }
                }
            }
            for (int i = 0, iMax = _list.Count; i < iMax; i++)
            {
                string newName = (_startNumber + i).ToString(CultureInfo.InvariantCulture);
                if (_persistOldName)
                {
                    if (_position == Position.Before)
                    {
                        newName = string.Format("{0}_{1}", newName, _list[i].name);
                    }
                    else if (_position == Position.After)
                    {
                        newName = string.Format("{0}_{1}", _list[i].name, newName);
                    }
                }
                _list[i].name = newName;
                EditorUtility.SetDirty(_list[i]);
            }
        }

        private void DrawGUIByAdd()
        {
            _renameString = EditorGUILayout.TextField("新字符", _renameString);
            _position = (Position)EditorGUILayout.EnumPopup("新字符位置", _position);
        }
        private void RenameByAdd()
        {
            GameObject[] selections = Selection.gameObjects;
            for (int i = 0, iMax = selections.Length; i < iMax; i++)
            {
                string newName = _renameString;
                if (_position == Position.Before)
                {
                    newName = string.Format("{0}{1}", newName, selections[i].name);
                }
                else if (_position == Position.After)
                {
                    newName = string.Format("{0}{1}", selections[i].name, newName);
                }
                selections[i].name = newName;
                EditorUtility.SetDirty(selections[i]);
            }
        }

        private void DrawGUIByDelete()
        {
            _renameString = EditorGUILayout.TextField("要删除的字符", _renameString);
        }
        private void RenameByDelete()
        {
            GameObject[] selections = Selection.gameObjects;
            for (int i = 0, iMax = selections.Length; i < iMax; i++)
            {
                string newName = selections[i].name;
                newName = newName.Replace(_renameString, "");
                selections[i].name = newName;
                EditorUtility.SetDirty(selections[i]);
            }
        }

        private void DrawGUIByReplace()
        {
            _renameString = EditorGUILayout.TextField("旧字符", _renameString);
            _renameStringNew = EditorGUILayout.TextField("新字符", _renameStringNew);
        }
        private void RenameByReplace()
        {
            GameObject[] selections = Selection.gameObjects;
            for (int i = 0, iMax = selections.Length; i < iMax; i++)
            {
                string newName = selections[i].name;
                newName = newName.Replace(_renameString, _renameStringNew);
                selections[i].name = newName;
                EditorUtility.SetDirty(selections[i]);
            }
        }

        private void DrawGUIByRename()
        {
            _renameString = EditorGUILayout.TextField("新字符", _renameString);
        }
        private void RenameByRename()
        {
            GameObject[] selections = Selection.gameObjects;
            foreach (GameObject selection in selections)
            {
                selection.name = _renameString;
                EditorUtility.SetDirty(selection);
            }
        }

        private void ResetAll()
        {
            _position = Position.Before;
            _persistOldName = false;
            _startNumber = 0;
            _renameString = string.Empty;
            _renameStringNew = string.Empty;
            _list = new List<GameObject>();
        }

        void OnDestroy()
        {
            _list = null;
        }

        void OnSelectionChange()
        {
            if (_useMySelection && _list != null)
            {
                GameObject target = Selection.activeGameObject;
                if (target != null)
                {
                    if (!_list.Contains(target))
                    {
                        _list.Add(target);
                    }
                }
            }
            Repaint();
        }
    }
}
