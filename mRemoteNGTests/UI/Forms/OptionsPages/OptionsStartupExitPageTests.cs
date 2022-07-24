using System.Threading;
using System.Windows.Forms;
using mRemoteNGTests.TestHelpers;
using NUnit.Framework;

namespace mRemoteNGTests.UI.Forms.OptionsPages
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class OptionsStartupExitPageTests : OptionsFormSetupAndTeardown
    {
        [Test]
        public void StartupExitPageLinkExistsInListView()
        {
            var listViewTester = new ListViewTester("lstOptionPages", _optionsForm);
            Assert.That(listViewTester.Items[0].Text, Does.Match("Startup/Exit"));
        }

        [Test]
        public void IconShownInListView()
        {
            var listViewTester = new ListViewTester("lstOptionPages", _optionsForm);
            Assert.That(listViewTester.Items[0].ImageList, Is.Not.Null);
        }

        [Test]
        public void SelectingStartupExitPageLoadsSettings()
        {
            var listViewTester = new ListViewTester("lstOptionPages", _optionsForm);
            listViewTester.Select("Startup/Exit");
            var checkboxTester = _optionsForm.FindControl<CheckBox>("chkSaveConsOnExit");
            Assert.That(checkboxTester.Text, Does.Match("Save connections"));
        }
    }
}