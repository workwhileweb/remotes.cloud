﻿using System;
using System.Drawing;
using System.Windows.Forms;
using mRemoteNG.Themes;

// ReSharper disable LocalizableElement

namespace mRemoteNG.UI.Controls
{
    //Repaint of the NumericUpDown, the composite control buttons are replaced because the
    //original ones cannot be themed due to protected inheritance
    internal class MrngNumericUpDown : NumericUpDown
    {
        private readonly ThemeManager _themeManager;
        private MrngButton _up;
        private MrngButton _down;

        public MrngNumericUpDown()
        {
            _themeManager = ThemeManager.GetInstance();
            ThemeManager.GetInstance().ThemeChanged += OnCreateControl;
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (!_themeManager.ActiveAndExtended) return;
            ForeColor = _themeManager.ActiveTheme.ExtendedPalette.GetColor("TextBox_Foreground");
            BackColor = _themeManager.ActiveTheme.ExtendedPalette.GetColor("TextBox_Background");
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);

            if (Controls.Count > 0)
            {
                for (var i = 0; i < Controls.Count; i++)
                {
                    //Remove those non-themable buttons
                    if (Controls[i].GetType().ToString().Equals("System.Windows.Forms.UpDownBase+UpDownButtons"))
                        Controls.Remove(Controls[i]);

                    /* This is a bit of a hack.
                     * But if we have the buttons that we created already, redraw/return and don't add any more...
                     *
                     * OptionsPages are an example where the control is potentially created twice:
                     * AddOptionsPagesToListView and then LstOptionPages_SelectedIndexChanged
                     */
                    if (!(Controls[i] is MrngButton)) continue;
                    if (!Controls[i].Text.Equals("\u25B2") && !Controls[i].Text.Equals("\u25BC")) continue;
                    Invalidate();
                    return;
                }
            }

            //Add new themable buttons
            _up = new MrngButton
            {
                Text = "\u25B2",
                Font = new Font(Font.FontFamily, 5f)
            };
            _up.SetBounds(Controls.Owner.Width - 17, 2, 16, Controls.Owner.Height / 2 - 1);
            _up.Click += Up_Click;
            _down = new MrngButton
            {
                Text = "\u25BC",
                Font = new Font(Font.FontFamily, 5f)
            };
            _down.SetBounds(Controls.Owner.Width - 17, Controls.Owner.Height / 2 + 1, 16, Controls.Owner.Height / 2 - 1);
            _down.Click += Down_Click;
            Controls.Add(_up);
            Controls.Add(_down);
            Invalidate();
        }

        private void Down_Click(object sender, EventArgs e)
        {
            DownButton();
        }

        private void Up_Click(object sender, EventArgs e)
        {
            UpButton();
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            if (_themeManager.ActiveAndExtended)
            {
                if (Enabled)
                {
                    ForeColor = _themeManager.ActiveTheme.ExtendedPalette.GetColor("TextBox_Foreground");
                    BackColor = _themeManager.ActiveTheme.ExtendedPalette.GetColor("TextBox_Background");
                }
                else
                {
                    BackColor = _themeManager.ActiveTheme.ExtendedPalette.GetColor("TextBox_Disabled_Background");
                }
            }

            base.OnEnabledChanged(e);
            Invalidate();
        }


        //Redrawing border
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (!_themeManager.ActiveAndExtended) return;
            //Fix Border
            if (BorderStyle != BorderStyle.None)
                e.Graphics.DrawRectangle(
                                         new Pen(_themeManager.ActiveTheme.ExtendedPalette.GetColor("TextBox_Border"),
                                                 1), 0, 0, Width - 1,
                                         Height - 1);
        }

        private void InitializeComponent()
        {
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            SuspendLayout();
            // 
            // NGNumericUpDown
            // 
            Font = new Font("Segoe UI", 8.25F, FontStyle.Regular,
                                                GraphicsUnit.Point, ((byte)(0)));
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            ResumeLayout(false);
        }
    }
}