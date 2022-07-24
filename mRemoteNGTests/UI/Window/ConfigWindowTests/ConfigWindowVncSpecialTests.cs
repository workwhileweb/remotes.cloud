using System.Threading;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.VNC;
using NUnit.Framework;

namespace mRemoteNGTests.UI.Window.ConfigWindowTests
{
    [Apartment(ApartmentState.STA)]
    public class ConfigWindowVncSpecialTests : ConfigWindowSpecialTestsBase
    {
        protected override ProtocolType Protocol => ProtocolType.Vnc;

        [Test]
        public void UserDomainPropertiesShown_WhenAuthModeIsWindows()
        {
            ConnectionInfo.VncAuthMode = ProtocolVnc.AuthMode.AuthWin;
            ExpectedPropertyList.AddRange(new []
            {
                nameof(ConnectionInfo.Username),
                nameof(ConnectionInfo.Domain),
            });
        }

        [TestCase(ProtocolVnc.ProxyType.ProxyHttp)]
        [TestCase(ProtocolVnc.ProxyType.ProxySocks5)]
        [TestCase(ProtocolVnc.ProxyType.ProxyUltra)]
        public void ProxyPropertiesShown_WhenProxyModeIsNotNone(ProtocolVnc.ProxyType proxyType)
        {
            ConnectionInfo.VncProxyType = proxyType;
            ExpectedPropertyList.AddRange(new[]
            {
                nameof(ConnectionInfo.VncProxyIp),
                nameof(ConnectionInfo.VncProxyPort),
                nameof(ConnectionInfo.VncProxyUsername),
                nameof(ConnectionInfo.VncProxyPassword),
            });
        }
    }
}
