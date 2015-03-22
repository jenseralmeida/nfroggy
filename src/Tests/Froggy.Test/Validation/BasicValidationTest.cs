using Xunit;
using Rhino.Mocks;

namespace Froggy.Test.Validation
{

    public class BasicValidationTest
    {
        public BasicValidationTest()
        {
            new MockRepository();
        }

        [Fact]
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

            //Assert.True(validation.IsValid(0));
            //Assert.False(validation.IsValid(1));
            ////_mockRepository.VerifyAll();
        }

        public void DirectCall()
        {
            //ValidatorDomain<int>.SetUp().
        }

    }
}
