using System;
using System.Collections.Generic;
using System.Text;

namespace Froggy.Validation.BaseValidator
{
	[Serializable]
    public class LengthValidator : ITestValidator
    {
        public LengthValidator(int equal)
        {
            _Equal = equal;
            _LengthValidatorType = IntervalValidatorType.Equal;
        }

        public LengthValidator(int minimum, int maximum)
        {
            _Minimum = minimum;
            _Maximum = maximum;
            _LengthValidatorType = IntervalValidatorType.IntervalInclusive;
        }

        public LengthValidator(int minimum, int maximum, IntervalValidatorType lengthValidatorType)
        {
            _Minimum = minimum;
            _Maximum = maximum;
            _LengthValidatorType = lengthValidatorType;
        }

        int _Equal;
        int _Minimum;
        int _Maximum;
        IntervalValidatorType _LengthValidatorType;

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

        public IntervalValidatorType LengthValidatorType
        {
            get { return _LengthValidatorType; }
            set { _LengthValidatorType = value; }
        }

        #region ITestValidator Members

        public bool Execute<T>(T value, out string errorMessageTemplate)
        {
            int lengthValue = value.ToString().Length;
            errorMessageTemplate = "";
            if (BasicValidatorUtil.ContainsValueInEnum((int)IntervalValidatorType.Equal, (int)LengthValidatorType))
            {
                if (lengthValue.CompareTo(this.Equal) != 0)
                {
                    errorMessageTemplate = "The value of {0} is not equal";
                }
            }
            else if (BasicValidatorUtil.ContainsValueInEnum((int)IntervalValidatorType.IntervalInclusive, (int)LengthValidatorType))
            {
                if (lengthValue.CompareTo(_Minimum) < 0 || lengthValue.CompareTo(this._Maximum) > 0)
                {
                    errorMessageTemplate = "The value of {0} is not in the defined inclusive interval";
                }
            }
            else if (BasicValidatorUtil.ContainsValueInEnum((int)IntervalValidatorType.MinimumInclusive, (int)LengthValidatorType))
            {
                if (lengthValue.CompareTo(_Minimum) < 0)
                {
                    errorMessageTemplate = "The value of {0} is below inclusive minimal";
                }
            }
            else if (BasicValidatorUtil.ContainsValueInEnum((int)IntervalValidatorType.MaximumInclusive, (int)LengthValidatorType))
            {
                if (lengthValue.CompareTo(_Maximum) > 0)
                {
                    errorMessageTemplate = "The value of {0} is above inclusive maximum";
                }
            }
            else if (BasicValidatorUtil.ContainsValueInEnum((int)IntervalValidatorType.IntervalExclusive, (int)LengthValidatorType))
            {
                if (lengthValue.CompareTo(_Minimum) <= 0 || lengthValue.CompareTo(this._Maximum) >= 0)
                {
                    errorMessageTemplate = "The value of {0} is not in the defined exclusive interval";
                }
            }
            else if (BasicValidatorUtil.ContainsValueInEnum((int)IntervalValidatorType.MinimumExclusive, (int)LengthValidatorType))
            {
                if (lengthValue.CompareTo(_Minimum) <= 0)
                {
                    errorMessageTemplate = "The value of {0} is below exclusive minimal";
                }
            }
            else if (BasicValidatorUtil.ContainsValueInEnum((int)IntervalValidatorType.MaximumExclusive, (int)LengthValidatorType))
            {
                if (lengthValue.CompareTo(_Maximum) >= 0)
                {
                    errorMessageTemplate = "The value of {0} is above exclusive maximum";
                }
            }
            return String.IsNullOrEmpty(errorMessageTemplate);
        }

        #endregion
    }
}
