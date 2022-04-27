using UnityEditor;
using UnityEngine;

namespace GRT.Editor
{
    public class GF47AlignWithSurface : ScriptableObject
    {
        private static bool _active;

        private static Vector3 _pos;
        private static Vector3 _normal = Vector3.forward;
        private static Vector3 _up = Vector3.up;

        private static bool _picking;

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
            GUILayout.BeginArea(new Rect(0f, 0f, 210f, 80f), EditorStyles.textArea);
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Align With Surface", EditorStyles.boldLabel, GUILayout.Width(172f));
                    if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(20f)))
                    {
                        _active = false;
                        SceneView.duringSceneGui -= SceneView_duringSceneGui;
                    }
                }
                GUILayout.EndHorizontal();

                GUILayout.Label("Press [Left Ctrl] to pick a location");

                if (GUILayout.Button("Align", GUILayout.Width(200f)))
                {
                    var target = Selection.activeTransform;
                    if (target != null)
                    {
                        Undo.RecordObject(target, "align with surface");
                        target.position = _pos;
                        target.rotation = Quaternion.LookRotation(-_normal, _up); // 法线的反方向
                        EditorUtility.SetDirty(target);
                    }
                }
            }
            GUILayout.EndArea();
            Handles.EndGUI();

            // HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
            if (Event.current.keyCode == KeyCode.LeftControl)
            {
                if (Event.current.type == EventType.KeyDown)
                {
                    _picking = true;
                }
                else if (Event.current.type == EventType.KeyUp)
                {
                    _picking = false;
                }
            }

            if (_picking)
            {
                var pos = Event.current.mousePosition;
                pos.y = scene.camera.pixelHeight - pos.y;
                var ray = scene.camera.ScreenPointToRay(pos);
                if (Physics.Raycast(ray, out var hit))
                {
                    _pos = hit.point;
                    _normal = hit.normal;
                    _up = hit.transform.up;
                    // scene.Repaint(); // handles已经repaint了，应该不需要了
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