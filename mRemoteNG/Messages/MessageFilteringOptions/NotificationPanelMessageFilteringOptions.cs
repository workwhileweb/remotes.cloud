using mRemoteNG.Properties;

namespace mRemoteNG.Messages.MessageFilteringOptions
{
    public class NotificationPanelMessageFilteringOptions : IMessageTypeFilteringOptions
    {
        public bool AllowDebugMessages
        {
            get => OptionsNotificationsPage.Default.NotificationPanelWriterWriteDebugMsgs;
            set => OptionsNotificationsPage.Default.NotificationPanelWriterWriteDebugMsgs = value;
        }

        public bool AllowInfoMessages
        {
            get => OptionsNotificationsPage.Default.NotificationPanelWriterWriteInfoMsgs;
            set => OptionsNotificationsPage.Default.NotificationPanelWriterWriteInfoMsgs = value;
        }

        public bool AllowWarningMessages
        {
            get => OptionsNotificationsPage.Default.NotificationPanelWriterWriteWarningMsgs;
            set => OptionsNotificationsPage.Default.NotificationPanelWriterWriteWarningMsgs = value;
        }

        public bool AllowErrorMessages
        {
            get => OptionsNotificationsPage.Default.NotificationPanelWriterWriteErrorMsgs;
            set => OptionsNotificationsPage.Default.NotificationPanelWriterWriteErrorMsgs = value;
        }
    }
}