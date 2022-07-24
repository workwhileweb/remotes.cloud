using System.Collections.Generic;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Container;
using mRemoteNG.Tools;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;

namespace mRemoteNG.Config.Serializers.MiscSerializers
{
    public class PortScanDeserializer : IDeserializer<IEnumerable<ScanHost>, ConnectionTreeModel>
    {
        private readonly ProtocolType _targetProtocolType;

        public PortScanDeserializer(ProtocolType targetProtocolType)
        {
            _targetProtocolType = targetProtocolType;
        }

        public ConnectionTreeModel Deserialize(IEnumerable<ScanHost> scannedHosts)
        {
            var connectionTreeModel = new ConnectionTreeModel();
            var root = new RootNodeInfo(RootNodeType.Connection);
            connectionTreeModel.AddRootNode(root);

            foreach (var host in scannedHosts)
                ImportScannedHost(host, root);

            return connectionTreeModel;
        }

        private void ImportScannedHost(ScanHost host, ContainerInfo parentContainer)
        {
            var finalProtocol = default(ProtocolType);
            var protocolValid = true;

            switch (_targetProtocolType)
            {
                case ProtocolType.Ssh2:
                    if (host.Ssh)
                        finalProtocol = ProtocolType.Ssh2;
                    break;
                case ProtocolType.Telnet:
                    if (host.Telnet)
                        finalProtocol = ProtocolType.Telnet;
                    break;
                case ProtocolType.Http:
                    if (host.Http)
                        finalProtocol = ProtocolType.Http;
                    break;
                case ProtocolType.Https:
                    if (host.Https)
                        finalProtocol = ProtocolType.Https;
                    break;
                case ProtocolType.Rlogin:
                    if (host.Rlogin)
                        finalProtocol = ProtocolType.Rlogin;
                    break;
                case ProtocolType.Rdp:
                    if (host.Rdp)
                        finalProtocol = ProtocolType.Rdp;
                    break;
                case ProtocolType.Vnc:
                    if (host.Vnc)
                        finalProtocol = ProtocolType.Vnc;
                    break;
                default:
                    protocolValid = false;
                    break;
            }

            if (!protocolValid) return;
            var newConnectionInfo = new ConnectionInfo
            {
                Name = host.HostNameWithoutDomain,
                Hostname = host.HostName,
                Protocol = finalProtocol
            };
            newConnectionInfo.SetDefaultPort();

            parentContainer.AddChild(newConnectionInfo);
        }
    }
}