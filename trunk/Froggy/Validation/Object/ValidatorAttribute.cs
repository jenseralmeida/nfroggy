using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Froggy.Validation.Object
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ValidatorAttribute : Attribute
    {
        Validator<object> validator;

        public Validator<object> Validator
        {
            get { return validator; }
            set { validator = value; }
        }

        public ValidatorAttribute(bool isNullable)
        {
            validator = new Validator<object>();
            validator.SetUpNullable(isNullable);
        }
    }
}
