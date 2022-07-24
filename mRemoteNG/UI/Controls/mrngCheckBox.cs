using System.Drawing;
using System.Windows.Forms;
using mRemoteNG.Themes;

namespace mRemoteNG.UI.Controls
{
    //Extended CheckBox class, the NGCheckBox onPaint completely repaint the control

    //
    // If this causes design issues in the future, may want to think about migrating to
    // CheckBoxRenderer:
    // https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.checkboxrenderer?view=netframework-4.6
    //
    public class MrngCheckBox : CheckBox
    {
        private ThemeManager _themeManager;
        private readonly Size _checkboxSize;
        private readonly int _checkboxYCoord;
        private readonly int _textXCoord;

        public MrngCheckBox()
        {
            InitializeComponent();
            ThemeManager.GetInstance().ThemeChanged += OnCreateControl;
            var display = new DisplayProperties();
            _checkboxSize = new Size(display.ScaleWidth(11), display.ScaleHeight(11));
            _checkboxYCoord = (display.ScaleHeight(Height) - _checkboxSize.Height) / 2 - display.ScaleHeight(5);
            _textXCoord = _checkboxSize.Width + display.ScaleWidth(2);
        }

        public enum MouseState
        {
            Hover,
            Down,
            Out
        }

        public MouseState Mice { get; set; }


        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            _themeManager = ThemeManager.GetInstance();
            if (!_themeManager.ThemingActive) return;
            Mice = MouseState.Out;
            MouseEnter += (sender, args) =>
            {
                Mice = MouseState.Hover;
                Invalidate();
            };
            MouseLeave += (sender, args) =>
            {
                Mice = MouseState.Out;
                Invalidate();
            };
            MouseDown += (sender, args) =>
            {
                if (args.Button != MouseButtons.Left) return;
                Mice = MouseState.Down;
                Invalidate();
            };
            MouseUp += (sender, args) =>
            {
                Mice = MouseState.Out;

                Invalidate();
            };

            Invalidate();
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            if (!_themeManager.ActiveAndExtended)
            {
                base.OnPaint(e);
                return;
            }

            //Get the colors
            Color fore;
            Color glyph;
            Color checkBorder;

            var back = _themeManager.ActiveTheme.ExtendedPalette.GetColor("CheckBox_Background");
            if (Enabled)
            {
                glyph = _themeManager.ActiveTheme.ExtendedPalette.GetColor("CheckBox_Glyph");
                fore = _themeManager.ActiveTheme.ExtendedPalette.GetColor("CheckBox_Text");
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (Mice)
                {
                    case MouseState.Hover:
                        checkBorder = _themeManager.ActiveTheme.ExtendedPalette.GetColor("CheckBox_Border_Hover");
                        break;
                    case MouseState.Down:
                        checkBorder = _themeManager.ActiveTheme.ExtendedPalette.GetColor("CheckBox_Border_Pressed");
                        break;
                    default:
                        checkBorder = _themeManager.ActiveTheme.ExtendedPalette.GetColor("CheckBox_Border");
                        break;
                }
            }
            else
            {
                fore = _themeManager.ActiveTheme.ExtendedPalette.GetColor("CheckBox_Text_Disabled");
                glyph = _themeManager.ActiveTheme.ExtendedPalette.GetColor("CheckBox_Glyph_Disabled");
                checkBorder = _themeManager.ActiveTheme.ExtendedPalette.GetColor("CheckBox_Border_Disabled");
            }

            e.Graphics.Clear(Parent.BackColor);

            using (var p = new Pen(checkBorder))
            {
                var boxRect = new Rectangle(0, _checkboxYCoord, _checkboxSize.Width, _checkboxSize.Height);
                e.Graphics.FillRectangle(new SolidBrush(back), boxRect);
                e.Graphics.DrawRectangle(p, boxRect);
            }

            if (Checked)
            {
                // | \uE001 | &#xE001; |  |  is the tick/check mark and it exists in Segoe UI Symbol at least...
                e.Graphics.DrawString("\uE001", new Font("Segoe UI Symbol", 7.75f), new SolidBrush(glyph), -4, 0);
            }

            var textRect = new Rectangle(_textXCoord, 0, Width - 16, Height);
            TextRenderer.DrawText(e.Graphics, Text, Font, textRect, fore, Parent.BackColor,
                                  TextFormatFlags.PathEllipsis);
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // NGCheckBox
            // 
            Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ResumeLayout(false);
        }
    }
}