using UnityEngine;

namespace GRT.AssetBundles
{
    public class ABConfig
    {
        /// <summary>
        /// 运行平台
        /// </summary>
        public const string PLATFORM =
#if UNITY_STANDALONE
            "Windows";
#elif UNITY_ANDROID
            "Android";
#elif UNITY_IOS
            "iOS";
#else
            "";
#endif

        public const string KEY_SERVER = "Server";
        public const string SERVER_URL = "http://127.0.0.1:8088";

        public const string KEY_VERSION = "Version";
        public const int VERSION = 1;

        public const string KEY_MANIFEST = "Manifest";
        /// <summary>
        /// Manifest文件的ab包名，通常与平台名相同
        /// </summary>
        public const string MANIFEST_NAME = PLATFORM;

        public const string KEY_ASSETBUNDLES = "AssetBundles";
        public const string KEY_ASSETS = "Assets";

        public const string ASSETSMAP_NAME = "AssetsMap.json";

        // TODO 修改为本机AB文件根目录
#if UNITY_EDITOR
        public const string LOCAL_PATH = "TODO 修改为本机AB文件根目录";
#endif

        public static string AssetBundle_Root_Streaming_AsFile =
#if UNITY_EDITOR
            Application.streamingAssetsPath;
#elif UNITY_STANDALONE
            Application.streamingAssetsPath;
#elif UNITY_ANDROID
            Application.streamingAssetsPath;
#elif UNITY_IOS
            Application.streamingAssetsPath;
#else
            "Application.streamingAssetsPath";
#endif

        public static string AssetBundle_Root_Streaming_AsWeb =
#if UNITY_EDITOR
            Application.streamingAssetsPath;
#elif UNITY_STANDALONE
            Application.streamingAssetsPath;
#elif UNITY_ANDROID
            Application.streamingAssetsPath;
#elif UNITY_IOS
            Application.streamingAssetsPath;
#else
            "Application.streamingAssetsPath";
#endif

        public static string AssetBundle_Root_Hotfix =
#if UNITY_STANDALONE
            Application.persistentDataPath;
#elif UNITY_ANDROID
            Application.persistentDataPath;
#elif UNITY_IOS
            Application.persistentDataPath;
#else
            Application.persistentDataPath;
#endif
    }
}
