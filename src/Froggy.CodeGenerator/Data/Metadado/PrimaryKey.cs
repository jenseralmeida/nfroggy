using System.Collections.Generic;

namespace Froggy.CodeGenerator.Data.Metadado
{
    public class PrimaryKey: Constraint
    {
        public PrimaryKey(string name, IEnumerable<Column> columns) : base(name, columns)
        {
        }
    }
}