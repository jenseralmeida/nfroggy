using System;
using System.Collections.Generic;
using System.Text;

namespace Froggy.Validation
{
    public interface IValidatorType<T>: IValidator
    {
        bool IsNullable
        {
            get;
            set;
        }
        bool Execute(object value, out T result, out string errorMessageTemplate);
    }
}
