using System.Data;
using Froggy.Data;
using Xunit;
using System;

namespace Froggy.Test.Data
{

    public class DbCommandUtilTest : IDisposable
    {
        public DbCommandUtilTest()
        {
            var comm = new DbCommandUtil("IF NOT EXISTS(SELECT * FROM SYS.OBJECTS WHERE Name = 'Test') CREATE TABLE TEST (ID INT, VALUE NUMERIC(4,2))");
            Assert.Equal(ConnectionState.Closed, comm.DaScopeContext.Connection.State);
            comm.ExecuteNonQuery();
            Assert.Equal(ConnectionState.Closed, comm.DaScopeContext.Connection.State);
        }

        [Fact]
        public void BasicDbCommandUtilTest()
        {
            // Select
            var comm = new DbCommandUtil("SELECT * FROM TEST");
            Assert.Equal(ConnectionState.Closed, comm.DaScopeContext.Connection.State);
            var dataTable = comm.GetDataTable();
            Assert.Equal(ConnectionState.Closed, comm.DaScopeContext.Connection.State);
            Assert.Equal(2, dataTable.Columns.Count);
            Assert.Equal("ID", dataTable.Columns[0].ColumnName);
        }

        [Fact]
        public void ParameterPrecisionTest()
        {
            Insert();
            var comm = new DbCommandUtil("SELECT ID FROM TEST WHERE Value = @Value");
            Assert.Equal(ConnectionState.Closed, comm.DaScopeContext.Connection.State);
            comm.AddParameter("@Value", DbType.Double, 4, 2, 35.4);
            var idValue = comm.ExecuteScalar<int?>();
            Assert.Equal(3, idValue);
        }

        [Fact]
        public void DbCommandUtilExecuteScalarTest()
        {
            // Null
            var comm = new DbCommandUtil("SELECT ID FROM TEST");
            Assert.Equal(ConnectionState.Closed, comm.DaScopeContext.Connection.State);
            var testNullValue = comm.ExecuteScalar<int?>();
            Assert.Equal(ConnectionState.Closed, comm.DaScopeContext.Connection.State);
            Assert.Null(testNullValue);
            // Insert
            Insert();
            // NotNull
            var testNotNullValue = comm.ExecuteScalar<int?>();
            Assert.Equal(ConnectionState.Closed, comm.DaScopeContext.Connection.State);
            Assert.NotNull(testNotNullValue);
            Assert.Equal(2, testNotNullValue);
        }

        [Fact]
        public void DataTableTest()
        {
            // DataTable
            Insert();
            var comm = new DbCommandUtil("SELECT * FROM TEST");
            Assert.Equal(ConnectionState.Closed, comm.DaScopeContext.Connection.State);
            var dataTable = comm.GetDataTable();
            Assert.Equal(ConnectionState.Closed, comm.DaScopeContext.Connection.State);
            Assert.Equal(2, dataTable.Columns.Count);
            Assert.Equal(2, dataTable.Rows.Count);
            Assert.Equal(2, dataTable.Rows[0][0]);
            Assert.Equal(1.2, dataTable.Rows[0][1]);
            Assert.Equal(3, dataTable.Rows[1][0]);
            Assert.Equal(35.4, dataTable.Rows[1][1]);
            Assert.Equal("ID", dataTable.Columns[0].ColumnName);
        }

        [Fact]
        public void NullParameterTest() 
        {
            Insert();
            var comm = new DbCommandUtil("SELECT * FROM TEST WHERE ID = @ID");
            Assert.Equal(ConnectionState.Closed, comm.DaScopeContext.Connection.State);
            comm.AddParameter("@ID", DbType.Int32, null);
            Assert.Equal(0, comm.GetDataTable().Rows.Count);
            comm.SetParameterValue("@ID", 0);
            Assert.Equal(0, comm.GetDataTable().Rows.Count);
        }

        private static void Insert()
        {
            // INSERT 1
            var insert1Comm = new DbCommandUtil("INSERT INTO TEST(ID,VALUE) VALUES(2, 1.2)");
            Assert.Equal(ConnectionState.Closed, insert1Comm.DaScopeContext.Connection.State);
            insert1Comm.ExecuteNonQuery();
            Assert.Equal(ConnectionState.Closed, insert1Comm.DaScopeContext.Connection.State);
            // INSERT 2
            var insert2Comm = new DbCommandUtil("INSERT INTO TEST(ID,VALUE) VALUES(3, 35.4)");
            Assert.Equal(ConnectionState.Closed, insert2Comm.DaScopeContext.Connection.State);
            insert2Comm.ExecuteNonQuery();
            Assert.Equal(ConnectionState.Closed, insert2Comm.DaScopeContext.Connection.State);
        }

        [Fact]
        private static void UpdateDataAdapter()
        {
            Insert();

            #region Config DataAdapater
            var comm = new DbCommandUtil("SELECT * FROM TEST");
            const string sqlIns = "INSERT INTO TEST(ID,VALUE) VALUES(@ID, @VALUES)";
            comm.ChangeToDataAdaterCommand(DataAdapterCommand.InsertCommand, sqlIns);
            comm.CreateParameter("@ID", DbType.Int32);
            comm.CreateParameter("@VALUE", DbType.Decimal, 4, 2);
            const string sqlUpd = "UPDATE TEST SET VALUE = @VALUE) WHERE ID=@ID";
            comm.ChangeToDataAdaterCommand(DataAdapterCommand.InsertCommand, sqlUpd);
            comm.CreateParameter("@ID", DbType.Int32);
            comm.CreateParameter("@VALUE", DbType.Decimal, 4, 2);
            const string sqlDel = "DELETE TEST WHERE ID=original_ID";
            comm.ChangeToDataAdaterCommand(DataAdapterCommand.DeleteCommand, sqlDel);
            comm.CreateParameter("@ID", DbType.Int32);
            #endregion Config DataAdapater

            var dt = comm.GetDataTable();
            dt.Rows.Add(new[] { 3, 100.1 });
            dt.Rows[0].Delete();
            dt.Rows[1]["VALOR"] = 100.2;
            comm.Update(dt);
            dt = comm.GetDataTable();
            Assert.Equal(dt.Rows.Count, 2);
            Assert.Equal(dt.Rows[0][0], 2);
            Assert.Equal(dt.Rows[0][1], 100.2);
            Assert.Equal(dt.Rows[1][0], 3);
            Assert.Equal(dt.Rows[1][1], 100.1);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

                    // Drop
                    var comm = new DbCommandUtil("IF EXISTS(SELECT * FROM SYS.OBJECTS WHERE Name = 'Test') DROP TABLE TEST");
                    Assert.Equal(ConnectionState.Closed, comm.DaScopeContext.Connection.State);
                    comm.ExecuteNonQuery();
                    Assert.Equal(ConnectionState.Closed, comm.DaScopeContext.Connection.State);
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources. 
        // ~DbCommandUtilTest() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
