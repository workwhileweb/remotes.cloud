using System;
using mRemoteNG.UI;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.Window;

namespace mRemoteNG.App
{
    public static class Windows
    {
        private static ActiveDirectoryImportWindow _adimportForm;
        private static ExternalToolsWindow _externalappsForm;
        private static PortScanWindow _portscanForm;
        private static UltraVncWindow _ultravncscForm;
        private static ConnectionTreeWindow _treeForm;

        internal static ConnectionTreeWindow TreeForm
        {
            get => _treeForm ?? (_treeForm = new ConnectionTreeWindow());
            set => _treeForm = value;
        }

        internal static ConfigWindow ConfigForm { get; set; } = new();
        internal static ErrorAndInfoWindow ErrorsForm { get; set; } = new();
        private static UpdateWindow UpdateForm { get; set; } = new();
        internal static SshTransferWindow SshtransferForm { get; private set; } = new();


        public static void Show(WindowType windowType)
        {
            try
            {
                var dockPanel = FrmMain.Default.pnlDock;
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (windowType)
                {
                    case WindowType.ActiveDirectoryImport:
                        if (_adimportForm == null || _adimportForm.IsDisposed)
                            _adimportForm = new ActiveDirectoryImportWindow();
                        _adimportForm.Show(dockPanel);
                        break;
                    case WindowType.Options:
                        using (var optionsForm = new FrmOptions())
                        {
                            optionsForm.ShowDialog(dockPanel);
                        }

                        break;
                    case WindowType.SshTransfer:
                        if (SshtransferForm == null || SshtransferForm.IsDisposed)
                            SshtransferForm = new SshTransferWindow();
                        SshtransferForm.Show(dockPanel);
                        break;
                    case WindowType.Update:
                        if (UpdateForm == null || UpdateForm.IsDisposed)
                            UpdateForm = new UpdateWindow();
                        UpdateForm.Show(dockPanel);
                        break;
                    case WindowType.ExternalApps:
                        if (_externalappsForm == null || _externalappsForm.IsDisposed)
                            _externalappsForm = new ExternalToolsWindow();
                        _externalappsForm.Show(dockPanel);
                        break;
                    case WindowType.PortScan:
                        _portscanForm = new PortScanWindow();
                        _portscanForm.Show(dockPanel);
                        break;
                    case WindowType.UltraVncsc:
                        if (_ultravncscForm == null || _ultravncscForm.IsDisposed)
                            _ultravncscForm = new UltraVncWindow();
                        _ultravncscForm.Show(dockPanel);
                        break;
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("App.Runtime.Windows.Show() failed.", ex);
            }
        }
    }
}