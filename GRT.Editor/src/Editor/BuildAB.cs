/***************************************************************
 * @File Name       : BuildAB
 * @Author          : GF47
 * @Description     : 打包命令
 *
 * 先选中需要打包的资源
 * 设置所在包名，可以选择将依赖的资源一起设置
 * ! 目录以[@]开头的，视为目录所有内容都打到以这个目录为名的包里
 * ! 目录或资源名不能出现[空格、#]等字符，在3ds Max或Maya生成的资源名经常会有这些字符，请自行处理
 * 执行打包命令，在指定平台的目录中可以看到生成的包文件
 *
 * @Date            : 2017/7/19/星期三 14:22:10
 * @Edit            : none
 **************************************************************/

using GRT.AssetBundles_Old;
using SimpleJSON;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GRT.Editor
{
    public class BuildAB
    {
        private const string CHAR_COLLECT_SUBASSETS_TO_SINGLE_ASSETBUNDLE = "@";
        private const string AB_FILE_EXTENSION = "unity3d";
        private const int GUID_SUB_LENGTH = 5;

        private const string ASSETBUNDLES_ROOT_DIRECTORY = "AssetBundles";

        private static readonly Regex Regex = new Regex(@"[\s#\.]+");

        /// <summary>
        /// 设置包名
        /// </summary>
        [MenuItem("AssetBundles/Set AssetBundle Name", false, 0)]
        private static void SetABName()
        {
            Object[] objects = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
            if (objects == null || objects.Length < 1)
            {
                Debug.LogWarning("请选择打包资源或者目录");
                return;
            }
            foreach (Object obj in objects) { SetSingleABName(obj); }
        }

        /// <summary>
        /// 设置包名，连同依赖资源一起
        /// </summary>
        [MenuItem("AssetBundles/Set AssetBundle Name With Dependencies", false, 1)]
        private static void SetABNameWithDependencies()
        {
            Object[] objects = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
            if (objects == null || objects.Length < 1)
            {
                Debug.LogWarning("请选择打包资源或者目录");
                return;
            }

            foreach (Object obj in objects)
            {
                string[] dependencies = AssetDatabase.GetDependencies(AssetDatabase.GetAssetPath(obj));
                foreach (string dependency in dependencies)
                {
                    SetSingleABName(AssetDatabase.LoadMainAssetAtPath(dependency));
                }
            }
        }

        private static void SetSingleABName(Object obj)
        {
            if (ProjectWindowUtil.IsFolder(obj.GetInstanceID())) return;    // 1、asset是文件夹，不设置name
            if (obj is MonoScript) return;                                  // 2、asset是脚本，不设置name

            string path = AssetDatabase.GetAssetPath(obj);
            if (path.EndsWith(".dll")) return;                              // 3、asset是dll，不设置name

            AssetImporter importer = AssetImporter.GetAtPath(path);
            if (!string.IsNullOrEmpty(importer.assetBundleName)) return;    // 4、asset已经有了name，不设置name

            string assetbundleName, guid;

            if (path.Contains(CHAR_COLLECT_SUBASSETS_TO_SINGLE_ASSETBUNDLE)) // 带有指定字符开头的目录下的资源被打成一个包，包名为目录名
            {
                int charID = path.LastIndexOf(CHAR_COLLECT_SUBASSETS_TO_SINGLE_ASSETBUNDLE, StringComparison.Ordinal);
                string right = path.Substring(charID);
                int slashID = right.IndexOf('/');

                int length = charID;
                length += slashID > -1 ? slashID : right.Length;

                guid = AssetDatabase.AssetPathToGUID(path.Substring(0, length)).Substring(0, GUID_SUB_LENGTH);

                assetbundleName = path.Substring(7, length - 7);
            }
            else // 单文件打包
            {
                guid = AssetDatabase.AssetPathToGUID(path).Substring(0, GUID_SUB_LENGTH);

                assetbundleName = path.Substring(7);
            }

            assetbundleName = string.Format("{0}_{1}", assetbundleName, guid); // 添加guid前几位，避免命名冲突
            assetbundleName = Regex.Replace(assetbundleName, "_"); // 去除路径中的非法字符
            importer.assetBundleName = assetbundleName;
        }

        /// <summary>
        /// 忽略掉选中的资源
        /// </summary>
        [MenuItem("AssetBundles/Ignore", false, 20)]
        private static void IgnoreSelected()
        {
            Object[] objects = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
            foreach (Object obj in objects)
            {
                IgnoreSingleObjec(obj);
            }

            AssetDatabase.RemoveUnusedAssetBundleNames();
        }

        /// <summary>
        /// 忽略掉选中的资源，连同依赖资源一起
        /// </summary>
        [MenuItem("AssetBundles/Ignore With Dependencies", false, 21)]
        private static void IgnoreSelectedWithDependencies()
        {
            Object[] objects = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
            foreach (Object obj in objects)
            {
                string[] dependencies = AssetDatabase.GetDependencies(AssetDatabase.GetAssetPath(obj));
                foreach (string dependency in dependencies)
                {
                    IgnoreSingleObjec(AssetDatabase.LoadMainAssetAtPath(dependency));
                }
            }
            AssetDatabase.RemoveUnusedAssetBundleNames();
        }

        private static void IgnoreSingleObjec(Object obj)
        {
            if (obj is MonoScript) return;
            AssetImporter importer = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(obj));
            importer.assetBundleName = string.Empty;
        }

        [MenuItem("AssetBundles/Build Windows x86", false, 40)]
        private static void BuildAll_Windows_x86()
        {
            BuildAll(BuildTarget.StandaloneWindows);
        }

        [MenuItem("AssetBundles/Build Windows x86_64", false, 41)]
        private static void BuildAll_Windows_x86_64()
        {
            BuildAll(BuildTarget.StandaloneWindows64);
        }

        [MenuItem("AssetBundles/Build Android", false, 42)]
        private static void BuildAll_Android()
        {
            BuildAll(BuildTarget.Android);
        }

        [MenuItem("AssetBundles/Build iOS", false, 43)]
        private static void BuildAll_iOS()
        {
            if (BuildPipeline.IsBuildTargetSupported(BuildTargetGroup.iOS, BuildTarget.iOS))
            {
                BuildAll(BuildTarget.iOS);
            }
            else
            {
                Debug.LogWarning("iOS not supported at current platform");
            }
        }

        [MenuItem("AssetBundles/Build WebGL", false, 44)]
        private static void BuildAll_WebGL()
        {
            BuildAll(BuildTarget.WebGL);
        }

        private static void BuildAll(BuildTarget target)
        {
            Caching.ClearCache();

            string[] assetbundleNames = AssetDatabase.GetAllAssetBundleNames();
            AssetBundleBuild[] abBuilds = new AssetBundleBuild[assetbundleNames.Length];
            for (int i = 0; i < abBuilds.Length; i++)
            {
                AssetBundleBuild build = new AssetBundleBuild
                {
                    assetNames = AssetDatabase.GetAssetPathsFromAssetBundle(assetbundleNames[i]),
                    assetBundleVariant = string.Empty,// TODO 添加LOD时会需要，或者根据机型性能区分
                    assetBundleName = string.Format("{0}.{1}", assetbundleNames[i], AB_FILE_EXTENSION)
                };

                abBuilds[i] = build;
            }

            string outputPath = CreateABDirectory(target, Application.dataPath.Substring(0, Application.dataPath.Length - 7) + "/" + ASSETBUNDLES_ROOT_DIRECTORY);
            BuildPipeline.BuildAssetBundles(outputPath, abBuilds, BuildAssetBundleOptions.ChunkBasedCompression, target);
            AssetDatabase.Refresh();

            CopyAssetsMapFileTo(outputPath, target);
        }

        /// <summary>
        /// 创建指定平台的输出目录
        /// </summary>
        /// <param name="platform">具体的平台</param>
        /// <param name="exportDirectory">输出目录，为空时则认为是输出到 [StreamingAssets] 目录下的指定平台目录</param>
        /// <returns>输出目录</returns>
        private static string CreateABDirectory(BuildTarget platform, string exportDirectory = null)
        {
            string platformDir = platform.ToString();

            if (string.IsNullOrEmpty(exportDirectory)) // 没有指定导出位置，则生成位置为 StreamingAssets 内
            {
                if (!Directory.Exists(Application.streamingAssetsPath)) { Directory.CreateDirectory(Application.streamingAssetsPath); }
                exportDirectory = string.Format("{0}/{1}", Application.streamingAssetsPath, platformDir);
                if (!Directory.Exists(exportDirectory)) { Directory.CreateDirectory(exportDirectory); }
            }
            else // 生成位置为指定目录
            {
                exportDirectory = string.Format("{0}/{1}", exportDirectory, platformDir);
                if (!Directory.Exists(exportDirectory)) { Directory.CreateDirectory(exportDirectory); }
            }

            return exportDirectory;
        }

        // [MenuItem("AssetBundles/Create Assets Map")]
        private static string CreateAssetsMap(string outputPath, BuildTarget target)
        {
            AssetDatabase.RemoveUnusedAssetBundleNames();

            string manifestPath = string.Format("{0}/{1}", outputPath, target.ToString()); // manifest文件自动生成的，没有后缀名
            string manifestMD5 = string.Empty;
            if (File.Exists(manifestPath))
            {
                manifestMD5 = FileUtility.GetFileHash(manifestPath);
            }
            JSONObject jsonManifest = new JSONObject();
            jsonManifest.Add(target.ToString(), manifestMD5);

            string[] assetBundleNames = AssetDatabase.GetAllAssetBundleNames();
            JSONObject jsonAssetBundleNames = new JSONObject();
            for (int i = 0; i < assetBundleNames.Length; i++)
            {
                string abPath = string.Format("{0}/{1}.{2}", outputPath, assetBundleNames[i], AB_FILE_EXTENSION);
                string abMD5 = string.Empty;
                if (File.Exists(abPath))
                {
                    abMD5 = FileUtility.GetFileHash(abPath);
                }

                jsonAssetBundleNames.Add(string.Format("{0}.{1}", assetBundleNames[i], AB_FILE_EXTENSION), abMD5);
            }

            JSONObject jsonAssets = new JSONObject();
            for (int i = 0; i < assetBundleNames.Length; i++)
            {
                string[] assetPaths = AssetDatabase.GetAssetPathsFromAssetBundle(assetBundleNames[i]);
                for (int j = 0; j < assetPaths.Length; j++)
                {
                    jsonAssets.Add(assetPaths[j], i);
                }
            }

            JSONObject jsonObject = new JSONObject();
            jsonObject.Add(ABConfig.KEY_SERVER, ABConfig.ServerURL);
            jsonObject.Add(ABConfig.KEY_VERSION, ABConfig.Version);
            jsonObject.Add(ABConfig.KEY_MANIFEST, jsonManifest);
            jsonObject.Add(ABConfig.KEY_ASSETBUNDLES, jsonAssetBundleNames);
            jsonObject.Add(ABConfig.KEY_ASSETS, jsonAssets);

            string json = jsonObject.ToString();
            string assetsMapFullName = Application.streamingAssetsPath + "/" + ABConfig.NAME_ASSETSMAP;

            if (!Directory.Exists(Application.streamingAssetsPath))
            {
                Directory.CreateDirectory(Application.streamingAssetsPath);
            }
            StreamWriter sw = new StreamWriter(assetsMapFullName, false, Encoding.UTF8);
            try
            {
                sw.Write(json);
                sw.Flush();
                sw.Close();
            }
            catch (Exception e)
            {
                Debug.Log(e);
                sw.Close();
            }

            AssetDatabase.Refresh();

            return assetsMapFullName;
        }

        private static void CopyAssetsMapFileTo(string outPutPath, BuildTarget target)
        {
            string fileName = CreateAssetsMap(outPutPath, target);
            if (File.Exists(fileName))
            {
                File.Copy(fileName, outPutPath + "/" + ABConfig.NAME_ASSETSMAP, true);
            }
        }

        [MenuItem("AssetBundles/Clean AssetBundles Name", false, 60)]
        private static void CleanABName()
        {
            string[] assetbundleNames = AssetDatabase.GetAllAssetBundleNames();
            for (int i = 0; i < assetbundleNames.Length; i++)
            {
                AssetDatabase.RemoveAssetBundleName(assetbundleNames[i], true);
            }
            // for (int i = 0; i < assetbundleNames.Length; i++)
            // {
            //     string[] assetPaths = AssetDatabase.GetAssetPathsFromAssetBundle(assetbundleNames[i]);
            //     for (int j = 0; j < assetPaths.Length; j++)
            //     {
            //         AssetImporter ai = AssetImporter.GetAtPath(assetPaths[j]);
            //         ai.assetBundleName = string.Empty;
            //         ai.assetBundleVariant = string.Empty;
            //     }
            // }
            // AssetDatabase.RemoveUnusedAssetBundleNames();
        }

        /// <summary>
        /// 删除旧的AB包，包括缓存中的
        /// </summary>
        [MenuItem("AssetBundles/Clean Cache", false, 61)]
        private static void CleanCache()
        {
            Caching.ClearCache();
        }

        /// <summary>
        /// 清理 StreamingAssets 目录
        /// </summary>
        [MenuItem("AssetBundles/Clean StreamingAssets", false, 62)]
        private static void CleanStreamingAssets()
        {
            if (Directory.Exists(Application.streamingAssetsPath))
            {
                Directory.Delete(Application.streamingAssetsPath, true);
            }
        }
    }
}