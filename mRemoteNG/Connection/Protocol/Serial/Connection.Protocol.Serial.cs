namespace mRemoteNG.Connection.Protocol.Serial
{
    public class ProtocolSerial : PuttyBase
    {
        public ProtocolSerial()
        {
            PuttyProtocol = PuttyBase.PuttyProtocols.Serial;
        }

        public enum Defaults
        {
            Port = 9600
        }
    }
}