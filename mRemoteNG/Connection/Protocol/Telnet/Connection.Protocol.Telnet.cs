namespace mRemoteNG.Connection.Protocol.Telnet
{
    public class ProtocolTelnet : PuttyBase
    {
        public ProtocolTelnet()
        {
            PuttyProtocol = PuttyBase.PuttyProtocols.Telnet;
        }

        public enum Defaults
        {
            Port = 23
        }
    }
}