using System;
namespace Froggy.Validation
{
    interface IValidation
    {
        bool IsValid(object value);
        bool IsValid(object value, out string errorMessage);
        void Validate(object value);
    }
}
