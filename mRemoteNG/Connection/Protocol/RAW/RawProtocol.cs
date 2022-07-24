namespace mRemoteNG.Connection.Protocol.RAW
{
    public class RawProtocol : PuttyBase
    {
        public RawProtocol()
        {
            PuttyProtocol = PuttyBase.PuttyProtocols.Raw;
        }

        public enum Defaults
        {
            Port = 23
        }
    }
}