using mRemoteNG.App;
using System;
using System.IO;
using System.Threading;
using mRemoteNG.Tools;
using WeifenLuo.WinFormsUI.Docking;
using System.Windows.Forms;
using mRemoteNG.Messages;
using mRemoteNG.UI.Controls;
using mRemoteNG.UI.Forms;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.UI.Window
{
    public class SshTransferWindow : BaseWindow
    {
        #region Form Init

        private MrngProgressBar _pbStatus;
        private MrngButton _btnTransfer;
        private MrngTextBox _txtUser;
        private MrngTextBox _txtPassword;
        private MrngTextBox _txtHost;
        private MrngTextBox _txtPort;
        private MrngLabel _lblHost;
        private MrngLabel _lblPort;
        private MrngLabel _lblUser;
        private MrngLabel _lblPassword;
        private MrngLabel _lblProtocol;
        private MrngRadioButton _radProtScp;
        private MrngRadioButton _radProtSftp;
        private MrngGroupBox _grpConnection;
        private MrngButton _btnBrowse;
        private MrngLabel _lblRemoteFile;
        private MrngTextBox _txtRemoteFile;
        private MrngTextBox _txtLocalFile;
        private MrngLabel _lblLocalFile;
        private MrngGroupBox _grpFiles;

        private void InitializeComponent()
        {
            var resources =
                new System.ComponentModel.ComponentResourceManager(typeof(SshTransferWindow));
            _grpFiles = new MrngGroupBox();
            _lblLocalFile = new MrngLabel();
            _txtLocalFile = new MrngTextBox();
            _btnTransfer = new MrngButton();
            _txtRemoteFile = new MrngTextBox();
            _lblRemoteFile = new MrngLabel();
            _btnBrowse = new MrngButton();
            _grpConnection = new MrngGroupBox();
            _radProtSftp = new MrngRadioButton();
            _radProtScp = new MrngRadioButton();
            _lblProtocol = new MrngLabel();
            _lblPassword = new MrngLabel();
            _lblUser = new MrngLabel();
            _lblPort = new MrngLabel();
            _lblHost = new MrngLabel();
            _txtPort = new MrngTextBox();
            _txtHost = new MrngTextBox();
            _txtPassword = new MrngTextBox();
            _txtUser = new MrngTextBox();
            _pbStatus = new MrngProgressBar();
            _grpFiles.SuspendLayout();
            _grpConnection.SuspendLayout();
            SuspendLayout();
            // 
            // grpFiles
            // 
            _grpFiles.Controls.Add(_lblLocalFile);
            _grpFiles.Controls.Add(_txtLocalFile);
            _grpFiles.Controls.Add(_btnTransfer);
            _grpFiles.Controls.Add(_txtRemoteFile);
            _grpFiles.Controls.Add(_lblRemoteFile);
            _grpFiles.Controls.Add(_btnBrowse);
            _grpFiles.FlatStyle = FlatStyle.Flat;
            _grpFiles.Location = new System.Drawing.Point(12, 172);
            _grpFiles.Name = "_grpFiles";
            _grpFiles.Size = new System.Drawing.Size(668, 175);
            _grpFiles.TabIndex = 2000;
            _grpFiles.TabStop = false;
            _grpFiles.Text = "Files";
            // 
            // lblLocalFile
            // 
            _lblLocalFile.AutoSize = true;
            _lblLocalFile.Location = new System.Drawing.Point(6, 30);
            _lblLocalFile.Name = "_lblLocalFile";
            _lblLocalFile.Size = new System.Drawing.Size(55, 13);
            _lblLocalFile.TabIndex = 10;
            _lblLocalFile.Text = "Local file:";
            // 
            // txtLocalFile
            // 
            _txtLocalFile.BorderStyle = BorderStyle.FixedSingle;
            _txtLocalFile.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular,
                                                             System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            _txtLocalFile.Location = new System.Drawing.Point(105, 28);
            _txtLocalFile.Name = "_txtLocalFile";
            _txtLocalFile.Size = new System.Drawing.Size(455, 22);
            _txtLocalFile.TabIndex = 20;
            // 
            // btnTransfer
            // 
            _btnTransfer.Mice = MrngButton.MouseState.Hover;
            _btnTransfer.FlatStyle = FlatStyle.Flat;
            _btnTransfer.Image = Properties.Resources.SyncArrow_16x;
            _btnTransfer.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            _btnTransfer.Location = new System.Drawing.Point(562, 145);
            _btnTransfer.Name = "_btnTransfer";
            _btnTransfer.Size = new System.Drawing.Size(100, 24);
            _btnTransfer.TabIndex = 10000;
            _btnTransfer.Text = "Transfer";
            _btnTransfer.UseVisualStyleBackColor = true;
            _btnTransfer.Click += new EventHandler(btnTransfer_Click);
            // 
            // txtRemoteFile
            // 
            _txtRemoteFile.BorderStyle = BorderStyle.FixedSingle;
            _txtRemoteFile.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular,
                                                              System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            _txtRemoteFile.Location = new System.Drawing.Point(105, 60);
            _txtRemoteFile.Name = "_txtRemoteFile";
            _txtRemoteFile.Size = new System.Drawing.Size(542, 22);
            _txtRemoteFile.TabIndex = 50;
            // 
            // lblRemoteFile
            // 
            _lblRemoteFile.AutoSize = true;
            _lblRemoteFile.Location = new System.Drawing.Point(6, 67);
            _lblRemoteFile.Name = "_lblRemoteFile";
            _lblRemoteFile.Size = new System.Drawing.Size(68, 13);
            _lblRemoteFile.TabIndex = 40;
            _lblRemoteFile.Text = "Remote file:";
            // 
            // btnBrowse
            // 
            _btnBrowse.Mice = MrngButton.MouseState.Hover;
            _btnBrowse.FlatStyle = FlatStyle.Flat;
            _btnBrowse.Location = new System.Drawing.Point(566, 28);
            _btnBrowse.Name = "_btnBrowse";
            _btnBrowse.Size = new System.Drawing.Size(81, 22);
            _btnBrowse.TabIndex = 30;
            _btnBrowse.Text = "Browse";
            _btnBrowse.UseVisualStyleBackColor = true;
            _btnBrowse.Click += new EventHandler(btnBrowse_Click);
            // 
            // grpConnection
            // 
            _grpConnection.Controls.Add(_radProtSftp);
            _grpConnection.Controls.Add(_radProtScp);
            _grpConnection.Controls.Add(_lblProtocol);
            _grpConnection.Controls.Add(_lblPassword);
            _grpConnection.Controls.Add(_lblUser);
            _grpConnection.Controls.Add(_lblPort);
            _grpConnection.Controls.Add(_lblHost);
            _grpConnection.Controls.Add(_txtPort);
            _grpConnection.Controls.Add(_txtHost);
            _grpConnection.Controls.Add(_txtPassword);
            _grpConnection.Controls.Add(_txtUser);
            _grpConnection.FlatStyle = FlatStyle.Flat;
            _grpConnection.Location = new System.Drawing.Point(12, 12);
            _grpConnection.Name = "_grpConnection";
            _grpConnection.Size = new System.Drawing.Size(668, 154);
            _grpConnection.TabIndex = 1000;
            _grpConnection.TabStop = false;
            _grpConnection.Text = "Connection";
            // 
            // radProtSFTP
            // 
            _radProtSftp.AutoSize = true;
            _radProtSftp.FlatStyle = FlatStyle.Flat;
            _radProtSftp.Location = new System.Drawing.Point(164, 113);
            _radProtSftp.Name = "_radProtSftp";
            _radProtSftp.Size = new System.Drawing.Size(47, 17);
            _radProtSftp.TabIndex = 90;
            _radProtSftp.Text = "SFTP";
            _radProtSftp.UseVisualStyleBackColor = true;
            // 
            // radProtSCP
            // 
            _radProtScp.AutoSize = true;
            _radProtScp.Checked = true;
            _radProtScp.FlatStyle = FlatStyle.Flat;
            _radProtScp.Location = new System.Drawing.Point(105, 113);
            _radProtScp.Name = "_radProtScp";
            _radProtScp.Size = new System.Drawing.Size(43, 17);
            _radProtScp.TabIndex = 80;
            _radProtScp.TabStop = true;
            _radProtScp.Text = "SCP";
            _radProtScp.UseVisualStyleBackColor = true;
            // 
            // lblProtocol
            // 
            _lblProtocol.AutoSize = true;
            _lblProtocol.Location = new System.Drawing.Point(6, 117);
            _lblProtocol.Name = "_lblProtocol";
            _lblProtocol.Size = new System.Drawing.Size(53, 13);
            _lblProtocol.TabIndex = 90;
            _lblProtocol.Text = "Protocol:";
            // 
            // lblPassword
            // 
            _lblPassword.AutoSize = true;
            _lblPassword.Location = new System.Drawing.Point(6, 88);
            _lblPassword.Name = "_lblPassword";
            _lblPassword.Size = new System.Drawing.Size(59, 13);
            _lblPassword.TabIndex = 70;
            _lblPassword.Text = "Password:";
            // 
            // lblUser
            // 
            _lblUser.AutoSize = true;
            _lblUser.Location = new System.Drawing.Point(6, 58);
            _lblUser.Name = "_lblUser";
            _lblUser.Size = new System.Drawing.Size(33, 13);
            _lblUser.TabIndex = 50;
            _lblUser.Text = "User:";
            // 
            // lblPort
            // 
            _lblPort.AutoSize = true;
            _lblPort.Location = new System.Drawing.Point(228, 115);
            _lblPort.Name = "_lblPort";
            _lblPort.Size = new System.Drawing.Size(31, 13);
            _lblPort.TabIndex = 30;
            _lblPort.Text = "Port:";
            // 
            // lblHost
            // 
            _lblHost.AutoSize = true;
            _lblHost.Location = new System.Drawing.Point(6, 27);
            _lblHost.Name = "_lblHost";
            _lblHost.Size = new System.Drawing.Size(34, 13);
            _lblHost.TabIndex = 10;
            _lblHost.Text = "Host:";
            // 
            // txtPort
            // 
            _txtPort.BorderStyle = BorderStyle.FixedSingle;
            _txtPort.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular,
                                                        System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            _txtPort.Location = new System.Drawing.Point(271, 110);
            _txtPort.Name = "_txtPort";
            _txtPort.Size = new System.Drawing.Size(30, 22);
            _txtPort.TabIndex = 100;
            _txtPort.Text = "22";
            _txtPort.TextAlign = HorizontalAlignment.Center;
            // 
            // txtHost
            // 
            _txtHost.BorderStyle = BorderStyle.FixedSingle;
            _txtHost.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular,
                                                        System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            _txtHost.Location = new System.Drawing.Point(105, 19);
            _txtHost.Name = "_txtHost";
            _txtHost.Size = new System.Drawing.Size(471, 22);
            _txtHost.TabIndex = 20;
            // 
            // txtPassword
            // 
            _txtPassword.BorderStyle = BorderStyle.FixedSingle;
            _txtPassword.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular,
                                                            System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            _txtPassword.Location = new System.Drawing.Point(105, 81);
            _txtPassword.Name = "_txtPassword";
            _txtPassword.Size = new System.Drawing.Size(471, 22);
            _txtPassword.TabIndex = 60;
            _txtPassword.UseSystemPasswordChar = true;
            // 
            // txtUser
            // 
            _txtUser.BorderStyle = BorderStyle.FixedSingle;
            _txtUser.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular,
                                                        System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            _txtUser.Location = new System.Drawing.Point(105, 51);
            _txtUser.Name = "_txtUser";
            _txtUser.Size = new System.Drawing.Size(471, 22);
            _txtUser.TabIndex = 40;
            // 
            // pbStatus
            // 
            _pbStatus.Location = new System.Drawing.Point(12, 353);
            _pbStatus.Name = "_pbStatus";
            _pbStatus.Size = new System.Drawing.Size(668, 23);
            _pbStatus.Style = ProgressBarStyle.Continuous;
            _pbStatus.TabIndex = 3000;
            // 
            // SSHTransferWindow
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new System.Drawing.Size(692, 423);
            Controls.Add(_grpFiles);
            Controls.Add(_grpConnection);
            Controls.Add(_pbStatus);
            Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular,
                                                System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Name = "SshTransferWindow";
            TabText = "SSH File Transfer";
            Text = "SSH File Transfer";
            Load += new EventHandler(SSHTransfer_Load);
            _grpFiles.ResumeLayout(false);
            _grpFiles.PerformLayout();
            _grpConnection.ResumeLayout(false);
            _grpConnection.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        #region Private Properties

        private readonly OpenFileDialog _oDlg;

        #endregion

        #region Public Properties

        public string Hostname
        {
            get => _txtHost.Text;
            set => _txtHost.Text = value;
        }

        public string Port
        {
            get => _txtPort.Text;
            set => _txtPort.Text = value;
        }

        public string Username
        {
            get => _txtUser.Text;
            set => _txtUser.Text = value;
        }

        public string Password
        {
            get => _txtPassword.Text;
            set => _txtPassword.Text = value;
        }

        #endregion

        #region Form Stuff

        private void SSHTransfer_Load(object sender, EventArgs e)
        {
            ApplyTheme();
            ApplyLanguage();
            Icon = Resources.ImageConverter.GetImageAsIcon(Properties.Resources.SyncArrow_16x);
            var display = new DisplayProperties();
            _btnTransfer.Image = display.ScaleImage(_btnTransfer.Image);
        }

        private void ApplyLanguage()
        {
            _grpFiles.Text = Language.Files;
            _lblLocalFile.Text = Language.LocalFile + ":";
            _lblRemoteFile.Text = Language.RemoteFile + ":";
            _btnBrowse.Text = Language._Browse;
            _grpConnection.Text = Language.Connection;
            _lblProtocol.Text = Language.Protocol;
            _lblPassword.Text = Language.Password;
            _lblUser.Text = Language.User + ":";
            _lblPort.Text = Language.Port;
            _lblHost.Text = Language.Host + ":";
            _btnTransfer.Text = Language.Transfer;
            TabText = Language.Transfer;
            Text = Language.Transfer;
        }

        #endregion

        #region Private Methods

        private SecureTransfer _st;

        private void StartTransfer(SecureTransfer.SshTransferProtocol protocol)
        {
            if (AllFieldsSet() == false)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.PleaseFillAllFields);
                return;
            }

            if (File.Exists(_txtLocalFile.Text) == false)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, Language.LocalFileDoesNotExist);
                return;
            }

            try
            {
                _st = new SecureTransfer(_txtHost.Text, _txtUser.Text, _txtPassword.Text, int.Parse(_txtPort.Text), protocol,
                                        _txtLocalFile.Text, _txtRemoteFile.Text);

                // Connect creates the protocol objects and makes the initial connection.
                _st.Connect();

                switch (protocol)
                {
                    case SecureTransfer.SshTransferProtocol.Scp:
                        _st.ScpClt.Uploading += ScpClt_Uploading;
                        break;
                    case SecureTransfer.SshTransferProtocol.Sftp:
                        _st.AsyncCallback = AsyncCallback;
                        break;
                }

                var t = new Thread(StartTransferBg);
                t.SetApartmentState(ApartmentState.STA);
                t.IsBackground = true;
                t.Start();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.SshTransferFailed, ex);
                _st?.Disconnect();
                _st?.Dispose();
            }
        }

        private void AsyncCallback(IAsyncResult ar)
        {
            Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, $"SFTP AsyncCallback completed.", true);
        }

        private void ScpClt_Uploading(object sender, Renci.SshNet.Common.ScpUploadEventArgs e)
        {
            // If the file size is over 2 gigs, convert to kb. This means we'll support a 2TB file.
            var max = e.Size > int.MaxValue ? Convert.ToInt32(e.Size / 1024) : Convert.ToInt32(e.Size);

            // yes, compare to size since that's the total/original file size
            var cur = e.Size > int.MaxValue ? Convert.ToInt32(e.Uploaded / 1024) : Convert.ToInt32(e.Uploaded);

            SshTransfer_Progress(cur, max);
        }

        private void StartTransferBg()
        {
            try
            {
                DisableButtons();
                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                                                    $"Transfer of {Path.GetFileName(_st.SrcFile)} started.", true);
                _st.Upload();

                // SftpClient is Asynchronous, so we need to wait here after the upload and handle the status directly since no status events are raised.
                if (_st.Protocol == SecureTransfer.SshTransferProtocol.Sftp)
                {
                    var fi = new FileInfo(_st.SrcFile);
                    while (!_st.AsyncResult.IsCompleted)
                    {
                        var max = fi.Length > int.MaxValue
                            ? Convert.ToInt32(fi.Length / 1024)
                            : Convert.ToInt32(fi.Length);

                        var cur = fi.Length > int.MaxValue
                            ? Convert.ToInt32(_st.AsyncResult.UploadedBytes / 1024)
                            : Convert.ToInt32(_st.AsyncResult.UploadedBytes);
                        SshTransfer_Progress(cur, max);
                        Thread.Sleep(50);
                    }
                }

                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                                                    $"Transfer of {Path.GetFileName(_st.SrcFile)} completed.", true);
                _st.Disconnect();
                _st.Dispose();
                EnableButtons();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.SshBackgroundTransferFailed, ex,
                                                                MessageClass.ErrorMsg, false);
                _st?.Disconnect();
                _st?.Dispose();
            }
        }

        private bool AllFieldsSet()
        {
            if (_txtHost.Text != "" && _txtPort.Text != "" && _txtUser.Text != "" && _txtLocalFile.Text != "" &&
                _txtRemoteFile.Text != "")
            {
                if (_txtPassword.Text == "")
                {
                    if (MessageBox.Show(FrmMain.Default, Language.EmptyPasswordContinue, @"Question?",
                                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return false;
                    }
                }

                if (_txtRemoteFile.Text.EndsWith("/") || _txtRemoteFile.Text.EndsWith("\\"))
                {
                    _txtRemoteFile.Text +=
                        _txtLocalFile.Text.Substring(_txtLocalFile.Text.LastIndexOf("\\", StringComparison.Ordinal) + 1);
                }

                return true;
            }
            else
            {
                return false;
            }
        }


        private int _maxVal;
        private int _curVal;

        private delegate void SetStatusCb();

        private void SetStatus()
        {
            if (_pbStatus.InvokeRequired)
            {
                SetStatusCb d = SetStatus;
                _pbStatus.Invoke(d);
            }
            else
            {
                _pbStatus.Maximum = _maxVal;
                _pbStatus.Value = _curVal;
            }
        }

        private delegate void EnableButtonsCb();

        private void EnableButtons()
        {
            if (_btnTransfer.InvokeRequired)
            {
                EnableButtonsCb d = EnableButtons;
                _btnTransfer.Invoke(d);
            }
            else
            {
                _btnTransfer.Enabled = true;
            }
        }

        private delegate void DisableButtonsCb();

        private void DisableButtons()
        {
            if (_btnTransfer.InvokeRequired)
            {
                DisableButtonsCb d = DisableButtons;
                _btnTransfer.Invoke(d);
            }
            else
            {
                _btnTransfer.Enabled = false;
            }
        }

        private void SshTransfer_Progress(int transferredBytes, int totalBytes)
        {
            _maxVal = totalBytes;
            _curVal = transferredBytes;

            SetStatus();
        }

        #endregion

        #region Public Methods

        public SshTransferWindow()
        {
            WindowType = WindowType.SshTransfer;
            DockPnl = new DockContent();
            InitializeComponent();

            _oDlg = new OpenFileDialog
            {
                Filter = @"All Files (*.*)|*.*",
                CheckFileExists = true
            };
        }

        #endregion

        #region Form Stuff

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (_oDlg.ShowDialog() != DialogResult.OK) return;
            if (_oDlg.FileName != "")
            {
                _txtLocalFile.Text = _oDlg.FileName;
            }
        }

        private void btnTransfer_Click(object sender, EventArgs e)
        {
            if (_radProtScp.Checked)
            {
                StartTransfer(SecureTransfer.SshTransferProtocol.Scp);
            }
            else if (_radProtSftp.Checked)
            {
                StartTransfer(SecureTransfer.SshTransferProtocol.Sftp);
            }
        }

        #endregion
    }
}