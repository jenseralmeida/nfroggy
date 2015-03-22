using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Froggy.Data
{
    /// <summary>
    /// Encapsula um command, sem instanciar ele
    /// </summary>
    internal class DbCommandWrapper
    {
        internal const int DEFAULT_COMMAND_TIMEOUT = 30;

        #region Campos

        string _commandText;
        CommandType _commandType;
        int _commandTimeout = DEFAULT_COMMAND_TIMEOUT;
        DbCommand _command;
        private Dictionary<String, DbParameter> _parametersOutputReturnValue;

        #endregion Campos

        /// <summary>
        /// Obtem ou atribui o tempo de timeout do command
        /// </summary>
        public int CommandTimeout
        {
            get
            {
                CheckDisposed();
                if (_command != null)
                {
                    return _command.CommandTimeout;
                }
                return _commandTimeout;
            }
            set
            {
                CheckDisposed();
                if (_command != null)
                {
                    _command.CommandTimeout = value;
                }
                _commandTimeout = value;
            }
        }

        /// <summary>
        /// Obtem ou atribui <see cref="DbCommand.CommandText"/>
        /// </summary>
        public string CommandText
        {
            get
            {
                CheckDisposed();
                if (_command != null)
                {
                    return _command.CommandText;
                }
                return _commandText;
            }
            set
            {
                CheckDisposed();
                if (_command != null)
                {
                    _command.CommandText = value;
                }
                _commandText = value;
            }
        }

        public CommandType CommandType
        {
            get
            {
                CheckDisposed();
                if (_command != null)
                {
                    return _command.CommandType;
                }
                return _commandType;
            }
            set
            {
                CheckDisposed();
                if (_command != null)
                {
                    _command.CommandType = value;
                }
                _commandType = value;
            }
        }

        public Dictionary<String, DbParameter> ParametersOutputReturnValue
        {
            get
            {
                CheckDisposed();
                return _parametersOutputReturnValue;
            }
            set
            {
                CheckDisposed();
                _parametersOutputReturnValue = value;
            }
        }

        public DbCommand GetDbCommand(DaScopeContext daScopeContext)
        {
            CheckDisposed();
            if (_command == null)
            {
                _command = DbCommandUtil.CreateCommand(daScopeContext, _commandText, _commandType, _commandTimeout);
            }
            return _command;
        }

        public DbCommandWrapper(string commandText, CommandType commandType)
        {
            _commandText = commandText;
            _commandType = commandType;
            _parametersOutputReturnValue = new Dictionary<String, DbParameter>();
        }

        #region IDisposable

        private bool isDisposed;

        public void CheckDisposed()
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException("DbCommandWrapper object is already disposed");
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            // Se chamado pelo dispose libera componentes gerenciados
            if (disposing)
            {
                if (_command != null)
                    _command.Dispose();
            }
            isDisposed = true;
        }

        ~DbCommandWrapper()
        {
            Dispose(false);
        }

        #endregion IDisposable
    }
}
