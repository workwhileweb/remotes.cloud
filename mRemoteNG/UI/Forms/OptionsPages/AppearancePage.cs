using System;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Properties;
using mRemoteNG.Tools;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    public sealed partial class AppearancePage
    {
        public AppearancePage()
        {
            InitializeComponent();
            ApplyTheme();
            PageIcon = Resources.ImageConverter.GetImageAsIcon(Properties.Resources.Panel_16x);
        }

        public override string PageName
        {
            get => Language.Appearance;
            set { }
        }

        public override void ApplyLanguage()
        {
            base.ApplyLanguage();

            lblLanguage.Text = Language.LanguageString;
            lblLanguageRestartRequired.Text =
                string.Format(Language.LanguageRestartRequired, Application.ProductName);
            chkShowDescriptionTooltipsInTree.Text = Language.ShowDescriptionTooltips;
            chkShowFullConnectionsFilePathInTitle.Text = Language.ShowFullConsFilePath;
            chkShowSystemTrayIcon.Text = Language.AlwaysShowSysTrayIcon;
            chkMinimizeToSystemTray.Text = Language.MinimizeToSysTray;
            chkCloseToSystemTray.Text = Language.CloseToSysTray;
        }

        public override void LoadSettings()
        {
            cboLanguage.Items.Clear();
            cboLanguage.Items.Add(Language.LanguageDefault);

            foreach (var nativeName in SupportedCultures.CultureNativeNames)
            {
                cboLanguage.Items.Add(nativeName);
            }

            if (!string.IsNullOrEmpty(Settings.Default.OverrideUICulture) &&
                SupportedCultures.IsNameSupported(Settings.Default.OverrideUICulture))
            {
                cboLanguage.SelectedItem = SupportedCultures.get_CultureNativeName(Settings.Default.OverrideUICulture);
            }

            if (cboLanguage.SelectedIndex == -1)
            {
                cboLanguage.SelectedIndex = 0;
            }

            chkShowDescriptionTooltipsInTree.Checked = OptionsAppearancePage.Default.ShowDescriptionTooltipsInTree;
            chkShowFullConnectionsFilePathInTitle.Checked = OptionsAppearancePage.Default.ShowCompleteConsPathInTitle;
            chkShowSystemTrayIcon.Checked = OptionsAppearancePage.Default.ShowSystemTrayIcon;
            chkMinimizeToSystemTray.Checked = OptionsAppearancePage.Default.MinimizeToTray;
            chkCloseToSystemTray.Checked = OptionsAppearancePage.Default.CloseToTray;
        }

        public override void SaveSettings()
        {
            if (cboLanguage.SelectedIndex > 0 &&
                SupportedCultures.IsNativeNameSupported(Convert.ToString(cboLanguage.SelectedItem)))
            {
                Settings.Default.OverrideUICulture = SupportedCultures.get_CultureName(Convert.ToString(cboLanguage.SelectedItem));
            }
            else
            {
                Settings.Default.OverrideUICulture = string.Empty;
            }

            OptionsAppearancePage.Default.ShowDescriptionTooltipsInTree = chkShowDescriptionTooltipsInTree.Checked;
            OptionsAppearancePage.Default.ShowCompleteConsPathInTitle = chkShowFullConnectionsFilePathInTitle.Checked;
            FrmMain.Default.ShowFullPathInTitle = chkShowFullConnectionsFilePathInTitle.Checked;

            OptionsAppearancePage.Default.ShowSystemTrayIcon = chkShowSystemTrayIcon.Checked;
            if (OptionsAppearancePage.Default.ShowSystemTrayIcon)
            {
                if (Runtime.NotificationAreaIcon == null)
                {
                    Runtime.NotificationAreaIcon = new NotificationAreaIcon();
                }
            }
            else
            {
                if (Runtime.NotificationAreaIcon != null)
                {
                    Runtime.NotificationAreaIcon.Dispose();
                    Runtime.NotificationAreaIcon = null;
                }
            }

            OptionsAppearancePage.Default.MinimizeToTray = chkMinimizeToSystemTray.Checked;
            OptionsAppearancePage.Default.CloseToTray = chkCloseToSystemTray.Checked;
        }
    }
}