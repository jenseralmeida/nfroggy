using Froggy.Validation;
using NUnit.Framework;

namespace Froggy.Test.Validation
{
    [TestFixture]
    public class CombinedValidationTest
    {
        [Test]
        public void TypeLengthComparableTest()
        {
            string errorMsg;
            var validator = new Validator<int?>("codigo")
                    .SetUpLength(3)
                    .SetUpComparable(1, 600);
            // Type validation
            Assert.IsTrue(validator.IsValid(null, out errorMsg), "null");
            Assert.IsFalse(validator.IsValid("a", out errorMsg), "Type");
            Assert.IsFalse(validator.IsValid(new object(), out errorMsg), "Type");
            // Length validation
            Assert.IsFalse(validator.IsValid(3, out errorMsg), "length");
            Assert.IsFalse(validator.IsValid(003, out errorMsg), "length");
            Assert.IsTrue(validator.IsValid("003", out errorMsg), "length");
            // Comparable validation
            Assert.IsFalse(validator.IsValid("-03", out errorMsg), "comparable");
            Assert.IsFalse(validator.IsValid("-03", out errorMsg), "comparable");
            Assert.IsTrue(validator.IsValid("500", out errorMsg), "comparable");
            Assert.IsTrue(validator.IsValid("600", out errorMsg), "comparable");
            Assert.IsFalse(validator.IsValid("601", out errorMsg), "comparable");
        }
    }
}
