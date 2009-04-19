using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Froggy.Validation.BaseValidator
{
	[Serializable]
    public class RegexValidator : ITestValidator
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
                    throw new ArgumentNullException("value", "Null is not allowed");
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

        #region ITestValidator Members

	    public bool IgnoreNullValue
	    {
	        get { return true;}
	    }

	    public bool Execute<T>(T value, object orgValue, out string errorMessageTemplate)
        {
            if (_Regex.IsMatch(orgValue.ToString()))
            {
                errorMessageTemplate = "";
                return true;
            }
            errorMessageTemplate = String.Concat("The input of {0} does not match with ", RegexTemplateMessage, ".");
            return false;
        }

        #endregion
    }
}
