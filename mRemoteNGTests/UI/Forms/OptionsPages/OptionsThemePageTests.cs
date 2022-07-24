using System.Threading;
using System.Windows.Forms;
using mRemoteNGTests.TestHelpers;
using NUnit.Framework;

namespace mRemoteNGTests.UI.Forms.OptionsPages
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class OptionsThemePageTests : OptionsFormSetupAndTeardown
    {
        [Test]
        public void ThemePageLinkExistsInListView()
        {
            var listViewTester = new ListViewTester("lstOptionPages", OptionsForm);
            Assert.That(listViewTester.Items[8].Text, Does.Match("Theme"));
        }

        [Test]
        public void ThemeIconShownInListView()
        {
            var listViewTester = new ListViewTester("lstOptionPages", OptionsForm);
            Assert.That(listViewTester.Items[8].ImageList, Is.Not.Null);
        }

        [Test]
        public void SelectingThemePageLoadsSettings()
        {
            var listViewTester = new ListViewTester("lstOptionPages", OptionsForm);
            listViewTester.Select("Theme");
            var buttonTester = OptionsForm.FindControl<Button>("btnThemeNew");
            Assert.That(buttonTester.Text, Does.Match("New"));
        }
    }
}