using Froggy.CodeGenerator.Data.Metadado;
using NUnit.Framework;

namespace Froggy.CodeGenerator.Test
{
    [TestFixture]
    public class BasicTest
    {
        [Test]
        [Ignore("Need a test database")]
        public void SimpleTest()
        {
            var table = new Table("dbo", "");
        }
    }
}
