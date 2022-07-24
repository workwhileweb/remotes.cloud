using mRemoteNG.Properties;

namespace mRemoteNG.Messages.MessageFilteringOptions
{
    public class LogMessageTypeFilteringOptions : IMessageTypeFilteringOptions
    {
        public bool AllowDebugMessages
        {
            get => OptionsNotificationsPage.Default.TextLogMessageWriterWriteDebugMsgs;
            set => OptionsNotificationsPage.Default.TextLogMessageWriterWriteDebugMsgs = value;
        }

        public bool AllowInfoMessages
        {
            get => OptionsNotificationsPage.Default.TextLogMessageWriterWriteInfoMsgs;
            set => OptionsNotificationsPage.Default.TextLogMessageWriterWriteInfoMsgs = value;
        }

        public bool AllowWarningMessages
        {
            get => OptionsNotificationsPage.Default.TextLogMessageWriterWriteWarningMsgs;
            set => OptionsNotificationsPage.Default.TextLogMessageWriterWriteWarningMsgs = value;
        }

        public bool AllowErrorMessages
        {
            get => OptionsNotificationsPage.Default.TextLogMessageWriterWriteErrorMsgs;
            set => OptionsNotificationsPage.Default.TextLogMessageWriterWriteErrorMsgs = value;
        }
    }
}