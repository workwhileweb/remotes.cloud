using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Properties;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    public sealed partial class NotificationsPage
    {
        public NotificationsPage()
        {
            InitializeComponent();
            ApplyTheme();
            PageIcon = Resources.ImageConverter.GetImageAsIcon(Properties.Resources.LogError_16x);
        }

        public override string PageName
        {
            get => Language.Notifications;
            set { }
        }

        public override void ApplyLanguage()
        {
            base.ApplyLanguage();

            // notifications panel
            groupBoxNotifications.Text = Language.Notifications;
            labelNotificationsShowTypes.Text = Language.ShowTheseMessageTypes;
            chkShowDebugInMC.Text = Language.Debug;
            chkShowInfoInMC.Text = Language.Informations;
            chkShowWarningInMC.Text = Language.Warnings;
            chkShowErrorInMC.Text = Language.Errors;
            labelSwitchToErrorsAndInfos.Text = Language.SwitchToErrorsAndInfos;
            chkSwitchToMCInformation.Text = Language.Informations;
            chkSwitchToMCWarnings.Text = Language.Warnings;
            chkSwitchToMCErrors.Text = Language.Errors;

            // logging
            groupBoxLogging.Text = Language.Logging;
            chkLogDebugMsgs.Text = Language.Debug;
            chkLogInfoMsgs.Text = Language.Informations;
            chkLogWarningMsgs.Text = Language.Warnings;
            chkLogErrorMsgs.Text = Language.Errors;
            chkLogToCurrentDir.Text = Language.LogToAppDir;
            labelLogFilePath.Text = Language.LogFilePath;
            labelLogTheseMsgTypes.Text = Language.LogTheseMessageTypes;
            buttonOpenLogFile.Text = Language.OpenFile;
            buttonSelectLogPath.Text = Language.ChoosePath;
            buttonRestoreDefaultLogPath.Text = Language.UseDefault;

            // popups
            groupBoxPopups.Text = Language.Popups;
            labelPopupShowTypes.Text = Language.ShowTheseMessageTypes;
            chkPopupDebug.Text = Language.Debug;
            chkPopupInfo.Text = Language.Informations;
            chkPopupWarning.Text = Language.Warnings;
            chkPopupError.Text = Language.Errors;
        }

        public override void LoadSettings()
        {
            LoadNotificationPanelSettings();
            LoadLoggingSettings();
            LoadPopupSettings();
        }

        public override void SaveSettings()
        {
            SaveNotificationPanelSettings();
            SaveLoggingSettings();
            SavePopupSettings();
        }

        private void LoadNotificationPanelSettings()
        {
            chkShowDebugInMC.Checked = OptionsNotificationsPage.Default.NotificationPanelWriterWriteDebugMsgs;
            chkShowInfoInMC.Checked = OptionsNotificationsPage.Default.NotificationPanelWriterWriteInfoMsgs;
            chkShowWarningInMC.Checked = OptionsNotificationsPage.Default.NotificationPanelWriterWriteWarningMsgs;
            chkShowErrorInMC.Checked = OptionsNotificationsPage.Default.NotificationPanelWriterWriteErrorMsgs;
            chkSwitchToMCInformation.Checked = OptionsNotificationsPage.Default.SwitchToMCOnInformation;
            chkSwitchToMCWarnings.Checked = OptionsNotificationsPage.Default.SwitchToMCOnWarning;
            chkSwitchToMCErrors.Checked = OptionsNotificationsPage.Default.SwitchToMCOnError;
        }

        private void LoadLoggingSettings()
        {
            chkLogToCurrentDir.Checked = OptionsNotificationsPage.Default.LogToApplicationDirectory;
            textBoxLogPath.Text = OptionsNotificationsPage.Default.LogFilePath;
            chkLogDebugMsgs.Checked = OptionsNotificationsPage.Default.TextLogMessageWriterWriteDebugMsgs;
            chkLogInfoMsgs.Checked = OptionsNotificationsPage.Default.TextLogMessageWriterWriteInfoMsgs;
            chkLogWarningMsgs.Checked = OptionsNotificationsPage.Default.TextLogMessageWriterWriteWarningMsgs;
            chkLogErrorMsgs.Checked = OptionsNotificationsPage.Default.TextLogMessageWriterWriteErrorMsgs;
        }

        private void LoadPopupSettings()
        {
            chkPopupDebug.Checked = OptionsNotificationsPage.Default.PopupMessageWriterWriteDebugMsgs;
            chkPopupInfo.Checked = OptionsNotificationsPage.Default.PopupMessageWriterWriteInfoMsgs;
            chkPopupWarning.Checked = OptionsNotificationsPage.Default.PopupMessageWriterWriteWarningMsgs;
            chkPopupError.Checked = OptionsNotificationsPage.Default.PopupMessageWriterWriteErrorMsgs;
        }

        private void SaveNotificationPanelSettings()
        {
            OptionsNotificationsPage.Default.NotificationPanelWriterWriteDebugMsgs = chkShowDebugInMC.Checked;
            OptionsNotificationsPage.Default.NotificationPanelWriterWriteInfoMsgs = chkShowInfoInMC.Checked;
            OptionsNotificationsPage.Default.NotificationPanelWriterWriteWarningMsgs = chkShowWarningInMC.Checked;
            OptionsNotificationsPage.Default.NotificationPanelWriterWriteErrorMsgs = chkShowErrorInMC.Checked;
            OptionsNotificationsPage.Default.SwitchToMCOnInformation = chkSwitchToMCInformation.Checked;
            OptionsNotificationsPage.Default.SwitchToMCOnWarning = chkSwitchToMCWarnings.Checked;
            OptionsNotificationsPage.Default.SwitchToMCOnError = chkSwitchToMCErrors.Checked;
        }

        private void SaveLoggingSettings()
        {
            OptionsNotificationsPage.Default.LogToApplicationDirectory = chkLogToCurrentDir.Checked;
            OptionsNotificationsPage.Default.LogFilePath = textBoxLogPath.Text;
            Logger.Instance.SetLogPath(OptionsNotificationsPage.Default.LogFilePath);
            OptionsNotificationsPage.Default.TextLogMessageWriterWriteDebugMsgs = chkLogDebugMsgs.Checked;
            OptionsNotificationsPage.Default.TextLogMessageWriterWriteInfoMsgs = chkLogInfoMsgs.Checked;
            OptionsNotificationsPage.Default.TextLogMessageWriterWriteWarningMsgs = chkLogWarningMsgs.Checked;
            OptionsNotificationsPage.Default.TextLogMessageWriterWriteErrorMsgs = chkLogErrorMsgs.Checked;
        }

        private void SavePopupSettings()
        {
            OptionsNotificationsPage.Default.PopupMessageWriterWriteDebugMsgs = chkPopupDebug.Checked;
            OptionsNotificationsPage.Default.PopupMessageWriterWriteInfoMsgs = chkPopupInfo.Checked;
            OptionsNotificationsPage.Default.PopupMessageWriterWriteWarningMsgs = chkPopupWarning.Checked;
            OptionsNotificationsPage.Default.PopupMessageWriterWriteErrorMsgs = chkPopupError.Checked;
        }

        private void buttonSelectLogPath_Click(object sender, System.EventArgs e)
        {
            var currentFile = textBoxLogPath.Text;
            var currentDirectory = Path.GetDirectoryName(currentFile);
            saveFileDialogLogging.Title = Language.ChooseLogPath;
            saveFileDialogLogging.Filter = @"Log file|*.log";
            saveFileDialogLogging.InitialDirectory = currentDirectory;
            saveFileDialogLogging.FileName = currentFile;
            var dialogResult = saveFileDialogLogging.ShowDialog();
            if (dialogResult != DialogResult.OK) return;
            textBoxLogPath.Text = saveFileDialogLogging.FileName;
        }

        private void buttonRestoreDefaultLogPath_Click(object sender, System.EventArgs e)
        {
            textBoxLogPath.Text = Logger.DefaultLogPath;
        }

        private void buttonOpenLogFile_Click(object sender, System.EventArgs e)
        {
            if (Path.GetExtension(textBoxLogPath.Text) == ".log")
                Process.Start(textBoxLogPath.Text);
        }

        private void chkLogToCurrentDir_CheckedChanged(object sender, System.EventArgs e)
        {
            buttonSelectLogPath.Enabled = !chkLogToCurrentDir.Checked;
            buttonRestoreDefaultLogPath.Enabled = !chkLogToCurrentDir.Checked;
            textBoxLogPath.Text = Logger.DefaultLogPath;
        }
    }
}