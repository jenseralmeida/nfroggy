using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Froggy.Data;

namespace Froggy.CodeGenerator.Data.Metadado
{
    public class Table
    {
        public static Table[] GetAll()
        {
            var tables = new List<Table>();
            const string sql =
                @"SELECT Schema_Name(Schema_Id) AS Schema_Name, Name, Type
  FROM sys.objects
 WHERE type = 'U'";
            var comm = new DbCommandUtil(sql);
            using (var reader = comm.ExecuteReader())
            {
                while (reader.Read())
                {
                    string schema = reader.GetString(0);
                    string name = reader.GetString(1);
                    tables.Add(new Table(schema, name));
                }
            }
            return tables.ToArray();
        }

        public Table(string schema, string name)
        {
            Schema = schema;
            Name = name;
            BuildTable();
        }

        private void BuildTable()
        {
            string sql = String.Format("SELECT * FROM {0}", FullName);
            DataTable schemaTable;
            using (var comm = new DbCommandUtil(sql))
            {
                // DbCommandUtil already dispose all objects created by then
                var conn = comm.DAScopeContext.Connection;
                conn.Open();
                schemaTable = comm.DAScopeContext.Connection.GetSchema();
                conn.Close();
            }
            // Build columns
            foreach (DataColumn column in schemaTable.Columns)
            {
                var tableColumn = new Column
                                      {
                                          Name = column.ColumnName,
                                          AllowNull = column.AllowDBNull,
                                          ReadOnly = column.ReadOnly,
                                          DataType = column.DataType,
                                          MaxLength = column.MaxLength
                                      };
                _Columns.Add(tableColumn.Name, tableColumn);
            }
            BuildIndexes();
        }

        private void BuildIndexes()
        {
            const string sql = @"SELECT DISTINCT
	   index_name		= [index].name,
	   column_name		= [columns].name
from sys.indexes [index]
          LEFT JOIN sys.objects [objects] ON ( [objects].object_id = [index].object_id )
          LEFT JOIN sys.index_columns [index_columns] ON ( [index_columns].index_id = [index].index_id )
		    											 AND ( [index_columns].object_id  = [index].object_id )
          LEFT JOIN sys.columns [columns] ON ( [index_columns].OBJECT_id = [columns].object_id )
                                             AND ( [index_columns].Column_id = [columns].column_id )
WHERE ( [objects].schema_id = SCHEMA_ID(@schema_name) )
	  AND ( [objects].name = @object_name )
	  AND ( [index].type = 2 )
ORDER BY [index].name, [index_columns].index_column_id";
            using (var comm = new DbCommandUtil(sql))
            {
                comm.AddParameter("@schema_name", DbType.String, Schema);
                comm.AddParameter("@object_name", DbType.String, Name);
                var reader = comm.ExecuteReader(CommandBehavior.CloseConnection);
                string previousIndexName = "";
                var indexColumns = new List<Column>();
                while (reader.Read())
                {
                    string indexName = reader.GetString(0);
                    if (previousIndexName != indexName)
                    {
                        previousIndexName = indexName;
                        indexColumns = new List<Column>();
                    }
                    indexColumns.Add(_Columns[reader.GetString(1)]);
                    _Indexes.Add(new Index(indexName, indexColumns));
                }
                reader.Close();
            }


        }

        private string _Schema;
        private string _Name;
        private readonly Dictionary<string, Column> _Columns = new Dictionary<string, Column>();
        private readonly IList<Index> _Indexes = new List<Index>();

        public string Schema
        {
            get { return _Schema; }
            set { _Schema = value; }
        }

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public string FullName
        {
            get { return String.Concat(_Schema, ".", _Name); }
        }

        public Index[] Indexes
        {
            get { return _Indexes.ToArray(); }
        }
    }
}