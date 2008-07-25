using NUnit.Framework;
using Froggy.Validation.Object;

namespace Froggy.Test.Validation.Object
{
    [TestFixture]
    public class StubTest
    {
        [Test]
        public void Validar()
        {
            var ovu = new ObjectValidator(typeof(Stub));
            var stub = new Stub();
            Assert.IsFalse(ovu.IsValid(stub));
            stub.Name = "joao";
            Assert.IsTrue(ovu.IsValid(stub));
        }

    }
}
