using System;
using System.Text;
using Froggy.Validation;
using Xunit;

namespace Froggy.Test.Validation
{
	public static class WorksTestValidatorExtension
	{
		public static Validator<T> SetUpWorksTest<T>(this Validator<T> validator)
		{
			return validator.SetUp(new WorksTestValidator());
		}
	}
    public class WorksTestValidator : ITestValidator
    {

        #region ITestValidator Members

        public bool IgnoreNullValue
        {
            get { return false; }
        }

        public bool Execute<T>(T value, object orgValue, out string errorMessageTemplate)
        {
            if (value == null)
            {
                errorMessageTemplate = "The value of {0} must have a value for work";
                return false;
            }
            if (value.Equals("works"))
            {
                errorMessageTemplate = "";
                return true;
            }
            errorMessageTemplate = "The value of {0} does not works";
            return false;
        }

        #endregion
    }


    public class CustomValidatorTest
    {
        [Fact]
        public void WorksTest()
        {
            Validator<string> worksValidation = new Validator<string>("works label")
            	.SetUpWorksTest();
            string message;

            bool thisWork = worksValidation.IsValid("works", out message);
            Console.WriteLine(message);
            Assert.True(thisWork, message);

            bool thisDoesNotWork = worksValidation.IsValid("does not works", out message);
            Console.WriteLine(message);
            Assert.False(thisDoesNotWork, message);

            bool nullDoesNotWork = worksValidation.IsValid(null);
            Console.WriteLine(message);
            Assert.False(nullDoesNotWork, message);
        }
    }
}