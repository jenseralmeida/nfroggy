using System;
using System.Data;
using System.Data.Common;
using System.Configuration;

namespace Froggy.Data
{
    /// <summary>
    /// Scope context to Data Access
    /// </summary>
    public class DaScopeContext : ScopeContext, IDisposable
    {
        public const string ConnectionStringSettingNameDefault = "Default";

        private readonly string _ConnectionStringSettingName;
        private readonly TransactionOption? _TransactionOption;
        private readonly string _ProviderName;
        private readonly string _ConnectionString;
        private readonly IsolationLevel? _IsolationLevel;
        private DbProviderFactory _ProviderFactory;
        private DbConnection _Connection;
        private DbTransaction _Transaction;
        private bool _MaitainConnectionOpen;
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
                if (_Connection != null)
                {
                    if (_MaitainConnectionOpen && (_Connection.State != ConnectionState.Open))
                        _Connection.Open();
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
        }

        #region Constructor

        /// <summary>
        /// Context to connection using <see cref="ConnectionStringSettingNameDefault"/> and <see cref="TransactionOption.Automatic"/>  
        /// </summary>
        public DaScopeContext()
            : this(ConnectionStringSettingNameDefault)
        {
        }

        /// <summary>
        /// Context to connection using <see cref="ConnectionStringSettingNameDefault"/> and <see cref="TransactionOption.Automatic"/>  
        /// </summary>
        public DaScopeContext(TransactionOption transactionOption)
            : this(ConnectionStringSettingNameDefault, transactionOption)
        {
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionStringSettingName"></param>
        public DaScopeContext(string connectionStringSettingName)
            : this(connectionStringSettingName, TransactionOption.Automatic, null)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionStringSettingName"></param>
        /// <param name="transactionOption"></param>
        public DaScopeContext(string connectionStringSettingName, TransactionOption transactionOption)
            : this(connectionStringSettingName, transactionOption, null)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isolationLevel"></param>
        public DaScopeContext(IsolationLevel isolationLevel)
            : this(ConnectionStringSettingNameDefault, TransactionOption.Required, isolationLevel)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionStringSettingName"></param>
        /// <param name="isolationLevel"></param>
        public DaScopeContext(string connectionStringSettingName, IsolationLevel isolationLevel)
            : this(connectionStringSettingName, TransactionOption.Required, isolationLevel)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="connectionString"></param>
        public DaScopeContext(string providerName, string connectionString)
        {
            if (String.IsNullOrEmpty(providerName) || String.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("ProviderName and ConnectionString can't be null");
            }
            _ProviderName = providerName;
            _ConnectionString = connectionString;
            _ConnectionStringSettingName = String.Empty;
        }

        public DaScopeContext(string providerName, string connectionString, TransactionOption transactionOption, IsolationLevel? isolationLevel)
            : this(providerName, connectionString)
        {
            _TransactionOption = transactionOption;
            _IsolationLevel = isolationLevel;
        }

        public DaScopeContext(string connectionStringSettingName, TransactionOption transactionOption, IsolationLevel? isolationLevel)
        {
            _ConnectionStringSettingName = connectionStringSettingName;
            _TransactionOption = transactionOption;
            _IsolationLevel = isolationLevel;
        }

        #endregion Constructor

        #region ScopeContext

        public override void Init()
        {
            Init(true);
        }

        private void Init(bool maitainConnectionOpen)
        {
            _MaitainConnectionOpen = maitainConnectionOpen;
            if (String.IsNullOrEmpty(_ConnectionStringSettingName))
            {
                _ProviderFactory = DbProviderFactories.GetFactory(_ProviderName);
                _Connection = _ProviderFactory.CreateConnection();
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
                _Connection = _ProviderFactory.CreateConnection();
                _Connection.ConnectionString = connectionStringSettings.ConnectionString;
            }
            if (_MaitainConnectionOpen)
            {
                _Connection.Open();
            }
            InitTransaction();
            _Initialized = true;
        }

        private void InitTransaction()
        {
            if (_TransactionOption == TransactionOption.Required)
            {
                _Transaction = IsolationLevel.HasValue ? _Connection.BeginTransaction(IsolationLevel.Value) : _Connection.BeginTransaction();
            }
        }

        private void TryInit()
        {
            if (!_Initialized)
            {
                Init(false);
            }
        }

        public override bool NewScopeContextIsCompatible(ScopeContext currentScopeContext)
        {
            var currentDaScopeContext = currentScopeContext as DaScopeContext;
            if (currentDaScopeContext == null)
                return true;
            bool useSameConnectionString = currentDaScopeContext.ConnectionStringSettingName == ConnectionStringSettingName;
            return useSameConnectionString;
        }

        public override bool RequireNewScope
        {
            get
            {
                if (Scope.Current == null)
                    return false;
                var currentScopeHasNotTransaction = Scope.Current.GetDaScopeContext().Transaction == null;
                var requireNewTransaction = _TransactionOption.HasValue && _TransactionOption == TransactionOption.Required;
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
                throw new ObjectDisposedException("DaScopeContext object is already disposed");
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

        ~DaScopeContext()
        {
            Dispose(false);
        }

        #endregion IDisposable
    }
}
