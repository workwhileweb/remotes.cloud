using System.Threading;
using System.Windows.Forms;
using mRemoteNGTests.TestHelpers;
using NUnit.Framework;

namespace mRemoteNGTests.UI.Forms.OptionsPages
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class OptionsUpdatesPageTests : OptionsFormSetupAndTeardown
    {
        [Test]
        public void UpdatesPageLinkExistsInListView()
        {
            var listViewTester = new ListViewTester("lstOptionPages", OptionsForm);
            Assert.That(listViewTester.Items[7].Text, Does.Match("Updates"));
        }

        [Test]
        public void UpdatesIconShownInListView()
        {
            var listViewTester = new ListViewTester("lstOptionPages", OptionsForm);
            Assert.That(listViewTester.Items[7].ImageList, Is.Not.Null);
        }

        [Test]
        public void SelectingUpdatesPageLoadsSettings()
        {
            var listViewTester = new ListViewTester("lstOptionPages", OptionsForm);
            listViewTester.Select("Updates");
            var checkboxTester = OptionsForm.FindControl<CheckBox>("chkCheckForUpdatesOnStartup");
            Assert.That(checkboxTester.Text, Does.Match("Check for updates"));
        }
    }
}