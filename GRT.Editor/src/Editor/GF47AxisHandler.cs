using UnityEditor;
using UnityEngine;

namespace LK.Assets._3rd.GF47.GRT.Editor
{
    public class GF47AxisHandler : ScriptableObject
    {
        private static bool isActive;
        private static Vector3 _offset;

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
            var offset = Handles.PositionHandle(_offset, Quaternion.identity);
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
        }
    }
}