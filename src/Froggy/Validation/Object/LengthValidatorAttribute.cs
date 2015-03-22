using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Froggy.Validation.BaseValidator;

namespace Froggy.Validation.Object
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class LengthValidatorAttribute : TestValidatorAttribute
    {
        LengthValidator _LengthValidator;

        public LengthValidatorAttribute(int equal)
        {
            _LengthValidator = new LengthValidator(equal);
        }

        public LengthValidatorAttribute(int minimum, int maximum)
        {
            _LengthValidator = new LengthValidator(minimum, maximum);
        }

        public LengthValidatorAttribute(int minimum, int maximum, IntervalValidatorType lengthValidatorType)
        {
            _LengthValidator = new LengthValidator(minimum, maximum, lengthValidatorType);
        }

        public override ITestValidator Create()
        {
            return _LengthValidator;
        }

    }
}
