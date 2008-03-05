using System;
using System.Collections.Generic;
using System.Text;

namespace Froggy.Validation
{
    public interface IValidatorTest<T>
    {
        bool Execute(T value, out string errorMessageTemplate);
    }
}
