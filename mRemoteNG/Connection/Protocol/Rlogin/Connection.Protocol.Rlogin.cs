namespace mRemoteNG.Connection.Protocol.Rlogin
{
    public class ProtocolRlogin : PuttyBase
    {
        public ProtocolRlogin()
        {
            PuttyProtocol = PuttyBase.PuttyProtocols.Rlogin;
        }

        public enum Defaults
        {
            Port = 513
        }
    }
}