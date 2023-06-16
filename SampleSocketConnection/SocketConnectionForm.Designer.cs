namespace SampleSocketConnection
{
    partial class SocketConnectionForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.grpKind = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.rdoClient = new System.Windows.Forms.RadioButton();
            this.rdoListener = new System.Windows.Forms.RadioButton();
            this.txtIpAddress = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSendText = new System.Windows.Forms.TextBox();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnSend = new System.Windows.Forms.Button();
            this.lstIpAdresses = new System.Windows.Forms.ListBox();
            this.btnEnd = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnConn = new System.Windows.Forms.Button();
            this.pnlIP = new System.Windows.Forms.Panel();
            this.flpIPPort = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlPort = new System.Windows.Forms.Panel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.pnlIpList = new System.Windows.Forms.Panel();
            this.grpKind.SuspendLayout();
            this.pnlIP.SuspendLayout();
            this.flpIPPort.SuspendLayout();
            this.pnlPort.SuspendLayout();
            this.pnlIpList.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpKind
            // 
            this.grpKind.Controls.Add(this.label7);
            this.grpKind.Controls.Add(this.rdoClient);
            this.grpKind.Controls.Add(this.rdoListener);
            this.grpKind.Location = new System.Drawing.Point(12, 12);
            this.grpKind.Name = "grpKind";
            this.grpKind.Size = new System.Drawing.Size(244, 62);
            this.grpKind.TabIndex = 0;
            this.grpKind.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 20);
            this.label7.TabIndex = 9;
            this.label7.Text = "種類";
            // 
            // rdoClient
            // 
            this.rdoClient.AutoSize = true;
            this.rdoClient.Location = new System.Drawing.Point(129, 27);
            this.rdoClient.Name = "rdoClient";
            this.rdoClient.Size = new System.Drawing.Size(96, 24);
            this.rdoClient.TabIndex = 1;
            this.rdoClient.Text = "クライアント";
            this.rdoClient.UseVisualStyleBackColor = true;
            this.rdoClient.CheckedChanged += new System.EventHandler(this.KindRadioButton_CheckedChanged);
            // 
            // rdoListener
            // 
            this.rdoListener.AutoSize = true;
            this.rdoListener.Checked = true;
            this.rdoListener.Location = new System.Drawing.Point(6, 27);
            this.rdoListener.Name = "rdoListener";
            this.rdoListener.Size = new System.Drawing.Size(74, 24);
            this.rdoListener.TabIndex = 0;
            this.rdoListener.TabStop = true;
            this.rdoListener.Text = "リスナー";
            this.rdoListener.UseVisualStyleBackColor = true;
            this.rdoListener.CheckedChanged += new System.EventHandler(this.KindRadioButton_CheckedChanged);
            // 
            // txtIpAddress
            // 
            this.txtIpAddress.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(70)))));
            this.txtIpAddress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.txtIpAddress.Location = new System.Drawing.Point(6, 31);
            this.txtIpAddress.MaxLength = 19;
            this.txtIpAddress.Name = "txtIpAddress";
            this.txtIpAddress.Size = new System.Drawing.Size(174, 28);
            this.txtIpAddress.TabIndex = 1;
            this.txtIpAddress.Text = "999.999.999.999";
            this.txtIpAddress.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "IPアドレス";
            // 
            // txtPort
            // 
            this.txtPort.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(70)))));
            this.txtPort.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.txtPort.Location = new System.Drawing.Point(3, 31);
            this.txtPort.MaxLength = 5;
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(89, 28);
            this.txtPort.TabIndex = 1;
            this.txtPort.Text = "99999";
            this.txtPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 20);
            this.label2.TabIndex = 0;
            this.label2.Text = "ポート";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 131);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 20);
            this.label3.TabIndex = 3;
            this.label3.Text = "送信メッセージ";
            // 
            // txtSendText
            // 
            this.txtSendText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSendText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(70)))));
            this.txtSendText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.txtSendText.Location = new System.Drawing.Point(12, 154);
            this.txtSendText.Multiline = true;
            this.txtSendText.Name = "txtSendText";
            this.txtSendText.Size = new System.Drawing.Size(641, 73);
            this.txtSendText.TabIndex = 4;
            this.txtSendText.Text = "テスト\r\nて\r\nて";
            // 
            // txtLog
            // 
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(70)))));
            this.txtLog.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.txtLog.Location = new System.Drawing.Point(12, 253);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(760, 252);
            this.txtLog.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 230);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 20);
            this.label4.TabIndex = 6;
            this.label4.Text = "送受信ログ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 5);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(120, 20);
            this.label5.TabIndex = 0;
            this.label5.Text = "接続中IPアドレス";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(186, 35);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(16, 20);
            this.label6.TabIndex = 2;
            this.label6.Text = ":";
            // 
            // btnSend
            // 
            this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSend.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(173)))), ((int)(((byte)(181)))));
            this.btnSend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSend.Location = new System.Drawing.Point(659, 177);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(113, 50);
            this.btnSend.TabIndex = 5;
            this.btnSend.Text = "送信";
            this.btnSend.UseVisualStyleBackColor = false;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // lstIpAdresses
            // 
            this.lstIpAdresses.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(70)))));
            this.lstIpAdresses.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.lstIpAdresses.FormattingEnabled = true;
            this.lstIpAdresses.ItemHeight = 20;
            this.lstIpAdresses.Items.AddRange(new object[] {
            "全体"});
            this.lstIpAdresses.Location = new System.Drawing.Point(6, 31);
            this.lstIpAdresses.Name = "lstIpAdresses";
            this.lstIpAdresses.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lstIpAdresses.Size = new System.Drawing.Size(178, 84);
            this.lstIpAdresses.TabIndex = 1;
            // 
            // btnEnd
            // 
            this.btnEnd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEnd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(173)))), ((int)(((byte)(181)))));
            this.btnEnd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnd.Location = new System.Drawing.Point(659, 511);
            this.btnEnd.Name = "btnEnd";
            this.btnEnd.Size = new System.Drawing.Size(113, 38);
            this.btnEnd.TabIndex = 9;
            this.btnEnd.Text = "終了";
            this.btnEnd.UseVisualStyleBackColor = false;
            this.btnEnd.Click += new System.EventHandler(this.btnEnd_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(173)))), ((int)(((byte)(181)))));
            this.btnClose.Enabled = false;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Location = new System.Drawing.Point(12, 511);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(113, 38);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "切断";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnConn
            // 
            this.btnConn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(173)))), ((int)(((byte)(181)))));
            this.btnConn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConn.Location = new System.Drawing.Point(12, 80);
            this.btnConn.Name = "btnConn";
            this.btnConn.Size = new System.Drawing.Size(100, 38);
            this.btnConn.TabIndex = 2;
            this.btnConn.Text = "接続";
            this.btnConn.UseVisualStyleBackColor = false;
            this.btnConn.Click += new System.EventHandler(this.btnConn_Click);
            // 
            // pnlIP
            // 
            this.pnlIP.Controls.Add(this.label6);
            this.pnlIP.Controls.Add(this.label1);
            this.pnlIP.Controls.Add(this.txtIpAddress);
            this.pnlIP.Location = new System.Drawing.Point(3, 3);
            this.pnlIP.Name = "pnlIP";
            this.pnlIP.Size = new System.Drawing.Size(206, 62);
            this.pnlIP.TabIndex = 0;
            // 
            // flpIPPort
            // 
            this.flpIPPort.Controls.Add(this.pnlIP);
            this.flpIPPort.Controls.Add(this.pnlPort);
            this.flpIPPort.Controls.Add(this.pnlIpList);
            this.flpIPPort.Location = new System.Drawing.Point(262, 12);
            this.flpIPPort.Name = "flpIPPort";
            this.flpIPPort.Size = new System.Drawing.Size(520, 139);
            this.flpIPPort.TabIndex = 1;
            // 
            // pnlPort
            // 
            this.pnlPort.Controls.Add(this.txtPort);
            this.pnlPort.Controls.Add(this.label2);
            this.pnlPort.Location = new System.Drawing.Point(215, 3);
            this.pnlPort.Name = "pnlPort";
            this.pnlPort.Size = new System.Drawing.Size(98, 62);
            this.pnlPort.TabIndex = 1;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // pnlIpList
            // 
            this.pnlIpList.Controls.Add(this.label5);
            this.pnlIpList.Controls.Add(this.lstIpAdresses);
            this.pnlIpList.Location = new System.Drawing.Point(319, 3);
            this.pnlIpList.Name = "pnlIpList";
            this.pnlIpList.Size = new System.Drawing.Size(190, 133);
            this.pnlIpList.TabIndex = 2;
            // 
            // SocketConnectionForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(40)))), ((int)(((byte)(49)))));
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.flpIPPort);
            this.Controls.Add(this.btnConn);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnEnd);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.txtSendText);
            this.Controls.Add(this.grpKind);
            this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "SocketConnectionForm";
            this.Text = "SocketConnectionSample";
            this.Load += new System.EventHandler(this.SocketConnectionForm_Load);
            this.grpKind.ResumeLayout(false);
            this.grpKind.PerformLayout();
            this.pnlIP.ResumeLayout(false);
            this.pnlIP.PerformLayout();
            this.flpIPPort.ResumeLayout(false);
            this.pnlPort.ResumeLayout(false);
            this.pnlPort.PerformLayout();
            this.pnlIpList.ResumeLayout(false);
            this.pnlIpList.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpKind;
        private System.Windows.Forms.RadioButton rdoListener;
        private System.Windows.Forms.RadioButton rdoClient;
        private System.Windows.Forms.TextBox txtIpAddress;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSendText;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.ListBox lstIpAdresses;
        private System.Windows.Forms.Button btnEnd;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnConn;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel pnlIP;
        private System.Windows.Forms.FlowLayoutPanel flpIPPort;
        private System.Windows.Forms.Panel pnlPort;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel pnlIpList;
    }
}

