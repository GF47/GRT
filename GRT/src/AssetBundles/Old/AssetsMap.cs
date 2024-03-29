﻿using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace GRT.AssetBundles_Old
{
    public class AssetsMap : CustomYieldInstruction
    {
        public static AssetsMap Instance { get { return _instance; } }
        private static AssetsMap _instance;

        public KeyValuePair<string, string>[] assetbundles;
        public Dictionary<string, int> assets;
        public KeyValuePair<string, string> manifest;
        public string serverAddress;
        public int version;

        public bool IsStreamingAssets { get; private set; }

        public override bool keepWaiting => !_isDone;
        private bool _isDone;

        public AssetsMap()
        {
            _instance = this;

            string nativePath = $"{ABConfig.RootPath_HotFix}/{ABConfig.NAME_ASSETSMAP}"; // 首先读取PersistentData文件夹，是否有map文件

            if (File.Exists(nativePath))
            {
                ReadJson(File.ReadAllText(nativePath));
                _isDone = true;
            }
            else
            {
                IsStreamingAssets = true;

                nativePath = $"{ABConfig.RootPath_WebStreaming}/{ABConfig.NAME_ASSETSMAP}"; // 尝试读取StreamingAssets文件夹
                GCoroutine.Yield(GetJson(nativePath));
            }
        }

        private IEnumerator GetJson(string nativePath)
        {
            using (var request = UnityWebRequest.Get(nativePath))
            {
                yield return request.SendWebRequest();
                if (request.isNetworkError || request.isHttpError)
                {
                    Debug.LogError(request.error);
                }
                else
                {
                    ReadJson(request.downloadHandler.text);
                }
            }

            _isDone = true;
        }

        private void ReadJson(string jsonStr)
        {
            if (!string.IsNullOrEmpty(jsonStr))
            {
                JSONObject assetsMap = JSON.Parse(jsonStr).AsObject;
                serverAddress = assetsMap[ABConfig.KEY_SERVER];
                version = assetsMap[ABConfig.KEY_VERSION];
                JSONObject manifestJsonObject = assetsMap[ABConfig.KEY_MANIFEST].AsObject;
                JSONObject assetBundleJsonObjects = assetsMap[ABConfig.KEY_ASSETBUNDLES].AsObject;
                JSONObject assetJsonObjects = assetsMap[ABConfig.KEY_ASSETS].AsObject;

                foreach (KeyValuePair<string, JSONNode> pair in manifestJsonObject)
                {
                    manifest = new KeyValuePair<string, string>(pair.Key, pair.Value);
                    break;
                }

                assetbundles = new KeyValuePair<string, string>[assetBundleJsonObjects.Count];
                int index = 0;
                foreach (KeyValuePair<string, JSONNode> pair in assetBundleJsonObjects)
                {
                    assetbundles[index] = new KeyValuePair<string, string>(pair.Key, pair.Value);
                    index++;
                }

                assets = new Dictionary<string, int>(assetJsonObjects.Count);
                foreach (KeyValuePair<string, JSONNode> pair in assetJsonObjects)
                {
                    assets.Add(pair.Key, pair.Value);
                }
            }
        }
    }
}