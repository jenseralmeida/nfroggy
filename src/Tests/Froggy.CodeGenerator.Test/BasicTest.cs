﻿using Froggy.CodeGenerator.Data;
using Xunit;

namespace Froggy.CodeGenerator.Test
{
    public class BasicTest
    {
        [Fact(Skip="Need a test database")]
        public void SimpleTest()
        {
            //var table = new Table("dbo", "");
            var da = new DataAccess("Produto", "dbo.tb1", "schema1.tb2", "schema1.tb3");
        }
    }
}
