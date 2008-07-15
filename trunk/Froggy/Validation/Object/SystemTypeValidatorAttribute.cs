using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Froggy.Validation.Object
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class SystemTypeValidatorAttribute : ITypeValidatorAttribute
    {
        private bool _IsNullable;

        public bool IsNullable
        {
            get { return _IsNullable; }
            set { _IsNullable = value; }
        }

        public SystemTypeValidatorAttribute(bool isNullable)
        {
            IsNullable = isNullable;
        }
    }
}
