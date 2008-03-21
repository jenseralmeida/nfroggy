using System;
using System.Collections.Generic;
using System.Text;
using Froggy.Validation;
using NUnit.Framework;

namespace Froggy.Test.Validation
{
    public class WorksTestValidator<T> : ITestValidator<T>
    {

        #region ITestValidator<T> Members

        public bool Execute(T value, out string errorMessageTemplate)
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
            else
            {
                errorMessageTemplate = "The value of {0} does not works";
                return false;
            }
        }

        #endregion
    }

    [TestFixture]
    public class CustomValidatorTest
    {
        [Test]
        public void WorksTest()
        {
            Validation<string> worksValidation = Validation<string>.Create()
                  .SetUp(new WorksTestValidator<string>());
            string message;

            bool thisWork = worksValidation.IsValid("works", out message);
            Assert.IsTrue(thisWork, message);
            bool thisDoesNotWork = worksValidation.IsValid("does not works", out message);
            Assert.IsFalse(thisDoesNotWork, message);
            bool nullDoesNotWork = worksValidation.IsValid(null);
            Assert.IsFalse(nullDoesNotWork, message);
        }
    }
}