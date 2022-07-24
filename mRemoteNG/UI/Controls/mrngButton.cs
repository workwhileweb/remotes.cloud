using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using mRemoteNG.Themes;

namespace mRemoteNG.UI.Controls
{
    [ToolboxBitmap(typeof(Button))]
    //Extended button class, the button onPaint completely repaint the control
    public class MrngButton : Button
    {
        private ThemeManager _themeManager;

        /// <summary>
        /// Store the mouse state, required for coloring the component according to the mouse state
        /// </summary>
        public enum MouseState
        {
            Hover,
            Down,
            Out
        }

        public MrngButton()
        {
            ThemeManager.GetInstance().ThemeChanged += OnCreateControl;
        }

        public MouseState Mice { get; set; }

        /// <summary>
        /// Rewrite the function to allow for coloring the component depending on the mouse state
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            _themeManager = ThemeManager.GetInstance();
            if (_themeManager.ThemingActive)
            {
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
                    if (args.Button == MouseButtons.Left)
                    {
                        Mice = MouseState.Down;
                        Invalidate();
                    }
                };
                MouseUp += (sender, args) =>
                {
                    Mice = MouseState.Out;

                    Invalidate();
                };
                Invalidate();
            }
        }


        /// <summary>
        /// Repaint the componente, the elements considered are the clipping rectangle, text and an icon
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (!_themeManager.ActiveAndExtended)
            {
                base.OnPaint(e);
                return;
            }

            Color back;
            Color fore;
            Color border;
            if (Enabled)
            {
                switch (Mice)
                {
                    case MouseState.Hover:
                        back = _themeManager.ActiveTheme.ExtendedPalette.GetColor("Button_Hover_Background");
                        fore = _themeManager.ActiveTheme.ExtendedPalette.GetColor("Button_Hover_Foreground");
                        border = _themeManager.ActiveTheme.ExtendedPalette.GetColor("Button_Hover_Border");
                        break;
                    case MouseState.Down:
                        back = _themeManager.ActiveTheme.ExtendedPalette.GetColor("Button_Pressed_Background");
                        fore = _themeManager.ActiveTheme.ExtendedPalette.GetColor("Button_Pressed_Foreground");
                        border = _themeManager.ActiveTheme.ExtendedPalette.GetColor("Button_Pressed_Border");
                        break;
                    default:
                        back = _themeManager.ActiveTheme.ExtendedPalette.GetColor("Button_Background");
                        fore = _themeManager.ActiveTheme.ExtendedPalette.GetColor("Button_Foreground");
                        border = _themeManager.ActiveTheme.ExtendedPalette.GetColor("Button_Border");
                        break;
                }
            }
            else
            {
                back = _themeManager.ActiveTheme.ExtendedPalette.GetColor("Button_Disabled_Background");
                fore = _themeManager.ActiveTheme.ExtendedPalette.GetColor("Button_Disabled_Foreground");
                border = _themeManager.ActiveTheme.ExtendedPalette.GetColor("Button_Disabled_Border");
            }


            e.Graphics.FillRectangle(new SolidBrush(back), e.ClipRectangle);
            e.Graphics.DrawRectangle(new Pen(border, 1), 0, 0, Width - 1, Height - 1);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            //Warning. the app doesnt use many images in buttons so this positions are kinda tailored just for the used by the app
            //not by general usage of iamges in buttons
            if (Image != null)
            {
                var stringSize = e.Graphics.MeasureString(Text, Font);

                e.Graphics.DrawImageUnscaled(Image, Width / 2 - (int)stringSize.Width / 2 - Image.Width,
                                             Height / 2 - Image.Height / 2);
            }

            TextRenderer.DrawText(e.Graphics, Text, Font, ClientRectangle, fore,
                                  TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // NGButton
            // 
            Font = new Font("Segoe UI", 8.25F, FontStyle.Regular,
                                                GraphicsUnit.Point, ((byte)(0)));
            ResumeLayout(false);
        }
    }
}