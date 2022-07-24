using System.Threading;
using System.Windows.Forms;
using mRemoteNGTests.TestHelpers;
using NUnit.Framework;

namespace mRemoteNGTests.UI.Forms.OptionsPages
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class OptionsSqlPageTests : OptionsFormSetupAndTeardown
    {
        [Test]
        public void SqlPageLinkExistsInListView()
        {
            var listViewTester = new ListViewTester("lstOptionPages", OptionsForm);
            Assert.That(listViewTester.Items[6].Text, Does.Match("SQL Server"));
        }

        [Test]
        public void SqlIconShownInListView()
        {
            var listViewTester = new ListViewTester("lstOptionPages", OptionsForm);
            Assert.That(listViewTester.Items[6].ImageList, Is.Not.Null);
        }

        [Test]
        public void SelectingSqlPageLoadsSettings()
        {
            var listViewTester = new ListViewTester("lstOptionPages", OptionsForm);
            listViewTester.Select("SQL Server");
            var checkboxTester = OptionsForm.FindControl<CheckBox>("chkUseSQLServer");
            Assert.That(checkboxTester.Text, Does.Match("Use SQL"));
        }
    }
}