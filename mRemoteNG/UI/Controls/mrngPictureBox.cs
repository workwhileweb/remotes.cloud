using System.ComponentModel;
using System.Windows.Forms;
using mRemoteNG.Themes;

namespace mRemoteNG.UI.Controls
{
    public partial class MrngPictureBox : PictureBox
    {
        private ThemeManager _themeManager;

        public MrngPictureBox()
        {
            ThemeManager.GetInstance().ThemeChanged += OnCreateControl;
        }

        public MrngPictureBox(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            _themeManager = ThemeManager.GetInstance();
            if (!_themeManager.ActiveAndExtended) return;
            ForeColor = _themeManager.ActiveTheme.ExtendedPalette.GetColor("TextBox_Foreground");
            BackColor = _themeManager.ActiveTheme.ExtendedPalette.GetColor("TextBox_Background");
            Invalidate();
        }
    }
}