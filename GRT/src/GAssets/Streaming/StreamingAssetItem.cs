using GRT.GTask;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Networking;

namespace GRT.GAssets.Streaming
{
    public class StreamingAssetItem : IGDisposable
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
            Assert.IsNotNull(_request, $"{nameof(StreamingAssetItem)} need a path to load asset");
            return new StreamingAssetItemAwaitable() { Request = _request };
        }

        public IAwaitable<IAwaiter<DownloadHandler>, DownloadHandler> Get(string location)
        {
            Assert.IsNull(_request, $"{nameof(StreamingAssetItem)} can not be loaded twice, pls use another {nameof(StreamingAssetItem)} to load {location}");
            Location = location;
            _request = UnityWebRequest.Get(GetAbsoluteStreamingAssetsPath(Location));
            return new StreamingAssetItemAwaitable() { Request = _request };
        }

        public const string STREAMING_ASSETS_PLACEHOLDER = "[StreamingAssets]";

        public static string GetAbsoluteStreamingAssetsPath(string path)
            => path.Replace(STREAMING_ASSETS_PLACEHOLDER, Application.streamingAssetsPath).Replace('\\', '/');
    }
}