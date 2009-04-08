using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Froggy.Data
{
    public partial class DbCommandUtil: IDisposable
    {
        private const string RETURN_PARAMETER_NAME = "__return";

        private DAScopeContext _daScopeContext;
        private Dictionary<DataAdapterCommand, DbCommandWrapper> _CommandWrappersDataAdapter;
        private DataAdapterCommand _OfDataAdapterCommand;
        private int _UpdateBatchSize;
        private DbDataAdapter _DataAdapter;

        #region IDisposable
        private bool _isDisposed;

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        private void Dipose(bool isDisposing)
        {
            
        }

        #endregion IDisposable

    }
}
