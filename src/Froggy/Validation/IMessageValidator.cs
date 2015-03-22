using System;
namespace Froggy.Validation
{
    interface IMessageValidator
    {
        string CustomErrorMessage { get; set; }
        string ErrorMessageLabel { get; set; }
    }
}
