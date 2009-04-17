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
        private static Stack<Scope> _scopeStack;

        [ThreadStatic]
        private static Scope _Current;

        private static void Pop()
        {
            // Pop scope's from stack until find one not yet disposed
            while ( (_scopeStack.Count > 0) && (Current._isDisposed) )
            {
                _Current = _scopeStack.Pop();
            }
            if (_scopeStack.Count == 0)
            {
                _Current = null;
            }
        }

        private static void Push(Scope scope)
        {
            _scopeStack.Push(scope);
            _Current = scope;
        }

        public static Scope Current
        {
            get
            {
                return _Current;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        static Scope()
        {
            _scopeStack = new Stack<Scope>();
        }

        #endregion Class Elements

        public Scope(params ScopeContext[] contexts)
        {
            scopeContexts = new Dictionary<Type, ScopeContext>();
            if (IsNewScopeRequired())
            {
                foreach (var context in contexts)
                {
                    AddScopeContext(context);
                }
                PushInstanceInStack();
            }
            else
            {
                // Supress destructor if this instance will not be used
                GC.SuppressFinalize(true);
                Current._InstanceCount = Current.InstanceCount + 1;
            }
        }

        #region Construtores

        ///// <summary>
        ///// Copia o escopo informado para <see cref="Escopo.Atual"/>
        ///// </summary>
        ///// <remarks>
        ///// Geralmente usado em casos onde o escopo atual é usado em outras Threads para aproveitar 
        ///// a mesma transacao por exemplo.
        ///// </remarks>
        ///// <param name="escopo"></param>
        //public Escopo(Escopo escopo)
        //{
        //    if (escopo.EstaFinalizado)
        //        throw new ObjectDisposedException("Não posso inicializar um escopo a partir de outro escopo finalizado");
        //    if (escopo == null)
        //        throw new NullReferenceException();
        //    this.AtribuirEscopoAtual();
        //}

        ///// <summary>
        ///// Inicia um novo escopo de execução ou participa de um existente
        ///// </summary>
        ///// <param name="opcaoTransacao">Opcao da transacao. Se for <see cref="OpcaoTransacao.Requerido"/> 
        ///// e ja existir um escopo, um novo escopo é criado de forma transparente</param>
        //public Escopo(OpcaoTransacao opcaoTransacao)
        //    : this(ContextoAcessoDado.NOME_CONEXAO_GERAL, opcaoTransacao)
        //{
        //}


        ///// <summary>
        ///// Inicia um novo escopo de execução ou participa de um existente
        ///// </summary>
        ///// <exception cref="InvalidOperationException"><see cref="OpcaoTransacao.Requerido"/> exige <see cref="OpcaoEscopo.Requerido"/></exception>
        ///// se um escopo ja existir sem uma transação ativa
        ///// <param name="opcaoEscopo">Configura a forma como esse escopo vai participar de um ja existente</param>
        ///// <param name="opcaoTransacao">Configura a forma como esse escopo vai participar de uma transação</param>
        //public Escopo(OpcaoEscopo opcaoEscopo, OpcaoTransacao opcaoTransacao)
        //    : this(opcaoEscopo, ContextoAcessoDado.NOME_CONEXAO_GERAL, opcaoTransacao)
        //{
        //}

        ///// <summary>
        ///// Inicia um novo escopo de execução ou participa de um existente
        ///// </summary>
        ///// <exception cref="InvalidOperationException"><see cref="OpcaoTransacao.Requerido"/> exige <see cref="OpcaoEscopo.Requerido"/></exception>
        ///// se um escopo ja existir sem uma transação ativa
        ///// <param name="opcaoEscopo">Configura a forma como esse escopo vai participar de um ja existente</param>
        ///// <param name="isolationLevel">Nivel de isolamento transacional. Quando esse construtor é usado <see cref="OpcaoTransacao.Requerido"/> 
        ///// é usado automaticamente e se já existir um escopo, um novo escopo é criado de forma transparente</param>
        //public Escopo(OpcaoEscopo opcaoEscopo, IsolationLevel isolationLevel)
        //    : this(opcaoEscopo, ContextoAcessoDado.NOME_CONEXAO_GERAL, isolationLevel)
        //{
        //}

        ///// <summary>
        ///// Inicia um novo escopo de execução ou participa de um existente
        ///// </summary>
        ///// <exception cref="InvalidOperationException"><see cref="OpcaoTransacao.Requerido"/> exige <see cref="OpcaoEscopo.Requerido"/></exception>
        ///// se um escopo ja existir sem uma transação ativa
        ///// <param name="opcaoEscopo">Configura a forma como esse escopo vai participar de um ja existente</param>
        ///// <param name="nomeConfigConexao">Nome da configuração de conexao em connectionStrings do arquivo de configuracao da aplicacao</param>
        ///// <param name="opcaoTransacao">Configura a forma como esse escopo vai participar de uma transação</param>
        //public Escopo(OpcaoEscopo opcaoEscopo, string nomeConfigConexao, OpcaoTransacao opcaoTransacao)
        //{
        //    // Se o usuario informou explicitamente opcoes de escopo e de transacao provoca erro
        //    if (OpcaoTransacaoRequerNovoEscopo(opcaoTransacao) && opcaoEscopo == OpcaoEscopo.Requerido)
        //        MensagemUtil.ThrowInvalidOperationException("ErroTransacaoRequerNovoEscopo");
        //    InicializarEscopo(opcaoEscopo, nomeConfigConexao, opcaoTransacao, null);
        //}

        ///// <summary>
        ///// Inicia um novo escopo de execução ou participa de um existente
        ///// </summary>
        ///// <exception cref="InvalidOperationException"><see cref="OpcaoTransacao.Requerido"/> exige <see cref="OpcaoEscopo.Requerido"/></exception>
        ///// se um escopo ja existir sem uma transação ativa
        ///// <param name="opcaoEscopo">Configura a forma como esse escopo vai participar de um ja existente</param>
        ///// <param name="nomeConfigConexao">Nome da configuração de conexao em connectionStrings do arquivo de configuracao da aplicacao</param>
        ///// <param name="isolationLevel">Nivel de isolamento transacional. Quando esse construtor é usado <see cref="OpcaoTransacao.Requerido"/> 
        ///// é usado automaticamente e se já existir um escopo, um novo escopo é criado de forma transparente</param>
        //public Escopo(OpcaoEscopo opcaoEscopo, string nomeConfigConexao, IsolationLevel isolationLevel)
        //{
        //    OpcaoTransacao opcaoTransacao = OpcaoTransacao.Requerido;
        //    // Se o usuario informou explicitamente opcoes de escopo e de transacao provoca erro
        //    if (OpcaoTransacaoRequerNovoEscopo(opcaoTransacao) && opcaoEscopo == OpcaoEscopo.Requerido)
        //        MensagemUtil.ThrowInvalidOperationException("ErroTransacaoRequerNovoEscopo");
        //    InicializarEscopo(opcaoEscopo, nomeConfigConexao, opcaoTransacao, isolationLevel);
        //}

        ///// <summary>
        ///// Inicia um novo escopo de execução ou participa de um existente
        ///// </summary>
        ///// <exception cref="InvalidOperationException"><see cref="OpcaoTransacao.Requerido"/> exige <see cref="OpcaoEscopo.Requerido"/></exception>
        ///// se um escopo ja existir sem uma transação ativa
        ///// <param name="opcaoEscopo">Configura a forma como esse escopo vai participar de um ja existente</param>
        ///// <param name="nomeConfigConexao">Nome da configuração de conexao em connectionStrings do arquivo de configuracao da aplicacao</param>
        ///// <param name="opcaoTransacao">Configura a forma como esse escopo vai participar de uma transação</param>
        //public Escopo(OpcaoEscopo opcaoEscopo, StringConexao nomeConfigConexao, OpcaoTransacao opcaoTransacao)
        //{
        //    // Se o usuario informou explicitamente opcoes de escopo e de transacao provoca erro
        //    if (OpcaoTransacaoRequerNovoEscopo(opcaoTransacao) && opcaoEscopo == OpcaoEscopo.Requerido)
        //        MensagemUtil.ThrowInvalidOperationException("ErroTransacaoRequerNovoEscopo");
        //    InicializarEscopo(opcaoEscopo, nomeConfigConexao.ToString(), opcaoTransacao, null);
        //}


        #endregion Construtores


        private readonly Dictionary<Type, ScopeContext> scopeContexts;
        private int _InstanceCount;

        internal int InstanceCount
        {
            get { return _InstanceCount; }
        }

        public void Complete()
        {
            CheckDisposed();
            if (_InstanceCount == 1)
            {
                Current._Completed = true;
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
            bool someIsNullButNotBoth = (leftNull || rightNull) && !bothAreNull;
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

        private void AddScopeContext(ScopeContext newScopeElement)
        {
            var scopeElementType = newScopeElement.GetType();
            if (scopeContexts.ContainsKey(scopeElementType))
            {
                var currentScopeElement = scopeContexts[scopeElementType];
                currentScopeElement.NewScopeContextIsCompatible(newScopeElement);
            }
            scopeContexts.Add(scopeElementType, newScopeElement);
        }

        internal T GetScopeContext<T>() where T : ScopeContext
        {
            CheckDisposed();
            var type = typeof(T);
            return scopeContexts.ContainsKey(type) ? (T) scopeContexts[type] : null;
        }

        private bool IsNewScopeRequired()
        {
            bool existsActiveScope = _Current != null;
            if (!existsActiveScope)
            {
                return true;
            }
            // Verify if any scope element vote for a new scope
            bool anyElementVoteForNewScope = new List<ScopeContext>(scopeContexts.Values).Exists(se => se.RequireNewScope);
            if (anyElementVoteForNewScope)
            {
                bool anyElementsRefuseNewScope = new List<ScopeContext>(scopeContexts.Values).Exists(se => se.RefuseNewScope);
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
            Current._InstanceCount = 1;
            //Iniyialize all scope contexts
            Array.ForEach(scopeContexts.Values.ToArray(), (scopeContext => scopeContext.Init()) );

        }

        #endregion Scope control

        #region IDisposable

        private bool _isDisposed;
        private bool _Completed;

        private void CheckDisposed()
        {
            if (_isDisposed)
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
            bool canFinalize = (!_isDisposed) &&  ( (Current == null) || (--Current._InstanceCount < 1) );
            if (canFinalize)
            {
                bool thisIsCurrentScope = Current == this;
                if (thisIsCurrentScope)
                {
                    _isDisposed = true;
                    // Dispose all scope contexts
                    foreach (var scopeContext in scopeContexts.Values)
                    {
                        scopeContext.CompletedNow(_Completed);
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

