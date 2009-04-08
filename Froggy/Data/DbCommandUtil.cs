using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Froggy.Data
{
    public partial class DbCommandUtil: IDisposable
    {
        private const string RETURN_PARAMETER_NAME = "__return";

        private DAScopeContext _daScopeContext;
        private DbDataAdapter _dataAdapter;
        private Dictionary<DataAdapterCommand, DbCommandWrapper> _CommandWrappersDataAdapter;
        private DataAdapterCommand _dataAdapterCommand;
        private int _updateBatchSize;

        #region IDisposable

        private bool _isDisposed;

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
            bool canDispose = _isDisposed && isDisposing;
            if (canDispose)
            {
                
            }
            _isDisposed = true;
        }

        #endregion IDisposable

    }
}
