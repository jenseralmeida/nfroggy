using System.Collections.Generic;

namespace Froggy.CodeGenerator.Data.Metadado
{
    public class ForeignKey: Constraint
    {
        private readonly Table _ReferenceTable;
        private readonly List<Column> _ReferenceColumns;

        public ForeignKey(string name, IEnumerable<Column> columns, Table referenceTable, IEnumerable<Column> referenceColumns)
            : base(name, columns)
        {
            _ReferenceTable = referenceTable;
            _ReferenceColumns = new List<Column>();
            _ReferenceColumns.AddRange(referenceColumns);
        }

        public Table ReferenceTable
        {
            get { return _ReferenceTable; }
        }

        public Column[] ReferenceColumns
        {
            get { return _ReferenceColumns.ToArray(); }
        }

    }
}