using mRemoteNG.Themes;
using System.Drawing;
using System.Windows.Forms;

namespace mRemoteNG.UI.Controls
{
    // Repaint of a ProgressBar on a flat style
    internal class MrngProgressBar : ProgressBar
    {
        private ThemeManager _themeManager;


        public MrngProgressBar()
        {
            ThemeManager.GetInstance().ThemeChanged += OnCreateControl;
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            _themeManager = ThemeManager.GetInstance();
            if (!_themeManager.ThemingActive) return;
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (!_themeManager.ActiveAndExtended)
            {
                base.OnPaint(e);
                return;
            }

            var progressFill = _themeManager.ActiveTheme.ExtendedPalette.GetColor("ProgressBar_Fill");
            var back = _themeManager.ActiveTheme.ExtendedPalette.GetColor("ProgressBar_Background");
            var doneProgress = (int)(e.ClipRectangle.Width * ((double)Value / Maximum));
            e.Graphics.FillRectangle(new SolidBrush(progressFill), 0, 0, doneProgress, e.ClipRectangle.Height);
            e.Graphics.FillRectangle(new SolidBrush(back), doneProgress, 0, e.ClipRectangle.Width,
                                     e.ClipRectangle.Height);
        }
    }
}