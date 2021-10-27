using System.Collections;
using System.IO;
using UnityEngine;

namespace GRT.AssetBundles_Old
{
    public class ABItem
    {
        public AssetBundle ab;
        public string path;
        public int referenceCount;

        public ABItem(string path, bool isAsync = false/*, Action<AssetBundle> callback = null*/)
        {
            this.path = path;

            var nativePath = $"{ABConfig.RootPath_HotFix}/{this.path}";
            if (!File.Exists(nativePath)) { nativePath = $"{ABConfig.RootPath_FileStreaming}/{this.path}"; }

            if (isAsync)
            {
                Coroutines.StartACoroutine(__GetABAsync(nativePath));
            }
            else
            {
                ab = AssetBundle.LoadFromFile(nativePath);
            }
        }

        private IEnumerator __GetABAsync(string path)
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