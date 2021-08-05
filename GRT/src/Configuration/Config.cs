// #define UNITY_WEBGL
// #undef UNITY_EDITOR
// 
using System.IO;
using UnityEngine;

namespace GRT.Configuration
{
    public static class Config
    {
        public const string configPath = "Application.config";
        private static XmlConfig _config;

        static Config()
        {
#if !UNITY_EDITOR && UNITY_WEBGL

            Coroutines.StartACoroutine(GetConfig());

#else
            (_config = new XmlConfig()).Initialize_XmlFileName($"{Application.streamingAssetsPath}/{configPath}");
#endif
        }

#if !UNITY_EDITOR && UNITY_WEBGL
        private static System.Collections.IEnumerator GetConfig()
        {
            var path = Path.Combine(Application.streamingAssetsPath, configPath);
            var uri = new System.Uri(path);
            var www = UnityEngine.Networking.UnityWebRequest.Get(uri);
            yield return www.SendWebRequest();

            if (www.isDone)
            {
                (_config = new XmlConfig()).Initialize_XmlString(www.downloadHandler.text);
            }
            else
            {
                _config = new XmlConfig();
            }
        }

#endif

        public static T Get<T>(string name, T @default = default)
        {
            return _config == null ? @default : _config.Get(name, @default);
        }

        public static bool Get<T>(string name, out T value, T @default = default)
        {
            value = @default;
            return _config == null ? false : _config.Get(name, out value, @default);
        }

        public static void Set<T>(string name, T value)
        {
            _config?.Set(name, value);
        }
    }
}
