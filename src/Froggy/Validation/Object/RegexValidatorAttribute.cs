using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Froggy.Validation.BaseValidator;
using System.Text.RegularExpressions;

namespace Froggy.Validation.Object
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class RegexValidatorAttribute : TestValidatorAttribute
    {
        RegexValidator _RegexValidator;

        public RegexValidatorAttribute(string pattern)
        {
            _RegexValidator = new RegexValidator(new Regex(pattern));
        }

        public RegexValidatorAttribute(string pattern, string regexTemplateMessage)
        {
            Regex regex = new Regex(pattern);
            _RegexValidator = new RegexValidator(regex, regexTemplateMessage);
        }

        public override ITestValidator Create()
        {
            return _RegexValidator;
        }
    }
}
