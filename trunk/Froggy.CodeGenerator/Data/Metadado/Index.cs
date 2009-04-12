using System.Collections.Generic;
using System.Linq;

namespace Froggy.CodeGenerator.Data.Metadado
{
    public class Index
    {
        public Index(string name, IEnumerable<Column> columns)
        {
            _Name = name;
            _Columns = columns.ToArray();
        }
        private readonly string _Name;
        private readonly Column[] _Columns;

        public string Name
        {
            get { return _Name; }
        }

        public Column[] Columns
        {
            get { return _Columns; }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}