using System;
using System.Collections.Generic;
using System.Text;
using Froggy.Validation.BaseValidator;

namespace Froggy.Validation.BaseValidator
{
    internal static class InternalBasicValidatorUtil
    {
        public static TypeValidator<T> GetInstanceOfTypeValidator<T>()
        {
            return new TypeValidator<T>();
        }

        public static TypeValidator<T> GetInstanceOfTypeValidator<T>(bool isNullable)
        {
            return new TypeValidator<T>(isNullable);
        }
    }
}
