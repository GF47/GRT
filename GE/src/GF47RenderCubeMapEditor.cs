using UnityEditor;
using UnityEngine;

namespace GF47Editor
{
    public class GF47RenderCubeMapEditor : EditorWindow
    {
        private Transform _renderFromPosition;
        private Cubemap _cubeMap;
        private Material _theSkyBoxMaterial;

        [MenuItem("Tools/GF47 Editor/Render/Render Into CubeMap")]
        static void Init()
        {
            GF47RenderCubeMapEditor window = GetWindow<GF47RenderCubeMapEditor>();
            window.position = new Rect(200, 200, 400, 400);
            window.Show();
        }

        void OnGUI()
        {
            _renderFromPosition = EditorGUILayout.ObjectField("相机位置",_renderFromPosition, typeof(Transform),true) as Transform;
            _cubeMap = EditorGUILayout.ObjectField("立方体环境贴图",_cubeMap, typeof(Cubemap), true) as Cubemap;
            _theSkyBoxMaterial = EditorGUILayout.ObjectField("天空球材质",_theSkyBoxMaterial, typeof(Material), true) as Material;
            if (_renderFromPosition != null && _cubeMap != null)
            {
                if (GUILayout.Button(new GUIContent("Render", "click to render the cubemap at the transform's position")))
                {
                    GameObject go = new GameObject("_CubeMapCamera", typeof(Camera));
                    go.GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;
	
                    if (!go.GetComponent<Skybox>())
                    {
                        go.AddComponent<Skybox>();
                    }
	
                    go.GetComponent<Skybox>().material = _theSkyBoxMaterial;
	
                    go.transform.position = _renderFromPosition.position;
                    if (_renderFromPosition.GetComponent<Renderer>())
                    {
                        go.transform.position = _renderFromPosition.GetComponent<Renderer>().bounds.center;
                    }
	
                    go.transform.rotation = Quaternion.identity;
	
                    go.GetComponent<Camera>().fieldOfView = 90f;
                    go.GetComponent<Camera>().aspect = 1f;
	
                    go.GetComponent<Camera>().RenderToCubemap(_cubeMap);
	
                    DestroyImmediate(go);
                }
            }
        }
    }
}
