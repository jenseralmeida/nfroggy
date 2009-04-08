using System;
using System.Data;
using System.Data.Common;

namespace Froggy.Data
{
    /// <summary>
    /// Scope context to Data Access
    /// </summary>
    public class DAScopeContext: IDisposable
    {
        public const string _DEFAULT_CONNECTION_NAME = "Default";

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

        #region IDisposable

        private bool isDisposed;

        private void CheckDisposed()
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException("DAScopeContext object is already disposed");
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
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
