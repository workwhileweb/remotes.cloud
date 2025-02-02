﻿using mRemoteNG.Tools;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.Connection.Protocol.RDP
{
    public enum RdGatewayUseConnectionCredentials
    {
        [LocalizedAttributes.LocalizedDescription(nameof(Language.UseDifferentUsernameAndPassword))]
        No = 0,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.UseSameUsernameAndPassword))]
        Yes = 1,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.UseSmartCard))]
        SmartCard = 2
    }
}