using System;
using System.Collections.Generic;
using System.Text;

namespace Froggy.Validation
{
    public interface ITypeValidator<T>: IValidator
    {
        bool TryParse(object value, out T result);
    }
}
