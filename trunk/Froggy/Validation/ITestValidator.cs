using System;
using System.Collections.Generic;
using System.Text;

namespace Froggy.Validation
{
    public interface ITestValidator<T>
    {
        bool Execute(T value, out string errorMessageTemplate);
    }
}
