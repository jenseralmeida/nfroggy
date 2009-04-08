using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Froggy
{
    public class TransactionScopeContext : ScopeContext
    {
        public static ScopeContext SetUp()
        {
            return new TransactionScopeContext();
        }

        public static ScopeContext SetUp(TransactionOption transactionOption)
        {
            return new TransactionScopeContext(transactionOption);
        }

        private TransactionOption transactionOption;

        public TransactionScopeContext()
        {
            
            
        }

        public TransactionScopeContext(TransactionOption transactionOption)
        {
            this.transactionOption = transactionOption;
        }

        #region ScopeElement
        public override bool NewScopeContextIsCompatible(ScopeContext newScopeElement)
        {
            throw new System.NotImplementedException(); 
        }


        public override bool RequireNewScope
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
}
