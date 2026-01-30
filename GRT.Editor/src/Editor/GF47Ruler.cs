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
            if (GUILayout.Button("As ViewTarget", GUILayout.Width(99f))) { view.pivot = a; }
            if (GUILayout.Button("As ViewCamera", GUILayout.Width(98f))) { view.camera.transform.position = a; }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("As GO_Pivot", GUILayout.Width(99f))) { Selection.activeTransform.position = a; }
            if (GUILayout.Button("As GO_Center", GUILayout.Width(98f)))
            {
                var go = Selection.activeGameObject;
                var objectCenter = CalculateGameObjectCenter(go).center;
                go.transform.position = a - objectCenter + go.transform.position;
            }
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
            if (GUILayout.Button("As ViewTarget", GUILayout.Width(99f))) { view.pivot = b; }
            if (GUILayout.Button("As ViewCamera", GUILayout.Width(98f))) { view.camera.transform.position = b; }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("As GO_Pivot", GUILayout.Width(99f))) { Selection.activeTransform.position = b; }
            if (GUILayout.Button("As GO_Center", GUILayout.Width(98f)))
            {
                var go = Selection.activeGameObject;
                var objectCenter = CalculateGameObjectCenter(go).center;
                go.transform.position = b - objectCenter + go.transform.position;
            }
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();

            #endregion B

            #region Center

            var center = (a + b) / 2f;

            GUILayout.BeginVertical(EditorStyles.textArea, GUILayout.Width(200f));
            GUILayout.Label($"Center X:{center.x:F3}, Y:{center.y:F3}, Z:{center.z:F3}");

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Copy", GUILayout.Width(65f))) { WriteToSystemBuffer(center); }
            if (GUILayout.Button("U Copy", GUILayout.Width(65f))) { WriteToSystemBufferU(center); }
            if (GUILayout.Button("Paste", GUILayout.Width(64f))) { b = CopyFromSystemBuffer(center) * 2f - a; }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("As ViewTarget", GUILayout.Width(99f))) { view.pivot = center; }
            if (GUILayout.Button("As ViewCamera", GUILayout.Width(98f))) { view.camera.transform.position = center; }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("As GO_Pivot", GUILayout.Width(99f))) { Selection.activeTransform.position = center; }
            if (GUILayout.Button("As GO_Center", GUILayout.Width(98f)))
            {
                var go = Selection.activeGameObject;
                var objectCenter = CalculateGameObjectCenter(go).center;
                go.transform.position = center - objectCenter + go.transform.position;
            }
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();

            #endregion Center

            Vector3 position;

            #region View Target

            GUILayout.BeginVertical(EditorStyles.textArea, GUILayout.Width(200f));
            position = view.pivot;
            GUILayout.Label("View Target");
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Copy", GUILayout.Width(48f))) { WriteToSystemBuffer(position); }
            if (GUILayout.Button("As A", GUILayout.Width(48f))) { a = position; }
            if (GUILayout.Button("As B", GUILayout.Width(48f))) { b = position; }
            if (GUILayout.Button("As C", GUILayout.Width(47f))) { b = position * 2f - a; }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            #endregion View Target

            #region View Camera

            GUILayout.BeginVertical(EditorStyles.textArea, GUILayout.Width(200f));
            position = view.camera.transform.position;
            GUILayout.Label("View Camera");
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Copy", GUILayout.Width(48f))) { WriteToSystemBuffer(position); }
            if (GUILayout.Button("As A", GUILayout.Width(48f))) { a = position; }
            if (GUILayout.Button("As B", GUILayout.Width(48f))) { b = position; }
            if (GUILayout.Button("As C", GUILayout.Width(47f))) { b = position * 2f - a; }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            #endregion View Camera

            if (Selection.activeGameObject != null)
            {
                #region Object Pivot

                GUILayout.BeginVertical(EditorStyles.textArea, GUILayout.Width(200f));
                position = Selection.activeTransform.position;
                GUILayout.Label("Object Pivot");
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Copy", GUILayout.Width(48f))) { WriteToSystemBuffer(position); }
                if (GUILayout.Button("As A", GUILayout.Width(48f))) { a = position; }
                if (GUILayout.Button("As B", GUILayout.Width(48f))) { b = position; }
                if (GUILayout.Button("As C", GUILayout.Width(47f))) { b = position * 2f - a; }
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();

                #endregion Object Pivot

                #region Object Center

                GUILayout.BeginVertical(EditorStyles.textArea, GUILayout.Width(200f));
                position = CalculateGameObjectCenter(Selection.activeGameObject).center;
                GUILayout.Label("Object Center");
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Copy", GUILayout.Width(48f))) { WriteToSystemBuffer(position); }
                if (GUILayout.Button("As A", GUILayout.Width(48f))) { a = position; }
                if (GUILayout.Button("As B", GUILayout.Width(48f))) { b = position; }
                if (GUILayout.Button("As C", GUILayout.Width(47f))) { b = position * 2f - a; }
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();

                #endregion Object Center
            }

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

        private static Bounds CalculateGameObjectCenter(GameObject go)
        {
            var max = Vector3.negativeInfinity;
            var min = Vector3.positiveInfinity;

            var hasRenderer = false;
            var hasCollider = false;

            var renderers = go.GetComponentsInChildren<Renderer>();
            if (renderers != null && renderers.Length > 0)
            {
                hasRenderer = true;
                for (int i = 0; i < renderers.Length; i++)
                {
                    max = Vector3.Max(max, renderers[i].bounds.max);
                    min = Vector3.Min(min, renderers[i].bounds.min);
                }
            }

            var colliders = go.GetComponentsInChildren<Collider>();
            if (colliders != null && colliders.Length > 0)
            {
                hasCollider = true;
                for (int i = 0; i < colliders.Length; i++)
                {
                    max = Vector3.Max(max, colliders[i].bounds.max);
                    min = Vector3.Min(min, colliders[i].bounds.min);
                }
            }

            if (hasRenderer || hasCollider)
            {
                var center = (max + min) / 2f;
                var size = max - min;
                size = new Vector3(Mathf.Abs(size.x), Mathf.Abs(size.y), Mathf.Abs(size.z));

                return new Bounds(center, size);
            }
            else
            {
                return new Bounds(go.transform.position, Vector3.zero);
            }
        }
    }
}