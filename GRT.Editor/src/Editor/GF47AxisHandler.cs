using UnityEditor;
using UnityEngine;

namespace LK.Assets._3rd.GF47.GRT.Editor
{
    public class GF47AxisHandler : ScriptableObject
    {
        private static bool isActive;
        private static Vector3 _offset;
        private static Quaternion _rot;

        [MenuItem("Tools/GF47 Editor/Transform/AxisHandler #&h")]
        private static void Init()
        {
            if (isActive)
            {
                SceneView.duringSceneGui -= DrawHandler;
                _offset = Vector3.zero;
            }
            else
            {
                SceneView.duringSceneGui += DrawHandler;
                _offset = SceneView.lastActiveSceneView.pivot;
            }
            isActive = !isActive;
        }

        private static void DrawHandler(SceneView view)
        {
            var offset = Handles.PositionHandle(_offset, _rot);
            var delta = offset - _offset;
            _offset = offset;

            var selection = Selection.transforms;

            if (selection != null)
            {
                for (int i = 0; i < selection.Length; i++)
                {
                    selection[i].position += delta;
                }
            }

            #region gui

            Handles.BeginGUI();
            {
                _offset = EditorGUILayout.Vector3Field("origin", _offset, GUILayout.Width(300f));
                _rot = Quaternion.Euler(EditorGUILayout.Vector3Field("rotation", _rot.eulerAngles, GUILayout.Width(300f)));
            }
            Handles.EndGUI();

            #endregion gui
        }
    }
}