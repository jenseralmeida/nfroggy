/*
 * Created by SharpDevelop.
 * User: jenser
 * Date: 3/6/2008
 * Time: 13:36
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using Froggy.Validation.BaseValidator;

namespace Froggy.Validation
{
	/// <summary>
	/// Description of LengthValidatorExtension.
	/// </summary>
    [Serializable]
	public static class LengthValidatorExtension
	{
		public static Validator<T> SetUpLength<T>(this Validator<T> validator, int equal)
		{
			return validator.SetUp(new LengthValidator(equal));
		}

		public static Validator<T> SetUpLength<T>(this Validator<T> validator, int minimum, int maximum)
		{
			return validator.SetUp(new LengthValidator(minimum, maximum));
		}

		public static Validator<T> SetUpLength<T>(this Validator<T> validator, int minimum, int maximum, IntervalValidatorType lengthValidatorType)
		{
			return validator.SetUp(new LengthValidator(minimum, maximum, lengthValidatorType));
		}

        public static Validator<T> SetUpLength<T>(this Validator<T> validator, int equal, NullValueLength nullValueLength)
        {
            return validator.SetUp(new LengthValidator(equal) {NullValueLength = nullValueLength });
        }

        public static Validator<T> SetUpLength<T>(this Validator<T> validator, int minimum, int maximum, NullValueLength nullValueLength)
        {
            return validator.SetUp(new LengthValidator(minimum, maximum) { NullValueLength = nullValueLength });
        }

        public static Validator<T> SetUpLength<T>(this Validator<T> validator, int minimum, int maximum, IntervalValidatorType lengthValidatorType, NullValueLength nullValueLength)
        {
            return validator.SetUp(new LengthValidator(minimum, maximum, lengthValidatorType) { NullValueLength = nullValueLength });
        }
    }
}
