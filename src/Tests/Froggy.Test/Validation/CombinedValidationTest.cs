using Froggy.Validation;
using Xunit;

namespace Froggy.Test.Validation
{

    public class CombinedValidationTest
    {
        [Fact]
        public void TypeLengthComparableTest()
        {
            string errorMsg;
            var validator = new Validator<int?>("codigo")
                    .SetUpLength(3)
                    .SetUpComparable(1, 600);
            // Type validation
            Assert.True(validator.IsValid(null, out errorMsg), "null");
            Assert.False(validator.IsValid("a", out errorMsg), "Type");
            Assert.False(validator.IsValid(new object(), out errorMsg), "Type");
            // Length validation
            Assert.False(validator.IsValid(3, out errorMsg), "length");
            Assert.False(validator.IsValid(003, out errorMsg), "length");
            Assert.True(validator.IsValid("003", out errorMsg), "length");
            // Comparable validation
            Assert.False(validator.IsValid("-03", out errorMsg), "comparable");
            Assert.False(validator.IsValid("-03", out errorMsg), "comparable");
            Assert.True(validator.IsValid("500", out errorMsg), "comparable");
            Assert.True(validator.IsValid("600", out errorMsg), "comparable");
            Assert.False(validator.IsValid("601", out errorMsg), "comparable");
        }
    }
}
