using System;

namespace mRemoteNG.Credential
{
    public class CredentialChangedEventArgs : EventArgs
    {
        public ICredentialRecord CredentialRecord { get; }
        public ICredentialRepository Repository { get; }

        public CredentialChangedEventArgs(ICredentialRecord credentialRecord, ICredentialRepository repository)
        {
            CredentialRecord = credentialRecord ?? throw new ArgumentNullException(nameof(credentialRecord));
            Repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
    }
}