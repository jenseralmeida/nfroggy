using Froggy.Validation;
using Xunit;

namespace Froggy.Test.Validation
{

    public class LengthValidationTest
    {
        [Fact]
        public void SimpleLengthEqualTest()
        {
            var validationInt32 = new Validator<int>()
                .SetUpLength(2);

            Assert.False(validationInt32.IsValid(null));
            Assert.False(validationInt32.IsValid(0));
            Assert.False(validationInt32.IsValid(2));
            Assert.False(validationInt32.IsValid(5));
            Assert.False(validationInt32.IsValid(7));
            Assert.True(validationInt32.IsValid(-2));
            Assert.True(validationInt32.IsValid("00"));
            Assert.True(validationInt32.IsValid("-2"));
            Assert.True(validationInt32.IsValid("02"));
            Assert.True(validationInt32.IsValid("05"));
            Assert.True(validationInt32.IsValid("07"));
        }

        [Fact]
        public void SimpleLengthIntervalTest()
        {
            var validationString = new Validator<string>()
                .SetUpLength(3, 5);

            Assert.True(validationString.IsValid(null));
            Assert.False(validationString.IsValid(""));
            Assert.False(validationString.IsValid("a"));
            Assert.False(validationString.IsValid("ab"));
            Assert.True(validationString.IsValid("abc"));
            Assert.True(validationString.IsValid("abcd"));
            Assert.True(validationString.IsValid("abcde"));
            Assert.False(validationString.IsValid("abcdef"));
            Assert.False(validationString.IsValid("00000000000000000000444"));
        }

        [Fact]
        public void SimpleLengthIntervalWithZeroTest()
        {
            var validationString = new Validator<string>()
                .SetUpLength(0, 5);

            Assert.True(validationString.IsValid(null));
            Assert.True(validationString.IsValid(""));
            Assert.True(validationString.IsValid("a"));
            Assert.True(validationString.IsValid("ab"));
            Assert.True(validationString.IsValid("abc"));
            Assert.True(validationString.IsValid("abcd"));
            Assert.True(validationString.IsValid("abcde"));
            Assert.False(validationString.IsValid("abcdef"));
            Assert.False(validationString.IsValid("00000000000000000000444"));
        }
    }
}
