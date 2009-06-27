using System;
using System.Data;
using System.Data.SqlClient;
using Froggy.Data;
using NUnit.Framework;

namespace Froggy.Test.Data
{
    [TestFixture]
    public class DAScopeContextTest
    {
        [Test]
        public void NestedWithTransactionTest()
        {
            using (var scope = new Scope(new DAScopeContext(TransactionOption.Required)))
            {
                Assert.IsNotNull(scope.GetDAScopeContext());
                Assert.IsNotNull(scope.GetDAScopeContext().Connection);
                Assert.IsNotNull(scope.GetDAScopeContext().Transaction);
                NestedLevel1WithTransactionTest(scope);
                scope.Complete();
            }
        }

        private static void NestedLevel1WithTransactionTest(Scope expected)
        {
            using (var scope = new Scope( new DAScopeContext(TransactionOption.Required) ))
            {
                Assert.AreSame(expected, Scope.Current);
                Assert.AreEqual(Scope.Current, scope);
                Assert.IsNotNull(scope.GetDAScopeContext());
                Assert.IsNotNull(scope.GetDAScopeContext().Connection);
                Assert.IsNotNull(scope.GetDAScopeContext().Transaction);
                Assert.AreSame(Scope.Current.GetDAScopeContext(), scope.GetDAScopeContext());
                Assert.AreSame(Scope.Current.GetDAScopeContext().Connection, scope.GetDAScopeContext().Connection);
                Assert.AreSame(Scope.Current.GetDAScopeContext().Transaction, scope.GetDAScopeContext().Transaction);

                scope.Complete();
            }
        }

        [Test]
        public void WithTransactionTest()
        {
            using (var scope = new Scope(new DAScopeContext(TransactionOption.Required)))
            {
                var scopeContext = scope.GetDAScopeContext();
                Assert.IsNotNull(scopeContext);
                var conn = (SqlConnection)scopeContext.Connection;
                Assert.IsNotNull(conn);
                Assert.AreEqual(ConnectionState.Open, conn.State);
                var trann = scopeContext.Transaction;
                Assert.IsNotNull(trann);

                scope.Complete();
            }
        }
        [Test]
        public void WithoutTransactionTest()
        {
            using (var scope = new Scope(new DAScopeContext()))
            {
                var scopeContext = scope.GetDAScopeContext();
                Assert.IsNotNull(scopeContext);
                var conn = (SqlConnection)scopeContext.Connection;
                Assert.IsNotNull(conn);
                Assert.AreEqual(ConnectionState.Open, conn.State);
                var trann = scopeContext.Transaction;
                Assert.IsNull(trann);

                scope.Complete();
            }
        }
    }
}
