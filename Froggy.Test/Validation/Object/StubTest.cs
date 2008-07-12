﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Froggy.Validation.Object;

namespace Froggy.Test.Validation.Object
{
    [TestFixture]
    class StubTest
    {
        [Test]
        public void Validar()
        {
            ObjectValidatorUtil ovu = new ObjectValidatorUtil(typeof(Stub));
            Stub stub = new Stub();
            Assert.IsFalse(ovu.IsValid(stub));
        }

    }
}
