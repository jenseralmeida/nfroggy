using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using Froggy.Validation;

namespace Froggy.Test.Validation
{
    [TestFixture]
    public class ValidationUnitTest
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
            IValidator<int> validator = _mockRepository.DynamicMock<IValidator<int>>();
            // Record
            Expect.Call(validator.IsValid(0)).Return(true);
            Expect.Call(validator.IsValid(1)).Return(false);
            // Stop Record
            _mockRepository.ReplayAll();
            
            Assert.IsTrue(validator.IsValid(0));
            Assert.IsFalse(validator.IsValid(1));
            //_mockRepository.VerifyAll();
        }

        public void DirectCall()
        {
            //ValidatorDomain<int>.SetUp().
        }

    }
}
