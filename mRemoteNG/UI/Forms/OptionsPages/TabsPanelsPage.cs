using mRemoteNG.Properties;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    public sealed partial class TabsPanelsPage
    {
        public TabsPanelsPage()
        {
            InitializeComponent();
            ApplyTheme();
            PageIcon = Resources.ImageConverter.GetImageAsIcon(Properties.Resources.Tab_16x);
        }

        public override string PageName
        {
            get => Language.TabsAndPanels.Replace("&&", "&");
            set { }
        }

        public override void ApplyLanguage()
        {
            base.ApplyLanguage();

            chkAlwaysShowPanelTabs.Text = Language.AlwaysShowPanelTabs;
            chkAlwaysShowConnectionTabs.Text = Language.AlwaysShowConnectionTabs;
            chkOpenNewTabRightOfSelected.Text = Language.OpenNewTabRight;
            chkShowLogonInfoOnTabs.Text = Language.ShowLogonInfoOnTabs;
            chkShowProtocolOnTabs.Text = Language.ShowProtocolOnTabs;
            chkIdentifyQuickConnectTabs.Text = Language.IdentifyQuickConnectTabs;
            chkDoubleClickClosesTab.Text = Language.DoubleClickTabClosesIt;
            chkAlwaysShowPanelSelectionDlg.Text = Language.AlwaysShowPanelSelection;
            chkCreateEmptyPanelOnStart.Text = Language.CreateEmptyPanelOnStartUp;
            lblPanelName.Text = $@"{Language.PanelName}:";
        }

        public override void LoadSettings()
        {
            chkAlwaysShowPanelTabs.Checked = OptionsTabsPanelsPage.Default.AlwaysShowPanelTabs;
            chkAlwaysShowConnectionTabs.Checked = OptionsTabsPanelsPage.Default.AlwaysShowConnectionTabs;
            chkOpenNewTabRightOfSelected.Checked = OptionsTabsPanelsPage.Default.OpenTabsRightOfSelected;
            chkShowLogonInfoOnTabs.Checked = OptionsTabsPanelsPage.Default.ShowLogonInfoOnTabs;
            chkShowProtocolOnTabs.Checked = OptionsTabsPanelsPage.Default.ShowProtocolOnTabs;
            chkIdentifyQuickConnectTabs.Checked = OptionsTabsPanelsPage.Default.IdentifyQuickConnectTabs;
            chkDoubleClickClosesTab.Checked = OptionsTabsPanelsPage.Default.DoubleClickOnTabClosesIt;
            chkAlwaysShowPanelSelectionDlg.Checked = OptionsTabsPanelsPage.Default.AlwaysShowPanelSelectionDlg;
            chkCreateEmptyPanelOnStart.Checked = OptionsTabsPanelsPage.Default.CreateEmptyPanelOnStartUp;
            txtBoxPanelName.Text = OptionsTabsPanelsPage.Default.StartUpPanelName;
            UpdatePanelNameTextBox();
        }

        public override void SaveSettings()
        {
            base.SaveSettings();

            OptionsTabsPanelsPage.Default.AlwaysShowPanelTabs = chkAlwaysShowPanelTabs.Checked;
            OptionsTabsPanelsPage.Default.AlwaysShowConnectionTabs = chkAlwaysShowConnectionTabs.Checked;
            FrmMain.Default.ShowHidePanelTabs();

            OptionsTabsPanelsPage.Default.OpenTabsRightOfSelected = chkOpenNewTabRightOfSelected.Checked;
            OptionsTabsPanelsPage.Default.ShowLogonInfoOnTabs = chkShowLogonInfoOnTabs.Checked;
            OptionsTabsPanelsPage.Default.ShowProtocolOnTabs = chkShowProtocolOnTabs.Checked;
            OptionsTabsPanelsPage.Default.IdentifyQuickConnectTabs = chkIdentifyQuickConnectTabs.Checked;
            OptionsTabsPanelsPage.Default.DoubleClickOnTabClosesIt = chkDoubleClickClosesTab.Checked;
            OptionsTabsPanelsPage.Default.AlwaysShowPanelSelectionDlg = chkAlwaysShowPanelSelectionDlg.Checked;
            OptionsTabsPanelsPage.Default.CreateEmptyPanelOnStartUp = chkCreateEmptyPanelOnStart.Checked;
            OptionsTabsPanelsPage.Default.StartUpPanelName = txtBoxPanelName.Text;
        }

        private void UpdatePanelNameTextBox()
        {
            txtBoxPanelName.Enabled = chkCreateEmptyPanelOnStart.Checked;
        }

        private void chkCreateEmptyPanelOnStart_CheckedChanged(object sender, System.EventArgs e)
        {
            UpdatePanelNameTextBox();
        }
    }
}