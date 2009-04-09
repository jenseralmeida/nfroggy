using System;
using System.Data;
using System.Data.Common;
using System.Configuration;

namespace Froggy.Data
{
    public static class DAScopeContextExtension
    {
        public static DAScopeContext GetDAScopeContext(this Scope scope)
		{
            return scope.GetScopeContext<DAScopeContext>();
		}
    }
    /// <summary>
    /// Scope context to Data Access
    /// </summary>
    public class DAScopeContext: ScopeContext, IDisposable
    {
        public const string ConnectionStringSettingName_DEFAULT = "Default";

        private string _connectionStringSettingName;
        private TransactionOption? _transactionOption;
        private readonly string _providerName;
        private readonly string _connectionString;
        private IsolationLevel? _isolationLevel;
        private DbProviderFactory _providerFactory;
        private DbConnection _connection;
        private DbTransaction _transaction;

        public string ConnectionStringSettingName
        {
            get
            {
                CheckDisposed();
                return _connectionStringSettingName;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public DbProviderFactory ProviderFactory
        {
            get
            {
                CheckDisposed();
                return _providerFactory;
            }
            set
            {
                CheckDisposed();
                _providerFactory = value;
            }

        }

        public DbConnection Connection
        {
            get
            {
                CheckDisposed();
                if (_connection == null && _providerFactory != null)
                {
                    _connection = ProviderFactory.CreateConnection();
                }
                return _connection;
            }
            set
            {
                CheckDisposed();
                _connectionStringSettingName = String.Empty;
                _connection = value;
            }

        }

        public DbTransaction Transaction
        {
            get 
            {
                CheckDisposed();
                return _transaction; 
            }
            set
            {
                CheckDisposed();
                _transaction = value;
            }

        }

        #region Constructor

        /// <summary>
        /// Context to connection using <see cref="ConnectionStringSettingName_DEFAULT"/> and <see cref="TransactionOption.Automatic"/>  
        /// </summary>
        public DAScopeContext()
            : this(ConnectionStringSettingName_DEFAULT)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionStringSettingName"></param>
        public DAScopeContext(string connectionStringSettingName)
            : this(connectionStringSettingName, TransactionOption.Automatic, null)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionStringSettingName"></param>
        /// <param name="transactionOption"></param>
        public DAScopeContext(string connectionStringSettingName, TransactionOption transactionOption)
            : this(connectionStringSettingName, transactionOption, null)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isolationLevel"></param>
        public DAScopeContext(IsolationLevel isolationLevel)
            : this(ConnectionStringSettingName_DEFAULT, TransactionOption.Required, isolationLevel)
        {
            _isolationLevel = isolationLevel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionStringSettingName"></param>
        /// <param name="isolationLevel"></param>
        public DAScopeContext(string connectionStringSettingName, IsolationLevel isolationLevel)
            : this(connectionStringSettingName, TransactionOption.Required, isolationLevel)
        {
            _isolationLevel = isolationLevel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="connectionString"></param>
        public DAScopeContext(string providerName, string connectionString)
        {
            if (String.IsNullOrEmpty(providerName) || String.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("ProviderName and ConnectionString can't be null");
            }
            _providerName = providerName;
            _connectionString = connectionString;
            _connectionStringSettingName = String.Empty;
        }

        public DAScopeContext(string connectionStringSettingName, TransactionOption transactionOption, IsolationLevel? isolationLevel)
        {
            _connectionStringSettingName = connectionStringSettingName;
            _transactionOption = transactionOption;
            _isolationLevel = isolationLevel;
        }

        #endregion Constructor

        #region ScopeContext

        public override void Init()
        {
            if (String.IsNullOrEmpty(_connectionStringSettingName))
            {
                _providerFactory = DbProviderFactories.GetFactory(_providerName);
                _connectionStringSettingName = String.Empty;
                _connection = ProviderFactory.CreateConnection();
                _connection.ConnectionString = _connectionString;
            }
            else
            {
                ConnectionStringSettings connectionStringSettings = ConfigurationManager.ConnectionStrings[_connectionStringSettingName];
                if (connectionStringSettings == null)
                {
                    throw new InvalidOperationException("The specified connection does not exists in <connectionString> in <configuration>");
                }
                _providerFactory = DbProviderFactories.GetFactory(connectionStringSettings.ProviderName);
                _connectionStringSettingName = connectionStringSettings.Name;
                _connection = ProviderFactory.CreateConnection();
                _connection.ConnectionString = connectionStringSettings.ConnectionString;

            }
        }

        public override bool NewScopeContextIsCompatible(ScopeContext currentScopeContext)
        {
            var currentDAScopeContext = currentScopeContext as DAScopeContext;
            if (currentDAScopeContext == null)
                return true;
            bool useSameConnectionString = currentDAScopeContext.ConnectionStringSettingName == ConnectionStringSettingName;
            return useSameConnectionString;
        }

        public override bool RequireNewScope
        {
            get
            {
                if (Scope.Current == null)
                    return false;
                bool currentScopeHasNotTransaction = Scope.Current.GetDAScopeContext().Transaction == null;
                bool requireNewTransaction = _transactionOption.HasValue && _transactionOption == TransactionOption.Required;
                return requireNewTransaction && currentScopeHasNotTransaction;
            }
        }

        public override bool RefuseNewScope
        {
            get { return false; }
        }

        public override void CompletedNow(bool completed)
        {
            if (completed && _transaction != null)
            {
                _transaction.Commit();
            }
        }

        #endregion ScopeContext

        #region IDisposable

        private bool isDisposed;

        private void CheckDisposed()
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException("DAScopeContext object is already disposed");
            }
        }

        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected override void Dispose(bool disposing)
        {
            bool canFinalize = (!isDisposed) && disposing;
            if (canFinalize)
            {
                Exception transactionException = null;
                try
                {
                    if (Transaction != null)
                        Transaction.Dispose();
                }
                catch (Exception exception)
                {
                    //Garante que qualquer erro no dispose da transacao nao impeca
                    //a chamada do dispose da conexao
                    transactionException = exception;
                }
                if (Connection != null)
                {
                    if (Connection.State == ConnectionState.Open)
                        Connection.Close();
                    Connection.Dispose();
                }
                //Se ocorreu alguma excessao no dispose da transacao dispara agora
                if (transactionException != null)
                    throw transactionException;
            }
            isDisposed = true;
        }

        ~DAScopeContext()
        {
            Dispose(false);
        }

        #endregion IDisposable
    }
}
