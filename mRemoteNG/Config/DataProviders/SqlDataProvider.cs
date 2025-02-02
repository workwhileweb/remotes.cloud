﻿using System.Data;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Messages;
using mRemoteNG.App;
using mRemoteNG.Properties;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;

namespace mRemoteNG.Config.DataProviders
{
    public class SqlDataProvider : IDataProvider<DataTable>
    {
        public IDatabaseConnector DatabaseConnector { get; }

        public SqlDataProvider(IDatabaseConnector databaseConnector)
        {
            DatabaseConnector = databaseConnector;
        }

        public DataTable Load()
        {
            var dataTable = new DataTable();
            var dbQuery = DatabaseConnector.DbCommand("SELECT * FROM tblCons ORDER BY PositionID ASC");
            DatabaseConnector.AssociateItemToThisConnector(dbQuery);
            if (!DatabaseConnector.IsConnected)
                OpenConnection();
            var dbDataReader = dbQuery.ExecuteReader(CommandBehavior.CloseConnection);

            if (dbDataReader.HasRows)
                dataTable.Load(dbDataReader);
            dbDataReader.Close();
            return dataTable;
        }

        public void Save(DataTable dataTable)
        {
            if (DbUserIsReadOnly())
            {
                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                                                    "Trying to save connections but the SQL read only checkbox is checked, aborting!");
                return;
            }

            if (!DatabaseConnector.IsConnected)
                OpenConnection();
            if (DatabaseConnector.GetType() == typeof(MsSqlDatabaseConnector))
            {
                var sqlConnection = (SqlConnection)DatabaseConnector.DbConnection();
                using (var transaction = sqlConnection.BeginTransaction(IsolationLevel.Serializable))
                {
                    using (var sqlCommand = new SqlCommand())
                    {
                        sqlCommand.Connection = sqlConnection;
                        sqlCommand.Transaction = transaction;
                        sqlCommand.CommandText = "SELECT * FROM tblCons";
                        using (var dataAdpater = new SqlDataAdapter())
                        {
                            dataAdpater.SelectCommand = sqlCommand;

                            var builder = new SqlCommandBuilder(dataAdpater);
                            // Avoid optimistic concurrency, check if it is necessary.
                            builder.ConflictOption = ConflictOption.OverwriteChanges;

                            dataAdpater.UpdateCommand = builder.GetUpdateCommand();

                            dataAdpater.DeleteCommand = builder.GetDeleteCommand();
                            dataAdpater.InsertCommand = builder.GetInsertCommand();

                            dataAdpater.Update(dataTable);
                            transaction.Commit();
                        }
                    }
                }

            }
            else if (DatabaseConnector.GetType() == typeof(MySqlDatabaseConnector))
            {
                var dbConnection = (MySqlConnection) DatabaseConnector.DbConnection();
                using (var transaction = dbConnection.BeginTransaction(IsolationLevel.Serializable))
                {
                    using (var sqlCommand = new MySqlCommand())
                    {
                        sqlCommand.Connection = dbConnection;
                        sqlCommand.Transaction = transaction;
                        sqlCommand.CommandText = "SELECT * FROM tblCons";
                        using (var dataAdapter = new MySqlDataAdapter(sqlCommand))
                        {
                            dataAdapter.UpdateBatchSize = 1000;
                            using (var cb = new MySqlCommandBuilder(dataAdapter))
                            {
                                dataAdapter.Update(dataTable);
                                transaction.Commit();
                            }
                        }
                    }
                }
            }
        }

        public void OpenConnection()
        {
            DatabaseConnector.Connect();
        }

        public void CloseConnection()
        {
            DatabaseConnector.Disconnect();
        }

        private bool DbUserIsReadOnly()
        {
            return OptionsDBsPage.Default.SQLReadOnly;
        }
    }
}