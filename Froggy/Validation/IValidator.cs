using System;
using System.Collections.Generic;
using System.Text;

namespace Froggy.Validation
{
    public interface IValidator<T>
    {
        bool ExecuteWithConvert(object value, out T result);

        bool Execute(T value);
    }
}
