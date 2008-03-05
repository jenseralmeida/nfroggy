using System;
using System.Collections.Generic;
using System.Text;

namespace Froggy.Validation.BaseValidator
{
    public sealed class ComparableValidator<T> : ITestValidator<T> 
    {
        public ComparableValidator(T equal)
        {
            _Equal = equal;
            _ComparableValidatorType = ComparableValidatorType.Equal;
        }

        public ComparableValidator(T minimum, T maximum)
        {
            _Minimum = minimum;
            _Maximum = maximum;
            _ComparableValidatorType = ComparableValidatorType.IntervalInclusive;
        }

        public ComparableValidator(T minimum, T maximum, ComparableValidatorType comparableValidatorType)
        {
            _Minimum = minimum;
            _Maximum = maximum;
            _ComparableValidatorType = comparableValidatorType;
        }

        T _Equal;
        T _Minimum;
        T _Maximum;
        ComparableValidatorType _ComparableValidatorType;

        public T Equal
        {
            get { return _Equal; }
        }

        public T Minimum
        {
            get { return _Minimum; }
        }

        public T Maximum
        {
            get { return _Maximum; }
        }

        public ComparableValidatorType ComparableValidatorType
        {
            get { return _ComparableValidatorType; }
        }

        #region ITestValidator<T> Members

        public bool Execute(T value, out string errorMessageTemplate)
        {
            IComparable comparable = (IComparable)value;
            errorMessageTemplate = "";
            if (BasicValidatorUtil.ContainsValueInEnum((int)ComparableValidatorType.Equal, (int)ComparableValidatorType))
            {
                if (comparable.CompareTo(this.Equal) != 0)
                {
                    errorMessageTemplate = "The value of {0} is not equal";
                }
            }
            else if (BasicValidatorUtil.ContainsValueInEnum((int)ComparableValidatorType.IntervalInclusive, (int)ComparableValidatorType))
            {
                if (comparable.CompareTo(_Minimum) < 0 || comparable.CompareTo(this._Maximum) > 0)
                {
                    errorMessageTemplate = "The value of {0} is not in the defined inclusive interval";
                }
            }
            else if (BasicValidatorUtil.ContainsValueInEnum((int)ComparableValidatorType.MinimumInclusive, (int)ComparableValidatorType))
            {
                if (comparable.CompareTo(_Minimum) < 0)
                {
                    errorMessageTemplate = "The value of {0} is below inclusive minimal";
                }
            }
            else if (BasicValidatorUtil.ContainsValueInEnum((int)ComparableValidatorType.MaximumInclusive, (int)ComparableValidatorType))
            {
                if (comparable.CompareTo(_Maximum) > 0)
                {
                    errorMessageTemplate = "The value of {0} is above inclusive maximum";
                }
            }
            else if (BasicValidatorUtil.ContainsValueInEnum((int)ComparableValidatorType.IntervalExclusive, (int)ComparableValidatorType))
            {
                if (comparable.CompareTo(_Minimum) <= 0 || comparable.CompareTo(this._Maximum) >= 0)
                {
                    errorMessageTemplate = "The value of {0} is not in the defined exclusive interval";
                }
            }
            else if (BasicValidatorUtil.ContainsValueInEnum((int)ComparableValidatorType.MinimumExclusive, (int)ComparableValidatorType))
            {
                if (comparable.CompareTo(_Minimum) <= 0)
                {
                    errorMessageTemplate = "The value of {0} is below exclusive minimal";
                }
            }
            else if (BasicValidatorUtil.ContainsValueInEnum((int)ComparableValidatorType.MaximumExclusive, (int)ComparableValidatorType))
            {
                if (comparable.CompareTo(_Maximum) >= 0)
                {
                    errorMessageTemplate = "The value of {0} is above exclusive maximum";
                }
            }
            return String.IsNullOrEmpty(errorMessageTemplate);
        }

        #endregion
    }
}
