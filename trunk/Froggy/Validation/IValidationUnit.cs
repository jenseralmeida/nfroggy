using System;
using System.Collections.Generic;
using System.Text;

namespace Froggy.Validation
{
    /// <summary>
    /// Represents a unit of validation. That unit will possue any validation necessary to represent a valid 
    /// and basic value
    /// </summary>
    public interface IValidationUnit
    {
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

        /// <summary>
        /// List of validator to be applied int unit of validation
        /// </summary>
        List<IValidator> Validators
        {
            get;
        }
    }
}
