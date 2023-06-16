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
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace SampleSocketConnection
{
    public partial class SocketConnectionForm : Form
    {
        /// <summary>IPアドレスの正規表現</summary>
        const string CONST_REGEX_IPADDRESS = @"^((25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])\.){3}(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])$";

        private static Queue<string> _que_log = new Queue<string>();    //ログのキュー(先入れ先出し)
        private static List<string> _connect_client_list = new List<string> { "全体" };

        private ListenerSocket _listener = null;
        private ClientSocket _client = null;
        private bool _ipaddress_refresh_flg = false;

        public SocketConnectionForm()
        {
            InitializeComponent();
        }

        #region event

        private void SocketConnectionForm_Load(object sender, EventArgs e)
        {
            Initlize();
        }

        #region button event

        /// <summary>
        /// 終了ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEnd_Click(object sender, EventArgs e)
        {
            //画面を閉じる
            this.Close();
        }

        /// <summary>
        /// 接続ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConn_Click(object sender, EventArgs e)
        {
            ushort port;     //ポートは0～65535なのでその範囲の数値型を宣言。

            //エラーチェック(ポート番号数値チェック)
            if (ushort.TryParse(this.txtPort.Text, out port) == false)
            {
                this.txtPort.Select();
                MessageBox.Show("入力されたポートが有効な数値ではありません。");
                return;
            }

            ////ポートが利用中か調べる
            //IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            //IPEndPoint[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpListeners();

            ////利用中ポートを検索し見つかったらエラーとする
            //if (tcpConnInfoArray.FirstOrDefault(x => x.Port == port) != null)
            //{
            //    this.txtPort.Select();
            //    MessageBox.Show("ポート使用中です。");
            //    return;
            //}

            //チェックボックス毎の処理
            if (this.rdoListener.Checked == true)
            {
                //Listnerが選ばれているとき、クライアント待ち状態にする
                _listener = new ListenerSocket();

                //イベントの設定
                _listener.OnConnected += OnConnected;
                _listener.OnDisConnected += OnDisConnected;
                _listener.OnRecved += OnRecved;
                _listener.OnSended += OnSended;
                _listener.OnError += OnError;

                _listener.StartListening(port);

                LogAppend("リスナーを「開始」しました。");
            }
            else if (this.rdoClient.Checked == true)
            {
                //Clientが選ばれているとき、Listnerに接続する

                string ip = this.txtIpAddress.Text;

                if (System.Text.RegularExpressions.Regex.IsMatch(ip, CONST_REGEX_IPADDRESS) == false)
                {
                    this.txtIpAddress.Select();
                    MessageBox.Show("入力されたIPアドレスは正しくありません。");
                    return;
                }

                _client = new ClientSocket();

                //イベントの設定
                _client.OnConnected += OnConnected;
                _client.OnDisConnected += OnDisConnected;
                _client.OnRecved += OnRecved;
                _client.OnSended += OnSended;
                _client.OnError += OnError;

                _client.Connect(ip, port);
            }

            //接続されたときにボタンのEnabledを変更する
            EnabledControls(false);

        }

        /// <summary>
        /// 送信ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSend_Click(object sender, EventArgs e)
        {
            //チェックボックス毎の処理
            if (this.rdoListener.Checked == true)
            {
                if (this.lstIpAdresses.SelectedIndices.Contains(0))
                {
                    //全体へ送信
                    _listener.Send(this.txtSendText.Text);

                } 
                else
                {
                    List<string> adresses = new List<string>();

                    foreach (int index in this.lstIpAdresses.SelectedIndices)
                    {
                        //全体は外す
                        if (index == 0)
                        {
                            continue;
                        }

                        //送信対象として追加
                        adresses.Add(this.lstIpAdresses.Items[0].ToString());
                    }

                    _listener.Send(this.txtSendText.Text, adresses.ToArray());

                }
            }
            else if (this.rdoClient.Checked == true)
            {
                //メッセージの送信
                _client.Send(this.txtSendText.Text);
            }

        }

        /// <summary>
        /// 切断ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            if (_listener != null)
            {
                _listener.Close();
                _listener.Dispose();
                _listener = null;
                _que_log.Enqueue("リスナーを「終了」しました。");
            }
            if (_client != null)
            {
                _client.Close();
                _client.Dispose();
                _client = null;
                _que_log.Enqueue("クライアントをサーバーから「切断」しました。");
            }

            EnabledControls(true);
        }

        /// <summary>
        /// ListenerとClientボタンを切り替えたとき
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KindRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            //選択されている内容でコントロールを切り替える
            if (this.rdoListener.Checked == true)
            {
                //Listnerが選ばれているとき、IPアドレスの指定はしない
                this.pnlIP.Visible = false;

                //リストは表示する
                this.pnlIpList.Visible = true;

            }
            else if (this.rdoClient.Checked == true)
            {
                //Clientが選ばれているとき、IPアドレスの指定をする
                this.pnlIP.Visible = true;

                //リストは表示しない
                this.pnlIpList.Visible = false;
            }
        }

        #endregion

        /// <summary>
        /// タイマーイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            //ログに追加するものがなくなるまで繰り返す
            while (_que_log.Count > 0)
            {
                //ログ情報の追加
                LogAppend(_que_log.Dequeue());

                //接続状況を更新(更新フラグが立っていたら)
                if (_ipaddress_refresh_flg == true)
                {
                    this.lstIpAdresses.DataSource = null;
                    this.lstIpAdresses.DataSource = _connect_client_list;
                    _ipaddress_refresh_flg = false;
                }
            }
        }

        #region ソケット通信イベント
        public void OnConnected(object sendor, SocketEventArgs e)
        {
            const string MSG = "{0}({2})が{1}({3})に接続しました。";

            if (this.rdoListener.Checked == true)
            {
                _connect_client_list.Add($"{e.RemoteIPorHost}:{e.RemotePort}");
                _ipaddress_refresh_flg = true;
            }

            //ログ表示
            _que_log.Enqueue(string.Format(MSG, e.LocalIPorHost, e.RemoteIPorHost, e.RemotePort, e.LocalPort));


        }

        public void OnDisConnected(object sendor, SocketEventArgs e)
        {
            const string MSG = "{0}({1})と切断しました。";

            if (this.rdoListener.Checked == true)
            {
                _connect_client_list.Remove($"{e.RemoteIPorHost}:{e.RemotePort}");
                _ipaddress_refresh_flg = true;
            }

            //ログ表示
            _que_log.Enqueue(string.Format(MSG, e.RemoteIPorHost, e.RemotePort));
        }

        public void OnRecved(object sendor, SocketEventArgs e)
        {
            //ログ表示
            _que_log.Enqueue(e.GetRecvString());
        }

        public void OnSended(object sendor, SocketEventArgs e)
        {
            //ログ表示
            _que_log.Enqueue(e.GetSendString());
        }

        public void OnError(object sendor, SocketErrorEventArgs e)
        {
            //ログ表示
            _que_log.Enqueue(e.ErrorMessage + "\r\n" + e.ErrorStack);
        }

        #endregion

        #endregion

        #region method

        /// <summary>
        /// 初期化
        /// </summary>
        private void Initlize()
        {
            //各コントロールを初期化
            this.rdoListener.Checked = true;    //リスナー状態を初期値
            this.pnlIP.Visible = false;         //リスナの時はIP、portはいらないので隠す
            this.txtIpAddress.Text = "";        //IPアドレス
            this.txtPort.Text = "";             //ポート
            this.lstIpAdresses.DataSource = _connect_client_list;   //IPアドレスリスト
            this.txtSendText.Text = "";         //送信テキスト
            this.txtLog.Text = "";              //ログ
        }

        /// <summary>
        /// 送受信ログテキストをクリア
        /// </summary>
        private void LogClear()
        {
            //テキストボックスの中をクリア
            this.txtLog.Clear();
        }

        /// <summary>
        /// 送受信ログテキストボックスへ追記
        /// </summary>
        /// <param name="format">ログ出力内容を記載</param>
        /// <param name="args">パラメータ</param>
        private void LogAppend(string format, params object[] args)
        {
            //出力内容を取得(三項演算子を使ってます。)
            string log = args.Length == 0 ? format : string.Format(format, args);

            //ログを出力
            this.txtLog.AppendText(log + "\r\n");
        }

        /// <summary>
        /// 接続時にボタンを変更できないようにする
        /// </summary>
        /// <param name="enabled"></param>
        private void SetControlEnabledforConnect(bool enabled)
        {
            this.grpKind.Enabled = enabled;
            this.flpIPPort.Enabled = enabled;
        }

        private void EnabledControls(bool enabled)
        {
            this.btnConn.Enabled = enabled;
            this.txtIpAddress.ReadOnly = !enabled;
            this.txtPort.ReadOnly = !enabled;
            this.btnClose.Enabled = !enabled;
        }

        #endregion

    }
}
