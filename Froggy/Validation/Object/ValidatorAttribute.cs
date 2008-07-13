using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Froggy.Validation.Object
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple=false, Inherited=true)]
    public class ValidatorAttribute : Attribute
    {
        string _ErrorMessageLabel;
        string _CustomErrorMessage;
        bool _IsNullable;
        ITestValidator[] _CustomTestValidators;

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

        public bool IsNullable
        {
            get { return _IsNullable; }
            set { _IsNullable = value; }
        }

        public ITestValidator[] CustomTestValidators
        {
            get { return _CustomTestValidators; }
            set { _CustomTestValidators = value; }
        } 

        public ValidatorAttribute()
        {
        }
    }
}
