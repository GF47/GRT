using System;
using System.IO;
using System.Net;

namespace GRT.AssetBundles_Old
{
    public class HttpDownloader : IDisposable
    {
        /// <summary>
        /// 正在下载
        /// </summary>
        public const int STATE_DOWNLOADING = 0;

        /// <summary>
        /// 下载完毕
        /// </summary>
        public const int STATE_COMPLETED = 1;

        /// <summary>
        /// 下载失败
        /// </summary>
        public const int STATE_FAILED = -1;

        private const int TIME_OUT_THRESHOLD = 20000;
        private const int RW_TIME_OUT_THRESHOLD = 10000;
        private const int BUFFER_SIZE = 1024;

        /// <summary>
        /// 超时阈值
        /// </summary>
        public static int TimeOutThreshold = TIME_OUT_THRESHOLD;

        /// <summary>
        /// 读写超时阈值
        /// </summary>
        public static int RWTimeOutThreshold = RW_TIME_OUT_THRESHOLD;

        public int Percent { get; private set; }

        private readonly string url;
        private readonly string nativePath;
        private readonly Action<bool> finishCallback;
        private readonly Action<Exception> errorCallback;

        private long _startPos;
        private FileStream _fileStream;

        private HttpWebRequest _request;
        private WebResponse _response;

        public HttpDownloader(string url, string nativePath, Action<bool> finishCallback = null, Action<Exception> errorCallback = null)
        {
            this.url = url;
            this.nativePath = nativePath;
            this.finishCallback = finishCallback;
            this.errorCallback = errorCallback;
        }

        public void Start()
        {
            if (File.Exists(nativePath))
            {
                _fileStream = File.OpenWrite(nativePath);
                _startPos = _fileStream.Length;
                _fileStream.Seek(_startPos, SeekOrigin.Current);
            }
            else
            {
                var dir = Path.GetDirectoryName(nativePath);
                if (!Directory.Exists(dir)) { Directory.CreateDirectory(dir); }
                _fileStream = new FileStream(nativePath, FileMode.Create);
                _startPos = 0;
            }

            if (_request != null) { _request.Abort(); }

            _request = (HttpWebRequest)WebRequest.Create(url);
            _request.Timeout = TimeOutThreshold;
            _request.ReadWriteTimeout = RWTimeOutThreshold;

            if (_startPos > 0)
            {
                _request.AddRange((int)_startPos);
            }

            _request.BeginGetResponse(FinishCallback, this);
        }

        public void Stop()
        {
            if (_fileStream != null)
            {
                _fileStream.Flush();
                _fileStream.Close();
            }
        }

        public void Dispose()
        {
            if (_fileStream != null) { _fileStream.Close(); _fileStream = null; }
            if (_response != null) { _response.Close(); _response = null; }
            if (_request != null) { _request.Abort(); _request = null; }
        }

        private static void FinishCallback(IAsyncResult ar)
        {
            var state = (HttpDownloader)ar.AsyncState;

            try
            {
                var response = state._request.EndGetResponse(ar);
                state._response = response;

                var stream = response.GetResponseStream();

                var contentLenght = response.ContentLength;
                var currentSize = state._startPos;

                var bytes = new byte[BUFFER_SIZE];
                var readSize = stream.Read(bytes, 0, bytes.Length);
                currentSize += readSize;

                while (readSize > 0)
                {
                    state._fileStream.Write(bytes, 0, readSize);

                    readSize = stream.Read(bytes, 0, bytes.Length);
                    currentSize += readSize;

                    var percent = (int)((double)currentSize * 100 / contentLenght);
                    if (state.Percent < percent) { state.Percent = percent; }
                    // 这里貌似需要写入？不然大概续传不了
                }

                stream.Close();

                state._fileStream.Flush();
                state.Dispose();

                state.finishCallback?.Invoke(true);
            }
            catch (Exception e)
            {
                state.Dispose();

                var info = new FileInfo(state.nativePath);
                if (info.Exists && info.Length == 0) { info.Delete(); }

                state.finishCallback?.Invoke(false);
                state.errorCallback?.Invoke(e);
            }
        }
    }
}