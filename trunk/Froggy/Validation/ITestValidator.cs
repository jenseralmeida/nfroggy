using System;
using System.Collections.Generic;
using System.Text;

namespace Froggy.Validation
{
    public interface ITestValidator
    {
        bool Execute<T>(T value, out string errorMessageTemplate);
    }
}
