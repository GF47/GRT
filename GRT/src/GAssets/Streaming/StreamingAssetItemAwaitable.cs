using GRT.GTask;
using System.Threading;
using UnityEngine.Networking;

namespace GRT.GAssets.Streaming
{
    public struct StreamingAssetItemAwaitable : IAwaitable<IAwaiter<DownloadHandler>, DownloadHandler>
    {
        public UnityWebRequest Request { get; set; }

        public IAwaiter<DownloadHandler> GetAwaiter()
        {
            var awaiter = new StreamingAssetItemAwaiter() { Request = Request };
            if (SynchronizationContext.Current == GCoroutine.UnityContext)
            {
                var operation = awaiter.Request.SendWebRequest();
                GCoroutine.YieldThen(operation, awaiter.Continue);
            }
            else
            {
                GCoroutine.UnityContext.Post(_ =>
                {
                    var operation = awaiter.Request.SendWebRequest();
                    GCoroutine.YieldThen(operation, awaiter.Continue);
                }, null);
            }

            return awaiter;
        }
    }
}