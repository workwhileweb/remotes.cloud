using System.Windows.Forms;
using mRemoteNG.Themes;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.UI.Forms
{
    public sealed partial class FrmInputBox : Form
    {
        internal string ReturnValue;

        public FrmInputBox(string title, string promptText, string value)
        {
            InitializeComponent();

            Text = title;
            label.Text = promptText;
            textBox.Text = value;
            ApplyLanguage();
            ApplyTheme();
        }

        private void ApplyLanguage()
        {
            _Ok.Text = Language._Ok;
            buttonCancel.Text = Language._Cancel;
        }

        private void ApplyTheme()
        {
            var themeManager = ThemeManager.GetInstance();
            if (!themeManager.ActiveAndExtended) return;
            BackColor = themeManager.ActiveTheme.ExtendedPalette.GetColor("Dialog_Background");
            ForeColor = themeManager.ActiveTheme.ExtendedPalette.GetColor("Dialog_Foreground");
        }

        private void _Ok_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.OK;
            ReturnValue = textBox.Text;
            Close();
        }

        private void buttonCancel_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}