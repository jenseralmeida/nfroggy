using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Data.SqlClient;
using Froggy.Validation;

namespace Froggy.Data
{
    public partial class DbCommandUtil: IDisposable
    {
        private const string RETURN_PARAMETER_NAME = "__return";

        private DaScopeContext _daScopeContext;
        private DbDataAdapter _dataAdapter;
        private readonly Dictionary<DataAdapterCommand, DbCommandWrapper> _commandWrappersDataAdapter;
        private DataAdapterCommand _dataAdapterCommand;
        private int _updateBatchSize;

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public DbCommandUtil()
        {
            _commandWrappersDataAdapter = new Dictionary<DataAdapterCommand, DbCommandWrapper>();
            _daScopeContextIsCreatedInThisInstance = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        public DbCommandUtil(string commandText, CommandType commandType)
            : this()
        {
            ChangeToDataAdaterCommand(DataAdapterCommand.SelectCommand, commandText, commandType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandText"></param>
        public DbCommandUtil(string commandText)
            : this(commandText, CommandType.Text)
        {
        }

        public DbCommandUtil(string commandText, CommandType commandType, DataAdapterCommand dataAdapterCommand)
            : this()
        {
            ChangeToDataAdaterCommand(DataAdapterCommand.SelectCommand, commandText, commandType);
            _dataAdapterCommand = dataAdapterCommand;
        }

        public DbCommandUtil(string commandText, DataAdapterCommand dataAdapterCommand)
            : this(commandText, CommandType.Text)
        {
            _dataAdapterCommand = dataAdapterCommand;
        }

        public DbCommandUtil(string commandText, string connectionStringSettingName, DataAdapterCommand _dataAdapterCommand)
            : this(commandText, CommandType.Text)
        {
            _daScopeContext = new DaScopeContext(connectionStringSettingName);
            this._dataAdapterCommand = _dataAdapterCommand;
        }

        public DbCommandUtil(DataAdapterCommand _dataAdapterCommand)
        {
            _commandWrappersDataAdapter = new Dictionary<DataAdapterCommand, DbCommandWrapper>();
            _daScopeContextIsCreatedInThisInstance = false;
            this._dataAdapterCommand = _dataAdapterCommand;
        }

        #endregion Constructors

        #region Properties

        public DbCommand Command
        {
            get
            {
                CheckDisposed();
                DataAdapterCommand dataAdapterCommand = GetCurrentDataAdapterCommand();
                if (!_commandWrappersDataAdapter.ContainsKey(dataAdapterCommand))
                {
                    ConfigCommand(dataAdapterCommand, String.Empty);
                }
                DbCommandWrapper commandWrapper = _commandWrappersDataAdapter[dataAdapterCommand];
                return commandWrapper.GetDbCommand(DaScopeContext);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int CommandTimeout
        {
            get
            {
                if (CommandWrapperSelected == null)
                {
                    return DbCommandWrapper.DEFAULT_COMMAND_TIMEOUT;
                }
                return CommandWrapperSelected.CommandTimeout;
            }
            set
            {
                if (CommandWrapperSelected == null)
                {
                    ConfigCommand(_dataAdapterCommand, String.Empty);
                }
                else
                {
                    CommandWrapperSelected.CommandTimeout = value;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int UpdateBatchSize
        {
            get
            {
                if (_dataAdapter == null)
                {
                    return _updateBatchSize;
                }
                return _dataAdapter.UpdateBatchSize;
            }
            set
            {
                if (_dataAdapter != null)
                {
                    _dataAdapter.UpdateBatchSize = value;
                }
                _updateBatchSize = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public DbDataAdapter DataAdapter
        {
            get
            {
                CheckDisposed();
                if (_dataAdapter == null)
                    _dataAdapter = CreateDataAdapterFromCommandWrappers(DaScopeContext, _commandWrappersDataAdapter);
                return _dataAdapter;
            }
        }

        public DaScopeContext DaScopeContext
        {
            get
            {
                CheckDisposed();
                if (_daScopeContext == null)
                {
                    if (Scope.Current == null)
                    {
                        _daScopeContext = new DaScopeContext();
                        _daScopeContextIsCreatedInThisInstance = true;
                    }
                    else
                    {
                        _daScopeContext = Scope.Current.GetDaScopeContext();
                    }
                }
                return _daScopeContext;
            }
            set
            {
                _daScopeContextIsCreatedInThisInstance = false;
                _daScopeContext = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string CommandText
        {
            get
            {
                return CommandWrapperSelected == null ? "" : CommandWrapperSelected.CommandText;
            }
            set
            {
                if (CommandWrapperSelected == null)
                {
                    ConfigCommand(_dataAdapterCommand, value);
                }
                else
                {
                    CommandWrapperSelected.CommandText = value;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <seealso cref="System.Data.CommandType"/>
        public CommandType CommandType
        {
            get
            {
                return (CommandWrapperSelected == null) ? CommandType.Text : CommandWrapperSelected.CommandType;
            }
            set
            {
                if (CommandWrapperSelected == null)
                {
                    ConfigCommand(_dataAdapterCommand, String.Empty, value);
                }
                else
                {
                    CommandWrapperSelected.CommandType = value;
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        internal DbCommandWrapper CommandWrapperSelected
        {
            get
            {
                DataAdapterCommand dataAdapterCommand = GetCurrentDataAdapterCommand();
                if (!_commandWrappersDataAdapter.ContainsKey(dataAdapterCommand))
                    return null;
                return _commandWrappersDataAdapter[dataAdapterCommand];
            }
            set
            {
                _commandWrappersDataAdapter[GetCurrentDataAdapterCommand()] = value;
            }
        }

        #endregion Properties

        public void ChangeToDataAdaterCommand(DataAdapterCommand value, string commandText)
        {
            ChangeToDataAdaterCommand(value, commandText, CommandType.Text);
        }

        public void ChangeToDataAdaterCommand(DataAdapterCommand value, string commandText, CommandType commandType)
        {
            _dataAdapterCommand = value;
            CommandText = commandText;
            CommandType = commandType;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ChangeToDataAdaterCommand(DataAdapterCommand value)
        {
            _dataAdapterCommand = value;
        }

        /// <summary>
        /// Obtemo tipo de command do <see cref="DataAdapter"/> configurado para uso
        /// </summary>
        /// <returns></returns>
        public DataAdapterCommand GetCurrentDataAdapterCommand()
        {
            return _dataAdapterCommand;
        }

        #region Parameters

        public void SetParameterValue(string parameterName, object value)
        {
            CheckDisposed();
            DbParameter parameter = Command.Parameters[parameterName];
            SetParameterValue(parameter, value);
        }

        public object GetParameterValue(string parameterName)
        {
            CheckDisposed();
            return CommandWrapperSelected.ParametersOutputReturnValue[parameterName].Value;
        }

        public T GetParameterValue<T>(string parameterName)
        {
            CheckDisposed();
            object value = CommandWrapperSelected.ParametersOutputReturnValue[parameterName].Value;
            if (value == null || Convert.IsDBNull(value))
            {
                return default(T);
            }
            return new Validator<T>().Convert(value);
        }

        public object GetReturnValue()
        {
            CheckDisposed();
            return GetParameterValue(RETURN_PARAMETER_NAME);
        }

        public T GetReturnValue<T>()
        {
            CheckDisposed();
            return GetParameterValue<T>(RETURN_PARAMETER_NAME);
        }

        #region New parameters

        public DbParameter NewParameter(string parameterName, DbType dbType, int size, ParameterDirection direction, string sourceColumn)
        {
            CheckDisposed();
            DbParameter parameter = DaScopeContext.ProviderFactory.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.DbType = dbType;
            parameter.Direction = direction;
            parameter.Size = size;
            if (sourceColumn != null)
            {
                parameter.SourceColumn = sourceColumn;
            }
            Command.Parameters.Add(parameter);
            if (IsParameterDirectionInputOutputOrReturnValue(direction))
            {
                CommandWrapperSelected.ParametersOutputReturnValue.Add(parameterName, parameter);
            }
            return parameter;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="dbType"></param>
        /// <param name="precision"></param>
        /// <param name="scale"></param>
        /// <param name="direction"></param>
        /// <param name="sourceColumn"></param>
        /// <returns></returns>
        public DbParameter NewParameter(string parameterName, DbType dbType, byte precision, byte scale, ParameterDirection direction, string sourceColumn)
        {
            DbParameter parameter = NewParameter(parameterName, dbType, 0, direction, sourceColumn);
            var sqlParameter = parameter as SqlParameter;
            if (sqlParameter == null)
            {
                throw new NotSupportedException("The specifed provider does not suporte precision and scale");
            }
            else
            {
                sqlParameter.Precision = precision;
                sqlParameter.Scale = scale;
                return parameter;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="dbType"></param>
        /// <param name="precision"></param>
        /// <param name="scale"></param>
        /// <param name="sourceColumn"></param>
        /// <returns></returns>
        public DbParameter NewParameter(string parameterName, DbType dbType, byte precision, byte scale, string sourceColumn)
        {
            return NewParameter(parameterName, dbType, precision, scale, ParameterDirection.Input, sourceColumn);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dbType"></param>
        /// <param name="size"></param>
        /// <param name="sourceColumn"></param>
        /// <returns></returns>
        public DbParameter NewParameter(string name, DbType dbType, int size, string sourceColumn)
        {
            return NewParameter(name, dbType, size, ParameterDirection.Input, sourceColumn);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dbType"></param>
        /// <param name="sourceColumn"></param>
        /// <returns></returns>
        public DbParameter NewParameter(string name, DbType dbType, string sourceColumn)
        {
            return NewParameter(name, dbType, -1, sourceColumn);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dbType"></param>
        /// <param name="size"></param>
        /// <param name="sourceColumn"></param>
        /// <returns></returns>
        public DbParameter CreateInputOutputParameter(string name, DbType dbType, int size, string sourceColumn)
        {
            return NewParameter(name, dbType, size, ParameterDirection.InputOutput, sourceColumn);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dbType"></param>
        /// <param name="sourceColumn"></param>
        /// <returns></returns>
        public DbParameter CreateInputOutputParameter(string name, DbType dbType, string sourceColumn)
        {
            return CreateInputOutputParameter(name, dbType, -1, sourceColumn);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dbType"></param>
        /// <param name="size"></param>
        /// <param name="sourceColumn"></param>
        /// <returns></returns>
        public DbParameter CreateOutputParameter(string name, DbType dbType, int size, string sourceColumn)
        {
            return NewParameter(name, dbType, size, ParameterDirection.Output, sourceColumn);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dbType"></param>
        /// <param name="sourceColumn"></param>
        /// <returns></returns>
        public DbParameter CreateOutputParameter(string name, DbType dbType, string sourceColumn)
        {
            return CreateOutputParameter(name, dbType, -1, sourceColumn);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbType"></param>
        /// <param name="size"></param>
        /// <param name="sourceColumn"></param>
        /// <returns></returns>
        public DbParameter CreateReturnParameter(DbType dbType, int size, string sourceColumn)
        {
            return NewParameter(RETURN_PARAMETER_NAME, dbType, size, ParameterDirection.ReturnValue, sourceColumn);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbType"></param>
        /// <param name="sourceColumn"></param>
        /// <returns></returns>
        public DbParameter CreateReturnParameter(DbType dbType, string sourceColumn)
        {
            return CreateReturnParameter(dbType, -1, sourceColumn);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="dbType"></param>
        /// <param name="precision"></param>
        /// <param name="scale"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public DbParameter NewParameter(string parameterName, DbType dbType, byte precision, byte scale, ParameterDirection direction)
        {
            return NewParameter(parameterName, dbType, precision, scale, direction, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="dbType"></param>
        /// <param name="precision"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        public DbParameter NewParameter(string parameterName, DbType dbType, byte precision, byte scale)
        {
            return NewParameter(parameterName, dbType, precision, scale, ParameterDirection.Input);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dbType"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public DbParameter NewParameter(string name, DbType dbType, int size)
        {
            return NewParameter(name, dbType, size, ParameterDirection.Input, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public DbParameter NewParameter(string name, DbType dbType)
        {
            return NewParameter(name, dbType, -1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dbType"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public DbParameter CreateInputOutputParameter(string name, DbType dbType, int size)
        {
            return NewParameter(name, dbType, size, ParameterDirection.InputOutput, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public DbParameter CreateInputOutputParameter(string name, DbType dbType)
        {
            return CreateInputOutputParameter(name, dbType, -1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dbType"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public DbParameter CreateOutputParameter(string name, DbType dbType, int size)
        {
            return NewParameter(name, dbType, size, ParameterDirection.Output, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public DbParameter CreateOutputParameter(string name, DbType dbType)
        {
            return CreateOutputParameter(name, dbType, -1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbType"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public DbParameter CreateReturnParameter(DbType dbType, int size)
        {
            return NewParameter(RETURN_PARAMETER_NAME, dbType, size, ParameterDirection.ReturnValue, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public DbParameter CreateReturnParameter(DbType dbType)
        {
            return CreateReturnParameter(dbType, -1);
        }

        #endregion New parameters

        #region Add parameters

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="dbType"></param>
        /// <param name="size"></param>
        /// <param name="direction"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public DbParameter AddParameter(string parameterName, DbType dbType, int size, ParameterDirection direction, object value)
        {
            DbParameter parameter = NewParameter(parameterName, dbType, size, direction, null);
            SetParameterValue(parameter, value);
            return parameter;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="dbType"></param>
        /// <param name="precision"></param>
        /// <param name="scale"></param>
        /// <param name="direction"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public DbParameter AddParameter(string parameterName, DbType dbType, byte precision, byte scale, ParameterDirection direction, object value)
        {
            DbParameter parameter = NewParameter(parameterName, dbType, precision, scale, direction, null);
            SetParameterValue(parameter, value);
            return parameter;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="dbType"></param>
        /// <param name="precision"></param>
        /// <param name="scale"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public DbParameter AddParameter(string parameterName, DbType dbType, byte precision, byte scale, object value)
        {
            DbParameter parameter = NewParameter(parameterName, dbType, precision, scale, null);
            SetParameterValue(parameter, value);
            return parameter;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dbType"></param>
        /// <param name="size"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public DbParameter AddParameter(string name, DbType dbType, int size, object value)
        {
            return AddParameter(name, dbType, size, ParameterDirection.Input, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dbType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public DbParameter AddParameter(string name, DbType dbType, object value)
        {
            return AddParameter(name, dbType, -1, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dbType"></param>
        /// <param name="size"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public DbParameter AddInputOutputParameter(string name, DbType dbType, int size, object value)
        {
            return AddParameter(name, dbType, size, ParameterDirection.InputOutput, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dbType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public DbParameter AddInputOutputParameter(string name, DbType dbType, object value)
        {
            return AddInputOutputParameter(name, dbType, -1, value);
        }

        #endregion Adicao de parametros

        #endregion Parameters

        #region Execucoes padrao command

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int ExecuteNonQuery()
        {
            CheckDisposed();
            ConnectionState originalState;
            OpenConnection(DaScopeContext.Connection, out originalState);
            try
            {
                RegisterLog();
                return Command.ExecuteNonQuery();
            }
            finally
            {
                CloseConnection(DaScopeContext.Connection, originalState);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DbDataReader ExecuteReader()
        {
            CheckDisposed();
            ConnectionState originalState;
            OpenConnection(DaScopeContext.Connection, out originalState);
            RegisterLog();
            return Command.ExecuteReader();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="behavior"></param>
        /// <returns></returns>
        public DbDataReader ExecuteReader(CommandBehavior behavior)
        {
            CheckDisposed();
            ConnectionState originalState;
            OpenConnection(DaScopeContext.Connection, out originalState);
            RegisterLog();
            return Command.ExecuteReader(behavior);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public object ExecuteScalar()
        {
            CheckDisposed();
            ConnectionState originalState;
            OpenConnection(DaScopeContext.Connection, out originalState);
            try
            {
                RegisterLog();
                return Command.ExecuteScalar();
            }
            finally
            {
                CloseConnection(DaScopeContext.Connection, originalState);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T ExecuteScalar<T>()
        {
            CheckDisposed();
            ConnectionState originalState;
            OpenConnection(DaScopeContext.Connection, out originalState);
            try
            {
                object value = ExecuteScalar();
                if (value == null || Convert.IsDBNull(value))
                {
                    return default(T);
                }
                return new Validator<T>().Convert(value);
            }
            finally
            {
                CloseConnection(DaScopeContext.Connection, originalState);
            }
        }

        #endregion 

        #region DataSet and DataTable operations

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataSet"></param>
        /// <returns></returns>
        public int Fill(DataSet dataSet)
        {
            CheckDisposed();
            RegisterLog();
            ConnectionState originalState;
            OpenConnection(DaScopeContext.Connection, out originalState);
            try
            {
                int returnValue = DataAdapter.Fill(dataSet);
                return returnValue;
            }
            finally
            {
                CloseConnection(DaScopeContext.Connection, originalState);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="srcTables"></param>
        /// <returns></returns>
        public int Fill(DataSet dataSet, params string[] srcTables)
        {
            CheckDisposed();
            RegisterLog();
            ConnectionState originalState;
            OpenConnection(DaScopeContext.Connection, out originalState);
            try
            {
                int returnValue;
                // Determina o tipo de mapeamento a ser usado
                if (srcTables == null || srcTables.Length == 0)
                {
                    // Se nenhuma tabela de origem entao usa o name retornado pela query
                    returnValue = Fill(dataSet);
                }
                else if (srcTables.Length == 1)
                {
                    // Usa o overload disponivel em DbDataAdapter se apenas uma tabela foi informado
                    returnValue = DataAdapter.Fill(dataSet, srcTables[0]);
                }
                else
                {
                    ConfigTableMappings(DataAdapter, srcTables);
                    returnValue = DataAdapter.Fill(dataSet);
                }
                return returnValue;
            }
            finally
            {
                CloseConnection(DaScopeContext.Connection, originalState);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public int Fill(DataTable dataTable)
        {
            CheckDisposed();
            RegisterLog();
            ConnectionState originalState;
            OpenConnection(DaScopeContext.Connection, out originalState);
            try
            {
                int returnValue = DataAdapter.Fill(dataTable);
                return returnValue;
            }
            finally
            {
                CloseConnection(DaScopeContext.Connection, originalState);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataSet GetDataSet()
        {
            var dataSet = new DataSet();
            Fill(dataSet);
            return dataSet;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetDataTable()
        {
            var dataTable = new DataTable();
            Fill(dataTable);
            return dataTable;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetDataSet<T>() where T : DataSet, new()
        {
            var dataSet = new T();
            Fill(dataSet);
            return dataSet;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="srcTables"></param>
        /// <returns></returns>
        public T GetDataSet<T>(params string[] srcTables) where T : DataSet, new()
        {
            var dataSet = new T();
            Fill(dataSet, srcTables);
            return dataSet;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetDataTable<T>() where T : DataTable, new()
        {
            var dataTable = new T();
            Fill(dataTable);
            return dataTable;
        }

        public int Update(DataSet dataSet)
        {
            return Update(dataSet, TransactionOption.Required);
        }

        public int Update(DataSet dataSet, TransactionOption transactionOption)
        {
            throw new NotImplementedException();
            //using (Scope scope = new Scope(new DaScopeContext(transactionOption)))
            //{
            //    int returnValue = DataAdapter.Update(dataSet);
            //    scope.Complete();
            //    return returnValue;
            //}
        }

        public int Update(DataSet dataSet, string srcTable)
        {
            return Update(dataSet.Tables[srcTable], TransactionOption.Required);
        }

        public int Update(DataTable dataTable)
        {
            return Update(dataTable, TransactionOption.Required);
        }
        public int Update(DataTable dataTable, TransactionOption transactionOption)
        {
            throw new NotImplementedException();
            //using (var scope = new Scope(new DaScopeContext(transactionOption)))
            //{
            //    int returnValue = DataAdapter.Update(dataTable);
            //    scope.Complete();
            //    return returnValue;
            //}
        }

        public int Update(DataRow[] dataRows)
        {
            return Update(dataRows, TransactionOption.Required);
        }

        public int Update(DataRow[] dataRows, TransactionOption transactionOption)
        {
            throw new NotImplementedException();
            //using (var scope = new Scope(new DaScopeContext(transactionOption)))
            //{
            //    var returnValue = DataAdapter.Update(dataRows);
            //    scope.Complete();
            //    return returnValue;
            //}
        }

        #endregion Consulta DataSet e DataTable

        #region Configuracao dos comandos

        private void ConfigCommand(DataAdapterCommand dataAdapterCommand, string commandText)
        {
            ConfigCommand(dataAdapterCommand, commandText, CommandType.Text);
        }

        private void ConfigCommand(DataAdapterCommand dataAdapterCommand, string commandText, CommandType commandType)
        {
            ConfigCommand(dataAdapterCommand, commandText, commandType, DbCommandWrapper.DEFAULT_COMMAND_TIMEOUT);
        }

        private void ConfigCommand(DataAdapterCommand dataAdapterCommand, string commandText, CommandType commandType, int commandTimeout)
        {
            DbCommandWrapper commandWrapper;
            if (!_commandWrappersDataAdapter.ContainsKey(dataAdapterCommand))
            {
                commandWrapper = new DbCommandWrapper(commandText, commandType);
                _commandWrappersDataAdapter.Add(dataAdapterCommand, commandWrapper);
            }
            else
            {
                commandWrapper = _commandWrappersDataAdapter[dataAdapterCommand];
                commandWrapper.CommandText = commandText;
                commandWrapper.CommandType = commandType;
                commandWrapper.CommandTimeout = commandTimeout;
            }
            UpdateCommandOfDataAdapter(dataAdapterCommand, commandWrapper);
        }

        /// <summary>
        /// Update commands of DataAdater if the instance already exists
        /// </summary>
        /// <param name="dataAdapterCommand"></param>
        /// <param name="commandWrapper"></param>
        private void UpdateCommandOfDataAdapter(DataAdapterCommand dataAdapterCommand, DbCommandWrapper commandWrapper)
        {
            if (_dataAdapter != null)
            {
                switch (dataAdapterCommand)
                {
                    case DataAdapterCommand.SelectCommand:
                        DataAdapter.SelectCommand = commandWrapper.GetDbCommand(DaScopeContext);
                        break;
                    case DataAdapterCommand.InsertCommand:
                        DataAdapter.InsertCommand = commandWrapper.GetDbCommand(DaScopeContext);
                        break;
                    case DataAdapterCommand.UpdateCommand:
                        DataAdapter.UpdateCommand = commandWrapper.GetDbCommand(DaScopeContext);
                        break;
                    case DataAdapterCommand.DeleteCommand:
                        DataAdapter.DeleteCommand = commandWrapper.GetDbCommand(DaScopeContext);
                        break;
                }
            }
        }

        #endregion 

        #region IDisposable

        private bool _isDisposed;
        private bool _daScopeContextIsCreatedInThisInstance;

        private void CheckDisposed()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("DbCommandUtil");
            }

        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool isDisposing)
        {
            bool canDispose = (!_isDisposed) && isDisposing;
            if (canDispose)
            {
                foreach (DbCommandWrapper commandWrapper in _commandWrappersDataAdapter.Values)
                {
                    commandWrapper.Dispose();
                }
                if (_daScopeContextIsCreatedInThisInstance && _daScopeContext != null)
                    _daScopeContext.Dispose();
            }
            _isDisposed = true;
        }

        #endregion IDisposable

        private void RegisterLog()
        {
            //throw new NotImplementedException();
            Console.WriteLine(this._isDisposed);
        }

    }
}