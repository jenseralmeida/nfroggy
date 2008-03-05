using System;
using System.Collections.Generic;
using System.Text;

namespace Froggy.Validation.BaseValidator
{
    public sealed class IntervalValidator<T> : IValidatorTest<T> 
    {
        T _Equal;
        T _Minimum;
        T _Maximum;

        public IntervalValidator(T equal)
        {
            _Equal = equal;
        }

        public IntervalValidator(T minimum, T maximum)
        {
            _Minimum = minimum;
            _Maximum = maximum;
        }

        #region IValidatorTest<T> Members

        public bool Execute(T value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
