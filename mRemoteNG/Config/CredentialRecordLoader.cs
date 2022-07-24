using System;
using System.Collections.Generic;
using System.Security;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Credential;


namespace mRemoteNG.Config
{
    public class CredentialRecordLoader
    {
        private readonly IDataProvider<string> _dataProvider;
        private readonly ISecureDeserializer<string, IEnumerable<ICredentialRecord>> _deserializer;

        public CredentialRecordLoader(IDataProvider<string> dataProvider,
                                      ISecureDeserializer<string, IEnumerable<ICredentialRecord>> deserializer)
        {
            _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
            _deserializer = deserializer ?? throw new ArgumentNullException(nameof(deserializer));
        }

        public IEnumerable<ICredentialRecord> Load(SecureString key)
        {
            var serializedCredentials = _dataProvider.Load();
            return _deserializer.Deserialize(serializedCredentials, key);
        }
    }
}