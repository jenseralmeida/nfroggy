using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Froggy.Validation.BaseValidator
{
    public class RegexValidator<T> : ITestValidator<T>
    {
        Regex _Regex;
        string _RegexTemplateMessage;

        public string RegexTemplateMessage
        {
            get { return _RegexTemplateMessage; }
            set { _RegexTemplateMessage = value; }
        }

        public Regex Regex
        {
            get { return _Regex; }
            set 
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Regex", "Null is not allowed");
                }
                _Regex = value; 
            }

        }

        public RegexValidator(Regex regex)
        {
            Regex = regex;
        }

        public RegexValidator(Regex regex, string regexTemplateMessage)
        {
            Regex = regex;
            RegexTemplateMessage = regexTemplateMessage;
        }

        #region ITestValidator<T> Members

        public bool Execute(T value, out string errorMessageTemplate)
        {
            if (_Regex.IsMatch(value.ToString()))
            {
                errorMessageTemplate = "";
                return true;
            }
            else
            {
                errorMessageTemplate = String.Concat("The input of {0} does not match with ", RegexTemplateMessage, ".");
                return false;
            }
        }

        #endregion
    }
}
