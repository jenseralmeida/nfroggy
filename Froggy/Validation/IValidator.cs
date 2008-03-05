using System;
using System.Collections.Generic;
using System.Text;

namespace Froggy.Validation
{
    public interface IValidator<T>
    {
        bool Execute(T value);
    }
}
