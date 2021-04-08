using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GRT.AssetBundles
{
    [CreateAssetMenu]
    public class ABConfig : ScriptableObject
    {
        #region Singleton

        private static ABConfig _instance;

        public static ABConfig Instance
        {
            get
            {
                if (_instance == null)
                {
#if UNITY_EDITOR
                    _instance = AssetDatabase.LoadAssetAtPath<ABConfig>("Assets/Resources/ABConfig.asset");
                    if (_instance == null)
                    {
                        var abc = CreateInstance(typeof(ABConfig));
                        if (!System.IO.Directory.Exists($"{Application.dataPath}/Resources"))
                        {
                            AssetDatabase.CreateFolder("Assets", "Resources");
                        }
                        AssetDatabase.CreateAsset(abc, "Assets/Resources/ABConfig.asset");
                        AssetDatabase.Refresh();
                        _instance = AssetDatabase.LoadAssetAtPath<ABConfig>("Assets/Resources/ABConfig.asset");
                    }
#else
                    _instance = Resources.Load<ABConfig>("ABConfig");
#endif
                }
                return _instance;
            }
        }

        #endregion Singleton

        public const string KEY_ASSETBUNDLES = "AssetBundles";

        public const string KEY_ASSETS = "Assets";

        public const string KEY_MANIFEST = "Manifest";

        public const string KEY_SERVER = "ServerURL";

        public const string KEY_VERSION = "Version";

        public const string NAME_ASSETSMAP = "AssetsMap.json";

        public static string Manifest => Platform;

        public static string Platform
        {
            get
            {
#if UNITY_EDITOR
                return EditorUserBuildSettings.activeBuildTarget.ToString();
#elif UNITY_STANDALONE_WIN
#if UNITY_64
            return "StandaloneWindows64";
#else
            return "StandaloneWindows";
#endif
#elif UNITY_ANDROID
            return "Android";
#elif UNITY_IOS
            return "iOS";
#elif UNITY_WEBGL
            return "WebGL";
#else
            return "unknown";
#endif
            }
        }

        [SerializeField] private string _serverURL = "http://127.0.0.1:8088";
        [SerializeField] private int _version = 1;

#if UNITY_EDITOR
        public static string RootPath_Editor => $"{Application.dataPath}/../../UnityAssetBundles";
#endif

        public static string RootPath_FileStreaming =>
#if UNITY_EDITOR
            Application.streamingAssetsPath;
#elif UNITY_STANDLONE
            Application.streamingAssetsPath;
#elif UNITY_ANDROID
            Application.streamingAssetsPath;
#elif UNITY_IOS
            Application.streamingAssetsPath;
#elif UNITY_WEBGL
            null;
#else
            Application.streamingAssetsPath;
#endif

        public static string RootPath_WebStreaming =>
#if UNITY_EDITOR
            $"file:///{Application.streamingAssetsPath}";
#elif UNITY_STANDLONE
            $"file:///{Application.streamingAssetsPath}";
#elif UNITY_ANDROID
            Application.streamingAssetsPath;
#elif UNITY_IOS
            $"file://{Application.streamingAssetsPath}";
#elif UNITY_WEBGL
            null;
#else
            $"file://{Application.streamingAssetsPath}";
#endif

        public static string RootPath_HotFix =>
#if UNITY_EDITOR
            Application.persistentDataPath; // %userprofile%\AppData\LocalLow\<companyname>\<productname>
#elif UNITY_STANDLONE
            Application.persistentDataPath; // %userprofile%\AppData\LocalLow\<companyname>\<productname>
#elif UNITY_ANDROID
            Application.persistentDataPath; // points to /storage/emulated/0/Android/data/<packagename>/files on most devices (some older phones might point to location on SD card if present)
#elif UNITY_IOS
            Application.persistentDataPath; // /var/mobile/Containers/Data/Application/<guid>/Documents
#elif UNITY_WEBGL
            Application.persistentDataPath; // points to /idbfs/<md5 hash of data path> where the data path is the URL stripped of everything including and after the last '/' before any '?' components.
#else
            Application.persistentDataPath;
#endif

        public static string ServerURL => Instance._serverURL;
        public static int Version => Instance._version;
    }
}