namespace ExternalConnectors.AWS
{
    public partial class AwsConnectionForm : Form
    {
        public AwsConnectionForm()
        {
            InitializeComponent();

        }

        private void AWSConnectionForm_Activated(object sender, EventArgs e)
        {
            tbAccesKeyID.Focus();
        }
    }
}
