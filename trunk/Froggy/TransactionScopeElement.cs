using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Froggy
{
    public class TransactionScopeElement : ScopeElement
    {

        #region ScopeElement
        public override bool VoteRequireNewScope
        {
            get { throw new System.NotImplementedException(); }
        }

        public override bool RefuseNewScope
        {
            get { throw new System.NotImplementedException(); }
        }

        protected override void Dispose(bool isDisposed)
        {
            throw new System.NotImplementedException();
        }

        public override void SetCompleted()
        {
            throw new System.NotImplementedException();
        }

        #endregion ScopeElement
    }

    [Serializable]
    public static class TransactionScopeElementExtension
    {
        public static Scope SetTransactionNeed(this Scope scope, TransactionNeed transactionNeed)
        {
            return scope;
        }

        public static Scope SetTransactionNeed(this Scope scope)
        {
            return SetTransactionNeed(scope, TransactionNeed.Required);
        }
    }
}
