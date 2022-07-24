using System;
using System.Drawing;

namespace mRemoteNG.Connection.Protocol.RDP
{
    public static class RdpExtensions
    {
        public static Rectangle GetResolutionRectangle(this RdpResolutions resolution)
        {
            string[] resolutionParts = null;
            if (resolution != RdpResolutions.FitToWindow & resolution != RdpResolutions.Fullscreen &
                resolution != RdpResolutions.SmartSize)
            {
                resolutionParts = resolution.ToString().Replace("Res", "").Split('x');
            }

            if (resolutionParts == null || resolutionParts.Length != 2)
            {
                return new Rectangle(0, 0, 0, 0);
            }
            else
            {
                return new Rectangle(0, 0, Convert.ToInt32(resolutionParts[0]), Convert.ToInt32(resolutionParts[1]));
            }
        }
    }
}
