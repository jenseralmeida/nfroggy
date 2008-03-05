using System;
using System.Collections.Generic;
using System.Text;
using Froggy.Validation;
using NUnit.Framework;

namespace Froggy.Test.Validation
{
    [TestFixture]
    public class SystemTypeValidatorTest
    {
        [Test]
        public void SimpleTypeTest()
        {
            bool result;
            result = Validation<int>.Create().IsValid("14");
            Assert.IsTrue(result);
            result = Validation<int>.Create().IsValid("a");
            Assert.IsFalse(result);
        }
    }
}
