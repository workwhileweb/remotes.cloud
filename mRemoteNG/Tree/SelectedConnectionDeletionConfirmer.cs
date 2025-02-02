﻿using mRemoteNG.Connection;
using mRemoteNG.Container;
using System;
using System.Windows.Forms;
using mRemoteNG.Resources.Language;


namespace mRemoteNG.Tree
{
    public class SelectedConnectionDeletionConfirmer : IConfirm<ConnectionInfo>
    {
        private readonly Func<string, DialogResult> _confirmationFunc;

        public SelectedConnectionDeletionConfirmer(Func<string, DialogResult> confirmationFunc)
        {
            _confirmationFunc = confirmationFunc;
        }

        public bool Confirm(ConnectionInfo deletionTarget)
        {
            return deletionTarget switch
            {
                null => false,
                ContainerInfo deletionTargetAsContainer => deletionTargetAsContainer.HasChildren()
                    ? UserConfirmsNonEmptyFolderDeletion(deletionTargetAsContainer)
                    : UserConfirmsEmptyFolderDeletion(deletionTargetAsContainer),
                _ => UserConfirmsConnectionDeletion(deletionTarget)
            };
        }

        private bool UserConfirmsEmptyFolderDeletion(AbstractConnectionRecord deletionTarget)
        {
            var messagePrompt = string.Format(Language.ConfirmDeleteNodeFolder, deletionTarget.Name);
            return PromptUser(messagePrompt);
        }

        private bool UserConfirmsNonEmptyFolderDeletion(AbstractConnectionRecord deletionTarget)
        {
            var messagePrompt = string.Format(Language.ConfirmDeleteNodeFolderNotEmpty, deletionTarget.Name);
            return PromptUser(messagePrompt);
        }

        private bool UserConfirmsConnectionDeletion(AbstractConnectionRecord deletionTarget)
        {
            var messagePrompt = string.Format(Language.ConfirmDeleteNodeConnection, deletionTarget.Name);
            return PromptUser(messagePrompt);
        }

        private bool PromptUser(string promptMessage)
        {
            var msgBoxResponse = _confirmationFunc(promptMessage);
            return msgBoxResponse == DialogResult.Yes;
        }
    }
}