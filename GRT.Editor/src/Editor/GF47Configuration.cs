/***************************************************************
 * @File Name       : GF47Configuration
 * @Author          : GF47
 * @Description     : 配置文件生成面板
 * @Date            : 2017/3/27/星期一 15:35:02
 * @Edit            : none
 **************************************************************/

using GRT.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEditor;
using UnityEngine;

namespace GRT.Editor
{
    public class GF47Configuration : EditorWindow
    {
        private class Item
        {
            public string name;
            public Convert.UnityStructs type;
            public object value;

            public Item(string name, Convert.UnityStructs type, object value)
            {
                this.name = name;
                this.type = type;
                this.value = value;
            }
        }

        // private static Color DEFAULTCOLOR = GUI.backgroundColor;
        // private static Color CHANGEDCOLOR = Color.yellow;

        private static string _configPath = string.Empty;
        private static List<Item> _config;
        private Vector2 _scrollvector2 = Vector2.zero;
        private int _insertItem = -1;
        private int _deleteItem = -1;
        // private HashSet<int> _dirtyItems;

        [MenuItem("Tools/GF47 Editor/Configuration")]
        private static void Init()
        {
            GF47Configuration window = GetWindow<GF47Configuration>();
            window.position = new Rect(200f, 200f, 400f, 400f);
            // window._dirtyItems = new HashSet<int>();
            window.Show();
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField(_configPath);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("打开", EditorStyles.miniButtonLeft, GUILayout.Width(100)))
            {
                _config = new List<Item>();

                _configPath = EditorUtility.OpenFilePanelWithFilters("打开配置文件", Application.dataPath, new[] { "Xml Files", "xml", "Config Files", "config", "All Files", "*" });
                Debug.Log(_configPath);
                Open();
            }
            if (GUILayout.Button("新建", EditorStyles.miniButtonRight, GUILayout.Width(100)))
            {
                _config = new List<Item>();

                _configPath = EditorUtility.SaveFilePanel("新建配置文件", Application.dataPath, "config", "xml");
                Debug.Log(_configPath);
                CreateNew();
            }
            EditorGUILayout.EndHorizontal();

            if (_config == null) { return; }

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("名称", GUILayout.Width(100f));
            EditorGUILayout.LabelField("类型", GUILayout.Width(100f));
            EditorGUILayout.LabelField("值");
            EditorGUILayout.EndHorizontal();

            _scrollvector2 = EditorGUILayout.BeginScrollView(_scrollvector2);
            for (int i = 0; i < _config.Count; i++)
            {
                // if (_dirtyItems.Contains(i))
                // {
                //     GUI.backgroundColor = CHANGEDCOLOR;
                // }
                EditorGUILayout.BeginHorizontal();
                string tmpName = EditorGUILayout.TextField(_config[i].name, GUILayout.Width(100f));
                if (!string.Equals(tmpName, _config[i].name))
                {
                    // _dirtyItems.Add(i);
                    _config[i].name = tmpName;
                }

                Convert.UnityStructs tmpType = (Convert.UnityStructs)EditorGUILayout.EnumPopup(_config[i].type, GUILayout.Width(100f));
                if (tmpType != _config[i].type)
                {
                    // _dirtyItems.Add(i);
                    _config[i].type = tmpType;
                    _config[i].value = Convert.GetDefaultValue(_config[i].type);
                }

                object tmpValue = null;
                switch (_config[i].type)
                {
                    case Convert.UnityStructs.String:
                        tmpValue = EditorGUILayout.TextField((string)_config[i].value);
                        break;

                    case Convert.UnityStructs.Boolean:
                        tmpValue = EditorGUILayout.Toggle((bool)_config[i].value);
                        break;

                    case Convert.UnityStructs.Int32:
                        tmpValue = EditorGUILayout.IntField((int)_config[i].value);
                        break;

                    case Convert.UnityStructs.Single:
                        tmpValue = EditorGUILayout.FloatField((float)_config[i].value);
                        break;

                    case Convert.UnityStructs.Double:
                        tmpValue = EditorGUILayout.DoubleField((double)_config[i].value);
                        break;

                    case Convert.UnityStructs.Vector2:
                        tmpValue = EditorGUILayout.Vector2Field(string.Empty, (Vector2)_config[i].value);
                        break;

                    case Convert.UnityStructs.Vector3:
                        tmpValue = EditorGUILayout.Vector3Field(string.Empty, (Vector3)_config[i].value);
                        break;

                    case Convert.UnityStructs.Vector4:
                        tmpValue = EditorGUILayout.Vector4Field(string.Empty, (Vector4)_config[i].value);
                        break;

                    case Convert.UnityStructs.Color:
                        tmpValue = EditorGUILayout.ColorField((Color)_config[i].value);
                        break;

                    case Convert.UnityStructs.Color32:
                        tmpValue = EditorGUILayout.ColorField((Color32)_config[i].value);
                        break;

                    case Convert.UnityStructs.Rect:
                        tmpValue = EditorGUILayout.RectField((Rect)_config[i].value);
                        break;
                }
                if (tmpValue != _config[i].value)
                {
                    // _dirtyItems.Add(i);
                    _config[i].value = tmpValue;
                }

                if (GUILayout.Button("+", EditorStyles.miniButtonLeft, GUILayout.Width(30f)))
                {
                    _insertItem = i;
                }
                if (GUILayout.Button("X", EditorStyles.miniButtonRight, GUILayout.Width(30f)))
                {
                    _deleteItem = i;
                }

                EditorGUILayout.EndHorizontal();

                // GUI.backgroundColor = DEFAULTCOLOR;

                if (_insertItem > -1)
                {
                    _config.Insert(i, new Item("new", Convert.UnityStructs.String, string.Empty));
                    _insertItem = -1;
                }

                if (_deleteItem > -1)
                {
                    _config.RemoveAt(_deleteItem);
                    _deleteItem = -1;
                }
            }
            EditorGUILayout.EndScrollView();

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("添加", EditorStyles.miniButtonLeft))
            {
                _config.Add(new Item("new", Convert.UnityStructs.String, string.Empty));
            }
            if (GUILayout.Button("保存", EditorStyles.miniButtonMid))
            {
                Save();
                AssetDatabase.Refresh();
            }
            if (GUILayout.Button("重新打开", EditorStyles.miniButtonRight))
            {
                _config.Clear();
                Open();
            }
            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// 保存配置到指定位置的文件中
        /// </summary>
        private static void Save()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(_configPath))
                {
                    using (XmlTextWriter w = new XmlTextWriter(sw))
                    {
                        w.Formatting = Formatting.Indented;
                        w.WriteStartDocument();
                        {
                            w.WriteStartElement(ConstValues.ROOTNODE);
                            for (int i = 0; i < _config.Count; i++)
                            {
                                w.WriteStartElement(ConstValues.NODE);
                                {
                                    w.WriteAttributeString(ConstValues.NAME, _config[i].name);
                                    w.WriteAttributeString(ConstValues.TYPE, _config[i].type.ToString());
                                    w.WriteAttributeString(ConstValues.VALUE, _config[i].value.ToFormattedString(_config[i].type));
                                }
                                w.WriteEndElement();
                            }
                            w.WriteEndElement();
                        }
                        w.WriteEndDocument();
                        w.Close();
                    }
                    sw.Close();
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        /// <summary>
        /// 新建配置文件
        /// </summary>
        private static void CreateNew()
        {
            if (!string.IsNullOrEmpty(_configPath))
            {
                try
                {
                    if (!string.IsNullOrEmpty(_configPath))
                    {
                        using (StreamWriter sw = new StreamWriter(_configPath))
                        {
                            XmlTextWriter w = new XmlTextWriter(sw);
                            w.Formatting = Formatting.Indented;
                            w.WriteStartDocument();
                            {
                                w.WriteStartElement(ConstValues.ROOTNODE);
                                w.WriteEndElement();
                            }
                            w.WriteEndDocument();
                            w.Close();
                        }
                    }
                    AssetDatabase.Refresh();
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }

        /// <summary>
        /// 打开指定路径的配置文件
        /// </summary>
        private static void Open()
        {
            if (File.Exists(_configPath))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(_configPath);
                XmlNode rn = doc.LastChild;

                XmlNodeList list = rn.SelectNodes(ConstValues.NODE);
                if (list != null)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        var nameAttribute = list[i].SelectSingleNode($"@{ConstValues.NAME}");
                        if (nameAttribute == null) continue;
                        var typeAttribute = list[i].SelectSingleNode($"@{ConstValues.TYPE}");
                        if (typeAttribute == null) continue;
                        var valueAttribute = list[i].SelectSingleNode($"@{ConstValues.VALUE}");
                        if (valueAttribute == null) continue;

                        _config.Add(new Item(nameAttribute.Value, Convert.ToUnityStructsEnum(typeAttribute.Value), Convert.ConvertTo(typeAttribute.Value, valueAttribute.Value)));
                    }
                }
            }
        }
    }
}