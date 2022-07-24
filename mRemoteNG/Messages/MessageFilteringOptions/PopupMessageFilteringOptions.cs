using mRemoteNG.Properties;

namespace mRemoteNG.Messages.MessageFilteringOptions
{
    public class PopupMessageFilteringOptions : IMessageTypeFilteringOptions
    {
        public bool AllowDebugMessages
        {
            get => OptionsNotificationsPage.Default.PopupMessageWriterWriteDebugMsgs;
            set => OptionsNotificationsPage.Default.PopupMessageWriterWriteDebugMsgs = value;
        }

        public bool AllowInfoMessages
        {
            get => OptionsNotificationsPage.Default.PopupMessageWriterWriteInfoMsgs;
            set => OptionsNotificationsPage.Default.PopupMessageWriterWriteInfoMsgs = value;
        }

        public bool AllowWarningMessages
        {
            get => OptionsNotificationsPage.Default.PopupMessageWriterWriteWarningMsgs;
            set => OptionsNotificationsPage.Default.PopupMessageWriterWriteWarningMsgs = value;
        }

        public bool AllowErrorMessages
        {
            get => OptionsNotificationsPage.Default.PopupMessageWriterWriteErrorMsgs;
            set => OptionsNotificationsPage.Default.PopupMessageWriterWriteErrorMsgs = value;
        }
    }
}