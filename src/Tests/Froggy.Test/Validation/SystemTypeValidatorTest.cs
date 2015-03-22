using Froggy.Validation;
using Xunit;

namespace Froggy.Test.Validation
{

    public class SystemTypeValidatorTest
    {
        [Fact]
        public void SimpleIsValidNullabeWithStringTypeTest()
        {
            Validator<string> validationNullable = new Validator<string>().SetUpNullable(true);
            Assert.True(validationNullable.IsValid(14), "14");
            Assert.True(validationNullable.IsValid("14"), "14");
            Assert.True(validationNullable.IsValid(""), "empty");
            Assert.True(validationNullable.IsValid(null), "null");
            Validator<string> validation = new Validator<string>().SetUpNullable(false);
            Assert.True(validation.IsValid(14), "14");
            Assert.True(validation.IsValid("14"), "14");
            Assert.False(validation.IsValid(""), "empty");
            Assert.False(validation.IsValid(null), "null");
        }

        [Fact]
        public void SimpleIsValidNullabeWithNullableTypeTest()
        {
            Validator<int?> validationNullableWithSetup = new Validator<int?>().SetUpNullable(true);
            Assert.True(validationNullableWithSetup.IsValid(14), "14");
            Assert.True(validationNullableWithSetup.IsValid(null), "null");
            Validator<int?> validationWithSetup = new Validator<int?>().SetUpNullable(false);
            Assert.True(validationWithSetup.IsValid(14), "14");
            Assert.False(validationWithSetup.IsValid(null), "null");

            var validationNullable = new Validator<int?>();
            Assert.True(validationNullable.IsValid(14), "14");
            Assert.True(validationNullable.IsValid(null), "null");
            var validation = new Validator<int>();
            Assert.True(validation.IsValid(14), "14");
            Assert.False(validation.IsValid(null), "null");
        }

        [Fact]
        public void SimpleIsValidTypeTest()
        {
            bool isValid;

            isValid = new Validator<int>().IsValid("14");
            Assert.True(isValid, "string 14");
            isValid = new Validator<int>().IsValid('1');
            Assert.True(isValid, "char 1");
            Assert.True(new Validator<int>().IsValid(16), "Int32");
            Assert.False(new Validator<int>().IsValid("a"), "string a");
            Assert.False(new Validator<int>().IsValid('a'), "char a");
            Assert.False(new Validator<int>().IsValid("16.5"), "string 16.5");
        }
        
        [Fact]
        public void SimpleTryConvertTest()
        {
            bool isValid;
            int value;

            isValid = new Validator<int>().TryConvert("14", out value);
            Assert.True(isValid, "string 14");
        }
        
        [Fact]
        public void SimpleConvertTest()
        {
            int value = new Validator<int>().Convert("14");
            Assert.Equal(14, value);
        }

        [Fact(Skip="Need to decide if this really is a bad caracteristic. First-shot? This is really bad")]
        public void DoubleToIntTest()
        {
            Assert.False(new Validator<int>().IsValid(16.5), "double 16.5");
        }
    }
}
