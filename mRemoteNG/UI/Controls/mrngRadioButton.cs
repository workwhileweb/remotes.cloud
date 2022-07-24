using mRemoteNG.Themes;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace mRemoteNG.UI.Controls
{
    // total replace of RadioButton to avoid disabled state inconsistency on the themes
    // and glyph color inconsistency
    class MrngRadioButton : RadioButton
    {
        private ThemeManager _themeManager;
        private readonly Rectangle _circle;
        private readonly Rectangle _circleSmall;
        private readonly int _textXCoord;

        // Constructor
        public MrngRadioButton()
        {
            var display = new DisplayProperties();

            _circleSmall = new Rectangle(display.ScaleWidth(4), display.ScaleHeight(4), display.ScaleWidth(6),
                                         display.ScaleHeight(6));
            _circle = new Rectangle(display.ScaleWidth(1), display.ScaleHeight(1), display.ScaleWidth(12),
                                    display.ScaleHeight(12));
            _textXCoord = display.ScaleWidth(16);
            ThemeManager.GetInstance().ThemeChanged += OnCreateControl;
        }


        private enum MouseState
        {
            Hover,
            Down,
            Out
        }

        private MouseState Mice { get; set; }


        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            _themeManager = ThemeManager.GetInstance();
            if (!_themeManager.ThemingActive) return;
            // Allows for Overlaying
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;
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


        //This class is painted with the checkbox colors, the glyph color is used for the radio inside
        protected override void OnPaint(PaintEventArgs e)
        {
            if (!_themeManager.ActiveAndExtended)
            {
                base.OnPaint(e);
                return;
            }

            // Init
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var fore = _themeManager.ActiveTheme.ExtendedPalette.GetColor("CheckBox_Text");
            var outline = _themeManager.ActiveTheme.ExtendedPalette.GetColor("CheckBox_Border");
            var centerBack = _themeManager.ActiveTheme.ExtendedPalette.GetColor("CheckBox_Background");
            Color center;

            // Overlay Graphic
            e.Graphics.Clear(Parent.BackColor);
            if (Enabled)
            {
                if (Checked)
                {
                    center = _themeManager.ActiveTheme.ExtendedPalette.GetColor("CheckBox_Glyph");
                }
                else
                {
                    center = Color.Transparent;
                    if (Mice == MouseState.Hover)
                    {
                        outline = _themeManager.ActiveTheme.ExtendedPalette.GetColor("CheckBox_Border_Hover");
                    }
                }
            }
            else
            {
                center = Color.Transparent;
                fore = _themeManager.ActiveTheme.ExtendedPalette.GetColor("CheckBox_Text_Disabled");
            }

            var textRect = new Rectangle(_textXCoord, Padding.Top, Width - 16, Height);
            TextRenderer.DrawText(e.Graphics, Text, Font, textRect, fore, Parent.BackColor,
                                  TextFormatFlags.PathEllipsis);

            g.FillEllipse(new SolidBrush(centerBack), _circle);
            g.FillEllipse(new SolidBrush(center), _circleSmall);
            g.DrawEllipse(new Pen(outline), _circle);
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // NGRadioButton
            // 
            Font = new Font("Segoe UI", 8.25F, FontStyle.Regular,
                                                GraphicsUnit.Point, ((byte)(0)));
            ResumeLayout(false);
        }
    }
}