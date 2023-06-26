using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace GRT
{
    /// <summary>
    /// 将字符串和具体类型互相转化，以及将结构类型与byte数组互相转化
    /// </summary>
    public static class GConvert
    {
        /// <summary>
        /// 结构类型
        /// </summary>
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

        public static bool IsTrue(this string value)
        {
            if (string.IsNullOrEmpty(value)) return false;
            return value == TrueString2
                || value == TrueString1
                || value == TrueString0
                || value == TrueString3
                || value == TrueString4
                || value == TrueString5
                || value == TrueString6
                || value == TrueString7
                || value == TrueString8
                || value == TrueString9
                || value == TrueString10;
        }

        public static bool IsFalse(this string value)
        {
            if (string.IsNullOrEmpty(value)) return false;
            return value == FalseString2
                || value == FalseString1
                || value == FalseString0
                || value == FalseString3
                || value == FalseString4
                || value == FalseString5
                || value == FalseString6
                || value == FalseString7
                || value == FalseString8
                || value == FalseString9
                || value == FalseString10;
        }

        public static bool ToBool(this string value, bool @default = default) => bool.TryParse(value, out bool b) ? b : @default;

        public static int ToInt32(this string value, int @default = default) => int.TryParse(value, out int i) ? i : @default;

        public static float ToFloat(this string value, float @default = default) => float.TryParse(value, out float f) ? f : @default;

        public static double ToDouble(this string value, double @default = default) => double.TryParse(value, out double d) ? d : @default;

        public static Vector2 ToVector2(this string value, float defaultX = default, float defaultY = default)
        {
            string[] array = SubStringInBrackets(value).Split(',');
            var x = array.Length > 0 && float.TryParse(array[0], out float x_) ? x_ : defaultX;
            var y = array.Length > 1 && float.TryParse(array[1], out float y_) ? y_ : defaultY;
            return new Vector2(x, y);
        }

        public static Vector3 ToVector3(this string value,
            float defaultX = default,
            float defaultY = default,
            float defaultZ = default)
        {
            string[] array = SubStringInBrackets(value).Split(',');
            var x = array.Length > 0 && float.TryParse(array[0], out float x_) ? x_ : defaultX;
            var y = array.Length > 1 && float.TryParse(array[1], out float y_) ? y_ : defaultY;
            var z = array.Length > 2 && float.TryParse(array[2], out float z_) ? z_ : defaultZ;
            return new Vector3(x, y, z);
        }

        public static Vector4 ToVector4(this string value,
            float defaultX = default,
            float defaultY = default,
            float defaultZ = default,
            float defaultW = default)
        {
            string[] array = SubStringInBrackets(value).Split(',');
            var x = array.Length > 0 && float.TryParse(array[0], out float x_) ? x_ : defaultX;
            var y = array.Length > 1 && float.TryParse(array[1], out float y_) ? y_ : defaultY;
            var z = array.Length > 2 && float.TryParse(array[2], out float z_) ? z_ : defaultZ;
            var w = array.Length > 3 && float.TryParse(array[3], out float w_) ? w_ : defaultW;
            return new Vector4(x, y, z, w);
        }

        public static Color ToColorFromTags(this string tag)
        {
            switch (tag)
            {
                case "AQUAMARINE": return new Color32(112, 219, 147, 255);
                case "BLACK": return new Color32(0, 0, 0, 255);
                case "BLUE": return new Color32(0, 0, 255, 255);
                case "BLUE VIOLET": return new Color32(159, 95, 159, 255);
                case "BROWN": return new Color32(165, 42, 42, 255);
                case "CADET BLUE": return new Color32(95, 159, 159, 255);
                case "CORAL": return new Color32(255, 127, 0, 255);
                case "CORNFLOWER BLUE": return new Color32(66, 66, 111, 255);
                case "CYAN": return new Color32(0, 255, 255, 255);
                case "DARK GREY": return new Color32(47, 47, 47, 255);

                case "DARK GREEN": return new Color32(47, 79, 47, 255);
                case "DARK OLIVE GREEN": return new Color32(79, 79, 47, 255);
                case "DARK ORCHID": return new Color32(153, 50, 204, 255);
                case "DARK SLATE BLUE": return new Color32(107, 35, 142, 255);
                case "DARK SLATE GREY": return new Color32(47, 79, 79, 255);
                case "DARK TURQUOISE": return new Color32(112, 147, 219, 255);
                case "DIM GREY": return new Color32(84, 84, 84, 255);
                case "FIREBRICK": return new Color32(142, 35, 35, 255);
                case "FOREST GREEN": return new Color32(35, 142, 35, 255);
                case "GOLD": return new Color32(204, 127, 50, 255);
                case "GOLDENROD": return new Color32(219, 219, 112, 255);
                case "GREY": return new Color32(128, 128, 128, 255);
                case "GREEN": return new Color32(0, 255, 0, 255);
                case "GREEN YELLOW": return new Color32(147, 219, 112, 255);
                case "INDIAN RED": return new Color32(79, 47, 47, 255);
                case "KHAKI": return new Color32(159, 159, 95, 255);
                case "LIGHT BLUE": return new Color32(191, 216, 216, 255);
                case "LIGHT GREY": return new Color32(192, 192, 192, 255);
                case "LIGHT STEEL BLUE": return new Color32(143, 143, 188, 255);
                case "LIME GREEN": return new Color32(50, 204, 50, 255);
                case "LIGHT MAGENTA": return new Color32(255, 119, 255, 255);
                case "MAGENTA": return new Color32(255, 0, 255, 255);
                case "MAROON": return new Color32(142, 35, 107, 255);
                case "MEDIUM AQUAMARINE": return new Color32(50, 204, 153, 255);
                case "MEDIUM GREY": return new Color32(100, 100, 100, 255);
                case "MEDIUM BLUE": return new Color32(50, 50, 204, 255);
                case "MEDIUM FOREST GREEN": return new Color32(107, 142, 35, 255);
                case "MEDIUM GOLDENROD": return new Color32(234, 234, 173, 255);
                case "MEDIUM ORCHID": return new Color32(147, 112, 219, 255);
                case "MEDIUM SEA GREEN": return new Color32(66, 111, 66, 255);
                case "MEDIUM SLATE BLUE": return new Color32(127, 0, 255, 255);
                case "MEDIUM SPRING GREEN": return new Color32(127, 255, 0, 255);
                case "MEDIUM TURQUOISE": return new Color32(112, 219, 219, 255);
                case "MEDIUM VIOLET RED": return new Color32(219, 112, 147, 255);
                case "MIDNIGHT BLUE": return new Color32(47, 47, 79, 255);
                case "NAVY": return new Color32(35, 35, 142, 255);
                case "ORANGE": return new Color32(204, 50, 50, 255);
                case "ORANGE RED": return new Color32(255, 0, 127, 255);
                case "ORCHID": return new Color32(219, 112, 219, 255);
                case "PALE GREEN": return new Color32(143, 188, 143, 255);
                case "PINK": return new Color32(255, 192, 203, 255);
                case "PLUM": return new Color32(234, 173, 234, 255);
                case "PURPLE": return new Color32(176, 0, 255, 255);
                case "RED": return new Color32(255, 0, 0, 255);
                case "SALMON": return new Color32(111, 66, 66, 255);
                case "SEA GREEN": return new Color32(35, 142, 107, 255);
                case "SIENNA": return new Color32(142, 107, 35, 255);
                case "SKY BLUE": return new Color32(50, 153, 204, 255);
                case "SLATE BLUE": return new Color32(0, 127, 255, 255);
                case "SPRING GREEN": return new Color32(0, 255, 127, 255);
                case "STEEL BLUE": return new Color32(35, 107, 142, 255);
                case "TAN": return new Color32(219, 147, 112, 255);
                case "THISTLE": return new Color32(216, 191, 216, 255);
                case "TURQUOISE": return new Color32(173, 234, 234, 255);
                case "VIOLET": return new Color32(79, 47, 79, 255);
                case "VIOLET RED": return new Color32(204, 50, 153, 255);
                case "WHEAT": return new Color32(216, 216, 191, 255);
                case "WHITE": return new Color32(255, 255, 255, 255);
                case "YELLOW": return new Color32(255, 255, 0, 255);
                case "YELLOW GREEN": return new Color32(153, 204, 50, 255);
                default:
                    return tag.ToColor(1, 0, 0, 1);
            }
        }

        public static Color ToColor(this string value,
            float defaultR = default,
            float defaultG = default,
            float defaultB = default,
            float defaultA = 1f)
        {
            if (ColorUtility.TryParseHtmlString(value, out var c))
            {
                return c;
            }

            string[] array = SubStringInBrackets(value).Split(',');
            var r = array.Length > 0 && float.TryParse(array[0], out float r_) ? r_ : defaultR;
            var g = array.Length > 1 && float.TryParse(array[1], out float g_) ? g_ : defaultG;
            var b = array.Length > 2 && float.TryParse(array[2], out float b_) ? b_ : defaultB;
            var a = array.Length > 3 && float.TryParse(array[3], out float a_) ? a_ : defaultA;
            return new Color(r, g, b, a);
        }

        public static Color32 ToColor32(this string value,
            byte defaultR = default,
            byte defaultG = default,
            byte defaultB = default,
            byte defaultA = 255)

        {
            string[] array = SubStringInBrackets(value).Split(',');
            var r = array.Length > 0 && byte.TryParse(array[0], out var r_) ? r_ : defaultR;
            var g = array.Length > 1 && byte.TryParse(array[1], out var g_) ? g_ : defaultG;
            var b = array.Length > 2 && byte.TryParse(array[2], out var b_) ? b_ : defaultB;
            var a = array.Length > 3 && byte.TryParse(array[3], out var a_) ? a_ : defaultA;
            return new Color32(r, g, b, a);
        }

        public static Rect ToRect(this string value,
            float defaultX = default,
            float defaultY = default,
            float defaultW = default,
            float defaultH = default)
        {
            string[] array = SubStringInBrackets(value).Split(',');
            var x = array.Length > 0 && float.TryParse(array[0], out float x_) ? x_ : defaultX;
            var y = array.Length > 1 && float.TryParse(array[1], out float y_) ? y_ : defaultY;
            var w = array.Length > 2 && float.TryParse(array[2], out float w_) ? w_ : defaultW;
            var h = array.Length > 3 && float.TryParse(array[3], out float h_) ? h_ : defaultH;
            return new Rect(x, y, w, h);
        }

        public static bool TryParseVector2(this string value, out Vector2 vector)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                string[] array = SubStringInBrackets(value).Split(',');
                if (array.Length > 1
                    && float.TryParse(array[0], out var x)
                    && float.TryParse(array[1], out var y))
                {
                    vector = new Vector2(x, y);
                    return true;
                }
            }
            vector = Vector2.zero;
            return false;
        }

        public static bool TryParseVector3(this string value, out Vector3 vector)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                string[] array = SubStringInBrackets(value).Split(',');
                if (array.Length > 2
                    && float.TryParse(array[0], out var x)
                    && float.TryParse(array[1], out var y)
                    && float.TryParse(array[2], out var z))
                {
                    vector = new Vector3(x, y, z);
                    return true;
                }
            }
            vector = Vector3.zero;
            return false;
        }

        public static bool TryParseVector4(this string value, out Vector4 vector)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                string[] array = SubStringInBrackets(value).Split(',');
                if (array.Length > 3
                    && float.TryParse(array[0], out var x)
                    && float.TryParse(array[1], out var y)
                    && float.TryParse(array[2], out var z)
                    && float.TryParse(array[3], out var w))
                {
                    vector = new Vector4(x, y, z, w);
                    return true;
                }
            }
            vector = Vector4.zero;
            return false;
        }

        public static bool TryParseColor(this string value, out Color color)
        {
            if (ColorUtility.TryParseHtmlString(value, out color))
            {
                return true;
            }

            if (!string.IsNullOrWhiteSpace(value))
            {
                string[] array = SubStringInBrackets(value).Split(',');
                if (array.Length > 2
                    && float.TryParse(array[0], out var r)
                    && float.TryParse(array[1], out var g)
                    && float.TryParse(array[2], out var b))
                {
                    var a = 1f;
                    if (array.Length > 3 && float.TryParse(array[3], out var tempa)) { a = tempa; }
                    color = new Color(r, g, b, a);
                    return true;
                }
            }
            return false;
        }

        public static bool TryParseColor32(this string value, out Color32 color)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                string[] array = SubStringInBrackets(value).Split(',');
                if (array.Length > 2
                    && byte.TryParse(array[0], out var r)
                    && byte.TryParse(array[1], out var g)
                    && byte.TryParse(array[2], out var b))
                {
                    byte a = 255;
                    if (array.Length > 3 && byte.TryParse(array[3], out var temp)) { a = temp; }
                    color = new Color32(r, g, b, a);
                    return true;
                }
            }
            color = new Color32();
            return false;
        }

        public static bool TryParseRect(this string value, out Rect rect)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                string[] array = SubStringInBrackets(value).Split(',');
                if (array.Length > 3
                    && float.TryParse(array[0], out var x)
                    && float.TryParse(array[1], out var y)
                    && float.TryParse(array[2], out var width)
                    && float.TryParse(array[3], out var height))
                {
                    rect = new Rect(x, y, width, height);
                    return true;
                }
            }
            rect = new Rect();
            return false;
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

        /// <summary>
        /// 提取小括号内的字符串，可以自定义
        /// </summary>
        /// <param name="s">被提取的字符串</param>
        /// <param name="leftBracket">左括号</param>
        /// <param name="rightBracket">右括号</param>
        /// <returns></returns>
        public static string SubStringInBrackets(this string s, char leftBracket = '(', char rightBracket = ')')
        {
            int left = s.IndexOf(leftBracket) + 1;
            int right = s.LastIndexOf(rightBracket);
            right = right > -1 ? right : s.Length;
            return s.Substring(left, right - left);
        }

        public static T[] ToArray<T>(this string s, char splitChar = ',')
        {
            if (string.IsNullOrEmpty(s)) { return null; }
            string[] strArray = s.Split(splitChar);
            T[] array = new T[strArray.Length];
            string type = typeof(T).Name;
            for (int i = 0; i < strArray.Length; i++)
            {
                object value = strArray[i].ConvertTo(type);
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

        public const string TrueString0 = "t";
        public const string TrueString1 = "true";
        public const string TrueString2 = "True";
        public const string TrueString3 = "y";
        public const string TrueString4 = "yes";
        public const string TrueString5 = "Yes";
        public const string TrueString6 = "r";
        public const string TrueString7 = "right";
        public const string TrueString8 = "Right";
        public const string TrueString9 = "on";
        public const string TrueString10 = "On";

        public const string FalseString0 = "f";
        public const string FalseString1 = "false";
        public const string FalseString2 = "False";
        public const string FalseString3 = "n";
        public const string FalseString4 = "no";
        public const string FalseString5 = "No";
        public const string FalseString6 = "w";
        public const string FalseString7 = "wrong";
        public const string FalseString8 = "Wrong";
        public const string FalseString9 = "off";
        public const string FalseString10 = "Off";

        public static UnityStructs ToUnityStructsEnum(this Type type)
        {
            UnityStructs t = UnityStructs.String;
            if (type == typeof(string))
            {
                t = UnityStructs.String;
            }
            else if (type == typeof(bool))
            {
                t = UnityStructs.Boolean;
            }
            else if (type == typeof(int))
            {
                t = UnityStructs.Int32;
            }
            else if (type == typeof(float))
            {
                t = UnityStructs.Single;
            }
            else if (type == typeof(double))
            {
                t = UnityStructs.Double;
            }
            else if (type == typeof(Vector2))
            {
                t = UnityStructs.Vector2;
            }
            else if (type == typeof(Vector3))
            {
                t = UnityStructs.Vector3;
            }
            else if (type == typeof(Vector4))
            {
                t = UnityStructs.Vector4;
            }
            else if (type == typeof(Color))
            {
                t = UnityStructs.Color;
            }
            else if (type == typeof(Color32))
            {
                t = UnityStructs.Color32;
            }
            else if (type == typeof(Rect))
            {
                t = UnityStructs.Rect;
            }

            return t;
        }

        public static UnityStructs ToUnityStructsEnum(this string type)
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

        public static Type ToType(this UnityStructs type)
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

        public static Type ToType(this string type)
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

        public static object ConvertTo(this string value, Type type)
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

        public static object ConvertTo(this string value, UnityStructs type)
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

        public static object ConvertTo(this string value, string type)
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

        public static object GetDefaultValue(this UnityStructs type)
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

        public static string ToFormattedString(object value, UnityStructs type)
        {
            string result;

            switch (type)
            {
                case UnityStructs.String:
                    result = (string)value;
                    break;

                case UnityStructs.Boolean:
                    result = ((bool)value).ToString();
                    break;

                case UnityStructs.Int32:
                    result = ((int)value).ToString();
                    break;

                case UnityStructs.Single:
                    result = ((float)value).ToString("F4");
                    break;

                case UnityStructs.Double:
                    result = ((double)value).ToString("F4");
                    break;

                case UnityStructs.Vector2:
                    Vector2 vector2 = (Vector2)value;
                    result = vector2.ToString("F4");
                    break;

                case UnityStructs.Vector3:
                    Vector3 vector3 = (Vector3)value;
                    result = vector3.ToString("F4");
                    break;

                case UnityStructs.Vector4:
                    Vector4 vector4 = (Vector4)value;
                    result = vector4.ToString("F4");
                    break;

                case UnityStructs.Color:
                    Color32 color = (Color)value;
                    result = color.ToString();
                    break;

                case UnityStructs.Color32:
                    Color32 color32 = (Color32)value;
                    result = color32.ToString();
                    break;

                case UnityStructs.Rect:
                    Rect rect = (Rect)value;
                    result = rect.ToString();
                    break;

                default:
                    return value.ToString();
            }
            return result;
        }

        public static string ToFormattedString(object value, UnityStructs type, string format)
        {
            string result = string.Empty;

            switch (type)
            {
                case UnityStructs.String:
                    result = (string)value;
                    break;

                case UnityStructs.Boolean:
                    result = ((bool)value).ToString();
                    break;

                case UnityStructs.Int32:
                    result = ((int)value).ToString();
                    break;

                case UnityStructs.Single:
                    result = ((float)value).ToString(format);
                    break;

                case UnityStructs.Double:
                    result = ((double)value).ToString(format);
                    break;

                case UnityStructs.Vector2:
                    Vector2 vector2 = (Vector2)value;
                    result = vector2.ToString(format);
                    break;

                case UnityStructs.Vector3:
                    Vector3 vector3 = (Vector3)value;
                    result = vector3.ToString(format);
                    break;

                case UnityStructs.Vector4:
                    Vector4 vector4 = (Vector4)value;
                    result = vector4.ToString(format);
                    break;

                case UnityStructs.Color:
                    Color32 color = (Color)value;
                    result = color.ToString(format);
                    break;

                case UnityStructs.Color32:
                    Color32 color32 = (Color32)value;
                    result = color32.ToString(format);
                    break;

                case UnityStructs.Rect:
                    Rect rect = (Rect)value;
                    result = rect.ToString(format);
                    break;
            }
            return result;
        }

        #endregion 类型转换
    }
}