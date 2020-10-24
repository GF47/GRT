using System;
using System.IO;
using UnityEngine;

namespace GRT.AssetBundles
{
    public class AssetsMapDownLoader : CustomYieldInstruction
    {
        public int Progress { get { return _downLoader.Percent; } }
        public override bool keepWaiting { get { return !_isDone; } }
        private bool _isDone;

        private HttpDownloader _downLoader;
        private int _retryNumber;

        private const string _url = ABConfig.SERVER_URL + "/" + ABConfig.PLATFORM + "/" + ABConfig.ASSETSMAP_NAME;

        private static readonly string _nativePath = ABConfig.AssetBundle_Root_Hotfix + "/" + ABConfig.ASSETSMAP_NAME;

        public AssetsMapDownLoader() { Start(); }

        private void Start()
        {
            if (File.Exists(_nativePath))
            {
                File.Delete(_nativePath);
            }
            _downLoader = new HttpDownloader(_url, _nativePath, FinishedCallback, ErrorCallback);
            _downLoader.Start();
        }

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

    }
}
