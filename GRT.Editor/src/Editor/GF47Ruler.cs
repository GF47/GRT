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

            if (GUILayout.Button("Delete Ruler", GUILayout.Width(200f)))
            {
                SceneView.duringSceneGui -= SceneView_duringSceneGui;
                isActive = false;
            }

            if (GUILayout.Button("A = View Camera & B = View Camera Target", GUILayout.Width(200f)))
            {
                a = view.camera.transform.position;
                b = view.pivot;
            }

            if (GUILayout.Button("Place A at view target", GUILayout.Width(200f))) { a = view.pivot; }

            GUILayout.BeginHorizontal();
            a = new Vector3(
                EditorGUILayout.FloatField(a.x, GUILayout.Width(56f)),
                EditorGUILayout.FloatField(a.y, GUILayout.Width(56f)),
                EditorGUILayout.FloatField(a.z, GUILayout.Width(56f)));
            if (GUILayout.Button("P", GUILayout.Width(22)))
            {
                var astr = a.ToString("F4");
                Debug.Log(astr);
                EditorGUIUtility.systemCopyBuffer = astr;
            }
            GUILayout.EndHorizontal();

            if (GUILayout.Button("Place B at view target", GUILayout.Width(200f))) { b = view.pivot; }

            GUILayout.BeginHorizontal();
            b = new Vector3(
                EditorGUILayout.FloatField(b.x, GUILayout.Width(56f)),
                EditorGUILayout.FloatField(b.y, GUILayout.Width(56f)),
                EditorGUILayout.FloatField(b.z, GUILayout.Width(56f)));
            if (GUILayout.Button("P", GUILayout.Width(22)))
            {
                var bstr = b.ToString("F4");
                Debug.Log(bstr);
                EditorGUIUtility.systemCopyBuffer = bstr;
            }
            GUILayout.EndHorizontal();

            if (GUILayout.Button("Get Length", GUILayout.Width(200f)))
            {
                var length = (b - a).magnitude.ToString();
                Debug.Log(length);
                EditorGUIUtility.systemCopyBuffer = length;
            }

            if (GUILayout.Button("Get A -> B Offset", GUILayout.Width(200f)))
            {
                var offset = (b - a).ToString("F4");
                Debug.Log(offset);
                EditorGUIUtility.systemCopyBuffer = offset;
            }

            Handles.EndGUI();

            a = Handles.PositionHandle(a, Quaternion.identity);
            b = Handles.PositionHandle(b, Quaternion.identity);

            Handles.color = Color.green;
            Handles.DrawLine(a, b);
            Handles.color = Color.white;

            Handles.Label(0.5f * (a + b), $"a:{a}\nb:{b}\nLenght:{(b - a).magnitude}");
        }
    }
}