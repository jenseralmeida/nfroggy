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
            return scope.GetScopeElement<DAScopeContext>();
		}
    }
    /// <summary>
    /// Scope context to Data Access
    /// </summary>
    public class DAScopeContext: ScopeContext, IDisposable
    {
        public const string DEFAULT_ConnectionStringSettingName = "Default";

        private string _connectionStringSettingName;
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
        /// Construtor usando configuração de string de conexao customizada, independente de app.config
        /// </summary>
        /// <param name="providerName">Nome do provider a ser usado</param>
        /// <param name="connectionString">String de conexao</param>
        public DAScopeContext(string providerName, string connectionString)
        {
            if (String.IsNullOrEmpty(providerName) || String.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("ProviderName e ConnectionString precisam conter um valor");
            }
            _providerFactory = DbProviderFactories.GetFactory(providerName);
            _connectionStringSettingName = String.Empty;
            _connection = ProviderFactory.CreateConnection();
            _connection.ConnectionString = connectionString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionStringSettingName"></param>
        public DAScopeContext(string connectionStringSettingName)
        {
            ConnectionStringSettings connectionStringSettings = ConfigurationManager.ConnectionStrings[connectionStringSettingName];
            if (connectionStringSettings == null)
            {
                throw new InvalidOperationException("A conexão especificada não existe em <connectionString> em <configuration>. Se nenhum conexão foi especificada a conexão Geral precisa estar definida");
            }
            _providerFactory = DbProviderFactories.GetFactory(connectionStringSettings.ProviderName);
            _connectionStringSettingName = connectionStringSettings.Name;
            _connection = ProviderFactory.CreateConnection();
            _connection.ConnectionString = connectionStringSettings.ConnectionString;
        }

        /// <summary>
        /// Contrutor para string de conexão padrão
        /// </summary>
        public DAScopeContext()
            : this(DEFAULT_ConnectionStringSettingName)
        {
        }

        #endregion Constructor

        #region IDisposable

        private bool isDisposed;

        private void CheckDisposed()
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException("DAScopeContext object is already disposed");
            }
        }

        public override bool NewScopeContextIsCompatible(ScopeContext newScopeElement)
        {
            throw new NotImplementedException();
        }

        public override bool RequireNewScope
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RefuseNewScope
        {
            get { throw new NotImplementedException(); }
        }

        public override void SetCompleted()
        {
            throw new NotImplementedException();
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
