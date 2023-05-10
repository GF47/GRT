using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GRT.Editor
{
    public class TextureCompresser : EditorWindow
    {
        private enum Size
        {
            Size_Half = 0,
            Size_32 = 32,
            Size_64 = 64,
            Size_128 = 128,
            Size_256 = 256,
            Size_512 = 512,
            Size_1024 = 1024,
            Size_2048 = 2048,
            Size_4096 = 4096,
            Size_8192 = 8192,
        }

        private readonly Dictionary<BuildTarget, TextureImporterFormat[]> _bestTextureFormats = new Dictionary<BuildTarget, TextureImporterFormat[]>()
        {
            {
                BuildTarget.Android, new TextureImporterFormat[]
                {
                    TextureImporterFormat.ETC_RGB4Crunched,
                    TextureImporterFormat.ETC_RGB4,
                    TextureImporterFormat.ETC2_RGBA8Crunched,
                    TextureImporterFormat.ASTC_6x6
                }
            },
            {
                BuildTarget.WebGL, new TextureImporterFormat[]
                {
                    TextureImporterFormat.DXT1Crunched,
                    TextureImporterFormat.DXT1,
                    TextureImporterFormat.DXT5Crunched,
                    TextureImporterFormat.DXT5
                }
            },
            {
                BuildTarget.StandaloneWindows64, new TextureImporterFormat[]
                {
                    TextureImporterFormat.DXT1Crunched,
                    TextureImporterFormat.DXT1,
                    TextureImporterFormat.DXT5Crunched,
                    TextureImporterFormat.DXT5
                }
            },
        };

        private TextureImporterFormat GetBestFormat(BuildTarget buildTarget, bool alpha, bool crunch)
            => _bestTextureFormats[buildTarget][(crunch ? 0 : 1) + (alpha ? 2 : 0)];

        private Texture[] _textures;
        private int _indicator;

        private bool _overrideMaxSize;
        private bool _overrideFormat;
        private bool _useCrunchCompression;

        private Size _maxSize = Size.Size_1024;

        [MenuItem("Tools/GF47 Editor/Texture Compresser")]
        private static void Init()
        {
            var window = GetWindow<TextureCompresser>();
            window.minSize = new Vector2(600, 200);
            window.Show();
        }

        private void OnGUI()
        {
            var selected = _textures != null && _textures.Length > 0;

            var defaultColor = GUI.color;

            if (selected) { GUI.color = Color.green; }
            else { GUI.color = Color.yellow; }
            if (GUILayout.Button(selected ? $"Select {_textures.Length} Textures" : "Select Textures"))
            {
                _textures = Selection.GetFiltered<Texture>(SelectionMode.DeepAssets);
                if (_textures.Length > 0)
                {
                    Selection.activeObject = _textures[0];
                }
            }
            GUI.color = defaultColor;

            if (selected)
            {
                _indicator = Mathf.Clamp(_indicator, 0, _textures.Length - 1);
                var texture = _textures[_indicator];
                var path = AssetDatabase.GetAssetPath(texture);
                var importer = AssetImporter.GetAtPath(path) as TextureImporter;
                EditorGUILayout.HelpBox($"Current: {_indicator}\n\tPath: {path}\n\tSize: {texture.width},{texture.height}", MessageType.Info);

                EditorGUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("<--", EditorStyles.miniButtonLeft))
                    {
                        _indicator = Mathf.Clamp(--_indicator, 0, _textures.Length - 1);
                        if (_textures.Length > 0)
                        {
                            Selection.activeObject = _textures[_indicator];
                        }
                    }
                    if (GUILayout.Button("-->", EditorStyles.miniButtonRight))
                    {
                        _indicator = Mathf.Clamp(++_indicator, 0, _textures.Length - 1);
                        if (_textures.Length > 0)
                        {
                            Selection.activeObject = _textures[_indicator];
                        }
                    }
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space(16);

                // max size
                EditorGUILayout.BeginHorizontal();
                {
                    _overrideMaxSize = EditorGUILayout.ToggleLeft("Max Size", _overrideMaxSize, GUILayout.Width(160));
                    EditorGUI.BeginDisabledGroup(!_overrideMaxSize);
                    {
                        _maxSize = (Size)EditorGUILayout.EnumPopup(_maxSize);
                    }
                    EditorGUI.EndDisabledGroup();
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    // format
                    _overrideFormat = EditorGUILayout.ToggleLeft("Best Format", _overrideFormat, GUILayout.Width(160));

                    EditorGUI.BeginDisabledGroup(!_overrideFormat);
                    {
                        // crunch compression
                        _useCrunchCompression = EditorGUILayout.ToggleLeft("Use Crunch Compression", _useCrunchCompression, GUILayout.Width(160));
                    }
                    EditorGUI.EndDisabledGroup();
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space(12);

                EditorGUILayout.BeginHorizontal();
                {
                    GUI.color = Color.red;
                    if (GUILayout.Button("Set All", GUILayout.Height(40)))
                    {
                        for (int i = 0; i < _textures.Length; i++)
                        {
                            var tex = _textures[i];
                            if (tex != null)
                            {
                                EditorUtility.DisplayProgressBar("Reimport selected textures", $"Current: {i + 1}/{_textures.Length}", (i + 1) / (float)_textures.Length);
                                var texImporter = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(tex)) as TextureImporter;
                                Reimport(texImporter, tex);
                            }
                        }
                        EditorUtility.ClearProgressBar();
                        AssetDatabase.Refresh();
                    }
                    GUI.color = Color.yellow;
                    if (GUILayout.Button("Set", GUILayout.Height(40)))
                    {
                        Reimport(importer, texture);
                    }
                    GUI.color = defaultColor;
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        private void Reimport(TextureImporter importer, Texture texture)
        {
            var size = _maxSize == Size.Size_Half ? Mathf.Max(GetNearestPOTScale(texture) / 2, 32) : (int)_maxSize;
            if (_overrideMaxSize && importer.maxTextureSize > size)
            {
                importer.maxTextureSize = size;
            }

            if (_overrideFormat)
            {
                var alpha = importer.DoesSourceTextureHaveAlpha() || importer.textureType == TextureImporterType.NormalMap;

                var androidSettings = importer.GetPlatformTextureSettings("Android");
                if (androidSettings != null)
                {
                    androidSettings.overridden = true;
                    if (_overrideMaxSize && androidSettings.maxTextureSize > size) { androidSettings.maxTextureSize = size; }
                    androidSettings.format = GetBestFormat(BuildTarget.Android, alpha, _useCrunchCompression);
                    importer.SetPlatformTextureSettings(androidSettings);
                }

                var webglSettings = importer.GetPlatformTextureSettings("WebGL");
                if (webglSettings != null)
                {
                    webglSettings.overridden = true;
                    if (_overrideMaxSize && webglSettings.maxTextureSize > size) { webglSettings.maxTextureSize = size; }
                    webglSettings.format = GetBestFormat(BuildTarget.WebGL, alpha, _useCrunchCompression);
                    importer.SetPlatformTextureSettings(webglSettings);
                }

                var standaloneSettings = importer.GetPlatformTextureSettings("Standalone");
                if (standaloneSettings != null)
                {
                    standaloneSettings.overridden = true;
                    if (_overrideMaxSize && standaloneSettings.maxTextureSize > size) { standaloneSettings.maxTextureSize = size; }
                    standaloneSettings.format = GetBestFormat(BuildTarget.StandaloneWindows64, alpha, _useCrunchCompression);
                    importer.SetPlatformTextureSettings(standaloneSettings);
                }
            }

            importer.SaveAndReimport();
        }

        private static int GetNearestPOTScale(Texture texture)
        {
            var v = Mathf.Max(texture.width, texture.height);

            var a = 32;
            while (a <= 4096)
            {
                var b = a * 2;
                if (v < b)
                {
                    return (v < (a + b) / 2) ? a : b;
                }

                a = b;
            }
            return a;
        }
    }
}