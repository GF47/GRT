//************************************************************//
//      Author      :       GF47
//      DataTime    :       2013/12/9 星期一 17:20:09
//      Edited      :       2013/12/9 星期一 17:20:09
//************************************************************//

using UnityEditor;
using UnityEngine;

namespace GF47Editor
{
    /// <summary>
    /// Author          :GF47
    /// DataTime        :2013/12/9 星期一 17:20:09
    /// [GF47SkyBoxGenerator] Introduction  :Nothing to introduce
    /// </summary>
    public class GF47SkyBoxGenerator : EditorWindow
    {
        private Transform _renderFromPosition;
        private float _farClip = 1001f;
        private int _imageSize = 512;
        private bool _includeCurrentSkyBox = true;
        private Color _skyColor;
        private bool _inverse;

        private readonly Vector3[] _direction =
        {
            new Vector3(0,0,0), 
            new Vector3(0,-90,0),
            new Vector3(0,180,0),
            new Vector3(0,90,0),
            new Vector3(-90,0,0),
            new Vector3(90,0,0)
        };
        private readonly string[] _imageName =
        {
            "front",
            "right",
            "back",
            "left",
            "up",
            "down"
        };

        private readonly string[] _inverseImageName =
        {
            "front",
            "left",
            "back",
            "right",
            "up",
            "down"
        };

        [MenuItem("Tools/GF47 Editor/Render/Generator SkyBox")]
        static void Init()
        {
            GF47SkyBoxGenerator window = GetWindow<GF47SkyBoxGenerator>();
            window.position = new Rect(200, 200, 400, 400);
            window.Show();
        }

        void OnGUI()
        {
            _renderFromPosition = EditorGUILayout.ObjectField("相机位置", _renderFromPosition, typeof(Transform), true) as Transform;
            _farClip = EditorGUILayout.FloatField("远裁切值", _farClip);
            _imageSize = EditorGUILayout.IntField("图片尺寸", _imageSize);
            _includeCurrentSkyBox = EditorGUILayout.Toggle("包括当前天空球", _includeCurrentSkyBox);
            _inverse = EditorGUILayout.Toggle("从天空球外向内看", _inverse);
            if (!_includeCurrentSkyBox)
            {
                _skyColor = EditorGUILayout.ColorField("天空颜色", _skyColor);
            }
            if (_renderFromPosition != null)
            {
                if (GUILayout.Button(new GUIContent("生成", "在Assets/SkyBox文件夹中产生六张图片，分别对应了六个方向")))
                {
                    GameObject go = new GameObject("_skyBoxCamera", typeof(Camera));
                    if (_includeCurrentSkyBox)
                    {
                        go.GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;
                    }
                    else
                    {
                        go.GetComponent<Camera>().backgroundColor = _skyColor;
                    }
                    go.GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;
                    go.GetComponent<Camera>().fieldOfView = 90;
                    go.GetComponent<Camera>().aspect = 1.0f;
                    go.GetComponent<Camera>().farClipPlane = _farClip;
                    go.transform.position = _renderFromPosition.position;
                    if (_renderFromPosition.GetComponent<Renderer>() != null)
                    {
                        go.transform.position = _renderFromPosition.GetComponent<Renderer>().bounds.center;
                    }
                    go.transform.rotation = Quaternion.identity;
                    RenderSkyImage(0, go);
                    RenderSkyImage(1, go);
                    RenderSkyImage(2, go);
                    RenderSkyImage(3, go);
                    RenderSkyImage(4, go);
                    RenderSkyImage(5, go);

                    DestroyImmediate(go);
                }
            }
        }

        private void RenderSkyImage(int index, GameObject go)
        {
            go.transform.eulerAngles = _direction[index];
            RenderTexture rt = new RenderTexture(_imageSize, _imageSize, 24);
            go.GetComponent<Camera>().targetTexture = rt;
            Texture2D screenShot = new Texture2D(_imageSize, _imageSize, TextureFormat.RGB24, false);
            go.GetComponent<Camera>().Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, _imageSize, _imageSize), 0, 0);
            if (_inverse)//将天空球贴图转化为cubeMap贴图
            {
                for (int y = 0,yMax = _imageSize; y < yMax; y++)
                {
                    for (int x = 0,xMax = _imageSize - 1, loopTime = _imageSize / 2; x <= loopTime; x++)
                    {
                        Color swap = screenShot.GetPixel(x, y);
                        screenShot.SetPixel(x, y, screenShot.GetPixel(xMax - x, y));
                        screenShot.SetPixel(xMax - x, y, swap);
                    }
                }
                screenShot.Apply();
            }

            RenderTexture.active = null;
            DestroyImmediate(rt);
            byte[] bytes = screenShot.EncodeToPNG();
            string directory = Application.dataPath + "/SkyBoxes";
            if (!System.IO.Directory.Exists(directory))
            {
                System.IO.Directory.CreateDirectory(directory);
            }
            System.IO.File.WriteAllBytes(System.IO.Path.Combine(directory, (_inverse ? _inverseImageName[index] : _imageName[index]) + ".png"), bytes);
        }
    }
}
