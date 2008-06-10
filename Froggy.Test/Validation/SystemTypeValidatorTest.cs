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
            bool isValid;

            isValid = Validator<int>.Create().IsValid("14");
            Assert.IsTrue(isValid, "string 14");
            isValid = Validator<int>.Create().IsValid('1');
            Assert.IsTrue(isValid, "char 1");
            Assert.IsTrue(Validator<int>.Create().IsValid(16), "Int32");
            Assert.IsFalse(Validator<int>.Create().IsValid("a"), "string a");
            Assert.IsFalse(Validator<int>.Create().IsValid('a'), "char a");
            Assert.IsFalse(Validator<int>.Create().IsValid("16.5"), "string 16.5");
        }
        
        [Test]
        public void SimpleTryConvertTest()
        {
            bool isValid;
            int value;

            isValid = Validator<int>.Create().TryConvert("14", out value);
            Assert.IsTrue(isValid, "string 14");
        }
        
        [Test]
        public void SimpleConvertTest()
        {
            int value = Validator<int>.Create().Convert("14");
            Assert.AreEqual(14, value, "string 14");
        }

        [Test]
        [Ignore("Need to decide if this really is a bad caracteristic. First-shot? This is really bad")]
        public void DoubleToIntTest()
        {
            Assert.IsFalse(Validator<int>.Create().IsValid(16.5), "double 16.5");
        }
    }
}