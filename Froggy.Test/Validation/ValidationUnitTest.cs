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
            IValidation<int> validation = _mockRepository.DynamicMock<IValidation<int>>();
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
