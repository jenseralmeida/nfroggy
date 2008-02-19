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
        /// List of validator to be applied int unit of validation
        /// </summary>
        IValidator[] Validators
        {
            get;
        }
    }
}
