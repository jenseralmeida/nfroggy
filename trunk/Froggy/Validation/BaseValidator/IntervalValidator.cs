using System;
using System.Collections.Generic;
using System.Text;

namespace Froggy.Validation.BaseValidator
{
    public sealed class IntervalValidator<T> : IValidatorTest<T> 
    {
        public IntervalValidator(T equal)
        {
            _Equal = equal;
            _IntervalValidatorType = IntervalValidatorType.Equal;
        }

        public IntervalValidator(T minimum, T maximum)
        {
            _Minimum = minimum;
            _Maximum = maximum;
            _IntervalValidatorType = IntervalValidatorType.IntervalInclusive;
        }

        public IntervalValidator(T minimum, T maximum, IntervalValidatorType intervalValidatorType)
        {
            _Minimum = minimum;
            _Maximum = maximum;
            _IntervalValidatorType = intervalValidatorType;
        }

        T _Equal;
        T _Minimum;
        T _Maximum;
        IntervalValidatorType _IntervalValidatorType;

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

        public IntervalValidatorType IntervalValidatorType
        {
            get { return _IntervalValidatorType; }
        }

        #region IValidatorTest<T> Members

        public bool Execute(T value, out string errorMessageTemplate)
        {
            IComparable comparer = (IComparable)value;
            errorMessageTemplate = "";
            if (BasicValidatorUtil.ContainsValueInEnum((int)IntervalValidatorType.Equal, (int)IntervalValidatorType))
            {
                if (comparer.CompareTo(this.Equal) != 0)
                {
                    errorMessageTemplate = "The value of {0} is not equal";
                }
            }
            else if (BasicValidatorUtil.ContainsValueInEnum((int)IntervalValidatorType.IntervalInclusive, (int)IntervalValidatorType))
            {
                if (comparer.CompareTo(_Minimum) < 0 || comparer.CompareTo(this._Maximum) > 0)
                {
                    errorMessageTemplate = "The value of {0} is not in the defined inclusive interval";
                }
            }
            else if (BasicValidatorUtil.ContainsValueInEnum((int)IntervalValidatorType.MinimumInclusive, (int)IntervalValidatorType))
            {
                if (comparer.CompareTo(_Minimum) < 0)
                {
                    errorMessageTemplate = "The value of {0} is below inclusive minimal";
                }
            }
            else if (BasicValidatorUtil.ContainsValueInEnum((int)IntervalValidatorType.MaximumInclusive, (int)IntervalValidatorType))
            {
                if (comparer.CompareTo(_Maximum) > 0)
                {
                    errorMessageTemplate = "The value of {0} is above inclusive maximum";
                }
            }
            else if (BasicValidatorUtil.ContainsValueInEnum((int)IntervalValidatorType.IntervalExclusive, (int)IntervalValidatorType))
            {
                if (comparer.CompareTo(_Minimum) <= 0 || comparer.CompareTo(this._Maximum) >= 0)
                {
                    errorMessageTemplate = "The value of {0} is not in the defined exclusive interval";
                }
            }
            else if (BasicValidatorUtil.ContainsValueInEnum((int)IntervalValidatorType.MinimumExclusive, (int)IntervalValidatorType))
            {
                if (comparer.CompareTo(_Minimum) <= 0)
                {
                    errorMessageTemplate = "The value of {0} is below exclusive minimal";
                }
            }
            else if (BasicValidatorUtil.ContainsValueInEnum((int)IntervalValidatorType.MaximumExclusive, (int)IntervalValidatorType))
            {
                if (comparer.CompareTo(_Maximum) >= 0)
                {
                    errorMessageTemplate = "The value of {0} is above exclusive maximum";
                }
            }
            return String.IsNullOrEmpty(errorMessageTemplate);
        }

        #endregion
    }
}
