using System;
using System.Collections.Generic;
using System.Text;

namespace Froggy.Validation
{
    /// <summary>
    /// Represents a unit of validation. That unit will possue any validation necessary to represent a valid 
    /// and basic value
    /// </summary>
    public interface IValidator<T>
    {
        IValidator<T> SetErrorMessageLabel(string errorMessageLabel);

        IValidator<T> SetCustomMessage(string customMessage);

        IValidator<T> AddValidatorUnit(IValidatorUnit<T> validatorUnit);

        /// <summary>
        /// Label injected in the template of a error message to represent the data being validated
        /// </summary>
        string ErrorMessageLabel
        {
            get;
            set;
        }

        /// <summary>
        /// When this property is set the custom message configured will be showed for any error 
        /// intercepted
        /// </summary>
        string CustomMessage
        {
            get;
            set;
        }

        /// <summary>
        /// Validate <paramref name="value"/> and returns a <see cref="ValidateException"/> if they dont follow the setup of the validation
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="ValidateException"></exception>
        void Validate(object value);

        bool IsValid(object value);

        bool IsValid(object value, out string errorMessage);

        T Convert(object value);

        T Convert(object value, out string errorMessage);

        bool TryConvert(object value, out T result);

        bool TryConvert(object value, out T result, out string errorMessage);
    }
}
