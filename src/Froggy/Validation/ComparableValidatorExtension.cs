using System;
using Froggy.Validation.BaseValidator;

namespace Froggy.Validation
{
    /// <summary>
    /// Description of ComparableValidatorExtension.
    /// </summary>
    [Serializable]
    public static class ComparableValidatorExtension
    {
        static ComparableValidatorExtension()
        {
            TypeOfIComparable = typeof (IComparable);
        }

        private static Type TypeOfIComparable { get; set; }

        public static Validator<T> SetUpComparable<T>(this Validator<T> validator, T equal)
        {
            IComparable equalComparable = ExtractComparableValue(equal);
            return validator.SetUp(new ComparableValidator(equalComparable));
        }

        public static Validator<T> SetUpComparable<T>(this Validator<T> validator, T minimum, T maximum)
        {
            IComparable minimumComparable = ExtractComparableValue(minimum);
            IComparable maximumComparable = ExtractComparableValue(maximum);
            return validator.SetUp(new ComparableValidator(minimumComparable, maximumComparable));
        }

        public static Validator<T> SetUpComparable<T>(this Validator<T> validator, T minimum, T maximum,
                                                      IntervalValidatorType comparableValidatorType)
        {
            IComparable minimumComparable = ExtractComparableValue(minimum);
            IComparable maximumComparable = ExtractComparableValue(maximum);
            return validator.SetUp(new ComparableValidator(minimumComparable, maximumComparable, comparableValidatorType));
        }

        private static IComparable ExtractComparableValue<T>(T value)
        {
            if (IsInstanceOfTypeIComparable(value))
                return (IComparable)value;
            
            IComparable comparableValue = null;
            if (value != null)
            {
                T result;
                SystemTypeValidator<T>.TryChangeType(value, false, out result);
                if (IsInstanceOfTypeIComparable(result))
                {
                    comparableValue = (IComparable) result;
                }
            }

            bool cannotExtractTypeIComparableOfValue = value != null && comparableValue == null;
            if (cannotExtractTypeIComparableOfValue)
            {
                throw new ArgumentException(
                    "Validation cannot determine the IComparable interface associated with value", "value");
            }
            return comparableValue;
        }

        private static bool IsInstanceOfTypeIComparable<T>(T result)
        {
            return TypeOfIComparable.IsInstanceOfType(result);
        }
    }
}