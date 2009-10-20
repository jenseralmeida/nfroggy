using System;
using NUnit.Framework;
using System.Threading;

namespace Froggy.Test
{
    [TestFixture]
    public class ScopeTest
    {
        [Test]
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
            Assert.IsNull(Scope.Current);
        }

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
        public void EqualityOfScopeTest()
        {
            using (var scope = new Scope())
            {
                Assert.AreSame(Scope.Current, scope);
                Assert.AreSame(scope, Scope.Current);
                NestedEqualityOfScopeLevel1Test(scope);
                Assert.AreSame(Scope.Current, scope);
                Assert.AreSame(scope, Scope.Current);
                scope.Complete();
            }
            Assert.IsNull(Scope.Current);
        }

        [Test]
        public void NotEqualityOfScopeTest()
        {
            Scope scope1;
            Scope scope2;
            using (scope1 = new Scope())
            {
                Assert.AreSame(Scope.Current, scope1);
                Assert.AreSame(scope1, Scope.Current);
                NestedEqualityOfScopeLevel1Test(scope1);
                Assert.AreSame(Scope.Current, scope1);
                Assert.AreSame(scope1, Scope.Current);
                Assert.AreEqual(Scope.Current, scope1);
                Assert.AreEqual(scope1, Scope.Current);

                using (scope2 = new Scope(ScopeOption.RequireNew))
                {
                    Assert.AreSame(Scope.Current, scope2);
                    Assert.AreSame(scope2, Scope.Current);
                    NestedEqualityOfScopeLevel1Test(scope2);
                    Assert.AreSame(Scope.Current, scope2);
                    Assert.AreSame(scope2, Scope.Current);

                    // Compare with the root (scope1)
                    Assert.AreNotEqual(Scope.Current, scope1);
                    Assert.AreNotEqual(scope1, Scope.Current);
                    Assert.AreNotEqual(scope2, scope1);
                    Assert.AreNotEqual(scope1, scope2);

                    scope2.Complete();
                }
                Assert.AreEqual(Scope.Current, scope1);
                Assert.AreEqual(scope1, Scope.Current);

                scope1.Complete();
            }
            Assert.IsNull(Scope.Current);
        }

        private void NestedEqualityOfScopeLevel1Test(Scope expected)
        {
            using (var scope = new Scope())
            {
                // They are all equals
                Assert.AreEqual(scope, expected);
                Assert.AreEqual(expected, scope);
                Assert.AreEqual(Scope.Current, expected);
                Assert.AreEqual(expected, Scope.Current);
                Assert.AreEqual(Scope.Current, scope);
                Assert.AreEqual(scope, Scope.Current);

                // They are diferent instances, but equals
                Assert.AreSame(Scope.Current, expected);
                Assert.AreSame(expected, Scope.Current);
                Assert.AreNotSame(scope, expected);
                Assert.AreNotSame(expected, scope);
                Assert.AreNotSame(Scope.Current, scope);
                Assert.AreNotSame(scope, Scope.Current);

                scope.Complete();
            }
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
            Assert.AreSame(expected, Scope.Current);
        }

        [Test]
        public void CustomScopeContextBasicTest()
        {
            var csc = new CustomScopeContext(true, false, false);
            Assert.IsFalse(csc.InitCalled);
            Assert.IsFalse(csc.DisposeCalled);
            Assert.IsFalse(csc.CompleteNowCalled);
            Assert.IsFalse(csc.Completed);
            using (var scope = new Scope(csc))
            {
                Assert.IsTrue(csc.InitCalled);
                Assert.IsFalse(csc.DisposeCalled);
                Assert.IsFalse(csc.CompleteNowCalled);
                Assert.IsFalse(csc.Completed);
                scope.Complete();
                Assert.IsTrue(csc.InitCalled);
                Assert.IsFalse(csc.DisposeCalled);
                Assert.IsFalse(csc.CompleteNowCalled);
                Assert.IsFalse(csc.Completed);
            }
            Assert.IsTrue(csc.InitCalled);
            Assert.IsTrue(csc.DisposeCalled);
            Assert.IsTrue(csc.CompleteNowCalled);
            Assert.IsTrue(csc.Completed);
        }

        [Test]
        public void CustomScopeContextNotCompleteTest()
        {
            var csc = new CustomScopeContext(true, false, false);
            Assert.IsFalse(csc.InitCalled);
            Assert.IsFalse(csc.DisposeCalled);
            Assert.IsFalse(csc.CompleteNowCalled);
            Assert.IsFalse(csc.Completed);
            using (new Scope(csc))
            {
                Assert.IsTrue(csc.InitCalled);
                Assert.IsFalse(csc.DisposeCalled);
                Assert.IsFalse(csc.CompleteNowCalled);
                Assert.IsFalse(csc.Completed);
            }
            Assert.IsTrue(csc.InitCalled);
            Assert.IsTrue(csc.DisposeCalled);
            Assert.IsTrue(csc.CompleteNowCalled);
            Assert.IsFalse(csc.Completed);
        }

        [Test]
        public void CustomNestedScopeContextBasicTest()
        {
            var csc = new CustomScopeContext(true, false, false);
            Assert.IsFalse(csc.InitCalled);
            Assert.IsFalse(csc.DisposeCalled);
            Assert.IsFalse(csc.CompleteNowCalled);
            Assert.IsFalse(csc.Completed);
            using (var scope = new Scope(csc))
            {
                CustomNestedScopeContextBasicLevel1(scope);
                Assert.AreSame(scope, Scope.Current);
                Assert.IsNotNull(Scope.Current.GetScopeContext<CustomScopeContext>());
                Assert.AreSame(csc, Scope.Current.GetScopeContext<CustomScopeContext>());

                Assert.IsTrue(csc.InitCalled);
                Assert.IsFalse(csc.DisposeCalled);
                Assert.IsFalse(csc.CompleteNowCalled);
                Assert.IsFalse(csc.Completed);
                scope.Complete();
                Assert.IsTrue(csc.InitCalled);
                Assert.IsFalse(csc.DisposeCalled);
                Assert.IsFalse(csc.CompleteNowCalled);
                Assert.IsFalse(csc.Completed);
            }
            Assert.IsTrue(csc.InitCalled);
            Assert.IsTrue(csc.DisposeCalled);
            Assert.IsTrue(csc.CompleteNowCalled);
            Assert.IsTrue(csc.Completed);
            Assert.IsNull(Scope.Current);
        }


        //[Test]
        //public void CustomNestedScopeContextNotCompleteTest()
        //{
        //    var csc = new CustomScopeContext(true, false, false);
        //    Assert.IsFalse(csc.InitCalled);
        //    Assert.IsFalse(csc.DisposeCalled);
        //    Assert.IsFalse(csc.CompleteNowCalled);
        //    Assert.IsFalse(csc.Completed);
        //    using (var scope = new Scope(csc))
        //    {
        //        Assert.IsTrue(csc.InitCalled);
        //        Assert.IsFalse(csc.DisposeCalled);
        //        Assert.IsFalse(csc.CompleteNowCalled);
        //        Assert.IsFalse(csc.Completed);
        //    }
        //    Assert.IsTrue(csc.InitCalled);
        //    Assert.IsTrue(csc.DisposeCalled);
        //    Assert.IsTrue(csc.CompleteNowCalled);
        //    Assert.IsFalse(csc.Completed);
        //}

        private static void CustomNestedScopeContextBasicLevel1(Scope expected)
        {
            var csc = new CustomScopeContext(true, false, false);
            Assert.IsFalse(csc.InitCalled);
            Assert.IsFalse(csc.DisposeCalled);
            Assert.IsFalse(csc.CompleteNowCalled);
            Assert.IsFalse(csc.Completed);
            using (var scope = new Scope(csc))
            {
                Assert.AreNotSame(scope, Scope.Current);
                Assert.AreEqual(scope, Scope.Current);
                Assert.AreEqual(Scope.Current, scope);
                Assert.IsNotNull(Scope.Current.GetScopeContext<CustomScopeContext>());
                csc = Scope.Current.GetScopeContext<CustomScopeContext>();

                Assert.IsTrue(csc.InitCalled);
                Assert.IsFalse(csc.DisposeCalled);
                Assert.IsFalse(csc.CompleteNowCalled);
                Assert.IsFalse(csc.Completed);
                NestedMethod(expected);
                scope.Complete();
                Assert.IsTrue(csc.InitCalled);
                Assert.IsFalse(csc.DisposeCalled);
                Assert.IsFalse(csc.CompleteNowCalled);
                Assert.IsFalse(csc.Completed);
            }
            Assert.IsTrue(csc.InitCalled);
            Assert.IsFalse(csc.DisposeCalled);
            Assert.IsFalse(csc.CompleteNowCalled);
            Assert.IsFalse(csc.Completed);
            Assert.IsNotNull(Scope.Current);
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
