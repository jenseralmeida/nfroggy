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

        private readonly string _ConnectionStringSettingName;
        private readonly TransactionOption? _TransactionOption;
        private readonly string _ProviderName;
        private readonly string _ConnectionString;
        private readonly IsolationLevel? _IsolationLevel;
        private DbProviderFactory _ProviderFactory;
        private DbConnection _Connection;
        private DbTransaction _Transaction;
        private bool _Initialized;

        public string ConnectionStringSettingName
        {
            get
            {
                CheckDisposed();
                TryInit();
                return _ConnectionStringSettingName;
            }
        }

        public IsolationLevel? IsolationLevel
        {
            get { return _IsolationLevel; }
        }

        /// <summary>
        /// 
        /// </summary>
        public DbProviderFactory ProviderFactory
        {
            get
            {
                CheckDisposed();
                TryInit();
                return _ProviderFactory;
            }
            set
            {
                CheckDisposed();
                    _ProviderFactory = value;
            }

        }

        public DbConnection Connection
        {
            get
            {
                CheckDisposed();
                TryInit();
                if (_Connection == null && _ProviderFactory != null)
                {
                    _Connection = ProviderFactory.CreateConnection();
                }
                return _Connection;
            }
        }

        public DbTransaction Transaction
        {
            get 
            {
                CheckDisposed();
                TryInit();
                return _Transaction; 
            }
            set
            {
                CheckDisposed();
                TryInit();
                _Transaction = value;
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
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionStringSettingName"></param>
        /// <param name="isolationLevel"></param>
        public DAScopeContext(string connectionStringSettingName, IsolationLevel isolationLevel)
            : this(connectionStringSettingName, TransactionOption.Required, isolationLevel)
        {
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
            _ProviderName = providerName;
            _ConnectionString = connectionString;
            _ConnectionStringSettingName = String.Empty;
        }

        public DAScopeContext(string connectionStringSettingName, TransactionOption transactionOption, IsolationLevel? isolationLevel)
        {
            _ConnectionStringSettingName = connectionStringSettingName;
            _TransactionOption = transactionOption;
            _IsolationLevel = isolationLevel;
        }

        #endregion Constructor

        #region ScopeContext

        public override void Init()
        {
            if (String.IsNullOrEmpty(_ConnectionStringSettingName))
            {
                _ProviderFactory = DbProviderFactories.GetFactory(_ProviderName);
                _Connection = ProviderFactory.CreateConnection();
                _Connection.ConnectionString = _ConnectionString;

            }
            else
            {
                ConnectionStringSettings connectionStringSettings = ConfigurationManager.ConnectionStrings[_ConnectionStringSettingName];
                if (connectionStringSettings == null)
                {
                    throw new InvalidOperationException("The specified connection does not exists in <connectionString> in <configuration>");
                }
                _ProviderFactory = DbProviderFactories.GetFactory(connectionStringSettings.ProviderName);
                _Connection = ProviderFactory.CreateConnection();
                _Connection.ConnectionString = connectionStringSettings.ConnectionString;
            }
            InitTransaction();
            _Initialized = true;
        }

        private void InitTransaction()
        {
            if (_TransactionOption == TransactionOption.Required)
            {
                if (IsolationLevel.HasValue)
                {
                    _Transaction = Connection.BeginTransaction(IsolationLevel.Value);
                }
                else
                {
                    Transaction = Connection.BeginTransaction();
                }
            }
        }

        private void TryInit()
        {
            if (!_Initialized)
            {
                Init();
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
                bool requireNewTransaction = _TransactionOption.HasValue && _TransactionOption == TransactionOption.Required;
                return requireNewTransaction && currentScopeHasNotTransaction;
            }
        }

        public override bool RefuseNewScope
        {
            get { return false; }
        }

        public override void CompletedNow(bool completed)
        {
            if (completed && _Transaction != null)
            {
                _Transaction.Commit();
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
