using System;
using System.Data;
using System.Data.SqlClient;
using Froggy.Data;
using NUnit.Framework;

namespace Froggy.Test.Data
{
    [TestFixture]
    public class DaScopeContextTest
    {
        [Test]
        public void GetDaScopeContextWhenItDoesNotExist()
        {
            var daScopeContextWithoutScope = Scope.Current.GetDaScopeContext();

            using (var scope = new Scope())
            {
                var daScopeContextWithScope = scope.GetDaScopeContext();
                Assert.IsNull(daScopeContextWithScope);
                scope.Complete();
            }
        }

        [Test]
        public void NestedWithTransactionTest()
        {
            using (var scope = new Scope(new DaScopeContext(TransactionOption.Required)))
            {
                Assert.IsNotNull(scope.GetDaScopeContext());
                Assert.IsNotNull(scope.GetDaScopeContext().Connection);
                Assert.IsNotNull(scope.GetDaScopeContext().Transaction);
                NestedLevel1WithTransactionTest(scope);
                scope.Complete();
            }
        }

        private static void NestedLevel1WithTransactionTest(Scope expected)
        {
            using (var scope = new Scope( new DaScopeContext(TransactionOption.Required) ))
            {
                Assert.AreSame(expected, Scope.Current);
                Assert.AreEqual(Scope.Current, scope);
                Assert.IsNotNull(scope.GetDaScopeContext());
                Assert.IsNotNull(scope.GetDaScopeContext().Connection);
                Assert.IsNotNull(scope.GetDaScopeContext().Transaction);
                Assert.AreSame(Scope.Current.GetDaScopeContext(), scope.GetDaScopeContext());
                Assert.AreSame(Scope.Current.GetDaScopeContext().Connection, scope.GetDaScopeContext().Connection);
                Assert.AreSame(Scope.Current.GetDaScopeContext().Transaction, scope.GetDaScopeContext().Transaction);

                scope.Complete();
            }
        }

        [Test]
        public void WithTransactionTest()
        {
            using (var scope = new Scope(new DaScopeContext(TransactionOption.Required)))
            {
                var scopeContext = scope.GetDaScopeContext();
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
            using (var scope = new Scope(new DaScopeContext()))
            {
                var scopeContext = scope.GetDaScopeContext();
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
