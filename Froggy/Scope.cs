using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Froggy
{
    public class Scope : IDisposable
    {
        #region Class Elements

        [ThreadStatic]
        private static Stack<Scope> _ScopeStack;


        [ThreadStatic]
        private static Scope _Current;

        private static Stack<Scope> ScopeStack
        {
            get
            {
                if (_ScopeStack == null)
                {
                    _ScopeStack = new Stack<Scope>();
                }
                return _ScopeStack;
            }
        }

        private static void Pop()
        {
            // Pop scope's from stack until find one not yet disposed
            while ((ScopeStack.Count > 0) && (Current._IsDisposed))
            {
                _Current = ScopeStack.Pop();
            }
            if (ScopeStack.Count == 0)
            {
                _Current = null;
            }
        }

        private static void Push(Scope scope)
        {
            ScopeStack.Push(scope);
            _Current = scope;
        }

        public static Scope Current
        {
            get
            {
                return _Current;
            }
        }

        #endregion Class Elements

        public Scope(params ScopeContext[] contexts)
        {
            if (IsNewScopeRequired())
            {
                _ScopeContexts = new Dictionary<Type, ScopeContext>();
                foreach (var newScopeElement in contexts)
                {
                    var scopeElementType = newScopeElement.GetType();
                    if (_ScopeContexts.ContainsKey(scopeElementType))
                    {
                        var currentScopeElement = _ScopeContexts[scopeElementType];
                        currentScopeElement.NewScopeContextIsCompatible(newScopeElement);
                    }
                    _ScopeContexts.Add(scopeElementType, newScopeElement);
                }
                PushInstanceInStack();
            }
            else
            {
                // Supress destructor if this instance will not be used
                GC.SuppressFinalize(true);
                InstanceCount = Current.InstanceCount + 1;
            }
        }

        private readonly Dictionary<Type, ScopeContext> _ScopeContexts;
        private int _InstanceCount;

        private Dictionary<Type, ScopeContext> ScopeContexts
        {
            get { return Current._ScopeContexts; }
        }

        private int InstanceCount
        {
            get { return Current._InstanceCount; }
            set { Current._InstanceCount = value; }
        }

        private bool Completed
        {
            get { return Current._Completed; }
            set { Current._Completed = value; }
        }

        public void Complete()
        {
            CheckDisposed();
            if (InstanceCount == 1)
            {
                Completed = true;
            }
        }

        #region Override equality operation

        public override bool Equals(object obj)
        {
            CheckDisposed();
            return ReferenceEquals(Current, obj);
        }

        public override int GetHashCode()
        {
            CheckDisposed();
            return Current.GetHashCode();
        }

        public static bool operator ==(Scope left, Scope right)
        {
            // Use ReferenceEqual to do not generate recursive call
            bool leftNull = ReferenceEquals(left, null);
            bool rightNull = ReferenceEquals(right, null);
            // Compare null values
            bool bothAreNull = leftNull && rightNull;
            if (bothAreNull) return true;
            bool someIsNullButNotBoth = (leftNull || rightNull);
            if (someIsNullButNotBoth) return false;
            // Compare values left to right and right to left, this is to make both order return the same logical value
            return left.Equals(right) && right.Equals(left);
        }

        public static bool operator !=(Scope left, Scope right)
        {
            return !(left == right);
        }

        #endregion Override equality operation

        #region Scope control

        public T GetScopeContext<T>() where T : ScopeContext
        {
            CheckDisposed();
            var type = typeof(T);
            return ScopeContexts.ContainsKey(type) ? (T)ScopeContexts[type] : null;
        }

        private bool IsNewScopeRequired()
        {
            bool existsActiveScope = _Current != null;
            if (!existsActiveScope)
            {
                return true;
            }
            // Verify if any scope element vote for a new scope
            bool anyElementVoteForNewScope = new List<ScopeContext>(ScopeContexts.Values).Exists(se => se.RequireNewScope);
            if (anyElementVoteForNewScope)
            {
                bool anyElementsRefuseNewScope = new List<ScopeContext>(ScopeContexts.Values).Exists(se => se.RefuseNewScope);
                if (anyElementsRefuseNewScope)
                {
                    throw new InvalidOperationException(
                        "Some elements are in a state that they refuse the creation of a new scope");
                }
                return true;
            }
            return false;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void PushInstanceInStack()
        {
            Push(this);
            InstanceCount = 1;
            //Iniyialize all scope contexts
            Array.ForEach(ScopeContexts.Values.ToArray(), (scopeContext => scopeContext.Init()) );

        }

        #endregion Scope control

        #region IDisposable

        private bool _IsDisposed;
        private bool _Completed;

        private void CheckDisposed()
        {
            if (_IsDisposed)
            {
                throw new ObjectDisposedException("Scope object is already disposed");
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            bool canFinalize = (!_IsDisposed) &&  ( (Current == null) || (--InstanceCount < 1) );
            if (canFinalize)
            {
                bool thisIsCurrentScope = Current == this;
                if (thisIsCurrentScope)
                {
                    _IsDisposed = true;
                    // Dispose all scope contexts
                    foreach (var scopeContext in ScopeContexts.Values)
                    {
                        scopeContext.CompletedNow(Completed);
                        scopeContext.Dispose();
                    }
                    Pop();
                }
            }
        }

        ~Scope()
        {
            Dispose(false);
        }

        #endregion IDisposable
    }
}