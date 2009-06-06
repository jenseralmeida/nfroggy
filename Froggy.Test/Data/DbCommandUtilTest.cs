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
            var comm = new DbCommandUtil("IF NOT EXISTS(SELECT * FROM SYS.OBJECTS WHERE Name = 'Test') CREATE TABLE TEST (ID INT)");
            Assert.AreEqual(ConnectionState.Closed, comm.DAScopeContext.Connection.State);
            comm.ExecuteNonQuery();
            Assert.AreEqual(ConnectionState.Closed, comm.DAScopeContext.Connection.State);
        }

        [TearDown]
        public void TearDown()
        {
            // Drop
            var comm = new DbCommandUtil("IF EXISTS(SELECT * FROM SYS.OBJECTS WHERE Name = 'Test') DROP TABLE TEST");
            Assert.AreEqual(ConnectionState.Closed, comm.DAScopeContext.Connection.State);
            comm.ExecuteNonQuery();
            Assert.AreEqual(ConnectionState.Closed, comm.DAScopeContext.Connection.State);
        }

        [Test]
        public void BasicDbCommandUtilTest()
        {
            // Select
            var comm = new DbCommandUtil("SELECT * FROM TEST");
            Assert.AreEqual(ConnectionState.Closed, comm.DAScopeContext.Connection.State);
            var dataTable = comm.GetDataTable();
            Assert.AreEqual(ConnectionState.Closed, comm.DAScopeContext.Connection.State);
            Assert.AreEqual(1, dataTable.Columns.Count);
            Assert.AreEqual("ID", dataTable.Columns[0].ColumnName);
        }

        [Test]
        public void DbCommandUtilExecuteScalarTest()
        {
            // Null
            var comm = new DbCommandUtil("SELECT ID FROM TEST");
            Assert.AreEqual(ConnectionState.Closed, comm.DAScopeContext.Connection.State);
            var testNullValue = comm.ExecuteScalar<int?>();
            Assert.AreEqual(ConnectionState.Closed, comm.DAScopeContext.Connection.State);
            Assert.IsNull(testNullValue);
            // Insert
            Insert();
            // NotNull
            var testNotNullValue = comm.ExecuteScalar<int?>();
            Assert.AreEqual(ConnectionState.Closed, comm.DAScopeContext.Connection.State);
            Assert.IsNotNull(testNotNullValue);
            Assert.AreEqual(2, testNotNullValue);
        }

        [Test]
        public void DataTableTest()
        {
            // DataTable
            Insert();
            var comm = new DbCommandUtil("SELECT * FROM TEST");
            Assert.AreEqual(ConnectionState.Closed, comm.DAScopeContext.Connection.State);
            var dataTable = comm.GetDataTable();
            Assert.AreEqual(ConnectionState.Closed, comm.DAScopeContext.Connection.State);
            Assert.AreEqual(1, dataTable.Columns.Count);
            Assert.AreEqual(1, dataTable.Rows.Count);
            Assert.AreEqual(2, dataTable.Rows[0][0]);
            Assert.AreEqual("ID", dataTable.Columns[0].ColumnName);
        }

        [Test]
        public void NullParameterTest() 
        {
            Insert();
            var comm = new DbCommandUtil("SELECT * FROM TEST WHERE ID = @ID");
            Assert.AreEqual(ConnectionState.Closed, comm.DAScopeContext.Connection.State);
            comm.AddParameter("@ID", DbType.Int32, null);
            Assert.AreEqual(0, comm.GetDataTable().Rows.Count);
            comm.SetParameterValue("@ID", 0);
            Assert.AreEqual(0, comm.GetDataTable().Rows.Count);
        }

        private static void Insert()
        {
            var comm = new DbCommandUtil("INSERT INTO TEST(ID) VALUES(2)");
            Assert.AreEqual(ConnectionState.Closed, comm.DAScopeContext.Connection.State);
            comm.ExecuteNonQuery();
            Assert.AreEqual(ConnectionState.Closed, comm.DAScopeContext.Connection.State);
        }
    }
}
