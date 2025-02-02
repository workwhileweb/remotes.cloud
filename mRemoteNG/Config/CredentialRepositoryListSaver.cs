﻿using System;
using System.Collections.Generic;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Serializers.CredentialProviderSerializer;
using mRemoteNG.Credential;

namespace mRemoteNG.Config
{
    public class CredentialRepositoryListSaver : ISaver<IEnumerable<ICredentialRepository>>
    {
        private readonly IDataProvider<string> _dataProvider;

        public CredentialRepositoryListSaver(IDataProvider<string> dataProvider)
        {
            _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
        }

        public void Save(IEnumerable<ICredentialRepository> repositories, string propertyNameTrigger = "")
        {
            var serializer = new CredentialRepositoryListSerializer();
            var data = serializer.Serialize(repositories);
            _dataProvider.Save(data);
        }
    }
}