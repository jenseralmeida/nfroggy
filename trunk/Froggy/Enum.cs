namespace Froggy
{
    /// <summary>
    /// Options to determine the use of a existing, or creation of a new, to <see cref="Scope"/>
    /// </summary>
    public enum ScopeOption
    {
        /// <summary>
        /// Use a existing scope, or create a new one if none exists. This is the Default
        /// </summary>
        Automatic,
        /// <summary>
        /// Create a new scope, always
        /// </summary>
        RequireNew
    }

    /// <summary>
    /// Determines how a new <see cref="Scope"/> will be created, when the new <see cref="ScopeContext"/> is added
    /// </summary>
    internal enum ScopeCompatibility
    {
        /// <summary>
        /// Require a new Scope
        /// </summary>
        RequireNew,
        /// <summary>
        /// Replace the current element
        /// </summary>
        ReplaceElement,
        /// <summary>
        /// Ignore new Element
        /// </summary>
        IgnoreNewElement
    }
}
