using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace GRT.Net
{
    public class TcpStringsReceiver
    {
        public Action<string> OnReceive;

        public const string DEFAULT_END_FLAG = ")-GF47-)";
        public string EndFlag
        {
            get => _endFlag;
            set
            {
                _endFlag = value;
                _endFlagBytes = Encoding.UTF8.GetBytes(_endFlag);
                _endFlagLength = _endFlagBytes.Length;
            }
        }
        private string _endFlag;
        private byte[] _endFlagBytes;
        private int _endFlagLength;

        private const int BUFFER_LENGTH = 1000;

        private string _host;
        private int _port;

        public bool IsReceiving { get; set; }

        public TcpStringsReceiver(string host, int port, string endFlag = DEFAULT_END_FLAG)
        {
            _host = host;
            _port = port;
            EndFlag = endFlag;
        }

        public void Start()
        {
            IsReceiving = true;

            using (var client = new TcpClient(_host, _port))
            {
                var stream = client.GetStream();

                var bufferList = new List<byte[]>();

                while (IsReceiving)
                {
                    var buffer = new byte[BUFFER_LENGTH];
                    var bufferLength =stream.Read(buffer, 0, BUFFER_LENGTH);

                    int n = 0, m = 0; // n为小缓冲区当前结束标志的起始位置
                                      // m为小缓冲区上一个结束标志的结束位置
                    byte[] temp;

                    if (bufferList.Count > 0) // 处理缓冲区衔接处是否能组合成为结束标志的情况
                    {
                        m = GetSubArrayIndexMerged(bufferList[bufferList.Count - 1], buffer, _endFlagBytes);
                        if (m > 0) // 大缓冲区结尾+小缓冲区开头可以组成结束标志，清空大缓冲区，小缓冲区从m处起始
                        {
                            var p = 0;
                            for (int k = 0; k < bufferList.Count; k++)
                            {
                                p += bufferList[k].Length;
                            }
                            temp = new byte[p + m - _endFlagLength];

                            p = 0;
                            for (int k = 0; k < bufferList.Count - 1; k++) // 将大缓冲区数据拷贝到temp中
                            {
                                Buffer.BlockCopy(bufferList[k], 0, temp, p, bufferList[k].Length);
                                p += bufferList[k].Length;
                            }
                            Buffer.BlockCopy(bufferList[bufferList.Count - 1], 0, temp, p, bufferList[bufferList.Count - 1].Length + m - _endFlagLength);
                            bufferList.Clear(); // 大缓冲区清空

                            OnReceive?.Invoke(Encoding.UTF8.GetString(temp));
                        }
                    }

                    n = GetSubArrayIndex(buffer, _endFlagBytes, m);
                    if (n > -1) // 找到了结束标志
                    {
                        var p = 0; // 大缓冲区组合成单个数组时的临时长度标记
                        for (int k = 0; k < bufferList.Count; k++)
                        {
                            p += bufferList[k].Length;
                        }
                        temp = new byte[p + n - m]; // 截止到当前结束标志，所有数据的长度，p为大缓冲区已经存在的数据长度，n - m为小缓冲区的当前结束标志前边的数据长度

                        p = 0;
                        for (int k = 0; k < bufferList.Count; k++) // 将大缓冲区数据拷贝到temp中
                        {
                            Buffer.BlockCopy(bufferList[k], 0, temp, p, bufferList[k].Length);
                            p += bufferList[k].Length;
                        }
                        bufferList.Clear(); // 大缓冲区清空

                        Buffer.BlockCopy(buffer, m, temp, p, n - m); // 将小缓冲区当前结束标志前边的数据拷贝到temp中

                        OnReceive?.Invoke(Encoding.UTF8.GetString(temp));

                        do // 处理小缓冲区其余的结束标志
                        {
                            m = n + _endFlagLength;
                            n = GetSubArrayIndex(buffer, _endFlagBytes, m);

                            if (n > -1) // 表明小缓冲区还存在其余的结束标志
                            {
                                OnReceive?.Invoke(Encoding.UTF8.GetString(buffer, m, n - m)); // 直接从小缓冲区取数据
                            }
                        } while (n > -1);
                    }

                    if (m > 0)
                    {
                        temp = new byte[bufferLength - m]; // 小缓冲区剩余数据
                        Buffer.BlockCopy(buffer, m, temp, 0, temp.Length);
                    }
                    else
                    {
                        temp = buffer;
                    }
                    bufferList.Add(temp);
                }
            }
        }

        /// <summary>
        /// 查找子数组，有空用KMP重新实现吧 :(
        /// </summary>
        /// <param name="list">父串</param>
        /// <param name="sub">子串</param>
        /// <param name="startIndex">起始位置</param>
        /// <returns>匹配起始位置，没有的话为-1</returns>
        private static int GetSubArrayIndex(IList<byte> list,
                                            IList<byte> sub,
                                            int startIndex = 0)
        {
            int j = 0;
            while (startIndex < list.Count && j < sub.Count)
            {
                if (list[startIndex] == sub[j])
                {
                    startIndex++;
                    j++;
                }
                else
                {
                    startIndex = startIndex - j + 1;
                    j = 0;
                }
            }
            if (j == sub.Count)
            {
                return startIndex - j;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// 查找两数组合并处是否有子数组
        /// </summary>
        /// <param name="list">左数组</param>
        /// <param name="list2">右数组</param>
        /// <param name="sub">子数组</param>
        /// <returns>匹配结束位置，没有的话为0</returns>
        private static int GetSubArrayIndexMerged(IList<byte> list,
                                                      IList<byte> list2,
                                                      IList<byte> sub)
        {
            int i = list.Count - 1, j = 0;
            while (i > 0 && i > list.Count - sub.Count && j < sub.Count)
            {
                if (i >= list.Count)
                {
                    if (list2[i - list.Count] == sub[j]) { i++; j++; }
                    else { i = i - j - 1; j = 0; }
                }
                else
                {
                    if (list[i] == sub[j]) { i++; j++; }
                    else { i = i - j - 1; j = 0; }
                }
            }
            if (j == sub.Count)
            {
                // if (i >= list.Count) { i -= list.Count; } // 貌似i不可能比list.count小……
                i -= list.Count;
                return i; // 结束标志在list2中的结束位置，跟正常返回起始位置不一样
            }
            return 0;
        }
    }
}
