using UnityEditor;
using UnityEngine;

namespace GRT.Editor
{
    public class GF47PopupInspector : ScriptableObject
    {
        [MenuItem("GameObject/GF47 Editor/Popup Inspector _i")]
        private static void Popup()
        {
            var go = Selection.activeGameObject;
            if (go != null)
            {
                var type = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.InspectorWindow");
                var window = EditorWindow.GetWindow(type);

                window = EditorWindow.Instantiate(window);
                window.minSize = new Vector2(400f, 300f);
                window.position = new Rect(200f, 200f, 400f, 300f);
                window.ShowUtility();

                type.GetProperty("isLocked").SetValue(window, true);
            }
        }
    }
}