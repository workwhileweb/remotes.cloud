using mRemoteNG.Properties;

namespace mRemoteNG.Messages.MessageFilteringOptions
{
    public class NotificationPanelSwitchOnMessageFilteringOptions : IMessageTypeFilteringOptions
    {
        public bool AllowDebugMessages
        {
            get => false;
            set { }
        }

        public bool AllowInfoMessages
        {
            get => OptionsNotificationsPage.Default.SwitchToMCOnInformation;
            set => OptionsNotificationsPage.Default.SwitchToMCOnInformation = value;
        }

        public bool AllowWarningMessages
        {
            get => OptionsNotificationsPage.Default.SwitchToMCOnWarning;
            set => OptionsNotificationsPage.Default.SwitchToMCOnWarning = value;
        }

        public bool AllowErrorMessages
        {
            get => OptionsNotificationsPage.Default.SwitchToMCOnError;
            set => OptionsNotificationsPage.Default.SwitchToMCOnError = value;
        }
    }
}