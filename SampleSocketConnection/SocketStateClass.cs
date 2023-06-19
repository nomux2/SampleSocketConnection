using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace SampleSocketConnection
{
    /// <summary>
    /// 通信時に付随する情報
    /// </summary>
    public class SocketStateClass
    {
        //送受信用バッファ
        public byte[] Buffer { set; get; } = null;

        //Encording
        public Encoding Encoding { set; get; } = System.Text.Encoding.UTF8;

        // ソケット
        public Socket WorkSocket { set; get; } = null;

        //文字列の取得
        public string GetString()
        {
            return Encoding.GetString(Buffer, 0, Buffer.Length);
        }
    }
}
