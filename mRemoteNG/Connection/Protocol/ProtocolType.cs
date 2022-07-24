using mRemoteNG.Tools;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.Connection.Protocol
{
    public enum ProtocolType
    {
        [LocalizedAttributes.LocalizedDescription(nameof(Language.Rdp))]
        Rdp = 0,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.Vnc))]
        Vnc = 1,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.SshV1))]
        Ssh1 = 2,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.SshV2))]
        Ssh2 = 3,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.Telnet))]
        Telnet = 4,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.Rlogin))]
        Rlogin = 5,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.Raw))]
        Raw = 6,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.Http))]
        Http = 7,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.Https))]
        Https = 8,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.PowerShell))]
        PowerShell = 10,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.ExternalTool))]
        IntApp = 20
    }
}