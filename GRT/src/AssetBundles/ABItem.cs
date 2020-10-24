using System;
using System.Collections;
using System.IO;
using UnityEngine;

namespace GRT.AssetBundles 
{
    public class ABItem
    {
        public string path;
        public AssetBundle ab;
        public int referenceCount;

        public ABItem(string path, bool isAsync = false, Action<AssetBundle> callback = null)
        {
            this.path = path;

            var nativePath = $"{ABConfig.AssetBundle_Root_Hotfix}/{this.path}";
            if (!File.Exists(nativePath)) { nativePath = $"{ABConfig.AssetBundle_Root_Streaming_AsFile}/{this.path}"; }

            if (isAsync)
            {
                Coroutines.StartACoroutine(GetABAsync(nativePath));
            }
            else
            {
                ab = AssetBundle.LoadFromFile(nativePath);
            }
        }

        private IEnumerator GetABAsync(string path)
        {
            var request = AssetBundle.LoadFromFileAsync(path);
            yield return request;
            ab = request.assetBundle;
        }

        public void Unload(bool force = true)
        {
            if (force)
            {
                ab.Unload(true);
                return;
            }

            if (referenceCount < 1)
            {
                ab.Unload(false);
            }
        }
    }
}
