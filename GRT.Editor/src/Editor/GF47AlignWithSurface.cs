using UnityEditor;
using UnityEngine;
using System.Reflection;

namespace GRT.Editor
{
    public class GF47AlignWithSurface : ScriptableObject
    {
        private static bool _active;

        private static Vector3 _pos;
        private static Vector3 _normal = Vector3.forward;
        private static Vector3 _up = Vector3.up;

        private static readonly MethodInfo _IntersectRayMeshImpl;
        private static readonly object[] _IntersectRayMeshParameters;
        public static bool IntersectRayMesh(Ray ray, Mesh mesh, Matrix4x4 matrix, out RaycastHit hit)
        {
            _IntersectRayMeshParameters[0] = ray;
            _IntersectRayMeshParameters[1] = mesh;
            _IntersectRayMeshParameters[2] = matrix;
            _IntersectRayMeshParameters[3] = null;

            var result = (bool)_IntersectRayMeshImpl.Invoke(null, _IntersectRayMeshParameters);
            hit = (RaycastHit)_IntersectRayMeshParameters[3];
            return result;
        }

        static GF47AlignWithSurface()
        {
            _IntersectRayMeshImpl = typeof(HandleUtility).GetMethod("IntersectRayMesh", BindingFlags.Static | BindingFlags.NonPublic);
            _IntersectRayMeshParameters = new object[4];
        }

        [MenuItem("Tools/GF47 Editor/Transform/Align With Surface")]
        private static void Init()
        {
            _active = !_active;
            SceneView.duringSceneGui -= SceneView_duringSceneGui;
            if (_active)
            {
                SceneView.duringSceneGui += SceneView_duringSceneGui;
            }
        }

        private static void SceneView_duringSceneGui(SceneView scene)
        {
            Handles.BeginGUI();
            GUILayout.BeginArea(new Rect(0f, 0f, 256f, 100f), EditorStyles.textArea);
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Align With Surface", EditorStyles.boldLabel, GUILayout.Width(226f));
                    if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(20f)))
                    {
                        _active = false;
                        SceneView.duringSceneGui -= SceneView_duringSceneGui;
                    }
                }
                GUILayout.EndHorizontal();

                EditorGUILayout.HelpBox(@"Press [Left Ctrl] to pick a point on the seleced object, Select another object and click the [ Align ] button to put the object on the point", MessageType.Info);

                if (GUILayout.Button("Align", GUILayout.Width(250)))
                {
                    var target = Selection.activeTransform;
                    if (target != null)
                    {
                        Undo.RecordObject(target, "align with surface");
                        target.SetPositionAndRotation(_pos, Quaternion.LookRotation(-_normal, _up)); // 法线的反方向
                        EditorUtility.SetDirty(target);
                    }
                }
            }
            GUILayout.EndArea();
            Handles.EndGUI();

            // HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
            if (Event.current.control && Selection.activeGameObject != null)
            {
                var pos = Event.current.mousePosition;
                pos.y = scene.camera.pixelHeight - pos.y;
                var ray = scene.camera.ScreenPointToRay(pos);

                var meshFilter = Selection.activeGameObject.GetComponent<MeshFilter>();
                if (meshFilter != null
                    && meshFilter.sharedMesh != null
                    && IntersectRayMesh(ray, meshFilter.sharedMesh, Selection.activeGameObject.transform.localToWorldMatrix, out var hit))
                {
                    _pos = hit.point;
                    _normal = hit.normal;
                    _up = Selection.activeGameObject.transform.up;
                }
            }

            var size = HandleUtility.GetHandleSize(_pos);

            Handles.color = Color.yellow;
            Handles.DotHandleCap(0, _pos, Quaternion.identity, 0.05f * size, EventType.Repaint);
            Handles.color = Color.green;
            Handles.CircleHandleCap(0, _pos, Quaternion.LookRotation(_normal, Vector3.up), size, EventType.Repaint);
            Handles.color = Color.blue;
            Handles.ArrowHandleCap(0, _pos, Quaternion.LookRotation(_normal, Vector3.up), size, EventType.Repaint);
        }
    }
}