using Froggy.Validation;
using NUnit.Framework;

namespace Froggy.Test.Validation
{
    [TestFixture]
    public class SystemTypeValidatorTest
    {
        [Test]
        public void SimpleIsValidNullabeWithStringTypeTest()
        {
            Validator<string> validationNullable = new Validator<string>().SetUpNullable(true);
            Assert.IsTrue(validationNullable.IsValid(14), "14");
            Assert.IsTrue(validationNullable.IsValid("14"), "14");
            Assert.IsTrue(validationNullable.IsValid(""), "empty");
            Assert.IsTrue(validationNullable.IsValid(null), "null");
            Validator<string> validation = new Validator<string>().SetUpNullable(false);
            Assert.IsTrue(validation.IsValid(14), "14");
            Assert.IsTrue(validation.IsValid("14"), "14");
            Assert.IsFalse(validation.IsValid(""), "empty");
            Assert.IsFalse(validation.IsValid(null), "null");
        }

        [Test]
        public void SimpleIsValidNullabeWithNullableTypeTest()
        {
            Validator<int?> validationNullableWithSetup = new Validator<int?>().SetUpNullable(true);
            Assert.IsTrue(validationNullableWithSetup.IsValid(14), "14");
            Assert.IsTrue(validationNullableWithSetup.IsValid(null), "null");
            Validator<int?> validationWithSetup = new Validator<int?>().SetUpNullable(false);
            Assert.IsTrue(validationWithSetup.IsValid(14), "14");
            Assert.IsFalse(validationWithSetup.IsValid(null), "null");

            var validationNullable = new Validator<int?>();
            Assert.IsTrue(validationNullable.IsValid(14), "14");
            Assert.IsTrue(validationNullable.IsValid(null), "null");
            var validation = new Validator<int>();
            Assert.IsTrue(validation.IsValid(14), "14");
            Assert.IsFalse(validation.IsValid(null), "null");
        }

        [Test]
        public void SimpleIsValidTypeTest()
        {
            bool isValid;

            isValid = new Validator<int>().IsValid("14");
            Assert.IsTrue(isValid, "string 14");
            isValid = new Validator<int>().IsValid('1');
            Assert.IsTrue(isValid, "char 1");
            Assert.IsTrue(new Validator<int>().IsValid(16), "Int32");
            Assert.IsFalse(new Validator<int>().IsValid("a"), "string a");
            Assert.IsFalse(new Validator<int>().IsValid('a'), "char a");
            Assert.IsFalse(new Validator<int>().IsValid("16.5"), "string 16.5");
        }
        
        [Test]
        public void SimpleTryConvertTest()
        {
            bool isValid;
            int value;

            isValid = new Validator<int>().TryConvert("14", out value);
            Assert.IsTrue(isValid, "string 14");
        }
        
        [Test]
        public void SimpleConvertTest()
        {
            int value = new Validator<int>().Convert("14");
            Assert.AreEqual(14, value, "string 14");
        }

        [Test]
        [Ignore("Need to decide if this really is a bad caracteristic. First-shot? This is really bad")]
        public void DoubleToIntTest()
        {
            Assert.IsFalse(new Validator<int>().IsValid(16.5), "double 16.5");
        }
    }
}
