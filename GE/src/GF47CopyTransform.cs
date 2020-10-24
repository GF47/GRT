using UnityEditor;
using UnityEngine;

namespace GF47Editor
{
    public class GF47CopyTransform : ScriptableObject
    {
        private static Vector3 _position;
        private static Quaternion _rotation;
        private static Vector3 _scale;

        //private static Vector3 _tmpVector3;

        [MenuItem("Tools/GF47 Editor/Transform/Copy Transform &%c")]
        static void TransformRecord()
        {
            _position = Selection.activeTransform.localPosition;
            _rotation = Selection.activeTransform.localRotation;
            _scale = Selection.activeTransform.localScale;
            string str = string.Format("position={0}, eulerAngles={1}, scale={2}", _position.ToString("F4"), _rotation.eulerAngles.ToString("F4"), _scale.ToString("F4"));
            Debug.Log(str);
            EditorGUIUtility.systemCopyBuffer = str;
        }

        [MenuItem("Tools/GF47 Editor/Transform/Paste Transform &%v")]
        static void TransformApply()
        {
            Selection.activeTransform.localPosition = _position;
            Selection.activeTransform.localRotation = _rotation;
            Selection.activeTransform.localScale = _scale;
        }
    }
}

