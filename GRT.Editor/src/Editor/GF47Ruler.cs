using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;

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

            if (GUILayout.Button("Delete Ruler", GUILayout.Width(200f))) { SceneView.duringSceneGui -= SceneView_duringSceneGui; }

            if (GUILayout.Button("Put A", GUILayout.Width(200f))) { a = view.pivot; }

            GUILayout.BeginHorizontal();
            a = new Vector3(
                EditorGUILayout.FloatField(a.x, GUILayout.Width(56f)),
                EditorGUILayout.FloatField(a.y, GUILayout.Width(56f)),
                EditorGUILayout.FloatField(a.z, GUILayout.Width(56f)));
            if (GUILayout.Button("P", GUILayout.Width(22))) { Debug.Log(a); }
            GUILayout.EndHorizontal();

            if (GUILayout.Button("Put B", GUILayout.Width(200f))) { b = view.pivot; }

            GUILayout.BeginHorizontal();
            b = new Vector3(
                EditorGUILayout.FloatField(b.x, GUILayout.Width(56f)),
                EditorGUILayout.FloatField(b.y, GUILayout.Width(56f)),
                EditorGUILayout.FloatField(b.z, GUILayout.Width(56f)));
            if (GUILayout.Button("P", GUILayout.Width(22))) { Debug.Log(b); }
            GUILayout.EndHorizontal();

            if (GUILayout.Button("Length", GUILayout.Width(200f))) 
            {
                var length = (b - a).magnitude.ToString();
                Debug.Log(length);
                EditorGUIUtility.systemCopyBuffer = length;
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
