using System;
using System.Data.SqlClient;
using System.IO;

namespace Froggy.Test
{
    public class DabaseTestsBase : IDisposable
    {
        TestDatabaseManager _testDatabaseManager;

        public DabaseTestsBase()
        {
            _testDatabaseManager = new TestDatabaseManager("TestDatabase");
            _testDatabaseManager.UpdateConnectionStringSettings("Default");
            _testDatabaseManager.CreateDatabase();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_testDatabaseManager != null)
                        _testDatabaseManager.Dispose();
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
