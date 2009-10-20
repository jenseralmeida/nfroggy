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

        public static Scope Current
        {
            get
            {
                return _Current;
            }
        }

        #endregion Class Elements

        public Scope(ScopeOption scopeOption, params ScopeContext[] contexts)
        {
            _ScopeOption = scopeOption;
            if (IsNewScopeRequired(scopeOption))
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
                ConfigCurrentScope();
            }
            else
            {
                // Supress destructor if this instance will not be used
                GC.SuppressFinalize(true);
                InstanceCount = InstanceCount + 1;
            }
            // At end of ctor the current static instance, is the scope used to equality
            _CurrentScopeToThisInstance = Current;
        }

        public Scope(params ScopeContext[] contexts)
            : this(ScopeOption.Automatic, contexts)
        {
            
        }

        private readonly Dictionary<Type, ScopeContext> _ScopeContexts;
        private int _InstanceCount;
        private readonly ScopeOption _ScopeOption;

        public ScopeOption ScopeOption
        {
            get 
            { 
                CheckDisposed();
                return Current._ScopeOption; 
            }
        }

        private Dictionary<Type, ScopeContext> ScopeContexts
        {
            get
            {
                CheckDisposed();
                return Current._ScopeContexts;
            }
        }

        private int InstanceCount
        {
            get
            {
                CheckDisposed();
                return Current._InstanceCount;
            }
            set
            {
                CheckDisposed();
                Current._InstanceCount = value;
            }
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
            var currentScopeToThisInstanceNull = ReferenceEquals(_CurrentScopeToThisInstance, null);
            if (currentScopeToThisInstanceNull)
                throw new InvalidOperationException("A unexpected status in scope was found, please contact us in http://code.google.com/p/nfroggy and open a issue with a test code to reproduce the trouble.");
            var objNull = ReferenceEquals(obj, null);
            if (objNull) // _CurrentScopeToThisInstance always will be not null, if obj is null then return false
                return false;
            return (obj is Scope) && ReferenceEquals(_CurrentScopeToThisInstance, ((Scope)obj)._CurrentScopeToThisInstance);
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

        private bool IsNewScopeRequired(ScopeOption scopeOption)
        {
            bool existsActiveScope = _Current != null;
            if (!existsActiveScope || scopeOption == ScopeOption.RequireNew)
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
        private void ConfigCurrentScope()
        {
            if (_Current != null)
            {
                // if exists current scope, push it in stack
                ScopeStack.Push(_Current);
            }
            _Current = this;
            InstanceCount = 1;
            //Iniyialize all scope contexts
            Array.ForEach(ScopeContexts.Values.ToArray(), (scopeContext => scopeContext.Init()) );

        }

        #endregion Scope control

        #region IDisposable

        private bool _IsDisposed;
        private bool _Completed;
        private readonly Scope _CurrentScopeToThisInstance;

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
                    var scopeContexts = ScopeContexts.Values;
                    var completed = Completed;
                    ConfigScopeToPrevious();
                    _IsDisposed = true;
                    // Dispose all scope contexts
                    foreach (var scopeContext in scopeContexts)
                    {
                        scopeContext.CompletedNow(completed);
                        scopeContext.Dispose();
                    }
                }
            }
        }

        private static void ConfigScopeToPrevious()
        {
            // If there is no scope in stack, then clear _Current scope only
            if (ScopeStack.Count == 0)
            {
                _Current = null;
                return;
            }
            // Pop from stack and make then the Current scope
            _Current = ScopeStack.Pop();
            // Continue pop scope's from stack until find one not yet disposed
            while ((ScopeStack.Count > 0) && (_Current._IsDisposed))
            {
                _Current = ScopeStack.Pop();
            }
        }
        //private static void Pop()
        //{
        //    // Pop scope's from stack until find one not yet disposed
        //    while ((ScopeStack.Count > 0) && (Current._IsDisposed))
        //    {
        //        _Current = ScopeStack.Pop();
        //    }
        //    if (_Current != null && _Current._IsDisposed)
        //    {
        //        _Current = null;
        //    }
        //}

        ~Scope()
        {
            Dispose(false);
        }

        #endregion IDisposable
    }
}