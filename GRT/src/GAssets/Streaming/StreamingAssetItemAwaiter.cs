using GRT.GTask;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace GRT.GAssets.Streaming
{
    public class StreamingAssetItemAwaiter : IAwaiter<DownloadHandler>
    {
        public UnityWebRequest Request { get; set; }

        public bool IsCompleted => Request.isDone;

        public DownloadHandler GetResult()
        {
            if (Request.isDone)
            {
#if UNITY_2020_2_OR_NEWER
                if (Request.result != UnityWebRequest.Result.Success)
#else
                if (Request.isHttpError || Request.isNetworkError)
#endif
                {
                    throw new UnityException($"{Request.uri} load failed, {Request.error}");
                }
                else
                {
                    return Request.downloadHandler;
                }
            }
            else
            {
                throw new UnityException($"{Request.uri} is loading, pls wait until completed");
            }
        }

        private Action _continuation;

        public void OnCompleted(Action continuation) => _continuation = continuation;

        public void Continue() => _continuation?.Invoke();
    }
}