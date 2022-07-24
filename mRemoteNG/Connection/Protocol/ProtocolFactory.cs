using mRemoteNG.Connection.Protocol.Http;
using mRemoteNG.Connection.Protocol.RAW;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Connection.Protocol.Rlogin;
using mRemoteNG.Connection.Protocol.SSH;
using mRemoteNG.Connection.Protocol.Telnet;
using mRemoteNG.Connection.Protocol.VNC;
using System;
using mRemoteNG.Connection.Protocol.PowerShell;
using mRemoteNG.Properties;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.Connection.Protocol
{
    public class ProtocolFactory
    {
        private readonly RdpProtocolFactory _rdpProtocolFactory = new();

        public ProtocolBase CreateProtocol(ConnectionInfo connectionInfo)
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (connectionInfo.Protocol)
            {
                case ProtocolType.Rdp:
                    var rdp = _rdpProtocolFactory.Build(connectionInfo.RdpVersion);
                    rdp.LoadBalanceInfoUseUtf8 = OptionsAdvancedPage.Default.RdpLoadBalanceInfoUseUtf8;
                    return rdp;
                case ProtocolType.Vnc:
                    return new ProtocolVnc();
                case ProtocolType.Ssh1:
                    return new ProtocolSsh1();
                case ProtocolType.Ssh2:
                    return new ProtocolSsh2();
                case ProtocolType.Telnet:
                    return new ProtocolTelnet();
                case ProtocolType.Rlogin:
                    return new ProtocolRlogin();
                case ProtocolType.Raw:
                    return new RawProtocol();
                case ProtocolType.Http:
                    return new ProtocolHttp(connectionInfo.RenderingEngine);
                case ProtocolType.Https:
                    return new ProtocolHttps(connectionInfo.RenderingEngine);
                case ProtocolType.PowerShell:
                    return new ProtocolPowerShell(connectionInfo);
                case ProtocolType.IntApp:
                    if (connectionInfo.ExtApp == "")
                    {
                        throw (new Exception(Language.NoExtAppDefined));
                    }
                    return new IntegratedProgram();
            }

            return default(ProtocolBase);
        }
    }
}