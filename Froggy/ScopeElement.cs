using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Froggy
{
    public abstract class ScopeElement : IDisposable
    {
        /// <summary>
        /// Indicate if this scope element vote for a new scope
        /// </summary>
        /// <returns></returns>
        public abstract bool VoteRequireNewScope { get; }

        /// <summary>
        /// Indicate if a new scope can be created
        /// </summary>
        /// <returns></returns>
        public abstract bool RefuseNewScope { get; }

        /// <summary>
        /// Code used when a Scope is setted as completed. The scope class will call the SetCompleted of all ScopeElements, only when
        /// the last SetCompleted call is made
        /// </summary>
        public abstract void SetCompleted();

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected abstract void Dispose(bool isDisposed);

        #endregion IDisposable
    }
}
