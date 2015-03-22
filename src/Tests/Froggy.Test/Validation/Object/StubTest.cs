using Xunit;
using Froggy.Validation.Object;

namespace Froggy.Test.Validation.Object
{

    public class StubTest
    {
        [Fact]
        public void Validar()
        {
            var ovu = new ObjectValidator(typeof(Stub));
            var stub = new Stub();
            Assert.False(ovu.IsValid(stub));
            stub.Name = "joao";
            Assert.True(ovu.IsValid(stub));
        }

    }
}
