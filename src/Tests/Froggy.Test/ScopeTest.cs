using System;
using Xunit;
using System.Threading;

namespace Froggy.Test
{

    public class ScopeTest
    {
        [Fact]
        public void ScopeDispose()
        {
            var thread = new Thread(ScopeDisposeThreadStart);
            thread.Start();
            Thread.Sleep(500);
            thread.Abort();
        }
        
        private void ScopeDisposeThreadStart()
        {
            using (var scope = new Scope())
            {
            	NestedMethod(scope);
            	Thread.Sleep(2000);
                // Operations
                scope.Complete();
            }
            Assert.Null(Scope.Current);
        }

        [Fact]
        public void BasicScope()
        {
            using(var scope = new Scope())
            {
                NestedMethod(scope);
                // Operations
                scope.Complete();
            }
            Assert.Null(Scope.Current);
        }

        [Fact]
        public void EqualityOfScopeTest()
        {
            using (var scope = new Scope())
            {
                Assert.Same(Scope.Current, scope);
                Assert.Same(scope, Scope.Current);
                NestedEqualityOfScopeLevel1Test(scope);
                Assert.Same(Scope.Current, scope);
                Assert.Same(scope, Scope.Current);
                scope.Complete();
            }
            Assert.Null(Scope.Current);
        }

        [Fact]
        public void NotEqualityOfScopeTest()
        {
            Scope scope1;
            Scope scope2;
            using (scope1 = new Scope())
            {
                Assert.Same(Scope.Current, scope1);
                Assert.Same(scope1, Scope.Current);
                NestedEqualityOfScopeLevel1Test(scope1);
                Assert.Same(Scope.Current, scope1);
                Assert.Same(scope1, Scope.Current);
                Assert.Equal(Scope.Current, scope1);
                Assert.Equal(scope1, Scope.Current);

                using (scope2 = new Scope(ScopeOption.RequireNew))
                {
                    Assert.Same(Scope.Current, scope2);
                    Assert.Same(scope2, Scope.Current);
                    NestedEqualityOfScopeLevel1Test(scope2);
                    Assert.Same(Scope.Current, scope2);
                    Assert.Same(scope2, Scope.Current);

                    // Compare with the root (scope1)
                    Assert.NotEqual(Scope.Current, scope1);
                    Assert.NotEqual(scope1, Scope.Current);
                    Assert.NotEqual(scope2, scope1);
                    Assert.NotEqual(scope1, scope2);

                    scope2.Complete();
                }
                Assert.Equal(Scope.Current, scope1);
                Assert.Equal(scope1, Scope.Current);

                scope1.Complete();
            }
            Assert.Null(Scope.Current);
        }

        private void NestedEqualityOfScopeLevel1Test(Scope expected)
        {
            using (var scope = new Scope())
            {
                // They are all equals
                Assert.Equal(scope, expected);
                Assert.Equal(expected, scope);
                Assert.Equal(Scope.Current, expected);
                Assert.Equal(expected, Scope.Current);
                Assert.Equal(Scope.Current, scope);
                Assert.Equal(scope, Scope.Current);

                // They are diferent instances, but equals
                Assert.Same(Scope.Current, expected);
                Assert.Same(expected, Scope.Current);
                Assert.NotSame(scope, expected);
                Assert.NotSame(expected, scope);
                Assert.NotSame(Scope.Current, scope);
                Assert.NotSame(scope, Scope.Current);

                scope.Complete();
            }
        }

        [Fact]
        public void NestedScope()
        {
            using (var scope = new Scope())
            {
                NestedScopeLevel1(scope);
                Assert.Same(scope, Scope.Current);
                scope.Complete();
            }
            Assert.Null(Scope.Current);
        }

        private static void NestedScopeLevel1(Scope expected)
        {
            using (var scope = new Scope())
            {
                NestedMethod(expected);
                scope.Complete();
            }
        }

        private static void NestedMethod(Scope expected)
        {
            Assert.Same(expected, Scope.Current);
        }

        [Fact]
        public void CustomScopeContextBasicTest()
        {
            var csc = new CustomScopeContext(true, false, false);
            Assert.False(csc.InitCalled);
            Assert.False(csc.DisposeCalled);
            Assert.False(csc.CompleteNowCalled);
            Assert.False(csc.Completed);
            using (var scope = new Scope(csc))
            {
                Assert.True(csc.InitCalled);
                Assert.False(csc.DisposeCalled);
                Assert.False(csc.CompleteNowCalled);
                Assert.False(csc.Completed);
                scope.Complete();
                Assert.True(csc.InitCalled);
                Assert.False(csc.DisposeCalled);
                Assert.False(csc.CompleteNowCalled);
                Assert.False(csc.Completed);
            }
            Assert.True(csc.InitCalled);
            Assert.True(csc.DisposeCalled);
            Assert.True(csc.CompleteNowCalled);
            Assert.True(csc.Completed);
        }

        [Fact]
        public void CustomScopeContextNotCompleteTest()
        {
            var csc = new CustomScopeContext(true, false, false);
            Assert.False(csc.InitCalled);
            Assert.False(csc.DisposeCalled);
            Assert.False(csc.CompleteNowCalled);
            Assert.False(csc.Completed);
            using (new Scope(csc))
            {
                Assert.True(csc.InitCalled);
                Assert.False(csc.DisposeCalled);
                Assert.False(csc.CompleteNowCalled);
                Assert.False(csc.Completed);
            }
            Assert.True(csc.InitCalled);
            Assert.True(csc.DisposeCalled);
            Assert.True(csc.CompleteNowCalled);
            Assert.False(csc.Completed);
        }

        [Fact]
        public void CustomNestedScopeContextBasicTest()
        {
            var csc = new CustomScopeContext(true, false, false);
            Assert.False(csc.InitCalled);
            Assert.False(csc.DisposeCalled);
            Assert.False(csc.CompleteNowCalled);
            Assert.False(csc.Completed);
            using (var scope = new Scope(csc))
            {
                CustomNestedScopeContextBasicLevel1(scope);
                Assert.Same(scope, Scope.Current);
                Assert.NotNull(Scope.Current.GetScopeContext<CustomScopeContext>());
                Assert.Same(csc, Scope.Current.GetScopeContext<CustomScopeContext>());

                Assert.True(csc.InitCalled);
                Assert.False(csc.DisposeCalled);
                Assert.False(csc.CompleteNowCalled);
                Assert.False(csc.Completed);
                scope.Complete();
                Assert.True(csc.InitCalled);
                Assert.False(csc.DisposeCalled);
                Assert.False(csc.CompleteNowCalled);
                Assert.False(csc.Completed);
            }
            Assert.True(csc.InitCalled);
            Assert.True(csc.DisposeCalled);
            Assert.True(csc.CompleteNowCalled);
            Assert.True(csc.Completed);
            Assert.Null(Scope.Current);
        }


        //[Fact]
        //public void CustomNestedScopeContextNotCompleteTest()
        //{
        //    var csc = new CustomScopeContext(true, false, false);
        //    Assert.False(csc.InitCalled);
        //    Assert.False(csc.DisposeCalled);
        //    Assert.False(csc.CompleteNowCalled);
        //    Assert.False(csc.Completed);
        //    using (var scope = new Scope(csc))
        //    {
        //        Assert.True(csc.InitCalled);
        //        Assert.False(csc.DisposeCalled);
        //        Assert.False(csc.CompleteNowCalled);
        //        Assert.False(csc.Completed);
        //    }
        //    Assert.True(csc.InitCalled);
        //    Assert.True(csc.DisposeCalled);
        //    Assert.True(csc.CompleteNowCalled);
        //    Assert.False(csc.Completed);
        //}

        private static void CustomNestedScopeContextBasicLevel1(Scope expected)
        {
            var csc = new CustomScopeContext(true, false, false);
            Assert.False(csc.InitCalled);
            Assert.False(csc.DisposeCalled);
            Assert.False(csc.CompleteNowCalled);
            Assert.False(csc.Completed);
            using (var scope = new Scope(csc))
            {
                Assert.NotSame(scope, Scope.Current);
                Assert.Equal(scope, Scope.Current);
                Assert.Equal(Scope.Current, scope);
                Assert.NotNull(Scope.Current.GetScopeContext<CustomScopeContext>());
                csc = Scope.Current.GetScopeContext<CustomScopeContext>();

                Assert.True(csc.InitCalled);
                Assert.False(csc.DisposeCalled);
                Assert.False(csc.CompleteNowCalled);
                Assert.False(csc.Completed);
                NestedMethod(expected);
                scope.Complete();
                Assert.True(csc.InitCalled);
                Assert.False(csc.DisposeCalled);
                Assert.False(csc.CompleteNowCalled);
                Assert.False(csc.Completed);
            }
            Assert.True(csc.InitCalled);
            Assert.False(csc.DisposeCalled);
            Assert.False(csc.CompleteNowCalled);
            Assert.False(csc.Completed);
            Assert.NotNull(Scope.Current);
        }
    }

    public class CustomScopeContext: ScopeContext
    {
        private readonly bool _Compatible;
        private readonly bool _RequireNewScope;
        private readonly bool _RefuseNewScope;
        private bool _InitCalled;
        private bool _CompleteNowCalled;
        private bool _Completed;
        private bool _DisposeCalled;

        public CustomScopeContext(bool compatible, bool requireNewScope, bool refuseNewScope)
        {
            _Compatible = compatible;
            _RefuseNewScope = refuseNewScope;
            _RequireNewScope = requireNewScope;
        }

        #region Overrides of ScopeContext

        public bool DisposeCalled
        {
            get { return _DisposeCalled; }
        }

        public bool CompleteNowCalled
        {
            get { return _CompleteNowCalled; }
        }

        public bool InitCalled
        {
            get { return _InitCalled; }
        }

        /// <summary>
        /// Indicate if this scope element vote for a new scope
        /// </summary>
        /// <returns></returns>
        public override bool NewScopeContextIsCompatible(ScopeContext currentScopeContext)
        {
            return _Compatible;
        }

        /// <summary>
        /// Indicate if this scope element vote for a new scope
        /// </summary>
        /// <returns></returns>
        public override bool RequireNewScope
        {
            get { return _RequireNewScope; }
        }

        /// <summary>
        /// Indicate if a new scope can be created
        /// </summary>
        /// <returns></returns>
        public override bool RefuseNewScope
        {
            get { return _RefuseNewScope; }
        }

        public bool Completed
        {
            get { return _Completed; }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void CompletedNow(bool completed)
        {
            _CompleteNowCalled = true;
            _Completed = completed;
            Console.WriteLine("complete now call");
        }

        protected override void Dispose(bool isDisposed)
        {
            _DisposeCalled = true;
            Console.WriteLine("Dispose call");
        }

        public override void Init()
        {
            _InitCalled = true;
            Console.WriteLine("Init call");
        }

        #endregion
    }
}
