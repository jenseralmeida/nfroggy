using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Froggy.Data;
using NUnit.Framework;

namespace Froggy.Test.Data
{
    [TestFixture]
    public class DbCommandUtilTest
    {

        [Test]
        public void BasicDbCommandUtilTest()
        {
            var comm = new DbCommandUtil("IF NOT EXISTS(SELECT * FROM SYS.OBJECTS WHERE Name = 'Test') CREATE TABLE TEST (ID INT)");
            comm.ExecuteNonQuery();
            comm = new DbCommandUtil("SELECT * FROM TEST");
            var dataTable = comm.GetDataTable();
            Assert.AreEqual(1, dataTable.Columns.Count);
            Assert.AreEqual("ID", dataTable.Columns[0].ColumnName);
            comm = new DbCommandUtil("IF EXISTS(SELECT * FROM SYS.OBJECTS WHERE Name = 'Test') DROP TABLE TEST");
            comm.ExecuteNonQuery();
        }

    }
}
