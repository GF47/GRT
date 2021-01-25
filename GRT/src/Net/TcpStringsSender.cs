using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace GRT.Net
{
    public class TcpStringsSender: IDisposable
    {
        TcpListener listener;
        TcpClient client;

        public TcpStringsSender(string host, int port)
        {
            listener = new TcpListener(IPAddress.Parse(host), port);
            listener.Start();
            try
            {
                client = listener.AcceptTcpClient(); // 同步的，没接收到会卡住
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Send(string content)
        {
            var stream = client.GetStream();
            var bytes = Encoding.UTF8.GetBytes(content);
            stream.Write(bytes, 0, bytes.Length);
        }

        public void Dispose()
        {
            client.Close();
            listener.Stop();
        }
    }
}
