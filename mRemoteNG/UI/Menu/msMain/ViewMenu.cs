using System;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Properties;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.Panels;
using mRemoteNG.UI.Window;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.UI.Menu
{
    public class ViewMenu : ToolStripMenuItem
    {
        private ToolStripMenuItem _mMenViewConnectionPanels;
        private ToolStripMenuItem _mMenReconnectAll;
        private ToolStripSeparator _mMenViewSep1;
        public ToolStripMenuItem MMenViewErrorsAndInfos;
        private ToolStripMenuItem _mMenViewAddConnectionPanel;
        private ToolStripSeparator _mMenViewSep2;
        private ToolStripMenuItem _mMenViewFullscreen;
        public ToolStripMenuItem MMenViewExtAppsToolbar;
        public ToolStripMenuItem MMenViewQuickConnectToolbar;
        public ToolStripMenuItem MMenViewMultiSshToolbar;
        private ToolStripMenuItem _mMenViewResetLayout;
        public ToolStripMenuItem MMenViewLockToolbars;
        private readonly PanelAdder _panelAdder;


        public ToolStrip TsExternalTools { get; set; }
        public ToolStrip TsQuickConnect { get; set; }
        public ToolStrip TsMultiSsh { get; set; }
        public FullscreenHandler FullscreenHandler { get; set; }
        public FrmMain MainForm { get; set; }


        public ViewMenu()
        {
            Initialize();
            _panelAdder = new PanelAdder();
        }

        private void Initialize()
        {
            _mMenViewAddConnectionPanel = new ToolStripMenuItem();
            _mMenViewConnectionPanels = new ToolStripMenuItem();
            _mMenViewSep1 = new ToolStripSeparator();
            MMenViewErrorsAndInfos = new ToolStripMenuItem();
            _mMenViewResetLayout = new ToolStripMenuItem();
            MMenViewLockToolbars = new ToolStripMenuItem();
            _mMenViewSep2 = new ToolStripSeparator();
            MMenViewQuickConnectToolbar = new ToolStripMenuItem();
            _mMenReconnectAll = new ToolStripMenuItem();
            MMenViewExtAppsToolbar = new ToolStripMenuItem();
            MMenViewMultiSshToolbar = new ToolStripMenuItem();
            _mMenViewFullscreen = new ToolStripMenuItem();

            // 
            // mMenView
            // 
            DropDownItems.AddRange(new ToolStripItem[]
            {
                MMenViewErrorsAndInfos,
                MMenViewQuickConnectToolbar,
                MMenViewExtAppsToolbar,
                MMenViewMultiSshToolbar,
                _mMenViewSep1,
                _mMenReconnectAll,
                _mMenViewAddConnectionPanel,
                _mMenViewConnectionPanels,
                _mMenViewResetLayout,
                MMenViewLockToolbars,
                _mMenViewSep2,
                _mMenViewFullscreen
            });
            Name = "mMenView";
            Size = new System.Drawing.Size(44, 20);
            Text = Language._View;
            //DropDownOpening += mMenView_DropDownOpening;
            // 
            // mMenViewAddConnectionPanel
            // 
            _mMenViewAddConnectionPanel.Image = Properties.Resources.InsertPanel_16x;
            _mMenViewAddConnectionPanel.Name = "mMenViewAddConnectionPanel";
            _mMenViewAddConnectionPanel.Size = new System.Drawing.Size(228, 22);
            _mMenViewAddConnectionPanel.Text = Language.AddConnectionPanel;
            _mMenViewAddConnectionPanel.Click += mMenViewAddConnectionPanel_Click;
            // 
            // mMenReconnectAll
            // 
            _mMenReconnectAll.Image = Properties.Resources.Refresh_16x;
            _mMenReconnectAll.Name = "mMenReconnectAll";
            _mMenReconnectAll.Size = new System.Drawing.Size(281, 22);
            _mMenReconnectAll.Text = Language.ReconnectAllConnections;
            _mMenReconnectAll.Click += mMenReconnectAll_Click;
            // 
            // mMenViewConnectionPanels
            // 
            _mMenViewConnectionPanels.Image = Properties.Resources.Panel_16x;
            _mMenViewConnectionPanels.Name = "mMenViewConnectionPanels";
            _mMenViewConnectionPanels.Size = new System.Drawing.Size(228, 22);
            _mMenViewConnectionPanels.Text = Language.ConnectionPanels;
            // 
            // mMenViewSep1
            // 
            _mMenViewSep1.Name = "mMenViewSep1";
            _mMenViewSep1.Size = new System.Drawing.Size(225, 6);
            // 
            // mMenViewErrorsAndInfos
            // 
            MMenViewErrorsAndInfos.Checked = true;
            MMenViewErrorsAndInfos.CheckState = CheckState.Checked;
            MMenViewErrorsAndInfos.Name = "mMenViewErrorsAndInfos";
            MMenViewErrorsAndInfos.Size = new System.Drawing.Size(228, 22);
            MMenViewErrorsAndInfos.Text = Language.Notifications;
            MMenViewErrorsAndInfos.Click += mMenViewErrorsAndInfos_Click;
            // 
            // mMenViewResetLayout
            // 
            _mMenViewResetLayout.Name = "mMenViewResetLayout";
            _mMenViewResetLayout.Size = new System.Drawing.Size(228, 22);
            _mMenViewResetLayout.Text = Language.ResetLayout;
            _mMenViewResetLayout.Click += mMenViewResetLayout_Click;
            // 
            // mMenViewLockToolbars
            // 
            MMenViewLockToolbars.Name = "mMenViewLockToolbars";
            MMenViewLockToolbars.Size = new System.Drawing.Size(228, 22);
            MMenViewLockToolbars.Text = Language.LockToolbars;
            MMenViewLockToolbars.Click += mMenViewLockToolbars_Click;
            // 
            // mMenViewSep2
            // 
            _mMenViewSep2.Name = "mMenViewSep2";
            _mMenViewSep2.Size = new System.Drawing.Size(225, 6);
            // 
            // mMenViewQuickConnectToolbar
            // 
            MMenViewQuickConnectToolbar.Name = "mMenViewQuickConnectToolbar";
            MMenViewQuickConnectToolbar.Size = new System.Drawing.Size(228, 22);
            MMenViewQuickConnectToolbar.Text = Language.QuickConnectToolbar;
            MMenViewQuickConnectToolbar.Click += mMenViewQuickConnectToolbar_Click;
            // 
            // mMenViewExtAppsToolbar
            // 
            MMenViewExtAppsToolbar.Name = "mMenViewExtAppsToolbar";
            MMenViewExtAppsToolbar.Size = new System.Drawing.Size(228, 22);
            MMenViewExtAppsToolbar.Text = Language.ExternalToolsToolbar;
            MMenViewExtAppsToolbar.Click += mMenViewExtAppsToolbar_Click;
            // 
            // mMenViewMultiSSHToolbar
            // 
            MMenViewMultiSshToolbar.Name = "mMenViewMultiSSHToolbar";
            MMenViewMultiSshToolbar.Size = new System.Drawing.Size(279, 26);
            MMenViewMultiSshToolbar.Text = Language.MultiSshToolbar;
            MMenViewMultiSshToolbar.Click += mMenViewMultiSSHToolbar_Click;
            // 
            // mMenViewFullscreen
            // 
            _mMenViewFullscreen.Image = Properties.Resources.FullScreen_16x;
            _mMenViewFullscreen.Name = "mMenViewFullscreen";
            _mMenViewFullscreen.ShortcutKeys = Keys.F11;
            _mMenViewFullscreen.Size = new System.Drawing.Size(228, 22);
            _mMenViewFullscreen.Text = Language.Fullscreen;
            _mMenViewFullscreen.Checked = Properties.App.Default.MainFormKiosk;
            _mMenViewFullscreen.Click += mMenViewFullscreen_Click;
        }


        public void ApplyLanguage()
        {
            Text = Language._View;
            _mMenViewAddConnectionPanel.Text = Language.AddConnectionPanel;
            _mMenViewConnectionPanels.Text = Language.ConnectionPanels;
            MMenViewErrorsAndInfos.Text = Language.Notifications;
            _mMenViewResetLayout.Text = Language.ResetLayout;
            MMenViewLockToolbars.Text = Language.LockToolbars;
            MMenViewQuickConnectToolbar.Text = Language.QuickConnectToolbar;
            MMenViewExtAppsToolbar.Text = Language.ExternalToolsToolbar;
            MMenViewMultiSshToolbar.Text = Language.MultiSshToolbar;
            _mMenViewFullscreen.Text = Language.Fullscreen;
        }

        #region View

        internal void mMenView_DropDownOpening(object sender, EventArgs e)
        {
            MMenViewErrorsAndInfos.Checked = !Windows.ErrorsForm.IsHidden;
            MMenViewLockToolbars.Checked = Settings.Default.LockToolbars;

            MMenViewExtAppsToolbar.Checked = TsExternalTools.Visible;
            MMenViewQuickConnectToolbar.Checked = TsQuickConnect.Visible;
            MMenViewMultiSshToolbar.Checked = TsMultiSsh.Visible;

            _mMenViewConnectionPanels.DropDownItems.Clear();

            for (var i = 0; i <= Runtime.WindowList.Count - 1; i++)
            {
                var tItem = new ToolStripMenuItem(Runtime.WindowList[i].Text, Runtime.WindowList[i].Icon.ToBitmap(), ConnectionPanelMenuItem_Click)
                { Tag = Runtime.WindowList[i] };
                _mMenViewConnectionPanels.DropDownItems.Add(tItem);
            }

            _mMenViewConnectionPanels.Visible = _mMenViewConnectionPanels.DropDownItems.Count > 0;
        }

        private void ConnectionPanelMenuItem_Click(object sender, EventArgs e)
        {
            ((BaseWindow)((ToolStripMenuItem)sender).Tag).Show(MainForm.pnlDock);
            ((BaseWindow)((ToolStripMenuItem)sender).Tag).Focus();
        }

        private void mMenViewErrorsAndInfos_Click(object sender, EventArgs e)
        {
            if (MMenViewErrorsAndInfos.Checked == false)
            {
                Windows.ErrorsForm.Show(MainForm.pnlDock);
                MMenViewErrorsAndInfos.Checked = true;
            }
            else
            {
                Windows.ErrorsForm.Hide();
                MMenViewErrorsAndInfos.Checked = false;
            }
        }

        private void mMenViewResetLayout_Click(object sender, EventArgs e)
        {
            var msgBoxResult = MessageBox.Show(Language.ConfirmResetLayout, string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (msgBoxResult == DialogResult.Yes)
            {
                MainForm.SetDefaultLayout();
            }
        }

        private void mMenViewLockToolbars_Click(object sender, EventArgs eventArgs)
        {
            if (MMenViewLockToolbars.Checked)
            {
                Settings.Default.LockToolbars = false;
                MMenViewLockToolbars.Checked = false;
            }
            else
            {
                Settings.Default.LockToolbars = true;
                MMenViewLockToolbars.Checked = true;
            }
        }

        private void mMenViewAddConnectionPanel_Click(object sender, EventArgs e)
        {
            _panelAdder.AddPanel();
        }

        private void mMenViewExtAppsToolbar_Click(object sender, EventArgs e)
        {
            if (MMenViewExtAppsToolbar.Checked)
            {
                Settings.Default.ViewMenuExternalTools = false;
                MMenViewExtAppsToolbar.Checked = false;
                TsExternalTools.Visible = false;
            }
            else
            {
                Settings.Default.ViewMenuExternalTools = true;
                MMenViewExtAppsToolbar.Checked = true;
                TsExternalTools.Visible = true;
            }
        }

        private void mMenViewQuickConnectToolbar_Click(object sender, EventArgs e)
        {
            if (MMenViewQuickConnectToolbar.Checked)
            {
                Settings.Default.ViewMenuQuickConnect = false;
                MMenViewQuickConnectToolbar.Checked = false;
                TsQuickConnect.Visible = false;
            }
            else
            {
                Settings.Default.ViewMenuQuickConnect = true;
                MMenViewQuickConnectToolbar.Checked = true;
                TsQuickConnect.Visible = true;
            }
        }

        private void mMenViewMultiSSHToolbar_Click(object sender, EventArgs e)
        {
            if (MMenViewMultiSshToolbar.Checked)
            {
                Settings.Default.ViewMenuMultiSSH = false;
                MMenViewMultiSshToolbar.Checked = false;
                TsMultiSsh.Visible = false;
            }
            else
            {
                Settings.Default.ViewMenuMultiSSH = true;
                MMenViewMultiSshToolbar.Checked = true;
                TsMultiSsh.Visible = true;
            }
        }

        private void mMenViewFullscreen_Click(object sender, EventArgs e)
        {
            FullscreenHandler.Value = !FullscreenHandler.Value;
            _mMenViewFullscreen.Checked = FullscreenHandler.Value;
        }

        private void mMenReconnectAll_Click(object sender, EventArgs e)
        {
            if (Runtime.WindowList == null || Runtime.WindowList.Count == 0) return;
            foreach (BaseWindow window in Runtime.WindowList)
            {
                if (!(window is ConnectionWindow connectionWindow))
                    return;

                connectionWindow.ReconnectAll(Runtime.ConnectionInitiator);
            }
        }

        #endregion
    }
}