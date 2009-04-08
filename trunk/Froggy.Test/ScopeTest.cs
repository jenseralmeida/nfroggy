using NUnit.Framework;

namespace Froggy.Test
{
    [TestFixture]
    public class ScopeTest
    {
        [Test]
        public void BasicScope()
        {
            using(var scope = new Scope())
            {
                NestedMethod(scope);
                // Operations
                scope.Complete();
            }
            Assert.IsNull(Scope.Current);
        }

        [Test]
        public void NestedScope()
        {
            using (var scope = new Scope())
            {
                NestedScopeLevel1(scope);
                Assert.AreSame(scope, Scope.Current);
                scope.Complete();
            }
            Assert.IsNull(Scope.Current);
        }

        private void NestedScopeLevel1(Scope expected)
        {
            using (var scope = new Scope())
            {
                NestedMethod(expected);
                scope.Complete();
            }
        }

        private static void NestedMethod(Scope expected)
        {
            Assert.AreSame(expected, Scope.Current);
        }
    }
}
