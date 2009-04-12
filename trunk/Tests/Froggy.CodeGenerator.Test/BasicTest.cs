using Froggy.CodeGenerator.Data;
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
            //var table = new Table("dbo", "");
            var da = new DataAccess("Produto", "dbo.tb1", "schema1.tb2", "schema1.tb3");
        }
    }
}
