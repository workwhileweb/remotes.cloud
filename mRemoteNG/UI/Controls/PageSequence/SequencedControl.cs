using System;
using System.Windows.Forms;
using mRemoteNG.Themes;

namespace mRemoteNG.UI.Controls.PageSequence
{
    public class SequencedControl : UserControl, ISequenceChangingNotifier
    {
        public event EventHandler Next;
        public event EventHandler Previous;
        public event SequencedPageReplcementRequestHandler PageReplacementRequested;

        public SequencedControl()
        {
            ThemeManager.GetInstance().ThemeChanged += ApplyTheme;
            InitializeComponent();
        }

        protected virtual void RaiseNextPageEvent()
        {
            Next?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void ApplyTheme()
        {
            if (!ThemeManager.GetInstance().ActiveAndExtended) return;
            BackColor = ThemeManager.GetInstance().ActiveTheme.ExtendedPalette.GetColor("Dialog_Background");
            ForeColor = ThemeManager.GetInstance().ActiveTheme.ExtendedPalette.GetColor("Dialog_Foreground");
        }

        protected virtual void RaisePreviousPageEvent()
        {
            Previous?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void RaisePageReplacementEvent(SequencedControl control, RelativePagePosition pagetoReplace)
        {
            PageReplacementRequested?.Invoke(this, new SequencedPageReplcementRequestArgs(control, pagetoReplace));
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // SequencedControl
            // 
            Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular,
                                                System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Name = "SequencedControl";
            ResumeLayout(false);
        }
    }
}