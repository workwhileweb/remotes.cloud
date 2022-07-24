using System.Drawing;
using System.Windows.Forms;
using mRemoteNG.Themes;

namespace mRemoteNG.UI.Controls
{
    //Extended ComboBox class, the NGComboBox onPaint completely repaint the control as does the item painting
    //warning: THe DropDown style rendering is glitchy in this control, only use DropDownList or correct the rendering method
    internal class MrngComboBox : ComboBox
    {
        private ThemeManager _themeManager;

        public enum MouseState
        {
            Hover,
            Down,
            Out
        }

        public MouseState Mice { get; set; }

        public MrngComboBox()
        {
            ThemeManager.GetInstance().ThemeChanged += OnCreateControl;
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            _themeManager = ThemeManager.GetInstance();
            if (!_themeManager.ActiveAndExtended) return;
            BackColor = _themeManager.ActiveTheme.ExtendedPalette.GetColor("ComboBox_Background");
            ForeColor = _themeManager.ActiveTheme.ExtendedPalette.GetColor("ComboBox_Foreground");
            DrawMode = DrawMode.OwnerDrawFixed;
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint, true);
            DrawItem += NG_DrawItem;
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

        private void NG_DrawItem(object sender, DrawItemEventArgs e)
        {
            var index = e.Index >= 0 ? e.Index : 0;
            Brush itemBrush = new SolidBrush(_themeManager.ActiveTheme.ExtendedPalette.GetColor("ComboBox_Foreground"));

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                itemBrush = new SolidBrush(
                                           _themeManager
                                               .ActiveTheme.ExtendedPalette.GetColor("List_Item_Selected_Foreground"));
                e.Graphics.FillRectangle(
                                         new SolidBrush(_themeManager
                                                        .ActiveTheme.ExtendedPalette
                                                        .GetColor("List_Item_Selected_Background")),
                                         e.Bounds);
            }
            else
                e.Graphics.FillRectangle(
                                         new SolidBrush(_themeManager
                                                        .ActiveTheme.ExtendedPalette.GetColor("ComboBox_Background")),
                                         e.Bounds);

            if (Items.Count > 0)
            {
                if (string.IsNullOrEmpty(DisplayMember))
                    e.Graphics.DrawString(Items[index].ToString(), e.Font, itemBrush, e.Bounds,
                                          StringFormat.GenericDefault);
                else
                {
                    if (Items[index].GetType().GetProperty(DisplayMember) != null)
                    {
                        e.Graphics.DrawString(
                                              Items[index]
                                                  .GetType().GetProperty(DisplayMember)?.GetValue(Items[index], null)
                                                  .ToString(),
                                              e.Font, itemBrush, e.Bounds, StringFormat.GenericDefault);
                    }
                }
            }

            e.DrawFocusRectangle();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (!_themeManager.ActiveAndExtended)
            {
                base.OnPaint(e);
                return;
            }

            //Colors
            var border = _themeManager.ActiveTheme.ExtendedPalette.GetColor("ComboBox_Border");
            var back = _themeManager.ActiveTheme.ExtendedPalette.GetColor("ComboBox_Background");
            var fore = _themeManager.ActiveTheme.ExtendedPalette.GetColor("ComboBox_Foreground");
            var buttBack = _themeManager.ActiveTheme.ExtendedPalette.GetColor("ComboBox_Button_Background");
            var buttFore = _themeManager.ActiveTheme.ExtendedPalette.GetColor("ComboBox_Button_Foreground");

            if (Mice == MouseState.Hover)
            {
                border = _themeManager.ActiveTheme.ExtendedPalette.GetColor("ComboBox_MouseOver_Border");
                buttBack = _themeManager.ActiveTheme.ExtendedPalette.GetColor("ComboBox_Button_MouseOver_Background");
                buttFore = _themeManager.ActiveTheme.ExtendedPalette.GetColor("ComboBox_Button_MouseOver_Foreground");
            }

            if (DroppedDown)
            {
                border = _themeManager.ActiveTheme.ExtendedPalette.GetColor("ComboBox_MouseOver_Border");
                buttBack = _themeManager.ActiveTheme.ExtendedPalette.GetColor("ComboBox_Button_Pressed_Background");
                buttFore = _themeManager.ActiveTheme.ExtendedPalette.GetColor("ComboBox_Button_Pressed_Foreground");
            }


            e.Graphics.Clear(back);

            //Border
            using (var p = new Pen(border))
            {
                var boxRect = new Rectangle(0, 0, Width - 1, Height - 1);
                e.Graphics.DrawRectangle(p, boxRect);
            }

            //Button
            using (var b = new SolidBrush(buttBack))
            {
                e.Graphics.FillRectangle(b, Width - 18, 2, 16, Height - 4);
            }

            //Arrow
            e.Graphics.DrawString("\u25BC", Font, new SolidBrush(buttFore), Width - 17, Height / 2 - 5);

            //Text
            var textRect = new Rectangle(2, 2, Width - 20, Height - 4);
            TextRenderer.DrawText(e.Graphics, Text, Font, textRect, fore, back,
                                  TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // NGComboBox
            // 
            Font = new Font("Segoe UI", 8.25F, FontStyle.Regular,
                                                GraphicsUnit.Point, ((byte)(0)));
            ResumeLayout(false);
        }
    }
}