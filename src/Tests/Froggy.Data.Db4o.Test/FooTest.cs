using System.Linq;
using Xunit;
using Froggy.Validation;
using Db4objects.Db4o.Events;

namespace Froggy.Data.Db4o.Test
{
    public class FooTest
    {
        [Fact]
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
                Assert.IsType<ValidateException>(ex.InnerException);
            }
        }
    }
}
