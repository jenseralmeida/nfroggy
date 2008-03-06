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
        public void SimpleIsValidTypeTest()
        {
            Assert.IsTrue(Validation<int>.Create().IsValid("14"), "string 14");
            Assert.IsTrue(Validation<int>.Create().IsValid('1'), "char 1");
            Assert.IsTrue(Validation<int>.Create().IsValid(16), "Int32");
            Assert.IsFalse(Validation<int>.Create().IsValid("a"), "string a");
            Assert.IsFalse(Validation<int>.Create().IsValid('a'), "char a");
            Assert.IsFalse(Validation<int>.Create().IsValid(16.5), "double 16.5");
        }
    }
}