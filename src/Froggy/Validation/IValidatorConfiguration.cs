using System;
namespace Froggy.Validation
{
    interface IValidatorConfiguration
    {
        void AddTestValidator(ITestValidator testValidator);
        string CustomErrorMessage { get; set; }
        string ErrorMessageLabel { get; set; }
        bool IsNullable { get; set; }
    }
}
