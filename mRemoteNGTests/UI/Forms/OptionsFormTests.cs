using NUnit.Framework;
using System.Threading;
using System.Windows.Forms;
using mRemoteNGTests.TestHelpers;

namespace mRemoteNGTests.UI.Forms
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class OptionsFormTests : OptionsFormSetupAndTeardown
    {
        [Test]
        public void ClickingCloseButtonClosesTheForm()
        {
            var eventFired = false;
            OptionsForm.FormClosed += (o, e) => eventFired = true;
            var cancelButton = OptionsForm.FindControl<Button>("btnCancel");
            cancelButton.PerformClick();
            Assert.That(eventFired, Is.True);
        }

        [Test]
        public void ClickingOkButtonSetsDialogResult()
        {
            var cancelButton = OptionsForm.FindControl<Button>("btnOK");
            cancelButton.PerformClick();
            Assert.That(OptionsForm.DialogResult, Is.EqualTo(DialogResult.OK));
        }

        [Test]
        public void ListViewContainsOptionsPages()
        {
            var listViewTester = new ListViewTester("lstOptionPages", OptionsForm);
            Assert.That(listViewTester.Items.Count, Is.EqualTo(12));
        }
    }
}