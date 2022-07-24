using System.Threading;
using System.Windows.Forms;
using mRemoteNGTests.TestHelpers;
using NUnit.Framework;

namespace mRemoteNGTests.UI.Forms.OptionsPages
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class OptionsAdvancedPageTests : OptionsFormSetupAndTeardown
    {
        [Test]
        public void AdvancedPageLinkExistsInListView()
        {
            var listViewTester = new ListViewTester("lstOptionPages", OptionsForm);
            Assert.That(listViewTester.Items[10].Text, Does.Match("Advanced"));
        }

        [Test]
        public void AdvancedIconShownInListView()
        {
            var listViewTester = new ListViewTester("lstOptionPages", OptionsForm);
            Assert.That(listViewTester.Items[10].ImageList, Is.Not.Null);
        }

        [Test]
        public void SelectingAdvancedPageLoadsSettings()
        {
            var listViewTester = new ListViewTester("lstOptionPages", OptionsForm);
            listViewTester.Select("Advanced");
            
            var checkboxTester = OptionsForm.FindControl<CheckBox>("chkAutomaticReconnect");
            Assert.That(checkboxTester.Text, Is.EqualTo("Automatically try to reconnect when disconnected from server (RDP && ICA only)"));
        }
    }
}