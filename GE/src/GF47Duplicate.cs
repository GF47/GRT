using System.Globalization;
using UnityEditor;
using UnityEngine;

namespace GF47Editor
{
    public class GF47Duplicate : ScriptableObject
    {
        [MenuItem("Tools/GF47 Editor/Duplicate With Sequence Number &%D")]
        static void Duplicate()
        {
            Transform[] trans = Selection.GetTransforms((SelectionMode) 9);
            if (trans == null) return;

            Object[] instances = new Object[trans.Length];
            for (int i = 0, iMax = trans.Length; i < iMax; i++)
            {
                Transform tempTransform = trans[i];
                GameObject tempObject = Instantiate(tempTransform.gameObject);
                instances[i] = tempObject;
                tempObject.transform.parent = tempTransform.parent;
                tempObject.transform.localPosition = tempTransform.localPosition;
                tempObject.transform.localEulerAngles = tempTransform.localEulerAngles;
                tempObject.transform.localScale = tempTransform.localScale;

                string tempNumber = GetLastNumber(tempTransform.name);
                int number = 0;
                if (!string.IsNullOrEmpty(tempNumber))
                {
                    number = int.Parse(tempNumber);
                }
                string tempName = trans[i].name.TrimEnd(number.ToString(CultureInfo.InvariantCulture).ToCharArray());
                tempObject.name = tempName + (++number).ToString(CultureInfo.InvariantCulture);
                for (int j = 0, jMax = trans.Length; j < jMax; j++)
                {
                    if (tempObject.name == trans[j].name)
                    {
                        tempObject.name = tempName + ++number;
                    }
                }
                for (int k = 0; k < i; k++)
                {
                    if (tempObject.name == instances[k].name)
                    {
                        tempObject.name = tempName + ++number;
                    }
                }
            }
            Selection.objects = instances;
        }

        static string GetLastNumber(string s)
        {
            if (string.IsNullOrEmpty(s)) return null;
            int split = 0;
            for (int i = s.Length - 1; i > -1; i--)
            {
                if (s[i] < 48 || s[i] > 57)
                {
                    split = i + 1;
                    break;
                }
            }
            if (split == s.Length) return null;
            string newStr = s.Substring(split);
            return newStr;
        }
    }
}

