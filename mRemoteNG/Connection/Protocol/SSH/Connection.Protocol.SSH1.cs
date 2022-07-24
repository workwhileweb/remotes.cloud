namespace mRemoteNG.Connection.Protocol.SSH
{
    public class ProtocolSsh1 : PuttyBase
    {
        public ProtocolSsh1()
        {
            PuttyProtocol = PuttyBase.PuttyProtocols.Ssh;
            PuttySshVersion = PuttyBase.PuttySshVersions.Ssh1;
        }

        public enum Defaults
        {
            Port = 22
        }
    }
}