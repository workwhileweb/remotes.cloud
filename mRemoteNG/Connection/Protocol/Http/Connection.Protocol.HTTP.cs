namespace mRemoteNG.Connection.Protocol.Http
{
    public class ProtocolHttp : HttpBase
    {
        public ProtocolHttp(RenderingEngine renderingEngine) : base(renderingEngine)
        {
            HttpOrS = "http";
            DefaultPort = (int)Defaults.Port;
        }

        public enum Defaults
        {
            Port = 80
        }
    }
}