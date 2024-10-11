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
            if (GUILayout.Button("Copy", GUILayout.Width(100f))) { WriteToSystemBuffer(a); }
            if (GUILayout.Button("Paste", GUILayout.Width(100f))) { a = CopyFromSystemBuffer(a); }
            GUILayout.EndHorizontal();

            if (GUILayout.Button("U Copy", GUILayout.Width(200f))) { WriteToSystemBufferU(a); }

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("View Target", GUILayout.Width(100f))) { a = view.pivot; }
            if (GUILayout.Button("Object Pivot", GUILayout.Width(100f))) { a = Selection.activeTransform.position; }
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
            #endregion

            #region B
            GUILayout.BeginVertical(EditorStyles.textArea, GUILayout.Width(200f));
            b = EditorGUILayout.Vector3Field("B", b, GUILayout.Width(200f));

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Copy", GUILayout.Width(100f))) { WriteToSystemBuffer(b); }
            if (GUILayout.Button("Paste", GUILayout.Width(100f))) { b = CopyFromSystemBuffer(b); }
            GUILayout.EndHorizontal();

            if (GUILayout.Button("U Copy", GUILayout.Width(200f))) { WriteToSystemBufferU(b); }

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("View Target", GUILayout.Width(100f))) { b = view.pivot; }
            if (GUILayout.Button("Object Pivot", GUILayout.Width(100f))) { b = Selection.activeTransform.position; }
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
            #endregion

            EditorGUIUtility.labelWidth = defaultLabelWidth;

            GUILayout.Space(5);
            if (GUILayout.Button("A = View Camera\nB = View Camera Target", GUILayout.Width(200f)))
            {
                a = view.camera.transform.position;
                b = view.pivot;
            }
            GUILayout.Space(5);

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

            Handles.EndGUI();

            a = Handles.PositionHandle(a, Quaternion.identity);
            b = Handles.PositionHandle(b, Quaternion.identity);

            Handles.color = Color.green;
            Handles.DrawLine(a, b);
            Handles.color = Color.white;

            Handles.Label(0.5f * (a + b), $"a:{a}\nb:{b}\nLenght:{(b - a).magnitude}");
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