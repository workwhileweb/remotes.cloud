using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using mRemoteNG.Tree;
using mRemoteNG.Resources.Language;


namespace mRemoteNG.Credential
{
    public class CredentialDeletionMsgBoxConfirmer : IConfirm<IEnumerable<ICredentialRecord>>
    {
        private readonly Func<string, string, MessageBoxButtons, MessageBoxIcon, DialogResult> _confirmationFunc;

        public CredentialDeletionMsgBoxConfirmer(
            Func<string, string, MessageBoxButtons, MessageBoxIcon, DialogResult> confirmationFunc)
        {
            _confirmationFunc = confirmationFunc ?? throw new ArgumentNullException(nameof(confirmationFunc));
        }

        public bool Confirm(IEnumerable<ICredentialRecord> confirmationTargets)
        {
            var targetsArray = confirmationTargets.ToArray();
            return targetsArray.Length switch
            {
                0 => false,
                > 1 => PromptUser($"Are you sure you want to delete these {targetsArray.Length} selected credentials?"),
                _ => PromptUser(string.Format(Language.ConfirmDeleteCredentialRecord, targetsArray.First().Title))
            };
        }

        private bool PromptUser(string promptMessage)
        {
            var msgBoxResponse = _confirmationFunc.Invoke(promptMessage, Application.ProductName,
                                                          MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return msgBoxResponse == DialogResult.Yes;
        }
    }
}