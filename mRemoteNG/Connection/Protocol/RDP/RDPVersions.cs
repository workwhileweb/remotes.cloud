using System;
using System.Collections.Generic;
using System.Text;

namespace mRemoteNG.Connection.Protocol.RDP
{
    public class RdpVersions
    {
        public static Version Rdc60 = new(6, 0, 6000);
        public static Version Rdc61 = new(6, 0, 6001);
        public static Version Rdc70 = new(6, 1, 7600);
        public static Version Rdc80 = new(6, 2, 9200);
    }
}