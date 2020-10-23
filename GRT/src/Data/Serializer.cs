// /***************************************************************
//  * @File Name       : Serializer
//  * @Author          : GF47
//  * @Description     : 序列化工具类
//  * @Date            : 2017/2/27/星期一 13:15:50
//  * @Edit            : none
//  **************************************************************/

// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.Reflection;
// using System.Text;
// using UnityEngine;

// namespace GF47
// {
//     public static class Serializer
//     {
//         public static Action<string, string> writeKeyValuePair;
//         public static Action<string> writeLeft;
//         public static Action writeRight;

//         private static Transform _root;

//         public static void DoSerialize<T>(T t, Transform root) where T : Component
//         {
//             _root = root;

//             FieldInfo[] f;
//             string fn;
//             object fv;

//             writeLeft(t.name.Replace(' ','_'));
//             Type type = t.GetType();
//             writeKeyValuePair(SerializeUtility.TYPE, type.FullName);
//             f = type.GetFields(SerializeUtility.FLAGS);
//             for (int i = 0; i < f.Length; i++)
//             {
//                 if (__CheckField(f[i]))
//                 {
//                     fn = f[i].Name;
//                     fv = f[i].GetValue(t);

//                     writeLeft(fn);
//                     __SerializeField(fv);
//                     writeRight();
//                 }
//             }
//             writeRight();

//             _root = null;
//         }

//         private static void __SerializeField(object t)
//         {
//             if (t == null) return;

//             Type type = t.GetType();
//             writeKeyValuePair(SerializeUtility.TYPE, type.FullName);

//             if (__SerializeValueType(t, type)) return;

//             if (__SerializeArray(t, type)) return;
//             if (__SerializeList(t, type)) return;

//             if (__SerializeObjectReference(t, type, _root)) return;


//             #region 被标记为可序列化的类

//             FieldInfo[] f;
//             string fn;
//             object fv;

//             f = type.GetFields(SerializeUtility.FLAGS);
//             for (int i = 0; i < f.Length; i++)
//             {
//                 if (__CheckField(f[i]))
//                 {
//                     fn = f[i].Name;
//                     fv = f[i].GetValue(t);

//                     writeLeft(fn);
//                     __SerializeField(fv);
//                     writeRight();
//                 }
//             }

//             #endregion
//         }

//         /// <summary>
//         /// Unity的Object
//         /// </summary>
//         private static bool __SerializeObjectReference(object o, Type type, Transform root)
//         {
//             if (type.IsSubclassOf(typeof(Component)))
//             {
//                 writeKeyValuePair(SerializeUtility.VALUE, __GetIndexRelativeToRoot(((Component)o).transform, root));
//                 return true;
//             }
//             return false;
//         }

//         /// <summary>
//         /// 列表的序列化
//         /// </summary>
//         private static bool __SerializeList(object t, Type type)
//         {
//             if (type.IsGenericType)
//             {
//                 Type genericTypeDefinition = type.GetGenericTypeDefinition();
//                 if (genericTypeDefinition == typeof (List<>))
//                 {
//                     IList list = t as IList;
//                     if (list != null)
//                     {
//                         for (int i = 0; i < list.Count; i++)
//                         {
//                             writeLeft(SerializeUtility.ITEM);
//                             __SerializeField(list[i]);
//                             writeRight();
//                         }
//                     }
//                 }
//                 return true;
//             }
//             return false;
//         }

//         /// <summary>
//         /// 数组的序列化
//         /// </summary>
//         private static bool __SerializeArray(object t, Type type)
//         {
//             if (type.IsArray)
//             {
//                 Array array = (Array) t;
//                 for (int i = 0; i < array.Length; i++)
//                 {
//                     writeLeft(SerializeUtility.ITEM);
//                     __SerializeField(array.GetValue(i));
//                     writeRight();
//                 }
//                 return true;
//             }
//             return false;
//         }

//         /// <summary>
//         /// 普通类型的序列化
//         /// </summary>
//         private static bool __SerializeValueType(object t,Type type)
//         {
//             if (type == typeof (bool) ||
//                 type == typeof (int) ||
//                 type == typeof (float) ||
//                 type == typeof (double) ||
//                 type == typeof (Vector2) ||
//                 type == typeof (Vector3) ||
//                 type == typeof (Vector4) ||
//                 type == typeof (Rect) ||
//                 type == typeof (Color) ||
//                 type == typeof (Color32) ||
//                 type == typeof (string))
//             {
//                 writeKeyValuePair(SerializeUtility.VALUE, t.ToString());
//                 return true;
//             }
//             if (type == typeof (LayerMask))
//             {
//                 writeKeyValuePair(SerializeUtility.VALUE, ((LayerMask) t).value.ToString());
//                 return true;
//             }
//             if (type.IsEnum)
//             {
//                 writeKeyValuePair(SerializeUtility.VALUE, ((int) t).ToString());
//                 return true;
//             }

//             return false;
//         }

//         private static string __GetIndexRelativeToRoot(Transform t, Transform root)
//         {
//             if (t == root) { return string.Empty; }

//             StringBuilder sb = new StringBuilder();
//             Transform parent = null;
//             while (parent != root)
//             {
//                 parent = t.parent;
//                 int i = t.GetSiblingIndex();
//                 t = parent;
//                 if (parent != root)
//                 {
//                     sb.AppendFormat("{0},", i);
//                 }
//                 else
//                 {
//                     sb.Append(i);
//                 }
//             }
//             return sb.ToString();
//         }

//         /// <summary>
//         /// 检测字段是否需要被序列化
//         /// </summary>
//         private static bool __CheckField(FieldInfo f)
//         {
//             object[] attributes;

//             bool isSerializable = f.IsPublic; // 公共字段
//             if (!isSerializable)
//             {
//                 attributes = f.GetCustomAttributes(typeof(SerializeField), true);
//                 if (attributes.Length > 0)
//                 {
//                     isSerializable = true; // 非公共字段但是被标记为可序列化
//                 }
//             }

//             if (isSerializable)
//             {
//                 Type type = f.FieldType;

//                 if (type == typeof(bool) ||
//                     type == typeof(int) ||
//                     type == typeof(float) ||
//                     type == typeof(double) ||
//                     type == typeof(Vector2) ||
//                     type == typeof(Vector3) ||
//                     type == typeof(Vector4) ||
//                     type == typeof(Rect) ||
//                     type == typeof(Color) ||
//                     type == typeof(Color32) ||
//                     type == typeof(string) ||

//                     type == typeof(LayerMask) ||
//                     type.IsEnum ||
//                     type.IsSubclassOf(typeof(Component)))
//                 {
//                     return true;
//                 }

//                 // 可序列化的类或结构体
//                 attributes = type.GetCustomAttributes(typeof(SerializableAttribute), false);
//                 if (attributes.Length > 0) { return true; }
//             }
//             return false;
//         }
//     }
// }
