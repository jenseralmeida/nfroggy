using System;

namespace Froggy.Validation.BaseValidator
{
	[Serializable]
    public class LengthValidator : ITestValidator
    {
        public LengthValidator(int equal)
        {
            Equal = equal;
            LengthValidatorType = IntervalValidatorType.Equal;
        }

        public LengthValidator(int minimum, int maximum)
        {
            _Minimum = minimum;
            _Maximum = maximum;
            LengthValidatorType = IntervalValidatorType.IntervalInclusive;
        }

        public LengthValidator(int minimum, int maximum, IntervalValidatorType lengthValidatorType)
        {
            _Minimum = minimum;
            _Maximum = maximum;
            LengthValidatorType = lengthValidatorType;
        }

	    int _Minimum;
        int _Maximum;
        private NullValueLength _NullValueLength = NullValueLength.Ignore;

	    public int Equal { get; set; }

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

	    public IntervalValidatorType LengthValidatorType { get; set; }

	    #region ITestValidator Members

	    public bool IgnoreNullValue
	    {
            get { return false;  }
	    }

	    public NullValueLength NullValueLength
	    {
	        get { return _NullValueLength; }
	        set { _NullValueLength = value; }
	    }

	    public bool Execute<T>(T value, object orgValue, out string errorMessageTemplate)
        {
	        int lengthValue;
            if (orgValue == null)
            {
                switch (_NullValueLength)
                {
                    case NullValueLength.Zero:
                        lengthValue = 0;
                        break;
                    default:
                        errorMessageTemplate = "";
                        return true;
                }
            }
            else
            {
                lengthValue = orgValue.ToString().Length;
            }
            errorMessageTemplate = "";
            if (BasicValidatorUtil.ContainsValueInEnum((int)IntervalValidatorType.Equal, (int)LengthValidatorType))
            {
                if (lengthValue.CompareTo(Equal) != 0)
                {
                    errorMessageTemplate = "The value of {0} is not equal";
                }
            }
            else if (BasicValidatorUtil.ContainsValueInEnum((int)IntervalValidatorType.IntervalInclusive, (int)LengthValidatorType))
            {
                if (lengthValue.CompareTo(_Minimum) < 0 || lengthValue.CompareTo(_Maximum) > 0)
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
                if (lengthValue.CompareTo(_Minimum) <= 0 || lengthValue.CompareTo(_Maximum) >= 0)
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
