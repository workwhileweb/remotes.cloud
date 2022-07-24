using System.Threading;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.RDP;
using NUnit.Framework;

namespace mRemoteNGTests.UI.Window.ConfigWindowTests
{
    [Apartment(ApartmentState.STA)]
    public class ConfigWindowRdpSpecialTests : ConfigWindowSpecialTestsBase
    {
        protected override ProtocolType Protocol => ProtocolType.Rdp;

        [Test]
        public void PropertyShownWhenActive_RdpMinutesToIdleTimeout()
        {
            ConnectionInfo.RdpMinutesToIdleTimeout = 1;
            ExpectedPropertyList.Add(nameof(mRemoteNG.Connection.ConnectionInfo.RdpAlertIdleTimeout));

            RunVerification();
        }

        [TestCase(RdGatewayUsageMethod.Always)]
        [TestCase(RdGatewayUsageMethod.Detect)]
        public void RdGatewayPropertiesShown_WhenRdGatewayUsageMethodIsNotNever(RdGatewayUsageMethod gatewayUsageMethod)
        {
            ConnectionInfo.RdGatewayUsageMethod = gatewayUsageMethod;
            ConnectionInfo.RdGatewayUseConnectionCredentials = RdGatewayUseConnectionCredentials.Yes;
            ExpectedPropertyList.AddRange(new []
            {
                nameof(mRemoteNG.Connection.ConnectionInfo.RdGatewayHostname),
                nameof(mRemoteNG.Connection.ConnectionInfo.RdGatewayUseConnectionCredentials)
            });

            RunVerification();
        }

        [TestCase(RdGatewayUseConnectionCredentials.No)]
        [TestCase(RdGatewayUseConnectionCredentials.SmartCard)]
        public void RdGatewayPropertiesShown_WhenRDGatewayUseConnectionCredentialsIsNotYes(RdGatewayUseConnectionCredentials useConnectionCredentials)
        {
            ConnectionInfo.RdGatewayUsageMethod = RdGatewayUsageMethod.Always;
            ConnectionInfo.RdGatewayUseConnectionCredentials = useConnectionCredentials;
            ExpectedPropertyList.AddRange(new []
            {
                nameof(mRemoteNG.Connection.ConnectionInfo.RdGatewayHostname),
                nameof(mRemoteNG.Connection.ConnectionInfo.RdGatewayUsername),
                nameof(mRemoteNG.Connection.ConnectionInfo.RdGatewayPassword),
                nameof(mRemoteNG.Connection.ConnectionInfo.RdGatewayDomain),
                nameof(mRemoteNG.Connection.ConnectionInfo.RdGatewayUseConnectionCredentials)
            });

            RunVerification();
        }

        [Test]
        public void SoundQualityPropertyShown_WhenRdpSoundsSetToBringToThisComputer()
        {
            ConnectionInfo.RedirectSound = RdpSounds.BringToThisComputer;
            ExpectedPropertyList.Add(nameof(mRemoteNG.Connection.ConnectionInfo.SoundQuality));

            RunVerification();
        }

        [TestCase(RdpResolutions.FitToWindow)]
        [TestCase(RdpResolutions.Fullscreen)]
        public void AutomaticResizePropertyShown_WhenResolutionIsDynamic(RdpResolutions resolution)
        {
            ConnectionInfo.Resolution = resolution;
            ExpectedPropertyList.Add(nameof(mRemoteNG.Connection.ConnectionInfo.AutomaticResize));

            RunVerification();
        }
    }
}
