using System.Collections.Generic;

namespace Froggy.CodeGenerator.Data.Metadado
{
    public class Unique: Constraint
    {
        public Unique(string name, IEnumerable<Column> columns) : base(name, columns)
        {
        }
    }
}