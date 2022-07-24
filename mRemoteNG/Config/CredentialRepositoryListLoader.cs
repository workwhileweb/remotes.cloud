using System;
using System.Collections.Generic;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Serializers.CredentialProviderSerializer;
using mRemoteNG.Credential;

namespace mRemoteNG.Config
{
    public class CredentialRepositoryListLoader : ILoader<IEnumerable<ICredentialRepository>>
    {
        private readonly IDataProvider<string> _dataProvider;
        private readonly CredentialRepositoryListDeserializer _deserializer;

        public CredentialRepositoryListLoader(IDataProvider<string> dataProvider,
                                              CredentialRepositoryListDeserializer deserializer)
        {
            _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
            _deserializer = deserializer ?? throw new ArgumentNullException(nameof(deserializer));
        }

        public IEnumerable<ICredentialRepository> Load()
        {
            var data = _dataProvider.Load();
            return _deserializer.Deserialize(data);
        }
    }
}