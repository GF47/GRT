using UnityEditor;
using UnityEngine;

namespace GRT.Editor
{
    public class GF47Ruler : ScriptableObject
    {
        private static Vector3 a, b;
        private static bool isActive;

        [MenuItem("Tools/GF47 Editor/Transform/Ruler #&r")]
        private static void Init()
        {
            if (isActive)
            {
                SceneView.duringSceneGui -= SceneView_duringSceneGui;
            }
            else
            {
                SceneView.duringSceneGui += SceneView_duringSceneGui;
                a = Vector3.zero;
                b = Vector3.forward;
            }
            isActive = !isActive;
        }

        private static void SceneView_duringSceneGui(SceneView view)
        {
            Handles.BeginGUI();

            if (GUILayout.Button("Delete Ruler", GUILayout.Width(100f)))
            {
                SceneView.duringSceneGui -= SceneView_duringSceneGui;
                isActive = false;
            }

            var defaultLabelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 22f;

            #region A

            GUILayout.BeginVertical(EditorStyles.textArea, GUILayout.Width(200f));
            a = EditorGUILayout.Vector3Field("A", a, GUILayout.Width(200f));

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Copy", GUILayout.Width(65f))) { WriteToSystemBuffer(a); }
            if (GUILayout.Button("U Copy", GUILayout.Width(65f))) { WriteToSystemBufferU(a); }
            if (GUILayout.Button("Paste", GUILayout.Width(64f))) { a = CopyFromSystemBuffer(a); }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("View Target", GUILayout.Width(133f))) { a = view.pivot; }
            if (GUILayout.Button("Set VT", GUILayout.Width(64f))) { view.pivot = a; }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Object Pivot", GUILayout.Width(133f))) { a = Selection.activeTransform.position; }
            if (GUILayout.Button("Set OP", GUILayout.Width(64f))) { Selection.activeTransform.position = a; }
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();

            #endregion A

            #region B

            GUILayout.BeginVertical(EditorStyles.textArea, GUILayout.Width(200f));
            b = EditorGUILayout.Vector3Field("B", b, GUILayout.Width(200f));

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Copy", GUILayout.Width(65f))) { WriteToSystemBuffer(b); }
            if (GUILayout.Button("U Copy", GUILayout.Width(65f))) { WriteToSystemBufferU(b); }
            if (GUILayout.Button("Paste", GUILayout.Width(64f))) { b = CopyFromSystemBuffer(b); }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("View Target", GUILayout.Width(133f))) { b = view.pivot; }
            if (GUILayout.Button("Set VT", GUILayout.Width(64f))) { view.pivot = b; }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Object Pivot", GUILayout.Width(133f))) { b = Selection.activeTransform.position; }
            if (GUILayout.Button("Set OP", GUILayout.Width(64f))) { Selection.activeTransform.position = b; }
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();

            #endregion B

            #region Center

            var center = (a + b) / 2f;

            GUILayout.BeginVertical(EditorStyles.textArea, GUILayout.Width(200f));
            GUILayout.Label($"C X:{center.x}, Y:{center.y}, Z:{center.z}");

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Copy", GUILayout.Width(65f))) { WriteToSystemBuffer(center); }
            if (GUILayout.Button("U Copy", GUILayout.Width(65f))) { WriteToSystemBufferU(center); }
            if (GUILayout.Button("Paste", GUILayout.Width(64f))) { b = CopyFromSystemBuffer(center) * 2f - a; }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("View Target", GUILayout.Width(133f))) { b = view.pivot * 2f - a; }
            if (GUILayout.Button("Set VT", GUILayout.Width(64f))) { view.pivot = center; }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Object Pivot", GUILayout.Width(133f))) { b = Selection.activeTransform.position * 2f - a; }
            if (GUILayout.Button("Set OP", GUILayout.Width(64f))) { Selection.activeTransform.position = center; }
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();

            #endregion Center

            EditorGUIUtility.labelWidth = defaultLabelWidth;

            GUILayout.BeginVertical(EditorStyles.textArea, GUILayout.Width(200f));
            if (GUILayout.Button("A = View Camera\nB = View Target", GUILayout.Width(200f)))
            {
                a = view.camera.transform.position;
                b = view.pivot;
            }
            if (GUILayout.Button("print camera=\"A\" target=\"B\"", GUILayout.Width(200f)))
            {
                var xmlAttributes = $"camera=\"{a.x:F4},{a.y:F4},{a.z:F4}\" target=\"{b.x:F4},{b.y:F4},{b.z:F4}\"";
                Debug.Log(xmlAttributes);
                EditorGUIUtility.systemCopyBuffer = xmlAttributes;
            }
            GUILayout.Space(10);
            if (GUILayout.Button("Get Length", GUILayout.Width(200f)))
            {
                var length = (b - a).magnitude.ToString();
                Debug.Log(length);
                EditorGUIUtility.systemCopyBuffer = length;
            }
            if (GUILayout.Button("Get A -> B Offset", GUILayout.Width(200f)))
            {
                WriteToSystemBuffer(b - a);
            }

            GUILayout.EndVertical();

            Handles.EndGUI();

            a = Handles.PositionHandle(a, Quaternion.identity);
            b = Handles.PositionHandle(b, Quaternion.identity);

            Handles.color = Color.green;
            Handles.DrawLine(a, b);

            Handles.color = Color.red;
            Handles.Label(0.5f * (a + b), $"a:{a}\nb:{b}\nLenght:{(b - a).magnitude}");

            Handles.color = Color.white;
        }

        private static Vector3 CopyFromSystemBuffer(Vector3 @default) =>
            GConvert.ToVector3(EditorGUIUtility.systemCopyBuffer, @default.x, @default.y, @default.z);

        private static void WriteToSystemBuffer(Vector3 v3)
        {
            var str = $"{v3.x:F4},{v3.y:F4},{v3.z:F4}";
            Debug.Log(str);
            EditorGUIUtility.systemCopyBuffer = str;
        }

        private static void WriteToSystemBufferU(Vector3 v3)
        {
            var str = $"Vector3({v3.x:F4},{v3.y:F4},{v3.z:F4})";
            Debug.Log(str);
            EditorGUIUtility.systemCopyBuffer = str;
        }
    }
}