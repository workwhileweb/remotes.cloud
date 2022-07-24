namespace mRemoteNGTests.TestHelpers
{
	/// <summary>
	/// A ConnectionInfo that has only the serializable properties as string types.
	/// Only used for testing.
	/// </summary>
	internal class SerializableConnectionInfoAllPropertiesOfType<TType>
	{
		public TType Description { get; set; }
		public TType Icon { get; set; }
		public TType Panel { get; set; }
		public TType Username { get; set; }
		public TType Password { get; set; }
		public TType Domain { get; set; }
		public TType Protocol { get; set; }
		public TType ExtApp { get; set; }
		public TType PuttySession { get; set; }
		public TType IcaEncryptionStrength { get; set; }
		public TType UseConsoleSession { get; set; }
		public TType RdpAuthenticationLevel { get; set; }
		public TType RdpMinutesToIdleTimeout { get; set; }
		public TType RdpAlertIdleTimeout { get; set; }
		public TType LoadBalanceInfo { get; set; }
		public TType RenderingEngine { get; set; }
		public TType UseCredSsp { get; set; }
		public TType UseRestrictedAdmin { get; set; }
		public TType UseRcg { get; set; }
		public TType RdGatewayUsageMethod { get; set; }
		public TType RdGatewayHostname { get; set; }
		public TType RdGatewayUseConnectionCredentials { get; set; }
		public TType RdGatewayUsername { get; set; }
		public TType RdGatewayPassword { get; set; }
		public TType RdGatewayDomain { get; set; }
		public TType Resolution { get; set; }
		public TType AutomaticResize { get; set; }
		public TType Colors { get; set; }
		public TType CacheBitmaps { get; set; }
		public TType DisplayWallpaper { get; set; }
		public TType DisplayThemes { get; set; }
		public TType EnableFontSmoothing { get; set; }
		public TType EnableDesktopComposition { get; set; }
		public TType DisableFullWindowDrag { get; set; }
		public TType DisableMenuAnimations { get; set; }
		public TType DisableCursorShadow { get; set; }
		public TType DisableCursorBlinking { get; set; }
		public TType RedirectKeys { get; set; }
		public TType RedirectDiskDrives { get; set; }
		public TType RedirectPrinters { get; set; }
        public TType RedirectClipboard { get; set; }
        public TType RedirectPorts { get; set; }
		public TType RedirectSmartCards { get; set; }
		public TType RedirectSound { get; set; }
		public TType SoundQuality { get; set; }
		public TType RedirectAudioCapture { get; set; }
		public TType PreExtApp { get; set; }
		public TType PostExtApp { get; set; }
		public TType MacAddress { get; set; }
        public TType UserField { get; set; }
        public TType Favorite { get; set; }
        public TType VmId { get; set; }
        public TType UseVmId { get; set; }
        public TType VncCompression { get; set; }
		public TType VncEncoding { get; set; }
		public TType VncAuthMode { get; set; }
		public TType VncProxyType { get; set; }
		public TType VncProxyIp { get; set; }
		public TType VncProxyPort { get; set; }
		public TType VncProxyUsername { get; set; }
		public TType VncProxyPassword { get; set; }
		public TType VncColors { get; set; }
		public TType VncSmartSizeMode { get; set; }
		public TType VncViewOnly { get; set; }
        public TType RdpVersion { get; set; }
        public TType UseEnhancedMode { get; set; }
        public TType SshOptions { get; set; }
        public TType SshTunnelConnectionName { get; set; }
        public TType RdpStartProgram { get; set; }
        public TType RdpStartProgramWorkDir { get; set; }
		public TType OpeningCommand { get; set; }
		public TType UserViaApi { get; set; }
		public TType Ec2InstanceId { get; set; }
		public TType Ec2Region { get; set; }
	}
}
