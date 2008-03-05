using System;
using System.Collections.Generic;
using System.Text;

namespace Froggy.Validation.BaseValidator
{
    public class LengthValidator<T> : ITestValidator<T>
    {
        public LengthValidator(int equal)
        {
            _Equal = equal;
            _LengthValidatorType = LengthValidatorType.Equal;
        }

        public LengthValidator(int minimum, int maximum)
        {
            _Minimum = minimum;
            _Maximum = maximum;
            _LengthValidatorType = LengthValidatorType.IntervalInclusive;
        }

        public LengthValidator(int minimum, int maximum, LengthValidatorType LengthValidatorType)
        {
            _Minimum = minimum;
            _Maximum = maximum;
            _LengthValidatorType = LengthValidatorType;
        }

        int _Equal;
        int _Minimum;
        int _Maximum;
        LengthValidatorType _LengthValidatorType;

        public int Equal
        {
            get { return _Equal; }
            set { _Equal = value; }
        }

        public int Minimum
        {
            get { return _Minimum; }
            set { _Minimum = value; }
        }

        public int Maximum
        {
            get { return _Maximum; }
            set { _Maximum = value; }
        }

        public LengthValidatorType LengthValidatorType
        {
            get { return _LengthValidatorType; }
            set { _LengthValidatorType = value; }
        }

        #region ITestValidator<T> Members

        public bool Execute(T value, out string errorMessageTemplate)
        {
            IComparable comparable = (IComparable)value;
            errorMessageTemplate = "";
            if (BasicValidatorUtil.ContainsValueInEnum((int)LengthValidatorType.Equal, (int)LengthValidatorType))
            {
                if (comparable.CompareTo(this.Equal) != 0)
                {
                    errorMessageTemplate = "The value of {0} is not equal";
                }
            }
            else if (BasicValidatorUtil.ContainsValueInEnum((int)LengthValidatorType.IntervalInclusive, (int)LengthValidatorType))
            {
                if (comparable.CompareTo(_Minimum) < 0 || comparable.CompareTo(this._Maximum) > 0)
                {
                    errorMessageTemplate = "The value of {0} is not in the defined inclusive interval";
                }
            }
            else if (BasicValidatorUtil.ContainsValueInEnum((int)LengthValidatorType.MinimumInclusive, (int)LengthValidatorType))
            {
                if (comparable.CompareTo(_Minimum) < 0)
                {
                    errorMessageTemplate = "The value of {0} is below inclusive minimal";
                }
            }
            else if (BasicValidatorUtil.ContainsValueInEnum((int)LengthValidatorType.MaximumInclusive, (int)LengthValidatorType))
            {
                if (comparable.CompareTo(_Maximum) > 0)
                {
                    errorMessageTemplate = "The value of {0} is above inclusive maximum";
                }
            }
            else if (BasicValidatorUtil.ContainsValueInEnum((int)LengthValidatorType.IntervalExclusive, (int)LengthValidatorType))
            {
                if (comparable.CompareTo(_Minimum) <= 0 || comparable.CompareTo(this._Maximum) >= 0)
                {
                    errorMessageTemplate = "The value of {0} is not in the defined exclusive interval";
                }
            }
            else if (BasicValidatorUtil.ContainsValueInEnum((int)LengthValidatorType.MinimumExclusive, (int)LengthValidatorType))
            {
                if (comparable.CompareTo(_Minimum) <= 0)
                {
                    errorMessageTemplate = "The value of {0} is below exclusive minimal";
                }
            }
            else if (BasicValidatorUtil.ContainsValueInEnum((int)LengthValidatorType.MaximumExclusive, (int)LengthValidatorType))
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
