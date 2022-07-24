using System.Diagnostics;
using System.Windows.Forms;
using mRemoteNG.App.Info;
using mRemoteNG.Themes;
using mRemoteNG.Resources.Language;
using System.Reflection;
using mRemoteNG.Properties;
using System.Runtime.InteropServices;

namespace mRemoteNG.UI.Forms
{
    public partial class FrmAbout : Form
    {
        public static FrmAbout Instance { get; set; } = new();

        private FrmAbout()
        {
            InitializeComponent();
            Icon = Resources.ImageConverter.GetImageAsIcon(Properties.Resources.UIAboutBox_16x);
            ThemeManager.GetInstance().ThemeChanged += ApplyTheme;
            ApplyLanguage();
            ApplyTheme();
        }

        private void ApplyLanguage()
        {
            lblLicense.Text = Language.ReleasedUnderGPL;
            base.Text = Language.About;
            llChangelog.Text = Language.Changelog;
            llCredits.Text = Language.Credits;
            llLicense.Text = Language.License;
            lblCopyright.Text = GeneralAppInfo.Copyright;
            lblVersion.Text = $@"Version {GeneralAppInfo.ApplicationVersion}";
            AddPortableString();
        }

        [Conditional("PORTABLE")]
        private void AddPortableString() => lblTitle.Text += $@" {Language.PortableEdition}";

        private void ApplyTheme()
        {
            if (!ThemeManager.GetInstance().ThemingActive) return;
            if (!ThemeManager.GetInstance().ActiveAndExtended) return;
            pnlBottom.BackColor = ThemeManager.GetInstance().ActiveTheme.ExtendedPalette.GetColor("Dialog_Background");
            pnlBottom.ForeColor = ThemeManager.GetInstance().ActiveTheme.ExtendedPalette.GetColor("Dialog_Foreground");
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            e.Cancel = true;
            Hide();
        }

        private void llLicense_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenUrl("https://raw.githubusercontent.com/mRemoteNG/mRemoteNG/v" + Assembly.GetExecutingAssembly().GetName().Version.ToString().Substring(0, Assembly.GetExecutingAssembly().GetName().Version.ToString().Length - 2) + "-" + OptionsUpdatesPage.Default.CurrentUpdateChannelType + "/COPYING.txt");
            Close();
        }

        private void llChangelog_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenUrl("https://raw.githubusercontent.com/mRemoteNG/mRemoteNG/v" + Assembly.GetExecutingAssembly().GetName().Version.ToString().Substring(0, Assembly.GetExecutingAssembly().GetName().Version.ToString().Length - 2) + "-" + OptionsUpdatesPage.Default.CurrentUpdateChannelType + "/CHANGELOG.md");
            Close();
        }

        private void llCredits_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenUrl("https://raw.githubusercontent.com/mRemoteNG/mRemoteNG/v" + Assembly.GetExecutingAssembly().GetName().Version.ToString().Substring(0, Assembly.GetExecutingAssembly().GetName().Version.ToString().Length - 2) + "-" + OptionsUpdatesPage.Default.CurrentUpdateChannelType + "/CREDITS.md");
            Close();
        }

        private void OpenUrl(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}