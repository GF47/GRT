using System;
using System.IO;
using UnityEngine;

namespace GRT.AssetBundles_Old
{
    public class AssetsMapDownLoader : CustomYieldInstruction
    {
        private static readonly string _nativePath = $"{ABConfig.RootPath_HotFix}/{ABConfig.NAME_ASSETSMAP}";
        private static readonly string _url = $"{ABConfig.ServerURL}/{ABConfig.Platform}/{ABConfig.NAME_ASSETSMAP}";
        private HttpDownloader _downLoader;
        private bool _isDone;
        private int _retryNumber;

        public AssetsMapDownLoader() { Start(); }

        public override bool keepWaiting => !_isDone;
        public int Progress { get { return _downLoader.Percent; } }

        private void ErrorCallback(Exception e)
        {
            if (_retryNumber < 2)
            {
                _retryNumber++;
                Start();
            }
            else
            {
                _isDone = true;
                if (File.Exists(_nativePath))
                {
                    File.Delete(_nativePath);
                }
            }
        }

        private void FinishedCallback(bool b)
        {
            _isDone = b;
        }

        private void Start()
        {
            if (File.Exists(_nativePath))
            {
                File.Delete(_nativePath);
            }
            _downLoader = new HttpDownloader(_url, _nativePath, FinishedCallback, ErrorCallback);
            _downLoader.Start();
        }
    }
}