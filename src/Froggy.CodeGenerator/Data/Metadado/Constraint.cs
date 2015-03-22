using System.Collections.Generic;

namespace Froggy.CodeGenerator.Data.Metadado
{
    public class Constraint
    {
        public Constraint(string name, IEnumerable<Column> columns)
        {
            Name = name;
            _Columns = new List<Column>();
            _Columns.AddRange(columns);
        }

        private readonly List<Column> _Columns;

        public string Name { get; set; }

        public Column[] Columns
        {
            get { return _Columns.ToArray(); }
        }
    }
}