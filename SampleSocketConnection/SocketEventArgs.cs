using System.Net.Sockets;

namespace SampleSocketConnection
{
    public class SocketEventArgs
    {
        public string RemoteIPorHost { set; get; } = "";
        public string LocalIPorHost { set; get; } = "";
        public int RemotePort { set; get; } = -1;
        public int LocalPort { set; get; } = -1;
        public byte[] SendBytes { set; get; } = null;
        public byte[] RecvBytes { set; get; } = null;
        public System.Text.Encoding Encoding { set; get; } = System.Text.Encoding.UTF8;
        public Socket WorkSocket { set; get; } = null;

        //文字列の取得
        public string GetSendString()
        {
            return Encoding.GetString(SendBytes, 0, SendBytes.Length).Trim('\0');
        }
        public string GetRecvString()
        {
            return Encoding.GetString(RecvBytes, 0, RecvBytes.Length).Trim('\0');
        }


    }
}
