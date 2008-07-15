using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Froggy.Validation.Object;
using Froggy.Validation;

namespace Froggy.Test.Validation.Object
{
    class Stub
    {
        string _Name;
        string _Vogal;

        [SystemTypeValidator(IsNullable=false)]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        //[Validator(CustomTestValidators={new StubNameTestValidator()})]
        public string Vogal
        {
            get { return _Vogal; }
            set { _Vogal = value; }
        }
    }
}
