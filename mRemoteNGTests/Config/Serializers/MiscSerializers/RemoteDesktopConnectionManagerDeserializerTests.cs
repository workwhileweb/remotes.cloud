using System.IO;
using System.Linq;
using mRemoteNG.Config.Serializers.MiscSerializers;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Container;
using mRemoteNG.Tree;
using mRemoteNGTests.Properties;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Serializers.MiscSerializers
{
    public class RemoteDesktopConnectionManagerDeserializerTests
    {
        private string _connectionFileContents;
        private RemoteDesktopConnectionManagerDeserializer _deserializer;
        private ConnectionTreeModel _connectionTreeModel;
        private const string EXPECTED_NAME = "server1_displayname";
        private const string EXPECTED_HOSTNAME = "server1";
        private const string EXPECTED_DESCRIPTION = "Comment text here";
        private const string EXPECTED_USERNAME = "myusername1";
        private const string EXPECTED_DOMAIN = "mydomain";
        private const string EXPECTED_PASSWORD = "passwordHere!";
        private const bool EXPECTED_USE_CONSOLE_SESSION = true;
        private const int EXPECTED_PORT = 9933;
        private const RdGatewayUsageMethod EXPECTED_GATEWAY_USAGE_METHOD = RdGatewayUsageMethod.Always;
        private const string EXPECTED_GATEWAY_HOSTNAME = "gatewayserverhost.innerdomain.net";
        private const string EXPECTED_GATEWAY_USERNAME = "gatewayusername";
        private const string EXPECTED_GATEWAY_DOMAIN = "innerdomain";
        private const string EXPECTED_GATEWAY_PASSWORD = "gatewayPassword123";
        private const RdpResolutions EXPECTED_RDP_RESOLUTION = RdpResolutions.FitToWindow;
        private const RdpColors EXPECTED_RDP_COLOR_DEPTH = RdpColors.Colors24Bit;
        private const RdpSounds EXPECTED_AUDIO_REDIRECTION = RdpSounds.DoNotPlay;
        private const bool EXPECTED_KEY_REDIRECTION = true;
        private const bool EXPECTED_SMARTCARD_REDIRECTION = true;
        private const bool EXPECTED_DRIVE_REDIRECTION = true;
        private const bool EXPECTED_PORT_REDIRECTION = true;
        private const bool EXPECTED_PRINTER_REDIRECTION = true;
        private const AuthenticationLevel EXPECTED_AUTH_LEVEL = AuthenticationLevel.AuthRequired;
        private const string EXPECTED_START_PROGRAM = "alternate shell";

        [OneTimeSetUp]
        public void OnetimeSetup()
        {
            _connectionFileContents = Resources.test_rdcman_v2_2_schema1;
            _deserializer = new RemoteDesktopConnectionManagerDeserializer();
            _connectionTreeModel = _deserializer.Deserialize(_connectionFileContents);
        }

        [Test]
        public void ConnectionTreeModelHasARootNode()
        {
            var numberOfRootNodes = _connectionTreeModel.RootNodes.Count;
            Assert.That(numberOfRootNodes, Is.GreaterThan(0));
        }

        [Test]
        public void RootNodeHasContents()
        {
            var rootNodeContents = _connectionTreeModel.RootNodes.First().Children;
            Assert.That(rootNodeContents, Is.Not.Empty);
        }

        [Test]
        public void AllSubRootFoldersImported()
        {
            var rootNode = _connectionTreeModel.RootNodes.First();
            var importedRdcmanRootNode = rootNode.Children.OfType<ContainerInfo>().First();
            var rootNodeContents = importedRdcmanRootNode.Children.Count(node => node.Name == "Group1" || node.Name == "Group2");
            Assert.That(rootNodeContents, Is.EqualTo(2));
        }

        [Test]
        public void ConnectionDisplayNameImported()
        {
            var rootNode = _connectionTreeModel.RootNodes.First();
            var importedRdcmanRootNode = rootNode.Children.OfType<ContainerInfo>().First();
            var group1 = importedRdcmanRootNode.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            Assert.That(connection.Name, Is.EqualTo(EXPECTED_NAME));
        }

        [Test]
        public void ConnectionHostnameImported()
        {
            var rootNode = _connectionTreeModel.RootNodes.First();
            var importedRdcmanRootNode = rootNode.Children.OfType<ContainerInfo>().First();
            var group1 = importedRdcmanRootNode.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            Assert.That(connection.Hostname, Is.EqualTo(EXPECTED_HOSTNAME));
        }

        [Test]
        public void ConnectionDescriptionImported()
        {
            var rootNode = _connectionTreeModel.RootNodes.First();
            var importedRdcmanRootNode = rootNode.Children.OfType<ContainerInfo>().First();
            var group1 = importedRdcmanRootNode.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            Assert.That(connection.Description, Is.EqualTo(EXPECTED_DESCRIPTION));
        }

        [Test]
        public void ConnectionUsernameImported()
        {
            var rootNode = _connectionTreeModel.RootNodes.First();
            var importedRdcmanRootNode = rootNode.Children.OfType<ContainerInfo>().First();
            var group1 = importedRdcmanRootNode.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            Assert.That(connection.Username, Is.EqualTo(EXPECTED_USERNAME));
        }

        [Test]
        public void ConnectionDomainImported()
        {
            var rootNode = _connectionTreeModel.RootNodes.First();
            var importedRdcmanRootNode = rootNode.Children.OfType<ContainerInfo>().First();
            var group1 = importedRdcmanRootNode.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            Assert.That(connection.Domain, Is.EqualTo(EXPECTED_DOMAIN));
        }

        // Since password is encrypted with a machine key, cant test decryption on another machine
        //[Test]
        //public void ConnectionPasswordImported()
        //{
        //    var rootNode = _connectionTreeModel.RootNodes.First();
        //    var importedRdcmanRootNode = rootNode.Children.OfType<ContainerInfo>().First();
        //    var group1 = importedRdcmanRootNode.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
        //    var connection = group1.Children.First();
        //    Assert.That(connection.Password, Is.EqualTo(ExpectedPassword));
        //}

        [Test]
        public void ConnectionProtocolSetToRdp()
        {
            var rootNode = _connectionTreeModel.RootNodes.First();
            var importedRdcmanRootNode = rootNode.Children.OfType<ContainerInfo>().First();
            var group1 = importedRdcmanRootNode.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            Assert.That(connection.Protocol, Is.EqualTo(ProtocolType.Rdp));
        }

        [Test]
        public void ConnectionUseConsoleSessionImported()
        {
            var rootNode = _connectionTreeModel.RootNodes.First();
            var importedRdcmanRootNode = rootNode.Children.OfType<ContainerInfo>().First();
            var group1 = importedRdcmanRootNode.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            Assert.That(connection.UseConsoleSession, Is.EqualTo(EXPECTED_USE_CONSOLE_SESSION));
        }

        [Test]
        public void ConnectionPortImported()
        {
            var rootNode = _connectionTreeModel.RootNodes.First();
            var importedRdcmanRootNode = rootNode.Children.OfType<ContainerInfo>().First();
            var group1 = importedRdcmanRootNode.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            Assert.That(connection.Port, Is.EqualTo(EXPECTED_PORT));
        }

        [Test]
        public void ConnectionGatewayUsageMethodImported()
        {
            var rootNode = _connectionTreeModel.RootNodes.First();
            var importedRdcmanRootNode = rootNode.Children.OfType<ContainerInfo>().First();
            var group1 = importedRdcmanRootNode.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            Assert.That(connection.RdGatewayUsageMethod, Is.EqualTo(EXPECTED_GATEWAY_USAGE_METHOD));
        }

        [Test]
        public void ConnectionGatewayHostnameImported()
        {
            var rootNode = _connectionTreeModel.RootNodes.First();
            var importedRdcmanRootNode = rootNode.Children.OfType<ContainerInfo>().First();
            var group1 = importedRdcmanRootNode.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            Assert.That(connection.RdGatewayHostname, Is.EqualTo(EXPECTED_GATEWAY_HOSTNAME));
        }

        [Test]
        public void ConnectionGatewayUsernameImported()
        {
            var rootNode = _connectionTreeModel.RootNodes.First();
            var importedRdcmanRootNode = rootNode.Children.OfType<ContainerInfo>().First();
            var group1 = importedRdcmanRootNode.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            Assert.That(connection.RdGatewayUsername, Is.EqualTo(EXPECTED_GATEWAY_USERNAME));
        }

        // Since password is encrypted with a machine key, cant test decryption on another machine
        //[Test]
        //public void ConnectionGatewayPasswordImported()
        //{
        //    var rootNode = _connectionTreeModel.RootNodes.First();
        //    var importedRdcmanRootNode = rootNode.Children.OfType<ContainerInfo>().First();
        //    var group1 = importedRdcmanRootNode.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
        //    var connection = group1.Children.First();
        //    Assert.That(connection.RDGatewayPassword, Is.EqualTo(ExpectedGatewayPassword));
        //}

        [Test]
        public void ConnectionGatewayDomainImported()
        {
            var rootNode = _connectionTreeModel.RootNodes.First();
            var importedRdcmanRootNode = rootNode.Children.OfType<ContainerInfo>().First();
            var group1 = importedRdcmanRootNode.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            Assert.That(connection.RdGatewayDomain, Is.EqualTo(EXPECTED_GATEWAY_DOMAIN));
        }

        [Test]
        public void ConnectionResolutionImported()
        {
            var rootNode = _connectionTreeModel.RootNodes.First();
            var importedRdcmanRootNode = rootNode.Children.OfType<ContainerInfo>().First();
            var group1 = importedRdcmanRootNode.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            Assert.That(connection.Resolution, Is.EqualTo(EXPECTED_RDP_RESOLUTION));
        }

        [Test]
        public void ConnectionColorDepthImported()
        {
            var rootNode = _connectionTreeModel.RootNodes.First();
            var importedRdcmanRootNode = rootNode.Children.OfType<ContainerInfo>().First();
            var group1 = importedRdcmanRootNode.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            Assert.That(connection.Colors, Is.EqualTo(EXPECTED_RDP_COLOR_DEPTH));
        }

        [Test]
        public void ConnectionAudioRedirectionImported()
        {
            var rootNode = _connectionTreeModel.RootNodes.First();
            var importedRdcmanRootNode = rootNode.Children.OfType<ContainerInfo>().First();
            var group1 = importedRdcmanRootNode.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            Assert.That(connection.RedirectSound, Is.EqualTo(EXPECTED_AUDIO_REDIRECTION));
        }

        [Test]
        public void ConnectionKeyRedirectionImported()
        {
            var rootNode = _connectionTreeModel.RootNodes.First();
            var importedRdcmanRootNode = rootNode.Children.OfType<ContainerInfo>().First();
            var group1 = importedRdcmanRootNode.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            Assert.That(connection.RedirectKeys, Is.EqualTo(EXPECTED_KEY_REDIRECTION));
        }

        [Test]
        public void ConnectionDriveRedirectionImported()
        {
            var rootNode = _connectionTreeModel.RootNodes.First();
            var importedRdcmanRootNode = rootNode.Children.OfType<ContainerInfo>().First();
            var group1 = importedRdcmanRootNode.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            Assert.That(connection.RedirectDiskDrives, Is.EqualTo(EXPECTED_DRIVE_REDIRECTION));
        }

        [Test]
        public void ConnectionPortRedirectionImported()
        {
            var rootNode = _connectionTreeModel.RootNodes.First();
            var importedRdcmanRootNode = rootNode.Children.OfType<ContainerInfo>().First();
            var group1 = importedRdcmanRootNode.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            Assert.That(connection.RedirectPorts, Is.EqualTo(EXPECTED_PORT_REDIRECTION));
        }

        [Test]
        public void ConnectionPrinterRedirectionImported()
        {
            var rootNode = _connectionTreeModel.RootNodes.First();
            var importedRdcmanRootNode = rootNode.Children.OfType<ContainerInfo>().First();
            var group1 = importedRdcmanRootNode.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            Assert.That(connection.RedirectPrinters, Is.EqualTo(EXPECTED_PRINTER_REDIRECTION));
        }

        [Test]
        public void ConnectionSmartcardRedirectionImported()
        {
            var rootNode = _connectionTreeModel.RootNodes.First();
            var importedRdcmanRootNode = rootNode.Children.OfType<ContainerInfo>().First();
            var group1 = importedRdcmanRootNode.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            Assert.That(connection.RedirectSmartCards, Is.EqualTo(EXPECTED_SMARTCARD_REDIRECTION));
        }

        [Test]
        public void ConnectionauthenticationLevelImported()
        {
            var rootNode = _connectionTreeModel.RootNodes.First();
            var importedRdcmanRootNode = rootNode.Children.OfType<ContainerInfo>().First();
            var group1 = importedRdcmanRootNode.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            Assert.That(connection.RdpAuthenticationLevel, Is.EqualTo(EXPECTED_AUTH_LEVEL));
        }

        [Test]
        public void ExceptionThrownOnBadSchemaVersion()
        {
            var badFileContents = Resources.test_rdcman_v2_2_badschemaversion;
            Assert.That(() => _deserializer.Deserialize(badFileContents), Throws.TypeOf<FileFormatException>());
        }

        [Test]
        public void ExceptionThrownOnUnsupportedVersion()
        {
            var badFileContents = Resources.test_rdcman_badVersionNumber;
            Assert.That(() => _deserializer.Deserialize(badFileContents), Throws.TypeOf<FileFormatException>());
        }

        [Test]
        public void ExceptionThrownOnNoVersion()
        {
            var badFileContents = Resources.test_rdcman_noversion;
            Assert.That(() => _deserializer.Deserialize(badFileContents), Throws.TypeOf<FileFormatException>());
        }

        [Test]
        public void StartProgramImported()
        {
            var rootNode = _connectionTreeModel.RootNodes.First();
            var importedRdcmanRootNode = rootNode.Children.OfType<ContainerInfo>().First();
            var group1 = importedRdcmanRootNode.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            Assert.That(connection.RdpStartProgram, Is.EqualTo(EXPECTED_START_PROGRAM));
        }
    }
}