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
        private static Stack<Scope> scopeStack;

        public static Scope Current
        {
            get
            {
                return scopeStack.Count > 0 ? scopeStack.Peek() : null;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        static Scope()
        {
            scopeStack = new Stack<Scope>();
        }

        #endregion Class Elements

        public Scope(params ScopeElement[] elements)
        {
            scopeElements = new Dictionary<Type, ScopeElement>();
            if (IsNewScopeRequired())
            {
                PushInstanceInStack();
            }
            else
            {
                Current._InstanceCount++;
            }

            foreach (var element in elements)
            {
                AddScopeElement(element);
            }
        }

        private readonly Dictionary<Type, ScopeElement> scopeElements;
        private int _InstanceCount;

        public void Complete()
        {
            foreach (var scopeElement in scopeElements.Values)
            {
                scopeElement.SetCompleted();
            }
        }


        private void AddScopeElement(ScopeElement newScopeElement)
        {
            var scopeElementType = typeof(ScopeElement);
            if (scopeElements.ContainsKey(scopeElementType))
            {
                var currentScopeElement = scopeElements[scopeElementType];
                currentScopeElement.NewScopeElementIsCompatible(newScopeElement);
            }
            scopeElements.Add(scopeElementType, newScopeElement);
        }

        internal T GetScopeElement<T>() where T : ScopeElement
        {
            var type = typeof(T);
            if (scopeElements.ContainsKey(type))
            {
                return (T)scopeElements[type];
            }
            return null;
        }

        private bool IsNewScopeRequired()
        {
            bool existsActiveScope = scopeStack.Count > 0;
            if (!existsActiveScope)
            {
                return true;
            }
            // Verify if any scope element vote for a new scope
            bool anyElementVoteForNewScope = new List<ScopeElement>(scopeElements.Values).Exists(se => se.RequireNewScope);
            if (anyElementVoteForNewScope)
            {
                bool anyElementsRefuseNewScope = new List<ScopeElement>(scopeElements.Values).Exists(se => se.RefuseNewScope);
                if (anyElementsRefuseNewScope)
                {
                    throw new InvalidOperationException(
                        "Some elements are in a state that they refuse the creation of a new scope");
                }
                return true;
            }
            return false;
        }

        #region Thread safe push and pop of scope stack

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void PushInstanceInStack()
        {
            scopeStack.Push(this);
            Current._InstanceCount = 1;
        }

        #endregion Thread safe push and pop of scope stack

        public void Dispose()
        {
            if (--Current._InstanceCount == 0)
            {
                if (scopeStack.Count > 0)
                {
                    scopeStack.Pop();
                }
                foreach (var scopeElement in scopeElements.Values)
                {
                    scopeElement.Dispose();
                }
                GC.SuppressFinalize(this);
            }
        }
    }
}
