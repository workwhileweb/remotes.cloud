using System;
using mRemoteNG.App;
using mRemoteNG.Properties;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    public sealed partial class CredentialsPage : OptionsPage
    {
        public CredentialsPage()
        {
            InitializeComponent();
            ApplyTheme();
            PageIcon = Resources.ImageConverter.GetImageAsIcon(Properties.Resources.Key_16x);
        }

        public override string PageName
        {
            get => Language.Credentials;
            set { }
        }

        public override void ApplyLanguage()
        {
            base.ApplyLanguage();
            lblDefaultCredentials.Text = Language.EmptyUsernamePasswordDomainFields;
            radCredentialsNoInfo.Text = Language.None;
            radCredentialsWindows.Text = Language.MyCurrentWindowsCreds;
            radCredentialsCustom.Text = Language.TheFollowing;
            lblCredentialsUsername.Text = Language.Username;
            lblCredentialsPassword.Text = Language.Password;
            lblCredentialsDomain.Text = Language.Domain;
        }

        public override void LoadSettings()
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (OptionsCredentialsPage.Default.EmptyCredentials)
            {
                case "noinfo":
                    radCredentialsNoInfo.Checked = true;
                    break;
                case "windows":
                    radCredentialsWindows.Checked = true;
                    break;
                case "custom":
                    radCredentialsCustom.Checked = true;
                    break;
            }

            txtCredentialsUsername.Text = OptionsCredentialsPage.Default.DefaultUsername;
            var cryptographyProvider = new LegacyRijndaelCryptographyProvider();
            txtCredentialsPassword.Text =
                cryptographyProvider.Decrypt(OptionsCredentialsPage.Default.DefaultPassword, Runtime.EncryptionKey);
            txtCredentialsDomain.Text = OptionsCredentialsPage.Default.DefaultDomain;
            txtCredentialsUserViaAPI.Text = OptionsCredentialsPage.Default.UserViaAPDefault;
        }

        public override void SaveSettings()
        {
            if (radCredentialsNoInfo.Checked)
            {
                OptionsCredentialsPage.Default.EmptyCredentials = "noinfo";
            }
            else if (radCredentialsWindows.Checked)
            {
                OptionsCredentialsPage.Default.EmptyCredentials = "windows";
            }
            else if (radCredentialsCustom.Checked)
            {
                OptionsCredentialsPage.Default.EmptyCredentials = "custom";
            }

            OptionsCredentialsPage.Default.DefaultUsername = txtCredentialsUsername.Text;
            var cryptographyProvider = new LegacyRijndaelCryptographyProvider();
            OptionsCredentialsPage.Default.DefaultPassword =
                cryptographyProvider.Encrypt(txtCredentialsPassword.Text, Runtime.EncryptionKey);
            OptionsCredentialsPage.Default.DefaultDomain = txtCredentialsDomain.Text;
            OptionsCredentialsPage.Default.UserViaAPDefault = txtCredentialsUserViaAPI.Text;
        }

        private void radCredentialsCustom_CheckedChanged(object sender, EventArgs e)
        {
            lblCredentialsUsername.Enabled = radCredentialsCustom.Checked;
            lblCredentialsPassword.Enabled = radCredentialsCustom.Checked;
            lblCredentialsDomain.Enabled = radCredentialsCustom.Checked;
            txtCredentialsUsername.Enabled = radCredentialsCustom.Checked;
            txtCredentialsPassword.Enabled = radCredentialsCustom.Checked;
            txtCredentialsDomain.Enabled = radCredentialsCustom.Checked;
            txtCredentialsUserViaAPI.Enabled = radCredentialsCustom.Checked;
            lblCredentialsUserViaAPI.Enabled = radCredentialsCustom.Checked;
        }
    }
}