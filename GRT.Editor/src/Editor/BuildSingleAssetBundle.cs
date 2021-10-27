using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

namespace GRT.Editor
{
    public class BuildSingleAssetBundle
    {
        private static string _directory;

        [MenuItem("Assets/Build AB")]
        private static void Build()
        {
            var name = Selection.activeObject.name;
            var assets = AssetDatabase.GetDependencies(AssetDatabase.GetAssetPath(Selection.activeObject));

            if (assets == null)
            {
                Debug.LogWarning("请选择打包资源");
                return;
            }

            assets = (from item in assets
                      where !item.StartsWith("Packages")
                         && !item.EndsWith(".cs")
                         && !item.EndsWith(".shader")
                      select item).ToArray();

            if (assets.Length < 1)
            {
                Debug.LogWarning("请选择打包资源");
                return;
            }

            var path = EditorUtility.SaveFilePanel("Select Asset Path", _directory ?? Application.streamingAssetsPath, name, "uab");

            _directory = Path.GetDirectoryName(path);

            AssetBundleBuild build = new AssetBundleBuild();

            build.assetNames = assets;
            build.assetBundleName = Path.GetFileName(path).ToLower();

            var manifest = BuildPipeline.BuildAssetBundles(_directory, new AssetBundleBuild[] { build }, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows64);

            EditorGUIUtility.systemCopyBuffer = build.assetBundleName;
        }
    }
}
