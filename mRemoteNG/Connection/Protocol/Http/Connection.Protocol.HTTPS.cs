namespace mRemoteNG.Connection.Protocol.Http
{
    public class ProtocolHttps : HttpBase
    {
        public ProtocolHttps(RenderingEngine renderingEngine) : base(renderingEngine)
        {
            HttpOrS = "https";
            DefaultPort = (int)Defaults.Port;
        }

        public enum Defaults
        {
            Port = 443
        }
    }
}