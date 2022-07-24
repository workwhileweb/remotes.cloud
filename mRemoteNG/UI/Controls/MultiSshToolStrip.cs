using System.ComponentModel;
using System.Windows.Forms;
using mRemoteNG.Themes;
using System;
using System.Collections;
using System.Linq;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.UI.Controls
{
    public partial class MultiSshToolStrip : ToolStrip
    {
        private IContainer components;
        private ToolStripLabel _lblMultiSsh;
        private ToolStripTextBox _txtMultiSsh;
        private int _previousCommandIndex = 0;
        private readonly ArrayList _processHandlers = new();
        private readonly ArrayList _quickConnectConnections = new();
        private readonly ArrayList _previousCommands = new();
        private readonly ThemeManager _themeManager;

        private int CommandHistoryLength { get; set; } = 100;

        public MultiSshToolStrip()
        {
            InitializeComponent();
            _themeManager = ThemeManager.GetInstance();
            _themeManager.ThemeChanged += ApplyTheme;
            ApplyTheme();
        }

        private void ApplyTheme()
        {
            if (!_themeManager.ActiveAndExtended) return;
            _txtMultiSsh.BackColor = _themeManager.ActiveTheme.ExtendedPalette.GetColor("TextBox_Background");
            _txtMultiSsh.ForeColor = _themeManager.ActiveTheme.ExtendedPalette.GetColor("TextBox_Foreground");
        }

        private ArrayList ProcessOpenConnections(ConnectionInfo connection)
        {
            var handlers = new ArrayList();

            foreach (ProtocolBase @base in connection.OpenConnections)
            {
                if (@base.GetType().IsSubclassOf(typeof(PuttyBase)))
                {
                    handlers.Add((PuttyBase)@base);
                }
            }

            return handlers;
        }

        private void SendAllKeystrokes(int keyType, int keyData)
        {
            if (_processHandlers.Count == 0) return;

            foreach (PuttyBase proc in _processHandlers)
            {
                NativeMethods.PostMessage(proc.PuttyHandle, keyType, new IntPtr(keyData), new IntPtr(0));
            }
        }

        #region Key Event Handler

        private void RefreshActiveConnections(object sender, EventArgs e)
        {
            _processHandlers.Clear();
            foreach (ConnectionInfo connection in _quickConnectConnections)
            {
                _processHandlers.AddRange(ProcessOpenConnections(connection));
            }

            var connectionTreeConnections = Runtime.ConnectionsService.ConnectionTreeModel.GetRecursiveChildList()
                                                   .Where(item => item.OpenConnections.Count > 0);

            foreach (var connection in connectionTreeConnections)
            {
                _processHandlers.AddRange(ProcessOpenConnections(connection));
            }
        }

        private void ProcessKeyPress(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
            {
                e.SuppressKeyPress = true;
                try
                {
                    switch (e.KeyCode)
                    {
                        case Keys.Up when _previousCommandIndex - 1 >= 0:
                            _previousCommandIndex -= 1;
                            break;
                        case Keys.Down when _previousCommandIndex + 1 < _previousCommands.Count:
                            _previousCommandIndex += 1;
                            break;
                        default:
                            return;
                    }
                }
                catch { }

                _txtMultiSsh.Text = _previousCommands[_previousCommandIndex].ToString();
                _txtMultiSsh.SelectAll();
            }

            if (e.Control && e.KeyCode != Keys.V && e.Alt == false)
            {
                SendAllKeystrokes(NativeMethods.WM_KEYDOWN, e.KeyValue);
            }

            if (e.KeyCode == Keys.Enter)
            {
                foreach (var chr1 in _txtMultiSsh.Text)
                {
                    SendAllKeystrokes(NativeMethods.WM_CHAR, Convert.ToByte(chr1));
                }

                SendAllKeystrokes(NativeMethods.WM_KEYDOWN, 13); // Enter = char13
            }
        }

        private void ProcessKeyRelease(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            if (string.IsNullOrWhiteSpace(_txtMultiSsh.Text)) return;

            _previousCommands.Add(_txtMultiSsh.Text.Trim());

            if (_previousCommands.Count >= CommandHistoryLength) _previousCommands.RemoveAt(0);

            _previousCommandIndex = _previousCommands.Count - 1;
            _txtMultiSsh.Clear();
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if(components != null)
                    components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this._lblMultiSsh = new ToolStripLabel();
            this._txtMultiSsh = new ToolStripTextBox();
            this.SuspendLayout();
            // 
            // lblMultiSSH
            // 
            this._lblMultiSsh.Name = "_lblMultiSsh";
            this._lblMultiSsh.Size = new System.Drawing.Size(77, 22);
            this._lblMultiSsh.Text = Language.MultiSsh;
            // 
            // txtMultiSsh
            // 
            this._txtMultiSsh.Name = "_txtMultiSsh";
            this._txtMultiSsh.Size = new System.Drawing.Size(new DisplayProperties().ScaleWidth(300), 25);
            this._txtMultiSsh.ToolTipText = Language.MultiSshToolTip;
            this._txtMultiSsh.Enter += RefreshActiveConnections;
            this._txtMultiSsh.KeyDown += ProcessKeyPress;
            this._txtMultiSsh.KeyUp += ProcessKeyRelease;

            this.Items.AddRange(new ToolStripItem[]
            {
                _lblMultiSsh,
                _txtMultiSsh
            });
            this.ResumeLayout(false);
        }

        #endregion

    }
}