using Froggy.Validation.Object;
using Froggy.Validation;

namespace Froggy.Test.Validation.Object
{
    class StubCustom
    {
        [StubNameTestValidator]
        [SystemTypeValidator(IsNullable = false)]
        public string Name { get; set; }
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

        public bool IgnoreNullValue
        {
            get { return true;}
        }

        public bool Execute<T>(T value, object orgValue, out string errorMessageTemplate)
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
