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
        private IValidationUnit _validationUnitMock;
                                           
        [SetUp]
        public void Init()
        {
            _mockRepository = new MockRepository();
            _validationUnitMock = _mockRepository.DynamicMock<IValidationUnit>();
        }

        [Test]
        public void FirstMock()
        {
            Expect.Call( _validationUnitMock.IsValid(1) ).Return(true);
            _mockRepository.ReplayAll();
            _mockRepository.VerifyAll();
        }
    }
}
