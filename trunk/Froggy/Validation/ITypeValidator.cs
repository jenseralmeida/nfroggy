using System;
using System.Collections.Generic;
using System.Text;

namespace Froggy.Validation
{
    public interface ITypeValidator
    {
        bool IsNullable
        {
            get;
            set;
        }
    }
}
