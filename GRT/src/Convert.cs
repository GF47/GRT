using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace GRT
{
    public static class Convert
    {
        public enum UnityStructs
        {
            String,
            Boolean,
            Int32,
            Single,
            Double,
            Vector2,
            Vector3,
            Vector4,
            Color,
            Color32,
            Rect,
        }

        public static bool ToBool(string value)
        {
            if (!bool.TryParse(value, out bool r))
            {
                return false;
            }
            return r;
        }

        public static int ToInt32(string value)
        {
            if (!int.TryParse(value, out int r))
            {
                return 0;
            }
            return r;
        }

        public static float ToFloat(string value)
        {
            if (!float.TryParse(value, out float r))
            {
                return 0f;
            }
            return r;
        }

        public static double ToDouble(string value)
        {
            if (!double.TryParse(value, out double r))
            {
                return 0d;
            }
            return r;
        }

        public static Vector2 ToVector2(string value)
        {
            string[] array = StringInBrackets(value).Split(',');
            float[] f = { 0f, 0f };
            for (int i = 0; i < array.Length && i < 2; i++)
            {
                float.TryParse(array[i], out f[i]);
            }
            return new Vector2(f[0], f[1]);
        }

        public static Vector3 ToVector3(string value)
        {
            string[] array = StringInBrackets(value).Split(',');
            float[] f = { 0f, 0f, 0f };
            for (int i = 0; i < array.Length && i < 3; i++)
            {
                float.TryParse(array[i], out f[i]);
            }
            return new Vector3(f[0], f[1], f[2]);
        }

        public static Vector4 ToVector4(string value)
        {
            string[] array = StringInBrackets(value).Split(',');
            float[] f = { 0f, 0f, 0f, 0f };
            for (int i = 0; i < array.Length && i < 4; i++)
            {
                float.TryParse(array[i], out f[i]);
            }
            return new Vector4(f[0], f[1], f[2], f[3]);
        }

        public static Color ToColor(string value)
        {
            string[] array = StringInBrackets(value).Split(',');
            float[] f = { 0f, 0f, 0f, 1f };
            for (int i = 0; i < array.Length && i < 4; i++)
            {
                float.TryParse(array[i], out f[i]);
            }
            return new Color(f[0], f[1], f[2], f[3]);
        }

        public static Color32 ToColor32(string value)
        {
            string[] array = StringInBrackets(value).Split(',');
            byte[] c = { 0, 0, 0, 255 };
            for (int i = 0; i < array.Length && i < 4; i++)
            {
                byte.TryParse(array[i], out c[i]);
            }
            return new Color32(c[0], c[1], c[2], c[3]);
        }

        public static Rect ToRect(string value)
        {
            string[] array = StringInBrackets(value).Split(',');
            float[] c = { 0f, 0f, 0f, 0f };
            for (int i = 0; i < array.Length && i < 4; i++)
            {
                float.TryParse(array[i], out c[i]);
            }
            return new Rect(c[0], c[1], c[2], c[3]);
        }

        public static byte[] StructToBytes(object structObj)
        {
            int size = Marshal.SizeOf(structObj);
            byte[] bytes = new byte[size];
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(structObj, structPtr, false);
            Marshal.Copy(structPtr, bytes, 0, size);
            Marshal.FreeHGlobal(structPtr);
            return bytes;
        }

        public static object BytesToStruct(byte[] bytes, Type type)
        {
            int size = Marshal.SizeOf(type);
            if (size > bytes.Length) { return null; }

            IntPtr structPtr = Marshal.AllocHGlobal(size);
            Marshal.Copy(bytes, 0, structPtr, size);
            object obj = Marshal.PtrToStructure(structPtr, type);
            Marshal.FreeHGlobal(structPtr);
            return obj;
        }

        public static string StringInBrackets(string s, char leftBracket = '(', char rightBracket = ')')
        {
            int left = s.IndexOf(leftBracket) + 1;
            int right = s.LastIndexOf(rightBracket);
            right = right > -1 ? right : s.Length;
            return s.Substring(left, right - left);

            // string tmp = s.Substring(s.IndexOf("(", StringComparison.Ordinal) + 1);
            // int last = tmp.LastIndexOf(")", StringComparison.Ordinal);
            // last = last > -1 ? last : tmp.Length;
            // tmp = tmp.Substring(0, last);
            // return tmp;
        }

        public static T[] StringToArray<T>(string s, char splitChar = ',')
        {
            if (string.IsNullOrEmpty(s)) { return null; }
            string[] strArray = s.Split(splitChar);
            T[] array = new T[strArray.Length];
            string type = typeof(T).Name;
            for (int i = 0; i < strArray.Length; i++)
            {
                object value = ConvertTo(type, strArray[i]);
                array[i] = value == null ? default : (T)value;
            }
            return array;
        }

        #region 类型转换

        public const string StringType0 = "string";
        public const string StringType1 = "String";
        public const string StringType2 = "System.String";
        public const string BooleanType0 = "bool";
        public const string BooleanType1 = "Boolean";
        public const string BooleanType2 = "System.Boolean";
        public const string IntType0 = "int";
        public const string IntType1 = "Int32";
        public const string IntType2 = "System.Int32";
        public const string FloatType0 = "float";
        public const string FloatType1 = "Single";
        public const string FloatType2 = "System.Single";
        public const string DoubleType0 = "double";
        public const string DoubleType1 = "Double";
        public const string DoubleType2 = "System.Double";
        public const string Vector2Type0 = "vector2";
        public const string Vector2Type1 = "Vector2";
        public const string Vector2Type2 = "UnityEngine.Vector2";
        public const string Vector3Type0 = "vector3";
        public const string Vector3Type1 = "Vector3";
        public const string Vector3Type2 = "UnityEngine.Vector3";
        public const string Vector4Type0 = "vector4";
        public const string Vector4Type1 = "Vector4";
        public const string Vector4Type2 = "UnityEngine.Vector4";
        public const string ColorType0 = "color";
        public const string ColorType1 = "Color";
        public const string ColorType2 = "UnityEngine.Color";
        public const string Color32Type0 = "color32";
        public const string Color32Type1 = "Color32";
        public const string Color32Type2 = "UnityEngine.Color32";
        public const string RectType0 = "rect";
        public const string RectType1 = "Rect";
        public const string RectType2 = "UnityEngine.Rect";

        public static UnityStructs ToUnityStructsEnum(Type type)
        { 
            UnityStructs t = UnityStructs.String;
            if (type == typeof (string))
            {
                t = UnityStructs.String;
            }
            else if (type == typeof (bool))
            {
                t = UnityStructs.Boolean;
            }
            else if (type == typeof (int))
            {
                t = UnityStructs.Int32;
            }
            else if (type == typeof (float))
            {
                t = UnityStructs.Single;
            }
            else if (type == typeof (double))
            {
                t = UnityStructs.Double;
            }
            else if (type == typeof (Vector2))
            {
                t = UnityStructs.Vector2;
            }
            else if (type == typeof (Vector3))
            {
                t = UnityStructs.Vector3;
            }
            else if (type == typeof (Vector4))
            {
                t = UnityStructs.Vector4;
            }
            else if (type == typeof (Color))
            {
                t = UnityStructs.Color;
            }
            else if (type == typeof (Color32))
            {
                t = UnityStructs.Color32;
            }
            else if (type == typeof (Rect))
            {
                t = UnityStructs.Rect;
            }

            return t;
        }
        public static UnityStructs ToUnityStructsEnum(string type)
        {
            UnityStructs t = UnityStructs.String;

            switch (type)
            {
                case StringType0:
                case StringType1:
                case StringType2:
                    t = UnityStructs.String;
                    break;
                case BooleanType0:
                case BooleanType1:
                case BooleanType2:
                    t = UnityStructs.Boolean;
                    break;
                case IntType0:
                case IntType1:
                case IntType2:
                    t = UnityStructs.Int32;
                    break;
                case FloatType0:
                case FloatType1:
                case FloatType2:
                    t = UnityStructs.Single;
                    break;
                case DoubleType0:
                case DoubleType1:
                case DoubleType2:
                    t = UnityStructs.Double;
                    break;
                case Vector2Type0:
                case Vector2Type1:
                case Vector2Type2:
                    t = UnityStructs.Vector2;
                    break;
                case Vector3Type0:
                case Vector3Type1:
                case Vector3Type2:
                    t = UnityStructs.Vector3;
                    break;
                case Vector4Type0:
                case Vector4Type1:
                case Vector4Type2:
                    t = UnityStructs.Vector4;
                    break;
                case ColorType0:
                case ColorType1:
                case ColorType2:
                    t = UnityStructs.Color;
                    break;
                case Color32Type0:
                case Color32Type1:
                case Color32Type2:
                    t = UnityStructs.Color32;
                    break;
                case RectType0:
                case RectType1:
                case RectType2:
                    t = UnityStructs.Rect;
                    break;
            }
            return t;
        }
        public static Type ToType(UnityStructs type)
        {
            Type t = null;
            if (type == UnityStructs.String)
            {
                t = typeof(string);
            }
            else if (type == UnityStructs.Boolean)
            {
                t = typeof(bool);
            }
            else if (type == UnityStructs.Int32)
            {
                t = typeof(int);
            }
            else if (type == UnityStructs.Single)
            {
                t = typeof(float);
            }
            else if (type == UnityStructs.Double)
            {
                t = typeof(double);
            }
            else if (type == UnityStructs.Vector2)
            {
                t = typeof(Vector2);
            }
            else if (type == UnityStructs.Vector3)
            {
                t = typeof(Vector3);
            }
            else if (type == UnityStructs.Vector4)
            {
                t = typeof(Vector4);
            }
            else if (type == UnityStructs.Color)
            {
                t = typeof(Color);
            }
            else if (type == UnityStructs.Color32)
            {
                t = typeof(Color32);
            }
            else if (type == UnityStructs.Rect)
            {
                t = typeof(Rect);
            }

            return t;
        }
        public static Type ToType(string type)
        {
            Type t = null;

            switch (type)
            {
                case StringType0:
                case StringType1:
                case StringType2:
                    t = typeof(string);
                    break;
                case BooleanType0:
                case BooleanType1:
                case BooleanType2:
                    t = typeof(bool);
                    break;
                case IntType0:
                case IntType1:
                case IntType2:
                    t = typeof(int);
                    break;
                case FloatType0:
                case FloatType1:
                case FloatType2:
                    t = typeof(float);
                    break;
                case DoubleType0:
                case DoubleType1:
                case DoubleType2:
                    t = typeof(double);
                    break;
                case Vector2Type0:
                case Vector2Type1:
                case Vector2Type2:
                    t = typeof(Vector2);
                    break;
                case Vector3Type0:
                case Vector3Type1:
                case Vector3Type2:
                    t = typeof(Vector3);
                    break;
                case Vector4Type0:
                case Vector4Type1:
                case Vector4Type2:
                    t = typeof(Vector4);
                    break;
                case ColorType0:
                case ColorType1:
                case ColorType2:
                    t = typeof(Color);
                    break;
                case Color32Type0:
                case Color32Type1:
                case Color32Type2:
                    t = typeof(Color32);
                    break;
                case RectType0:
                case RectType1:
                case RectType2:
                    t = typeof(Rect);
                    break;
            }
            return t;
        }
        public static object ConvertTo(Type type, string value)
        {
            object result = null;
            if (type == typeof(string))
            {
                result = value;
            }
            else if (type == typeof(bool))
            { 
                result = bool.Parse(value);
            }
            else if (type == typeof(int))
            { 
                result = int.Parse(value);
            }
            else if (type == typeof(float))
            { 
                result = float.Parse(value);
            }
            else if (type == typeof(double))
            { 
                result = double.Parse(value);
            }
            else if (type == typeof(Vector2))
            {
                result = ToVector2(value);
            }
            else if (type == typeof(Vector3))
            {
                result = ToVector3(value);
            }
            else if (type == typeof(Vector4))
            {
                result = ToVector4(value);
            }
            else if (type == typeof(Color))
            {
                result = ToColor(value);
            }
            else if (type == typeof(Color32))
            {
                result = ToColor32(value);
            }
            else if (type == typeof(Rect))
            {
                result = ToRect(value);
            }
            return result;
        }
        public static object ConvertTo(UnityStructs type, string value)
        {
            object result = null;
            switch (type)
            {
                case UnityStructs.String:
                    result = value;
                    break;
                case UnityStructs.Boolean:
                    result = bool.Parse(value);
                    break;
                case UnityStructs.Int32:
                    result = int.Parse(value);
                    break;
                case UnityStructs.Single:
                    result = float.Parse(value);
                    break;
                case UnityStructs.Double:
                    result = double.Parse(value);
                    break;
                case UnityStructs.Vector2:
                    result = ToVector2(value);
                    break;
                case UnityStructs.Vector3:
                    result = ToVector3(value);
                    break;
                case UnityStructs.Vector4:
                    result = ToVector4(value);
                    break;
                case UnityStructs.Color:
                    result = ToColor(value);
                    break;
                case UnityStructs.Color32:
                    result = ToColor32(value);
                    break;
                case UnityStructs.Rect:
                    result = ToRect(value);
                    break;
            }
            return result;
        }
        public static object ConvertTo(string type, string value)
        {
            object result = null;
            switch (type)
            {
                case StringType0:
                case StringType1:
                case StringType2:
                    result = value;
                    break;
                case BooleanType0:
                case BooleanType1:
                case BooleanType2:
                    result = bool.Parse(value);
                    break;
                case IntType0:
                case IntType1:
                case IntType2:
                    result = int.Parse(value);
                    break;
                case FloatType0:
                case FloatType1:
                case FloatType2:
                    result = float.Parse(value);
                    break;
                case DoubleType0:
                case DoubleType1:
                case DoubleType2:
                    result = double.Parse(value);
                    break;
                case Vector2Type0:
                case Vector2Type1:
                case Vector2Type2:
                    result = ToVector2(value);
                    break;
                case Vector3Type0:
                case Vector3Type1:
                case Vector3Type2:
                    result = ToVector3(value);
                    break;
                case Vector4Type0:
                case Vector4Type1:
                case Vector4Type2:
                    result = ToVector4(value);
                    break;
                case ColorType0:
                case ColorType1:
                case ColorType2:
                    result = ToColor(value);
                    break;
                case Color32Type0:
                case Color32Type1:
                case Color32Type2:
                    result = ToColor32(value);
                    break;
                case RectType0:
                case RectType1:
                case RectType2:
                    result = ToRect(value);
                    break;
            }
            return result;
        }

        public static object GetDefaultValue(UnityStructs type)
        {
            object result = null;
            switch (type)
            {
                case UnityStructs.String:
                    result = string.Empty;
                    break;
                case UnityStructs.Boolean:
                    result = false;
                    break;
                case UnityStructs.Int32:
                    result = 0;
                    break;
                case UnityStructs.Single:
                    result = 0f;
                    break;
                case UnityStructs.Double:
                    result = 0d;
                    break;
                case UnityStructs.Vector2:
                    result = Vector2.zero;
                    break;
                case UnityStructs.Vector3:
                    result = Vector3.zero;
                    break;
                case UnityStructs.Vector4:
                    result = Vector4.zero;
                    break;
                case UnityStructs.Color:
                    result = Color.black;
                    break;
                case UnityStructs.Color32:
                    result = (Color32)Color.black;
                    break;
                case UnityStructs.Rect:
                    result = new Rect(0, 0, 0, 0);
                    break;
            }
            return result;
        }

        public static string FormatToString(UnityStructs type, object value)
        {
            string result = string.Empty;

            switch (type)
            {
                case UnityStructs.String:
                    result = (string) value;
                    break;
                case UnityStructs.Boolean:
                    result = ((bool) value).ToString();
                    break;
                case UnityStructs.Int32:
                    result = ((int) value).ToString();
                    break;
                case UnityStructs.Single:
                    result = ((float) value).ToString("F4");
                    break;
                case UnityStructs.Double:
                    result = ((double) value).ToString("F4");
                    break;
                case UnityStructs.Vector2:
                    Vector2 vector2 = (Vector2) value;
                    result = vector2.ToString("F4");
                    break;
                case UnityStructs.Vector3:
                    Vector3 vector3 = (Vector3) value;
                    result = vector3.ToString("F4");
                    break;
                case UnityStructs.Vector4:
                    Vector4 vector4 = (Vector4) value;
                    result = vector4.ToString("F4");
                    break;
                case UnityStructs.Color:
                    Color32 color = (Color) value;
                    result = color.ToString();
                    break;
                case UnityStructs.Color32:
                    Color32 color32 = (Color32) value;
                    result = color32.ToString();
                    break;
                case UnityStructs.Rect:
                    Rect rect = (Rect) value;
                    result = rect.ToString();
                    break;
            }
            return result;
        }

        #endregion
    }
}
