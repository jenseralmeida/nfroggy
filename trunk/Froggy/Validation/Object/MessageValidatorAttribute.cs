using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Froggy.Validation.Object
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class MessageValidatorAttribute : Attribute, IMessageValidator, IValidatorAttribute
    {
        private string _CustomErrorMessage;
        private string _ErrorMessageLabel;

        #region IMessageValidator Members

        public string CustomErrorMessage
        {
            get
            {
                return _CustomErrorMessage;
            }
            set
            {
                _CustomErrorMessage = value;
            }
        }

        public string ErrorMessageLabel
        {
            get
            {
                return _ErrorMessageLabel;
            }
            set
            {
                _ErrorMessageLabel = value;
            }
        }

        #endregion
    }
}
