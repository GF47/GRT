using GRT.GTask;
using UnityEngine.Assertions;
using UnityEngine.Networking;

namespace GRT.GAssets.Local
{
    public class LocalAssetItem : IGDisposable
    {
        private UnityWebRequest _request;

        public string Location { get; private set; }

#if UNITY_2020_2_OR_NEWER
        public bool IsAlive => _request != null && _request.result == UnityWebRequest.Result.Success;
#else
        public bool IsAlive => _request != null && !_request.isHttpError && !_request.isNetworkError;
#endif

        public IGScope Scope { get; set; }

        public void GDispose()
        {
            if (_request != null)
            {
                _request.Dispose();
                _request = null;
            }
        }

        public IAwaitable<IAwaiter<DownloadHandler>, DownloadHandler> Get()
        {
            Assert.IsNotNull(_request, $"{nameof(LocalAssetItem)} need a path to load asset");
            return new LocalAssetItemAwaitable() { Request = _request };
        }

        public IAwaitable<IAwaiter<DownloadHandler>, DownloadHandler> Get(string location)
        {
            Assert.IsNull(_request, $"{nameof(LocalAssetItem)} can not be loaded twice, pls use another {nameof(LocalAssetItem)} to load {location}");
            Location = location;
            _request = UnityWebRequest.Get(GetLocalAssetsPath(Location));
            return new LocalAssetItemAwaitable() { Request = _request };
        }

        public const string LOCAL_ASSETS_PLACEHOLDER = "file://";

        public static string GetLocalAssetsPath(string path)
        {
            if (path.StartsWith(LOCAL_ASSETS_PLACEHOLDER)) { return path; }
            else
            {
                return $"{LOCAL_ASSETS_PLACEHOLDER}{path}".Replace('\\', '/');
            }
        }
    }
}