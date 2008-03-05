using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using Froggy.Validation;

namespace Froggy.Test.Validation
{
    [TestFixture]
    public class BasicValidationTest
    {
        private MockRepository _mockRepository;

        [SetUp]
        public void Init()
        {
            _mockRepository = new MockRepository();
        }

        [Test]
        public void FirstMock()
        {
            IValidator customValidatorExample = _mockRepository.DynamicMock<IValidator>();

            IValidation<int> validation = _mockRepository.DynamicMock<IValidation<int>>()
                .SetUpErrorMessageLabel("teste")
                .SetUpInterval(0)
                .SetUp(customValidatorExample);

            string errorMessage;
            validation.IsValid(10);
            validation.IsValid(10, out errorMessage);
            validation.Convert("1");
            validation.Convert("1", out errorMessage);
            int result;
            if (validation.TryConvert("a", out result)) { }
            if (validation.TryConvert("a", out result, out errorMessage)) { }

            // Record
            Expect.Call(validation.IsValid(0)).Return(true);
            Expect.Call(validation.IsValid(1)).Return(false);
            // Stop Record
            _mockRepository.ReplayAll();

            Assert.IsTrue(validation.IsValid(0));
            Assert.IsFalse(validation.IsValid(1));
            //_mockRepository.VerifyAll();
        }

        public void DirectCall()
        {
            //ValidatorDomain<int>.SetUp().
        }

    }
}
