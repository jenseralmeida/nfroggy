using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Froggy.Validation.Object
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ValidatorAttribute : Attribute
    {
        string _ErrorMessageLabel;
        string _CustomErrorMessage;

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


        public ValidatorAttribute(bool isNullable)
        {
        }
    }
}
