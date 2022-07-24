using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using mRemoteNG.Config.Serializers.MiscSerializers;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Container;
using mRemoteNGTests.Properties;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Serializers.MiscSerializers
{
	public class RemoteDesktopConnectionManager27DeserializerTests
    {
        private string _connectionFileContents;
        private RemoteDesktopConnectionManagerDeserializer _deserializer;
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
        private const AuthenticationLevel EXPECTED_AUTH_LEVEL = AuthenticationLevel.WarnOnFailedAuth;
        private const string EXPECTED_START_PROGRAM = "alternate shell";

        [OneTimeSetUp]
        public void OnetimeSetup()
        {
            _connectionFileContents = Resources.test_rdcman_v2_7_schema3;
            _deserializer = new RemoteDesktopConnectionManagerDeserializer();
        }

        [Test]
        public void ConnectionTreeModelHasARootNode()
        {
	        var connectionTreeModel = _deserializer.Deserialize(_connectionFileContents);
			var numberOfRootNodes = connectionTreeModel.RootNodes.Count;
            Assert.That(numberOfRootNodes, Is.GreaterThan(0));
        }

        [Test]
        public void RootNodeHasContents()
        {
	        var connectionTreeModel = _deserializer.Deserialize(_connectionFileContents);
			var rootNodeContents = connectionTreeModel.RootNodes.First().Children;
            Assert.That(rootNodeContents, Is.Not.Empty);
        }

        [Test]
        public void AllSubRootFoldersImported()
        {
	        var connectionTreeModel = _deserializer.Deserialize(_connectionFileContents);
			var rootNode = connectionTreeModel.RootNodes.First();
            var importedRdcmanRootNode = rootNode.Children.OfType<ContainerInfo>().First();
            var rootNodeContents = importedRdcmanRootNode.Children.Count(node => node.Name == "Group1" || node.Name == "Group2");
            Assert.That(rootNodeContents, Is.EqualTo(2));
        }

        [TestCaseSource(nameof(ExpectedPropertyValues))]
        public void PropertiesWithValuesAreCorrectlyImported(Func<ConnectionInfo, object> propSelector, object expectedValue)
        {
	        var connectionTreeModel = _deserializer.Deserialize(_connectionFileContents);

			var connection = connectionTreeModel
				.GetRecursiveChildList()
				.OfType<ContainerInfo>()
				.First(node => node.Name == "Group1")
				.Children
				.First();

            Assert.That(propSelector(connection), Is.EqualTo(expectedValue));
        }

		[TestCaseSource(nameof(NullPropertyValues))]
        public void PropertiesWithoutValuesAreIgnored(Func<ConnectionInfo, object> propSelector)
        {
	        var connectionTreeModel = _deserializer.Deserialize(Resources.test_rdcman_v2_7_schema3_empty_values);

	        var importedConnection = connectionTreeModel
		        .GetRecursiveChildList()
		        .OfType<ContainerInfo>()
		        .First(node => node.Name == "Group1")
		        .Children
		        .First();

			Assert.That(propSelector(importedConnection), Is.EqualTo(propSelector(new ConnectionInfo())));
        }

        [TestCaseSource(nameof(NullPropertyValues))]
        public void NonExistantPropertiesAreIgnored(Func<ConnectionInfo, object> propSelector)
        {
	        var connectionTreeModel = _deserializer.Deserialize(Resources.test_rdcman_v2_7_schema3_null_values);

	        var importedConnection = connectionTreeModel
		        .GetRecursiveChildList()
		        .OfType<ContainerInfo>()
		        .First(node => node.Name == "Group1")
		        .Children
		        .First();

	        Assert.That(propSelector(importedConnection), Is.EqualTo(propSelector(new ConnectionInfo())));
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

        private static IEnumerable<TestCaseData> ExpectedPropertyValues()
        {
	        return new[]
	        {
		        new TestCaseData((Func<ConnectionInfo,object>)(con => con.Name), EXPECTED_NAME).SetName(nameof(ConnectionInfo.Name)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.Hostname), EXPECTED_HOSTNAME).SetName(nameof(ConnectionInfo.Hostname)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.Description), EXPECTED_DESCRIPTION).SetName(nameof(ConnectionInfo.Description)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.Username), EXPECTED_USERNAME).SetName(nameof(ConnectionInfo.Username)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.Domain), EXPECTED_DOMAIN).SetName(nameof(ConnectionInfo.Domain)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.Protocol), ProtocolType.Rdp).SetName(nameof(ConnectionInfo.Protocol)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.UseConsoleSession), EXPECTED_USE_CONSOLE_SESSION).SetName(nameof(ConnectionInfo.UseConsoleSession)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.Port), EXPECTED_PORT).SetName(nameof(ConnectionInfo.Port)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.RdGatewayUsageMethod), EXPECTED_GATEWAY_USAGE_METHOD).SetName(nameof(ConnectionInfo.RdGatewayUsageMethod)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.RdGatewayHostname), EXPECTED_GATEWAY_HOSTNAME).SetName(nameof(ConnectionInfo.RdGatewayHostname)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.RdGatewayUsername), EXPECTED_GATEWAY_USERNAME).SetName(nameof(ConnectionInfo.RdGatewayUsername)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.RdGatewayDomain), EXPECTED_GATEWAY_DOMAIN).SetName(nameof(ConnectionInfo.RdGatewayDomain)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.Resolution), EXPECTED_RDP_RESOLUTION).SetName(nameof(ConnectionInfo.Resolution)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.Colors), EXPECTED_RDP_COLOR_DEPTH).SetName(nameof(ConnectionInfo.Colors)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.RedirectSound), EXPECTED_AUDIO_REDIRECTION).SetName(nameof(ConnectionInfo.RedirectSound)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.RedirectKeys), EXPECTED_KEY_REDIRECTION).SetName(nameof(ConnectionInfo.RedirectKeys)),
		        new TestCaseData((Func<ConnectionInfo,object>)(con => con.RdpAuthenticationLevel), EXPECTED_AUTH_LEVEL).SetName(nameof(ConnectionInfo.RdpAuthenticationLevel)),
		        new TestCaseData((Func<ConnectionInfo,object>)(con => con.RedirectSmartCards), EXPECTED_SMARTCARD_REDIRECTION).SetName(nameof(ConnectionInfo.RedirectSmartCards)),
		        new TestCaseData((Func<ConnectionInfo,object>)(con => con.RedirectPrinters), EXPECTED_PRINTER_REDIRECTION).SetName(nameof(ConnectionInfo.RedirectPrinters)),
		        new TestCaseData((Func<ConnectionInfo,object>)(con => con.RedirectPorts), EXPECTED_PORT_REDIRECTION).SetName(nameof(ConnectionInfo.RedirectPorts)),
		        new TestCaseData((Func<ConnectionInfo,object>)(con => con.RedirectDiskDrives), EXPECTED_DRIVE_REDIRECTION).SetName(nameof(ConnectionInfo.RedirectDiskDrives)),
		        new TestCaseData((Func<ConnectionInfo,object>)(con => con.RdpStartProgram), EXPECTED_START_PROGRAM).SetName(nameof(ConnectionInfo.RdpStartProgram)),
			};
        }

        private static IEnumerable<TestCaseData> NullPropertyValues()
        {
			return new[]
			{
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.Name)).SetName(nameof(ConnectionInfo.Name)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.Hostname)).SetName(nameof(ConnectionInfo.Hostname)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.Description)).SetName(nameof(ConnectionInfo.Description)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.Username)).SetName(nameof(ConnectionInfo.Username)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.Domain)).SetName(nameof(ConnectionInfo.Domain)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.Protocol)).SetName(nameof(ConnectionInfo.Protocol)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.UseConsoleSession)).SetName(nameof(ConnectionInfo.UseConsoleSession)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.Port)).SetName(nameof(ConnectionInfo.Port)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.RdGatewayUsageMethod)).SetName(nameof(ConnectionInfo.RdGatewayUsageMethod)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.RdGatewayHostname)).SetName(nameof(ConnectionInfo.RdGatewayHostname)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.RdGatewayUsername)).SetName(nameof(ConnectionInfo.RdGatewayUsername)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.RdGatewayDomain)).SetName(nameof(ConnectionInfo.RdGatewayDomain)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.Resolution)).SetName(nameof(ConnectionInfo.Resolution)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.Colors)).SetName(nameof(ConnectionInfo.Colors)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.RedirectSound)).SetName(nameof(ConnectionInfo.RedirectSound)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.RedirectKeys)).SetName(nameof(ConnectionInfo.RedirectKeys)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.RdpAuthenticationLevel)).SetName(nameof(ConnectionInfo.RdpAuthenticationLevel)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.RedirectSmartCards)).SetName(nameof(ConnectionInfo.RedirectSmartCards)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.RedirectPrinters)).SetName(nameof(ConnectionInfo.RedirectPrinters)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.RedirectPorts)).SetName(nameof(ConnectionInfo.RedirectPorts)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.RedirectDiskDrives)).SetName(nameof(ConnectionInfo.RedirectDiskDrives)),
			};
		}
    }
}