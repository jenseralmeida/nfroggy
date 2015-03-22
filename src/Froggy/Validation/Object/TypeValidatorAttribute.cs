using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Froggy.Validation.Object
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public abstract class TypeValidatorAttribute : Attribute, IValidatorAttribute
    {
        #region ITypeValidator Members

        public abstract bool IsNullable
        {
            get;
            set;
        }

        #endregion
    }
}
