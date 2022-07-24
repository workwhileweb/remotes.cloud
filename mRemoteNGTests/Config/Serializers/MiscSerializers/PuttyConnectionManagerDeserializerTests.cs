using System.Linq;
using mRemoteNG.Config.Serializers.MiscSerializers;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Container;
using mRemoteNGTests.Properties;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Serializers.MiscSerializers
{
    public class PuttyConnectionManagerDeserializerTests
    {
        private PuttyConnectionManagerDeserializer _deserializer;
        private ContainerInfo _rootImportedFolder;
        private const string EXPECTED_ROOT_FOLDER_NAME = "test_puttyConnectionManager_database";
        private const string EXPECTED_CONNECTION_DISPLAY_NAME = "my ssh connection";
        private const string EXPECTED_CONNECTION_HOSTNAME = "server1.mydomain.com";
        private const string EXPECTED_CONNECTION_DESCRIPTION = "My Description Here";
        private const int EXPECTED_CONNECTION_PORT = 22;
        private const ProtocolType EXPECTED_PROTOCOL_TYPE = ProtocolType.Ssh2;
        private const string EXPECTED_PUTTY_SESSION = "MyCustomPuttySession";
        private const string EXPECTED_CONNECTION_USERNAME = "mysshusername";
        private const string EXPECTED_CONNECTION_PASSWORD = "password123";


        [OneTimeSetUp]
        public void OnetimeSetup()
        {
            var fileContents = Resources.test_puttyConnectionManager_database;
            _deserializer = new PuttyConnectionManagerDeserializer();
            var connectionTreeModel = _deserializer.Deserialize(fileContents);
            var rootNode = connectionTreeModel.RootNodes.First();
            _rootImportedFolder = rootNode.Children.Cast<ContainerInfo>().First();
        }

        [OneTimeTearDown]
        public void OnetimeTeardown()
        {
            _deserializer = null;
            _rootImportedFolder = null;
        }

        [Test]
        public void RootFolderImportedWithCorrectName()
        {
            Assert.That(_rootImportedFolder.Name, Is.EqualTo(EXPECTED_ROOT_FOLDER_NAME));
        }

        [Test]
        public void ConnectionDisplayNameImported()
        {
            var connection = GetSshConnection();
            Assert.That(connection.Name, Is.EqualTo(EXPECTED_CONNECTION_DISPLAY_NAME));
        }

        [Test]
        public void ConnectionHostNameImported()
        {
            var connection = GetSshConnection();
            Assert.That(connection.Hostname, Is.EqualTo(EXPECTED_CONNECTION_HOSTNAME));
        }

        [Test]
        public void ConnectionDescriptionImported()
        {
            var connection = GetSshConnection();
            Assert.That(connection.Description, Is.EqualTo(EXPECTED_CONNECTION_DESCRIPTION));
        }

        [Test]
        public void ConnectionPortImported()
        {
            var connection = GetSshConnection();
            Assert.That(connection.Port, Is.EqualTo(EXPECTED_CONNECTION_PORT));
        }

        [Test]
        public void ConnectionProtocolTypeImported()
        {
            var connection = GetSshConnection();
            Assert.That(connection.Protocol, Is.EqualTo(EXPECTED_PROTOCOL_TYPE));
        }

        [Test]
        public void ConnectionPuttySessionImported()
        {
            var connection = GetSshConnection();
            Assert.That(connection.PuttySession, Is.EqualTo(EXPECTED_PUTTY_SESSION));
        }

        [Test]
        public void ConnectionUsernameImported()
        {
            var connection = GetSshConnection();
            Assert.That(connection.Username, Is.EqualTo(EXPECTED_CONNECTION_USERNAME));
        }

        [Test]
        public void ConnectionPasswordImported()
        {
            var connection = GetSshConnection();
            Assert.That(connection.Password, Is.EqualTo(EXPECTED_CONNECTION_PASSWORD));
        }

        private ConnectionInfo GetSshConnection()
        {
            var sshFolder = _rootImportedFolder.Children.OfType<ContainerInfo>().First(node => node.Name == "SSHFolder");
            return sshFolder.Children.First();
        }
    }
}