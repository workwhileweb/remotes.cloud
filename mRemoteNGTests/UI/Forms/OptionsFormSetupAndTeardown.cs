using NUnit.Framework;
using mRemoteNG.UI.Forms;

namespace mRemoteNGTests.UI.Forms
{
    public class OptionsFormSetupAndTeardown
    {
        protected FrmOptions OptionsForm;

        [OneTimeSetUp]
        public void OnetimeSetup()
        {
        }

        [SetUp]
        public void Setup()
        {
            OptionsForm = new FrmOptions();
            OptionsForm.Show();
        }

        [TearDown]
        public void Teardown()
        {
            OptionsForm.Dispose();
            while (OptionsForm.Disposing) ;
            OptionsForm = null;
        }
    }
}