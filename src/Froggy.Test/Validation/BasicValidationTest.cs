using NUnit.Framework;
using Rhino.Mocks;

namespace Froggy.Test.Validation
{
    [TestFixture]
    public class BasicValidationTest
    {
        [SetUp]
        public void Init()
        {
            new MockRepository();
        }

        [Test]
        public void FirstMock()
        {
            //ITestValidator<int> customValidatorExample = _mockRepository.DynamicMock<ITestValidator<int>>();

            //Validation<int> validation = _mockRepository.DynamicMock<Validation<int>>()
            //    .SetUpErrorMessageLabel("teste")
            //    .SetUp(customValidatorExample);

            //string errorMessage;
            //validation.IsValid(10);
            //validation.IsValid(10, out errorMessage);
            //validation.Convert("1");
            //validation.Convert("1", out errorMessage);
            //int result;
            //if (validation.TryConvert("a", out result)) { }
            //if (validation.TryConvert("a", out result, out errorMessage)) { }

            //// Record
            //Expect.Call(validation.IsValid(0)).Return(true);
            //Expect.Call(validation.IsValid(1)).Return(false);
            //// Stop Record
            //_mockRepository.ReplayAll();

            //Assert.IsTrue(validation.IsValid(0));
            //Assert.IsFalse(validation.IsValid(1));
            ////_mockRepository.VerifyAll();
        }

        public void DirectCall()
        {
            //ValidatorDomain<int>.SetUp().
        }

    }
}
