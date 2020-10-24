/* ****************************************************************
 * @File Name   :   GF47Serialization
 * @Author      :   GF47
 * @Date        :   2015/9/22 10:11:15
 * @Description :   to do
 * @Edit        :   2015/9/22 10:11:15
 * ***************************************************************/

using System;
using System.IO;
using System.Reflection;
using System.Xml;
using UnityEditor;
using UnityEngine;

namespace GF47Editor
{
    /// <summary>
    /// A example class in Unity
    /// </summary>
    public class GF47Serialization : EditorWindow
    {
        private MonoBehaviour _component;
        private bool _includeNonPublic;

        [MenuItem("Tools/GF47 Editor/Serialization")]
        static void Init()
        {
            GF47Serialization window = GetWindow<GF47Serialization>();
            window.position = new Rect(200f, 200f, 400f, 400f);
            window.Show();
        }

        void OnGUI()
        {
            _component = (MonoBehaviour)EditorGUILayout.ObjectField("组件", _component, typeof(MonoBehaviour), true);
            if (_component != null)
            {
                _includeNonPublic = EditorGUILayout.ToggleLeft("包含非公共属性或字段", _includeNonPublic);

                if (GUILayout.Button("序列化"))
                {
                    string path = EditorUtility.SaveFilePanel("保存位置", Application.dataPath, string.Format("{0}.xml", _component.name), "xml");
                    if (string.IsNullOrEmpty(path))
                    {
                        return;
                    }
                    using (StreamWriter writer = new StreamWriter(path))
                    {
                        Type type = _component.GetType();

                        XmlTextWriter xmlTextWriter = new XmlTextWriter(writer)
                        {
                            Formatting = Formatting.Indented
                        };
                        xmlTextWriter.WriteStartDocument();
                        xmlTextWriter.WriteStartElement(_component.name);

                        BindingFlags flag  = BindingFlags.Instance | BindingFlags.Public;
                        if (_includeNonPublic)
                        {
                            flag |= BindingFlags.NonPublic;
                        }

                        FieldInfo[] fields = type.GetFields(flag);
                        for (int i = 0; i < fields.Length; i++)
                        {
                            /* if (fields[i].GetCustomAttribute<ObsoleteAttribute>() != null) { continue; } */
                            if (fields[i].IsDefined(typeof(ObsoleteAttribute), true)) { continue; }

                            string fName = fields[i].Name;
                            try
                            {
                                object fValue = fields[i].GetValue(_component);
                                Type fType = fields[i].FieldType;
                                string value = FormatToString(fType, fValue);
                                if (!string.IsNullOrEmpty(value))
                                {
                                    xmlTextWriter.WriteStartElement(fName);
                                    xmlTextWriter.WriteAttributeString("type", fType.ToString());
                                    xmlTextWriter.WriteAttributeString("value", value);
                                    xmlTextWriter.WriteEndElement();
                                }
                            }
                            catch (Exception e)
                            {
                                Debug.LogError(e);
                            }
                        }

                        PropertyInfo[] properties = type.GetProperties(flag);
                        for (int i = 0; i < properties.Length; i++)
                        {
                            /* if (properties[i].GetCustomAttribute<ObsoleteAttribute>() != null) { continue; } */
                            if (properties[i].IsDefined(typeof(ObsoleteAttribute), true)) { continue; }
                            string pName = properties[i].Name;
                            try
                            {
                                object pValue = properties[i].GetValue(_component, null);
                                Type pType = properties[i].PropertyType;
                                string value = FormatToString(pType, pValue);
                                if (!string.IsNullOrEmpty(value))
                                {
                                    xmlTextWriter.WriteStartElement(pName);
                                    xmlTextWriter.WriteAttributeString("type", pType.ToString());
                                    xmlTextWriter.WriteAttributeString("value", value);
                                    xmlTextWriter.WriteEndElement();
                                }
                            }
                            catch (Exception e)
                            {
                                Debug.LogError(e);
                            }
                        }

                        xmlTextWriter.WriteEndElement();
                        xmlTextWriter.WriteEndDocument();
                        xmlTextWriter.Close();
                    }

                    AssetDatabase.Refresh();
                }
            }
        }

        private static string FormatToString(Type type, object value)
        {
            string result = string.Empty;

            switch (type.ToString())
            {
                case "System.Boolean":
                    result = ((bool)value).ToString();
                    break;
                case "System.Int32":
                    result = ((int)value).ToString();
                    break;
                case "System.Single":
                    result = ((float)value).ToString("F4");
                    break;
                case "System.Double":
                    result = ((double) value).ToString("F4");
                    break;
                case "UnityEngine.Vector2":
                    Vector2 vector2 = (Vector2)value;
                    result = vector2.ToString("F4");
                    break;
                case "UnityEngine.Vector3":
                    Vector3 vector3 = (Vector3)value;
                    result = vector3.ToString("F4");
                    break;
                case "UnityEngine.Vector4":
                    Vector4 vector4 = (Vector4)value;
                    result = vector4.ToString("F4");
                    break;
                case "UnityEngine.Color":
                    Color32 color = (Color)value;
                    result = color.ToString();
                    break;
                case "UnityEngine.Color32":
                    Color32 color32 = (Color32)value;
                    result = color32.ToString();
                    break;
                case "System.String":
                    result = (string)value;
                    break;
            }

            return result;
        }
    }
}
