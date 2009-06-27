namespace Froggy.Data
{
    public static class DaScopeContextExtension
    {
        public static DaScopeContext GetDaScopeContext(this Scope scope)
        {
            return scope.GetScopeContext<DaScopeContext>();
        }
    }
}