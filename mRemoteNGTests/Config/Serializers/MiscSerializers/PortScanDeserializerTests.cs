using System.Linq;
using mRemoteNG.Config.Serializers.MiscSerializers;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Tools;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Serializers.MiscSerializers
{
    public class PortScanDeserializerTests
    {
        private PortScanDeserializer _deserializer;
        private ConnectionInfo _importedConnectionInfo;
        private const string EXPECTED_HOST_NAME = "server1.domain.com";
        private const string EXPECTED_DISPLAY_NAME = "server1";
        private const ProtocolType EXPECTED_PROTOCOL_TYPE = ProtocolType.Ssh2;



        [OneTimeSetUp]
        public void OnetimeSetup()
        {
            var host = new ScanHost("10.20.30.40")
            {
                HostName = "server1.domain.com",
                Ssh = true
            };
            _deserializer = new PortScanDeserializer(ProtocolType.Ssh2);
            var connectionTreeModel = _deserializer.Deserialize(new[] { host });
            var root = connectionTreeModel.RootNodes.First();
            _importedConnectionInfo = root.Children.First();
        }

        [OneTimeTearDown]
        public void OnetimeTeardown()
        {
            _deserializer = null;
            _importedConnectionInfo = null;
        }

        [Test]
        public void DisplayNameImported()
        {
            Assert.That(_importedConnectionInfo.Name, Is.EqualTo(EXPECTED_DISPLAY_NAME));
        }

        [Test]
        public void HostNameImported()
        {
            Assert.That(_importedConnectionInfo.Hostname, Is.EqualTo(EXPECTED_HOST_NAME));
        }

        [Test]
        public void ProtocolImported()
        {
            Assert.That(_importedConnectionInfo.Protocol, Is.EqualTo(EXPECTED_PROTOCOL_TYPE));
        }
    }
}