using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Froggy.Validation;
using Db4objects.Db4o.Events;

namespace Froggy.Data.Db4o.Test
{
    [TestFixture]
    public class FooTest
    {
        [Test]
        public void SimpleCreate()
        {
            // Create a valid obj
            Foo fooOk = new Foo() { Name = "guga", Age = 32 };
            DbUtil.Db.Store(fooOk);
            DbUtil.Db.Commit();
            // Create a invalid obj
            try
            {
                Foo fooError1 = new Foo() { Name = "", Age = 32 };
                DbUtil.Db.Store(fooError1);
                DbUtil.Db.Commit();
            }
            catch (EventException ex)
            {
                Assert.IsInstanceOfType(typeof(ValidateException), ex.InnerException);
            }
        }
    }
}
