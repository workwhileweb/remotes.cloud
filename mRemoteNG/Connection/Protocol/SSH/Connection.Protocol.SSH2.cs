namespace mRemoteNG.Connection.Protocol.SSH
{
    public class ProtocolSsh2 : PuttyBase
    {
        public ProtocolSsh2()
        {
            PuttyProtocol = PuttyBase.PuttyProtocols.Ssh;
            PuttySshVersion = PuttyBase.PuttySshVersions.Ssh2;
        }

        public enum Defaults
        {
            Port = 22
        }
    }
}