using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Froggy.Validation.Object;
using Froggy.Validation;

namespace Froggy.Test.Validation.Object
{
    class StubCustom
    {
        string _Name;

        [StubNameTestValidator]
        [SystemTypeValidator(IsNullable=false)]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
    }

    internal class StubNameTestValidatorAttribute : TestValidatorAttribute
    {
        public override ITestValidator Create()
        {
            return new StubNameTestValidator();
        }
    }

    internal class StubNameTestValidator : ITestValidator
    {
        #region ITestValidator Members

        public bool Execute<T>(T value, out string errorMessageTemplate)
        {
            switch (value.ToString())
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
