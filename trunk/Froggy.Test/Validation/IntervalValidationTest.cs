using System;
using System.Collections.Generic;
using System.Text;
using Froggy.Validation;
using NUnit.Framework;

namespace Froggy.Test.Validation
{
    [TestFixture]
    public class ComparableValidationTest
    {
        [Test]
        public void SimpleDefaultInterval()
        {
            Validator<int> validation = Validator<int>.Create()
                .SetUpComparable(0, 10);

            Assert.IsTrue(validation.IsValid(0));
            Assert.IsTrue(validation.IsValid(2));
            Assert.IsTrue(validation.IsValid(5));
            Assert.IsTrue(validation.IsValid(7));
            Assert.IsTrue(validation.IsValid(10));
            Assert.IsFalse(validation.IsValid(11));
            Assert.IsFalse(validation.IsValid(-1));
            Assert.IsFalse(validation.IsValid(Int32.MinValue));
            Assert.IsFalse(validation.IsValid(Int32.MaxValue));
        }

        [Test]
        public void SimpleDefaultEqual()
        {
            Validator<int> validation = Validator<int>.Create()
                .SetUpComparable(5);

            Assert.IsTrue(validation.IsValid(5));
            Assert.IsFalse(validation.IsValid(0));
            Assert.IsFalse(validation.IsValid(4));
            Assert.IsFalse(validation.IsValid(6));
            Assert.IsFalse( validation.IsValid(Int32.MinValue));
            Assert.IsFalse( validation.IsValid(Int32.MaxValue));
        }
    }
}
