using System.Threading;
using System.Windows.Forms;
using mRemoteNGTests.TestHelpers;
using NUnit.Framework;

namespace mRemoteNGTests.UI.Forms.OptionsPages
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class OptionsTabsPanelPageTests : OptionsFormSetupAndTeardown
    {
        [Test]
        public void TabsPanelPageLinkExistsInListView()
        {
            var listViewTester = new ListViewTester("lstOptionPages", OptionsForm);
            Assert.That(listViewTester.Items[2].Text, Does.Match("Tabs & Panels"));
        }

        [Test]
        public void TabsPanelIconShownInListView()
        {
            var listViewTester = new ListViewTester("lstOptionPages", OptionsForm);
            Assert.That(listViewTester.Items[2].ImageList, Is.Not.Null);
        }

        [Test]
        public void SelectingTabsPanelPageLoadsSettings()
        {
            var listViewTester = new ListViewTester("lstOptionPages", OptionsForm);
            listViewTester.Select("Tabs & Panels");
            var checkboxTester = OptionsForm.FindControl<CheckBox>("chkAlwaysShowPanelTabs");
            Assert.That(checkboxTester.Text, Does.Match("Always show panel tabs"));
        }
    }
}