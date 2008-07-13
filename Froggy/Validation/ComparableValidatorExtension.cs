/*
 * Created by SharpDevelop.
 * User: jenser
 * Date: 3/6/2008
 * Time: 13:40
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using Froggy.Validation.BaseValidator;

namespace Froggy.Validation
{
	/// <summary>
	/// Description of ComparableValidatorExtension.
	/// </summary>
	public static class ComparableValidatorExtension
	{

		public static Validator<T> SetUpComparable<T>(this Validator<T> validator, T equal) where T: IComparable
		{
			return validator.SetUp(new ComparableValidator(equal));
		}

        public static Validator<T> SetUpComparable<T>(this Validator<T> validator, T minimum, T maximum) where T : IComparable
		{
			return validator.SetUp(new ComparableValidator(minimum, maximum));
		}

        public static Validator<T> SetUpComparable<T>(this Validator<T> validator, T minimum, T maximum, IntervalValidatorType comparableValidatorType) where T : IComparable
		{
			return validator.SetUp(new ComparableValidator(minimum, maximum, comparableValidatorType));
		}	}
}
