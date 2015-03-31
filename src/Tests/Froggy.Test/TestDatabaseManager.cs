using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;

namespace Froggy.Test
{
    public class TestDatabaseManager : IDisposable
    {
        private const string LocalDbMaster = @"Data Source=(LocalDB)\v11.0;Initial Catalog=master;Integrated Security=True";
        private const string ConnectionStringTemplate = @"Data Source=(LocalDB)\v11.0;Initial Catalog={0};Integrated Security=True;MultipleActiveResultSets=True;AttachDBFilename={1}.mdf";

        private readonly string _databaseName;

        public TestDatabaseManager(string databaseName)
        {
            _databaseName = databaseName;
        }

        public void CreateDatabase()
        {
            var isDetached = DetachDatabase();
            if (!isDetached) return; //reuse database
            var fileName = CleanupDatabase();

            using (var connection = new SqlConnection(LocalDbMaster))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = string.Format(@"CREATE DATABASE {0} ON (NAME = N'{0}', FILENAME = '{1}.mdf');
ALTER DATABASE {0} SET ALLOW_SNAPSHOT_ISOLATION ON;",
                    _databaseName,
                    fileName);
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateConnectionStringSettings(string connectionStringName)
        {
            var connectionString = String.Format(ConnectionStringTemplate, _databaseName, DatabaseFilePath());
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = config.ConnectionStrings.ConnectionStrings[connectionStringName];
            if (settings == null)
            {
                settings = new ConnectionStringSettings(connectionStringName, connectionString, "System.Data.SqlClient");
                config.ConnectionStrings.ConnectionStrings.Add(settings);
            }
            settings.ConnectionString = connectionString;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("connectionStrings");
        }

        private string CleanupDatabase()
        {
            var fileName = DatabaseFilePath();
            try
            {
                if (File.Exists(fileName + ".mdf")) File.Delete(fileName + ".mdf");
                if (File.Exists(fileName + "_log.ldf")) File.Delete(fileName + "_log.ldf");
            }
            catch
            {
                Console.WriteLine("Could not delete the files (open in Visual Studio?)");
            }
            return fileName;
        }
        private bool DetachDatabase()
        {

            using (var connection = new SqlConnection(LocalDbMaster))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = String.Format("exec sp_detach_db '{0}'", _databaseName);
                try
                {
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch
                {
                    Console.WriteLine("Could not detach");
                    return false;
                }
            }
        }
        private string DatabaseFilePath()
        {
            var configurationPath = Path.GetDirectoryName(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            return Path.Combine(configurationPath, _databaseName);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing) { }

                if (DetachDatabase())
                    CleanupDatabase();

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources. 
         ~TestDatabaseManager()
        {
           Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
             GC.SuppressFinalize(this);
        }
        #endregion
    }
}
