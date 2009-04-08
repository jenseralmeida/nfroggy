using System;

namespace Froggy.Data
{
    public class DbCommandUtil: IDisposable
    {
        private const string RETURN_PARAMETER_NAME = "__return";

        #region IDisposable

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
