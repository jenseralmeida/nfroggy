using System;
using Froggy.Validation;
using Xunit;

namespace Froggy.Test.Validation
{

    public class ComparableValidationTest
    {
        [Fact]
        public void SimpleDefaultInterval()
        {
            var validation = new Validator<int>()
                .SetUpComparable(0, 10);

            Assert.False(validation.IsValid(null));
            Assert.True(validation.IsValid(0));
            Assert.True(validation.IsValid(2));
            Assert.True(validation.IsValid(5));
            Assert.True(validation.IsValid(7));
            Assert.True(validation.IsValid("00"));
            Assert.True(validation.IsValid("02"));
            Assert.True(validation.IsValid("05"));
            Assert.True(validation.IsValid("07"));
            Assert.True(validation.IsValid(10));
            Assert.False(validation.IsValid("11"));
            Assert.False(validation.IsValid(11));
            Assert.False(validation.IsValid(-1));
            Assert.False(validation.IsValid(Int32.MinValue));
            Assert.False(validation.IsValid(Int32.MaxValue));
        }

        [Fact]
        public void SimpleDefaultEqual()
        {
            Validator<int> validation = new Validator<int>()
                .SetUpComparable(5);

            Assert.False(validation.IsValid(null));
            Assert.True(validation.IsValid(5));
            Assert.False(validation.IsValid(0));
            Assert.False(validation.IsValid(4));
            Assert.False(validation.IsValid(6));
            Assert.False( validation.IsValid(Int32.MinValue));
            Assert.False( validation.IsValid(Int32.MaxValue));
        }
        [Fact]
        public void SimpleNullableInterval()
        {
            var validation = new Validator<int?>()
                .SetUpComparable(0, 10);

            Assert.True(validation.IsValid(null));
            Assert.True(validation.IsValid(0));
            Assert.True(validation.IsValid(2));
            Assert.True(validation.IsValid(5));
            Assert.True(validation.IsValid(7));
            Assert.True(validation.IsValid("00"));
            Assert.True(validation.IsValid("02"));
            Assert.True(validation.IsValid("05"));
            Assert.True(validation.IsValid("07"));
            Assert.True(validation.IsValid(10));
            Assert.False(validation.IsValid("11"));
            Assert.False(validation.IsValid(11));
            Assert.False(validation.IsValid(-1));
            Assert.False(validation.IsValid(Int32.MinValue));
            Assert.False(validation.IsValid(Int32.MaxValue));
        }

        [Fact]
        public void SimpleNullabeEqual()
        {
            Validator<int?> validation = new Validator<int?>()
                .SetUpComparable(5);

            Assert.True(validation.IsValid(null));
            Assert.True(validation.IsValid(5));
            Assert.False(validation.IsValid(0));
            Assert.False(validation.IsValid(4));
            Assert.False(validation.IsValid(6));
            Assert.False(validation.IsValid(Int32.MinValue));
            Assert.False(validation.IsValid(Int32.MaxValue));
        }
    }
}
