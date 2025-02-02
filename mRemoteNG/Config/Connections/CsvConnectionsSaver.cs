﻿using System;
using mRemoteNG.App;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Serializers.ConnectionSerializers.Csv;
using mRemoteNG.Security;
using mRemoteNG.Tree;

namespace mRemoteNG.Config.Connections
{
    public class CsvConnectionsSaver : ISaver<ConnectionTreeModel>
    {
        private readonly string _connectionFileName;
        private readonly SaveFilter _saveFilter;

        public CsvConnectionsSaver(string connectionFileName, SaveFilter saveFilter)
        {
            if (string.IsNullOrEmpty(connectionFileName))
                throw new ArgumentException($"Argument '{nameof(connectionFileName)}' cannot be null or empty");

            _connectionFileName = connectionFileName;
            _saveFilter = saveFilter ?? throw new ArgumentNullException(nameof(saveFilter));
        }

        public void Save(ConnectionTreeModel connectionTreeModel, string propertyNameTrigger = "")
        {
            var csvConnectionsSerializer =
                new CsvConnectionsSerializerMremotengFormat(_saveFilter, Runtime.CredentialProviderCatalog);
            var dataProvider = new FileDataProvider(_connectionFileName);
            var csvContent = csvConnectionsSerializer.Serialize(connectionTreeModel);
            dataProvider.Save(csvContent);
        }
    }
}