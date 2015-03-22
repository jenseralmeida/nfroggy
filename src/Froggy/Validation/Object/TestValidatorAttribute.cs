using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Froggy.Validation.Object
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public abstract class TestValidatorAttribute : Attribute, IValidatorAttribute
    {
        public abstract ITestValidator Create();
    }
}
