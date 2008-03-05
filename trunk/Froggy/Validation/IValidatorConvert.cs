using System;
using System.Collections.Generic;
using System.Text;

namespace Froggy.Validation
{
    public interface IValidatorConvert<T>
    {
        bool Execute(object value, out T result);
    }
}
