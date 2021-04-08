/***************************************************************
 * @File Name       : AssetBundlesManager
 * @Author          : GF47
 * @Description     : ab包管理器，平时使用时需要 BeginLoadABContain()，使用完毕之后 调用 EndLoad()，true表示保留对这个ab包的引用，false表示用完ab包之后随即卸载
 * @Date            : 2017/8/1/星期二 10:59:55
 * @Edit            : none
 **************************************************************/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace GRT.AssetBundles
{
    public class AssetBundlesManager : Singleton<AssetBundlesManager>
    {
        private AssetBundleManifest _manifest;

        private List<ABItem> _assetBundles;

        private List<ABItem> _assetBundlesTemp;

        public void Init()
        {
            _assetBundles = new List<ABItem>();
            _assetBundlesTemp = new List<ABItem>();
            GetManifest();
        }

        public void GetManifest()
        {
            // Debug.Log(AssetsMap.Instance.manifest.Key);
            string abPath = (AssetsMap.Instance.IsStreamingAssets
                ? ABConfig.RootPath_FileStreaming
                : ABConfig.RootPath_HotFix)
                + "/" + AssetsMap.Instance.manifest.Key;
            AssetBundle ab = AssetBundle.LoadFromFile(abPath);
            _manifest = ab.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            Assert.IsNotNull(_manifest);
            ab.Unload(false);
        }

        public ABItem BeginLoadABContain(string assetName)
        {
            // _assetBundlesTemp.Clear();

            string abName = GetABNameByAssetName(assetName);

            ABItem item = LoadABAndDependencies(abName);

            for (int i = 0; i < _assetBundlesTemp.Count; i++)
            {
                _assetBundlesTemp[i].referenceCount++;
            }

            return item;
        }

        /// <summary>
        /// 卸载ab包
        /// </summary>
        /// <param name="resident">是否常驻内存</param>
        public void EndLoad(bool resident)
        {
            if (!resident)
            {
                for (int i = 0; i < _assetBundlesTemp.Count; i++)
                {
                    _assetBundlesTemp[i].referenceCount--;
                }
            }
            UnloadTemp(false);
            _assetBundlesTemp.Clear();
        }

        private void UnloadTemp(bool force)
        {
            for (int i = 0; i < _assetBundlesTemp.Count; i++)
            {
                ABItem item = _assetBundlesTemp[i];
                if (item.referenceCount < 1)
                {
                    if (item.ab != null) item.ab.Unload(force);
                }
            }
            _assetBundles.RemoveAll(abItem => abItem.referenceCount < 1);
        }

        public void ReleaseAllAB(bool force)
        {
            for (int i = 0; i < _assetBundles.Count; i++)
            {
                ABItem item = _assetBundles[i];
                if (item.referenceCount < 1)
                {
                    item.ab.Unload(force);
                }
            }
            _assetBundles.RemoveAll(abItem => abItem.referenceCount < 1);
        }

        private ABItem LoadABAndDependencies(string abName)
        {
            ABItem item = _assetBundles.Find(abItem => abItem.path == abName);
            if (item == null)
            {
                item = new ABItem(abName);
                _assetBundles.Add(item);
            }

            Assert.IsNotNull(_manifest);
            string[] dependencies = _manifest.GetDirectDependencies(abName);
            foreach (var dependency in dependencies)
            {
                LoadABAndDependencies(dependency);
            }
            _assetBundlesTemp.Add(item);
            return item;
        }

        public static string GetABNameByAssetName(string assetName)
        {
            AssetsMap map = AssetsMap.Instance;
            if (!map.assets.ContainsKey(assetName)) { return null; }
            int abID = map.assets[assetName];
            KeyValuePair<string, string> abPair = map.assetbundles[abID];
            return abPair.Key;
        }
    }
}