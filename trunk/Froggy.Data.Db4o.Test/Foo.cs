using System.Linq;
using Froggy.Validation.Object;
using Froggy.Validation.BaseValidator;

namespace Froggy.Data.Db4o.Test
{
    public class Foo
    {
        [SystemTypeValidator(IsNullable = false)]
        public string Name { get; set; }

        [ComparableValidator(0, 200, IntervalValidatorType.IntervalInclusive)]
        public int Age { get; set; }
    }
}
