using System.Threading;
using System.Windows.Forms;
using mRemoteNGTests.TestHelpers;
using NUnit.Framework;

namespace mRemoteNGTests.UI.Forms.OptionsPages
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class OptionsAppearancePageTests : OptionsFormSetupAndTeardown
    {
        [Test]
        public void AppearancePageLinkExistsInListView()
        {
            var listViewTester = new ListViewTester("lstOptionPages", _optionsForm);
            Assert.That(listViewTester.Items[1].Text, Does.Match("Appearance"));
        }

        [Test]
        public void IconShownInListView()
        {
            var listViewTester = new ListViewTester("lstOptionPages", _optionsForm);
            Assert.That(listViewTester.Items[1].ImageList, Is.Not.Null);
        }

        [Test]
        public void SelectingAppearancePageLoadsSettings()
        {
            var listViewTester = new ListViewTester("lstOptionPages", _optionsForm);
            listViewTester.Select("Appearance");
            var checkboxTester = _optionsForm.FindControl<CheckBox>("chkShowSystemTrayIcon");
            Assert.That(checkboxTester.Text, Does.Match("show notification area icon"));
        }
    }
}