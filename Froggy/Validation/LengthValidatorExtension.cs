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
	public static class LengthValidatorExtension
	{
		public static Validator<T> SetUpLength<T>(this Validator<T> validator, int equal)
		{
			return validator.SetUp(new LengthValidator<T>(equal));
		}

		public static Validator<T> SetUpLength<T>(this Validator<T> validator, int minimum, int maximum)
		{
			return validator.SetUp(new LengthValidator<T>(minimum, maximum));
		}

		public static Validator<T> SetUpLength<T>(this Validator<T> validator, int minimum, int maximum, IntervalValidatorType lengthValidatorType)
		{
			return validator.SetUp(new LengthValidator<T>(minimum, maximum, lengthValidatorType));
		}
	}
}
