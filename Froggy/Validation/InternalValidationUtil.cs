using System;
using System.Collections.Generic;
using System.Text;
using Froggy.Validation.BaseValidator;

namespace Froggy.Validation
{
    internal static class InternalValidationUtil
    {
        public TypeValidator<T> GetInstanceOfTypeValidator()
        {
            return new TypeValidator<T>();
        }

        public TypeValidator<T> GetInstanceOfTypeValidator(bool isNullable)
        {
            return new TypeValidator<T>(isNullable);
        }
    }
}
