using System;
using System.Collections.Generic;
using System.Text;

namespace Froggy.Validation
{
    public interface IValidatorTest<T>: IValidator
    {
        bool Execute(T value);
    }
}
