using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Froggy
{
    public enum TransactionNeed
    {
        Automatic,
        Required
    }

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
