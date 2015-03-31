using System.Data;
using System.Data.SqlClient;
using Froggy.Data;
using Xunit;

namespace Froggy.Test.Data
{

    public class DaScopeContextTest : DabaseTestsBase
    {
        [Fact]
        public void GetDaScopeContextWhenItDoesNotExist()
        {
            var daScopeContextWithoutScope = Scope.Current.GetDaScopeContext();
            using (var scope = new Scope())
            {
                var daScopeContextWithScope = scope.GetDaScopeContext();
                Assert.Null(daScopeContextWithScope);
                scope.Complete();
            }
        }

        [Fact]
        public void NestedWithTransactionTest()
        {
            using (var scope = new Scope(new DaScopeContext(TransactionOption.Required)))
            {
                Assert.NotNull(scope.GetDaScopeContext());
                Assert.NotNull(scope.GetDaScopeContext().Connection);
                Assert.NotNull(scope.GetDaScopeContext().Transaction);
                NestedLevel1WithTransactionTest(scope);
                scope.Complete();
            }
        }

        private static void NestedLevel1WithTransactionTest(Scope expected)
        {
            using (var scope = new Scope( new DaScopeContext(TransactionOption.Required) ))
            {
                Assert.Same(expected, Scope.Current);
                Assert.Equal(Scope.Current, scope);
                Assert.NotNull(scope.GetDaScopeContext());
                Assert.NotNull(scope.GetDaScopeContext().Connection);
                Assert.NotNull(scope.GetDaScopeContext().Transaction);
                Assert.Same(Scope.Current.GetDaScopeContext(), scope.GetDaScopeContext());
                Assert.Same(Scope.Current.GetDaScopeContext().Connection, scope.GetDaScopeContext().Connection);
                Assert.Same(Scope.Current.GetDaScopeContext().Transaction, scope.GetDaScopeContext().Transaction);

                scope.Complete();
            }
        }

        [Fact]
        public void WithTransactionTest()
        {
            using (var scope = new Scope(new DaScopeContext(TransactionOption.Required)))
            {
                var scopeContext = scope.GetDaScopeContext();
                Assert.NotNull(scopeContext);
                var conn = (SqlConnection)scopeContext.Connection;
                Assert.NotNull(conn);
                Assert.Equal(ConnectionState.Open, conn.State);
                var trann = scopeContext.Transaction;
                Assert.NotNull(trann);

                scope.Complete();
            }
        }
        [Fact]
        public void WithoutTransactionTest()
        {
            using (var scope = new Scope(new DaScopeContext()))
            {
                var scopeContext = scope.GetDaScopeContext();
                Assert.NotNull(scopeContext);
                var conn = (SqlConnection)scopeContext.Connection;
                Assert.NotNull(conn);
                Assert.Equal(ConnectionState.Open, conn.State);
                var trann = scopeContext.Transaction;
                Assert.Null(trann);

                scope.Complete();
            }
        }
    }
}
