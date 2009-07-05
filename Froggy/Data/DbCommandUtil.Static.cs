using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Froggy.Validation;

namespace Froggy.Data
{
    public partial class DbCommandUtil
    {
        private static bool IsParameterDirectionInputOutputOrReturnValue(ParameterDirection direction)
        {
            switch (direction)
            {
                case ParameterDirection.InputOutput:
                case ParameterDirection.Output:
                case ParameterDirection.ReturnValue:
                    return true;
                default:
                    return false;
            }
        }

        private static void OpenConnection(DbConnection connection, out ConnectionState originalState)
        {
            originalState = connection.State;
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
                ConfigureSnapshotToSqlClientFactory(connection);
            }
        }

        private static void CloseConnection(IDbConnection connection, ConnectionState estadoOriginal)
        {
            if ((connection != null) && (estadoOriginal == ConnectionState.Closed))
            {
                connection.Close();
            }
        }

        private static void ConfigureSnapshotToSqlClientFactory(DbConnection connection)
        {
            if (connection.GetType() == typeof(SqlConnection))
            {
                if (CanActivateSnapshotToSqlServerVersion(connection.ServerVersion))
                {
                    DbCommand comm = connection.CreateCommand();
                    comm.CommandText = "SET TRANSACTION ISOLATION LEVEL SNAPSHOT";
                    comm.UpdatedRowSource = UpdateRowSource.None;
                    comm.ExecuteNonQuery();
                }
            }
        }

        private static bool CanActivateSnapshotToSqlServerVersion(string serverVersion)
        {
            string majorVersion = serverVersion.Split('.')[0];
            if (Convert.ToInt32(majorVersion) > 8)
                return true;
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="daScopeContext"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        internal static DbCommand CreateCommand(DaScopeContext daScopeContext, string commandText, CommandType commandType, int commandTimeout)
        {
            if (daScopeContext == null)
                throw new ArgumentNullException("daScopeContext");
            DbCommand command = daScopeContext.ProviderFactory.CreateCommand();
            command.Connection = daScopeContext.Connection;
            command.CommandText = commandText;
            command.CommandType = commandType;
            command.CommandTimeout = commandTimeout;
            if (daScopeContext.Transaction != null)
            {
                command.Transaction = daScopeContext.Transaction;
            }
            return command;
        }

        private static void ConfigTableMappings(DataAdapter adapter, string[] srcTables)
        {
            for (int i = 0; i < srcTables.Length; i++)
            {
                string tableName = "Table" + (i == 0 ? String.Empty : i.ToString());
                adapter.TableMappings.Add(tableName, srcTables[i]);
            }
        }

        private static void SetParameterValue(IDataParameter parameter, object value)
        {
            value = new Validator<object>().Convert(value);
            parameter.Value = value ?? DBNull.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="daScopeContext"></param>
        /// <param name="dataAdapterCommand"></param>
        /// <param name="commandWrappersDataAdapter"></param>
        /// <returns></returns>
        internal static DbCommand GetDbCommand(DaScopeContext daScopeContext, DataAdapterCommand dataAdapterCommand, Dictionary<DataAdapterCommand, DbCommandWrapper> commandWrappersDataAdapter)
        {
            if (commandWrappersDataAdapter.ContainsKey(dataAdapterCommand))
            {
                DbCommandWrapper commandWrapper = commandWrappersDataAdapter[dataAdapterCommand];
                return commandWrapper.GetDbCommand(daScopeContext);
            }
            return null;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="daScopeContext"></param>
        /// <param name="commandWrappers"></param>
        /// <returns></returns>
        internal static DbDataAdapter CreateDataAdapterFromCommandWrappers(DaScopeContext daScopeContext, Dictionary<DataAdapterCommand, DbCommandWrapper> commandWrappers)
        {
            if (daScopeContext == null)
                throw new ArgumentNullException("daScopeContext");
            DbCommand selectCommand = GetDbCommand(daScopeContext, DataAdapterCommand.SelectCommand, commandWrappers);
            if (selectCommand == null)
                throw new NullReferenceException("SelectCommand cannot be null");
            DbDataAdapter dataAdapter = daScopeContext.ProviderFactory.CreateDataAdapter();
            dataAdapter.SelectCommand = selectCommand;
            dataAdapter.InsertCommand = GetDbCommand(daScopeContext, DataAdapterCommand.InsertCommand, commandWrappers);
            dataAdapter.UpdateCommand = GetDbCommand(daScopeContext, DataAdapterCommand.UpdateCommand, commandWrappers);
            dataAdapter.DeleteCommand = GetDbCommand(daScopeContext, DataAdapterCommand.DeleteCommand, commandWrappers);

            return dataAdapter;
        }
    }
}
