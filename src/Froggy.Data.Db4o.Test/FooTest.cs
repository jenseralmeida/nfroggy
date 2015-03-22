using System.Linq;
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
            var fooOk = new Foo() { Name = "guga", Age = 32 };
            Db.Get.Store(fooOk);
            Db.Get.Commit();
            // Create a invalid obj
            try
            {
                var fooError1 = new Foo() { Name = "", Age = 32 };
                Db.Get.Store(fooError1);
                Db.Get.Commit();
            }
            catch (EventException ex)
            {
                Assert.IsInstanceOfType(typeof(ValidateException), ex.InnerException);
            }
        }
    }
}
