// /***************************************************************
//  * @File Name       : Deserializer
//  * @Author          : GF47
//  * @Description     : 反序列化工具类
//  * @Date            : 2017/2/28/星期二 18:54:43
//  * @Edit            : none
//  **************************************************************/

// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.Reflection;
// using UnityEngine;
// using System.Xml;

// namespace GF47
// {
//     public static class Deserializer
//     {
//         private static Transform _root;

//         public static void DoDeserialize<T>(T t, XmlNode n, Transform root) where T : Component
//         {
//             _root = root;

//             Type type = typeof(T);
//             __SetFieldsValue(type, n, t);

//             _root = null;
//         }

//         private static void __SetFieldsValue(Type type, XmlNode n, object obj)
//         {
//             XmlNodeList fieldList = n.ChildNodes;
//             for (int i = 0; i < fieldList.Count; i++)
//             {
//                 FieldInfo fInfo = type.GetField(fieldList[i].Name, SerializeUtility.FLAGS);
//                 if (fInfo == null) continue;

//                 object value = __DesField(fInfo, fieldList[i]);
//                 if (value != null)
//                 {
//                     fInfo.SetValue(obj, value);
//                 }
//             }
//         }

//         private static object __DesField(FieldInfo f, XmlNode n)
//         {
//             if (f == null) return null;

//             return __DesByType(f.FieldType, n);
//         }

//         #region 反序列化具体类型

//         private static object __DesByType(Type type1, XmlNode n)
//         {
//             if (n == null) return null;

//             string strType = string.Empty;
//             if (!n.HasAttribute(SerializeUtility.TYPE, ref strType)) return null; // 找不到字段的类型名，返回
//             Type type = Type.GetType(strType); // 尝试根据类型名查找字段的类型
//             if (type == null) { type = type1; } // 找不到字段的类型，设为默认类型

//             string strValue = string.Empty;
//             object value = null;
//             if (n.HasAttribute(SerializeUtility.VALUE, ref strValue)) // 找到了字段的值字符串
//             {
//                 if (type == typeof(bool)) { value = bool.Parse(strValue); }
//                 else if (type == typeof(int)) { value = int.Parse(strValue); }
//                 else if (type == typeof(float)) { value = float.Parse(strValue); }
//                 else if (type == typeof(double)) { value = double.Parse(strValue); }
//                 else if (type == typeof(Vector2)) { value = Convert.ToVector2(strValue); }
//                 else if (type == typeof(Vector3)) { value = Convert.ToVector3(strValue); }
//                 else if (type == typeof(Vector4)) { value = Convert.ToVector4(strValue); }
//                 else if (type == typeof(Rect)) { value = Convert.ToRect(strValue); }
//                 else if (type == typeof(Color)) { value = Convert.ToColor(strValue); }
//                 else if (type == typeof(Color32)) { value = Convert.ToColor32(strValue); }
//                 else if (type == typeof(string)) { value = strValue; }
//                 else if (type == typeof(LayerMask)) { value = (LayerMask)int.Parse(strValue); }
//                 else if (type.IsEnum) { value = Enum.Parse(type, strValue); }

//                 else if (type.IsSubclassOf(typeof(Component))) { value = __GetComponentByIndexRelativeToRoot(strValue, type, _root); }
//             }
//             else // 没有值字符串，说明是数组，列表或可序列化类
//             {
//                 if (type.IsArray) // 尝试数组
//                 {
//                      value = __DesArrayType(type, n);
//                 }
//                 else if (type.IsGenericType) // 尝试以列表方式反序列化
//                 {
//                     Type genericTypeDefinition = type.GetGenericTypeDefinition();
//                     if (genericTypeDefinition == typeof (List<>))
//                     {
//                         value = __DesListType(type, genericTypeDefinition, n);
//                     }
//                 }
//                 else
//                 {
//                     object[] attributes = type.GetCustomAttributes(typeof(SerializableAttribute), false);
//                     if (attributes.Length > 0) // 尝试以可序列化类方式反序列化
//                     {
//                         value = __DesSerializableClassType(type, n);
//                     }
//                 }
//             }
//             return value;
//         }

//         #region 三种特殊类型

//         private static object __DesArrayType(Type type, XmlNode n)
//         {
//             XmlNodeList xnl = n.ChildNodes;
//             if (xnl.Count == 0) return null;

//             Type elemenType = type.GetElementType();
//             if (elemenType == null)
//             {
//                 string strType = string.Empty;
//                 if (!xnl[0].HasAttribute(SerializeUtility.TYPE, ref strType)) return null; // 找不到字段的类型名，返回
//                 elemenType = Type.GetType(strType); // 尝试根据类型名查找字段的类型
//             }
//             if (elemenType != null)
//             {
//                 Array array = Array.CreateInstance(elemenType, xnl.Count);
//                 for (int i = 0; i < xnl.Count; i++)
//                 {
//                     array.SetValue(__DesByType(elemenType, xnl[i]), i);
//                 }
//                 return array;
//             }
//             return null;
//         }

//         private static object __DesListType(Type type, Type genericTypeDefinition, XmlNode n)
//         {
//             XmlNodeList xnl = n.ChildNodes;
//             if (xnl.Count == 0) return null;

//             Type[] args = type.GetGenericArguments();
//             if (args.Length == 0) return null;
//             Type elemenType = args[0];
//             if (elemenType != null)
//             {
//                 Type listType = genericTypeDefinition.MakeGenericType(elemenType);
//                 IList list = (IList)Activator.CreateInstance(listType);
//                 for (int i = 0; i < xnl.Count; i++)
//                 {
//                     list.Add(__DesByType(elemenType, xnl[i]));
//                 }
//                 return list;
//             }
//             return null;
//         }

//         private static object __DesSerializableClassType(Type type, XmlNode n)
//         {
//             object obj = Activator.CreateInstance(type);
//             __SetFieldsValue(type, n, obj);
//             return obj;
//         }

//         #endregion

//         #endregion

//         private static object __GetComponentByIndexRelativeToRoot(string indexString, Type type, Transform root)
//         {
//             int[] index = Convert.StringToArray<int>(indexString);
//             if (index == null) return root?.gameObject.GetComponent(type);
//             for (int i = index.Length - 1; i > -1; i--)
//             {
//                 root = root.GetChild(index[i]);
//             }
//             return root?.gameObject.GetComponent(type);
//         }
//     }
// }
