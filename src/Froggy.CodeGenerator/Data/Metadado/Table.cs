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

        public Table(string fullName)
        {
            string[] parts = fullName.Split('.');
            string schema = parts[0];
            string name = parts[1];
            Init(schema, name);
        }
        public Table(string schema, string name)
        {
            Init(schema, name);
        }

        private void Init(string schema, string name)
        {
            Schema = schema;
            Name = name;
            BuildTable();
        }

        private void BuildTable()
        {
            string sql = String.Format("SELECT TOP 0 * FROM {0}", FullName);
            using (var comm = new DbCommandUtil(sql))
            {
                _TableSchema = comm.GetDataTable();
                _TableSchema.TableName = Name;
            }
            // Build columns
            foreach (DataColumn column in _TableSchema.Columns)
            {
                var tableColumn = new Column
                                      {
                                          Name = column.ColumnName,
                                          AllowNull = column.AllowDBNull,
                                          ReadOnly = column.ReadOnly,
                                          DataType = column.DataType,
                                          MaxLength = column.MaxLength
                                      };
                Columns.Add(tableColumn.Name, tableColumn);
            }
            BuildIndexes();
        }

        private void BuildIndexes()
        {
            const string sql = @"SELECT DISTINCT
	   index_name		= [index].name,
	   column_name		= [columns].name,
	   index_column_id  = [index_columns].index_column_id
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
                comm.AddParameter("@schema_name", DbType.AnsiString, Schema);
                comm.AddParameter("@object_name", DbType.AnsiString, Name);
                var reader = comm.ExecuteReader(CommandBehavior.CloseConnection);
                var previousIndexName = "";
                var indexColumns = new List<Column>();
                while (reader.Read())
                {
                    var indexName = reader.GetString(0);
                    if (previousIndexName == "")
                    {
                        previousIndexName = indexName;
                    }

                    if (previousIndexName != indexName)
                    {
                        _Indexes.Add(new Index(previousIndexName, indexColumns));
                        indexColumns = new List<Column>();
                    }
                    indexColumns.Add(Columns[reader.GetString(1)]);
                    previousIndexName = indexName;
                }
                _Indexes.Add(new Index(previousIndexName, indexColumns));

                reader.Close();
            }
        }

        private DataTable _TableSchema;
        private string _Schema;
        private string _Name;
        private readonly Dictionary<string, Column> _Columns = new Dictionary<string, Column>();
        private readonly IList<Index> _Indexes = new List<Index>();

        public string Schema
        {
            get { return _Schema; }
            set { _Schema = value; }
        }

        public DataTable DataTableSchema
        {
            get { return _TableSchema; }
            set { _TableSchema = value; }
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

        public Dictionary<string, Column> Columns
        {
            get { return _Columns; }
        }

        public Index[] Indexes
        {
            get { return _Indexes.ToArray(); }
        }
    }
}