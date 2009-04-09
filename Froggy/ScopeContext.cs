using System;

namespace Froggy
{
    public abstract class ScopeContext : IDisposable
    {
        /// <summary>
        /// Indicate if this scope element vote for a new scope
        /// </summary>
        /// <returns></returns>
        public abstract bool NewScopeContextIsCompatible(ScopeContext currentScopeContext);

        /// <summary>
        /// Indicate if this scope element vote for a new scope
        /// </summary>
        /// <returns></returns>
        public abstract bool RequireNewScope { get; }

        /// <summary>
        /// Indicate if a new scope can be created
        /// </summary>
        /// <returns></returns>
        public abstract bool RefuseNewScope { get; }

        /// <summary>
        /// 
        /// </summary>
        public abstract void CompletedNow(bool completed);

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected abstract void Dispose(bool isDisposed);

        #endregion IDisposable

        public abstract void Init();
    }
}
