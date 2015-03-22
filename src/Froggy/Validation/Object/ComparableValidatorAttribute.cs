using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Froggy.Validation.BaseValidator;

namespace Froggy.Validation.Object
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class ComparableValidatorAttribute : TestValidatorAttribute
    {
        ComparableValidator _ComparableValidator;

        public ComparableValidatorAttribute(object equal)
        {
            _ComparableValidator = new ComparableValidator((IComparable)equal);
        }

        public ComparableValidatorAttribute(object minimum, object maximum)
        {
            _ComparableValidator = new ComparableValidator((IComparable)minimum, (IComparable)maximum);
        }

        public ComparableValidatorAttribute(object minimum, object maximum, IntervalValidatorType comparableValidatorType)
        {
            _ComparableValidator = new ComparableValidator((IComparable)minimum, (IComparable)maximum, comparableValidatorType);
        }

        public override ITestValidator Create()
        {
            return _ComparableValidator;
        }
    }
}
