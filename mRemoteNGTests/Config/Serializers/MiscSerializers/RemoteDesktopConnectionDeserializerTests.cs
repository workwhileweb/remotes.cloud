using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Tree;
using mRemoteNGTests.Properties;
using NUnit.Framework;
using System.Linq;
using mRemoteNG.Config.Serializers.MiscSerializers;

namespace mRemoteNGTests.Config.Serializers.MiscSerializers
{
    public class RemoteDesktopConnectionDeserializerTests
    {
        // .rdp file schema: https://technet.microsoft.com/en-us/library/ff393699(v=ws.10).aspx
        private RemoteDesktopConnectionDeserializer _deserializer;
        private ConnectionTreeModel _connectionTreeModel;
        private const string EXPECTED_HOSTNAME = "testhostname.domain.com";
        private const string EXPECTED_USER_NAME = "myusernamehere";
        private const string EXPECTED_DOMAIN = "myspecialdomain";
        private const string EXPECTED_GATEWAY_HOSTNAME = "gatewayhostname.domain.com";
        private const string EXPECTED_LOAD_BALANCE_INFO = "tsv://MS Terminal Services Plugin.1.RDS-NAME";
        private const int EXPECTED_PORT = 9933;
        private const RdpColors EXPECTED_COLORS = RdpColors.Colors24Bit;
        private const bool EXPECTED_BITMAP_CACHING = false;
        private const RdpResolutions EXPECTED_RESOLUTION_MODE = RdpResolutions.FitToWindow;
        private const bool EXPECTED_WALLPAPER_DISPLAY = true;
        private const bool EXPECTED_THEMES_DISPLAY = true;
        private const bool EXPECTED_FONT_SMOOTHING = true;
        private const bool EXPECTED_DESKTOP_COMPOSITION = true;
        private const bool EXPECTED_SMARTCARD_REDIRECTION = true;
        private const bool EXPECTED_DRIVE_REDIRECTION = true;
        private const bool EXPECTED_PORT_REDIRECTION = true;
        private const bool EXPECTED_PRINTER_REDIRECTION = true;
        private const RdpSounds EXPECTED_SOUND_REDIRECTION = RdpSounds.BringToThisComputer;
        private const string EXPECTED_START_PROGRAM = "alternate shell";

        [OneTimeSetUp]
        public void OnetimeSetup()
        {
            var connectionFileContents = Resources.test_remotedesktopconnection_rdp;
            _deserializer = new RemoteDesktopConnectionDeserializer();
            _connectionTreeModel = _deserializer.Deserialize(connectionFileContents);
        }

        [Test]
        public void ConnectionTreeModelHasARootNode()
        {
            var numberOfRootNodes = _connectionTreeModel.RootNodes.Count;
            Assert.That(numberOfRootNodes, Is.GreaterThan(0));
        }

        [Test]
        public void RootNodeHasConnectionInfo()
        {
            var rootNodeContents = _connectionTreeModel.RootNodes.First().Children.OfType<ConnectionInfo>();
            Assert.That(rootNodeContents, Is.Not.Empty);
        }

        [Test]
        public void HostnameImportedCorrectly()
        {
            var connectionInfo = _connectionTreeModel.RootNodes.First().Children.First();
            Assert.That(connectionInfo.Hostname, Is.EqualTo(EXPECTED_HOSTNAME));
        }

        [Test]
        public void PortImportedCorrectly()
        {
            var connectionInfo = _connectionTreeModel.RootNodes.First().Children.First();
            Assert.That(connectionInfo.Port, Is.EqualTo(EXPECTED_PORT));
        }

        [Test]
        public void UsernameImportedCorrectly()
        {
            var connectionInfo = _connectionTreeModel.RootNodes.First().Children.First();
            Assert.That(connectionInfo.Username, Is.EqualTo(EXPECTED_USER_NAME));
        }

        [Test]
        public void DomainImportedCorrectly()
        {
            var connectionInfo = _connectionTreeModel.RootNodes.First().Children.First();
            Assert.That(connectionInfo.Domain, Is.EqualTo(EXPECTED_DOMAIN));
        }

        [Test]
        public void RdpColorsImportedCorrectly()
        {
            var connectionInfo = _connectionTreeModel.RootNodes.First().Children.First();
            Assert.That(connectionInfo.Colors, Is.EqualTo(EXPECTED_COLORS));
        }

        [Test]
        public void BitmapCachingImportedCorrectly()
        {
            var connectionInfo = _connectionTreeModel.RootNodes.First().Children.First();
            Assert.That(connectionInfo.CacheBitmaps, Is.EqualTo(EXPECTED_BITMAP_CACHING));
        }

        [Test]
        public void ResolutionImportedCorrectly()
        {
            var connectionInfo = _connectionTreeModel.RootNodes.First().Children.First();
            Assert.That(connectionInfo.Resolution, Is.EqualTo(EXPECTED_RESOLUTION_MODE));
        }

        [Test]
        public void DisplayWallpaperImportedCorrectly()
        {
            var connectionInfo = _connectionTreeModel.RootNodes.First().Children.First();
            Assert.That(connectionInfo.DisplayWallpaper, Is.EqualTo(EXPECTED_WALLPAPER_DISPLAY));
        }

        [Test]
        public void DisplayThemesImportedCorrectly()
        {
            var connectionInfo = _connectionTreeModel.RootNodes.First().Children.First();
            Assert.That(connectionInfo.DisplayThemes, Is.EqualTo(EXPECTED_THEMES_DISPLAY));
        }

        [Test]
        public void FontSmoothingImportedCorrectly()
        {
            var connectionInfo = _connectionTreeModel.RootNodes.First().Children.First();
            Assert.That(connectionInfo.EnableFontSmoothing, Is.EqualTo(EXPECTED_FONT_SMOOTHING));
        }

        [Test]
        public void DesktopCompositionImportedCorrectly()
        {
            var connectionInfo = _connectionTreeModel.RootNodes.First().Children.First();
            Assert.That(connectionInfo.EnableDesktopComposition, Is.EqualTo(EXPECTED_DESKTOP_COMPOSITION));
        }

        [Test]
        public void SmartcardRedirectionImportedCorrectly()
        {
            var connectionInfo = _connectionTreeModel.RootNodes.First().Children.First();
            Assert.That(connectionInfo.RedirectSmartCards, Is.EqualTo(EXPECTED_SMARTCARD_REDIRECTION));
        }

        [Test]
        public void DriveRedirectionImportedCorrectly()
        {
            var connectionInfo = _connectionTreeModel.RootNodes.First().Children.First();
            Assert.That(connectionInfo.RedirectDiskDrives, Is.EqualTo(EXPECTED_DRIVE_REDIRECTION));
        }

        [Test]
        public void PortRedirectionImportedCorrectly()
        {
            var connectionInfo = _connectionTreeModel.RootNodes.First().Children.First();
            Assert.That(connectionInfo.RedirectPorts, Is.EqualTo(EXPECTED_PORT_REDIRECTION));
        }

        [Test]
        public void PrinterRedirectionImportedCorrectly()
        {
            var connectionInfo = _connectionTreeModel.RootNodes.First().Children.First();
            Assert.That(connectionInfo.RedirectPrinters, Is.EqualTo(EXPECTED_PRINTER_REDIRECTION));
        }

        [Test]
        public void SoundRedirectionImportedCorrectly()
        {
            var connectionInfo = _connectionTreeModel.RootNodes.First().Children.First();
            Assert.That(connectionInfo.RedirectSound, Is.EqualTo(EXPECTED_SOUND_REDIRECTION));
        }

        [Test]
        public void LoadBalanceInfoImportedCorrectly()
        {
            var connectionInfo = _connectionTreeModel.RootNodes.First().Children.First();
            Assert.That(connectionInfo.LoadBalanceInfo, Is.EqualTo(EXPECTED_LOAD_BALANCE_INFO));
        }

        [Test]
        public void StartProgramImportedCorrectly()
        {
            var connectionInfo = _connectionTreeModel.RootNodes.First().Children.First();
            Assert.That(connectionInfo.RdpStartProgram, Is.EqualTo(EXPECTED_START_PROGRAM));
        }

        //[Test]
        //public void GatewayHostnameImportedCorrectly()
        //{
        //    var connectionInfo = _connectionTreeModel.RootNodes.First().Children.First();
        //    Assert.That(connectionInfo.RDGatewayHostname, Is.EqualTo(_expectedGatewayHostname));
        //}
    }
}