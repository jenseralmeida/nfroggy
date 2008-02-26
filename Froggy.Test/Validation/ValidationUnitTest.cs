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
        private IValidationUnit _validationUnit;
                                           
        [SetUp]
        public void Init()
        {
            _mockRepository = new MockRepository();
            _validationUnit = _mockRepository.DynamicMock<IValidationUnit>();
        }

        [Test]
        public void FirstMock()
        {
            Expect.Call(_validationUnit.IsValid(0))
                .Return(true);
            Expect.Call(_validationUnit.IsValid(1))
                .Return(false);
            _mockRepository.ReplayAll();

            //bool isValid = _validationUnit.IsValid(1);
            Assert.IsTrue(_validationUnit.IsValid(0));
            Assert.IsFalse(_validationUnit.IsValid(1));
            //_mockRepository.VerifyAll();
        }
    }
}
