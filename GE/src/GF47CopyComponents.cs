/* ****************************************************************
 * @File Name   :   GF47CopyComponents
 * @Author      :   GF47
 * @Date        :   2015/6/2 14:50:52
 * @Description :   to do
 * @Edit        :   2015/6/2 14:50:52
 * ***************************************************************/

using UnityEditor;
using UnityEngine;

namespace GF47Editor
{
    /// <summary>
    /// A example class in Unity
    /// </summary>
    public class GF47CopyComponents : EditorWindow
    {
        private static MeshFilter _meshFilter;
        private static Renderer _renderer;

        [MenuItem("Tools/GF47 Editor/复制粘贴渲染相关的组件")]
        static void Init()
        {
            var window = GetWindow<GF47CopyComponents>();
            window.position = new Rect(200f, 200f, 400f, 250f);
            window.minSize = new Vector2(300f, 25f);
            window.autoRepaintOnSceneChange = true;
            window.titleContent = new GUIContent("复制粘贴渲染相关的组件");
            window.Show();
        }

        void OnGUI()
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("复制",GUILayout.Width(100f)))
            {
                ComponentsRecord();
            }
            if (GUILayout.Button("粘贴", GUILayout.Width(100f)))
            {
                ComponentsApplay();
            }
            GUILayout.EndHorizontal();
        }

        static void ComponentsRecord()
        {
            GameObject target = Selection.activeGameObject;
            if (target == null) return;

            _meshFilter = target.GetComponent<MeshFilter>();
            _renderer = target.GetComponent<Renderer>();
        }

        static void ComponentsApplay()
        {
            GameObject[] targets = Selection.gameObjects;
            for (int i = 0; i < targets.Length; i++)
            {
                MeshFilter filter = targets[i].GetComponent<MeshFilter>();
                if (filter == null)
                {
                    filter = targets[i].AddComponent<MeshFilter>();
                }
                filter.sharedMesh = _meshFilter.sharedMesh;

                Renderer renderer = targets[i].GetComponent<Renderer>();
                if (renderer == null)
                {
                    renderer = targets[i].AddComponent<MeshRenderer>();
                }
                renderer.shadowCastingMode = _renderer.shadowCastingMode;
                renderer.receiveShadows = _renderer.receiveShadows;
                renderer.sharedMaterials = _renderer.sharedMaterials;
            }
        }
    }
}
