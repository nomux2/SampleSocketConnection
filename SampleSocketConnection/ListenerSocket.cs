using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace SampleSocketConnection
{
    internal class ListenerSocket : IDisposable
    {
        public enum EnumServerState
        {
            None,
            Listening,
            Stopped
        }

        private Socket _listener_socket;
        private IPEndPoint _socket_EP;

        List<ClientSocket> _client_list = new List<ClientSocket>();

        //マニュアルリセットイベントのインスタンスを生成
        public ManualResetEvent DoneCheckAll = new ManualResetEvent(false);
        private bool disposedValue;
        private bool _cancel_flg;   //キャンセル

        protected System.Text.Encoding Encoding { get; set; } = System.Text.Encoding.UTF8;


        #region properties

        /// <summary>リスナーの状態を返す</summary>
        public EnumServerState Status { private set; get; }

        public bool Cancel
        {
            set
            {
                _cancel_flg = value;
                DoneCheckAll.Set();
            }
            get 
            { 
                return _cancel_flg;
            }
        }

        #endregion

        #region イベント定義

        public event ClientSocket.SocketEventHandller OnConnected = null;
        public event ClientSocket.SocketEventHandller OnDisConnected = null;
        public event ClientSocket.SocketEventHandller OnRecved = null;
        public event ClientSocket.SocketEventHandller OnSended = null;
        public event ClientSocket.SocketErrorEventHandller OnError = null;

        #endregion

        #region dispose

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: マネージド状態を破棄します (マネージド オブジェクト)
                    Close();
                }

                // TODO: アンマネージド リソース (アンマネージド オブジェクト) を解放し、ファイナライザーをオーバーライドします
                // TODO: 大きなフィールドを null に設定します
                disposedValue = true;
            }
        }

        // // TODO: 'Dispose(bool disposing)' にアンマネージド リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします
        // ~ListenerSocket()
        // {
        //     // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }


        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="port">ポート番号</param>
        /// <param name="back_log">保留中の接続のキューの最大数</param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        public async void StartListening(ushort port, int back_log = 100)
        {
            try
            {
                //Listener用のSocketを作成
                if (_listener_socket == null)
                {
                    _listener_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    this.Status = EnumServerState.None;
                }

                if (this.Status != EnumServerState.None)
                    throw new ApplicationException("すでに待ち状態です。");

                // IPアドレスとポート番号を指定して、ローカルエンドポイントを設定
                _socket_EP = new IPEndPoint(IPAddress.Any, port);

                //_socket_EP = new IPEndPoint(Dns.Resolve(host).AddressList[0], portNum);
                //_socket_EP = new IPEndPoint(Dns.GetHostAddresses(host)[0], portNum);

                // TCP/IPのソケットをローカルエンドポイントにバインド
                _listener_socket.Bind(_socket_EP);

                //Listenを開始する
                _listener_socket.Listen(back_log);
                this.Status = EnumServerState.Listening;

                //接続されているクライアントを0にする
                _client_list.Clear();

                //CancelをFalseにする
                this.Cancel = false;

                await Task.Run(() =>
                {
                    AcceptLoop();
                });

                //キャンセルなどされて処理が終わった場合、Closeする
                Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace);
            }

        }

        /// <summary>
        /// 接続承認ループ
        /// </summary>
        public void AcceptLoop()
        {
            //キャンセルされたら抜ける
            while (this.Cancel == false)
            {
                // シグナル状態にし、メインスレッドの処理を続行する
                DoneCheckAll.Reset();

                //承認開始クライアントからの接続があればCallbackAcceptが発生する
                _listener_socket.BeginAccept(new AsyncCallback(CallbackAccept), _listener_socket);

                // シグナル状態になるまで待つ
                DoneCheckAll.WaitOne();
            }
        }

        /// <summary>
        /// 接続要求がくるのでデータ受信待ち状態にする。
        /// </summary>
        /// <param name="ar"></param>
        public void CallbackAccept(IAsyncResult ar)
        {

            // クライアント要求を処理するソケットを取得(_listener_socketと同じはず)
            Socket server = (Socket)ar.AsyncState;

            //リスナーが終了している場合もしくはすでに破棄されている場合は、nullが返るので終了する。
            if (server == null || this.disposedValue == true) 
            {
                this.Cancel = true; //ループから抜けるためにCancelする
                DoneCheckAll.Set();
                return;
            };

            if (this.Status == EnumServerState.None) return;

            // シグナル状態にし、メインスレッドの処理を続行する
            DoneCheckAll.Set();

            //接続してきたクライアントのソケットを取得
            Socket socket = server.EndAccept(ar);

            //クライアントソケットオブジェクトを生成
            ClientSocket client = new ClientSocket(socket);

            //各種イベントを設定
            client.OnDisConnected += OnHostDisConnected;
            if (OnRecved != null) client.OnRecved += OnRecved;
            if (OnSended != null) client.OnSended += OnSended;
            if (OnError != null) client.OnError += OnError;

            //接続完了イベントを発生させる
            if (OnConnected != null)
            {
                SocketEventArgs args = new SocketEventArgs()
                {
                    LocalIPorHost = client.LocalEndPoint.Address.ToString(),
                    RemoteIPorHost = client.RemoteEndPoint.Address.ToString(),
                    RemotePort = client.RemoteEndPoint.Port,
                    LocalPort = client.LocalEndPoint.Port,
                    SendBytes = null,
                    RecvBytes = null,
                    Encoding = null,
                    WorkSocket = client.Client,
                };

                OnConnected(this, args);
            }

            //クライアントをよけておく
            _client_list.Add(client);

            //端末からデータ受信を開始する
            client.StartReceive();

        }

        /// <summary>
        /// 終了するときはCloseする
        /// </summary>
        public void Close()
        {
            //ループを止める
            this.Cancel = true;

            //ソケット通信を終了
            if (this.Status != EnumServerState.None)
            {
                //リスナーソケットを終了して破棄
                if (_client_list.Count > 0)
                {
                    //すべてのクライアントを遮断
                    foreach(ClientSocket client in _client_list)
                    {
                        if (client.Client != null)
                        {
                            client.Client.Shutdown(SocketShutdown.Both);
                        }
                    }
                    //_listener_socket.Shutdown(SocketShutdown.Both);
                    _client_list.Clear();
                }
                _listener_socket.Close();
                this.Status = EnumServerState.None;
                _listener_socket.Dispose();
                _listener_socket = null;
            }
        }

        public void OnHostDisConnected(object sendor, SocketEventArgs e)
        {
            //同じものを検索する
            ClientSocket client = _client_list.FirstOrDefault(x => x.Client == e.WorkSocket);

            //接続されているものがあったら取り除く
            if (client != null)
            {
                _client_list.Remove(client);
            }

            //切断イベントを発生させる
            if (OnDisConnected != null) OnDisConnected(sendor, e);
        }


        public void Send(string msg, params string[] ips)
        {
            //バイト配列にする
            byte[] buffer = this.Encoding.GetBytes(msg);

            Send(buffer, ips);
        }

        public void Send(byte[] buffer, params string[] ips)
        {
            List<ClientSocket> socket_list = new List<ClientSocket>();

            foreach (string ip_post in ips)
            {
                string ip = ip_post.Split(':')[0];
                string port = ip_post.Split(':')[1];

                //現在接続されているsocketを検索
                ClientSocket socket = _client_list.FirstOrDefault(x => x.RemoteEndPoint.Address.ToString() == ip && x.RemoteEndPoint.Port.ToString() == port);

                //なければ送信対象としない
                if (socket == null) continue;

                //送信対象として追加
                socket_list.Add(socket);
            }

            Send(buffer, socket_list.ToArray());
        }

        public void Send(string msg, ClientSocket[] sockets)
        {
            //バイト配列にする
            byte[] buffer = this.Encoding.GetBytes(msg);

            Send(buffer, sockets);
        }

        public void Send(byte[] buffer, ClientSocket[] sockets)
        {

            //対象ソケットの指定がない場合、全体に送る
            if (sockets == null)
            {
                sockets = _client_list.ToArray();
            }

            //送り先がない場合は、全体に送る
            if (sockets.Length == 0)
            {
                sockets = _client_list.ToArray();
            }

            //送信処理
            foreach (ClientSocket client in sockets)
            {
                client.Send(buffer);
            }

        }
    }
}

