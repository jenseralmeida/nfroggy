using System;
using System.Collections.Generic;
using System.Text;

namespace Froggy.Validation
{
    public interface ITypeValidator<T> : ITypeValidator
    {
        bool Execute(object value, out T result, out string errorMessageTemplate);
    }
}
