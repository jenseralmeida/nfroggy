using System.Data;
using Froggy.Data;
using NUnit.Framework;

namespace Froggy.Test.Data
{
    [TestFixture]
    public class DbCommandUtilTest
    {
        [SetUp]
        public void SetUp()
        {
            var comm = new DbCommandUtil("IF NOT EXISTS(SELECT * FROM SYS.OBJECTS WHERE Name = 'Test') CREATE TABLE TEST (ID INT, VALUE NUMERIC(4,2))");
            Assert.AreEqual(ConnectionState.Closed, comm.DaScopeContext.Connection.State);
            comm.ExecuteNonQuery();
            Assert.AreEqual(ConnectionState.Closed, comm.DaScopeContext.Connection.State);
        }

        [TearDown]
        public void TearDown()
        {
            // Drop
            var comm = new DbCommandUtil("IF EXISTS(SELECT * FROM SYS.OBJECTS WHERE Name = 'Test') DROP TABLE TEST");
            Assert.AreEqual(ConnectionState.Closed, comm.DaScopeContext.Connection.State);
            comm.ExecuteNonQuery();
            Assert.AreEqual(ConnectionState.Closed, comm.DaScopeContext.Connection.State);
        }

        [Test]
        public void BasicDbCommandUtilTest()
        {
            // Select
            var comm = new DbCommandUtil("SELECT * FROM TEST");
            Assert.AreEqual(ConnectionState.Closed, comm.DaScopeContext.Connection.State);
            var dataTable = comm.GetDataTable();
            Assert.AreEqual(ConnectionState.Closed, comm.DaScopeContext.Connection.State);
            Assert.AreEqual(2, dataTable.Columns.Count);
            Assert.AreEqual("ID", dataTable.Columns[0].ColumnName);
        }

        [Test]
        public void ParameterPrecisionTest()
        {
            Insert();
            var comm = new DbCommandUtil("SELECT ID FROM TEST WHERE Value = @Value");
            Assert.AreEqual(ConnectionState.Closed, comm.DaScopeContext.Connection.State);
            comm.AddParameter("@Value", DbType.Double, 4, 2, 35.4);
            var idValue = comm.ExecuteScalar<int?>();
            Assert.AreEqual(3, idValue);
        }

        [Test]
        public void DbCommandUtilExecuteScalarTest()
        {
            // Null
            var comm = new DbCommandUtil("SELECT ID FROM TEST");
            Assert.AreEqual(ConnectionState.Closed, comm.DaScopeContext.Connection.State);
            var testNullValue = comm.ExecuteScalar<int?>();
            Assert.AreEqual(ConnectionState.Closed, comm.DaScopeContext.Connection.State);
            Assert.IsNull(testNullValue);
            // Insert
            Insert();
            // NotNull
            var testNotNullValue = comm.ExecuteScalar<int?>();
            Assert.AreEqual(ConnectionState.Closed, comm.DaScopeContext.Connection.State);
            Assert.IsNotNull(testNotNullValue);
            Assert.AreEqual(2, testNotNullValue);
        }

        [Test]
        public void DataTableTest()
        {
            // DataTable
            Insert();
            var comm = new DbCommandUtil("SELECT * FROM TEST");
            Assert.AreEqual(ConnectionState.Closed, comm.DaScopeContext.Connection.State);
            var dataTable = comm.GetDataTable();
            Assert.AreEqual(ConnectionState.Closed, comm.DaScopeContext.Connection.State);
            Assert.AreEqual(2, dataTable.Columns.Count);
            Assert.AreEqual(2, dataTable.Rows.Count);
            Assert.AreEqual(2, dataTable.Rows[0][0]);
            Assert.AreEqual(1.2, dataTable.Rows[0][1]);
            Assert.AreEqual(3, dataTable.Rows[1][0]);
            Assert.AreEqual(35.4, dataTable.Rows[1][1]);
            Assert.AreEqual("ID", dataTable.Columns[0].ColumnName);
        }

        [Test]
        public void NullParameterTest() 
        {
            Insert();
            var comm = new DbCommandUtil("SELECT * FROM TEST WHERE ID = @ID");
            Assert.AreEqual(ConnectionState.Closed, comm.DaScopeContext.Connection.State);
            comm.AddParameter("@ID", DbType.Int32, null);
            Assert.AreEqual(0, comm.GetDataTable().Rows.Count);
            comm.SetParameterValue("@ID", 0);
            Assert.AreEqual(0, comm.GetDataTable().Rows.Count);
        }

        private static void Insert()
        {
            // INSERT 1
            var insert1Comm = new DbCommandUtil("INSERT INTO TEST(ID,VALUE) VALUES(2, 1.2)");
            Assert.AreEqual(ConnectionState.Closed, insert1Comm.DaScopeContext.Connection.State);
            insert1Comm.ExecuteNonQuery();
            Assert.AreEqual(ConnectionState.Closed, insert1Comm.DaScopeContext.Connection.State);
            // INSERT 2
            var insert2Comm = new DbCommandUtil("INSERT INTO TEST(ID,VALUE) VALUES(3, 35.4)");
            Assert.AreEqual(ConnectionState.Closed, insert2Comm.DaScopeContext.Connection.State);
            insert2Comm.ExecuteNonQuery();
            Assert.AreEqual(ConnectionState.Closed, insert2Comm.DaScopeContext.Connection.State);
        }
    }
}
