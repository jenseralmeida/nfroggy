using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Froggy.Validation.Object
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class SystemTypeValidatorAttribute : TypeValidatorAttribute
    {
        private bool _IsNullable;

        public override bool IsNullable
        {
            get { return _IsNullable; }
            set { _IsNullable = value; }
        }

        public SystemTypeValidatorAttribute()
            : this(true)
        {
        }

        public SystemTypeValidatorAttribute(bool isNullable)
        {
            IsNullable = isNullable;
        }
    }
}
