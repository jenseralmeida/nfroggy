using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Froggy.Validation.Object;
using Froggy.Validation.BaseValidator;

namespace Froggy.Data.Db4o.Test
{
    public class Foo
    {
        string _Name;
        int _Age;

        [SystemTypeValidator(IsNullable=false)]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        [ComparableValidator(0, 200, IntervalValidatorType.IntervalInclusive)]
        public int Age
        {
            get { return _Age; }
            set { _Age = value; }
        }
    }
}
