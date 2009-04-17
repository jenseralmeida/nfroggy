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
