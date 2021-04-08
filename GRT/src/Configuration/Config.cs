using UnityEngine;

namespace GRT.Configuration
{
    public static class Config
    {
        public const string configPath = "application.config";
        private static readonly XmlConfig _config;

        static Config()
        {
            (_config = new XmlConfig()).Initialize_XmlFileName($"{Application.streamingAssetsPath}/{configPath}");
        }

        public static T Get<T>(string name, T @default = default)
        {
            return _config.Get(name, @default);
        }

        public static bool Get<T>(string name, out T value, T @default = default)
        {
            return _config.Get(name, out value, @default);
        }

        public static void Set<T>(string name, T value)
        {
            _config.Set(name, value);
        }
    }
}