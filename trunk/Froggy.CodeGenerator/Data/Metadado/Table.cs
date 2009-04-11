using System;
using System.Collections.Generic;
using System.Data;
using Froggy.Data;

namespace Froggy.CodeGenerator.Data.Metadado
{
    public class Table
    {
        public PrimaryKey PrimaryKey
        {
            get { return _PrimaryKey; }
        }

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
            BuildConstraints(schemaTable);
        }

        private void BuildConstraints(DataTable schemaTable)
        {
            bool hasPrimaryKey = schemaTable.PrimaryKey.Length > 0;
            if (hasPrimaryKey)
            {
                var primaryKeyColumns = new List<Column>(schemaTable.PrimaryKey.Length);
                foreach (var column in schemaTable.PrimaryKey)
                {
                    primaryKeyColumns.Add(_Columns[column.ColumnName]);
                }
                var primaryKey = new PrimaryKey("", primaryKeyColumns);
                AddConstraint(primaryKey);
            }

        }

        private string _Schema;
        private string _Name;
        private readonly Dictionary<string, Column> _Columns = new Dictionary<string, Column>();
        private readonly IList<Constraint> _Constraints = new List<Constraint>();
        private PrimaryKey _PrimaryKey;

        private void AddConstraint(Constraint constraint)
        {
            if (constraint is PrimaryKey)
            {
                _PrimaryKey = (PrimaryKey) constraint;
            }
            _Constraints.Add(constraint);
        }

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
    }
}