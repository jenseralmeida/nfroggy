using Froggy.Validation;
using NUnit.Framework;

namespace Froggy.Test.Validation
{
    [TestFixture]
    public class LengthValidationTest
    {
        [Test]
        public void SimpleLengthEqualTest()
        {
            var validationInt32 = new Validator<int>()
                .SetUpLength(2);

            Assert.IsFalse(validationInt32.IsValid(null));
            Assert.IsFalse(validationInt32.IsValid(0));
            Assert.IsFalse(validationInt32.IsValid(2));
            Assert.IsFalse(validationInt32.IsValid(5));
            Assert.IsFalse(validationInt32.IsValid(7));
            Assert.IsTrue(validationInt32.IsValid(-2));
            Assert.IsTrue(validationInt32.IsValid("00"));
            Assert.IsTrue(validationInt32.IsValid("-2"));
            Assert.IsTrue(validationInt32.IsValid("02"));
            Assert.IsTrue(validationInt32.IsValid("05"));
            Assert.IsTrue(validationInt32.IsValid("07"));
        }

        [Test]
        public void SimpleLengthIntervalTest()
        {
            var validationString = new Validator<string>()
                .SetUpLength(3, 5);

            Assert.IsTrue(validationString.IsValid(null));
            Assert.IsFalse(validationString.IsValid(""));
            Assert.IsFalse(validationString.IsValid("a"));
            Assert.IsFalse(validationString.IsValid("ab"));
            Assert.IsTrue(validationString.IsValid("abc"));
            Assert.IsTrue(validationString.IsValid("abcd"));
            Assert.IsTrue(validationString.IsValid("abcde"));
            Assert.IsFalse(validationString.IsValid("abcdef"));
            Assert.IsFalse(validationString.IsValid("00000000000000000000444"));
        }

        [Test]
        public void SimpleLengthIntervalWithZeroTest()
        {
            var validationString = new Validator<string>()
                .SetUpLength(0, 5);

            Assert.IsTrue(validationString.IsValid(null));
            Assert.IsTrue(validationString.IsValid(""));
            Assert.IsTrue(validationString.IsValid("a"));
            Assert.IsTrue(validationString.IsValid("ab"));
            Assert.IsTrue(validationString.IsValid("abc"));
            Assert.IsTrue(validationString.IsValid("abcd"));
            Assert.IsTrue(validationString.IsValid("abcde"));
            Assert.IsFalse(validationString.IsValid("abcdef"));
            Assert.IsFalse(validationString.IsValid("00000000000000000000444"));
        }
    }
}
