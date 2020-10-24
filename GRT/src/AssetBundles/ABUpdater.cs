/***************************************************************
 * @File Name       : ABUpdater
 * @Author          : GF47
 * @Description     : 更新ab包，只需要new一下然后等待yield就行
 * @Date            : 2017/8/1/星期二 11:11:10
 * @Edit            : none
 **************************************************************/

// TODO 如果不联网更新，则需要把 NEED_TO_CONNECT_TO_THE_AB_SERVER 开启
#define NEED_TO_CONNECT_TO_THE_AB_SERVER
// #undef NEED_TO_CONNECT_TO_THE_AB_SERVER

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace GRT.AssetBundles
{
    public class ABUpdater : CustomYieldInstruction
    {
        public int Progress
        {
            get
            {
                if (_isUpdateFinished) { return 100; }

                if (_downLoaders == null) { return 0; }
                if (_downLoaders.Count == 0) { return 100; }

                int value = 0;
                for (int i = 0; i < _downLoaders.Count; i++)
                {
                    value += _downLoaders[i].Progress;
                }
                value /= _downLoaders.Count;
                return value;
            }
        }
        public override bool keepWaiting
        {
            get
            {
                if (_isUpdateFinished) { return false; }
                if (_downLoaders == null) { return true; }
                for (int i = 0; i < _downLoaders.Count; i++)
                {
                    if (_downLoaders[i].keepWaiting) { return true; }
                }
                return false;
            }

        }

        private bool _isUpdateFinished;

        private List<ABDownloader> _downLoaders;

        private readonly bool _need2Connect2Server; // 是否需要联网，如果为false，则省略联网更新步骤

        public ABUpdater(bool need2Connect2Server = true)
        {
            _need2Connect2Server = need2Connect2Server;
            AssetBundlesManager.ConstructFunc = () => new AssetBundlesManager();
            Coroutines.StartACoroutine(__Init());
        }

        private IEnumerator __Init()
        {
#if NEED_TO_CONNECT_TO_THE_AB_SERVER
            if (_need2Connect2Server) { yield return new AssetsMapDownLoader(); }
#else
            // 无需联网，省略此步骤
#endif
            yield return new AssetsMap();

            if (AssetsMap.Instance.IsStreamingAssets)
            {
                _isUpdateFinished = true;
                yield break;
            }

            KeyValuePair<string, string>[] abArray = AssetsMap.Instance.assetbundles;

            _downLoaders  = new List<ABDownloader>(abArray.Length + 1);

            CheckIfShouldUpdate(AssetsMap.Instance.manifest.Key, AssetsMap.Instance.manifest.Value);

            for (int i = 0; i < abArray.Length; i++)
            {
                CheckIfShouldUpdate(abArray[i].Key, abArray[i].Value);
            }

            yield return this;

            _isUpdateFinished = true;
        }

        private void CheckIfShouldUpdate(string abName, string md5)
        {
            bool shouldUpdate = true;
            string nativePath = ABConfig.AssetBundle_Root_Hotfix + "/" + abName;

            if (File.Exists(nativePath))
            {
                string nativeMD5 = FileUtility.GetFileHash(nativePath);
                if (string.Equals(md5, nativeMD5)) { shouldUpdate = false; }
            }
            if (shouldUpdate)
            {
                _downLoaders.Add(new ABDownloader(abName));
            }
        }

    }
}
