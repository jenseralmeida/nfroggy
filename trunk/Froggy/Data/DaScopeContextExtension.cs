namespace Froggy.Data
{
    public static class DaScopeContextExtension
    {
        public static DaScopeContext GetDaScopeContext(this Scope scope)
        {
            if (scope == null)
                return null;
            return scope.GetScopeContext<DaScopeContext>();
        }
    }
}