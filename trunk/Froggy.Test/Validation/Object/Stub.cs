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
        string name;

        [Validator(false)]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        class StupNameTestValidator : ITestValidator<string>
        {
            #region ITestValidator<string> Members

            public bool Execute(string value, out string errorMessageTemplate)
            {
                switch (value)
                {
                    case "a":
                    case "e":
                    case "i":
                    case "o":
                    case "u":
                        errorMessageTemplate = "";
                        return true;
                    default:
                        errorMessageTemplate = "o valor para {0} não é uma vogal";
                        return false;
                }
            }

            #endregion
        }
    }
}
