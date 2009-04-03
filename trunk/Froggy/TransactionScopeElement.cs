using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Froggy
{
    public class TransactionScopeElement : ScopeElement
    {
        public static ScopeElement SetUp()
        {
            return new TransactionScopeElement();
        }

        public static ScopeElement SetUp(TransactionNeed transactionNeed)
        {
            return new TransactionScopeElement(transactionNeed);
        }

        private TransactionNeed transactionNeed;

        public TransactionScopeElement()
        {
            
            
        }

        public TransactionScopeElement(TransactionNeed transactionNeed)
        {
            this.transactionNeed = transactionNeed;
        }

        #region ScopeElement
        public override bool NewScopeElementIsCompatible(ScopeElement newScopeElement)
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
