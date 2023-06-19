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
using System.Diagnostics;

namespace SampleSocketConnection
{
    internal class ClientSocket : IDisposable
    {
        private Socket _socket = null;
        private IPEndPoint _localEndPoint;
        private IPEndPoint _remoteEndPoint;
        private bool _recving_flg = false;

        //エラー情報
        private string _errmsg = "";
        private string _errstack = "";

        public delegate void SocketEventHandller(object sendor, SocketEventArgs e);
        public delegate void SocketErrorEventHandller(object sendor, SocketErrorEventArgs e);

        /// <summary>
        /// サーバーに接続した
        /// </summary>
        public event SocketEventHandller OnConnected = null;
        public event SocketEventHandller OnDisConnected = null;
        public event SocketEventHandller OnRecved = null;
        public event SocketEventHandller OnSended = null;
        public event SocketErrorEventHandller OnError = null;

        protected virtual void OnConnectedEvent(SocketEventArgs e)
        {
            if (this.OnConnected != null)
            {
                this.OnConnected(this, e);
            }
        }
        protected virtual void OnRecvedEvent(SocketEventArgs e)
        {
            if (this.OnRecved != null)
            {
                this.OnRecved(this, e);
            }
        }
        protected virtual void OnSendedEvent(SocketEventArgs e)
        {
            if (this.OnSended != null)
            {
                this.OnSended(this, e);
            }
        }
        protected virtual void OnDisConnectedEvent(SocketEventArgs e)
        {
            if (this.OnDisConnected != null)
            {
                this.OnDisConnected(this, e);
            }
        }
        protected virtual void OnErrorEvent(string error_message, string error_stack)
        {

            SocketErrorEventArgs args = new SocketErrorEventArgs(error_message, error_stack);

            OnErrorEvent(args);
        }
        
        protected virtual void OnErrorEvent(SocketErrorEventArgs e)
        {

            if (this.OnError != null)
            {
                this.OnError(this, e);
            }
        }

        /// <summary>
        /// 閉じているか
        /// </summary>
        public bool IsClosed
        {
            get
            {
                return (_socket == null);
            }
        }
        public Socket Client
        {
            get
            {
                return _socket;
            }
        }

        /// <summary>
        /// ローカルエンドポイント
        /// </summary>
        public IPEndPoint LocalEndPoint
        {
            get
            {
                return _localEndPoint;
            }
        }

        /// <summary>
        /// ローカルエンドポイント
        /// </summary>
        public IPEndPoint RemoteEndPoint
        {
            get
            {
                return _remoteEndPoint;
            }
        }

        /// <summary>
        /// 使用する文字コード
        /// </summary>
        protected System.Text.Encoding Encoding { get; set; } = System.Text.Encoding.UTF8;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ClientSocket(Socket soc = null)
        {

            if (soc == null)
            {
                this._socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            } else
            {
                _socket = soc;
                _localEndPoint = (IPEndPoint)soc.LocalEndPoint;
                _remoteEndPoint = (IPEndPoint)soc.RemoteEndPoint;
            }

            this.Initialize();

        }

        private void Initialize()
        {
        }

        /// <summary>
        /// Hostに接続
        /// </summary>
        /// <param name="host">接続先ホスト</param>
        /// <param name="port">ポート</param>
        public bool Connect(string host, int port)
        {
            try
            {
                //コネクションが閉じているときは新しく作成する
                if (this.IsClosed)
                {
                    if (_socket != null) this.Close();

                    _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    _recving_flg = false;


                }

                //既に接続されている場合は、接続処理を行う
                if (_socket.Connected)
                {
                    throw new ApplicationException("すでに接続されています。");
                }

                //接続する
                //IPEndPoint ipEnd = new IPEndPoint(Dns.Resolve(host).AddressList[0], port);
                //this._socket.Connect(ipEnd);
                IPEndPoint ipEnd = new IPEndPoint(Dns.GetHostAddresses(host)[0], port);
                _socket.Connect(ipEnd);
                //this._socket.Connect(host, port);

                _localEndPoint = (IPEndPoint)_socket.LocalEndPoint;
                _remoteEndPoint = (IPEndPoint)_socket.RemoteEndPoint;

                //接続情報を送信
                SocketEventArgs args = new SocketEventArgs()
                {
                    RemoteIPorHost = _remoteEndPoint.Address.ToString(),
                    LocalIPorHost = _localEndPoint.Address.ToString(),
                    RemotePort = _remoteEndPoint.Port,
                    LocalPort = _localEndPoint.Port,
                    SendBytes = null,
                    RecvBytes = null,
                    Encoding = this.Encoding,
                    WorkSocket = _socket
                };

                //イベントを発生
                OnConnectedEvent(args);

                //非同期データ受信を開始する
                this.StartReceive();

            }
            catch (Exception ex)
            {
                _errmsg = ex.Message;
                _errstack = ex.StackTrace;

                OnErrorEvent(ex.Message, ex.StackTrace);

                return false;
            }

            return true;

        }


        /// <summary>
        /// SocketClose
        /// </summary>
        public bool Close()
        {
            Debug.WriteLine("Close" + " ThreadID:" + Thread.CurrentThread.ManagedThreadId);

            try
            {
                if (this.IsClosed)
                    return true;

                //接続情報を送信
                SocketEventArgs args = new SocketEventArgs()
                {
                    RemoteIPorHost = _remoteEndPoint.Address.ToString(),
                    LocalIPorHost = _localEndPoint.Address.ToString(),
                    RemotePort = _remoteEndPoint.Port,
                    LocalPort = _localEndPoint.Port,
                    SendBytes = null,
                    RecvBytes = null,
                    Encoding = this.Encoding,
                    WorkSocket = _socket
                };

                //切断
                _socket.Shutdown(SocketShutdown.Both);

                //イベントを発生
                this.OnDisConnectedEvent(args);

                //ソケットを破棄
                _socket.Close();
                _socket.Dispose();
                _socket = null;


            }
            catch (Exception ex)
            {
                _errmsg = ex.Message;
                _errstack = ex.StackTrace;

                OnErrorEvent(ex.Message, ex.StackTrace);

                return false;
            }

            return true;
        }


        /// <summary>
        /// データの非同期受信を開始する
        /// </summary>
        public void StartReceive()
        {
            if (this.IsClosed)
                throw new ApplicationException("閉じています。");
            if (_recving_flg)
                throw new ApplicationException("StartReceiveがすでに呼び出されています。");

            //初期化
            byte[] recv_buffer = new byte[1024];
            _recving_flg = true;

            SocketStateClass state = new SocketStateClass()
            {
                Buffer = recv_buffer,
                Encoding = this.Encoding,
                WorkSocket = _socket
            };


            //非同期受信を開始
            this._socket.BeginReceive(recv_buffer, 0, recv_buffer.Length, SocketFlags.None, new AsyncCallback(RecvCallback), state);
        }

        //BeginReceiveのコールバック
        private void RecvCallback(IAsyncResult ar)
        {
            int len = -1;
            //読み込んだ長さを取得
            try
            {
                if (_socket == null) return;

                len = _socket.EndReceive(ar);

                //切断されたか調べる
                if (len <= 0)
                {
                    this.Close();
                    return;
                }

                //受信したデータを取得する
                SocketStateClass state = (SocketStateClass)ar.AsyncState;

                //受信イベントを発生させる
                SocketEventArgs args = new SocketEventArgs()
                {
                    RemoteIPorHost = ((IPEndPoint)state.WorkSocket.RemoteEndPoint).Address.ToString(),
                    LocalIPorHost = ((IPEndPoint)state.WorkSocket.LocalEndPoint).Address.ToString(),
                    RemotePort = ((IPEndPoint)state.WorkSocket.RemoteEndPoint).Port,
                    LocalPort = ((IPEndPoint)state.WorkSocket.LocalEndPoint).Port,
                    SendBytes = null,
                    RecvBytes = state.Buffer,
                    Encoding = state.Encoding,
                    WorkSocket = state.WorkSocket
                };

                OnRecvedEvent(args);

                if (_socket != null)
                {
                    //再び受信開始
                    _socket.BeginReceive(state.Buffer, 0, state.Buffer.Length, SocketFlags.None, new AsyncCallback(RecvCallback), state);

                }
            }
            catch(Exception ex)
            {
                _errmsg = ex.Message;
                _errstack = ex.StackTrace;

                OnErrorEvent(ex.Message, ex.StackTrace);
            }

        }


        /// <summary>
        /// メッセージ送信
        /// </summary>
        /// <param name="msg">メッセージ</param>
        public void Send(string msg)
        {
            //バイト配列にする
            byte[] buffer = this.Encoding.GetBytes(msg);

            //送信
            Send(buffer);
        }

        /// <summary>
        /// データ送信
        /// </summary>
        /// <param name="buffer">データ</param>
        public void Send(byte[] buffer)
        {
            if (this.IsClosed)
                throw new ApplicationException("閉じています。");

            SocketStateClass state = new SocketStateClass()
            {
                Buffer = buffer,
                Encoding = this.Encoding,
                WorkSocket = _socket
            };

            //データを送信する
            _socket.BeginSend(buffer, 0, buffer.Length, 0, new AsyncCallback(CallBackSend), state);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ar"></param>
        private void CallBackSend(IAsyncResult ar)
        {
            try
            {
                // 非同期オブジェクトからソケット情報を取得
                SocketStateClass state = (SocketStateClass)ar.AsyncState;

                // クライアントへデータ送信完了
                int bytesSent = state.WorkSocket.EndSend(ar);

                //接続情報を送信
                SocketEventArgs args = new SocketEventArgs()
                {
                    RemoteIPorHost = ((IPEndPoint)state.WorkSocket.RemoteEndPoint).Address.ToString(),
                    LocalIPorHost = ((IPEndPoint)state.WorkSocket.LocalEndPoint).Address.ToString(),
                    RemotePort = ((IPEndPoint)state.WorkSocket.RemoteEndPoint).Port,
                    LocalPort = ((IPEndPoint)state.WorkSocket.LocalEndPoint).Port,
                    SendBytes = state.Buffer,
                    RecvBytes = null,
                    Encoding = this.Encoding,
                    WorkSocket = state.WorkSocket
                };


                OnSendedEvent(args);


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        #region dispose

        private bool disposedValue;
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

    }
}
