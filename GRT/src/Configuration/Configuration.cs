using GRT;
using GRT.Configuration;
using UnityEngine;

namespace DigitalTwin
{
    public class Configuration : Singleton<Configuration>, IBlackBoard
    {
        public const string configPath = "application.config";
        private readonly Config _config;
        private readonly BlackBoard _dynamicConfig;

        public Configuration()
        {
            _config = new Config();
            _config.Initialize_XmlFileName($"{Application.streamingAssetsPath}/{configPath}");

            _dynamicConfig = new BlackBoard();

        }
        public T Get<T>(string name, T @default = default)
        {
            var has = _dynamicConfig.Get(name, out T value, @default);
            if (!has)
            {
                value = _config.Get(name, @default);
            }
            return value;
        }
        public bool Get<T>(string name, out T value, T @default = default)
        {
            return _dynamicConfig.Get(name, out value, @default) || _config.Get(name, out value, @default);
        }

        public void Set<T>(string name, T value)
        {
            _dynamicConfig.Set(name, value);
        }
    }
}
