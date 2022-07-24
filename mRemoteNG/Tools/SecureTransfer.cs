using System;
using System.IO;
using mRemoteNG.App;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using static System.IO.FileMode;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.Tools
{
    internal class SecureTransfer : IDisposable
    {
        private readonly string _host;
        private readonly string _user;
        private readonly string _password;
        private readonly int _port;
        public readonly SshTransferProtocol Protocol;
        public string SrcFile;
        public string DstFile;
        public ScpClient ScpClt;
        public SftpClient SftpClt;
        public SftpUploadAsyncResult AsyncResult;
        public AsyncCallback AsyncCallback;


        public SecureTransfer()
        {
        }

        public SecureTransfer(string host, string user, string pass, int port, SshTransferProtocol protocol)
        {
            _host = host;
            _user = user;
            _password = pass;
            _port = port;
            Protocol = protocol;
        }

        public SecureTransfer(string host,
            string user,
            string pass,
            int port,
            SshTransferProtocol protocol,
            string source,
            string dest)
        {
            _host = host;
            _user = user;
            _password = pass;
            _port = port;
            Protocol = protocol;
            SrcFile = source;
            DstFile = dest;
        }

        public void Connect()
        {
            if (Protocol == SshTransferProtocol.Scp)
            {
                ScpClt = new ScpClient(_host, _port, _user, _password);
                ScpClt.Connect();
            }

            if (Protocol == SshTransferProtocol.Sftp)
            {
                SftpClt = new SftpClient(_host, _port, _user, _password);
                SftpClt.Connect();
            }
        }

        public void Disconnect()
        {
            if (Protocol == SshTransferProtocol.Scp)
            {
                ScpClt.Disconnect();
            }

            if (Protocol == SshTransferProtocol.Sftp)
            {
                SftpClt.Disconnect();
            }
        }


        public void Upload()
        {
            if (Protocol == SshTransferProtocol.Scp)
            {
                if (!ScpClt.IsConnected)
                {
                    Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg,
                        Language.SshTransferFailed + Environment.NewLine +
                        "SCP Not Connected!");
                    return;
                }

                ScpClt.Upload(new FileInfo(SrcFile), $"{DstFile}");
            }

            if (Protocol == SshTransferProtocol.Sftp)
            {
                if (!SftpClt.IsConnected)
                {
                    Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg,
                        Language.SshTransferFailed + Environment.NewLine +
                        "SFTP Not Connected!");
                    return;
                }

                AsyncResult =
                    (SftpUploadAsyncResult)SftpClt.BeginUploadFile(new FileStream(SrcFile, Open), $"{DstFile}",
                        AsyncCallback);
            }
        }

        public enum SshTransferProtocol
        {
            Scp = 0,
            Sftp = 1
        }

        private void Dispose(bool disposing)
        {
            if (!disposing) return;

            if (Protocol == SshTransferProtocol.Scp)
            {
                ScpClt.Dispose();
            }

            if (Protocol == SshTransferProtocol.Sftp)
            {
                SftpClt.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}