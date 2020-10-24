using System.IO;
using UnityEditor;
using UnityEngine;

namespace GF47Editor
{
    public class GF47GatherAssets : ScriptableObject
    {
        [MenuItem("Assets/GF47 Editor/Gather Asset To &g", false , 0)]
        static void GatherAssets()
        {
            Caching.ClearCache();

            Object[] selectedAssets = Selection.GetFiltered(typeof(Object), SelectionMode.TopLevel);

            string sourceDir = Application.dataPath.Replace('/', '\\');
            sourceDir = sourceDir.Substring(0, sourceDir.Length - 7);
            string targetDir = EditorUtility.OpenFolderPanel("复制到", Application.dataPath, string.Empty);
            targetDir = targetDir.Replace('/', '\\');

            if (string.IsNullOrEmpty(targetDir))
            {
                Debug.Log("取消复制操作");
                return;
            }
            bool retainDirectory = EditorUtility.DisplayDialog("是否保留目录结构", "是否选择保留目录结构，如果选择否，则文件被收集到刚选择的目录下", "是", "否");

            for (int i = 0; i < selectedAssets.Length; i++)
            {
                if (AssetDatabase.Contains(selectedAssets[i]))
                {
                    string assetFileNameRelative = AssetDatabase.GetAssetPath(selectedAssets[i]).Replace('/', '\\');
                    string assetFileName = assetFileNameRelative.Substring(assetFileNameRelative.LastIndexOf('\\') + 1);
                    string sourceFile = string.Format("{0}\\{1}", sourceDir, assetFileNameRelative);
                    string targetFile = string.Format("{0}\\{1}", targetDir, retainDirectory ? assetFileNameRelative : assetFileName);
                    string targetDirNew = Path.GetDirectoryName(targetFile);
                    if (!string.IsNullOrEmpty(targetDirNew) && !Directory.Exists(targetDirNew))
                    {
                        Directory.CreateDirectory(targetDirNew);
                    }
                    if (File.Exists(sourceFile) && !File.Exists(targetFile))
                    {
                        File.Copy(sourceFile, targetFile);
                        if (File.Exists(sourceFile + ".meta"))
                        {
                            File.Copy(sourceFile + ".meta", targetFile + ".meta");
                        }
                    }
                }
            }
        }
    }
}
