using System.Threading;
using System.Windows.Forms;
using mRemoteNGTests.TestHelpers;
using NUnit.Framework;

namespace mRemoteNGTests.UI.Forms.OptionsPages
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class OptionsConnectionsPageTests : OptionsFormSetupAndTeardown
    {
        [Test]
        public void ConnectionsPageLinkExistsInListView()
        {
            var listViewTester = new ListViewTester("lstOptionPages", _optionsForm);
            Assert.That(listViewTester.Items[4].Text, Does.Match("Connections"));
        }

        [Test]
        public void ConnectionsIconShownInListView()
        {
            var listViewTester = new ListViewTester("lstOptionPages", _optionsForm);
            Assert.That(listViewTester.Items[4].ImageList, Is.Not.Null);
        }

        [Test]
        public void SelectingConnectionsPageLoadsSettings()
        {
            var listViewTester = new ListViewTester("lstOptionPages", _optionsForm);
            listViewTester.Select("Connections");
            var checkboxTester = _optionsForm.FindControl<CheckBox>("chkSingleClickOnConnectionOpensIt");
            Assert.That(checkboxTester.Text, Does.Match("Single click on connection"));
        }
    }
}