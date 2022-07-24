using System;
using mRemoteNG.Properties;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    public sealed partial class StartupExitPage
    {
        public StartupExitPage()
        {
            InitializeComponent();
            ApplyTheme();
            PageIcon = Resources.ImageConverter.GetImageAsIcon(Properties.Resources.StartupProject_16x);
        }

        public override string PageName
        {
            get => Language.StartupExit;
            set { }
        }

        public override void ApplyLanguage()
        {
            base.ApplyLanguage();

            chkReconnectOnStart.Text = Language.ReconnectAtStartup;
            chkSingleInstance.Text = Language.AllowOnlySingleInstance;
            chkStartMinimized.Text = Language.StartMinimized;
        }

        public override void SaveSettings()
        {
            base.SaveSettings();

            OptionsStartupExitPage.Default.OpenConsFromLastSession = chkReconnectOnStart.Checked;
            OptionsStartupExitPage.Default.SingleInstance = chkSingleInstance.Checked;
            OptionsStartupExitPage.Default.StartMinimized = chkStartMinimized.Checked;
            OptionsStartupExitPage.Default.StartFullScreen = chkStartFullScreen.Checked;
        }

        private void StartupExitPage_Load(object sender, EventArgs e)
        {
            chkReconnectOnStart.Checked = OptionsStartupExitPage.Default.OpenConsFromLastSession;
            chkSingleInstance.Checked = OptionsStartupExitPage.Default.SingleInstance;
            chkStartMinimized.Checked = OptionsStartupExitPage.Default.StartMinimized;
            chkStartFullScreen.Checked = OptionsStartupExitPage.Default.StartFullScreen;
            ;
        }

        private void chkStartFullScreen_CheckedChanged(object sender, EventArgs e)
        {
            if (chkStartFullScreen.Checked && chkStartMinimized.Checked)
            {
                chkStartMinimized.Checked = false;
            } 
        }

        private void chkStartMinimized_CheckedChanged(object sender, EventArgs e)
        {
            if (chkStartMinimized.Checked && chkStartFullScreen.Checked)
            {
                chkStartFullScreen.Checked = false;
            }
        }
    }
}