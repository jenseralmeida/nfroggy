using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Froggy.Test
{
    class ScopeTest
    {
        public void BasicScope()
        {

            using(var scope = new Scope(new TransactionScopeElement()))
            {
                // Operations
                scope.SetCompleted();
            }
        }
    }
}
