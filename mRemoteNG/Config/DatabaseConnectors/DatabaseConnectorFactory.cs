using mRemoteNG.App;
using mRemoteNG.Properties;
using mRemoteNG.Security.SymmetricEncryption;

namespace mRemoteNG.Config.DatabaseConnectors
{
    public class DatabaseConnectorFactory
    {
        public static IDatabaseConnector DatabaseConnectorFromSettings()
        {
            var sqlType = OptionsDBsPage.Default.SQLServerType;
            var sqlHost = OptionsDBsPage.Default.SQLHost;
            var sqlCatalog = OptionsDBsPage.Default.SQLDatabaseName;
            var sqlUsername = OptionsDBsPage.Default.SQLUser;
            var cryptographyProvider = new LegacyRijndaelCryptographyProvider();
            var sqlPassword = cryptographyProvider.Decrypt(OptionsDBsPage.Default.SQLPass, Runtime.EncryptionKey);

            return DatabaseConnector(sqlType, sqlHost, sqlCatalog, sqlUsername, sqlPassword);
        }

        public static IDatabaseConnector DatabaseConnector(string type, string server, string database, string username, string password)
        {
            switch (type)
            {
                case "mysql":
                    return new MySqlDatabaseConnector(server, database, username, password);
                case "mssql":
                default:
                    return new MsSqlDatabaseConnector(server, database, username, password);
            }
        }
    }
}